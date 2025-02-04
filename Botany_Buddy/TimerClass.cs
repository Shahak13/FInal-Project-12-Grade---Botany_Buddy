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
using System.Threading;

namespace Botany_Buddy
{
    internal class TimerClass
    {
        private int counter;
        private TimerHandler handler;
        private int totalSeconds;
        private bool stop;

        public TimerClass(TimerHandler handler, int totalSeconds)
        {
            this.counter = 0;
            this.handler = handler;
            this.totalSeconds = totalSeconds;
        }

        public void Start()
        {
            stop = false;
            ThreadStart threadStart = new ThreadStart(Run);
            Thread t = new Thread(threadStart);
            t.Start();
        }

        public void Stop()
        {
            stop = true;
        }

        private void Run()
        {
            while (counter < totalSeconds && !stop)
            {
                counter++;
                Thread.Sleep(1000);
                int remainingTime = totalSeconds - counter;
                int progress = (counter * 100) / totalSeconds;
                Message msg = new Message
                {
                    Arg1 = remainingTime,
                    Arg2 = progress
                };
                handler.SendMessage(msg);
            }
        }
    }
}