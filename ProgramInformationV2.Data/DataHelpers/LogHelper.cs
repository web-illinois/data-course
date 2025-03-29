using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Data.DataHelpers {

    public class LogHelper(ProgramRepository programRepository) {
        private readonly ProgramRepository _programRepository = programRepository;

        public async Task<IEnumerable<Log>> GetLog(string sourceName) {
            var date = DateTime.UtcNow.AddDays(-30);
            return (await _programRepository.ReadAsync(c => c.Logs.Include(l => l.Source).Where(l => l.Source != null &&
                l.Source.Code == sourceName && l.LastUpdated > date).OrderByDescending(s => s.LastUpdated))).ToList();
        }

        public async Task<bool> Log(CategoryType categoryType, FieldType fieldType, string netId, string sourceName, BaseObject data, string subject = "") {
            var sourceId = (await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == sourceName)))?.Id ?? 0;
            if (sourceId == 0) {
                return false;
            }
            var log = new Log {
                Title = data.Title,
                FieldType = fieldType,
                ChangedByNetId = netId,
                SubjectId = subject,
                CategoryType = categoryType,
                SourceId = sourceId,
                Data = data.ToString()
            };
            return (await _programRepository.CreateAsync(log)) > 0;
        }
    }
}