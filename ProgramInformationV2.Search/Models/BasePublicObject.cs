namespace ProgramInformationV2.Search.Models {

    public abstract class BasePublicObject : BaseObject {
        public string AdmissionRequirements { get; set; } = "";
        public string ContactName { get; set; } = "";
        public string ContactEmail { get; set; } = "";
        public string Description { get; set; } = "";
        public string DetailImageAltText { get; set; } = "";
        public string DetailImageUrl { get; set; } = "";
        public string ImageAltText { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string InternalSearch { get; set; } = "";
        public string JobTypeInformation { get; set; } = "";
        public string ProfessionalInformation { get; set; } = "";
        public string ProgramTracks { get; set; } = "";
        public string Skills { get; set; } = "";
        public string StudentQuoteText { get; set; } = "";
        public string StudentQuoteTitle { get; set; } = "";
        public string StudentQuoteSubtitle { get; set; } = "";
        public string StudentQuoteImageAltText { get; set; } = "";
        public string StudentQuoteImageUrl { get; set; } = "";
        public string SummaryImageAltText { get; set; } = "";
        public string SummaryImageUrl { get; set; } = "";
        public string SummaryText { get; set; } = "";
        public string SummaryTitle { get; set; } = "";
        public string TransferStudentDetails { get; set; } = "";
        public string Url { get; set; } = "";
        public string UrlFull { get; set; } = "";
        public string WhoShouldApply { get; set; } = "";
        public string VideoUrl { get; set; } = "";

        public static string ConvertVideoToEmbed(string href) {
            if (string.IsNullOrWhiteSpace(href)) {
                return string.Empty;
            } else if (href.Contains("youtube", StringComparison.InvariantCultureIgnoreCase) || href.Contains("youtu.be", StringComparison.InvariantCultureIgnoreCase)) {
                href = href.Trim('/').Replace("https://www.youtube.com/watch?v=", string.Empty).Replace("http://www.youtube.com/watch?v=", string.Empty).Replace("https://youtu.be/", string.Empty).Replace("https://www.youtube.com/embed/", string.Empty).Replace("http://www.youtube.com/embed/", string.Empty);
                return $"https://www.youtube.com/embed/{href}";
            } else if (href.Contains("mediaspace", StringComparison.InvariantCultureIgnoreCase) && !href.Contains("embed", StringComparison.InvariantCultureIgnoreCase)) {
                href = href.Trim('/').Split('/').Last();
                return $"https://mediaspace.illinois.edu/embed/secure/iframe/entryId/{href}/uiConfId/26883701";
            }
            return href;
        }

        public void SetFullUrl(string baseUrl) {
            if (string.IsNullOrWhiteSpace(Url)) {
                UrlFull = Url;
            } else if (Url.StartsWith("http")) {
                UrlFull = Url;
            } else {
                UrlFull = $"{baseUrl.TrimEnd('/')}/{Url.TrimStart('/')}";
            }
        }

        internal void ProcessBaseItems() {
            WhoShouldApply = CleanHtml(WhoShouldApply);
            Description = CleanHtml(Description);
            VideoUrl = ConvertVideoToEmbed(VideoUrl);
            TransferStudentDetails = CleanHtml(TransferStudentDetails);
            ProfessionalInformation = CleanHtml(ProfessionalInformation);
            JobTypeInformation = CleanHtml(JobTypeInformation);
            AdmissionRequirements = CleanHtml(AdmissionRequirements);
            SummaryText = CleanHtml(SummaryText);
        }
    }
}