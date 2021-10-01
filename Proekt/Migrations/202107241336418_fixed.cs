namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _fixed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Movies", "MovieModel_Id", "dbo.Movies");
            DropIndex("dbo.Movies", new[] { "MovieModel_Id" });
            CreateTable(
                "dbo.RelatedMovies",
                c => new
                    {
                        MovieId = c.Int(nullable: false),
                        RelatedId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieId, t.RelatedId })
                .ForeignKey("dbo.Movies", t => t.MovieId)
                .ForeignKey("dbo.Movies", t => t.RelatedId)
                .Index(t => t.MovieId)
                .Index(t => t.RelatedId);
            
            DropColumn("dbo.Movies", "MovieModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "MovieModel_Id", c => c.Int());
            DropForeignKey("dbo.RelatedMovies", "RelatedId", "dbo.Movies");
            DropForeignKey("dbo.RelatedMovies", "MovieId", "dbo.Movies");
            DropIndex("dbo.RelatedMovies", new[] { "RelatedId" });
            DropIndex("dbo.RelatedMovies", new[] { "MovieId" });
            DropTable("dbo.RelatedMovies");
            CreateIndex("dbo.Movies", "MovieModel_Id");
            AddForeignKey("dbo.Movies", "MovieModel_Id", "dbo.Movies", "Id");
        }
    }
}
