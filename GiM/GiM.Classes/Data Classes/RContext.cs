using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiM.Classes.Data_Classes
{
    public class RContext : DbContext
    {
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Note> Notes { get; set; }
    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<Composition>()
    //.HasMany(a => a.Artists)
    //.WithMany(c => c.Compositions).
    //        Map(
    //   m =>
    //   {
    //       m.MapLeftKey("CompositionId");
    //       m.MapRightKey("ArtistId");
    //       m.ToTable("ArtistComposition");
    //   });
    //    }

    }
}