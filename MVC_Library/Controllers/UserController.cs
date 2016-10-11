using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using MVC_Library.Models;
using MVC_Library.Repositories;
using System.Threading.Tasks;
using System.Net;

namespace MVC_Library.Controllers
{
    /// <summary>
    /// The main controller of this application, this controller
    /// can Create, Update, Delete, See details and List ALL users
    /// </summary>
    //[RoutePrefix("Returns")]
    public class UserController : Controller
    {
        /// <summary>
        /// GET: /Initial/
        /// 
        /// List/Get all users into the database
        /// </summary>
        /// <returns>
        /// All users into the database
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<UserModels> users;
            using (var repo = new UserRepository())
            {
                users = repo.GetAll();
            }

            return View("Index", users);
        }

        /// <summary>
        /// GET: /Initial/Details/5
        /// 
        /// Get the entire data of a given user id
        /// </summary>
        /// <param name="id">The id for searching into database</param>
        /// <returns>The found data of the given id</returns>
        public ActionResult Details(int id)
        {
            var user = GetUser(id);
            using (var repo = new BookRepository())
            {
                user.BooksTaken.AddRange(repo.FindTakenBooks(id));
            }
            return View("Details", user);
        }

        [HttpPost]
        public ActionResult Details()
        {
            return View("SendMail");
        }

        /// <summary>
        /// GET: /Initial/Create
        /// 
        /// Show the form to create a new user
        /// </summary>
        /// <returns>
        /// Form to create a new user
        /// </returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: /Initial/Create
        /// 
        /// Create a new user with the given data
        /// </summary>
        /// <param name="user">The user data to put it into database</param>
        /// <returns>
        /// Redirect to navigation page
        /// </returns>
        [HttpPost]
        public ActionResult Create(UserModels user)
        {
            try
            {
                using (var repo = new UserRepository())
                {
                    repo.Create(user);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditByName(string name)
        {
            var model = GetUserByName(name);
            return RedirectToAction("Edit", model);
        }

        /// <summary>
        /// GET: /Initial/Edit/5
        /// 
        /// Show the user data to be edited on form
        /// </summary>
        /// <param name="id">Id to search on database</param>
        /// <returns>
        /// The edit view loaded with user data
        /// </returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View("Edit", GetUser(id));
        }

        /// <summary>
        /// POST: /Initial/Edit/5
        /// 
        /// Persists the modifications into user data inside the database
        /// </summary>
        /// <param name="id">Edited user data</param>
        /// <param name="user">Id of the edited user</param>
        /// <returns>
        /// Redirect to navigation page
        /// </returns>
        [HttpPost]
        public ActionResult Edit(UserModels user)
        {
            try
            {
                using (var repo = new UserRepository())
                {
                    repo.Edit(user);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }

        //
        // GET: /Initial/Delete/5
        public ActionResult Delete(int id)
        {
            return View("Delete", GetUser(id));
        }

        //
        // POST: /Initial/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, UserModels user)
        {
            try
            {
                using (var repo = new UserRepository())
                {
                    repo.Delete(id);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Get the user inside the database using the id with param to search
        /// </summary>
        /// <param name="id">UserModels id to search</param>
        /// <returns>
        /// The found user data
        /// </returns>
        private UserModels GetUser(int id)
        {
            using (var repo = new UserRepository())
            {
                return repo.Details(id);
            }
        }

        private UserModels GetUserByName(string name)
        {
            using (var repo = new UserRepository())
            {
                return repo.Details(name);
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModels user)
        {
            if (ModelState.IsValid)
            {
                using (var repo = new UserRepository())
                {
                    if (repo.IsValid(user))
                    {
                        FormsAuthentication.SetAuthCookie(user.Name, user.RememberMe);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Login data is incorrect!");
                    }
                }
            }
            return View(user);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}
