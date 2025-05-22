using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Components.Controls {

    public partial class SearchGenericItem {

        [Parameter]
        public string ActionButtonTitle { get; set; } = "";

        [Parameter]
        public string AddButtonTitle { get; set; } = "";

        [Parameter]
        public EventCallback<string> AddClicked { get; set; }

        [Parameter]
        public EventCallback<string> EditClicked { get; set; }

        [Parameter]
        public List<GenericItem> GenericItems { get; set; } = default!;

        public bool IsEditDisabled => SelectedItemId == null || string.IsNullOrWhiteSpace(SelectedItemId);

        [Parameter]
        public string ItemTitle { get; set; } = "";

        [Parameter]
        public EventCallback<string> SearchClicked { get; set; }

        [Parameter]
        public string SearchItem { get; set; } = "";

        public string SelectedItemId { get; set; } = "";

        public string SelectedItemTitle => GenericItems.FirstOrDefault(gi => gi.Id == SelectedItemId)?.Title ?? "";

        public bool UseAddButton => !string.IsNullOrWhiteSpace(AddButtonTitle);

        public void Add() {
            AddClicked.InvokeAsync();
        }

        public void Edit() {
            if (GenericItems.Count == 1) {
                SelectedItemId = GenericItems[0].Id;
            }
            EditClicked.InvokeAsync();
        }

        public void Search() {
            SearchClicked.InvokeAsync();
        }

        protected async Task FilterChange(ChangeEventArgs e) {
            SearchItem = e.Value?.ToString() ?? "";
            await SearchClicked.InvokeAsync();
        }

        protected override void OnInitialized() {
            if (string.IsNullOrWhiteSpace(ActionButtonTitle)) {
                ActionButtonTitle = "Edit " + ItemTitle;
            }
            base.OnInitialized();
        }
    }
}