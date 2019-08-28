namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class esporteFlagEditoria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Editoriais", "esportes", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Editoriais", "esportes");
        }
    }
}
