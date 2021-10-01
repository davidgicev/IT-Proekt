namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usertest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.UserDetailsModelMovieModels",
                c => new
                    {
                        UserDetailsModel_Id = c.Int(nullable: false),
                        MovieModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserDetailsModel_Id, t.MovieModel_Id })
                .ForeignKey("dbo.UserDetails", t => t.UserDetailsModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieModel_Id, cascadeDelete: true)
                .Index(t => t.UserDetailsModel_Id)
                .Index(t => t.MovieModel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserDetailsModelMovieModels", "MovieModel_Id", "dbo.Movies");
            DropForeignKey("dbo.UserDetailsModelMovieModels", "UserDetailsModel_Id", "dbo.UserDetails");
            DropIndex("dbo.UserDetailsModelMovieModels", new[] { "MovieModel_Id" });
            DropIndex("dbo.UserDetailsModelMovieModels", new[] { "UserDetailsModel_Id" });
            DropIndex("dbo.UserDetails", new[] { "user_Id" });
            DropTable("dbo.UserDetailsModelMovieModels");
            DropTable("dbo.UserDetails");
        }
    }
}
