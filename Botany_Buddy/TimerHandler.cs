using Android.Content;
using Android.OS;
using Android.Widget;

namespace Botany_Buddy
{
    internal class TimerHandler : Handler
    {
        private readonly TextView tvTimer;
        private readonly ProgressBar progressBar;

        public TimerHandler(TextView tvTimer, ProgressBar progressBar)
        {
            this.tvTimer = tvTimer;
            this.progressBar = progressBar;
        }

        public override void HandleMessage(Message msg)
        {
            int remainingTime = msg.Arg1;
            int progress = msg.Arg2;

            int hours = remainingTime / 3600;
            int minutes = (remainingTime % 3600) / 60;
            int seconds = remainingTime % 60;

            tvTimer.Text = hours+ ":" + minutes + ":" + seconds;
            progressBar.Progress = progress;
        }
    }
}