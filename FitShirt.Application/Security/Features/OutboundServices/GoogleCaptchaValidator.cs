// Application/Security/OutboundServices/GoogleCaptchaValidator.cs
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using FitShirt.Domain.Security.Services;

namespace FitShirt.Application.Security.Features.OutboundServices
{
    public class GoogleCaptchaValidator : IGoogleCaptchaValidator
    {
        private readonly HttpClient _httpClient;
        private readonly string? _secretKey;

        public GoogleCaptchaValidator(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _secretKey = configuration["GoogleCaptcha:SecretKey"];
        }

        public async Task<bool> ValidateAsync(string captchaResponse)
        {
            if (string.IsNullOrEmpty(captchaResponse))
            {
                return false;
            }

            var googleUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={_secretKey}&response={captchaResponse}";
            var response = await _httpClient.GetStringAsync(googleUrl);
            Console.WriteLine(response);
            var captchaVerificationResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(response);
            
            return captchaVerificationResult != null && captchaVerificationResult.Success;
        }
    }

    public class GoogleCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTimestamp { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}