using System.Text.Json;
using google_recaptcha.Models;

namespace google_recaptcha;

public interface IReCaptchaService
{
    Task<GoogleReCaptchaResponse> VerifyAsync(string captchaResponse);
}

public class ReCaptchaService : IReCaptchaService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ReCaptchaService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<GoogleReCaptchaResponse> VerifyAsync(string captchaResponse)
    {
        var secretKey = _configuration["GoogleReCaptcha:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("Secret key is missing in configuration");

        var postData = new FormUrlEncodedContent(new []
        {
            new KeyValuePair<string, string>("secret", secretKey),
            new KeyValuePair<string, string>("response", captchaResponse)
        });
        
        var apiUrl = $"https://www.google.com/recaptcha/api/siteverify";

        var httpResponse =
            await _httpClient.PostAsync(apiUrl, postData);
        httpResponse.EnsureSuccessStatusCode();

        var responseString = await httpResponse.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GoogleReCaptchaResponse>(responseString);
    }
}