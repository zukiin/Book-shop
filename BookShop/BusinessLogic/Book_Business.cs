using BookShop.Models;
using BookShop.Models.Shopping;
using BookShop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
//using MontclairModels.ViewModels;

namespace BookShop.BusinessLogic
{
    public class Book_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Book item = new Book();
        private Category category = new Category();

        public List<Book> all()
        {
            return db.Books.Include(i => i.Genre).ToList();
        }
        public bool add(Book model)
        {
            try
            {
                db.Books.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Book model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool delete(Book model)
        {
            try
            {
                db.Books.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception )
            { return false; }
        }
        public Book find_by_id(int? id)
        {
            return db.Books.Find(id);
        }
        public List<Book> find_by_Genre(string genre)
        {
            var gen = db.Genres.ToList().Where(x => x.GenreName == genre).FirstOrDefault();

            return db.Books.ToList().Where(x => x.Genre.GenreName == gen.ToString()).ToList();
        }

        //methods for stock management
        public bool update(Book model)
        {
            try
            {
                var update = db.Books.Find(model.BookId);
                var genre = db.Genres.Where(x => x.GenreId == model.GenreId).FirstOrDefault();

                update.Quantity_In_Stock = update.Quantity_In_Stock + model.QTY_Received;
                update.Price = ((update.Price * update.Mark_Up) / 100) + (update.Price);


                db.Entry(update).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }


        public List<Out_of_stock_VM> OutOfStock()
        {
            var itms = db.Books.ToList();
            //var PE = db.Items.ToList().Where(x => ).Select(p => p.platformID).Single();

            List<Out_of_stock_VM> oosVm = new List<Out_of_stock_VM>();
            var All = (from gen in db.Genres
                       join itm in db.Books.Where(x => x.Quantity_In_Stock == 0) on gen.GenreId equals itm.GenreId
                       select new { gen.GenreName, itm.Title, itm.Quantity_In_Stock, itm.Description, itm.Image }).ToList();

            foreach (var item in All)
            {
                Out_of_stock_VM model = new Out_of_stock_VM();
                model.Name = item.Title;
                model.Category = item.GenreName;
                model.Picture = item.Image;
                model.stockQTY = item.Quantity_In_Stock;
                model.Description = item.Description;
                oosVm.Add(model);
            }
            return (oosVm.ToList());
        }
        public Book findItem_by_id(int? id)
        {
            return db.Books.Find(id);
        }

    }
}