namespace BookShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shopping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        GenreName = c.String(),
                    })
                .PrimaryKey(t => t.GenreId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Genres", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Genres", new[] { "CategoryId" });
            DropTable("dbo.Genres");
            DropTable("dbo.Categories");
        }
    }
}
