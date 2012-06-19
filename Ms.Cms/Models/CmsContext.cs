using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace Ms.Cms.Models
{
    public partial class CmsEntities : DbContext
    {
        public CmsEntities(string nameOrConnectionString) 
            : base(nameOrConnectionString)
        {
            var brickAssemblies = MsCms.RegisteredBrickTypes
                .Select(br => br.Type.Assembly)
                .Where(a => a != Assembly.GetExecutingAssembly())
                .Distinct();
            foreach(var assembly in brickAssemblies)
            {
                ((IObjectContextAdapter)this).ObjectContext.MetadataWorkspace.LoadFromAssembly(assembly);
            }
        }

        public CmsEntities()
            : this("Ms.Cms")
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
