using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Botany_Buddy.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Square.Picasso;
using Android.Preferences;
using AndroidX.Core.App;
using Android.Widget;
using Java.Lang;
using System;
using System.Threading;
using Java.Util.Logging;

namespace Botany_Buddy.Activities
{
    [Activity(Label = "ShowItemActivity")]
    public class ShowItemActivity : Activity
    {
        ImageView iv1;
        ImageView iv2;
        ImageButton btnBack;
        ImageButton btnWater, btnGear;
        Button btnClose;
        Button btnSubmitDate;
        Button btnSelectDate;
        Button btnStart,btnJustWater,btnCancel;
        EditText etHours,etMinutes,etSeconds;
        TextView tvName;
        TextView tvInfo,tvTime;
        Switch swWatering;
        Switch swRepoting;
        Switch swFertelizing;
        EditText etDays;
        CheckBox cbNotification;
        ProgressBar pb1;
        int pos,PbDoubleProgress;
        double change;
        ProgressDialog progressDialog;
        public static string Info;
        Dialog dialog,dialogTime,dialogTimerStarted;


        DateTime Fertilize;

        int From;




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShowItem_layout);
            iv1 = (ImageView)FindViewById(Resource.Id.ivImage);
            iv2 = (ImageView)FindViewById(Resource.Id.ivImage2);
            btnBack = (ImageButton)FindViewById(Resource.Id.btnBack);
            btnGear = (ImageButton)FindViewById(Resource.Id.btnGear);
            btnWater = (ImageButton)FindViewById(Resource.Id.btnWater);
            tvName = (TextView)FindViewById(Resource.Id.tvName);
            tvInfo = (TextView)FindViewById(Resource.Id.tvInfo);
            progressDialog = new ProgressDialog(this);
            progressDialog.SetMessage("Loading...");
            progressDialog.SetCancelable(false);
            pos = Intent.GetIntExtra("pos", 0);

            CreateNotificationChannel();
            CheckAndRequestNotificationPermission();
            LoadInto();
            btnBack.Click += BtnBack_Click;
            btnGear.Click += BtnGear_Click;
            btnWater.Click += BtnWater_Click;

        }

        private void BtnWater_Click(object sender, EventArgs e)
        {
            dialogTime = new Dialog(this);
            dialogTime.SetContentView(Resource.Layout.DialogHowMuchTime); // Ensure you set the content view here
            btnStart = (Button)dialogTime.FindViewById(Resource.Id.btnStart);
            btnJustWater = (Button)dialogTime.FindViewById(Resource.Id.btnJustWater);
            btnCancel = (Button)dialogTime.FindViewById(Resource.Id.btnCancel);
            etHours = (EditText)dialogTime.FindViewById(Resource.Id.etHours);
            etMinutes = (EditText)dialogTime.FindViewById(Resource.Id.etMinutes);
            etSeconds = (EditText)dialogTime.FindViewById(Resource.Id.etSecondes);
            btnStart.Click += BtnStart_Click;
            dialogTime.Show();
        }


        private void BtnStart_Click(object sender, EventArgs e)
        {
            dialogTimerStarted = new Dialog(this);
            dialogTimerStarted.SetContentView(Resource.Layout.DialogTimer); // Ensure the dialog content is set
            tvTime = (TextView)dialogTimerStarted.FindViewById(Resource.Id.tvTime);
            pb1 = (ProgressBar)dialogTimerStarted.FindViewById(Resource.Id.pb1);
            pb1.Max = 100; // Set the max value of the ProgressBar to 100

            int Seconds = 0;
            if (!string.IsNullOrEmpty(etSeconds.Text))
            {
                Seconds = int.Parse(etSeconds.Text);
            }

            int Minutes = 0;
            if (!string.IsNullOrEmpty(etMinutes.Text))
            {
                Minutes = int.Parse(etMinutes.Text);
            }

            int Hours = 0;
            if (!string.IsNullOrEmpty(etHours.Text))
            {
                Hours = int.Parse(etHours.Text);
            }

            int totalSeconds = (Hours * 3600) + (Minutes * 60) + Seconds;

            if (totalSeconds == 0)
            {
                Toast.MakeText(this, "Please enter a valid time", ToastLength.Short).Show();
                return;
            }

            tvTime.Text = Hours+":"+Minutes+":"+Seconds;

            TimerHandler handler = new TimerHandler(tvTime, pb1);
            TimerClass timer = new TimerClass(handler, totalSeconds);
            timer.Start();

            dialogTimerStarted.Show();
        }




        private void CheckAndRequestNotificationPermission()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                if (!NotificationManagerCompat.From(this).AreNotificationsEnabled())
                {
                    ActivityCompat.RequestPermissions(this, new string[] { Android.Manifest.Permission.PostNotifications }, 0);
                }
            }
        }

        private void BtnGear_Click(object sender, EventArgs e)
        {
            Dialog d = new Dialog(this);
            d.SetContentView(Resource.Layout.GearDialog);
            swWatering = (Switch)d.FindViewById(Resource.Id.swWatering);
            swRepoting = (Switch)d.FindViewById(Resource.Id.swRepoting);
            swFertelizing = (Switch)d.FindViewById(Resource.Id.swFertelizing);
            btnClose = (Button)d.FindViewById(Resource.Id.btnClose);
            swFertelizing.Checked = MainItemsActivity.user.myGarden.MyPlants[pos].FertelizeNot.On;
            swWatering.Checked = MainItemsActivity.user.myGarden.MyPlants[pos].WaterNot.On;
            swRepoting.Checked = MainItemsActivity.user.myGarden.MyPlants[pos].RepotNot.On;
            swFertelizing.CheckedChange += SwFertelizing_CheckedChange;
            d.Show();

        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                string channelId = "fertilization_reminder_channel";
                string channelName = "Plant Care Notifications";
                string channelDescription = "Notifications for plant care activities";

                NotificationImportance importance = NotificationImportance.Default;

                NotificationChannel channel = new NotificationChannel(channelId, channelName, importance)
                {
                    Description = channelDescription
                };

                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }

        private void SwFertelizing_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (swFertelizing.Checked)
            {
                From = 0;

                MainItemsActivity.user.myGarden.MyPlants[pos].FertelizeNot.OnOff();

                dialog = new Dialog(this);
                dialog.SetContentView(Resource.Layout.SelectDateDialog);

                btnSelectDate = (Button)dialog.FindViewById(Resource.Id.btnSelectDate);
                btnSubmitDate = (Button)dialog.FindViewById(Resource.Id.btnSubmitDate);
                etDays = (EditText)dialog.FindViewById(Resource.Id.etDays);
                cbNotification = (CheckBox)dialog.FindViewById(Resource.Id.cbNotification);
                dialog.Show();
                btnSelectDate.Text = "Select Date Last Fertelized";
                btnSelectDate.Click += BtnSelectDate_Click;
                btnSubmitDate.Click += BtnSubmitDate_Click;
            }
            else
            {
                MainItemsActivity.user.myGarden.MyPlants[pos].FertelizeNot.OnOff();

            }
        }

        private void BtnSubmitDate_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            int days = int.Parse(etDays.Text);
            if (Fertilize != null && etDays != null && cbNotification.Checked)
            {
                long triggerAtMillis = Java.Lang.JavaSystem.CurrentTimeMillis() + (days*1000);// * 24 * 60 * 60 * 1000); 
                MainItemsActivity.user.myGarden.MyPlants[pos].FertelizeNot.SetAlarm(this, triggerAtMillis);
                Toast.MakeText(this, "Notification scheduled in "+ days + " days", ToastLength.Short).Show();
                dialog.Dismiss();
            }

        }

        private void BtnSelectDate_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Today;
            DatePickerDialog datePick = new DatePickerDialog(this, onDateRepotPick, date.Year, date.Month - 1, date.Day);
            datePick.Show();
        }
        



        private void onDateRepotPick(object sender, DatePickerDialog.DateSetEventArgs e)//הפעולה מתחילה דיאלוג של תאריך ומציבה את התוצאה בשם הכפתור
        {
            if (From == 0)
            {
                Fertilize = e.Date;
                btnSelectDate.Text = Fertilize.Day + "/" + Fertilize.Month + "/" + Fertilize.Year;
            }


        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private async void LoadInto()
        {
            progressDialog.Show();
            int targetWidth = iv2.LayoutParameters.Width;
            int targetHeight = iv2.LayoutParameters.Height;
            OwnedPlant ownedPlant = MainItemsActivity.user.myGarden.getMyPlants()[pos];
            iv1.SetImageBitmap(ownedPlant.GetImage());
            Action onSuccess = () =>
            {
                progressDialog.Dismiss();
            };
            tvName.Text = ownedPlant.Name;
            tvInfo.Text = await ownedPlant.GetDescription();
            Picasso.With(this).Load(ownedPlant.ImageFoundUrl)
                        .Resize(targetWidth, targetHeight)
                        .Into(iv2, new CustomCallback(onSuccess));
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }

    }
}
