using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace TomWebApi.Hubs
{
    [EnableCors()]
    public class CodeBlockHub : Hub
    {
        private static Dictionary<string, string> mentors = new Dictionary<string, string>();
        private static Dictionary<string, List<string>> students = new Dictionary<string, List<string>>();
        private static Dictionary<string, string> roomCodes = new Dictionary<string, string>();

        public async Task JoinRoom(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);

            if (!mentors.ContainsKey(room))
            {
                mentors[room] = Context.ConnectionId;
                await Clients.Caller.SendAsync("roleAssigned", "mentor");
            }
            else
            {
                if (!students.ContainsKey(room))
                {
                    students[room] = new List<string>();
                }
                students[room].Add(Context.ConnectionId);

                await Clients.Caller.SendAsync("roleAssigned", "student");
                await Clients.Group(room).SendAsync("studentCountUpdate", students[room].Count);
            }
        }

        public async Task CodeChanged(string room, string code)
        {
            roomCodes[room] = code; 
            await Clients.OthersInGroup(room).SendAsync("receiveCodeChange", code); 
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string room = mentors.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

            if (room != null)
            {
                await Clients.Group(room).SendAsync("RedirectToLobby");
                mentors.Remove(room);
                roomCodes[room] = "";

                if (students.ContainsKey(room))
                {
                    students[room].Clear(); 
                    await Clients.Group(room).SendAsync("studentCountUpdate", 0); 
                }
            }
            else
            {
                foreach (var kvp in students)
                {
                    if (kvp.Value.Contains(Context.ConnectionId))
                    {
                        kvp.Value.Remove(Context.ConnectionId); 

                        await Clients.Group(kvp.Key).SendAsync("studentCountUpdate", kvp.Value.Count);
                        break;
                    }
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
