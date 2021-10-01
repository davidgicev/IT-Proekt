namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attemp2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Movies", "ActorModel_Id", "dbo.Actors");
            DropIndex("dbo.Movies", new[] { "ActorModel_Id" });
            CreateTable(
                "dbo.ActorModelMovieModels",
                c => new
                    {
                        ActorModel_Id = c.Int(nullable: false),
                        MovieModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ActorModel_Id, t.MovieModel_Id })
                .ForeignKey("dbo.Actors", t => t.ActorModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieModel_Id, cascadeDelete: true)
                .Index(t => t.ActorModel_Id)
                .Index(t => t.MovieModel_Id);
            
            DropColumn("dbo.Movies", "ActorModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "ActorModel_Id", c => c.Int());
            DropForeignKey("dbo.ActorModelMovieModels", "MovieModel_Id", "dbo.Movies");
            DropForeignKey("dbo.ActorModelMovieModels", "ActorModel_Id", "dbo.Actors");
            DropIndex("dbo.ActorModelMovieModels", new[] { "MovieModel_Id" });
            DropIndex("dbo.ActorModelMovieModels", new[] { "ActorModel_Id" });
            DropTable("dbo.ActorModelMovieModels");
            CreateIndex("dbo.Movies", "ActorModel_Id");
            AddForeignKey("dbo.Movies", "ActorModel_Id", "dbo.Actors", "Id");
        }
    }
}
