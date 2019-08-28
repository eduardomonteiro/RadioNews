namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeOuvintexCidades : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Ouvintes", new[] { "cidadeId" });
            RenameColumn(table: "dbo.Ouvintes", name: "cidadeId", newName: "cidade_id");
            AlterColumn("dbo.Ouvintes", "cidade_id", c => c.Int());
            CreateIndex("dbo.Ouvintes", "cidade_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Ouvintes", new[] { "cidade_id" });
            AlterColumn("dbo.Ouvintes", "cidade_id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Ouvintes", name: "cidade_id", newName: "cidadeId");
            CreateIndex("dbo.Ouvintes", "cidadeId");
        }
    }
}
