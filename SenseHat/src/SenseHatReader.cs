using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SenseHat
{
    public class SenseHatReader
    {
        private HTS221 hts221;
        private LPS25H lps25h;
        private DispatcherTimer timer;
        private int updateIntervalMS;
        private int capacity;

        public delegate void SenseHatTicker(SenseHatReader reader, SenseHatReading reading);

        public SenseHatTicker Tick;

        public Queue<SenseHatReading> Readings { get; set; }

        public SenseHatReader(int updateIntervalMS, int capacity)
        {
            this.updateIntervalMS = updateIntervalMS;
            this.capacity = capacity;
            Readings = new Queue<SenseHatReading>(capacity);
        }

        public void Init()
        {
            var task = Task.Run(async () =>
            {
                hts221 = await Sensor.CreateAsync<HTS221>().ConfigureAwait(false);
                lps25h = await Sensor.CreateAsync<LPS25H>().ConfigureAwait(false);
            });
            if (!task.Wait(5000))
            {
                throw new Exception("timed out waiting for sensor to initialize");
            }
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(updateIntervalMS) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            var temp = hts221.ReadTemperature();
            var humid = hts221.ReadHumidity();
            var press = lps25h.ReadPressure();

            var reading = new SenseHatReading { Temperature = temp, Humidity = humid, Pressure = press };

            if (Readings.Count == 10)
                Readings.Dequeue();

            Readings.Enqueue(reading);

            Tick?.Invoke(this, reading);
        }
    }
}
