using Microsoft.EntityFrameworkCore;
using TomWebApi;
using TomWebApi.Hubs;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Services.AddDbContext<tomcodeblocksContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddSignalR();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<tomcodeblocksContext>();

            if (!context.CodeBlocks.Any())
            {
                context.CodeBlocks.AddRange(
                    new CodeBlock
                    {
                        Title = "Async-Case",
                        InitialCode = @"
                            async function fetchData() {
                            try {
                             const response = await fetch('https://api.example.com/data');
                             const data = await response.json();
                              console.log('Data:', data);
                             } catch (error) {
                              console.error('Error:', error);
                             }
                                    }

                                                    fetchData();",
                        SolutionCode = @"
                            async function fetchData() {
                            try {
                                const response = await fetch('https://api.example.com/data');
                                const data = await response.json();
                                console.log('Data:', data);
                            } catch (error) {
                                console.error('Error fetching data:', error);
                            }
                            }

                            fetchData();"
                    },

                    new CodeBlock
                    {
                        Title = "Callback-Hell",
                        InitialCode = @"
                            function fetchData(callback) {
                                setTimeout(() => {
                                    console.log('Fetching data...');
                                    callback('Data received');
                                }, 1000);
                            }

                            fetchData(function(data) {
                                console.log(data);
                            });",
                        SolutionCode = @"
                            function fetchData(callback) {
                                setTimeout(() => {
                                    console.log('Fetching data...');
                                    callback(null, 'Data received');
                                }, 1000);
                            }

                            fetchData(function(error, data) {
                                if (error) {
                                    console.error('Error:', error);
                                } else {
                                    console.log(data);
                                }
                            });"
                    },

                    new CodeBlock
                    {
                        Title = "Promises-Puzzle",
                        InitialCode = @"
                            function fetchData() {
                                return new Promise((resolve, reject) => {
                                    setTimeout(() => {
                                        resolve('Data received');
                                    }, 1000);
                                });
                            }

                            fetchData().then((data) => {
                                console.log(data);
                            }).catch((error) => {
                                console.error('Error:', error);
                            });",
                        SolutionCode = @"
                            function fetchData() {
                                return new Promise((resolve, reject) => {
                                    setTimeout(() => {
                                        if (Math.random() > 0.5) {
                                            resolve('Data received');
                                        } else {
                                            reject('Error fetching data');
                                        }
                                    }, 1000);
                                });
                            }

                            fetchData().then((data) => {
                                console.log(data);
                            }).catch((error) => {
                                console.error('Error:', error);
                            });"
                    },

                    new CodeBlock
                    {
                        Title = "Event-Loop-Explained",
                        InitialCode = @"
                            console.log('Start');

                            setTimeout(() => {
                                console.log('Inside setTimeout');
                            }, 0);

                            console.log('End');",
                        SolutionCode = @"
                            console.log('Start');

                            setTimeout(() => {
                                console.log('Inside setTimeout');
                            }, 0);

                            Promise.resolve().then(() => {
                                console.log('Inside Promise');
                            });

                            console.log('End');"
                    },

                    new CodeBlock
                    {
                        Title = "Array-Methods-Mastery",
                        InitialCode = @"
                            const numbers = [1, 2, 3, 4, 5];

                            const doubled = numbers.map((number) => {
                                return number * 2;
                            });

                            console.log(doubled);",
                        SolutionCode = @"
                            const numbers = [1, 2, 3, 4, 5];

                            const doubled = numbers.map((number) => number * 2);

                            const sum = doubled.reduce((acc, curr) => acc + curr, 0);

                            console.log('Doubled:', doubled);
                            console.log('Sum:', sum);"
                    }
                );

                context.SaveChanges();
            }
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapHub<CodeBlockHub>("/codeblockHub");
        app.MapControllers();
        app.UseCors("AllowAll");

        app.Run();
    }
}
