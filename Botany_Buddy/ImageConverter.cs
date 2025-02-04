using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Botany_Buddy
{
    public static class ImageConverter
    {
        public static Bitmap stringToBitmap(string base64String)//מקבל מחרושת שמרכיבה תמונה ומחזירה אותה כתמונה
        {
            byte[] imageAsBytes = Base64.Decode(base64String, Base64Flags.Default);
            return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
        }
        public static string BitmapToString(Bitmap bitmap)//מקבל תמונה ומחזיר אותה כמחרוזת
        {
            string str = "";
            using(var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byte[] bytes = stream.ToArray();
                str=Convert.ToBase64String(bytes);
            }
            return str;
        }
    }
}