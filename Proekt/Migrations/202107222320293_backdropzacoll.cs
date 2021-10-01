namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class backdropzacoll : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Collections", "Backdrop", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Collections", "Backdrop");
        }
    }
}
