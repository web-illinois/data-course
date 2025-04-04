namespace ProgramInformationV2.Search.Models {

    public abstract class BasePublicObject : BaseObject {
        public string Description { get; set; } = "";

        public string Url { get; set; } = "";

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
    }
}