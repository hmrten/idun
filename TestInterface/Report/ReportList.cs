using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestInterface.Report
{
    public class ReportList
    {
        public string MaxNr { get; set; }
        public string DTofServiceCall { get; set; }
        public string SCPressure { get; set; }
        public string SCTemperature { get; set; }
        public string SCHumidity { get; set; }
        public string Note { get; set; }
        public string MaxHumidity { get; set; }
        public string MinHumidity { get; set; }
        public string MaxPressure { get; set; }
        public string MinPressure { get; set; }
        public string MaxTemperature { get; set; }
        public string MinTemperature { get; set; }
    }
}
