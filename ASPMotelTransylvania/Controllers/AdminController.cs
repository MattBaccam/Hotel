using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ASPScenicHotel.Models;
using LogicLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ASPScenicHotel.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager userManager;

        // GET: Admin
        public ActionResult Index()
        {
            var users = new List<ApplicationUser>();
            try
            {
                userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                users = userManager.Users.OrderBy(user => user.LastName).ToList();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(users);
        }

        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            var applicationUser = new ApplicationUser();
            try
            {
                //ApplicationUser applicationUser = db.ApplicationUsers.Find(id);
                userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                applicationUser = userManager.FindById(id);
                if (applicationUser == null)
                {
                    return HttpNotFound();
                }
                //Get a list of roles the user has an put them into a viewbag as roles
                //along with a  list of roles the user doesnt have noRoles
                var usrMngr = new LogicLayer.EmployeeManager();
                var allRoles = usrMngr.GetPositions().Select(role => role.PositionTitle);

                var roles = userManager.GetRoles(id);
                var noRoles = allRoles.Except(roles);

                ViewBag.Roles = roles;
                ViewBag.NoRoles = noRoles;

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            
            return View(applicationUser);
        }

        public ActionResult RemoveRole(string id, string role)
        {
            if (id == null || role == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            var user = new ApplicationUser();
            try
            {
                userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                user = userManager.Users.First(u => u.Id == id);
                var usrMngr = new LogicLayer.EmployeeManager();
                var allRoles = usrMngr.GetPositions().Select(r => r.PositionTitle);
                if (role == "Admin")
                {
                    var adminUsers = userManager.Users.ToList().Where(u => userManager.IsInRole(u.Id, "Admin")).ToList().Count();
                    if (adminUsers < 2)
                    {
                        ViewBag.Error = "Cannot remove last administrator";
                    }
                    else
                    {
                        userManager.RemoveFromRole(id, role);
                        var oldEmployee = usrMngr.GetEmployeeByEmail(user.Email);
                        var newEmployee = oldEmployee.DeepCopy();
                        newEmployee.PositionID = usrMngr.GetPositions().FirstOrDefault(position => position.PositionTitle == role).PositionID;
                        usrMngr.UpdateEmployeeInformation(newEmployee, oldEmployee);
                    }
                }
                else
                {
                    if (user.Roles.Count >= 1)
                    {
                        var oldEmployee = usrMngr.GetEmployeeByEmail(user.Email);
                        var newEmployee = oldEmployee.DeepCopy();
                        newEmployee.PositionID = usrMngr.GetPositions().FirstOrDefault(position => position.PositionTitle == role).PositionID;
                        usrMngr.UpdateEmployeeInformation(newEmployee, oldEmployee);
                    }
                    else
                    {
                        ViewBag.Error = "Must have at least one role";
                    }
                }
                var roles = userManager.GetRoles(id);
                var noRoles = allRoles.Except(roles);
                ViewBag.Roles = roles;
                ViewBag.NoRoles = noRoles;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Details", user);
        }

        public ActionResult AddRole(string id, string role)
        {
            if(id == null || role == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            var user = new ApplicationUser();
            try
            {
                userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                user = userManager.Users.First(u => u.Id == id);

                userManager.RemoveFromRole(id, user.PositionTitle);//removes the old role because i only want to allow one role per user

                userManager.AddToRole(id, role);
                switch (role)
                {
                    case "Front Desk Agent":
                        user.PositionTitle = "Front Desk Agent";
                        break;
                    case "Admin":
                        user.PositionTitle = "Admin";
                        break;
                    case "Housekeeper":
                        user.PositionTitle = "Housekeeper";
                        break;
                    default:
                        throw new Exception("Role was not under the three categories Admin, Housekeeper, or Front Desk Agent");
                }
                userManager.Update(user);

                var usrMngr = new LogicLayer.EmployeeManager();
                var oldEmployee = usrMngr.GetEmployeeByEmail(user.Email);
                var newEmployee = oldEmployee.DeepCopy();
                newEmployee.PositionID = usrMngr.GetPositions().FirstOrDefault(position => position.PositionTitle == role).PositionID;
                usrMngr.UpdateEmployeeInformation(newEmployee, oldEmployee);//FAILS HERE 
                var allRoles = usrMngr.GetPositions().Select(r => r.PositionTitle);

                var roles = userManager.GetRoles(id);
                var noRoles = allRoles.Except(roles);

                ViewBag.Roles = roles;
                ViewBag.NoRoles = noRoles;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("Details", user);
        }
    }
}
