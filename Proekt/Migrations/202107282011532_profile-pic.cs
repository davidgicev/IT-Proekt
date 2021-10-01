namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profilepic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDetails", "Picture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDetails", "Picture");
        }
    }
}
