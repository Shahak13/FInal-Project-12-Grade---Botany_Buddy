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
using static Xamarin.Grpc.LoadBalancer;

namespace Botany_Buddy.Adapters
{
    internal class PlantAdapter : BaseAdapter<OwnedPlant>
    {

        Context context;
        List<OwnedPlant> lstPlants;

        public PlantAdapter(Context context, List<OwnedPlant> lstPlants)
        {
            this.context = context;
            this.lstPlants = lstPlants;
        }
        public List<OwnedPlant> GetList()
        {
            return lstPlants;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((MyGardenActivity)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.MyGardenListView, parent, false);
            ImageView ivImage = view.FindViewById<ImageView>(Resource.Id.ivImage);
            TextView tvName = view.FindViewById<TextView>(Resource.Id.tvName);
            TextView tvGotDate = view.FindViewById<TextView>(Resource.Id.tvGotDate);
            ImageView ivThirsty = view.FindViewById<ImageView>(Resource.Id.ivThirsty);
            ImageView ivRepot = view.FindViewById<ImageView>(Resource.Id.ivRepot);

            OwnedPlant temp = lstPlants[position];
            if (temp != null)
            {
                tvName.Text = temp.Name;
                tvGotDate.Text = "Since: " + temp.Age.Year+"/"+temp.Age.Month+"/"+temp.Age.Day;
                ivImage.SetImageBitmap(ImageConverter.stringToBitmap(temp.GetImageUrl()));
                

            }
            return view;
        }


        public override int Count
        {
            get { return lstPlants.Count; }
        }
        public override OwnedPlant this[int position]
        {
            get { return lstPlants[position]; }
        }
        public void SetList(List<OwnedPlant> plants)
        {
            this.lstPlants = plants;
        }
        internal class PlantAdapterViewHolder : Java.Lang.Object
        {
            //Your adapter views to re-use
            //public TextView Title { get; set; }
        }
    }
}