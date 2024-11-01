using Gestao.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao.Data
{
    public class GestaoDbContext : DbContext
    {
        public GestaoDbContext(DbContextOptions<GestaoDbContext> options) : base(options)
        {
        }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<usuarios> user{ get; set; }
    }
}
