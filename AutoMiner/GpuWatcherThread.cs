using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MSI.Afterburner;
using System.Diagnostics;

namespace AutoMiner
{
    class GpuWatcherThread
    {
        private Thread thread;
        private int interval = 5000;

        public void Start()
        {
            if (this.thread == null)
            {
                this.thread = new Thread(routine);
                this.thread.Start();
            }
        }

        public void Stop()
        {
            if (this.thread != null)
            {
                this.thread.Interrupt();
            }
        }

        private void routine()
        {
            try
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    GpuManager.applyMiningOcParams();
                    Thread.Sleep(this.interval);
                }
            } catch (ThreadInterruptedException e )
            {
                return;
            }
            
        }
    }
}
