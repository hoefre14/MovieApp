using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieApp.Models;
using System.IO;


namespace MovieApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult LogIn()
        {
            //if (Session["LoggetInn"] != null)
            //{
            //    if((String)Session["LoggetInn"] == "admin")
            //    {
            //
            //    }
            //}
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(Users loginUsers)
        {
            String adminUser = "HouseSchiffer";

            if (loginUsers.Username == adminUser)
            {
                Session["LoggetInn"] = adminUser;
                ViewBag.LoggetInn = true;
            }
            else
            {
                ViewBag.LoggetInn = false;
            }
            return View();
        }

        public ActionResult ShowAllMovies()
        {
            try
            {
                using (var dbkobling = new MovieDBEntities1())
                {
                    var movielist = (from films in dbkobling.Movies
                                     select films).ToList();
                    return View(movielist);
                }
            }
            catch (Exception exeption)
            {
                ViewBag.Feilmelding = exeption.Message;
                return View();
            }

        }

        [HttpGet]
        public ActionResult RegisterMovie()
        {
            if (Session["LoggetInn"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn");
            }

        }

        [HttpPost]
        public ActionResult RegisterMovie(Movies newMovie, HttpPostedFileBase file)
        {
            if (file != null)
            {
                // needs using system.io; for Path. For å finne riktig fil
                String filnavn = Path.GetFileName(file.FileName);

                String filsti = Path.Combine(Server.MapPath("~/Bilder"), filnavn);

                file.SaveAs(filsti);

                newMovie.Picture = filnavn;
            }

            using (var dbkobling = new MovieDBEntities1())
            {
                dbkobling.Movies.Add(newMovie);
                dbkobling.SaveChanges();
            }
            return View();
        }

        [HttpGet]
        public ActionResult DeleteMovie(int id)
        {
            using (var dbkobling = new MovieDBEntities1())
            {
                var deleteMovie = (from films in dbkobling.Movies
                                 where films.Movie_ID == id
                                 select films).SingleOrDefault();


                return View(deleteMovie);
            }
        }

        [HttpPost]
        public ActionResult DeleteMovie(Movies movie)
        {
            using (var dbkobling = new MovieDBEntities1())
            {
                var deleteMovie = (from films in dbkobling.Movies
                                 where films.Movie_ID == movie.Movie_ID
                                 select films).SingleOrDefault();

                dbkobling.Movies.Remove(deleteMovie);
                dbkobling.SaveChanges();

                return RedirectToAction("ShowAllMovies");
            }
        }

        [HttpGet]
        public ActionResult EditMovie(int id)
        {
            using (var dbkobling = new MovieDBEntities1())
            {
                var editMovie = (from film in dbkobling.Movies
                                   where film.Movie_ID == id
                                   select film).SingleOrDefault();


                return View(editMovie);
            }
        }

        [HttpPost]
        public ActionResult EditMovie(Movies film)
        {
            using (var dbkobling = new MovieDBEntities1())
            {
                var editMovie = (from films in dbkobling.Movies
                                   where films.Movie_ID == film.Movie_ID
                                   select films).SingleOrDefault();

                editMovie.Movie_ID = film.Movie_ID;
                editMovie.Title = film.Title;
                editMovie.Picture = film.Picture;
                editMovie.Release_Year = film.Release_Year;
                editMovie.Description = film.Description;
                editMovie.Category_ID = film.Category_ID;

                dbkobling.SaveChanges();

                return RedirectToAction("ShowAllMovies");
            }
        }

    }
}