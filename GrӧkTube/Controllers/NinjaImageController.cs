using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

[ApiController]
[Route("Api/Images")]
public class NinjaImageController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _apiKey;

    public NinjaImageController(
        IHttpClientFactory clientFactory,
        IConfiguration config)
    {
        _clientFactory = clientFactory;
        _apiKey = config["ApiNinjas:Key"]; 
    }

    [HttpGet("Random")]
    public async Task<IActionResult> GetRandomImage()
    {
        var client = _clientFactory.CreateClient();

       
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("image/jpg"));

        client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);

        try
        {
            var response = await client.GetAsync(
                "https://api.api-ninjas.com/v1/randomimage");

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            return File(stream, "image/jpeg");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(
                (int)(ex.StatusCode ?? System.Net.HttpStatusCode.InternalServerError),
                ex.Message);
        }
    }
}