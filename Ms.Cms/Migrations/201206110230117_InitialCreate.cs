namespace Ms.Cms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Cms.Scene",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OwnerGuidId = c.Guid(nullable: false),
                        OwnerLongId = c.Long(nullable: false),
                        OwnerIntId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Cms.Walls",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SceneId = c.Long(nullable: false),
                        Title = c.String(maxLength: 50),
                        Width = c.Single(nullable: false),
                        Order = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Cms.Scene", t => t.SceneId, cascadeDelete: true)
                .Index(t => t.SceneId);
            
            CreateTable(
                "Cms.Bricks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        Width = c.Single(nullable: false),
                        Order = c.Byte(nullable: false),
                        ContentTitle = c.String(maxLength: 50),
                        WallId = c.Long(nullable: false),
                        Html = c.String(),
                        GpsLocation = c.String(maxLength: 100),
                        Height = c.Int(),
                        Zoom = c.Int(),
                        ImageListId = c.Long(),
                        LinkedBrickId = c.Long(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Cms.Walls", t => t.WallId, cascadeDelete: true)
                .ForeignKey("Cms.Bricks", t => t.LinkedBrickId)
                .Index(t => t.WallId)
                .Index(t => t.LinkedBrickId);
            
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
            DropIndex("Cms.Bricks", new[] { "LinkedBrickId" });
            DropIndex("Cms.Bricks", new[] { "WallId" });
            DropIndex("Cms.Walls", new[] { "SceneId" });
            DropForeignKey("Cms.Bricks", "LinkedBrickId", "Cms.Bricks");
            DropForeignKey("Cms.Bricks", "WallId", "Cms.Walls");
            DropForeignKey("Cms.Walls", "SceneId", "Cms.Scene");
            DropTable("Cms.SceneTemplates");
            DropTable("Cms.Bricks");
            DropTable("Cms.Walls");
            DropTable("Cms.Scene");
        }
    }
}
