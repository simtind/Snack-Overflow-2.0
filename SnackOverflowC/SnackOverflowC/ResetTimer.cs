using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SnackOverflowC
{
    class ResetTimer
    {
        private DispatcherTimer timer;
        

        public event EventHandler ThresholdReached;
        public event EventHandler TimeChanged;
        public int countdown;
        public int threshold;


        public ResetTimer(int threshold)
        {
            this.threshold = threshold;
            countdown = threshold;

            timer = new DispatcherTimer();
            timer.Tick += resetTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        public void startTimer()
        {
            timer.Start();
        }
        public void stopTimer()
        {
            timer.Stop();
        }
        public void resetTimer()
        {
            countdown = threshold;
        }
        private void resetTimer_Tick(object sender, EventArgs e)
        {
            
            
            //fire event to update the timer graphic
            if (countdown == 0)
            {
                stopTimer();
                resetTimer();
                OnThresholdReached(EventArgs.Empty);
            }
            else
                OnTimeChanged(EventArgs.Empty);

            countdown--;
        }

        protected virtual void OnThresholdReached(EventArgs e)
        {
            EventHandler handler = ThresholdReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnTimeChanged(EventArgs e)
        {
            EventHandler handler = TimeChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        







    }
}
