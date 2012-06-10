namespace Ms.Cms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Cms.Walls",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OwnerId = c.Guid(nullable: false),
                        Title = c.String(maxLength: 50),
                        Width = c.Single(nullable: false),
                        Order = c.Byte(nullable: false),
                        SceneTemplate_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Cms.SceneTemplates", t => t.SceneTemplate_Id)
                .Index(t => t.SceneTemplate_Id);
            
            CreateTable(
                "Cms.Bricks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        Width = c.Single(nullable: false),
                        Order = c.Byte(nullable: false),
                        ContentTitle = c.String(maxLength: 50),
                        Html = c.String(),
                        GpsLocation = c.String(maxLength: 100),
                        Height = c.Int(),
                        Zoom = c.Int(),
                        ImageListId = c.Long(),
                        SharedBrickId = c.Long(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Wall_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Cms.Walls", t => t.Wall_Id)
                .Index(t => t.Wall_Id);
            
            CreateTable(
                "Cms.SceneTemplates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IconUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("Cms.Bricks", new[] { "Wall_Id" });
            DropIndex("Cms.Walls", new[] { "SceneTemplate_Id" });
            DropForeignKey("Cms.Bricks", "Wall_Id", "Cms.Walls");
            DropForeignKey("Cms.Walls", "SceneTemplate_Id", "Cms.SceneTemplates");
            DropTable("Cms.SceneTemplates");
            DropTable("Cms.Bricks");
            DropTable("Cms.Walls");
        }
    }
}
