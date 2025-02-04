using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Firebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botany_Buddy.Helpers
{
    public class FireStoreHelper
    {
        public static FirebaseFirestore getDatabase()//מוצא את מסד הנתונים ומחזיר אותו
        {
            FirebaseFirestore database;
            var app = FirebaseApp.InitializeApp(Application.Context);
            if (app == null)
            {
                var option = new FirebaseOptions.Builder()
                .SetProjectId("botany-buddy-8f14e")
                .SetApplicationId("botany-buddy-8f14e")
                .SetApiKey("AIzaSyBojiOf2KByaVqYGZIMeZf0TQvdWiZC26o")
                .SetStorageBucket("botany-buddy-8f14e.appspot.com")
                .Build();
                app = FirebaseApp.InitializeApp(Application.Context, option);
            }
            database = FirebaseFirestore.GetInstance(app);
            return database;



            

        }


        

    }
}


