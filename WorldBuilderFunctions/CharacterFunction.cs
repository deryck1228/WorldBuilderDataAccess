using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WorldBuilderDataAccessLib;
using WorldBuilderDataAccessLib.Models;

namespace WorldBuilderFunctions
{
    public class CharacterFunction
    {
        private readonly CampaignContext _context;

        public CharacterFunction(CampaignContext context)
        {
            _context = context;
        }

        [Function("GetCharacter")]
        public async Task<HttpResponseData> GetCharacter(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "character/{id}")] HttpRequestData req,
            int id, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetCharacter");
            logger.LogInformation("Retrieving character");

            var character = await _context.Characters.FindAsync(id);
            var response = req.CreateResponse();
            if (character == null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return response;
            }
            response.StatusCode = System.Net.HttpStatusCode.OK;
            await response.WriteAsJsonAsync(character);
            return response;
        }

        [Function("CreateCharacter")]
        public async Task<HttpResponseData> CreateCharacter(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "character")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("CreateCharacter");
            logger.LogInformation("Creating a new character");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var character = JsonConvert.DeserializeObject<Character>(requestBody);

            var response = req.CreateResponse();
            if (character == null)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                await response.WriteStringAsync("Invalid character data");
                return response;
            }

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            response.StatusCode = System.Net.HttpStatusCode.OK;
            await response.WriteAsJsonAsync(character);
            return response;
        }
    }
}
