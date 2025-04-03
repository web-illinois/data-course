using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Search.Helpers;

namespace ProgramInformationV2.Data.DataHelpers {

    public class FilterHelper(ProgramRepository? programRepository, BulkEditor? bulkEditor) {
        private readonly BulkEditor? _bulkEditor = bulkEditor;
        private readonly ProgramRepository _programRepository = programRepository ?? throw new ArgumentNullException("programRepository");

        public async Task<IEnumerable<IGrouping<TagType, TagSource>>> GetAllFilters(string source) =>
            await _programRepository.ReadAsync(c => c.TagSources.Include(c => c.Source).Where(ts => ts.Source != null && ts.Source.Code == source).OrderBy(ts => ts.Order).GroupBy(rv => rv.TagType));

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
                        await _bulkEditor.UpdateTags(sourceName, tag.TagTypeSourceName, tag.OldTitle, tag.Title);
                    }
                }
            }
            if (_bulkEditor != null) {
                foreach (var tag in tagsForDeletion) {
                    _ = await _programRepository.DeleteAsync(tag);
                    await _bulkEditor.DeleteTags(sourceName, tag.TagTypeSourceName, tag.OldTitle);
                }
            }
            return true;
        }
    }
}