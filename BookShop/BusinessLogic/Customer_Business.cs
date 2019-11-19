using BookShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace BookShop.BusinessLogic
{
    public class Customer_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<CustomerEntity> all()
        {
            return db.Customers.ToList();
        }

       
        public bool add(CustomerEntity model, string affiliate_key)
        {
            try
            {
                db.Customers.Add(model);
              
                db.SaveChanges();
              
                return true;
            }
            catch (Exception ex)
            { return false; }
        }

        public bool edit(CustomerEntity model)
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
        public CustomerEntity findCustomer_by_email(string email)
        {
            return db.Customers.FirstOrDefault(x => x.Email == email);
        }
        public CustomerEntity findCustomer_by_id(int id)
        {
            return db.Customers.FirstOrDefault(x => x.CustomerId == id);
        }

        public string SendConfirmationEmail(CustomerEntity model, string callbackUrl)
        {
            //create your variables

            string strSubject = "Verify Your email";

            string strFromEmail = "info@ebookshop.co.za";

            string strFromName = "eBook Administrator";


            try
            {
                //create the new mail message[/color]

                MailMessage MailMsg = new MailMessage();
                MailMsg.Sender = new MailAddress(strFromEmail);
                //cretate the FROM[/color]

                MailMsg.From = new MailAddress(strFromEmail);


                //create the subject[/color]

                MailMsg.Subject = strSubject;

                MailMsg.To.Add(model.Email);

                //MailMsg.To.Add(Email); //assuming there was a form to fill out and they put the right email address in.

                MailMsg.IsBodyHtml = true; //I decided to make it html - so I could format the text.

                MailMsg.Body = "<h3>Thank you for registering with eBook Shop. </h3><br />";

                MailMsg.Body += "<p>To take advantage of all we have to offer," +
                    "including access to our catalog, be sure to verify your account by clicking <a href=\"" + callbackUrl + "\">here</a>.<br />";
                MailMsg.Body = "Your registration details as : <br />";
                MailMsg.Body = "<br />";
                MailMsg.Body += "First Name" + " : " + model.FirstName + "<br />";
                MailMsg.Body += "Last Name" + " : " + model.LastName + "<br />";
                MailMsg.Body += "Email" + " : " + model.Email + "<br />";
                MailMsg.Body += "Contact Number" + " : " + model.phone + "<br />";

                MailMsg.Body += "If you didn't register with us you can report this problem by replying to this mail. </br>";


                //utilizing SMTP (simple mail transfer protocol)

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
                //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["G_Email"].ToString(), ConfigurationManager.AppSettings["E_Password"].ToString());
                //smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = true;
                smtpClient.Send(MailMsg); //if my domain had a way of sending out emails.


                //smtp.Send(MailMsg);  //send it

                return "Message Sent Successfully";
            }

            catch (Exception)
            {
                return "Error";

            }
        }

    }
}