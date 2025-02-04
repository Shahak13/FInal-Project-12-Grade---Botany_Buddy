using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using System;
using System.Collections.Generic;
using Botany_Buddy.Adapters;
using Botany_Buddy.Helpers;
using Botany_Buddy.Activities;

namespace Botany_Buddy
{
    [Activity(Label = "My Garden")]
    public class MyGardenActivity : Activity
    {
        ListView lvMyGarden;
        PlantAdapter plantAdapter;
        List<OwnedPlant> ownedPlants;
        ImageButton btnHome;
        ImageButton btnIdentify;
        private ProgressDialog p;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MyGarden_layout);

            lvMyGarden = FindViewById<ListView>(Resource.Id.lvMyGarden);
            btnHome = (ImageButton)FindViewById(Resource.Id.btnHome);
            btnIdentify = (ImageButton)FindViewById(Resource.Id.btnIdentify);

               ownedPlants = MainItemsActivity.user.myGarden.getMyPlants();

            

            lvMyGarden.Adapter = plantAdapter;
            startListView();

            lvMyGarden.ItemClick += LvMyGarden_ItemClick;

            lvMyGarden.ItemLongClick += LvMyGarden_ItemLongClick;
            btnHome.Click += BtnHome_Click;
            btnIdentify.Click += (sender, e) =>
            {
                IdentifyButtonManager.TakePicture(this);
            };
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

        private void BtnHome_Click(object sender, EventArgs e)//כפתור שמעביר אותך למסך הראשי
        {
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }

        private void startListView()//מגדיר את הליסטויו
        {
            plantAdapter = new PlantAdapter(this, ownedPlants);
            lvMyGarden.Adapter = plantAdapter;
        }

        private void LvMyGarden_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent i = new Intent(this, typeof(ShowItemActivity));
            i.PutExtra("pos", e.Position);
            StartActivity(i);
        }

        private void LvMyGarden_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
        }
        protected override void OnResume()
        {
            base.OnResume();
            plantAdapter.NotifyDataSetChanged();
        }


    }
}
