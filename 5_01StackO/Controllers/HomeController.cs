using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _5_01StackO.Models;
using Microsoft.Extensions.Configuration;
using Library.Data;
using Microsoft.AspNetCore.Http;

namespace _5_01StackO.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            Manager mgr = new Manager(_connectionString);
            IEnumerable<Question> qe = mgr.AllQuestions();
            return View(qe);
        }

        public IActionResult Question(int Id)
        {
            Manager mgr = new Manager(_connectionString);
            QuestionViewModel vm = new QuestionViewModel();
            vm.Question = mgr.GetQuestion(Id);
            IEnumerable<Answer>answers= mgr.GetAnswersForQ(Id);
            vm.Answers = answers;
            vm.LoggedIn = User.Identity.IsAuthenticated;
            return View(vm);
        }

        public IActionResult AddQuestion()
        {   if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("/account/login");
            }
        }

        public IActionResult Adding(Question question, IEnumerable<string> tags)
        {
            question.DatePosted = DateTime.Now;
            var mgr = new Manager(_connectionString);
            mgr.AddQuestion(question, tags);
            return Redirect("/home/index");
        }

        public IActionResult Answer(Answer answer)
        {
            answer.DatePosted = DateTime.Now;
            var mgr = new Manager(_connectionString);
            mgr.AddAnswer(answer);
            return Redirect($"/home/question?id={answer.QuestionId}");
        }

        [HttpPost]
        public IActionResult GetLikes(int id)
        {
            Manager mgr = new Manager(_connectionString);
            int likes = mgr.GetLike(id);
            return Json(likes);
        }

        [HttpPost]
        public IActionResult addLike(int Id)
        {
            string Like = HttpContext.Session.GetString(Id.ToString());

            if (Like == null)
            {
                HttpContext.Session.SetString(Id.ToString(), Id.ToString());
                Manager mgr = new Manager(_connectionString);
                mgr.SetLike(Id);
                return Json("Thank You!");
            }
            return Json("You have already 'Liked' this image");
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
