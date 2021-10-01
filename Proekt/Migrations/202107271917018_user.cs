namespace Proekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserDetails", name: "user_Id", newName: "UserId");
            RenameIndex(table: "dbo.UserDetails", name: "IX_user_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UserDetails", name: "IX_UserId", newName: "IX_user_Id");
            RenameColumn(table: "dbo.UserDetails", name: "UserId", newName: "user_Id");
        }
    }
}
