namespace ProgramInformationV2.Search.JsonThinModels {

    public class SearchObject<T> {
        public string DidYouMean { get; set; } = "";
        public string Error { get; set; } = "";
        public List<T> Items { get; set; } = [];
        public int Total { get; set; } = 0;
    }
}