using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL_Utils
{
    public class Timer
    {
        private readonly Stopwatch _sw = new();

        public long ElapsedTimeMs => _sw.ElapsedMilliseconds;

        public void Start()
        {
            _sw.Reset();
            _sw.Start();                        
        }

        public void Stop() 
        { 
            _sw.Stop();
        }
    }
}
