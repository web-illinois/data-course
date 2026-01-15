using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Search.Helpers;
using ProgramInformationV2.Search.JsonThinModels;

namespace ProgramInformationV2.Data.DataHelpers {

    public class FilterHelper(ProgramRepository? programRepository, BulkEditor? bulkEditor) {
        private readonly BulkEditor? _bulkEditor = bulkEditor;
        private readonly ProgramRepository _programRepository = programRepository ?? throw new ArgumentNullException("programRepository");

        public async Task<IEnumerable<IGrouping<TagType, TagSource>>> GetAllFilters(string source) =>
            await _programRepository.ReadAsync(c => c.TagSources.Include(c => c.Source).Where(ts => ts.Source != null && ts.Source.Code == source).OrderBy(ts => ts.Order).GroupBy(rv => rv.TagType));

        public async Task<List<TagList>> GetFilterListForExport(string source) => [.. (await GetAllFilters(source)).Select(t => new TagList {
                Title = t.Key.ToString(),
                List = [.. t.OrderBy(l => l.Order).Select(l => l.Title)],
            })];

        public async Task<string> GetFilterJsonForExport(string source) => JsonSerializer.Serialize(await GetFilterListForExport(source));

        public async Task<string> ImportFiltersFromJson(string source, string json) {
            var importedTags = JsonSerializer.Deserialize<List<TagList>>(json);
            var sourceEntity = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == source));
            if (sourceEntity == null || importedTags == null) {
                return "No tags loaded";
            }
            foreach (var tagList in importedTags) {
                var count = 1;
                var tagType = Enum.TryParse<TagType>(tagList.Title, out var parsedTagType) ? parsedTagType : TagType.None;
                if (tagType != TagType.None) {
                    foreach (var tag in tagList.List) {
                        var newTag = new TagSource {
                            SourceId = sourceEntity.Id,
                            TagType = tagType,
                            Title = tag,
                            Order = count++,
                        };
                        _ = await _programRepository.CreateAsync(newTag);
                    }
                }
            }
            return "Configuration loaded";
        }

        public async Task<(List<TagSource> TagSources, int SourceId)> GetFilters(string source, TagType tagType) {
            var returnValue = await _programRepository.ReadAsync(c => c.TagSources.Include(c => c.Source).Where(ts => ts.Source != null && ts.Source.Code == source && ts.TagType == tagType).OrderBy(ts => ts.Order).ToList());
            var sourceId = 0;
            foreach (var item in returnValue) {
                item.OldTitle = item.Title;
                sourceId = item.SourceId;
            }
            if (sourceId == 0) {
                sourceId = (await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == source)))?.Id ?? 0;
            }
            return (returnValue, sourceId);
        }

        public async Task<bool> SaveFilters(IEnumerable<TagSource> tags, IEnumerable<TagSource> tagsForDeletion, string sourceName) {
            var i = 1;
            foreach (var tag in tags) {
                tag.Order = i++;
                if (tag.Id == 0) {
                    _ = await _programRepository.CreateAsync(tag);
                } else {
                    _ = await _programRepository.UpdateAsync(tag);
                    if (tag.Title != tag.OldTitle && _bulkEditor != null) {
                        _ = await _bulkEditor.UpdateTags(sourceName, tag.TagTypeSourceName, tag.OldTitle, tag.Title);
                    }
                }
            }
            if (_bulkEditor != null) {
                foreach (var tag in tagsForDeletion) {
                    _ = await _programRepository.DeleteAsync(tag);
                    _ = await _bulkEditor.DeleteTags(sourceName, tag.TagTypeSourceName, tag.OldTitle);
                }
            }
            return true;
        }
    }
}