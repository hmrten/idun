using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SenseHat
{
    public class SenseHat
    {
        private HTS221 hts221;
        private LPS25H lps25h;
        private DispatcherTimer timer;
        private int updateIntervalMS;

        public delegate void SensorReadings(float temperature, float humidity, float pressure);

        public SensorReadings SensorReadingsUpdate { get; set; }

        public SenseHat(int updateIntervalMS)
        {
            this.updateIntervalMS = updateIntervalMS;
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
                throw new Exception("tiemd out waiting for sensor to initialize");
            }
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(updateIntervalMS) };
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            var temp = hts221.ReadTemperature();
            var humid = hts221.ReadHumidity();
            var press = lps25h.ReadPressure();
            SensorReadingsUpdate?.Invoke(temp, humid, press);
        }
    }
}
