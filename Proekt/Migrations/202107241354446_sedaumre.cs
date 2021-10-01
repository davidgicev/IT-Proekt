namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sedaumre : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RelatedMovies", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.RelatedMovies", "RelatedId", "dbo.Movies");
            DropIndex("dbo.RelatedMovies", new[] { "MovieId" });
            DropIndex("dbo.RelatedMovies", new[] { "RelatedId" });
            DropTable("dbo.RelatedMovies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RelatedMovies",
                c => new
                    {
                        MovieId = c.Int(nullable: false),
                        RelatedId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieId, t.RelatedId });
            
            CreateIndex("dbo.RelatedMovies", "RelatedId");
            CreateIndex("dbo.RelatedMovies", "MovieId");
            AddForeignKey("dbo.RelatedMovies", "RelatedId", "dbo.Movies", "Id");
            AddForeignKey("dbo.RelatedMovies", "MovieId", "dbo.Movies", "Id");
        }
    }
}
