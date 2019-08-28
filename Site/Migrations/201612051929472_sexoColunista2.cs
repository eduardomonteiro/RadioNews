namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sexoColunista2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Colunista", "sexo", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Colunista", "sexo", c => c.String());
        }
    }
}
