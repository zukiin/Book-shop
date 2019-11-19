using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookShop.BusinessLogic;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
//using SendGrid;
using BookShop.Models;
using BookShop;
using BookShop.Models.ViewModels;
//using SendGrid.Helpers.Mail;

namespace MontclairStore.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        Customer_Business cb = new Customer_Business();


        ApplicationDbContext con = new ApplicationDbContext();
        public CustomersController()
        {
        }

        public CustomersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }



        public ActionResult Index()
        {
            if (User.IsInRole("Customer"))
            {
                var owner = HttpContext.User.Identity.Name;
                var user = db.Customers.FirstOrDefault(s => s.Email == owner);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(cb.all().Where(x => x.Email == user.Email));
                }
            }
            else
                return View(cb.all());
        }

        public ActionResult MyIndex(int? id, int? PetID, int? visitID)
        {
            var viewModel = new CustomerIndexData();

            viewModel.Customers = db.Customers
                .OrderBy(i => i.LastName);

            if (id != null)
            {
                ViewBag.CustomerID = id.Value;
            }

            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (cb.findCustomer_by_id(id) != null)
                return View(cb.findCustomer_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerCreateVM model)
        {
            //RolesBusiness rb = new RolesBusiness();

            //if (ModelState.IsValid)
            //{
            //    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Full_Name = model.FirstName + " " + model.LastName, PhoneNumber = model.phone, isCustomer = true, Comm_Method = model.CommMethod, EmailConfirmed = true };
            //    var adminresult = await UserManager.CreateAsync(user, model.Password);

            //    //cb.add(model);

            //    if (adminresult.Succeeded)
            //    {
            //        ApplicationDbContext db = new ApplicationDbContext();
            //        rb.AddUserToRole(db.Users.FirstOrDefault(x => x.UserName == model.Email).Id, WebConstants.CustomerRole);

            //        // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            //        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme);
            //        var client = new SendGridClient("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");
            //        var from = new EmailAddress("no-reply@MontclairOnlineStore.com", "MontClair Veterinary");
            //        var subject = "Confirm your account";
            //        var to = new EmailAddress(model.Email, model.FirstName + " " + model.LastName);
            //        var htmlContent = "Hello " + model.FirstName + " " + model.LastName + ", Kindly confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
            //        var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            //        var response = client.SendEmailAsync(msg);

            //    }

                //Success(string.Format("<b>{0}</b> was successfully added to the database.", model.FirstName), true);
            //    return RedirectToAction("Create");
            //}

            return View(model);
        }
        public ActionResult Edit(int id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (cb.findCustomer_by_id(id) != null)
                return View(cb.findCustomer_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerEntity model)
        {
            if (ModelState.IsValid)
            {
                cb.edit(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        
    }
}
