using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using Botany_Buddy.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botany_Buddy.Helpers
{
    [BroadcastReceiver]
    public class Notification_BroadcastReciver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            int pos =intent.GetIntExtra("NOTIFICATION_ID", 0);
            NotificationCompat.Builder builder;
            if (pos%10 == 1) 
            {
                builder = new NotificationCompat.Builder(context, "fertilization_reminder_channel")
                .SetContentTitle("Plant Care Reminder")
                .SetContentText("It's time to Fertelize your plant!")
                .SetSmallIcon(Resource.Mipmap.ic_launcher) // Make sure this is a valid icon
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetVisibility(NotificationCompat.VisibilityPublic);
            }
            else if(pos%10 == 2)
            {
                builder = new NotificationCompat.Builder(context, "fertilization_reminder_channel")
                .SetContentTitle("Plant Care Reminder")
                .SetContentText("It's time to Repot your plant!")
                .SetSmallIcon(Resource.Mipmap.ic_launcher) // Make sure this is a valid icon
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetVisibility(NotificationCompat.VisibilityPublic);
            }
            else if( pos%10 == 3)
            {
                builder = new NotificationCompat.Builder(context, "fertilization_reminder_channel")
                .SetContentTitle("Plant Care Reminder")
                .SetContentText("It's time to water your plant!")
                .SetSmallIcon(Resource.Mipmap.ic_launcher) // Make sure this is a valid icon
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetVisibility(NotificationCompat.VisibilityPublic);
            }
            else
            {
                builder = new NotificationCompat.Builder(context, "fertilization_reminder_channel")
                .SetContentTitle("Plant Care Reminder")
                .SetContentText("It's time to care for your plant!")
                .SetSmallIcon(Resource.Mipmap.ic_launcher) // Make sure this is a valid icon
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetVisibility(NotificationCompat.VisibilityPublic);
            }
            // Build the notification to show when the alarm rings
            

            // Creates an explicit intent for an Activity in your app
            Intent resultIntent = new Intent(context, typeof(ShowItemActivity));
            PendingIntent resultPendingIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            builder.SetContentIntent(resultPendingIntent);

            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(1000, builder.Build()); // ID for notification

            // Optional: if you want to show a toast as well
            Toast.MakeText(context, "Alarm Ringing!", ToastLength.Short).Show();
        }
    }
}