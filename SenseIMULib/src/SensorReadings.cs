using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseHatIMU.src
{
    /// <summary>
	/// The various values from a sensor reading.
	/// </summary>
	public struct SensorReadings
    {
        /// <summary>
        /// Sample timestamp
        /// </summary>
        public DateTime Timestamp;

        /// <summary>
        /// True if fusion pose valid
        /// </summary>
        public bool FusionPoseValid;

        /// <summary>
        /// The fusion pose
        /// </summary>
        public Vector3 FusionPose;

        /// <summary>
        /// True if the fusion quaternion is valid
        /// </summary>
        public bool FusionQPoseValid;

        /// <summary>
        /// The fusion quaternion
        /// </summary>
        public Quaternion FusionQPose;

        /// <summary>
        /// True if gyro data is valid
        /// </summary>
        public bool GyroValid;

        /// <summary>
        /// Gyro data in radians/sec
        /// </summary>
        public Vector3 Gyro;

        /// <summary>
        /// True if accel data valid
        /// </summary>
        public bool AccelerationValid;

        /// <summary>
        /// Acceleration data in g
        /// </summary>
        public Vector3 Acceleration;

        /// <summary>
        /// True if mag data valid
        /// </summary>
        public bool MagneticFieldValid;

        /// <summary>
        /// Magnetic field vector in uT
        /// </summary>
        public Vector3 MagneticField;

    }
}
