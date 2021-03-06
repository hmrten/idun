﻿using Addovation.Cloud.Apps.AddoResources.Client.Portable;
using IDUNv2.Common;
using IDUNv2.DataAccess;
using IDUNv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IDUNv2.DataAccess.DAL;

namespace IDUNv2.ViewModels
{
    class FaultReportDetailsViewModel : NotifyBase
    {
        #region Fields

        private IFaultReportAccess faultReportAccess;

        #endregion

        #region Notify Fields
        private List<Attachment> _attachements;
        private FaultReport model;
        private DocumentString attachementDataText;
        #endregion

        #region Notify Properties

        public FaultReport Model
        {
            get { return model; }
            set { model = value; Notify(); }
        }

        #endregion

        #region Properties

        public string Discovery
        {
            get { return faultReportAccess.LookupWorkOrderDiscovery(model.ErrDiscoverCode)?.Description; }
        }

        public string Symptom
        {
            get { return faultReportAccess.LookupWorkOrderSymptom(model.ErrSymptom)?.Description; }
        }

        public string Priority
        {
            get { return faultReportAccess.LookupMaintenancePriority(model.PriorityId)?.Description; }
        }

        public List<Attachment> Attachements
        {
            get { return _attachements; }
            set { _attachements = value;Notify(); }    
        }

        public DocumentString AttachementDataText
        {
            get { return attachementDataText; }
            set { attachementDataText = value; Notify(); }
        }
        #endregion

        public FaultReportDetailsViewModel(IFaultReportAccess faultReportAccess)
        {
            this.faultReportAccess = faultReportAccess;
        }
        
        public async Task InitAsync()
        {
            Attachements = await DAL.GetAttachements(model.WoNo);
        }
    }
}
