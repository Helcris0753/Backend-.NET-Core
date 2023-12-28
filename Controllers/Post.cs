using Autohorario;
using Backend.DBInteractions;
using Backend.GetInformationForINTEC;
using Backend.Models.Alerts;
using Backend.Models.Query_Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("Post")]
    [ApiController]
    public class Post : ControllerBase
    {

        [HttpPost]
        // [Authorize(Roles = "Administrator")]
        [Route("ExecuteSortingAlgorithm")]
        public IActionResult ExecuteSortingAlgorithm()
        {
            // Creación de un objeto ReturnMessage
            try
            {
                //Si el algoritmo se ejecuta sin error, se devuelve una respuesta ok con el tiempo de ejecucion que duro.
                return Ok("Time Elapsed: " + Obtaining.Run() + " seconds");
            }
            catch (Exception e)
            {
                //En caso contrario, devuelve un statuscode con un error 300 y el tipo de exception
                return StatusCode(300, "Error durint Sorting Algoritm. Exception type: " + e);
            }

        }
        [HttpPost]
        // [Authorize(Roles = "Administrator")]
        [Route("GetInformacionINTEC")]
        public async Task<IActionResult> GetInformacionINTEC()
        {
            //Se verifica que todo el proceso para tomar la informacion del intec y de insertarla en nuestra base de datos 
            //si es true, devuelve un respuesta positiva, sino, nun statuscode con error 300 y una banner
            return await GetDataIntec.GetDataForNTEC() ?
                Ok("Data correctly insert") : //
                StatusCode(300, "Intec Api Dawn or insert process is not working well");
        }
    }
}
