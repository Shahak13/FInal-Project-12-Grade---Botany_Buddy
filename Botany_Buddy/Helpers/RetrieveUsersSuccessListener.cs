using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botany_Buddy.Helpers
{
    [Activity(Label = "RetrieveUsersSuccessListener")]
    public class RetrieveUsersSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        
        private Action<List<User>> callback;

        public RetrieveUsersSuccessListener(Action<List<User>> callback)
        {
            this.callback = callback;
        }
        public void OnSuccess(Java.Lang.Object result)
        {
            List<User> users = new List<User>();

            QuerySnapshot querySnapshot = (QuerySnapshot)result;

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                if (documentSnapshot.Exists())
                {
                    IDictionary<string, Java.Lang.Object> userData = documentSnapshot.Data;
                    User user = new User
                    {
                        ID = userData["ID"].ToString(),
                        Name = userData["Name"].ToString(),
                        Password = userData["Password"].ToString(),
                        // You may need to load the user's garden here if necessary
                    };
                    users.Add(user);
                }
            }

            callback?.Invoke(users);
        }
    }
}