using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IRecaptchaService
    {
        public Task<bool> VerifyRecaptchaAsync(string recaptchaToken);
    }
}
