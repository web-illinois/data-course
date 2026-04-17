using ProgramInformationV2.Data.Uploads;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Data.Versioning {
    public class VersionManager(UploadStorage uploadStorage, ProgramSetter programSetter) {
        private readonly UploadStorage _uploadStorage = uploadStorage;
        private readonly ProgramSetter _programSetter = programSetter;

        public async Task<Program> CopyProgramFromProduction(Program program) {
            var productionSource = program.Source.Trim('!') + "-";
            var testSource = productionSource + "!-";
            program.ConvertToTest();
            program.SummaryImageUrl = await _uploadStorage.CopyImage(program?.SummaryImageUrl, program?.Source);
            program.DetailImageUrl = await _uploadStorage.CopyImage(program?.DetailImageUrl, program?.Source);
            foreach (var credential in program?.Credentials ?? []) {
                credential.ConvertToTest();
                credential.ImageUrl = await _uploadStorage.CopyImage(credential?.ImageUrl, program?.Source);
                var newRequirementSetIds = new List<string>();
                foreach (var requirementSetId in credential?.RequirementSetIds ?? []) {
                    newRequirementSetIds.Add(requirementSetId.Replace(productionSource, testSource));
                }
                credential.RequirementSetIds = newRequirementSetIds;
            }
            program.Prepare();
            _ = await _programSetter.SetProgram(program);
            return program ?? new Program();
        }

        public async Task<Program> TransferProgramToProduction(Program program) {
            var productionSource = program.Source.Trim('!') + "-";
            var testSource = productionSource + "!-";
            program.ConvertToProduction();
            program.SummaryImageUrl = await _uploadStorage.CopyImage(program?.SummaryImageUrl, program?.Source);
            program.DetailImageUrl = await _uploadStorage.CopyImage(program?.DetailImageUrl, program?.Source);
            foreach (var credential in program?.Credentials ?? []) {
                credential.ConvertToProduction();
                credential.ImageUrl = await _uploadStorage.CopyImage(credential?.ImageUrl, program?.Source);
                var newRequirementSetIds = new List<string>();
                foreach (var requirementSetId in credential?.RequirementSetIds ?? []) {
                    newRequirementSetIds.Add(requirementSetId.Replace(testSource, productionSource));
                }
                credential.RequirementSetIds = newRequirementSetIds;
            }
            program.Prepare();
            _ = await _programSetter.SetProgram(program);
            return program ?? new Program();
        }

    }
}
