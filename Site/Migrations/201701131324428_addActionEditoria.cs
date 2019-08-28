namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addActionEditoria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Editoriais", "Action", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Editoriais", "Action");
        }
    }
}
