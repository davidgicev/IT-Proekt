namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lists : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lists", "Description", c => c.String());
            AddColumn("dbo.Lists", "Poster", c => c.String());
            AddColumn("dbo.Lists", "Backdrop", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lists", "Backdrop");
            DropColumn("dbo.Lists", "Poster");
            DropColumn("dbo.Lists", "Description");
        }
    }
}
