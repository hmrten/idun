using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseHatIMU.src
{
    /// <summary>
    /// Direction of axis
    /// </summary>
    public enum AxisDirection
    {
        Default = 0,
        XNorthYEast = 0,
        XEastYSouth,
        XSouthYEest,
        XWestYNorth,
        XNorthYWest,
        XEastYNorth,
        XSouthYEast,
        XWestYSouth,
        XUpYNorth,
        XUpYEast,
        XUpYSouth,
        XUpYWest,
        XDownYNorth,
        XDownYEast,
        XDownYSouth,
        XDownYWest,
        XNorthYUp,
        XEastYUp,
        XSouthYUp,
        XWestYUp,
        XNorthYDown,
        XEastYDown,
        XSouthYDown,
        XWestYDown,
    }
}
