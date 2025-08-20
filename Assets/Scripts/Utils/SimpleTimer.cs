namespace Utils
{
    public class SimpleTimer
    {
        private float time;
        private float latency;

        private bool repeat;

        private object data;
        
        public delegate void TimerEndHandler(object data);
        public event TimerEndHandler OnTimerEnd;
        
        public void SetTimer(float _time, bool _repeat, object _data)
        {
            time = _time;
            latency = time;
            
            repeat = _repeat;
            data = _data;
        }

        public void Clear()
        {
            time = 0;
            latency = 0;
            data = null;
            OnTimerEnd = null;
        }
        
        void Tick(float deltaTime)
        {
            if (latency <= 0)
            {
                OnTimerEnd.Invoke(data);
                
                if (repeat)
                    latency = time;
            }
            else
            {
                latency -= deltaTime;
            }
        }
    }
}