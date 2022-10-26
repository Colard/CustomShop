namespace CustomShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        GoodId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goods", t => t.GoodId, cascadeDelete: true)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.PurchaseId)
                .Index(t => t.GoodId);
            
            CreateTable(
                "dbo.Goods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Image = c.Binary(nullable: false),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        GoodTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodTypes", t => t.GoodTypeId, cascadeDelete: true)
                .Index(t => t.GoodTypeId);
            
            CreateTable(
                "dbo.GoodTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ClientId = c.Int(nullable: false),
                        PurchaseStateId = c.Int(nullable: false),
                        SizeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseStates", t => t.PurchaseStateId, cascadeDelete: true)
                .ForeignKey("dbo.Sizes", t => t.SizeId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.PurchaseStateId)
                .Index(t => t.SizeId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Surname = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        E_Mail = c.String(nullable: false),
                        Adress = c.String(nullable: false),
                        PostIndex = c.String(nullable: false, maxLength: 5),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseStates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        GoodId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goods", t => t.GoodId, cascadeDelete: true)
                .Index(t => t.GoodId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Colors", "GoodId", "dbo.Goods");
            DropForeignKey("dbo.Carts", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.Purchases", "SizeId", "dbo.Sizes");
            DropForeignKey("dbo.Purchases", "PurchaseStateId", "dbo.PurchaseStates");
            DropForeignKey("dbo.Purchases", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Carts", "GoodId", "dbo.Goods");
            DropForeignKey("dbo.Goods", "GoodTypeId", "dbo.GoodTypes");
            DropIndex("dbo.Colors", new[] { "GoodId" });
            DropIndex("dbo.Clients", new[] { "PostId" });
            DropIndex("dbo.Purchases", new[] { "SizeId" });
            DropIndex("dbo.Purchases", new[] { "PurchaseStateId" });
            DropIndex("dbo.Purchases", new[] { "ClientId" });
            DropIndex("dbo.Goods", new[] { "GoodTypeId" });
            DropIndex("dbo.Carts", new[] { "GoodId" });
            DropIndex("dbo.Carts", new[] { "PurchaseId" });
            DropTable("dbo.Colors");
            DropTable("dbo.Sizes");
            DropTable("dbo.PurchaseStates");
            DropTable("dbo.Posts");
            DropTable("dbo.Clients");
            DropTable("dbo.Purchases");
            DropTable("dbo.GoodTypes");
            DropTable("dbo.Goods");
            DropTable("dbo.Carts");
        }
    }
}
