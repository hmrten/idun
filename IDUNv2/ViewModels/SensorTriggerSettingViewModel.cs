﻿using IDUNv2.Common;
using IDUNv2.DataAccess;
using IDUNv2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDUNv2.ViewModels
{
    public class SensorTriggerSettingViewModel : NotifyBase
    {
        public ObservableCollection<SensorTrigger> SensorTriggerList { get; set; }

        public SensorTriggerComparer[] Comparers { get; set; }
        public List<FaultReportTemplate> Templates { get; set; }

        public SensorTriggerSettingViewModel()
        {
            Comparers = Enum.GetValues(typeof(SensorTriggerComparer)).Cast<SensorTriggerComparer>().ToArray();
            Templates = DAL.GetFaultReportTemplates().Result;
            
            SensorTriggerList =new ObservableCollection<SensorTrigger>(DAL.GetSensorTriggers().Result);
            CurrentTrigger = new SensorTrigger();
        }

        private SensorTrigger currentTrigger;
        public SensorTrigger CurrentTrigger
        {
            get { return currentTrigger; }
            set { currentTrigger = value; Notify(); }
        }

        internal void AddTrigger()
        {
            DAL.SetSensorTrigger(CurrentTrigger);
        }
    }
}
