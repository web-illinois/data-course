using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;

namespace ProgramInformationV2.Components.Pages.Configuration {

    public partial class Filters {

        private readonly Dictionary<string, TagType> _translator = new() {
            { "", TagType.None },
            { "Departments", TagType.Department },
            { "Tags", TagType.Tag },
            { "Skills", TagType.Skill },
            { "Length", TagType.Length }
        };

        private int _sourceId;

        public List<TagSource> FilterTags { get; set; } = [];
        public List<TagSource> FilterTagsForDeletion { get; set; } = [];
        public string FilterType { get; set; } = "";

        public TagType FilterTypeEnum => _translator[FilterType];

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public string NewFilterName { get; set; } = "";

        [Inject]
        protected FilterHelper FilterHelper { get; set; } = default!;

        public void Add() {
            Layout.SetDirty();
            FilterTags.Add(new TagSource { IsActive = true, LastUpdated = DateTime.Now, Title = NewFilterName, SourceId = _sourceId, TagType = FilterTypeEnum });
            NewFilterName = "";
        }

        public async Task ChangeFilter() {
            var sourceCode = await Layout.CheckSource();
            (FilterTags, _sourceId) = await FilterHelper.GetFilters(sourceCode, FilterTypeEnum);
            FilterTagsForDeletion.Clear();
        }

        public void MoveDown(TagSource tagSource) {
            Layout.SetDirty();
            FilterTags.MoveItemDown(tagSource);
        }

        public void MoveUp(TagSource tagSource) {
            Layout.SetDirty();
            FilterTags.MoveItemUp(tagSource);
        }

        public void Remove(TagSource tagSource) {
            Layout.SetDirty();
            FilterTags.Remove(tagSource);
            FilterTagsForDeletion.Add(tagSource);
        }

        public async Task<bool> Save() {
            await FilterHelper.SaveFilters(FilterTags, FilterTagsForDeletion, await Layout.CheckSource());
            await Layout.AddMessage($"Filters for {FilterType} have been saved");
            Layout.RemoveDirty();
            FilterTagsForDeletion.Clear();
            return true;
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            await Layout.CheckSource();
            Layout.SetSidebar(SidebarEnum.Configuration, "Configuration");
        }
    }
}