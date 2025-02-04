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

namespace Botany_Buddy.Classes
{
    public class PlantNotifications
    {
        public bool On { get; set; }
        public int DaysBetweenNotification { get; set; }
        public int notificationId {  get; private set; }
        public PlantNotifications(int notificationId)
        {
            On = false;
            DaysBetweenNotification = -1;
            this.notificationId = notificationId;
        }
        public void OnOff()
        {
            this.On = !this.On;
        }
        public void changeDays(int daysBetweenNotification)
        {
            this.DaysBetweenNotification = daysBetweenNotification;
        }
        public void SetAlarm(Context context, long triggerAtMillis)
        {

            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            Intent intent = new Intent(context, typeof(Notification_BroadcastReciver));
            intent.PutExtra("NOTIFICATION_ID", notificationId); 

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, notificationId, intent, PendingIntentFlags.OneShot | PendingIntentFlags.Immutable);

            alarmManager.Set(AlarmType.RtcWakeup, triggerAtMillis, pendingIntent);
        }
    }
}