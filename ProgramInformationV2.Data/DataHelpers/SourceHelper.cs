using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.DataHelpers {

    public class SourceHelper(ProgramRepository programRepository) {
        private readonly ProgramRepository _programRepository = programRepository;

        public async Task<string> CreateSource(string newSourceCode, string newTitle, string email) {
            var source = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == newSourceCode.ToLowerInvariant() || s.Title == newTitle));
            if (source != null) {
                return "Source code or name is in use";
            }
            _ = await _programRepository.CreateAsync(new Source { Code = newSourceCode.ToLowerInvariant(), CreatedByEmail = email, IsActive = true, IsTest = false, Title = newTitle });
            var newSource = await _programRepository.ReadAsync(pr => pr.Sources.FirstOrDefault(s => s.Code == newSourceCode.ToLowerInvariant()));
            if (newSource != null) {
                _ = await _programRepository.CreateAsync(new SecurityEntry { SourceId = newSource.Id, IsActive = true, IsFullAdmin = true, IsOwner = true, IsPublic = true, IsRequested = false, Email = email });
            }
            return $"Added source {newTitle} with code {newSourceCode}";
        }

        public async Task<bool> DoesSourceUseItem(string sourceCode, CategoryType categoryType) {
            var source = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == sourceCode));
            if (source == null) {
                return false;
            }
            switch (categoryType) {
                case CategoryType.Program:
                    return source.UsePrograms;

                case CategoryType.Credential:
                    return source.UseCredentials;

                case CategoryType.Course:
                    return source.UseCourses;

                case CategoryType.Section:
                    return source.UseSections;

                case CategoryType.RequirementSet:
                    return source.UseRequirementSets;
            }
            return false;
        }

        public async Task<string> GetBaseUrlFromSource(string sourceCode) {
            var source = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == sourceCode.ToLowerInvariant()));
            return source?.BaseUrl ?? "";
        }

        public async Task<Dictionary<string, string>> GetSources(string netId) => await _programRepository.ReadAsync(c => c.SecurityEntries.Include(se => se.Source).Where(se => se.IsActive && !se.IsRequested && se.Email == netId).ToDictionary(se => se.Source?.Code ?? "", se2 => se2.Source?.Title ?? ""));

        public async Task<IEnumerable<Tuple<string, string>>> GetSourcesAndOwners() => await _programRepository.ReadAsync(c => c.Sources.Where(s => s.IsActive).OrderBy(s => s.Title).Select(s => new Tuple<string, string>(s.CreatedByEmail, $"{s.Title} ({s.Code})")));

        public async Task<string> GetUrlTemplateFromSource(string sourceCode) {
            var source = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == sourceCode.ToLowerInvariant()));
            return source?.UrlTemplate ?? "";
        }

        public async Task<string> RequestAccess(string sourceCode, string email) {
            var source = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == sourceCode));
            if (source == null) {
                return "Source Code not found";
            }

            var existingItem = await _programRepository.ReadAsync(c => c.SecurityEntries.FirstOrDefault(s => s.SourceId == source.Id && s.Email == email));
            if (existingItem != null) {
                if (existingItem.IsActive) {
                    return "You already have access";
                } else if (existingItem.IsRequested) {
                    return "You entry is pending";
                } else {
                    return "You entry has been rejected -- please contact the owner for more information";
                }
            }

            var value = await _programRepository.CreateAsync(new SecurityEntry(email, source.Id, true));
            return $"Requested access to code {sourceCode}";
        }

        public async Task<string> RequestDeletion(string sourceCode, string email) {
            var source = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == sourceCode));
            if (source == null) {
                return "Source Code not found";
            }
            if (source.RequestDeletion) {
                return "Source was already requested to be deleted";
            }
            if (source.IsTest) {
                return "Test source cannot be deleted";
            }
            source.RequestDeletion = true;
            source.RequestDeletionByEmail = email;
            source.LastUpdated = DateTime.Now;
            var value = await _programRepository.UpdateAsync(source);
            return $"Code {sourceCode} has been marked for deletion";
        }

        public async Task<bool> SaveBaseUrl(string sourceCode, string baseUrl) {
            var source = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == sourceCode.ToLowerInvariant()));
            if (source == null) {
                return false;
            }
            source.BaseUrl = baseUrl;
            var value = await _programRepository.UpdateAsync(source);
            return true;
        }

        public async Task<bool> SaveUrlTemplate(string sourceCode, string urlTemplate) {
            var source = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == sourceCode.ToLowerInvariant()));
            if (source == null) {
                return false;
            }
            source.UrlTemplate = urlTemplate;
            var value = await _programRepository.UpdateAsync(source);
            return true;
        }
    }
}