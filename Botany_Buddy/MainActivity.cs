using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.Content.PM;
using Android.Content;
using Botany_Buddy.Helpers;
using Android.Preferences;
using Firebase.Firestore;
using Android.Runtime;
using System;
using Android.Views;
using Javax.Security.Auth.Login;
using System.Threading;
using System.Collections.Generic;

namespace Botany_Buddy
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ImageButton btnHome, btnIdentify, btnMyGarden, btnHistory;
        Button btnSubmit;
        ToggleButton tbtnSL;
        CheckBox cbStayLoggedIn;
        EditText etName,etPassword;
        ProgressDialog p;
        Dialog dialog, CameraOrGalleryDialog;
        ISharedPreferences sharedPreferences;
        public static string ID;
        bool Connected;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            btnHome = FindViewById<ImageButton>(Resource.Id.btnHome);
            btnIdentify = FindViewById<ImageButton>(Resource.Id.btnIdentify);
            btnMyGarden = FindViewById<ImageButton>(Resource.Id.btnMyGarden);
            btnHistory = FindViewById<ImageButton>(Resource.Id.btnHistory);
            Connected = false;

            sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);

            RequestCameraPermission();
            Console.WriteLine(sharedPreferences.GetString("ID", null));    
            if (sharedPreferences.GetString("ID", null) == null)//בודק אם התז לא קקים במערכת
            {
                NewUser();
            }
            else
            {
                LoadUser();
            }
            btnIdentify.Click += BtnIdentify_Click;

            

            btnMyGarden.Click += BtnMyGarden_Click1;
        }

        private void BtnIdentify_Click(object sender, EventArgs e)
        {
            CameraOrGalleryDialog = new Dialog(this);
            CameraOrGalleryDialog.SetContentView(Resource.Layout.GallaryOrCameraDialog);
            ImageButton btnCamera = (ImageButton)CameraOrGalleryDialog.FindViewById(Resource.Id.btnCamera);
            ImageButton btnGallery = (ImageButton)CameraOrGalleryDialog.FindViewById(Resource.Id.btnGallery);
            CameraOrGalleryDialog.Show();
            btnCamera.Click += BtnCamera_Click;
            btnGallery.Click += BtnGallery_Click;
        }

        private void BtnGallery_Click(object sender, EventArgs e)
        {
            IdentifyButtonManager.ChoosePicture(this);
        }

        private void BtnCamera_Click(object sender, EventArgs e)
        {
            IdentifyButtonManager.TakePicture(this);
        }

        private void BtnMyGarden_Click1(object sender, System.EventArgs e)//מעביר לאקטיביטי הגינה שלי
        {
            Intent i = new Intent(this, typeof(MyGardenActivity));
            StartActivity(i);
        }

        private void LoadUser()
        {
            p = ProgressDialog.Show(this, "Loading", "Please Wait...", true);
            p.SetCancelable(false);
            p.Show();

            string userID = sharedPreferences.GetString("ID", null);
            if (userID != null)
            {
                MainItemsActivity.user = new User();
                MainItemsActivity.user.LoadUser(userID, () =>
                {
                    RunOnUiThread(() => p.Dismiss());
                });
                Connected = true;
            }
            else
            {
                p.Dismiss();
            }
        }

        private void NewUser()//בגלל שהמשתמש הכללי לק קיים אז הוא מפעיל דיאלוג שלוקח את השם וכשלוחצים על שלח זה מפעיל את הפעולה של כפתור השליחה 
        {
            dialog = new Dialog(this);
            dialog.SetContentView(Resource.Layout.DialogName);
            dialog.SetCancelable(false);
            btnSubmit = dialog.FindViewById<Button>(Resource.Id.btnSubmit);
            etName = dialog.FindViewById<EditText>(Resource.Id.etUsername);
            etPassword = dialog.FindViewById<EditText>(Resource.Id.etPassword);
            tbtnSL = dialog.FindViewById<ToggleButton>(Resource.Id.toggle_mode);

            

            dialog.Show();
            btnSubmit.Click += BtnSubmit_Click;
        }

        private void BtnSubmit_Click(object sender, System.EventArgs e)//יוצר משתמש כללי חדש עם השם מהדיאלוג ותז רנדומלי וגינה ריקה
        {

            User user = new User(etName.Text,etPassword.Text);
            MainItemsActivity.user = user;
            ISharedPreferencesEditor editor = sharedPreferences.Edit();
            editor.PutString("ID", user.ID);
            editor.Commit();
            ID = MainItemsActivity.user.ID;
            

            MainItemsActivity.user.SaveToDatabase();
            dialog.Dismiss();
            Connected = true;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)//מקבל את התמונה שנשלחה מכפתור הזיהוי 
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1 && resultCode == Result.Ok)
            {
                p = ProgressDialog.Show(this, "Identifying", "Please Wait...", true);
                p.SetCancelable(false);
                IdentifyButtonManager.HandleActivityResult(requestCode, resultCode, data, this, p);
            }
            if(requestCode == 2 && resultCode == Result.Ok)
            {
                p = ProgressDialog.Show(this, "Identifying", "Please Wait...", true);
                p.SetCancelable(false);

                IdentifyButtonManager.HandleActivityResult(requestCode, resultCode, data, this, p);
                
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)//אם יש אישור לבקשה זה מציב את זה
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void RequestCameraPermission()//מבקש רשות לשימוש במצלמה
        {
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.Camera) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Android.Manifest.Permission.Camera }, 0);
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.optionsMenu, menu);
            if (!Connected)
            {
                menu.FindItem(Resource.Id.action_logout).SetEnabled(false);
            }
            if(Connected)
            {
                menu.FindItem(Resource.Id.action_login).SetEnabled(false);
            }
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Resource.Id.action_login)
            {
                Login();
                return true;
            }
            else if (item.ItemId == Resource.Id.action_logout)
            {
                Logout(); 
                return true;
            }
            else if(item.ItemId == Resource.Id.action_changename)
            {
                ChangeName();
                return true;
            }
            else
            {
                return base.OnOptionsItemSelected(item);

            }
        }

        private void ChangeName()
        {
            dialog = new Dialog(this);
            dialog.SetContentView(Resource.Layout.DialogName);
            dialog.SetCancelable(false);
            btnSubmit = dialog.FindViewById<Button>(Resource.Id.btnSubmit);
            etName = dialog.FindViewById<EditText>(Resource.Id.etUsername);
            etPassword = dialog.FindViewById<EditText>(Resource.Id.etPassword);
            etPassword.Visibility = ViewStates.Invisible;

            dialog.Show();
            btnSubmit.Click += BtnSubmit_Click2;
            
        }

        private void BtnSubmit_Click2(object sender, EventArgs e)
        {
            MainItemsActivity.user.ChangeName(etName.Text);
            MainItemsActivity.user.UpdateUser();
            dialog.Dismiss();
        }

        private void Logout()
        {
            MainItemsActivity.user = null;
            ISharedPreferencesEditor editor = sharedPreferences.Edit();
            
            editor.PutString("ID", null);
            editor.Commit();
            Connected = false;
        }

        private void Login()
        {
            dialog = new Dialog(this);
            dialog.SetContentView(Resource.Layout.DialogName);
            dialog.SetCancelable(false);
            btnSubmit = dialog.FindViewById<Button>(Resource.Id.btnSubmit);
            etName = dialog.FindViewById<EditText>(Resource.Id.etUsername);
            etPassword = dialog.FindViewById<EditText>(Resource.Id.etPassword);


            dialog.Show();
            btnSubmit.Click += BtnSubmit_Click1;
        }

        private void BtnSubmit_Click1(object sender, EventArgs e)
        {
            string LoginName = etName.Text;
            string LoginPassword = etPassword.Text;

            FirebaseFirestore database = FireStoreHelper.getDatabase();
            database.Collection("User").Get().AddOnSuccessListener(new RetrieveUsersSuccessListener(users =>
            {
                foreach (User user in users)
                {
                    if (user.Name.Equals(LoginName) && user.Password.Equals(LoginPassword))
                    {
                        p = ProgressDialog.Show(this, "Loading", "Please Wait...", true);
                        p.SetCancelable(false);
                        p.Show();

                        string userID = user.ID;
                        ISharedPreferencesEditor editor = sharedPreferences.Edit();
                        editor.PutString("ID", userID);
                        editor.Commit();
                        if (userID != null)
                        {
                            MainItemsActivity.user = new User();
                            MainItemsActivity.user.LoadUser(userID, () =>
                            {
                                RunOnUiThread(() => p.Dismiss());
                            });
                        }

                        dialog.Dismiss();
                        Connected = true;
                        return;
                    }
                }

                // Handle invalid login
                Toast.MakeText(this, "Invalid username or password", ToastLength.Long).Show();
            }));
        }





    }
}
