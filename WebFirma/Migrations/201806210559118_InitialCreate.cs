namespace WebFirma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ispyts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Kvals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Otdels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Age = c.Int(nullable: false),
                        OtdelId = c.Int(nullable: false),
                        KvalId = c.Int(nullable: false),
                        IspytId = c.Int(nullable: false),
                        Date_Start = c.DateTime(nullable: false),
                        fl = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ispyts", t => t.IspytId, cascadeDelete: true)
                .ForeignKey("dbo.Kvals", t => t.KvalId, cascadeDelete: true)
                .ForeignKey("dbo.Otdels", t => t.OtdelId, cascadeDelete: true)
                .Index(t => t.OtdelId)
                .Index(t => t.KvalId)
                .Index(t => t.IspytId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "OtdelId", "dbo.Otdels");
            DropForeignKey("dbo.Users", "KvalId", "dbo.Kvals");
            DropForeignKey("dbo.Users", "IspytId", "dbo.Ispyts");
            DropIndex("dbo.Users", new[] { "IspytId" });
            DropIndex("dbo.Users", new[] { "KvalId" });
            DropIndex("dbo.Users", new[] { "OtdelId" });
            DropTable("dbo.Users");
            DropTable("dbo.Otdels");
            DropTable("dbo.Kvals");
            DropTable("dbo.Ispyts");
        }
    }
}
