using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.DataHelpers {

    public class SecurityHelper(ProgramRepository programRepository) {
        private readonly ProgramRepository _programRepository = programRepository;

        public async Task<string> AddName(string sourceName, string netId) {
            var sourceId = (await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == sourceName)))?.Id ?? 0;
            var isSuccessful = sourceId == 0
                ? false
                : await _programRepository.CreateAsync(new SecurityEntry {
                    IsActive = true,
                    IsFullAdmin = false,
                    IsOwner = false,
                    IsPublic = false,
                    SourceId = sourceId,
                    Email = UpdateNetId(netId),
                }) > 0;
            return isSuccessful ? UpdateNetId(netId) : "";
        }

        public async Task<bool> ConfirmNetIdCanAccessSource(string sourceName, string netId) {
            return await _programRepository.ReadAsync(c => c.SecurityEntries.Include(c => c.Source).Any(se => se.Source != null && se.Source.Code == sourceName && se.Email == netId));
        }

        public async Task<List<SecurityEntry>> GetNames(string sourceName) => await _programRepository.ReadAsync(c => c.SecurityEntries.Include(c => c.Source).Where(se => se.Source != null && se.Source.Code == sourceName).OrderBy(se => se.Email).ToList());

        public async Task<(bool, string[])> GetRestrictions(string netId, string source) {
            var item = await _programRepository.ReadAsync(c => c.SecurityEntries.Include(c => c.Source).FirstOrDefault(c => c.Email == netId && c.Source != null && c.Source.Code == source));
            return item == null ? (true, []) : (item.IsRestricted, item.RestrictedIds.Split(";"));
        }

        public async Task<bool> RemoveName(string sourceName, string netId) {
            var deletedName = await _programRepository.ReadAsync(c => c.SecurityEntries.Include(c => c.Source).FirstOrDefault(se => se.Source != null && se.Source.Code == sourceName && se.Email == netId));
            return deletedName == null
                ? false
                : await _programRepository.DeleteAsync(deletedName) > 0;
        }

        public async Task<bool> UpdateName(SecurityEntry securityEntry) {
            var item = await _programRepository.UpdateAsync(securityEntry);
            return item > 0;
        }

        private string UpdateNetId(string netId) => $"{netId.Replace("@illinois.edu", "").ToLowerInvariant()}@illinois.edu";
    }
}