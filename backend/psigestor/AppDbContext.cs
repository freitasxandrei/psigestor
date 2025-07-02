using Microsoft.EntityFrameworkCore;

namespace UserAuthApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

    public class Questionnaire
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ResponseDate { get; set; }
        public int TotalScore { get; set; }
        public string SufferingLevel { get; set; }
        public User User { get; set; }
    }

    public class Answer
    {
        public int Id { get; set; }
        public int QuestionnaireId { get; set; }
        public int QuestionNumber { get; set; }
        public bool AnswerValue { get; set; }
        public Questionnaire Questionnaire { get; set; }
    }
}
