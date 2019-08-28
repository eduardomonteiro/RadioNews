namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDestaqueEditoria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Noticias", "destaqueEditoria", c => c.Boolean(nullable: false));
            DropColumn("dbo.Noticias", "destaqueMenor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Noticias", "destaqueMenor", c => c.Boolean(nullable: false));
            DropColumn("dbo.Noticias", "destaqueEditoria");
        }
    }
}
