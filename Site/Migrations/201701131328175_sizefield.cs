namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sizefield : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Editoriais", "Action", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Editoriais", "Action", c => c.String());
        }
    }
}
