namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlocal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Noticias", "local", c => c.String(maxLength: 350));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Noticias", "local");
        }
    }
}
