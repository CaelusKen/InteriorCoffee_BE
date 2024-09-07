using AutoMapper;
using InteriorCoffee.Application.DTOs.Recaptcha;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class RecaptchaService : BaseService<RecaptchaService>, IRecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _recaptchaSecretKey;

        public RecaptchaService(ILogger<RecaptchaService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient, IConfiguration configuration) 
            : base(logger, mapper, httpContextAccessor)
        {
            _httpClient = httpClient;
            _recaptchaSecretKey = configuration["Recaptcha:SecretKey"];
        }

        public async Task<bool> VerifyRecaptchaAsync(string recaptchaToken)
        {
            var response = await _httpClient.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={_recaptchaSecretKey}&response={recaptchaToken}",
                null);

            if (!response.IsSuccessStatusCode) throw new Exception("Failed to verify reCAPTCHA with the external service.");

            var responseContent = await response.Content.ReadAsStringAsync();
            var recaptchaResponse = JsonSerializer.Deserialize<RecaptchaResponseDTO>(responseContent);

            return recaptchaResponse.success;
        }
    }
}
