using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TomWebApi
{
    public class tomcodeblocksContext : DbContext
    {
        public tomcodeblocksContext()
        {
        }

        public tomcodeblocksContext(DbContextOptions<tomcodeblocksContext> options) : base(options) { }

        public DbSet<CodeBlock> CodeBlocks { get; set; }
    

    }
    public class CodeBlock
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string InitialCode { get; set; } = string.Empty;
        public string SolutionCode { get; set; } = string.Empty;
    }
}

