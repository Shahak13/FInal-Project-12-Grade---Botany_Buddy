
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Firebase.Firestore.Model;
using Java.Interop;
using Java.Lang.Reflect;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botany_Buddy.Helpers
{
    public class LoadUserSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private readonly User user;
        private Action callback;

        public LoadUserSuccessListener(User user, Action callback)
        {
            this.user = user;
            this.callback = callback;
        }

        public void OnSuccess(Java.Lang.Object result)//אם נמצא הפריט במסד הנתונים הוא שולך את כל הנתונים לתוך המשתמש הכללי
        {
            DocumentSnapshot documentSnapshot = (DocumentSnapshot)result;

            if (documentSnapshot.Exists())
            {
                IDictionary<string, Java.Lang.Object> userData = documentSnapshot.Data;

                user.ID = userData["ID"].ToString();
                user.Name = userData["Name"].ToString();
                user.Password = userData["Password"].ToString();
                MyGarden myGarden = new MyGarden();

                if (userData.ContainsKey("MyGarden"))
                {
                    var thisDate = userData["MyGarden"];
                    var dictionaryFromHashmap = new JavaDictionary<string, JavaDictionary>(thisDate.Handle, JniHandleOwnership.DoNotRegister);
                    foreach (KeyValuePair<string, JavaDictionary> OwnedPlant1 in dictionaryFromHashmap)
                    {
                        JavaDictionary propertyData = OwnedPlant1.Value;
                        Plant plant = new Plant();
                        plant.Name = propertyData["Name"].ToString();
                        plant.ImageUrl = propertyData["ImageUrl"].ToString();
                        plant.ImageFoundUrl = propertyData["ImageFoundUrl"].ToString();
                        DateTime Age = Convert.ToDateTime(propertyData["Age"].ToString());
                        

                        int Size = int.Parse(propertyData["Size"].ToString());
                        int LeafSize = int.Parse(propertyData["LeafSize"].ToString());
                        myGarden.AddPlantToGarden(plant, Age, Size, LeafSize);
                        int Num = int.Parse(propertyData["Num"].ToString());
                        myGarden.MyPlants[myGarden.MyPlants.Count-1].setNotifiactions(Num);
                        int i = myGarden.MyPlants.Count-1;
                        if (propertyData.Contains("LastWatering"))
                        {
                            DateTime LastWatering = Convert.ToDateTime(propertyData["LastWatering"].ToString());
                            myGarden.MyPlants[i].LastWatering = LastWatering;

                        }
                        if (propertyData.Contains("LastRepot"))
                        {
                            DateTime LastRepot = Convert.ToDateTime(propertyData["LastRepot"].ToString());
                            myGarden.MyPlants[i].LastRepot = LastRepot;
                        }

                    }
                    myGarden.MyPlants.Reverse();
                    user.myGarden = myGarden;
                    MainItemsActivity.user = user;
                }
                else
                {
                }
                
            }
            else
            {
            }
            callback?.Invoke();
        }


    }
}

