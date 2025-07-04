#nullable disable

using Microsoft.EntityFrameworkCore;

namespace psigestor.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Answer> Answers { get; set; } // Perguntas individuais
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

    // Tabela para armazenar as respostas do questionário
    public class Questionnaire
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Relacionado ao usuário
        public DateTime ResponseDate { get; set; }
        public int TotalScore { get; set; }  // Calcula a pontuação total
        public string SufferingLevel { get; set; }  // Nível de sofrimento (exemplo: "Alto", "Baixo")
        
        // Perguntas do questionário (Sim/Não)
        public int Question1 { get; set; }
        public int Question2 { get; set; }
        public int Question3 { get; set; }
        public int Question4 { get; set; }
        public int Question5 { get; set; }
        public int Question6 { get; set; }
        public int Question7 { get; set; }
        public int Question8 { get; set; }
        public int Question9 { get; set; }
        public int Question10 { get; set; }
        public int Question11 { get; set; }
        public int Question12 { get; set; }
        public int Question13 { get; set; }
        public int Question14 { get; set; }
        public int Question15 { get; set; }
        public int Question16 { get; set; }
        public int Question17 { get; set; }
        public int Question18 { get; set; }
        public int Question19 { get; set; }
        public int Question20 { get; set; }

        public User User { get; set; }  // Relacionamento com a tabela User
    }

    public class Answer
    {
        public int Id { get; set; }
        public int QuestionnaireId { get; set; }
        public int QuestionNumber { get; set; }
        public bool AnswerValue { get; set; }  // Resposta da pergunta (Sim ou Não)
        public Questionnaire Questionnaire { get; set; }  // Relacionamento com o questionário
    }
}
