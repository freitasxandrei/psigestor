using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using psigestor.Data;

[ApiController]
[Route("api/[controller]")]
public class QuestionnaireController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuestionnaireController(AppDbContext context)
    {
        _context = context;
    }

    // Endpoint para enviar respostas do questionário
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitQuestionnaire([FromBody] QuestionnaireResponse response)
    {
        if (response == null)
        {
            return BadRequest("Dados do questionário inválidos.");
        }

        // Verificando se todas as perguntas foram respondidas
        if (response.Question1 == 0 && response.Question2 == 0 && response.Question3 == 0 && response.Question4 == 0 &&
            response.Question5 == 0 && response.Question6 == 0 && response.Question7 == 0 && response.Question8 == 0 &&
            response.Question9 == 0 && response.Question10 == 0 && response.Question11 == 0 && response.Question12 == 0 &&
            response.Question13 == 0 && response.Question14 == 0 && response.Question15 == 0 && response.Question16 == 0 &&
            response.Question17 == 0 && response.Question18 == 0 && response.Question19 == 0 && response.Question20 == 0)
        {
            return BadRequest("Todas as perguntas devem ser respondidas.");
        }

        // Verificando se o usuário existe no banco de dados antes de salvar o questionário
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == response.Username); // Buscando pelo nome do usuário
        if (user == null)
        {
            return BadRequest("Usuário não encontrado.");
        }

        // Calculando o total de pontos
        response.TotalScore = response.Question1 + response.Question2 + response.Question3 + response.Question4 + response.Question5 +
                              response.Question6 + response.Question7 + response.Question8 + response.Question9 + response.Question10 +
                              response.Question11 + response.Question12 + response.Question13 + response.Question14 + response.Question15 +
                              response.Question16 + response.Question17 + response.Question18 + response.Question19 + response.Question20;

        // Definindo o nível de sofrimento (no backend)
        response.SufferingLevel = response.TotalScore >= 7 ? "Alto" : "Baixo";

        // Salvando os dados no banco de dados
        var questionnaire = new Questionnaire
        {
            UserId = user.Id, // Associando ao usuário encontrado pelo nome
            ResponseDate = DateTime.Now,
            TotalScore = response.TotalScore,
            SufferingLevel = response.SufferingLevel, // Atribuindo o valor calculado do SufferingLevel
            Question1 = response.Question1,
            Question2 = response.Question2,
            Question3 = response.Question3,
            Question4 = response.Question4,
            Question5 = response.Question5,
            Question6 = response.Question6,
            Question7 = response.Question7,
            Question8 = response.Question8,
            Question9 = response.Question9,
            Question10 = response.Question10,
            Question11 = response.Question11,
            Question12 = response.Question12,
            Question13 = response.Question13,
            Question14 = response.Question14,
            Question15 = response.Question15,
            Question16 = response.Question16,
            Question17 = response.Question17,
            Question18 = response.Question18,
            Question19 = response.Question19,
            Question20 = response.Question20
        };

        // Adicionando e salvando os dados no banco
        _context.Questionnaires.Add(questionnaire);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Respostas salvas com sucesso!" });
    }
}

// Modelo para as respostas do front-end
public class QuestionnaireResponse
{
    public string? Username { get; set; } // Agora recebendo o nome do usuário
    public DateTime Date { get; set; }

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

    public int TotalScore { get; set; }
    public string? SufferingLevel { get; set; }
}
