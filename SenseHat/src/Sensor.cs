using System;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace SenseHat
{
    public abstract class Sensor : IDisposable
    {
        protected I2cDevice device;

        public virtual void Dispose()
        {
            if (device != null)
            {
                device.Dispose();
                device = null;
            }
        }

        protected abstract Task InitAsync();

        public static async Task<T> CreateAsync<T>()
            where T : Sensor, new()
        {
            var sensor = new T();
            await sensor.InitAsync();
            return sensor;
        }
    }
}
