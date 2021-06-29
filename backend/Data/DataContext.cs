using Microsoft.EntityFrameworkCore;
using PetFelizApi.Models;
using PetFelizApi.Models.Enuns;

namespace PetFelizApi.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }   
        
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Cao> Cao { get; set; }
        public DbSet<InformacoesServicoDogWalker> ServicoDogWalker { get; set; }
        public DbSet<Servico> Servico { get; set; }
        public DbSet<UsuariosServico> UsuariosServico { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuariosServico>()
                .HasKey(us => new { us.UsuarioId, us.ServicoId });

            modelBuilder.Entity<CaoServico>()
                .HasKey(cs => new { cs.CaoId, cs.ServicoId});

            modelBuilder.Entity<UsuarioAvaliacao>()
                .HasKey(ua => new { ua.AvaliacaoId, ua.UsuarioId});
        }

        public DbSet<CaoServico> CaesServico { get ;set; }
        public DbSet<PesoCao> PesoCao { get; set; }
        public DbSet<Curso> Curso { get; set; }

        public DbSet<Avaliacao> Avaliacao { get; set; }
        public DbSet<UsuarioAvaliacao> UsuarioAvaliacao { get; set; }
        
    }
}