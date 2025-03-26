using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.FieldList {

    public class FieldManager(ProgramRepository programRepository, ProgramFieldItemMultipleDelete programFieldItemMultipleDelete) {
        private readonly ProgramFieldItemMultipleDelete _programFieldItemMultipleDelete = programFieldItemMultipleDelete;
        private readonly ProgramRepository _programRepository = programRepository;

        public async Task<IEnumerable<FieldItem>> GetMergedFieldItems(string sourceCode, BaseGroup baseGroup, FieldType fieldType) {
            var source = await _programRepository.ReadAsync(pr => pr.Sources.First(fs => fs.Code == sourceCode));
            var items = (await _programRepository.ReadAsync(pr => pr.FieldSources.Where(fs => fs.SourceId == source.Id && fs.CategoryType == baseGroup.CategoryType && fs.IsActive))).ToList();
            foreach (var item in baseGroup.FieldItems.Where(f => f.FieldType == fieldType)) {
                var savedItem = items.FirstOrDefault(a => a.Title == item.Title);
                if (savedItem != null) {
                    item.Description = savedItem.Description;
                    item.ShowItem = savedItem.ShowItem;
                }
            }

            return baseGroup.FieldItems.Where(f => f.FieldType == fieldType);
        }

        public async Task<(bool IsUsed, IEnumerable<IGrouping<FieldType, FieldItem>> fieldItems)> MergeFieldItems(BaseGroup baseGroup, string sourceCode) {
            var source = await _programRepository.ReadAsync(pr => pr.Sources.First(fs => fs.Code == sourceCode));
            var items = (await _programRepository.ReadAsync(pr => pr.FieldSources.Where(fs => fs.SourceId == source.Id))).ToList();

            var isUsed = false;
            switch (baseGroup.CategoryType) {
                case CategoryType.Program:
                    isUsed = source.UsePrograms;
                    break;

                case CategoryType.Credential:
                    isUsed = source.UseCredentials;
                    break;

                case CategoryType.Course:
                    isUsed = source.UseCourses;
                    break;

                case CategoryType.Section:
                    isUsed = source.UseSections;
                    break;

                case CategoryType.RequirementSet:
                    isUsed = source.UseRequirementSets;
                    break;

                case CategoryType.None:
                default:
                    break;
            }

            foreach (var item in baseGroup.FieldItems) {
                var savedItem = items.FirstOrDefault(a => a.Title == item.Title);
                if (savedItem != null) {
                    item.Description = savedItem.Description;
                    item.ShowItem = savedItem.ShowItem;
                }
            }

            return (isUsed, baseGroup.FieldItems.GroupBy(f => f.FieldType));
        }

        public async Task SaveFieldItems(string sourceCode, CategoryType categoryType, bool isUsed, IEnumerable<FieldItem> fieldItems) {
            var source = await _programRepository.ReadAsync(pr => pr.Sources.First(fs => fs.Code == sourceCode));

            switch (categoryType) {
                case CategoryType.Program:
                    source.UsePrograms = isUsed;
                    break;

                case CategoryType.Credential:
                    source.UseCredentials = isUsed;
                    break;

                case CategoryType.Course:
                    source.UseCourses = isUsed;
                    break;

                case CategoryType.Section:
                    source.UseSections = isUsed;
                    break;

                case CategoryType.RequirementSet:
                    source.UseRequirementSets = isUsed;
                    break;

                case CategoryType.None:
                default:
                    break;
            }

            _ = await _programRepository.UpdateAsync(source);
            _ = await _programFieldItemMultipleDelete.DeleteAsync(categoryType, source.Id);

            foreach (var fieldItem in fieldItems.Where(f => !f.IsDefault)) {
                _ = await _programRepository.CreateAsync(fieldItem.Translate(source.Id));
            }
        }
    }
}