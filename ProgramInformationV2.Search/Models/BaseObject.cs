using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProgramInformationV2.Search.Models {

    public abstract class BaseObject {
        private static readonly string[] _badHtmlItems = ["<br>", "<p></p>", "<p><br></p>", "<p>&nbsp;</p>", "&nbsp;"];

        public DateTime CreatedOn { get; set; }

        public string Fragment { get; set; } = "";

        public string Id { get; set; } = "";

        [JsonIgnore]
        public virtual string InternalTitle => Title;

        public bool IsActive { get; set; }

        [JsonIgnore]
        public virtual bool IsIdValid => !string.IsNullOrWhiteSpace(Id) && !string.IsNullOrWhiteSpace(Source) && Id.StartsWith(Source + "-");

        public DateTime LastUpdated { get; set; }

        public string LastUpdatedBy { get; set; } = "";

        public int Order { get; set; }

        public string Source { get; set; } = "";

        public string Title { get; set; } = "";

        internal virtual string CreateId => Source + "-" + Guid.NewGuid().ToString();

        public static string CleanHtml(string s) => string.IsNullOrWhiteSpace(s) || _badHtmlItems.Contains(s) ? string.Empty : s.Replace(" style=", " data-style=", StringComparison.OrdinalIgnoreCase);

        public virtual void CleanHtmlFields() {
        }

        public virtual GenericItem GetGenericItem() => new() { Id = Id, IsActive = IsActive, Order = Order, Title = InternalTitle };

        public virtual void Prepare() {
            SetId();
            SetFragment();
            CleanHtmlFields();
        }

        public virtual void SetFragment() => Fragment = string.IsNullOrWhiteSpace(Fragment) ? "" : new string(Fragment.Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-').ToArray()).Replace(" ", "-").ToLowerInvariant();

        public virtual void SetId() => Id = string.IsNullOrWhiteSpace(Id) ? CreateId : Id;

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}