using System.Web.Mvc;
using System.Collections.Generic;
using MVC_Library.Repositories;
using MVC_Library.Models;

namespace MVC_Library.Controllers
{
    public class AuthorController : Controller
    {
        /// <summary>
        /// GET: /Initial/
        /// 
        /// List/Get all users into the database
        /// </summary>
        /// <returns>
        /// All users into the database
        /// </returns>
        public ActionResult Index()
        {
            IEnumerable<AuthorModels> authors;
            using (var repo = new AuthorRepository())
            {
                authors = repo.GetAll();
            }
            foreach (var author in authors)
            {
                using (var repo_a = new BookRepository())
                {
                    author.BookList = (List<BookModels>)(repo_a.FindBooksByAuthor(author.Id));
                }
            }

            return View("Index", authors);
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
            return View("Details", GetAuthor(id));
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
        /// <param name="author">The user data to put it into database</param>
        /// <returns>
        /// Redirect to navigation page
        /// </returns>
        [HttpPost]
        public ActionResult Create(AuthorModels author)
        {
            try
            {
                using (var repo = new AuthorRepository())
                {
                    repo.Create(author);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
        public ActionResult Edit(int id)
        {
            return View("Edit", GetAuthor(id));
        }

        /// <summary>
        /// POST: /Initial/Edit/5
        /// 
        /// Persists the modifications into user data inside the database
        /// </summary>
        /// <param name="id">Edited user data</param>
        /// <param name="editAuthor">Id of the edited user</param>
        /// <returns>
        /// Redirect to navigation page
        /// </returns>
        [HttpPost]
        public ActionResult Edit(int id, AuthorModels editAuthor)
        {
            try
            {
                using (var repo = new AuthorRepository())
                {
                    repo.Edit(editAuthor);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Initial/Delete/5

        public ActionResult Delete(int id)
        {
            return View("Delete", GetAuthor(id));
        }

        //
        // POST: /Initial/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (var repo = new AuthorRepository())
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
        /// <param name="id">AuthorModels id to search</param>
        /// <returns>
        /// The found user data
        /// </returns>
        private AuthorModels GetAuthor(int id)
        {
            AuthorModels model;
            using (var repo = new AuthorRepository())
            {
                model = repo.Details(id);
            }

            return model;
        }
    }
}
