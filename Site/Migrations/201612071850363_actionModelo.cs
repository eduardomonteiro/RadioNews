namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actionModelo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Especiais_Modelos", "Action", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Especiais_Modelos", "Action");
        }
    }
}
