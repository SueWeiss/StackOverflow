using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Data
{
    public class Manager
    {
        string _connectionString { get; set; }
        public Manager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<Question> AllQuestions()
        {
            using (var context = new QuestionsContext(_connectionString))
            {
                return context.Questions.ToList().OrderByDescending(d => d.DatePosted);
            }
        }
        public Question GetQuestion(int Id)
        {
            using (var context = new QuestionsContext(_connectionString))
            {
                return context.Questions.Include(q=>q.QuestionTags).FirstOrDefault(q => q.Id == Id);
            }
        }
       

        public void AddQuestion(Question question, IEnumerable<string> tags)
        {
            using (var ctx = new QuestionsContext(_connectionString))
            {
                ctx.Questions.Add(question);
                foreach (string tag in tags)
                {
                    Tag t = GetTag(tag);
                    int tagId;
                    if (t == null)
                    {
                        tagId = AddTag(tag);
                    }
                    else
                    {
                        tagId = t.Id;
                    }
                    ctx.QuestionTags.Add(new QuestionTags
                    {
                        QuestionId = question.Id,
                        TagId = tagId
                    });
                }

                ctx.SaveChanges();
            }
        }

        public void AddAnswer(Answer answer)
        {
            using (var context = new QuestionsContext(_connectionString))
            {
                context.Answers.Add(answer);
                context.SaveChanges();

            }
        }
        public IEnumerable<Answer> GetAnswersForQ(int questionId)
        {
            using (var context = new QuestionsContext(_connectionString))
            {
                return context.Answers.Where(a => a.QuestionId == questionId).ToList();
            }
        }
        //Tags
        private Tag GetTag(string name)
        {
            using (var contex = new QuestionsContext(_connectionString))
            {
                return contex.Tags.FirstOrDefault(t => t.Name == name);
            }
        }

        private int AddTag(string name)
        {
            using (var contex = new QuestionsContext(_connectionString))
            {
                var tag = new Tag { Name = name };
                contex.Tags.Add(tag);
                contex.SaveChanges();
                return tag.Id;
            }
        }
        //Likes  
        public void SetLike(int id)
        {
            using (var context = new QuestionsContext(_connectionString))
            {
                var question = context.Questions.FirstOrDefault(q => q.Id == id);
                question.Likes++;
                context.SaveChanges();
            }
        }

        public int GetLike(int id)
        {
            using (var context = new QuestionsContext(_connectionString))
            {
                return context.Questions.FirstOrDefault(p => p.Id == id).Likes;
            }
        }

        //LogIns
        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = hash;
            using (var context = new QuestionsContext(_connectionString))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null; //incorrect email
            }
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid)
            {
                return null;
            }

            return user;
        }

        public User GetByEmail(string email)
        {
            using (var context = new QuestionsContext(_connectionString))
            {
              return  context.Users.ToList().Where(e => e.Email == email).First();
            }
            }
        }
    }



