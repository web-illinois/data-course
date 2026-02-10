using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.DataContext {

    public class ProgramRepository(IDbContextFactory<ProgramContext> factory) {
        private readonly IDbContextFactory<ProgramContext> _factory = factory;

        public int Create<T>(T item) where T : BaseDataItem {
            using var context = _factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            _ = context.Add(item);
            return context.SaveChanges();
        }

        public async Task<int> CreateAsync<T>(T item) where T : BaseDataItem {
            using var context = _factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            _ = context.Add(item);
            return await context.SaveChangesAsync();
        }

        public int Delete<T>(T item) {
            if (item == null) {
                return 0;
            }
            using var context = _factory.CreateDbContext();
            _ = context.Remove(item);
            return context.SaveChanges();
        }

        public async Task<int> DeleteAsync<T>(T item) {
            if (item == null) {
                return 0;
            }
            using var context = _factory.CreateDbContext();
            _ = context.Remove(item);
            return await context.SaveChangesAsync();
        }

        public T Read<T>(Func<ProgramContext, T> work) {
            var context = _factory.CreateDbContext();
            return work(context);
        }

        public async Task<T> ReadAsync<T>(Func<ProgramContext, T> work) {
            var context = _factory.CreateDbContext();
            return await Task.Run(() => work(context));
        }

        public int Update<T>(T item) where T : BaseDataItem {
            using var context = _factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            _ = context.Update(item);
            return context.SaveChanges();
        }

        public int UpdateActive<T>(T item, bool active) where T : BaseDataItem {
            using var context = _factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            item.IsActive = active;
            _ = context.Update(item);
            return context.SaveChanges();
        }

        public async Task<int> UpdateActiveAsync<T>(T item, bool active) where T : BaseDataItem {
            using var context = _factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            item.IsActive = active;
            _ = context.Update(item);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync<T>(T item) where T : BaseDataItem {
            using var context = _factory.CreateDbContext();
            item.LastUpdated = DateTime.Now;
            _ = context.Update(item);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteSource(int sourceId) {
            using var context = _factory.CreateDbContext();
            context.Database.ExecuteSqlInterpolated($"DELETE FROM dbo.FieldSources WHERE SourceId = {sourceId}");
            context.Database.ExecuteSqlInterpolated($"DELETE FROM dbo.Logs WHERE SourceId = {sourceId}");
            context.Database.ExecuteSqlInterpolated($"DELETE FROM dbo.SecurityEntries WHERE SourceId = {sourceId}");
            context.Database.ExecuteSqlInterpolated($"DELETE FROM dbo.TagSources WHERE SourceId = {sourceId}");
            return context.Database.ExecuteSqlInterpolated($"DELETE FROM dbo.Sources WHERE Id = {sourceId}");
        }
    }
}