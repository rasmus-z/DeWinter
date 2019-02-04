using UnityEngine;
using UFlow;
using System.Timers;

namespace Ambition
{
    public class DelayLink : ULink<float>
    {
        private Timer _timer;
        public override void SetValue(float data)
        {
            _timer = new Timer();
            _timer.Interval = data;
        }
        public override void Initialize()
        {
            _timer.Elapsed += OnTimer;
            _timer.Enabled = true;
        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            Activate();
        }

        public override void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
