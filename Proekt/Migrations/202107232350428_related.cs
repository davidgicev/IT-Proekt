namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class related : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "MovieModel_Id", c => c.Int());
            CreateIndex("dbo.Movies", "MovieModel_Id");
            AddForeignKey("dbo.Movies", "MovieModel_Id", "dbo.Movies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movies", "MovieModel_Id", "dbo.Movies");
            DropIndex("dbo.Movies", new[] { "MovieModel_Id" });
            DropColumn("dbo.Movies", "MovieModel_Id");
        }
    }
}
