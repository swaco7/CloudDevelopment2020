using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ZodiacFuncApp {
    public static class Function1 {
        /*[FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }*/

        [FunctionName("GetSign")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log) {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!DateTime.TryParse(req.Query["dateOfBirth"], out DateTime dateOfBirth)) {
                return new BadRequestObjectResult("Please pass a dateOfBirth in the query string.");
            }

            ZodiacSign sign = GetSign(dateOfBirth);

            return new OkObjectResult($"Hello, your sign is {sign}");
        }

        private static ZodiacSign GetSign(DateTime dateOfBirth) {
            if (dateOfBirth.Day <= 20 && dateOfBirth.Month <= 1) {
                return ZodiacSign.Capricornus;
            }
            if (dateOfBirth.Day <= 20 && dateOfBirth.Month <= 2) {
                return ZodiacSign.Aquarius;
            }
            if (dateOfBirth.Day <= 20 && dateOfBirth.Month <= 3) {
                return ZodiacSign.Pisces;
            }
            if (dateOfBirth.Day <= 20 && dateOfBirth.Month <= 4) {
                return ZodiacSign.Aries;
            }
            if (dateOfBirth.Day <= 20 && dateOfBirth.Month <= 5) {
                return ZodiacSign.Taurus;
            }
            if (dateOfBirth.Day <= 21 && dateOfBirth.Month <= 6) {
                return ZodiacSign.Gemini;
            }
            if (dateOfBirth.Day <= 22 && dateOfBirth.Month <= 7) {
                return ZodiacSign.Cancer;
            }
            if (dateOfBirth.Day <= 22 && dateOfBirth.Month <= 8) {
                return ZodiacSign.Leo;
            }
            if (dateOfBirth.Day <= 22 && dateOfBirth.Month <= 9) {
                return ZodiacSign.Virgo;
            }
            if (dateOfBirth.Day <= 23 && dateOfBirth.Month <= 10) {
                return ZodiacSign.Libra;
            }
            if (dateOfBirth.Day <= 22 && dateOfBirth.Month <= 11) {
                return ZodiacSign.Scorpius;
            }
            if (dateOfBirth.Day <= 21 && dateOfBirth.Month <= 12) {
                return ZodiacSign.Sagittarius;
            }

            return ZodiacSign.Capricornus;
        }

        private enum ZodiacSign {
            Aries,
            Taurus,
            Gemini,
            Cancer,
            Leo,
            Virgo,
            Libra,
            Scorpius,
            Sagittarius,
            Capricornus,
            Aquarius,
            Pisces
        }
    }
}
