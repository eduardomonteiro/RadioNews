namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Radios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Radios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 50),
                        Stream1 = c.String(maxLength: 400),
                        Stream2 = c.String(maxLength: 400),
                        Chave = c.String(maxLength: 150),
                        Ativo = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Radios");
        }
    }
}
