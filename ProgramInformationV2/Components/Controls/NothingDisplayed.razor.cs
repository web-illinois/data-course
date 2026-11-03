using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;

namespace ProgramInformationV2.Components.Controls {
    public partial class NothingDisplayed {
        [Parameter]
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        [Parameter]
        public FieldType Screen { get; set; } = default!;

        public bool NothingShown() {
            return !(FieldItems != null && FieldItems.Any(fi => fi.FieldType == Screen && fi.ShowItem == true));
        }

    }
}
