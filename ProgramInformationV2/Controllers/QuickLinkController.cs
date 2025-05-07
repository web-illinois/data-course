using Microsoft.AspNetCore.Mvc;
using ProgramInformationV2.Data.Cache;
using ProgramInformationV2.Data.DataHelpers;

namespace ProgramInformationV2.Controllers {

    [Route("[controller]")]
    public class QuickLinkController(CacheHolder cacheHolder, SecurityHelper securityHelper, SourceHelper sourceHelper) : Controller {
        private readonly CacheHolder _cacheHolder = cacheHolder;
        private readonly SecurityHelper _securityHelper = securityHelper;
        private readonly SourceHelper _sourceHelper = sourceHelper;

        [Route("course/{id}")]
        [HttpGet]
        public async Task<IActionResult> Course(string id) => await Set(id, "/course/general");

        [Route("credential/{id}")]
        [HttpGet]
        public async Task<IActionResult> Credential(string id) => await Set(id, "/credential/general");

        [Route("program/{id}")]
        [HttpGet]
        public async Task<IActionResult> Program(string id) => await Set(id, "/program/general");

        [Route("requirementset/{id}")]
        [HttpGet]
        public async Task<IActionResult> RequirementSet(string id) => await Set(id, "/requirementset/general");

        [Route("section/{id}")]
        [HttpGet]
        public async Task<IActionResult> Section(string id) => await Set(id, "/section/general");

        private async Task<IActionResult> Set(string id, string url) {
            if (string.IsNullOrEmpty(id)) {
                return Content("Error: ID needs to be added");
            }
            var netId = User.Identities.FirstOrDefault()?.Name;
            if (string.IsNullOrWhiteSpace(netId)) {
                return Content("Error: Net ID not found");
            }
            var sourceName = id.Split('-')[0];
            if (!await _securityHelper.ConfirmNetIdCanAccessSource(sourceName, netId)) {
                return Content($"Error: Net ID not allowed for source {sourceName} / {netId}");
            }
            var baseUrl = await _sourceHelper.GetBaseUrlFromSource(sourceName);
            _cacheHolder.SetCacheSource(netId, sourceName, baseUrl);
            _cacheHolder.SetCacheItem(netId, id);
            return Redirect(url);
        }
    }
}