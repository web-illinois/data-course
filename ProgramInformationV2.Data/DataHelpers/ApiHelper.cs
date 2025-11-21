using System.Security.Cryptography;
using System.Text;
using ProgramInformationV2.Data.DataContext;

namespace ProgramInformationV2.Data.DataHelpers {

    public class ApiHelper(ProgramRepository programRepository) {
        private readonly ProgramRepository _programRepository = programRepository;

        public async Task<string> AdvanceApi(string source) {
            var sourceItem = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == source));
            if (sourceItem == null) {
                return "";
            }
            var guid = Guid.NewGuid().ToString().ToLowerInvariant();
            var guidHash = HashWithSHA256(guid);
            sourceItem.ApiSecretPrevious = sourceItem.ApiSecretCurrent;
            sourceItem.ApiSecretCurrent = guidHash;
            sourceItem.ApiSecretLastChanged = DateTime.Now;
            return await _programRepository.UpdateAsync(sourceItem) > 0 ? guid : "";
        }

        public async Task<bool> CheckApi(string source, string key) {
            var guidHash = HashWithSHA256(key);
            var sourceItem = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == source));
            if (sourceItem == null || string.IsNullOrWhiteSpace(sourceItem.ApiSecretCurrent)) {
                return false;
            }
            return guidHash.Equals(sourceItem.ApiSecretCurrent, StringComparison.OrdinalIgnoreCase) || guidHash.Equals(sourceItem.ApiSecretPrevious, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<(bool isValid, DateTime lastChanged)> GetApi(string source) {
            var sourceItem = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == source));
            return (sourceItem == null || sourceItem.ApiSecretCurrent == "") ? (false, DateTime.MinValue) : (true, sourceItem.ApiSecretLastChanged ?? DateTime.MinValue);
        }

        public async Task<int> InvalidateApi(string source) {
            var sourceItem = await _programRepository.ReadAsync(c => c.Sources.FirstOrDefault(s => s.Code == source));
            if (sourceItem == null) {
                return 0;
            }
            sourceItem.ApiSecretPrevious = "";
            sourceItem.ApiSecretCurrent = "";
            sourceItem.ApiSecretLastChanged = DateTime.Now;
            return await _programRepository.UpdateAsync(sourceItem);
        }

        private static string HashWithSHA256(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value + "-Entering-Seismic-Passivism-Critter")));
    }
}