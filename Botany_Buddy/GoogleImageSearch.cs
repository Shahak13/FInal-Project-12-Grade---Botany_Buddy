using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Botany_Buddy
{
    public class GoogleImageSearch
    {
        private readonly string apiKey = "AIzaSyDFTDla-Gfh5FPo9I3ZlkhlA5NmbRBjsCc";
        private readonly string cx = "070ff36185d5c4985";
        private readonly string searchUrl = "https://www.googleapis.com/customsearch/v1";

        public async Task<ImageData> SearchForImages(string query)// מחפש תמונה בגוגל לפי שם שמתקבל
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string encodedQuery = System.Net.WebUtility.UrlEncode(query);
                    string requestUri = $"{searchUrl}?key={apiKey}&cx={cx}&q={encodedQuery}&searchType=image";
                    var response = await httpClient.GetStringAsync(requestUri);

                  
                    Console.WriteLine(response);

                    var imageData = JsonConvert.DeserializeObject<ImageData>(response);
                    return imageData;
                }
            }
            catch (JsonException jsonEx)//אם יש בעיה הוא כותב אותה
            {
                Console.WriteLine("JSON Error: " + jsonEx.Message);
                throw;
            }
            catch (Exception ex)//אותו דבר
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
        




    }

    


    public class ImageData
    {
        [JsonProperty("items")]
        public List<ImageItem> Items { get; set; }
    }

    public class ImageItem
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        // Add other relevant properties from the API response if needed
    }

}