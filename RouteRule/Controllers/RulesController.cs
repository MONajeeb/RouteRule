using System.Net;
using Microsoft.AspNetCore.Mvc;
using RouteRule.Bl.RuleOperations;
using RouteRule.Models;

namespace RouteRule.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class RulesController : ControllerBase
    {
       
        private readonly ILogger<RulesController> _logger;
        private readonly IRuleRepository _ruleRepository;

        public RulesController(ILogger<RulesController> logger,IRuleRepository ruleRepository)
        {
            _logger = logger;
            _ruleRepository = ruleRepository;
        }
        
        [HttpGet(Name = "GetAllRules")]
        [ProducesResponseType(typeof(IList<Rule>),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Enumerable),(int)HttpStatusCode.NoContent)]
        public async Task<IList<Rule>> Get()
        {
            return await _ruleRepository.GetAllRules();
        }
        
        [HttpPost(Name = "AddRule")]
        [ProducesResponseType(typeof(Response),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add([FromBody] Rule rule)
        {
            var addingStatus = await _ruleRepository.AddRule(rule);
            if (addingStatus == FileStatus.AppendDone)
            {
                return new OkObjectResult(new Response(Status.Success,"Adding new rule done successfully"));
            }
            return new OkObjectResult(new Response(Status.Error, addingStatus == FileStatus.Error ? "Error while adding new rule" : "The same rule is already exist"));
        }


    }
}