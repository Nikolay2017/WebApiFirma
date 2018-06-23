namespace WebFirma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Age = c.Int(nullable: false),
                        OtdelId = c.Int(nullable: false),
                        KvalId = c.Int(nullable: false),
                        IspytId = c.Int(nullable: false),
                        Date_Start = c.DateTime(nullable: false),
                        fl = c.Boolean(nullable: false),
                        ActionUsers = c.String(),
                        DateActionUsers = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UsersHistories");
        }
    }
}
