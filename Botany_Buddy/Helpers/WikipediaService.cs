using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace Botany_Buddy
{
    public class WikipediaService
    {
        public async Task<string> FetchWikipediaContent(string title)//שולף מידע מויקפדיה על הצמח לפי שם
        {
            var apiUrl = $"https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&titles={Uri.EscapeDataString(title)}&redirects=true&exintro=true&explaintext=true";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(apiUrl);
                var data = JsonConvert.DeserializeObject<WikipediaApiResponse>(response);

                var pageContent = ExtractContentFromApiResponse(data);
                return pageContent;
            }
        }

        private string ExtractContentFromApiResponse(WikipediaApiResponse response)//מהמידע שנשלף שולף את המידע מהפסקה הראשונה
        {
            var page = response.Query.Pages.FirstOrDefault().Value;
            return page?.Extract ?? "No content available.";
        }

        public class WikipediaApiResponse
        {
            [JsonProperty("query")]
            public Query Query { get; set; }
        }

        public class Query
        {
            [JsonProperty("pages")]
            public Dictionary<long, Page> Pages { get; set; }
        }

        public class Page
        {
            [JsonProperty("extract")]
            public string Extract { get; set; }
        }
    }
}
