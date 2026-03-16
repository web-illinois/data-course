namespace ProgramInformationV2.Data.NoteTemplateCache {
    public class NoteTemplateCacheClearer(string url) {
        private string _url = url;

        public async Task<bool> Clear() {
            using var clientAuth = new HttpClient();
            var response = await clientAuth.SendAsync(new HttpRequestMessage(HttpMethod.Get, _url));
            return response.IsSuccessStatusCode;
        }
    }
}
