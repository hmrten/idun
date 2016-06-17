﻿using Addovation.Cloud.Apps.AddoResources.Client.Portable;
using IDUNv2.Common;
using IDUNv2.DataAccess;
using IDUNv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IDUNv2.ViewModels
{
    public class FaultReportTemplateViewModel : NotifyBase
    {
        #region Fields

        private FaultReportTemplate model;

        #endregion

        #region Notify Fields

        private bool dirty;

        #endregion

        #region Notify Properties

        public bool Dirty
        {
            get { return dirty; }
            set { dirty = value; Notify(); }
        }

        public FaultReportTemplate Model
        {
            get { return model; }
            set { model = value; Notify(); Notify("IdString"); }
        }

        public string IdString
        {
            get { return $"{model.Id}:"; }
        }

        public string Name
        {
            get { return model.Name; }
            set { model.Name = value; SetDirty(); }
        }

        public string Directive
        {
            get { return model.Directive; }
            set { model.Directive = value; SetDirty(); }
        }

        public string FaultDescr
        {
            get { return model.FaultDescr; }
            set { model.FaultDescr = value; SetDirty(); }
        }

        public WorkOrderDiscCode Discovery
        {
            get { return DAL.GetWorkOrderDiscovery(model.DiscCode); }
            set { model.DiscCode = value.ErrDiscoverCode; SetDirty(); }
        }

        public WorkOrderSymptCode Symptom
        {
            get { return DAL.GetWorkOrderSymptom(model.SymptCode); }
            set { model.SymptCode = value.ErrSymptom; SetDirty(); }
        }

        public MaintenancePriority Priority
        {
            get { return DAL.GetWorkOrderPiority(model.PrioCode); }
            set { model.PrioCode = value.PriorityId; SetDirty(); }
        }

        #endregion

        private void SetDirty([CallerMemberName] string caller = "")
        {
            Dirty = true;
            Notify(caller);
        }

        #region Constructor

        public FaultReportTemplateViewModel(FaultReportTemplate model)
        {
            this.model = model;
        }

        #endregion
    }
}
