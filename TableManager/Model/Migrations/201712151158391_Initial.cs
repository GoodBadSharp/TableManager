namespace TableManagerData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dishes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DishInOrders",
                c => new
                    {
                        DishID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DishID, t.OrderID })
                .ForeignKey("dbo.Dishes", t => t.DishID, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.DishID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderTime = c.DateTime(nullable: false),
                        Status_Id = c.Int(),
                        Table_Id = c.Int(),
                        Waiter_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderStatus", t => t.Status_Id)
                .ForeignKey("dbo.Tables", t => t.Table_Id)
                .ForeignKey("dbo.Waiters", t => t.Waiter_Id)
                .Index(t => t.Status_Id)
                .Index(t => t.Table_Id)
                .Index(t => t.Waiter_Id);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfSeats = c.Int(nullable: false),
                        Location = c.String(maxLength: 5),
                        Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TableStatus", t => t.Status_Id)
                .Index(t => t.Location, unique: true)
                .Index(t => t.Status_Id);
            
            CreateTable(
                "dbo.TableStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Waiters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DishInOrders", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Waiter_Id", "dbo.Waiters");
            DropForeignKey("dbo.Tables", "Status_Id", "dbo.TableStatus");
            DropForeignKey("dbo.Orders", "Table_Id", "dbo.Tables");
            DropForeignKey("dbo.Orders", "Status_Id", "dbo.OrderStatus");
            DropForeignKey("dbo.DishInOrders", "DishID", "dbo.Dishes");
            DropIndex("dbo.Tables", new[] { "Status_Id" });
            DropIndex("dbo.Tables", new[] { "Location" });
            DropIndex("dbo.Orders", new[] { "Waiter_Id" });
            DropIndex("dbo.Orders", new[] { "Table_Id" });
            DropIndex("dbo.Orders", new[] { "Status_Id" });
            DropIndex("dbo.DishInOrders", new[] { "OrderID" });
            DropIndex("dbo.DishInOrders", new[] { "DishID" });
            DropTable("dbo.Waiters");
            DropTable("dbo.TableStatus");
            DropTable("dbo.Tables");
            DropTable("dbo.OrderStatus");
            DropTable("dbo.Orders");
            DropTable("dbo.DishInOrders");
            DropTable("dbo.Dishes");
        }
    }
}
