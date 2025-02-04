using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Botany_Buddy.Helpers;
using Square.Picasso;
using System;
using System.Threading.Tasks;

namespace Botany_Buddy
{
    [Activity(Label = "InformationActivity")]
    public class InformationActivity : Activity
    {
        TextView tvName;
        TextView tvInfo;
        ImageView ivImage;
        ImageView ivImage2;
        string PlantName;
        string content;
        string ImageTakenUrl;
        string ImageFoundUrl;
        GoogleImageSearch imageSearch;
        Button btnSave;
        Button btnTimeGot;
        Button btnSubmit;
        EditText etSize;
        EditText etLeafSize;
        ProgressDialog progressDialog;
        DateTime Got;
        Dialog d;
        ImageButton btnHome;
        ImageButton btnIdentify;
        ProgressDialog p;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Information_layout);
            tvName = (TextView)FindViewById(Resource.Id.tvName);
            tvInfo = (TextView)FindViewById(Resource.Id.tvInfo);
            ivImage = (ImageView)FindViewById(Resource.Id.ivImage);
            ivImage2 = (ImageView)FindViewById(Resource.Id.ivImage2);
            btnSave = (Button)FindViewById(Resource.Id.btnSave);
            btnHome =(ImageButton)FindViewById(Resource.Id.btnHome);
            btnIdentify = (ImageButton)FindViewById(Resource.Id.btnIdentify);
            btnSave.Click += BtnSave_Click;
            btnHome.Click += BtnHome_Click;

            PlantName = Intent.GetStringExtra("PlantName");
            tvName.Text = PlantName;
            ImageTakenUrl = IdentifyButtonManager.ImageString;
            //Bitmap imagetaken = ImageConverter.stringToBitmap(Intent.GetStringExtra("Image"));
            Bitmap imagetaken = ImageConverter.stringToBitmap(IdentifyButtonManager.ImageString);

            imageSearch = new GoogleImageSearch();
            ivImage2.SetImageBitmap(imagetaken);
            progressDialog = new ProgressDialog(this);
            progressDialog.SetMessage("Loading...");
            progressDialog.SetCancelable(false);
            LoadImage(PlantName);
            LoadInfoFromWikipedia(PlantName);
            btnIdentify.Click += (sender, e) =>
            {
                IdentifyButtonManager.TakePicture(this);
            };



        }

        private void BtnHome_Click(object sender, EventArgs e)//כפתור שמחזיר למסך הראשי
        {
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }

        private void BtnSave_Click(object sender, EventArgs e)//כפתור שמירה אם אתה רוצה לשמור את הצמח לגינה שלך אתה לוחץ על זה ומופיע לך דיאלוג ששואל אותך פרטים על הצמח עד שנלחץ כפתור שליחה
        {
            
            d =new Dialog(this);
            d.SetContentView(Resource.Layout.OwnedPlant_Dialog);
            btnTimeGot = (Button)d.FindViewById(Resource.Id.btnTimeGot);
            etSize = (EditText)d.FindViewById(Resource.Id.etSize);
            etLeafSize = (EditText)d.FindViewById(Resource.Id.etLeafSize);
            btnSubmit = (Button)d.FindViewById(Resource.Id.btnSubmit);

            btnTimeGot.Click += BtnTimeGot_Click;
            btnSubmit.Click += BtnSubmit_Click;
            d.Show();
            
        }

        private void BtnSubmit_Click(object sender, EventArgs e)//הפעולה שליחה בודקת כל השדות מלאים נכון ואם כן מוסיפה את הצמח למשתמש הכללי ואם לא היא שולחת הודעה שאומרת למלא הכל
        {
            Plant plant = new Plant(PlantName, ImageTakenUrl,ImageFoundUrl);
            if(plant!=null && Got != null && etSize.Text != null &&etSize.Text!=""&&etLeafSize.Text != null&&etLeafSize.Text!="")
            {
                MainItemsActivity.user.myGarden.AddPlantToGardenMain(plant, Got, int.Parse(etSize.Text), int.Parse(etLeafSize.Text));
                d.Dismiss();
            }
            else
            {
                Toast.MakeText(this, "You Did Not Fill Everything",ToastLength.Short).Show();
            }
            
        }

        

        

        

        private void BtnTimeGot_Click(object sender, EventArgs e)//הפעולה מתחילה דיאלוג של תאריך ומציבה את התוצאה בשם הכפתור
        {
            DateTime date = DateTime.Today;
            DatePickerDialog datePick = new DatePickerDialog(this, onDateGotPick, date.Year, date.Month - 1, date.Day);
            datePick.Show();

        }

        private void onDateGotPick(object sender, DatePickerDialog.DateSetEventArgs e)//הפעולה מתחילה דיאלוג של תאריך ומציבה את התוצאה בשם הכפתור
        {
            Got = e.Date;
            btnTimeGot.Text = Got.ToString();
            
        }

         async void LoadInfoFromWikipedia(string plantName)//הפעולה הזאת מביאה את המידע מויקפדיה בכך שיוצרת סרויס ויקפדיה
        {
            var wikipediaService = new WikipediaService();
            try
            {
                 content = await wikipediaService.FetchWikipediaContent(plantName);
                RunOnUiThread(() => tvInfo.Text = content);
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => tvInfo.Text = "Failed to load content.");
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)// הפעולה הזאת בודקת אם תוצאה של אקטיביטי היא של צילום תמונה ואם כן היא מתחילה את בדיקת הצמח
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == IdentifyButtonManager.REQUEST_IMAGE_CAPTURE && resultCode == Result.Ok)
            {
                p = ProgressDialog.Show(this, "Identifying", "Please Wait...", true);
                p.SetCancelable(false);
                IdentifyButtonManager.HandleActivityResult(requestCode, resultCode, data, this, p);
            }
        }
        private async void LoadImage(string plantName)//מפעיל את הפעולה של טעינת תמונה מהאינטרנט של הצמח שנמצא
        {
            progressDialog.Show();
            try
            {
                var imageData = await imageSearch.SearchForImages(plantName);
                if (imageData != null && imageData.Items.Count > 0)
                {
                    string imageUrl = imageData.Items[0].Link;

                    int targetWidth = ivImage.LayoutParameters.Width;
                    int targetHeight = ivImage.LayoutParameters.Height;
                    Console.WriteLine(imageUrl);

                    Action onSuccess = () =>
                    {
                        progressDialog.Dismiss();
                    };

                    ImageFoundUrl = imageUrl;
                    Picasso.With(this).Load(imageUrl)
                        .Resize(targetWidth, targetHeight) 
                        .Into(ivImage, new CustomCallback(onSuccess));
                }
            }
            catch (Exception ex)
            {
                progressDialog.Dismiss();
            }
        }
        public override void OnBackPressed()//מונע מאנשים לעשות סיום לאינטנט ובמקום יוצר אינטנט חדש שמחזיר למסך הראשי כדי שלא יגרמו בעיות
        {
            base.OnBackPressed();
            Intent i = new Intent(this,typeof(MainActivity));
            StartActivity(i);
        }


    }
}
