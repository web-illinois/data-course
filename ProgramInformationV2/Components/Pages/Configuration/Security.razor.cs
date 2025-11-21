using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.Cache;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Helpers;

namespace ProgramInformationV2.Components.Pages.Configuration {

    public partial class Security {
        public bool ApiDraft { get; set; } = false;

        public string ApiGuid { get; set; } = "";

        public bool IsRestricted { get; set; }
        public DateTime LastDateApiChanged { get; set; }

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public List<SecurityEntry> NetIds { get; set; } = [];
        public string NewNetId { get; set; } = "";
        public bool UseApi { get; set; } = false;

        [Inject]
        protected ApiHelper ApiHelper { get; set; } = default!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SecurityHelper SecurityHelper { get; set; } = default!;

        public async Task Add() {
            if (!string.IsNullOrWhiteSpace(NewNetId)) {
                var source = await Layout.CheckSource();
                var newId = await SecurityHelper.AddName(source, NewNetId);
                if (!string.IsNullOrWhiteSpace(newId)) {
                    NetIds.Add(new SecurityEntry { Email = newId, IsActive = true, IsRestricted = false });
                }
                NewNetId = "";
            }
        }

        public async Task CreateApi() {
            ApiGuid = await ApiHelper.AdvanceApi(await Layout.CheckSource());
            UseApi = true;
            ApiDraft = true;
            LastDateApiChanged = DateTime.Now;
        }

        public async Task Deactivate(SecurityEntry entry) {
            entry.IsActive = !entry.IsActive;
            await SecurityHelper.UpdateName(entry);
            var email = await UserHelper.GetUser(AuthenticationStateProvider);
            if (entry.Email == email) {
                CacheHolder.ClearCache(email);
                NavigationManager.NavigateTo("/", true);
            }
            NewNetId = "";
            StateHasChanged();
        }

        public async Task InvalidateApi() {
            _ = await ApiHelper.InvalidateApi(await Layout.CheckSource());
            ApiGuid = "";
            UseApi = false;
        }

        public async Task Remove(SecurityEntry entry) {
            var source = await Layout.CheckSource();
            if (await SecurityHelper.RemoveName(source, entry.Email)) {
                NetIds.Remove(entry);
            }
            var email = await UserHelper.GetUser(AuthenticationStateProvider);
            if (entry.Email == email) {
                CacheHolder.ClearCache(email);
                NavigationManager.NavigateTo("/", true);
            }
            NewNetId = "";
            StateHasChanged();
        }

        public async Task Restrict(SecurityEntry entry) {
            entry.IsRestricted = !entry.IsRestricted;
            await SecurityHelper.UpdateName(entry);
            var email = await UserHelper.GetUser(AuthenticationStateProvider);
            if (entry.Email == email) {
                CacheHolder.ClearCache(email);
                NavigationManager.NavigateTo("/", true);
            }
            NewNetId = "";
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            var source = await Layout.CheckSource();
            NetIds = await SecurityHelper.GetNames(source);
            Layout.SetSidebar(SidebarEnum.Configuration, "Configuration");
            var netId = await AuthenticationStateProvider.GetUser();
            (UseApi, LastDateApiChanged) = await ApiHelper.GetApi(source);
            IsRestricted = NetIds.FirstOrDefault(n => n.Email == netId)?.IsRestricted ?? true;
        }
    }
}