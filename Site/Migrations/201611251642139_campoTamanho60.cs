namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class campoTamanho60 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Noticias", "subtitulo", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Noticias", "subtitulo", c => c.String(maxLength: 50));
        }
    }
}
