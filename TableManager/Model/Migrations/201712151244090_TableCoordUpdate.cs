namespace TableManagerData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableCoordUpdate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Tables", new[] { "Location" });
            AddColumn("dbo.Tables", "X", c => c.Int(nullable: false));
            AddColumn("dbo.Tables", "Y", c => c.Int(nullable: false));
            DropColumn("dbo.Tables", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tables", "Location", c => c.String(maxLength: 5));
            DropColumn("dbo.Tables", "Y");
            DropColumn("dbo.Tables", "X");
            CreateIndex("dbo.Tables", "Location", unique: true);
        }
    }
}
