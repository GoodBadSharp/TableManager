namespace TableManagerData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Status_Id", "dbo.OrderStatus");
            DropForeignKey("dbo.Orders", "Table_Id", "dbo.Tables");
            DropForeignKey("dbo.Orders", "Waiter_Id", "dbo.Waiters");
            DropForeignKey("dbo.Tables", "Status_Id", "dbo.TableStatus");
            DropIndex("dbo.Orders", new[] { "Status_Id" });
            DropIndex("dbo.Orders", new[] { "Table_Id" });
            DropIndex("dbo.Orders", new[] { "Waiter_Id" });
            DropIndex("dbo.Tables", new[] { "Status_Id" });
            AlterColumn("dbo.Orders", "Status_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "Table_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "Waiter_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Tables", "Status_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "Status_Id");
            CreateIndex("dbo.Orders", "Table_Id");
            CreateIndex("dbo.Orders", "Waiter_Id");
            CreateIndex("dbo.Tables", "Status_Id");
            AddForeignKey("dbo.Orders", "Status_Id", "dbo.OrderStatus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "Table_Id", "dbo.Tables", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "Waiter_Id", "dbo.Waiters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tables", "Status_Id", "dbo.TableStatus", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tables", "Status_Id", "dbo.TableStatus");
            DropForeignKey("dbo.Orders", "Waiter_Id", "dbo.Waiters");
            DropForeignKey("dbo.Orders", "Table_Id", "dbo.Tables");
            DropForeignKey("dbo.Orders", "Status_Id", "dbo.OrderStatus");
            DropIndex("dbo.Tables", new[] { "Status_Id" });
            DropIndex("dbo.Orders", new[] { "Waiter_Id" });
            DropIndex("dbo.Orders", new[] { "Table_Id" });
            DropIndex("dbo.Orders", new[] { "Status_Id" });
            AlterColumn("dbo.Tables", "Status_Id", c => c.Int());
            AlterColumn("dbo.Orders", "Waiter_Id", c => c.Int());
            AlterColumn("dbo.Orders", "Table_Id", c => c.Int());
            AlterColumn("dbo.Orders", "Status_Id", c => c.Int());
            CreateIndex("dbo.Tables", "Status_Id");
            CreateIndex("dbo.Orders", "Waiter_Id");
            CreateIndex("dbo.Orders", "Table_Id");
            CreateIndex("dbo.Orders", "Status_Id");
            AddForeignKey("dbo.Tables", "Status_Id", "dbo.TableStatus", "Id");
            AddForeignKey("dbo.Orders", "Waiter_Id", "dbo.Waiters", "Id");
            AddForeignKey("dbo.Orders", "Table_Id", "dbo.Tables", "Id");
            AddForeignKey("dbo.Orders", "Status_Id", "dbo.OrderStatus", "Id");
        }
    }
}
