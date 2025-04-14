using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProgramInformationV2.Data.Cache;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Helpers;

namespace ProgramInformationV2.Components.Pages {

    public partial class Home {

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        public int NumberOfSources { get; set; }

        [SupplyParameterFromQuery]
        public bool RedirectIfNoSource { get; set; } = false;

        public string SelectedSource { get; set; } = "";
        public string SelectedSourceTitle { get; set; } = "";
        public bool UseCourses { get; set; }

        public bool UseCredentials { get; set; }

        public bool UsePrograms { get; set; }

        public bool UseRequirementSets { get; set; }

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        protected Dictionary<string, string> Sources { get; set; } = default!;

        protected async Task ChangeSource(ChangeEventArgs e) {
            SelectedSource = e.Value?.ToString() ?? "";
            SelectedSourceTitle = Sources[SelectedSource];
            var email = await UserHelper.GetUser(AuthenticationStateProvider);
            CacheHolder.SetCacheSource(email, SelectedSource);
            RedirectIfNoSource = false;
            await ChangeBoxes();
        }

        protected override async Task OnInitializedAsync() {
            var email = await UserHelper.GetUser(AuthenticationStateProvider);
            Sources = await SourceHelper.GetSources(email);
            NumberOfSources = Sources.Count;
            if (NumberOfSources == 1) {
                SelectedSource = Sources.First().Key;
                SelectedSourceTitle = Sources[SelectedSource];
                CacheHolder.SetCacheSource(email, SelectedSource);
            } else if (CacheHolder.HasCachedItem(email)) {
                SelectedSource = CacheHolder.GetCacheSource(email) ?? "";
                SelectedSourceTitle = Sources[SelectedSource];
            }
            await ChangeBoxes();
            base.OnInitialized();
        }

        private async Task ChangeBoxes() {
            if (!string.IsNullOrWhiteSpace(SelectedSource)) {
                UseCredentials = await SourceHelper.DoesSourceUseItem(SelectedSource, CategoryType.Credential);
                UseCourses = await SourceHelper.DoesSourceUseItem(SelectedSource, CategoryType.Course);
                UseRequirementSets = await SourceHelper.DoesSourceUseItem(SelectedSource, CategoryType.RequirementSet);
                UsePrograms = await SourceHelper.DoesSourceUseItem(SelectedSource, CategoryType.Program);
            }
        }
    }
}