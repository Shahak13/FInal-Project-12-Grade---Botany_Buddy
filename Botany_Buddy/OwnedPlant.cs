using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Botany_Buddy.Classes;
using Botany_Buddy.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using static Android.Util.EventLogTags;
using static Java.Util.Jar.Attributes;

namespace Botany_Buddy
{
    public class OwnedPlant : Plant
    {
        public DateTime Age { get; set; }
        public int Size { get; set; }
        public DateTime LastWatering { get; set; }
        public DateTime LastRepot { get; set; }
        public DateTime LastFertilization { get; set; }
        public int LeafSize { get; set; }
         public bool InData { get; set; }  
        public PlantNotifications FertelizeNot { get; set; }
        public PlantNotifications RepotNot { get; set; }
        public PlantNotifications WaterNot { get; set; }


        public OwnedPlant( string name,  string imageUrl,string imageFoundUrl) : base( name,  imageUrl,imageFoundUrl)
        {
            int Num = MainItemsActivity.user.myGarden.MyPlants.Count;
            FertelizeNot = new PlantNotifications(Num*10+1);
            RepotNot = new PlantNotifications(Num * 10 + 2);
            WaterNot = new PlantNotifications(Num * 10 + 3);
            
        }
        public OwnedPlant( string name, string imageUrl,string imageFoundUrl,  DateTime age, int size, int leafSize) : base( name, imageUrl, imageFoundUrl)
        {
            Age = age;
            Size = size;
            LeafSize = leafSize;
            
        }
        public int AgeCalc()//מחשב כמה זמן יש לך כבר את הצמח בימים
        {
            DateTime today = DateTime.Now;
            TimeSpan timeDiff = today - LastWatering;
            return timeDiff.Days;
        }
        public void UpdateSize(int NewSize)
        {
            Size = NewSize;

        }
        public void water()//מאפס את ההשקייה
        {
            LastWatering = DateTime.Now;
        }
        public void Repot()//מאפס את השתילה מחדש
        {
            LastRepot = DateTime.Now;
        }
        public void UpdateLeafSize(int NewLeafSize)//מעדכן את גודל העלה לגודל העכשיוי
        {
            LeafSize = NewLeafSize;
        }

        internal void setNumber()
        {
            int Num;
            if (MainItemsActivity.user.myGarden.MyPlants.Any())
            {
                Num = MainItemsActivity.user.myGarden.MyPlants.Count;

            }
            else
            {
                Num = 0;
            }
            FertelizeNot = new PlantNotifications(Num * 10 + 1);
            RepotNot = new PlantNotifications(Num * 10 + 2);
            WaterNot = new PlantNotifications(Num * 10 + 3);
        }
        public void setNotifiactions(int Num)
        {
            
            FertelizeNot = new PlantNotifications(Num * 10 + 1);
            RepotNot = new PlantNotifications(Num * 10 + 2);
            WaterNot = new PlantNotifications(Num * 10 + 3);
        }
    }
}