using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;




namespace Botany_Buddy.Helpers
{
    public class PlantIdService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey = "WVDfGbfNVA7Jyd09dxSaWJ05Ut7mGMnK5uvEP5NjWHN3p2ou9h";
        //SaarL1232
        //galaxyhorseclipart@gmail.com
        public PlantIdService()
        {
            _client = new HttpClient();
        }

        public async Task<string> IdentifyPlantAsync(byte[] imageBytes)//יוצר משימה שפועלת עד שמוחזר שם הצמח על פי התמונה שנשלחת אל האפי
        {
            var content = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(imageBytes);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            content.Add(imageContent, "images", "image.jpg");
            content.Add(new StringContent(_apiKey), "api_key");

            var response = await _client.PostAsync("https://api.plant.id/v2/identify", content);
            var responseString = await response.Content.ReadAsStringAsync();

            return ParsePlantDetails(responseString);
        }



        private string ParsePlantDetails(string json)//הופך את הפרטים של הצמח שהתקבלו לקובץ גסון שנשלח וממנו יוצא השם אחר כך
        {
            var jObject = JObject.Parse(json);
            if (jObject["suggestions"]?.Any() == true)
            {
                var firstSuggestion = jObject["suggestions"][0];
                var plantName = firstSuggestion["plant_name"]?.ToString();

                return (plantName ?? "Plant name not found");
            }
            return ("No suggestions found");
        }


    }



}