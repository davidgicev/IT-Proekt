namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nazad : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActorModelMovieModels", "ActorModel_Id", "dbo.Actors");
            DropForeignKey("dbo.ActorModelMovieModels", "MovieModel_Id", "dbo.Movies");
            DropIndex("dbo.ActorModelMovieModels", new[] { "ActorModel_Id" });
            DropIndex("dbo.ActorModelMovieModels", new[] { "MovieModel_Id" });
            AddColumn("dbo.Movies", "ActorModel_Id", c => c.Int());
            CreateIndex("dbo.Movies", "ActorModel_Id");
            AddForeignKey("dbo.Movies", "ActorModel_Id", "dbo.Actors", "Id");
            DropTable("dbo.ActorModelMovieModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ActorModelMovieModels",
                c => new
                    {
                        ActorModel_Id = c.Int(nullable: false),
                        MovieModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ActorModel_Id, t.MovieModel_Id });
            
            DropForeignKey("dbo.Movies", "ActorModel_Id", "dbo.Actors");
            DropIndex("dbo.Movies", new[] { "ActorModel_Id" });
            DropColumn("dbo.Movies", "ActorModel_Id");
            CreateIndex("dbo.ActorModelMovieModels", "MovieModel_Id");
            CreateIndex("dbo.ActorModelMovieModels", "ActorModel_Id");
            AddForeignKey("dbo.ActorModelMovieModels", "MovieModel_Id", "dbo.Movies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ActorModelMovieModels", "ActorModel_Id", "dbo.Actors", "Id", cascadeDelete: true);
        }
    }
}
