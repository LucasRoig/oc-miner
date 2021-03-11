using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AutoMiner
{
    class LogService
    {
        static int maxLines = 1000;
        static int lineCount = 0;
        static List<LogListener> listeners = new List<LogListener>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void addLine(string line)
        {
            if (lineCount >= maxLines)
            {
                listeners.ForEach(l => l.removeLine());
            }
            listeners.ForEach(l => l.addLine(line));
            lineCount++;
        }

        public static void addListener(LogListener l)
        {
            listeners.Add(l);
        }
    }

    interface LogListener
    {
        void addLine(String s);
        void removeLine();
    }
}
