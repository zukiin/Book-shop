using BookShop.Models;
using BookShop.Models.Shopping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookShop.BusinessLogic
{
    public class Category_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Category> all()
        {

            return db.Categories.ToList();
        }
        //public List<CategoryEntity> all2(int? id, int? ItemID)
        //{
        //    var viewModel = new DepartmentIndexData();

        //    viewModel.Categories = db.Categories
        //            .Include(i => i.Departments)
        //            .Include(i => i.Items.Select(c => c.Name))
        //            .OrderBy(i => i.Category_ID);

        //    //if (id != null)
        //    //{
        //    //    ViewBag.ItemID = id.Value;
        //    //    viewModel.Items = viewModel.Categories.Where(
        //    //        i => i.Category_ID == id.Value).Single().Items;
        //    //}


        //    return db.Categories.Include(i => i.Departments).ToList();
        //}
        public bool add(Category model)
        {
            try
            {
                db.Categories.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Category model)
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
        public bool delete(Category model)
        {
            try
            {
                db.Categories.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Category find_by_id(int? id)
        {
            return db.Categories.Find(id);
        }
        public List<Book> category_items(int? id)
        {
            return find_by_id(id).books.ToList();
        }
    }
}