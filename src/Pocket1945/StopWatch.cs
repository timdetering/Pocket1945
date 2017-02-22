using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Pocket1945
{
    /// <summary>
    /// This class is taken from the article series "Gaming with the .NET Compact Framework"
    /// writen by Geoff Schwab. The articles can be found on http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnnetcomp/html/BustThisGame.asp.
    /// This class use external method calls to make a high performance counter.
    /// </summary>
    public class StopWatch
    {
        /// <summary>
        /// Frequency of the counter.
        /// </summary>
        protected Int64 m_freq;

        /// <summary>
        /// Specifies if performance counters are in use.  Only set to false
        /// if the OEM does not provide these counters.
        /// </summary>
        protected bool m_usePerf = true;

        /// <summary>
        /// Initializes a stop watch instances by storing the counter
        /// frequency.
        /// </summary>
        public StopWatch()
        {
            if (SafeNativeMethods.QueryPerformanceFrequency(ref m_freq) == 0)
            {
                m_freq = 1000;
                m_usePerf = false;
            }

            Debug.Assert(m_freq != 0,
                "StopWatch.StopWatch: Invalid frequency");
        }

        /// <summary>
        /// Get the current tick.
        /// </summary>
        /// <returns>Tick</returns>
        public Int64 CurrentTick()
        {
            if (m_usePerf)
            {
                Int64 curTick = 0;
                SafeNativeMethods.QueryPerformanceCounter(ref curTick);
                return curTick;
            }

            return (Int64)Environment.TickCount;
        }

        /// <summary>
        /// Calculate the delta, taking into account roll-over.
        /// </summary>
        /// <param name="curTime">Current tick</param>
        /// <param name="prevTime">Previous tick</param>
        /// <param name="maxValue">Maximum tick value (for roll-over)</param>
        /// <returns>Current - Previous</returns>
        private Int64 GetSafeDelta(Int64 curTime, Int64 prevTime, Int64 maxValue)
        {
            if (curTime < prevTime)
            {
                return curTime + maxValue - prevTime;
            }

            return curTime - prevTime;
        }

        /// <summary>
        /// Calculate the time, in milliseconds, between ticks. 
        /// </summary>
        /// <param name="curTick">Current tick</param>
        /// <param name="prevTick">Previous tick</param>
        /// <returns>Current - Previous (milliseconds)</returns>
        public Int64 DeltaTime_ms(Int64 curTick, Int64 prevTick)
        {
            if (m_usePerf)
            {
                return 1000 * GetSafeDelta(curTick, prevTick, Int64.MaxValue) / m_freq;
            }

            return GetSafeDelta(curTick, prevTick, Int32.MaxValue);
        }
    }
}
