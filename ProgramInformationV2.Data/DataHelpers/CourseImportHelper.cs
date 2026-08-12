using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.DataHelpers {

    public class CourseImportHelper(ProgramRepository programRepository) {
        private readonly ProgramRepository _programRepository = programRepository;

        public async Task<DateTime?> GetLastItemUpdated() {
            return await _programRepository.ReadAsync(pr => pr.CourseImportEntries.Where(c => c.DateImported != null).OrderByDescending(c => c.DateImported).FirstOrDefault()?.DateImported);
        }

        public async Task<CourseImportEntry?> GetLatestPending() {
            return await _programRepository.ReadAsync(pr => pr.CourseImportEntries.OrderBy(c => c.LastUpdated).FirstOrDefault(c => c.DateImported == null));
        }

        public async Task<IEnumerable<CourseImportEntry>> GetLog(string sourceCode, string rubric, int importType) {
            var sourceId = _programRepository.Read(c => c.Sources.FirstOrDefault(s => s.Code == sourceCode))?.Id ?? 0;
            if (importType == 1) {
                return await _programRepository.ReadAsync(pr => pr.CourseImportEntries.Where(c => c.SourceId == sourceId && (rubric == "" || c.Rubric == rubric) && c.DateImported == null).OrderBy(c => c.Rubric).ThenBy(c => c.CourseNumber));
            } else if (importType == 2) {
                return await _programRepository.ReadAsync(pr => pr.CourseImportEntries.Where(c => c.SourceId == sourceId && (rubric == "" || c.Rubric == rubric) && c.DateImported != null).OrderByDescending(c => c.DateImported).ThenBy(c => c.Rubric).ThenBy(c => c.CourseNumber));
            } else if (importType == 3) {
                return await _programRepository.ReadAsync(pr => pr.CourseImportEntries.Where(c => c.SourceId == sourceId && (rubric == "" || c.Rubric == rubric) && c.DateImported != null && c.Log.Contains("not found in system")).OrderByDescending(c => c.DateImported).ThenBy(c => c.Rubric).ThenBy(c => c.CourseNumber));
            }
            return await _programRepository.ReadAsync(pr => pr.CourseImportEntries.Where(c => c.SourceId == sourceId && (rubric == "" || c.Rubric == rubric)).OrderByDescending(c => c.DateImported).ThenBy(c => c.Rubric).ThenBy(c => c.CourseNumber));
        }

        public async Task<int> LoadComplete(string rubric, string courseNumber, string urlPattern, bool importTitleAndDescriptionOnly, bool includeSections, string sourceCode, string log) {
            var sourceId = _programRepository.Read(c => c.Sources.FirstOrDefault(s => s.Code == sourceCode))?.Id ?? 0;
            return await _programRepository.CreateAsync(new CourseImportEntry {
                Rubric = rubric,
                IsActive = true,
                CourseNumber = courseNumber,
                UrlPattern = urlPattern,
                IncludeTitleAndDescriptionOnly = importTitleAndDescriptionOnly,
                IncludeSections = includeSections,
                SourceId = sourceId,
                LastUpdated = DateTime.UtcNow,
                DateImported = DateTime.UtcNow,
                Log = log
            });
        }

        public async Task<int> LoadPending(string rubric, string courseNumber, string sourceCode) {
            var sourceId = _programRepository.Read(c => c.Sources.FirstOrDefault(s => s.Code == sourceCode))?.Id ?? 0;
            return await _programRepository.CreateAsync(new CourseImportEntry {
                Rubric = rubric,
                IsActive = true,
                CourseNumber = courseNumber,
                UrlPattern = "",
                IncludeTitleAndDescriptionOnly = false,
                IncludeSections = true,
                SourceId = sourceId,
                LastUpdated = DateTime.UtcNow,
                DateImported = null,
                Log = sourceCode
            });
        }

        public async Task<int> NumberItemsPending() {
            return await _programRepository.ReadAsync(pr => pr.CourseImportEntries.Count(c => c.DateImported == null));
        }

        public async Task<int> UpdatePending(CourseImportEntry entry, string log) {
            if (entry != null) {
                entry.Log = log;
                entry.DateImported = DateTime.UtcNow;
                return await _programRepository.UpdateAsync(entry);
            } else {
                return 0;
            }
        }
    }
}