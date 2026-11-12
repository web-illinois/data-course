using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.CourseraImport;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Credential {
    public partial class CourseraImport {
        private string _sourceCode = "";
        private bool? _useCourses = true;
        public string SearchTerm { get; set; } = "";
        public string ListOfCourseraSpecialtyId { get; set; } = "";

        public string ListOfCourseraSpecialtyIdSelected { get; set; } = "";
        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        [Inject]
        protected CourseraImportManager CourseraImportManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public Dictionary<string, string> ListOfCourseraSpecialties { get; set; } = default!;

        public Dictionary<string, string> ListOfCourseraSpecialtiesSelected { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            Layout.SetSidebar(SidebarEnum.ProgramCredential, "Coursera Import");
            _sourceCode = await Layout.CheckSource();
            _useCourses = await SourceHelper.DoesSourceUseItem(_sourceCode, Data.DataModels.CategoryType.Credential);
            ListOfCourseraSpecialtiesSelected = new Dictionary<string, string>();
            await Search();
            await base.OnInitializedAsync();
        }


        protected async Task<bool> Transfer() {
            if (ListOfCourseraSpecialties.ContainsKey(ListOfCourseraSpecialtyId)) {
                ListOfCourseraSpecialtiesSelected.TryAdd(ListOfCourseraSpecialtyId, ListOfCourseraSpecialties[ListOfCourseraSpecialtyId]);
                ListOfCourseraSpecialties.Remove(ListOfCourseraSpecialtyId);
                ListOfCourseraSpecialtyId = ListOfCourseraSpecialties.FirstOrDefault().Key ?? "";
                ListOfCourseraSpecialtiesSelected = ListOfCourseraSpecialtiesSelected.OrderBy(c => c.Value).ToDictionary();
            }
            StateHasChanged();
            return true;
        }

        protected async Task<bool> Remove() {
            if (ListOfCourseraSpecialtiesSelected.ContainsKey(ListOfCourseraSpecialtyIdSelected)) {
                ListOfCourseraSpecialties.TryAdd(ListOfCourseraSpecialtyIdSelected, ListOfCourseraSpecialtiesSelected[ListOfCourseraSpecialtyIdSelected]);
                ListOfCourseraSpecialtiesSelected.Remove(ListOfCourseraSpecialtyIdSelected);
                ListOfCourseraSpecialtyIdSelected = ListOfCourseraSpecialtiesSelected.FirstOrDefault().Key ?? "";
                ListOfCourseraSpecialties = ListOfCourseraSpecialties.OrderBy(c => c.Value).ToDictionary();
            }
            StateHasChanged();
            return true;
        }

        protected async Task<bool> TransferAll() {
            foreach (var item in ListOfCourseraSpecialties) {
                ListOfCourseraSpecialtiesSelected.TryAdd(item.Key, item.Value);
            }
            ListOfCourseraSpecialtiesSelected = ListOfCourseraSpecialtiesSelected.OrderBy(c => c.Value).ToDictionary();
            ListOfCourseraSpecialties.Clear();
            StateHasChanged();
            return true;
        }

        protected async Task<bool> RemoveAll() {
            foreach (var item in ListOfCourseraSpecialtiesSelected) {
                ListOfCourseraSpecialties.TryAdd(item.Key, item.Value);
            }
            ListOfCourseraSpecialties = ListOfCourseraSpecialties.OrderBy(c => c.Value).ToDictionary();
            ListOfCourseraSpecialtiesSelected.Clear();
            StateHasChanged();
            return true;
        }

        protected async Task Search() {
            ListOfCourseraSpecialties = await CourseraImportManager.GetSpecialties(SearchTerm);
            StateHasChanged();
        }

        protected async Task SendImport() {
            await Layout.AddMessage("Starting to add credentials - please wait");
            var success = 0;
            var failedTitles = new List<string>();
            foreach (var credential in ListOfCourseraSpecialtiesSelected) {
                var newProgram = await CourseraImportManager.GetSpecialty(_sourceCode, credential.Key);
                if (await ProgramSetter.SetProgram(newProgram) != "") {
                    await Layout.AddMessage("Credential added: " + newProgram.Title);
                    success++;
                } else {
                    failedTitles.Add(string.IsNullOrWhiteSpace(newProgram.Title) ? "unknown credential" : newProgram.Title);
                }
            }
            await Layout.AddMessage($"Total credentials added: {success}. Failed items: {(failedTitles.Count == 0 ? "none" : string.Join(", ", failedTitles))}");
        }

    }
}
