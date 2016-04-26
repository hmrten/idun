using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SenseHat
{
    public class SenseHatReader : SensorReader
    {
        private HTS221 hts221;
        private LPS25H lps25h;

        public SenseHatReader(int updateIntervalMs, int capacity) : base(updateIntervalMs, capacity)
        {
        }

        public override void Init()
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

            base.Init();
        }

        protected override void Timer_Tick(object sender, object e)
        {
            var data = new SensorData
            {
                Date = DateTime.Now,
                Temperature = hts221.ReadTemperature(),
                Humidity = hts221.ReadHumidity(),
                Pressure = lps25h.ReadPressure(),
            };
            AddData(data);
        }
    }
}
