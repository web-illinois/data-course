using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Components.Controls {

    public partial class RestrictedEntry {
        public bool IsAdded { get; set; }

        [Parameter]
        public string ItemId { get; set; } = "";

        [Parameter]
        public SecurityEntry SecurityEntry { get; set; } = new();

        [Inject]
        protected SecurityHelper SecurityHelper { get; set; } = default!;

        public async Task Change(ChangeEventArgs args) {
            if (!IsAdded && !SecurityEntry.RestrictedIds.Contains(ItemId)) {
                SecurityEntry.RestrictedIds += ItemId + ";";
            } else {
                SecurityEntry.RestrictedIds.Replace(ItemId + ";", "");
            }
            if (await SecurityHelper.UpdateName(SecurityEntry)) {
                IsAdded = !IsAdded;
                StateHasChanged();
            }
        }

        protected override void OnInitialized() {
            IsAdded = SecurityEntry.RestrictedIds.Contains(ItemId);
            StateHasChanged();
        }
    }
}