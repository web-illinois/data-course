using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.Cache;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Helpers;

namespace ProgramInformationV2.Components.Pages.Configuration {

    public partial class Testing {
        private string _email = "";

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        public void SwitchToTesting(bool gotoSecurity) {
            CacheHolder.SetCacheSource(_email, "test", "");
            NavigationManager.NavigateTo(gotoSecurity ? "/configuration/security" : "/", true);
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            var sidebar = string.IsNullOrWhiteSpace(await Layout.CheckSource(false)) ? SidebarEnum.ConfigurationNoSource : SidebarEnum.Configuration;
            Layout.SetSidebar(sidebar, "Configuration");
            _email = await UserHelper.GetUser(AuthenticationStateProvider);
            Layout.SetSidebar(SidebarEnum.Configuration, "Configuration");
        }
    }
}