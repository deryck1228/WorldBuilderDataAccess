using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        [Function("GetAllCharacters")]
        public async Task<HttpResponseData> GetAllCharacters(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "characters")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetAllCharacters");
            logger.LogInformation("Getting all characters");

            var queryParameters = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int.TryParse(queryParameters["page"], out var page);
            int.TryParse(queryParameters["pageSize"], out var pageSize);

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            int skip = (page - 1) * pageSize;

            var characters = _context.Characters
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(characters);
            return response;
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

        [Function("GetTotalCharacterCount")]
        public async Task<HttpResponseData> GetTotalCharacterCount(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "characters/count")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetTotalCharacterCount");
            logger.LogInformation("Getting total character count");

            var count = _context.Characters.Count();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(count);
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

        [Function("UpdateCharacter")]
        public async Task<HttpResponseData> UpdateCharacter(
                    [HttpTrigger(AuthorizationLevel.Function, "put", Route = "character/{id}")] HttpRequestData req,
                    int id, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("UpdateCharacter");
            logger.LogInformation("Updating character");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedData = JObject.Parse(requestBody);

            var response = req.CreateResponse();

            var existingCharacter = await _context.Characters.FindAsync(id);
            if (existingCharacter == null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return response;
            }

            // Use reflection to update properties dynamically, excluding the primary key (CharacterId)
            foreach (var property in typeof(Character).GetProperties())
            {
                if (property.Name == nameof(Character.CharacterId))
                {
                    continue; // Skip the primary key property
                }

                if (updatedData.TryGetValue(property.Name, out var value))
                {
                    var typedValue = value.ToObject(property.PropertyType);
                    property.SetValue(existingCharacter, typedValue);
                }
            }

            _context.Characters.Update(existingCharacter);
            await _context.SaveChangesAsync();

            response.StatusCode = System.Net.HttpStatusCode.OK;
            await response.WriteAsJsonAsync(existingCharacter);
            return response;
        }
    }
}
