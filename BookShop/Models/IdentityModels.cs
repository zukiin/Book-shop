using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using BookShop.Models.Shopping;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookShop.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<BookShop.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<BookShop.Models.Genre> Genres { get; set; }

        //Temporal Shopping
        public DbSet<CartEntity> Carts { get; set; }
        public DbSet<CartItemEntity> Cart_Items { get; set; }
        //Shopping
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> Order_Items { get; set; }
        public DbSet<OrderAddressEntity> Order_Addresses { get; set; }

        public DbSet<OrderTrackingEntity> Order_Trackings { get; set; }

        public DbSet<Shipping_Address> Shipping_Addresses { get; set; }

        public DbSet<PaymentEntity> Payments { get; set; }
        //
        public DbSet<CustomerEntity> Customers { get; set; }

        //Stock Control
        public DbSet<SupplierItem> SupplierItems { get; set; }
        public DbSet<SupplierQuoteItem> SupplierQuoteItems { get; set; }
        public DbSet<StockOrder> StockOrders { get; set; }
        public DbSet<StockOrderItem> StockOrderItems { get; set; }
        public DbSet<StockCart> StockCarts { get; set; }
        public DbSet<StockCartItem> StockCartItems { get; set; }
        public DbSet<Notifications> Notificationss { get; set; }
        public DbSet<ItemSupplier> ItemSuppliers { get; set; }

        //Wihslist 
        public DbSet<WishList> wishLists { get; set; }
        public DbSet<WishList_Item> list_Items { get; set; }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public System.Data.Entity.DbSet<BookShop.Models.Book> Books { get; set; }
    }
}