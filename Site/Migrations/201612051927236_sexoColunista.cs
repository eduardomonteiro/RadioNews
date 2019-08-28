namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sexoColunista : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Colunista", "sexo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Colunista", "sexo");
        }
    }
}
