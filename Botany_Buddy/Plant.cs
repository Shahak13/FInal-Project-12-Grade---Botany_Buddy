using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Botany_Buddy
{
    public class Plant
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ImageFoundUrl { get; set; }
        string content;

        public Plant(string name, string imageUrl, string imageFoundUrl)
        {
            Name = name;
            ImageUrl = imageUrl;
            ImageFoundUrl = imageFoundUrl;
        }

        public Plant()
        {
        }

        private async Task<string>  LoadInfoFromWikipedia(string plantName)//טוען את המידע מויקיפדיה
        {
            var wikipediaService = new WikipediaService();
            try
            {
                content = await wikipediaService.FetchWikipediaContent(plantName);
                return content;
            }
            catch (Exception ex)
            {
                content = "Could not find an info";
                return content;
                }
        }

        public string GetName() { return Name; }
        public string GetImageUrl() {  return ImageUrl; }
        public Bitmap GetImage()
        {
            return ImageConverter.stringToBitmap(this.ImageUrl);
        }
       
        public async Task<string> GetDescription()//מחזיר את המידע שנמצא מויקפדיה על הצמח
        {
            if (string.IsNullOrEmpty(content))
            {
                await LoadInfoFromWikipedia(this.Name);
            }
            return content;
        }
    }


}

