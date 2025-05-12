using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Course {

    public partial class Faculty {
        public ProgramInformationV2.Search.Models.Course CourseItem { get; set; } = new();

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public string NewName { get; set; } = "";
        public string NewNetId { get; set; } = "";
        public string NewProfileUrl { get; set; } = "";
        public bool NewShowInProfile { get; set; }

        public string QuickLinkUrl { get; set; } = "";

        [Inject]
        protected CourseGetter CourseGetter { get; set; } = default!;

        [Inject]
        protected CourseSetter CourseSetter { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        public void Add() {
            Layout.SetDirty();
            CourseItem.FacultyNameList.Add(new SectionFaculty {
                Name = NewName,
                NetId = NewNetId,
                Url = NewProfileUrl,
                ShowInProfile = NewShowInProfile
            });
        }

        public void Remove(int i) {
            CourseItem.FacultyNameList.RemoveAt(i);
        }

        public async Task Save() {
            Layout.RemoveDirty();
            _ = await CourseSetter.SetCourse(CourseItem);
            await Layout.Log(CategoryType.Course, FieldType.Faculty, CourseItem);
            await Layout.AddMessage("Course saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            CourseItem = await CourseGetter.GetCourse(id);
            if (CourseItem.FacultyNameList == null) {
                CourseItem.FacultyNameList = new List<SectionFaculty>();
            }
            Layout.SetSidebar(SidebarEnum.Course, CourseItem.Title);
            QuickLinkUrl = await Layout.GetCachedQuickLink();
            await base.OnInitializedAsync();
        }
    }
}