using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Library.Models;
using MVC_Library.Repositories;
using MVC_Library.Repositories.DAL;

namespace MVC_Library.Controllers
{
    public class BookController : Controller
    {

        //[HttpGet]
        public ActionResult TakeBook(int bookId, string username)
        {
            if (!String.IsNullOrEmpty(username))
            {
                var book = GetBook(bookId);
                if ((int)book.Quantity > 0)
                {
                    book.Quantity--;
                    using (var repo = new BookRepository())
                    {
                        repo.Edit(book);
                        repo.BindBookToUser(book.Id, username);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /BookModels/
        public ActionResult Index()
        {
            IEnumerable<BookModels> books;
            using (var repo = new BookRepository())
            {
                books = repo.FindBooks();
            }
            foreach (var book in books)
            {
                using (var repo_a = new AuthorRepository())
                {
                    //ViewBag.AuthorList = repo.GetAll().ToList();
                    book.AuthorList = repo_a.FindAuthorsByBook(book.Id).ToList();
                }
            }

            return View("Index", books);
        }

        //
        // GET: /BookModels/Details/5

        public ActionResult Details(int id)
        {
            var book = GetBook(id);

            using (var repo_a = new AuthorRepository())
            {
                ViewBag.AuthorList = repo_a.GetAll();
                //ViewBag.AuthorList = repo_a.FindAuthorsByBook(book.Id).ToList();
            }

            return View("Details", GetBook(id));
        }

        private BookModels GetBook(int bookId)
        {
            var model = new BookModels();
            using (var repo = new BookRepository())
            {
                model = repo.Details(bookId);
            }
            using (var repo_a = new AuthorRepository())
            {
                model.AuthorList = repo_a.FindAuthorsByBook(bookId).ToList();
            }
            foreach (var i in model.AuthorList)
            {
                model.SelectedAuthors.Add(i.Id);
            }
            return model;
        }

        /// <summary>
        /// GET: /Initial/Create
        /// 
        /// Show the form to create a new user
        /// </summary>
        /// <returns>
        /// Form to create a new user
        /// </returns>
        /// 
        public ActionResult Create()
        {
            var model = new BookModels();
            using (var repo = new AuthorRepository())
            {
                model.AuthorList = repo.GetAll().ToList();
            }
            return View(model);
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
        public ActionResult Create(BookModels book)
        {
            try
            {
                using (var repo = new BookRepository())
                {
                    repo.Create(book);
                    book.Id = repo.GetBookByTitle(book.Title);
                }

                foreach (var author in book.SelectedAuthors)
                {
                    using (var repo = new BookRepository())
                    {
                        repo.BindAuthorToBook(book.Id, author);
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /BookModels/Edit/5
        public ActionResult Edit(int id)
        {
            using (var repo = new AuthorRepository())
            {
                ViewBag.AuthorList = repo.GetAll().ToList();
            }
            return View("Edit", GetBook(id));
        }

        //
        // POST: /BookModels/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, BookModels editBook)
        {
            try
            {
                using (var repo = new BookRepository())
                {
                    repo.Edit(editBook);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /BookModels/Delete/5

        public ActionResult Delete(int id)
        {
            return View("Delete", GetBook(id));
        }

        //
        // POST: /BookModels/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (var repo = new BookRepository())
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
    }
}
