using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseHatIMU.src
{
    /// <summary>
	/// A generic sensor.
	/// </summary>
	public abstract class Sensor : IDisposable
    {
        protected Sensor()
        {
        }

        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Has the sensor been initialized?
        /// </summary>
        public bool Initiated
        { get; private set; }

        /// <summary>
        /// The last sensor readings.
        /// </summary>
        public SensorReadings Readings
        { get; private set; }

        /// <summary>
        /// Event fired when the readings has changed.
        /// </summary>
        public event EventHandler OnReadingsChanged;

        /// <summary>
        /// Initiates the sensor.
        /// If failing, an exception is thrown.
        /// </summary>
        public async Task InitAsync()
        {
            if (Initiated)
            {
                return;
            }

            bool initiated = await InitDeviceAsync();

            if (initiated)
            {
                AfterInitDevice();
            }

            Initiated = initiated;
        }

        protected abstract Task<bool> InitDeviceAsync();

        protected virtual void AfterInitDevice()
        {
        }

        protected void AssignNewReadings(SensorReadings readings, bool processReadings = true)
        {
            if (processReadings)
            {
                ProcessReadings(ref readings);
            }

            Readings = readings;

            OnReadingsChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void ProcessReadings(ref SensorReadings readings)
        {
        }

        /// <summary>
        /// Tries to update the readings.
        /// Returns true if new readings are available, otherwise false.
        /// An exception is thrown if something goes wrong.
        /// </summary>
        public abstract bool Update();
    }
}
