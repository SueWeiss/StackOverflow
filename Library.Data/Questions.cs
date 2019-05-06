using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Library.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }
        public List<QuestionTags> QuestionTags { get; set; }
        public int Likes { get; set; }
    }
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }
        public string Name { get; set; }
        public int QuestionId { get; set; }
    }
    public class User
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Id { get; set; }
    }
    public class Likes
    {
        public int UserId { get; set; }
        public int QuestionId { get; set; }
    }
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionTags> QuestionTags { get; set; }
    }
    public class QuestionTags
    {
        public int QuestionId { get; set; }
        public int TagId { get; set; }
        public Question Question { get; set; }
        public Tag Tag { get; set; }

    }
    public class QuestionsContext : DbContext
    {
        private string _connectionString;
        public QuestionsContext(string connectionString)
        {
            _connectionString = connectionString;
        }
       public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionTags> QuestionTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Taken from here:
            //https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration

            //set up composite primary key
            modelBuilder.Entity<QuestionTags>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

            //set up foreign key from QuestionsTags to Questions
            modelBuilder.Entity<QuestionTags>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionTags)
                .HasForeignKey(q => q.QuestionId);

            //set up foreign key from QuestionsTags to Tags
            modelBuilder.Entity<QuestionTags>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuestionTags)
                .HasForeignKey(q => q.TagId);
        }
    }

    public class PeopleContextFactory : IDesignTimeDbContextFactory<QuestionsContext>
    {
        public QuestionsContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}5_01StackO"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new QuestionsContext(config.GetConnectionString("ConStr"));
        }
    }
}
