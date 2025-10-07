namespace ProgramInformationV2.Search.Models {

    public class GenericItemWithChildren : GenericItem {
        public List<GenericItem> Children { get; set; } = [];
    }
}