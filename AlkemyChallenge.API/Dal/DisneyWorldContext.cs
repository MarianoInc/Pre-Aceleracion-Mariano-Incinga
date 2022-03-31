using AlkemyChallenge.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AlkemyChallenge.API.Dal
{
    public class DisneyWorldContext : DbContext
    {
        
        public DisneyWorldContext(DbContextOptions<DisneyWorldContext> options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-R262SOH\\SQLEXPRESS;Database=DisneyWorldDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Indico entidad y declaro la clave.
            builder.Entity<CharacterMovie>()
                .HasKey(cm => new {cm.CharacterId, cm.MovieId});
            //Defino relación uno-a-muchos personaje a películas.
            builder.Entity<CharacterMovie>()
                .HasOne(cm => cm.Character)
                .WithMany(cm => cm.Movies)
                .HasForeignKey(cm => cm.CharacterId);
            //Defino relación uno-a-muchos película a personajes.
            builder.Entity<CharacterMovie>()
                .HasOne(cm => cm.Movie)
                .WithMany(cm => cm.Characters)
                .HasForeignKey(cm => cm.MovieId);
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterMovie> CharactersMovies  { get; set; }
        public DbSet<Genre> Genres { get; set; }

    }
}
