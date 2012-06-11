using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Ms.Cms.Models
{
    public partial class CmsEntities : DbContext
    {
        public CmsEntities(string nameOrConnectionString) 
            : base(nameOrConnectionString)
        {
        }

        public CmsEntities()
            : base("Ms.Cms")
        {
        }
    
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //}

        public DbSet<Scene> Scenes { get; set; }
        public DbSet<Wall> Walls { get; set; }
        public DbSet<Brick> Bricks { get; set; }       

        public DbSet<SceneTemplate> SceneTemplates { get; set; }
    }
}
