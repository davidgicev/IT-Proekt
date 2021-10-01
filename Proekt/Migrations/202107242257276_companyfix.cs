namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyfix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Companies", "MovieModel_Id", "dbo.Movies");
            DropIndex("dbo.Companies", new[] { "MovieModel_Id" });
            CreateTable(
                "dbo.MovieModelCompanyModels",
                c => new
                    {
                        MovieModel_Id = c.Int(nullable: false),
                        CompanyModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieModel_Id, t.CompanyModel_Id })
                .ForeignKey("dbo.Movies", t => t.MovieModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.CompanyModel_Id, cascadeDelete: true)
                .Index(t => t.MovieModel_Id)
                .Index(t => t.CompanyModel_Id);
            
            DropColumn("dbo.Companies", "MovieModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "MovieModel_Id", c => c.Int());
            DropForeignKey("dbo.MovieModelCompanyModels", "CompanyModel_Id", "dbo.Companies");
            DropForeignKey("dbo.MovieModelCompanyModels", "MovieModel_Id", "dbo.Movies");
            DropIndex("dbo.MovieModelCompanyModels", new[] { "CompanyModel_Id" });
            DropIndex("dbo.MovieModelCompanyModels", new[] { "MovieModel_Id" });
            DropTable("dbo.MovieModelCompanyModels");
            CreateIndex("dbo.Companies", "MovieModel_Id");
            AddForeignKey("dbo.Companies", "MovieModel_Id", "dbo.Movies", "Id");
        }
    }
}
