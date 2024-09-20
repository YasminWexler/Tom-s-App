using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TomWebApi.Controllers
{
    [EnableCors()]
    public class CodeBlocksController : Controller
    {
        private readonly tomcodeblocksContext db;

      
        public CodeBlocksController(tomcodeblocksContext dataconnection)
        {
            db = dataconnection;
        }

        [HttpGet]
        [Route("getinitialCode/{name}")]
        public IActionResult GetCodeBlock(string name)
        {
            Console.WriteLine("Fetching Code Block: " + name);
            var codeBlock = db.CodeBlocks.FirstOrDefault(cb => cb.Title == name);
            if (codeBlock != null)
            {
                Console.WriteLine("Code Block Found");
                return Ok(new { initialCode = codeBlock.InitialCode });
            }
            else
            {
                Console.WriteLine("Code Block Not Found");
                return NotFound();
            }
        }
    }
}
