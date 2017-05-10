using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Common.Utilities {
    
    /// <summary>
    /// This class uses a standard System.Timers.Timer to obtain long time delays greater than the
    /// 65535ms limit.
    /// </summary>
    /// <remarks>
    /// Usage is similar to a standard Timer but interval is in seconds not milliseconds.
    /// </remarks>
    public class LongTimer : IDisposable
    {
        private int mInterval;
        private Timer mTimer;
        private int tickCounter;
        private int multiplier;

        public event ElapsedEventHandler Elapsed;

        public int Interval {
            get { return mInterval; }
            set { mInterval = value; }
        }

        public LongTimer() {
            mInterval = 1;
            mTimer = new Timer();
            mTimer.Elapsed += new ElapsedEventHandler(HandleTimerElapsed);
        }

        public LongTimer(int interval) {
            mInterval = interval;
            mTimer = new Timer();
            mTimer.Elapsed += new ElapsedEventHandler(HandleTimerElapsed);
        }

        public void Start() {
            mTimer.Stop();
            tickCounter = 0;
            if (mInterval <= 10) {
                multiplier = 1000;
            }
            else if (mInterval <= 100) {
                multiplier = 100;
            }
            else {
                multiplier = 10;
            }
            mTimer.Interval = mInterval * multiplier;
            mTimer.Start();
        }

        public void Stop() {
            mTimer.Stop();
            tickCounter = 0;
        }

        private void HandleTimerElapsed(object sender, ElapsedEventArgs e) {
            tickCounter += 1;
            if (tickCounter >= 1000 / multiplier) {
                if (Elapsed != null) Elapsed(this, e);
                tickCounter = 0;
            }
        }
        
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                mTimer.Dispose();
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }

}