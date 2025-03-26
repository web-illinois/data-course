using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.DataContext {

    public class ProgramFieldItemMultipleDelete(IDbContextFactory<ProgramContext> factory) {
        private readonly IDbContextFactory<ProgramContext> _factory = factory;

        public async Task<int> DeleteAsync(CategoryType categoryType, int sourceId) {
            using var context = _factory.CreateDbContext();
            _ = await context.FieldSources.Where(fs => fs.CategoryType == categoryType && fs.SourceId == sourceId).ExecuteDeleteAsync();
            return context.SaveChanges();
        }
    }
}