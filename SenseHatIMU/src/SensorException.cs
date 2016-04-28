using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseHatIMU.src
{
    /// <summary>
	/// An exception thrown by the sensor code
	/// </summary>
	public class SensorException : Exception
    {
        public SensorException(string message)
            : base(message)
        {
        }
        public SensorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
