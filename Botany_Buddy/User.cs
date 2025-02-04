using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Botany_Buddy.Helpers;
using Firebase.Firestore;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace Botany_Buddy
{
    public class User
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public MyGarden myGarden { get; set; }

        public User(string Name,string Password) 
        {
            this.Name = Name;
            this.Password = Password;
            ID = Guid.NewGuid().ToString();
            myGarden = new MyGarden();

        }
        public void ChangeName(string NewName)
        {
            this.Name = NewName;
        }
        public User() { }
        public void LoadUser(string ID, Action callback)//מפעיל פעולה שמייבאת את הנתונים ממסד הנתונים
        {
            FirebaseFirestore database = FireStoreHelper.getDatabase();
            DocumentReference reference = database.Collection("User").Document(ID);

            reference.Get().AddOnSuccessListener(new LoadUserSuccessListener(this, callback));
        }

        public void UpdateUser()//מעדכן את המשתמש הכללי במסד הנתונים
        {
            int i = 0;
            FirebaseFirestore database = FireStoreHelper.getDatabase();
            DocumentReference reference = database.Collection("User").Document(this.ID);
            HashMap MyGardenMap = new HashMap();

            foreach (OwnedPlant ownedPlant in myGarden.getMyPlants())
            {
                HashMap OwnedPlantMap = new HashMap();
                OwnedPlantMap.Put("Name", ownedPlant.GetName());
                OwnedPlantMap.Put("Size", ownedPlant.Size);
                OwnedPlantMap.Put("LeafSize", ownedPlant.LeafSize);
                OwnedPlantMap.Put("ImageUrl", ownedPlant.GetImageUrl());
                OwnedPlantMap.Put("ImageFoundUrl", ownedPlant.ImageFoundUrl);
                OwnedPlantMap.Put("Age", ownedPlant.Age.ToString());
                Console.WriteLine(ownedPlant.GetImageUrl().Length);
                OwnedPlantMap.Put("Num", ownedPlant.FertelizeNot.notificationId / 10);

                if (ownedPlant.LastWatering != DateTime.MinValue)
                {
                    OwnedPlantMap.Put("LastWatering", ownedPlant.LastWatering.ToString());
                }
                if (ownedPlant.LastRepot != DateTime.MinValue)
                {
                    OwnedPlantMap.Put("LastRepot", ownedPlant.LastRepot.ToString());
                }
                if (ownedPlant.LastFertilization != DateTime.MinValue)
                {
                    OwnedPlantMap.Put("LastFertilization", ownedPlant.LastFertilization.ToString());
                }

                OwnedPlantMap.Put("FertilizeNotificationOn", ownedPlant.FertelizeNot.On);
                if (ownedPlant.FertelizeNot.DaysBetweenNotification != -1)
                {
                    OwnedPlantMap.Put("FertilizeNotDays", ownedPlant.FertelizeNot.DaysBetweenNotification);
                }

                OwnedPlantMap.Put("RepotNotificationOn", ownedPlant.RepotNot.On);
                if (ownedPlant.RepotNot.DaysBetweenNotification != -1)
                {
                    OwnedPlantMap.Put("RepotNotDays", ownedPlant.RepotNot.DaysBetweenNotification);
                }

                OwnedPlantMap.Put("WaterNotificationOn", ownedPlant.WaterNot.On);
                if (ownedPlant.WaterNot.DaysBetweenNotification != -1)
                {
                    OwnedPlantMap.Put("WaterNotDays", ownedPlant.WaterNot.DaysBetweenNotification);
                }
                MyGardenMap.Put("OwnedPlant" + i, OwnedPlantMap);
                i++;
            }
            //foreach (OwnedPlant ownedPlant in MainItemsActivity.user.myGarden.getMyPlants())

            HashMap UserMap = new HashMap();
            UserMap.Put("ID", this.ID);
            UserMap.Put("Name", this.Name);
            UserMap.Put("Password", this.Password);
            UserMap.Put("MyGarden", MyGardenMap);
            reference.Set(UserMap);

        }





        public void SaveToDatabase()//מוסיף את המשתמש הכללי למסד הנתונים
        {
            int i = 0;
            FirebaseFirestore database = FireStoreHelper.getDatabase();
            
            HashMap MyGardenMap = new HashMap();
            
            foreach (OwnedPlant ownedPlant in myGarden.getMyPlants())
            {
                HashMap OwnedPlantMap = new HashMap();
                OwnedPlantMap.Put("Name", ownedPlant.GetName());
                OwnedPlantMap.Put("Size", ownedPlant.Size);
                OwnedPlantMap.Put("LeafSize", ownedPlant.LeafSize);
                OwnedPlantMap.Put("ImageUrl", ownedPlant.GetImageUrl());
                OwnedPlantMap.Put("ImageFoundUrl", ownedPlant.ImageFoundUrl);
                OwnedPlantMap.Put("Age", ownedPlant.Age.ToString());
                OwnedPlantMap.Put("Num", ownedPlant.FertelizeNot.notificationId / 10);

                if (ownedPlant.LastWatering != DateTime.MinValue)
                {
                    OwnedPlantMap.Put("LastWatering", ownedPlant.LastWatering.ToString());
                }
                if (ownedPlant.LastRepot != DateTime.MinValue)
                {
                    OwnedPlantMap.Put("LastRepot", ownedPlant.LastRepot.ToString());
                }
                if (ownedPlant.LastFertilization != DateTime.MinValue)
                {
                    OwnedPlantMap.Put("LastFertilization", ownedPlant.LastFertilization.ToString());
                }

                OwnedPlantMap.Put("FertilizeNotificationOn", ownedPlant.FertelizeNot.On);
                if (ownedPlant.FertelizeNot.DaysBetweenNotification != -1)
                {
                    OwnedPlantMap.Put("FertilizeNotDays", ownedPlant.FertelizeNot.DaysBetweenNotification);
                }

                OwnedPlantMap.Put("RepotNotificationOn", ownedPlant.RepotNot.On);
                if (ownedPlant.RepotNot.DaysBetweenNotification != -1)
                {
                    OwnedPlantMap.Put("RepotNotDays", ownedPlant.RepotNot.DaysBetweenNotification);
                }

                OwnedPlantMap.Put("WaterNotificationOn", ownedPlant.WaterNot.On);
                if (ownedPlant.WaterNot.DaysBetweenNotification != -1)
                {
                    OwnedPlantMap.Put("WaterNotDays", ownedPlant.WaterNot.DaysBetweenNotification);
                }
                MyGardenMap.Put("OwnedPlant" + i, OwnedPlantMap);
                i++;
            }
            HashMap UserMap = new HashMap();
            UserMap.Put("ID",this.ID);
            UserMap.Put("Name",this.Name);
            UserMap.Put("Password",this.Password);
            UserMap.Put("MyGarden", MyGardenMap);
            DocumentReference reference = database.Collection("User").Document(this.ID);
            reference.Set(UserMap);

        }
    }

    
}