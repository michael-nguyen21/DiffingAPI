namespace DiffingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Diff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Diffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DiffId = c.Int(nullable: false),
                        Left = c.String(),
                        Right = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Diffs");
        }
    }
}
