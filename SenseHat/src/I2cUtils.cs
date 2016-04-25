using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace SenseHat
{
    /// <summary>
    /// Helper class for connecting to an I2cDevice and read/write bytes
    /// </summary>
    public static class I2cUtils
    {
        public static async Task<I2cDevice> GetDeviceAsync(int addr, I2cBusSpeed speed = I2cBusSpeed.StandardMode, I2cSharingMode sharing = I2cSharingMode.Exclusive)
        {
            var aqs = I2cDevice.GetDeviceSelector();
            var infos = await DeviceInformation.FindAllAsync(aqs);
            var settings = new I2cConnectionSettings(addr)
            {
                BusSpeed = speed,
                SharingMode = sharing
            };
            return await I2cDevice.FromIdAsync(infos[0].Id, settings);
        }

        public static byte[] ReadBytes(I2cDevice device, byte addr, int count)
        {
            try
            {
                byte[] wb = { addr };
                byte[] rb = new byte[count];
                device.WriteRead(wb, rb);
                return rb;
            }
            catch (Exception e)
            {
                var msg = String.Format("failed to read {0} bytes from address: {1:X}", count, addr);
                throw new Exception(msg, e);
            }
        }

        public static byte Read8(I2cDevice device, byte addr)
        {
            var bytes = ReadBytes(device, addr, 1);
            return bytes[0];
        }

        public static UInt16 Read16LE(I2cDevice device, byte addr)
        {
            var bytes = ReadBytes(device, addr, 2);
            return (UInt16)(((UInt16)bytes[1] << 8) | (UInt16)bytes[0]);
        }

        public static UInt32 Read24LE(I2cDevice device, byte addr)
        {
            var bytes = ReadBytes(device, addr, 3);
            return (UInt32)(((UInt32)bytes[2] << 16) | ((UInt32)bytes[1] << 8) | (UInt32)bytes[0]);
        }

        public static void WriteByte(I2cDevice device, byte addr, byte val)
        {
            try
            {
                var buf = new byte[2] { addr, val };
                device.Write(buf);
            }
            catch (Exception e)
            {
                var msg = String.Format("failed to write {0} to address: {1:X}", val, addr);
                throw new Exception(msg, e);
            }
        }
    }
}
