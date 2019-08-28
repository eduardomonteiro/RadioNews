namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class campomigracao : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Noticias", "migrado", c => c.Boolean(defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Noticias", "migrado");
        }
    }
}
