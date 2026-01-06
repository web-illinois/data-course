using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Search.Helpers;
using ProgramInformationV2.Search.JsonThinModels;
using System.Text.Json;

namespace ProgramInformationV2.Data.DataHelpers {

    public class FilterHelper(ProgramRepository? programRepository, BulkEditor? bulkEditor) {
        private readonly BulkEditor? _bulkEditor = bulkEditor;
        private readonly ProgramRepository _programRepository = programRepository ?? throw new ArgumentNullException("programRepository");

        public async Task<IEnumerable<IGrouping<TagType, TagSource>>> GetAllFilters(string source) =>
            await _programRepository.ReadAsync(c => c.TagSources.Include(c => c.Source).Where(ts => ts.Source != null && ts.Source.Code == source).OrderBy(ts => ts.Order).GroupBy(rv => rv.TagType));

        public async Task<List<TagList>> GetFilterListForExport(string source) => [.. (await GetAllFilters(source)).Select(t => new TagList {
                Title = t.Key.ToString(),
                List = [.. t.Select(l => l.Title)],
            })];

        public async Task<string> GetFilterJsonForExport(string source) => JsonSerializer.Serialize(await GetFilterListForExport(source));

        public async Task<string> ImportFiltersFromJson(string source, string json) {
            List<TagList>? importedTags = JsonSerializer.Deserialize<List<TagList>>(json);
            Source? sourceEntity = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == source));
            if (sourceEntity == null || importedTags == null) {
                return "No tags loaded";
            }
            foreach (TagList tagList in importedTags) {
                var count = 1;
                TagType tagType = Enum.TryParse<TagType>(tagList.Title, out TagType parsedTagType) ? parsedTagType : TagType.None;
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
            List<TagSource> returnValue = await _programRepository.ReadAsync(c => c.TagSources.Include(c => c.Source).Where(ts => ts.Source != null && ts.Source.Code == source && ts.TagType == tagType).OrderBy(ts => ts.Order).ToList());
            var sourceId = 0;
            foreach (TagSource? item in returnValue) {
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
            foreach (TagSource tag in tags) {
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
                foreach (TagSource tag in tagsForDeletion) {
                    _ = await _programRepository.DeleteAsync(tag);
                    _ = await _bulkEditor.DeleteTags(sourceName, tag.TagTypeSourceName, tag.OldTitle);
                }
            }
            return true;
        }
    }
}