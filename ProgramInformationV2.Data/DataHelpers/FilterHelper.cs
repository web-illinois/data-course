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
                List = [.. t.OrderBy(l => l.Order).Select(l => l.Title)],
            })];

        public async Task<string> GetFilterJsonForExport(string sourceCode) {
            var filters = await GetFilterListForExport(sourceCode);
            var source = await _programRepository.ReadAsync(c => c.Sources.Where(s => s.Code == sourceCode).FirstOrDefault());
            var sourceId = source?.Id ?? 0;
            if (source != null) {
                source.ApiSecretCurrent = "";
                source.ApiSecretPrevious = "";
                source.ApiSecretLastChanged = null;
                source.Code = "";
                source.Id = 0;
            }
            var instructions = await _programRepository.ReadAsync(c => c.FieldSources.Where(i => i.SourceId == sourceId).OrderBy(i => i.CategoryType).ThenBy(i => i.Title).ToList());
            return JsonSerializer.Serialize(new { source, filters, instructions });
        }

        public async Task<string> ImportFiltersFromJson(string source, string json) {
            var sourceEntity = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == source));
            if (sourceEntity == null) {
                return "Source not found.";
            }
            if (sourceEntity.UseCourses || sourceEntity.UseCredentials || sourceEntity.UsePrograms || sourceEntity.UseRequirementSets || sourceEntity.UseSections) {
                return "Source already being used.";
            }
            var returnObject = "";
            var jsonObject = JsonSerializer.Deserialize<dynamic>(json);
            Source sourceObject = JsonSerializer.Deserialize<Source>(jsonObject?.GetProperty("source"));
            List<FieldSource> instructions = JsonSerializer.Deserialize<List<FieldSource>>(jsonObject?.GetProperty("instructions"));
            List<TagList> importedTags = JsonSerializer.Deserialize<List<TagList>>(jsonObject?.GetProperty("filters"));
            if (sourceObject == null) {
                returnObject += "No source information loaded. ";
            } else {
                sourceEntity.BaseUrl = sourceObject?.BaseUrl ?? sourceEntity.BaseUrl;
                sourceEntity.UrlTemplate = sourceObject?.UrlTemplate ?? sourceEntity.UrlTemplate;
                sourceEntity.UseCourses = sourceObject?.UseCourses ?? sourceEntity.UseCourses;
                sourceEntity.UseCredentials = sourceObject?.UseCredentials ?? sourceEntity.UseCredentials;
                sourceEntity.UsePrograms = sourceObject?.UsePrograms ?? sourceEntity.UsePrograms;
                sourceEntity.UseRequirementSets = sourceObject?.UseRequirementSets ?? sourceEntity.UseRequirementSets;
                sourceEntity.UseSections = sourceObject?.UseSections ?? sourceEntity.UseSections;
                _ = await _programRepository.UpdateAsync(sourceEntity);
            }
            if (instructions == null) {
                returnObject += "No field instructions loaded. ";
            } else {
                foreach (var instruction in instructions) {
                    _ = await _programRepository.CreateAsync(new FieldSource {
                        SourceId = sourceEntity.Id,
                        CategoryType = instruction.CategoryType,
                        Description = instruction.Description,
                        ShowItem = instruction.ShowItem,
                        Title = instruction.Title
                    });
                }
                returnObject += $"{instructions.Count} instruction(s) loaded. ";
            }
            if (importedTags == null) {
                returnObject += "No tags loaded. ";
            } else {
                foreach (var tagList in importedTags) {
                    var count = 1;
                    var tagType = Enum.TryParse(tagList.Title, out TagType parsedTagType) ? parsedTagType : TagType.None;
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
                returnObject += $"{importedTags.SelectMany(it => it.List).Count()} tag(s) loaded. ";
            }
            return returnObject;
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