namespace ProgramInformationV2.Search.Models {

    public enum CredentialType { None, Base_Undergraduate, Undergraduate_Certificate, BA, BS, Undergraduate_Minor, Base_Graduate = 20, Graduate_Certificate, EdM, MBA, MA, MS, CAS, EdD, PhD, Graduate_Concentration, Graduate_Minor, Base_Certificate = 40, Certificate_of_Specialization, Endorsement, Specialty, MOOC_Specialization_Certificate, Illinois_Graduate_Certificate }

    [Flags]
    public enum FormatType { None = 0, Online = 1, On__Campus = 2, Off__Campus = 4, Hybrid = 8 }

    public enum Terms { None, Fall, Spring, Summer, Summer1, Summer2, Winter, Ongoing }

    public enum UrlTypes { Programs, Courses, RequirementSets }

    public enum NoteTemplateTypes { Programs = 1, Credentials = 2, Courses = 3 }

    public static class ExtensionTypes {

        internal static Dictionary<DayOfWeek, string> daysList = new() {
            { DayOfWeek.Monday, "Mo" },
            { DayOfWeek.Tuesday, "Tu" },
            { DayOfWeek.Wednesday, "We" },
            { DayOfWeek.Thursday, "Th" },
            { DayOfWeek.Friday, "Fr" },
            { DayOfWeek.Saturday, "Sa" },
            { DayOfWeek.Sunday, "Su" }
        };

        private static readonly List<CredentialType> _certificates = [CredentialType.Base_Certificate, CredentialType.CAS, CredentialType.Graduate_Certificate, CredentialType.Illinois_Graduate_Certificate, CredentialType.Certificate_of_Specialization, CredentialType.Endorsement, CredentialType.MOOC_Specialization_Certificate];
        private static readonly List<CredentialType> _graduateDegrees = [CredentialType.Base_Graduate, CredentialType.EdM, CredentialType.MA, CredentialType.MS, CredentialType.EdD, CredentialType.PhD, CredentialType.Graduate_Concentration, CredentialType.Graduate_Minor, CredentialType.MBA];
        private static readonly List<CredentialType> _undergraduateDegrees = [CredentialType.Undergraduate_Certificate, CredentialType.Base_Undergraduate, CredentialType.BA, CredentialType.BS, CredentialType.Undergraduate_Minor];

        public static IEnumerable<CredentialType> Certificates(this IEnumerable<CredentialType> e) => e.Where(ct => _certificates.Contains(ct));

        public static FormatType CombineFormatList(this IEnumerable<FormatType> e) {
            var format = new FormatType();
            foreach (var item in e)
                format |= item;
            return format;
        }

        public static string ConvertDaysToString(this IEnumerable<DayOfWeek> daysOfWeek) => daysOfWeek == null ? "" : (daysOfWeek.Count() > 4 ? "Multiple" : string.Join(",", daysOfWeek.OrderBy(d => d).Select(d => daysList[d])));

        public static IEnumerable<string> ConvertFormatList(this FormatType e) => e.ToString("G").Split(',').Select(ConvertToString);

        public static string ConvertToSingleString(this Enum e) => ConvertToString(e.ToString());

        public static string ConvertToUrlString(this Enum e) => "pcr2_" + e.ToString().ToLowerInvariant();

        public static IEnumerable<CredentialType> GraduateDegrees(this IEnumerable<CredentialType> e) => e.Where(_graduateDegrees.Contains);

        public static bool IsFormat(this FormatType? format, FormatType check) => format != null && format != FormatType.None && format.Value.HasFlag(check);

        public static IEnumerable<CredentialType> UndergraduateDegrees(this IEnumerable<CredentialType> e) => e.Where(_undergraduateDegrees.Contains);

        private static string ConvertToString(string? s) => s == null ? "" : s.Replace("Base_", "").Replace("__", "-").Replace("_", " ").Replace("None", "").Trim();
    }
}