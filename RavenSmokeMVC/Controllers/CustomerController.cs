using Microsoft.AspNet.Identity;
using RavenSmoke.Models.Customer;
using RavenSmoke.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RavenSmokeMVC.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CustomerService(userId);
            var model = service.GetCustomers();
            return View(model);
        }

        //GET: Create
        //Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Create
        //Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            //Can refactor these 2 lines to CreateCustomerService()
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CustomerService(userId);

            if (service.CreateCustomer(model))
            {
                TempData["SaveResult"] = "Your Customer was created.";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", "Your customer could not be created.");
            return View(model);
        }

        //GET: Details
        //Customer/Details/{id}
        public ActionResult Details(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CustomerService(userId);
            var model = service.GetCustomerById(id);

            return View(model);

        }

        //GET: Edit
        //Customer/Edit/{id}
        public ActionResult Edit(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CustomerService(userId);
            var detail = service.GetCustomerById(id);
            var model = new CustomerEdit
            {
                CustomerId = detail.CustomerId,
                FirstName = detail.FirstName,
                LastName = detail.LastName,
                Address = detail.Address,
            };
            return View(model);
        }

        //POST: Edit
        //Customer/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustomerEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.CustomerId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CustomerService(userId);

            if (service.UpdateCustomer(model))
            {
                TempData["SaveResult"] = "Customer has been updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "The customer could not be updated.");
            return View(model);
        }

        //GET: Delete
        //Customer/Delete/{id}
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CustomerService(userId);
            var model = service.GetCustomerById(id);

            return View(model);
        }

        //POST: Delete
        //Customer/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CustomerService(userId);

            service.DeleteCustomer(id);

            TempData["SaveResult"] = "Customer has been deleted.";
            return RedirectToAction("Index");
        }
        
    }
}