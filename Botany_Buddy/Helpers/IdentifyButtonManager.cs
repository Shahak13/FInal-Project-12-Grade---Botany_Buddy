using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Botany_Buddy.Helpers;
using Botany_Buddy;
using System.IO;
using System.Threading.Tasks;
using System;

public class IdentifyButtonManager
{
    public static int REQUEST_IMAGE_CAPTURE = 1;
    public static string ImageString;


    public static void TakePicture(Activity activity)//מופעל אם נלחץ כפתור הצילום באקטיביטי וזה לוקח תמונה ומחכה לתוצאה באקטיביטי שזה הופעל ממנו
    {
        if (ContextCompat.CheckSelfPermission(activity, Android.Manifest.Permission.Camera) == Android.Content.PM.Permission.Granted)
        {
            Intent takePictureIntent = new Intent(MediaStore.ActionImageCapture);
            if (takePictureIntent.ResolveActivity(activity.PackageManager) != null)
            {
                activity.StartActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
            }
        }
        else
        {
            ActivityCompat.RequestPermissions(activity, new string[] { Android.Manifest.Permission.Camera }, REQUEST_IMAGE_CAPTURE);
        }
    }
    public static void ChoosePicture(Activity activity)
    {
        Intent intent = new Intent();
        intent.SetType("image/*");
        intent.SetAction(Intent.ActionGetContent);
        activity.StartActivityForResult(Intent.CreateChooser(intent,"Choose Picture"),2);
    }

    public static void HandleActivityResult(int requestCode, Result resultCode, Intent data, Activity activity, ProgressDialog progressDialog)//מופעל אם היה לחיצה על כפתור זיהוי באחד האקטיביטים וזה מקבל מאיזה אקטיביטי
    {
        Android.Net.Uri uri;
        if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == Result.Ok)
        {
            Bundle extras = data.Extras;
            Bitmap imageBitmap = (Bitmap)extras.Get("data");
            Bitmap resizedBitmap = ResizeBitmap(imageBitmap, 320, 240);

            progressDialog = ProgressDialog.Show(activity, "Identifying", "Please Wait...", true);
            progressDialog.SetCancelable(false);

            Task.Run(async () =>
            {
                var PlantName = await new PlantIdService().IdentifyPlantAsync(BitmapToByteArray(resizedBitmap));

                activity.RunOnUiThread(() =>
                {
                    progressDialog.Dismiss();
                   
                    Intent intent = new Intent(activity, typeof(InformationActivity));
                    intent.PutExtra("PlantName", PlantName);
                    //intent.PutExtra("Image", ImageConverter.BitmapToString(imageBitmap));
                    IdentifyButtonManager.ImageString = ImageConverter.BitmapToString(resizedBitmap);
                   
                    activity.StartActivity(intent);
                });
            });
        }
        else if (requestCode == 2 &&resultCode == Result.Ok)
        {
            uri = data.Data;
            if (uri != null)
            {
                Bitmap bitmap = BitmapFactory.DecodeStream(activity.ContentResolver.OpenInputStream(uri));
                Bitmap resizedBitmap = ResizeBitmap(bitmap, 80, 60);
                progressDialog = ProgressDialog.Show(activity, "Identifying", "Please Wait...", true);
                progressDialog.SetCancelable(false);
                Task.Run(async () =>
                {
                    var PlantName = await new PlantIdService().IdentifyPlantAsync(BitmapToByteArray(resizedBitmap));

                    activity.RunOnUiThread(() =>
                    {
                        progressDialog.Dismiss();

                        Intent intent = new Intent(activity, typeof(InformationActivity));
                        intent.PutExtra("PlantName", PlantName);
                        //intent.PutExtra("Image", ImageConverter.BitmapToString(bitmap));
                        IdentifyButtonManager.ImageString = ImageConverter.BitmapToString(resizedBitmap);

                        activity.StartActivity(intent);

                    });
                });

            }
        }
    }

    private static byte[] BitmapToByteArray(Bitmap bitmap)//הופך תמונה לאררי
    {
        using (var stream = new MemoryStream())
        {
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            return stream.ToArray();
        }
    }
    public static Bitmap ResizeBitmap(Bitmap originalImage, int maxWidth, int maxHeight)
    {
        int width = originalImage.Width;
        int height = originalImage.Height;
        float ratioBitmap = (float)width / (float)height;
        float ratioMax = (float)maxWidth / (float)maxHeight;

        int finalWidth = maxWidth;
        int finalHeight = maxHeight;
        if (ratioMax > ratioBitmap)
        {
            finalWidth = (int)((float)maxHeight * ratioBitmap);
        }
        else
        {
            finalHeight = (int)((float)maxWidth / ratioBitmap);
        }
        return Bitmap.CreateScaledBitmap(originalImage, finalWidth, finalHeight, true);
    }

}
