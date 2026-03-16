using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.NoteTemplateCache;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Components.Pages.Configuration {
    public partial class NoteTemplateList {
        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected FilterHelper FilterHelper { get; set; } = default!;

        [Inject]
        protected NoteTemplateHelper NoteTemplateHelper { get; set; } = default!;

        [Inject]
        protected NoteTemplateCacheClearer NoteTemplateCacheClearer { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;
        public List<string> CriteriaOptions { get; set; } = default!;

        public CategoryType CategorySelector { get; set; }
        public List<NoteTemplate> NoteTemplates { get; set; } = default!;
        public List<NoteTemplate> NoteTemplatesForDeletion { get; set; } = default!;
        public string NewNoteDescription { get; set; } = "";
        public int NewNoteId { get; set; }
        public string NewNoteLinkName { get; set; } = "";
        public string NewNoteLinkUrl { get; set; } = "";
        public string NewNoteCriteriaType { get; set; } = "";
        public string NewNoteCriteriaText { get; set; } = "";
        public string NewNoteTitle { get; set; } = "";

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            var source = await Layout.CheckSource();
            Layout.SetSidebar(SidebarEnum.Configuration, "Configuration");
        }

        protected async Task ChangeCategorySelector() {
            var source = await Layout.CheckSource();
            NoteTemplates = await NoteTemplateHelper.GetNoteTemplatesAsync(source, CategorySelector);
            NoteTemplatesForDeletion = [];
            Clear();
        }

        protected async Task ChangeFilter() {
            NewNoteCriteriaText = "";
            var source = await Layout.CheckSource();
            if (NewNoteCriteriaType == "Department") {
                var filters = await FilterHelper.GetFilters(source, TagType.Department);
                CriteriaOptions = [.. filters.TagSources.Select(f => f.Title)];
            } else if (NewNoteCriteriaType == "Skill") {
                var filters = await FilterHelper.GetFilters(source, TagType.Skill);
                CriteriaOptions = [.. filters.TagSources.Select(f => f.Title)];
            } else if (NewNoteCriteriaType == "Tag") {
                var filters = await FilterHelper.GetFilters(source, TagType.Tag);
                CriteriaOptions = [.. filters.TagSources.Select(f => f.Title)];
            } else if (NewNoteCriteriaType == "Credential Degree") {
                CriteriaOptions = [.. Enum.GetNames(typeof(CredentialType))];
            } else if (NewNoteCriteriaType == "Format") {
                CriteriaOptions = [.. Enum.GetNames(typeof(FormatType))];
            } else {
                CriteriaOptions = [];
            }
        }

        protected async Task Add() {
            if (NewNoteId == 0) {
                NoteTemplates.Add(new NoteTemplate {
                    Title = NewNoteTitle,
                    Description = NewNoteDescription,
                    LinkText = NewNoteLinkName,
                    LinkUrl = NewNoteLinkUrl,
                    CategoryType = CategorySelector,
                    CredentialTypeCriteria = NewNoteCriteriaType == "Credential Degree" ? Enum.TryParse<CredentialType>(NewNoteCriteriaText, out var credentialType) ? credentialType : CredentialType.None : CredentialType.None,
                    DepartmentCriteria = NewNoteCriteriaType == "Department" ? NewNoteCriteriaText : "",
                    SkillCriteria = NewNoteCriteriaType == "Skill" ? NewNoteCriteriaText : "",
                    TagCriteria = NewNoteCriteriaType == "Tag" ? NewNoteCriteriaText : "",
                    FormatTypeCriteria = NewNoteCriteriaType == "Format" ? Enum.TryParse<FormatType>(NewNoteCriteriaText, out var formatType) ? formatType : FormatType.None : FormatType.None,
                });
            } else if (NoteTemplates.FirstOrDefault(nt => nt.Id == NewNoteId) != null) {
                NoteTemplates.First(nt => nt.Id == NewNoteId).Title = NewNoteTitle;
                NoteTemplates.First(nt => nt.Id == NewNoteId).Description = NewNoteDescription;
                NoteTemplates.First(nt => nt.Id == NewNoteId).LinkText = NewNoteLinkName;
                NoteTemplates.First(nt => nt.Id == NewNoteId).LinkUrl = NewNoteLinkUrl;
                NoteTemplates.First(nt => nt.Id == NewNoteId).CategoryType = CategorySelector;
                NoteTemplates.First(nt => nt.Id == NewNoteId).CredentialTypeCriteria = NewNoteCriteriaType == "Credential Degree" ? Enum.TryParse<CredentialType>(NewNoteCriteriaText, out var credentialType) ? credentialType : CredentialType.None : CredentialType.None;
                NoteTemplates.First(nt => nt.Id == NewNoteId).DepartmentCriteria = NewNoteCriteriaType == "Department" ? NewNoteCriteriaText : "";
                NoteTemplates.First(nt => nt.Id == NewNoteId).SkillCriteria = NewNoteCriteriaType == "Skill" ? NewNoteCriteriaText : "";
                NoteTemplates.First(nt => nt.Id == NewNoteId).TagCriteria = NewNoteCriteriaType == "Tag" ? NewNoteCriteriaText : "";
                NoteTemplates.First(nt => nt.Id == NewNoteId).FormatTypeCriteria = NewNoteCriteriaType == "Format" ? Enum.TryParse<FormatType>(NewNoteCriteriaText, out var formatType) ? formatType : FormatType.None : FormatType.None;
            }
            Clear();
        }

        protected async Task Save() {
            var sourceId = await SourceHelper.GetSourceId(await Layout.CheckSource());
            for (var i = 0; i < NoteTemplates.Count; i++) {
                NoteTemplates[i].Order = i + 1;
                NoteTemplates[i].SourceId = sourceId;
                await NoteTemplateHelper.SaveNoteTemplate(NoteTemplates[i]);
            }
            NoteTemplatesForDeletion.ForEach(async t => await NoteTemplateHelper.DeleteNoteTemplate(t));
            var refresh = await NoteTemplateCacheClearer.Clear();
            await Layout.AddMessage($"{CategorySelector} note templates have been saved. {(refresh ? "Cache refreshed successfully." : "Failed to refresh cache.")}");
        }

        protected async Task SelectItem(NoteTemplate item) {
            NewNoteId = item.Id;
            NewNoteTitle = item.Title;
            NewNoteDescription = item.Description;
            NewNoteLinkName = item.LinkText;
            NewNoteLinkUrl = item.LinkUrl;
            if (item.CredentialTypeCriteria != CredentialType.None) {
                NewNoteCriteriaType = "Credential Degree";
                await ChangeFilter();
                NewNoteCriteriaText = Enum.GetName(item.CredentialTypeCriteria) ?? "";
            } else if (item.FormatTypeCriteria != FormatType.None) {
                NewNoteCriteriaType = "Format";
                await ChangeFilter();
                NewNoteCriteriaText = Enum.GetName(item.FormatTypeCriteria) ?? "";
            } else if (item.DepartmentCriteria != "") {
                NewNoteCriteriaType = "Department";
                await ChangeFilter();
                NewNoteCriteriaText = item.DepartmentCriteria;
            } else if (item.SkillCriteria != "") {
                NewNoteCriteriaType = "Skill";
                await ChangeFilter();
                NewNoteCriteriaText = item.SkillCriteria;
            } else if (item.TagCriteria != "") {
                NewNoteCriteriaType = "Tag";
                await ChangeFilter();
                NewNoteCriteriaText = item.TagCriteria;
            }
        }

        protected async Task MoveDown(NoteTemplate item) {
            NoteTemplates.MoveItemDown(item);
        }

        protected async Task MoveUp(NoteTemplate item) {
            NoteTemplates.MoveItemUp(item);
        }

        protected async Task Remove(NoteTemplate item) {
            NoteTemplatesForDeletion.Add(item);
            NoteTemplates.Remove(item);
        }

        private void Clear() {
            NewNoteCriteriaText = "";
            NewNoteCriteriaType = "";
            NewNoteDescription = "";
            NewNoteLinkName = "";
            NewNoteLinkUrl = "";
            NewNoteTitle = "";
            NewNoteId = 0;
        }
    }
}
