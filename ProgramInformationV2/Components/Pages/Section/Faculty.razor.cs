using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Section {

    public partial class Faculty {

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public string NewName { get; set; } = "";
        public string NewNetId { get; set; } = "";
        public string NewProfileUrl { get; set; } = "";
        public bool NewShowInProfile { get; set; }
        public Search.Models.Section SectionItem { get; set; } = new Search.Models.Section();

        [Inject]
        protected CourseGetter CourseGetter { get; set; } = default!;

        [Inject]
        protected CourseSetter CourseSetter { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        public void Add() {
            Layout.SetDirty();
            SectionItem.FacultyNameList.Add(new SectionFaculty {
                Name = NewName,
                NetId = NewNetId,
                Url = NewProfileUrl,
                ShowInProfile = NewShowInProfile
            });
        }

        public void Remove(int i) {
            SectionItem.FacultyNameList.RemoveAt(i);
        }

        public async Task Save() {
            Layout.RemoveDirty();
            _ = await CourseSetter.SetSection(SectionItem);
            await Layout.Log(CategoryType.Section, FieldType.Faculty, SectionItem);
            await Layout.AddMessage("Section saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            SectionItem = await CourseGetter.GetSection(id);
            await Layout.SetSidebar(SidebarEnum.Section, SectionItem.Title);
            await base.OnInitializedAsync();
        }
    }
}