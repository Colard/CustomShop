namespace CustomShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Purchases", "SizeId", "dbo.Sizes");
            DropForeignKey("dbo.Colors", "GoodId", "dbo.Goods");
            DropIndex("dbo.Purchases", new[] { "SizeId" });
            DropIndex("dbo.Colors", new[] { "GoodId" });
            AddColumn("dbo.Carts", "SizeId", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "ColorId", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "Client_Id", c => c.Int());
            AlterColumn("dbo.Goods", "Image", c => c.Binary());
            AlterColumn("dbo.Goods", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Goods", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Colors", "GoodId", c => c.Int());
            CreateIndex("dbo.Carts", "ColorId");
            CreateIndex("dbo.Carts", "Client_Id");
            CreateIndex("dbo.Colors", "GoodId");
            AddForeignKey("dbo.Carts", "Client_Id", "dbo.Clients", "Id");
            AddForeignKey("dbo.Carts", "ColorId", "dbo.Colors", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Colors", "GoodId", "dbo.Goods", "Id");
            DropColumn("dbo.Purchases", "SizeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Purchases", "SizeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Colors", "GoodId", "dbo.Goods");
            DropForeignKey("dbo.Carts", "ColorId", "dbo.Colors");
            DropForeignKey("dbo.Carts", "Client_Id", "dbo.Clients");
            DropIndex("dbo.Colors", new[] { "GoodId" });
            DropIndex("dbo.Carts", new[] { "Client_Id" });
            DropIndex("dbo.Carts", new[] { "ColorId" });
            AlterColumn("dbo.Colors", "GoodId", c => c.Int(nullable: false));
            AlterColumn("dbo.Goods", "Price", c => c.Double(nullable: false));
            AlterColumn("dbo.Goods", "Description", c => c.String());
            AlterColumn("dbo.Goods", "Image", c => c.Binary(nullable: false));
            DropColumn("dbo.Carts", "Client_Id");
            DropColumn("dbo.Carts", "ColorId");
            DropColumn("dbo.Carts", "SizeId");
            CreateIndex("dbo.Colors", "GoodId");
            CreateIndex("dbo.Purchases", "SizeId");
            AddForeignKey("dbo.Colors", "GoodId", "dbo.Goods", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Purchases", "SizeId", "dbo.Sizes", "Id", cascadeDelete: true);
        }
    }
}
