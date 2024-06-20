using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OpenAIController : ControllerBase
{
    private readonly OpenAIService _openAIService;

    public OpenAIController(OpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    [HttpGet]
    public ActionResult<string> GetOpenAIResponse(string input)
    {
        var response = _openAIService.GetOpenAIResponse(input);
        return Ok(response);
    }
}
