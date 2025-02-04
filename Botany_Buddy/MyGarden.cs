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

namespace Botany_Buddy
{
    public class MyGarden
    {
        public List <OwnedPlant> MyPlants { get; set; }
        
        public MyGarden() 
        {
            MyPlants = new List <OwnedPlant>();

        }
        public void AddPlantToGardenMain(Plant plant, DateTime age, int size, int leafSize)//מוסיף צמח לגינה של המשתמש הכללי ואז מפעיל את הפעולה עדכון משתמש
        {

            OwnedPlant NewPlant = new OwnedPlant(plant.GetName(), plant.GetImageUrl(),plant.ImageFoundUrl, age, size,  leafSize);
            NewPlant.setNumber();
            MyPlants.Add(NewPlant);
            MainItemsActivity.user.UpdateUser();

        }
        public void AddPlantToGarden(Plant plant, DateTime age, int size, int leafSize)//מוסיף צמח לגינה מבלי לעדכן את המשתמש
        {

            OwnedPlant NewPlant = new OwnedPlant(plant.GetName(),plant.GetImageUrl(),plant.ImageFoundUrl,age,size,leafSize);
            MyPlants.Add(NewPlant);

        }
        public List<OwnedPlant> getMyPlants()
        {
            return MyPlants;
        }
    }
}