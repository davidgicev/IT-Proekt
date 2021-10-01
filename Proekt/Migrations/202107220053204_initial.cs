namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActorRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TmdbID = c.String(),
                        Role = c.String(),
                        MovieModel_Id = c.Int(),
                        Actor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.MovieModel_Id)
                .ForeignKey("dbo.Actors", t => t.Actor_Id)
                .Index(t => t.MovieModel_Id)
                .Index(t => t.Actor_Id);
            
            CreateTable(
                "dbo.Actors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TmdbID = c.Int(),
                        Name = c.String(),
                        Poster = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ReleaseDate = c.DateTime(),
                        ImdbID = c.String(),
                        TmdbID = c.Int(),
                        Overview = c.String(),
                        Language = c.String(),
                        Poster = c.String(),
                        Backdrop = c.String(),
                        Runtime = c.Int(),
                        Trailer = c.String(),
                        Status = c.String(),
                        ImdbRating = c.String(),
                        RottenTomatoesRating = c.String(),
                        MetacriticRating = c.String(),
                        Collection_Id = c.Int(),
                        ActorModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Collections", t => t.Collection_Id)
                .ForeignKey("dbo.Actors", t => t.ActorModel_Id)
                .Index(t => t.Collection_Id)
                .Index(t => t.ActorModel_Id);
            
            CreateTable(
                "dbo.Collections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TmdbID = c.Int(),
                        Overview = c.String(),
                        Poster = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TmdbID = c.Int(),
                        Name = c.String(),
                        Country = c.String(),
                        Logo = c.String(),
                        MovieModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.MovieModel_Id)
                .Index(t => t.MovieModel_Id);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TmdbID = c.Int(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Hidden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GenreModelMovieModels",
                c => new
                    {
                        GenreModel_Id = c.Int(nullable: false),
                        MovieModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GenreModel_Id, t.MovieModel_Id })
                .ForeignKey("dbo.Genres", t => t.GenreModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieModel_Id, cascadeDelete: true)
                .Index(t => t.GenreModel_Id)
                .Index(t => t.MovieModel_Id);
            
            CreateTable(
                "dbo.ListModelMovieModels",
                c => new
                    {
                        ListModel_Id = c.Int(nullable: false),
                        MovieModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ListModel_Id, t.MovieModel_Id })
                .ForeignKey("dbo.Lists", t => t.ListModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieModel_Id, cascadeDelete: true)
                .Index(t => t.ListModel_Id)
                .Index(t => t.MovieModel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ActorRoles", "Actor_Id", "dbo.Actors");
            DropForeignKey("dbo.Movies", "ActorModel_Id", "dbo.Actors");
            DropForeignKey("dbo.ListModelMovieModels", "MovieModel_Id", "dbo.Movies");
            DropForeignKey("dbo.ListModelMovieModels", "ListModel_Id", "dbo.Lists");
            DropForeignKey("dbo.GenreModelMovieModels", "MovieModel_Id", "dbo.Movies");
            DropForeignKey("dbo.GenreModelMovieModels", "GenreModel_Id", "dbo.Genres");
            DropForeignKey("dbo.Companies", "MovieModel_Id", "dbo.Movies");
            DropForeignKey("dbo.Movies", "Collection_Id", "dbo.Collections");
            DropForeignKey("dbo.ActorRoles", "MovieModel_Id", "dbo.Movies");
            DropIndex("dbo.ListModelMovieModels", new[] { "MovieModel_Id" });
            DropIndex("dbo.ListModelMovieModels", new[] { "ListModel_Id" });
            DropIndex("dbo.GenreModelMovieModels", new[] { "MovieModel_Id" });
            DropIndex("dbo.GenreModelMovieModels", new[] { "GenreModel_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Companies", new[] { "MovieModel_Id" });
            DropIndex("dbo.Movies", new[] { "ActorModel_Id" });
            DropIndex("dbo.Movies", new[] { "Collection_Id" });
            DropIndex("dbo.ActorRoles", new[] { "Actor_Id" });
            DropIndex("dbo.ActorRoles", new[] { "MovieModel_Id" });
            DropTable("dbo.ListModelMovieModels");
            DropTable("dbo.GenreModelMovieModels");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Lists");
            DropTable("dbo.Genres");
            DropTable("dbo.Companies");
            DropTable("dbo.Collections");
            DropTable("dbo.Movies");
            DropTable("dbo.Actors");
            DropTable("dbo.ActorRoles");
        }
    }
}
