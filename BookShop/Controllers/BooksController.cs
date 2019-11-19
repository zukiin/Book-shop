using BookShop.BusinessLogic;
using BookShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShop.Controllers
{
    public class BooksController : Controller
    {
        // GET: Books
        private ApplicationDbContext db = new ApplicationDbContext();
        private Book_Business ib = new Book_Business();
        Category_Business cb = new Category_Business();
        public string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";
        public BooksController() { }

        public ActionResult Index()
        {
            return View(ib.all());
        }

        public ActionResult Index_OTS()
        {
            return View(ib.all());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (ib.find_by_id(id) != null)
                return View(ib.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }

        public ActionResult Manage_Index()
        {

            return View();
        }

        public ActionResult Create_Category()
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");

            return View();
        }
        public ActionResult Creates()
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName");

            return View();
        }
        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book model, HttpPostedFileBase img_upload)
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName");
            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            model.Image = data;
            //model.Quantity_In_Stock = ib.Calc_Quantity();
            //  model.ReOrder_Level = ib.Calc_ReOrder();
            if (ModelState.IsValid)
            {
                ib.add(model);
               
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (ib.find_by_id(id) != null)
                return View(ib.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book model, HttpPostedFileBase img_upload)
        {
            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            model.Image = data;
            if (ModelState.IsValid)
            {
                ib.edit(model);
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (ib.find_by_id(id) != null)
                return View(ib.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ib.delete(ib.find_by_id(id));
            return RedirectToAction("Index");
        }
      

        //
        [ChildActionOnly]
        public ActionResult OutOfStock()
        {

            return PartialView("_OutOfStock", ib.OutOfStock());
        }
        

        public ActionResult ManageItems(int? id)
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            if (ib.find_by_id(id) != null)
                return View(ib.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageItems(Book model, int? value)
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");

            if (ModelState.IsValid)
            {
                ib.update(model);
             
                return View();
            }
           
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            return View(model);
        }
        public ActionResult Fall_Catalog()
        {
            return View();
        }
        //    public ActionResult Fall_catalog()
        //{
        //    return View(ib.all());
        //}

        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp_cart_ID = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[CartSessionKey] = temp_cart_ID.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[CartSessionKey].ToString();
        }
    }
}
