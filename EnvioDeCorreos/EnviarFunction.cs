using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EnvioDeCorreos.Models;
using EnvioDeCorreos.Models.Response;

namespace EnvioDeCorreos
{
    public static class EnviarFunction
    {
        [FunctionName("EnviarFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            bool result = false;
            Response response = new Response();
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();                
                if (!string.IsNullOrEmpty(requestBody))
                {                    
                    Email email = JsonConvert.DeserializeObject<Email>(requestBody);                    
                    if (result)
                    {
                        response.exitoso = result;
                        response.mensaje = "Se envió el email";
                        return (ActionResult)new OkObjectResult(response);
                    }
                    else
                    {
                        response.exitoso = result;
                        response.mensaje = "SNo se pudo enviar el email";
                        return (ActionResult)new OkObjectResult(response);
                    }
                }
                else
                {
                    response.exitoso = result;
                    response.mensaje = "Por favor ingrese información en el body";
                    return (ActionResult)new BadRequestObjectResult(response);
                }
            }
            catch(Exception ex)
            {
                response.exitoso = result;
                response.mensaje = ex.Message;
                return (ActionResult)new BadRequestObjectResult(response);
            }          
        }
    }
}
