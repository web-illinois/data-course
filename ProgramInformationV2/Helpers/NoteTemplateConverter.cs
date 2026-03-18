using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Search.NoteTemplates;

namespace ProgramInformationV2.Helpers {
    public class NoteTemplateConverter() : INoteTemplateConvert {
        public string ConvertToHtml(string s) => NoteTemplateHelper.ConvertMarkdownToHtml(s);
    }
}
