namespace BookShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shopping1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Carts", "StockNo", "dbo.Stocks");
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "StockNo", "dbo.Stocks");
            DropIndex("dbo.Carts", new[] { "StockNo" });
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            DropIndex("dbo.OrderItems", new[] { "StockNo" });
            CreateTable(
                "dbo.CartItemEntities",
                c => new
                    {
                        cart_item_id = c.String(nullable: false, maxLength: 128),
                        cart_id = c.String(maxLength: 128),
                        BookId = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.cart_item_id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.CartEntities", t => t.cart_id)
                .Index(t => t.cart_id)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.CartEntities",
                c => new
                    {
                        cart_id = c.String(nullable: false, maxLength: 128),
                        date_created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.cart_id);
            
            CreateTable(
                "dbo.CustomerEntities",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 35),
                        LastName = c.String(nullable: false, maxLength: 35),
                        Email = c.String(nullable: false),
                        phone = c.String(nullable: false, maxLength: 10),
                        address = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.OrderEntities",
                c => new
                    {
                        Order_ID = c.String(nullable: false, maxLength: 128),
                        date_created = c.DateTime(nullable: false),
                        customerId = c.Int(nullable: false),
                        Email = c.String(),
                        shipped = c.Boolean(nullable: false),
                        status = c.String(),
                        packed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Order_ID)
                .ForeignKey("dbo.CustomerEntities", t => t.customerId, cascadeDelete: true)
                .Index(t => t.customerId);
            
            CreateTable(
                "dbo.OrderAddressEntities",
                c => new
                    {
                        address_id = c.Int(nullable: false, identity: true),
                        Order_ID = c.String(maxLength: 128),
                        street = c.String(),
                        city = c.String(),
                        zipcode = c.String(),
                    })
                .PrimaryKey(t => t.address_id)
                .ForeignKey("dbo.OrderEntities", t => t.Order_ID)
                .Index(t => t.Order_ID);
            
            CreateTable(
                "dbo.OrderItemEntities",
                c => new
                    {
                        Order_Item_id = c.Int(nullable: false, identity: true),
                        Order_id = c.String(maxLength: 128),
                        BookId = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                        replied = c.Boolean(nullable: false),
                        date_replied = c.DateTime(),
                        accepted = c.Boolean(nullable: false),
                        date_accepted = c.DateTime(),
                        shipped = c.Boolean(nullable: false),
                        status = c.String(),
                        date_shipped = c.DateTime(),
                    })
                .PrimaryKey(t => t.Order_Item_id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.OrderEntities", t => t.Order_id)
                .Index(t => t.Order_id)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.OrderTrackingEntities",
                c => new
                    {
                        tracking_ID = c.Int(nullable: false, identity: true),
                        order_ID = c.String(maxLength: 128),
                        date = c.DateTime(nullable: false),
                        status = c.String(),
                        Recipient = c.String(),
                    })
                .PrimaryKey(t => t.tracking_ID)
                .ForeignKey("dbo.OrderEntities", t => t.order_ID)
                .Index(t => t.order_ID);
            
            CreateTable(
                "dbo.PaymentEntities",
                c => new
                    {
                        payment_ID = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        AmountPaid = c.Double(nullable: false),
                        PaymentFor = c.String(nullable: false),
                        PaymentMethod = c.String(nullable: false),
                        Order_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.payment_ID)
                .ForeignKey("dbo.CustomerEntities", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.OrderEntities", t => t.Order_ID)
                .Index(t => t.CustomerId)
                .Index(t => t.Order_ID);
            
            CreateTable(
                "dbo.ItemSuppliers",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 80),
                        Email = c.String(nullable: false, maxLength: 150),
                        Phone = c.String(nullable: false),
                        Member_Since = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyID)
                .Index(t => t.Name, unique: true, name: "Company_Index")
                .Index(t => t.Email, unique: true, name: "Company_Email_Index");
            
            CreateTable(
                "dbo.SupplierItems",
                c => new
                    {
                        CompanyID = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyID, t.BookId })
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.ItemSuppliers", t => t.CompanyID, cascadeDelete: true)
                .Index(t => t.CompanyID)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.SupplierQuoteItems",
                c => new
                    {
                        QuoteItem_id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.String(maxLength: 128),
                        CompanyID = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        Replied = c.Boolean(nullable: false),
                        DateReplied = c.DateTime(),
                        Accepted = c.Boolean(nullable: false),
                        DateAccepted = c.DateTime(),
                        Shipped = c.Boolean(nullable: false),
                        Status = c.String(),
                        DateShipped = c.DateTime(),
                    })
                .PrimaryKey(t => t.QuoteItem_id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.StockOrders", t => t.OrderNumber)
                .ForeignKey("dbo.ItemSuppliers", t => t.CompanyID, cascadeDelete: true)
                .Index(t => t.OrderNumber)
                .Index(t => t.CompanyID)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.StockOrders",
                c => new
                    {
                        OrderNumber = c.String(nullable: false, maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        Shipped = c.Boolean(nullable: false),
                        Status = c.String(),
                        Packed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderNumber);
            
            CreateTable(
                "dbo.StockOrderItems",
                c => new
                    {
                        OrderItemID = c.Int(nullable: false, identity: true),
                        OrderNumber = c.String(maxLength: 128),
                        BookId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        SubTotal = c.Double(nullable: false),
                        Replied = c.Boolean(nullable: false),
                        DateReplied = c.DateTime(),
                        Accepted = c.Boolean(nullable: false),
                        DateAccepted = c.DateTime(),
                        Shipped = c.Boolean(nullable: false),
                        Status = c.String(),
                        DateShipped = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrderItemID)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.StockOrders", t => t.OrderNumber)
                .Index(t => t.OrderNumber)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.WishList_Item",
                c => new
                    {
                        WishList_Item_Id = c.String(nullable: false, maxLength: 128),
                        BookId = c.Int(nullable: false),
                        WishList_id = c.String(),
                        price = c.Double(nullable: false),
                        isAvailable = c.Boolean(nullable: false),
                        Date_Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.WishList_Item_Id);
            
            CreateTable(
                "dbo.WishLists",
                c => new
                    {
                        WishList_id = c.String(nullable: false, maxLength: 128),
                        date_created = c.DateTime(nullable: false),
                        Wish_Items_WishList_Item_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WishList_id)
                .ForeignKey("dbo.WishList_Item", t => t.Wish_Items_WishList_Item_Id)
                .Index(t => t.Wish_Items_WishList_Item_Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificatioID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        IsViewed = c.Boolean(nullable: false),
                        Url = c.String(),
                        ReplyToEmail = c.String(),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NotificatioID);
            
            CreateTable(
                "dbo.Shipping_Address",
                c => new
                    {
                        Address_ID = c.Int(nullable: false, identity: true),
                        Building_Name = c.String(),
                        Floor = c.String(),
                        Contact_Number = c.String(),
                        Address_Type = c.String(),
                        Comments = c.String(),
                        Order_ID = c.String(maxLength: 128),
                        street_number = c.Int(nullable: false),
                        Street_name = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.Address_ID)
                .ForeignKey("dbo.OrderEntities", t => t.Order_ID)
                .Index(t => t.Order_ID);
            
            CreateTable(
                "dbo.StockCartItems",
                c => new
                    {
                        CartItemID = c.String(nullable: false, maxLength: 128),
                        CartID = c.String(maxLength: 128),
                        BookId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        SubTotal = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CartItemID)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.StockCarts", t => t.CartID)
                .Index(t => t.CartID)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.StockCarts",
                c => new
                    {
                        CartID = c.String(nullable: false, maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CartID);
            
            AddColumn("dbo.Books", "Price", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "Mark_Up", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "ReOrder_Level", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "Quantity_In_Stock", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "QTY_Received", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "ReOrder_Percent", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "WishList_Item_WishList_Item_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Books", "WishList_Item_WishList_Item_Id");
            AddForeignKey("dbo.Books", "WishList_Item_WishList_Item_Id", "dbo.WishList_Item", "WishList_Item_Id");
            DropTable("dbo.Carts");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Orders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Address = c.String(),
                        Order_Status = c.String(),
                        Payement_Status = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderItemId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        StockNo = c.String(maxLength: 128),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Returned = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.OrderItemId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        CartId = c.String(),
                        StockNo = c.String(maxLength: 128),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RecordId);
            
            DropForeignKey("dbo.StockCartItems", "CartID", "dbo.StockCarts");
            DropForeignKey("dbo.StockCartItems", "BookId", "dbo.Books");
            DropForeignKey("dbo.Shipping_Address", "Order_ID", "dbo.OrderEntities");
            DropForeignKey("dbo.WishLists", "Wish_Items_WishList_Item_Id", "dbo.WishList_Item");
            DropForeignKey("dbo.Books", "WishList_Item_WishList_Item_Id", "dbo.WishList_Item");
            DropForeignKey("dbo.SupplierQuoteItems", "CompanyID", "dbo.ItemSuppliers");
            DropForeignKey("dbo.SupplierQuoteItems", "OrderNumber", "dbo.StockOrders");
            DropForeignKey("dbo.StockOrderItems", "OrderNumber", "dbo.StockOrders");
            DropForeignKey("dbo.StockOrderItems", "BookId", "dbo.Books");
            DropForeignKey("dbo.SupplierQuoteItems", "BookId", "dbo.Books");
            DropForeignKey("dbo.SupplierItems", "CompanyID", "dbo.ItemSuppliers");
            DropForeignKey("dbo.SupplierItems", "BookId", "dbo.Books");
            DropForeignKey("dbo.PaymentEntities", "Order_ID", "dbo.OrderEntities");
            DropForeignKey("dbo.PaymentEntities", "CustomerId", "dbo.CustomerEntities");
            DropForeignKey("dbo.OrderTrackingEntities", "order_ID", "dbo.OrderEntities");
            DropForeignKey("dbo.OrderItemEntities", "Order_id", "dbo.OrderEntities");
            DropForeignKey("dbo.OrderItemEntities", "BookId", "dbo.Books");
            DropForeignKey("dbo.OrderAddressEntities", "Order_ID", "dbo.OrderEntities");
            DropForeignKey("dbo.OrderEntities", "customerId", "dbo.CustomerEntities");
            DropForeignKey("dbo.CartItemEntities", "cart_id", "dbo.CartEntities");
            DropForeignKey("dbo.CartItemEntities", "BookId", "dbo.Books");
            DropIndex("dbo.StockCartItems", new[] { "BookId" });
            DropIndex("dbo.StockCartItems", new[] { "CartID" });
            DropIndex("dbo.Shipping_Address", new[] { "Order_ID" });
            DropIndex("dbo.WishLists", new[] { "Wish_Items_WishList_Item_Id" });
            DropIndex("dbo.StockOrderItems", new[] { "BookId" });
            DropIndex("dbo.StockOrderItems", new[] { "OrderNumber" });
            DropIndex("dbo.SupplierQuoteItems", new[] { "BookId" });
            DropIndex("dbo.SupplierQuoteItems", new[] { "CompanyID" });
            DropIndex("dbo.SupplierQuoteItems", new[] { "OrderNumber" });
            DropIndex("dbo.SupplierItems", new[] { "BookId" });
            DropIndex("dbo.SupplierItems", new[] { "CompanyID" });
            DropIndex("dbo.ItemSuppliers", "Company_Email_Index");
            DropIndex("dbo.ItemSuppliers", "Company_Index");
            DropIndex("dbo.PaymentEntities", new[] { "Order_ID" });
            DropIndex("dbo.PaymentEntities", new[] { "CustomerId" });
            DropIndex("dbo.OrderTrackingEntities", new[] { "order_ID" });
            DropIndex("dbo.OrderItemEntities", new[] { "BookId" });
            DropIndex("dbo.OrderItemEntities", new[] { "Order_id" });
            DropIndex("dbo.OrderAddressEntities", new[] { "Order_ID" });
            DropIndex("dbo.OrderEntities", new[] { "customerId" });
            DropIndex("dbo.CartItemEntities", new[] { "BookId" });
            DropIndex("dbo.CartItemEntities", new[] { "cart_id" });
            DropIndex("dbo.Books", new[] { "WishList_Item_WishList_Item_Id" });
            DropColumn("dbo.Books", "WishList_Item_WishList_Item_Id");
            DropColumn("dbo.Books", "ReOrder_Percent");
            DropColumn("dbo.Books", "QTY_Received");
            DropColumn("dbo.Books", "Quantity_In_Stock");
            DropColumn("dbo.Books", "ReOrder_Level");
            DropColumn("dbo.Books", "Mark_Up");
            DropColumn("dbo.Books", "Price");
            DropTable("dbo.StockCarts");
            DropTable("dbo.StockCartItems");
            DropTable("dbo.Shipping_Address");
            DropTable("dbo.Notifications");
            DropTable("dbo.WishLists");
            DropTable("dbo.WishList_Item");
            DropTable("dbo.StockOrderItems");
            DropTable("dbo.StockOrders");
            DropTable("dbo.SupplierQuoteItems");
            DropTable("dbo.SupplierItems");
            DropTable("dbo.ItemSuppliers");
            DropTable("dbo.PaymentEntities");
            DropTable("dbo.OrderTrackingEntities");
            DropTable("dbo.OrderItemEntities");
            DropTable("dbo.OrderAddressEntities");
            DropTable("dbo.OrderEntities");
            DropTable("dbo.CustomerEntities");
            DropTable("dbo.CartEntities");
            DropTable("dbo.CartItemEntities");
            CreateIndex("dbo.OrderItems", "StockNo");
            CreateIndex("dbo.OrderItems", "OrderId");
            CreateIndex("dbo.Carts", "StockNo");
            AddForeignKey("dbo.OrderItems", "StockNo", "dbo.Stocks", "StockNo");
            AddForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
            AddForeignKey("dbo.Carts", "StockNo", "dbo.Stocks", "StockNo");
        }
    }
}
