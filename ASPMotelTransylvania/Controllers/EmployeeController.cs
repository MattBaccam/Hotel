using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using DataObjects;
using LogicLayerInterfaces;

namespace ASPScenicHotel.Controllers
{
    [Authorize(Roles = "Front Desk Agent,Housekeeper,Admin")]
    public class EmployeeController : Controller
    {
        IEmployeeManager _employeeManager = new EmployeeManager();
        // GET: Employee
        public ActionResult Index()
        {
            var employee = new Employee();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                employee = _employeeManager.GetEmployeeVM(user.AppID);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(employee);
        }

        // GET: Employee/Edit/
        public ActionResult Edit()
        {
            var employee = new Employee();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                employee = _employeeManager.GetEmployeeVM(user.AppID);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            var employee = new Employee();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                employee = _employeeManager.GetEmployeeVM(user.AppID);
                var newEmployee = new Employee()
                {
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    Email = collection["Email"],
                    Phone = collection["Phone"],
                };
                var newUser = user;
                newUser.FirstName = newEmployee.FirstName;
                newUser.LastName = newEmployee.LastName;
                newUser.Email = newEmployee.Email;
                newUser.PhoneNumber = newEmployee.Phone;
                if (manager.Update(newUser).Succeeded)
                {
                    if (_employeeManager.UpdateEmployeeContactInformation(newEmployee, employee))
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.Error = "Failed to update via identity";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ViewBag.Error != null ? ViewBag.Error += $"\n{ex.Message}": ex.Message;
            }
            return View(employee);
        }
    }
}
