﻿using IDUNv2.Common;
using IDUNv2.DataAccess;
using IDUNv2.Models;
using IDUNv2.SensorLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace IDUNv2.Pages
{
    public class SensorTriggerViewModel : NotifyBase
    {
        private SensorTriggerComparer _comparer = SensorTriggerComparer.Below;

        public SensorTriggerComparer Comparer
        {
            get { return _comparer; }
            set
            {
                _comparer = value;
                Model.Comparer = value;
                Notify("ComparerBelow");
                Notify("ComparerAbove");
            }
        }

        public bool ComparerBelow
        {
            get { return Comparer == SensorTriggerComparer.Below; }
            set { Comparer = SensorTriggerComparer.Below;}
        }

        public bool ComparerAbove
        {
            get { return Comparer == SensorTriggerComparer.Above; }
            set { Comparer = SensorTriggerComparer.Above; }
        }

        private SensorTrigger _model;
        public SensorTrigger Model { get {return _model;} set {_model = value; Notify(); }  }

        public SensorTriggerViewModel(SensorTrigger model)
        {
            Model = model;
            Comparer = model.Comparer;
            
        }
        public int Id
        {
            get { return Model.Id; }
            set { Model.Id = value; Notify(); }
        }
        public int TemplateId
        {
            get { return Model.TemplateId; }
            set { Model.TemplateId = value; Notify(); }
        }
        public float Value
        {
            get { return Model.Value; }
            set { Model.Value = value; Notify(); }
        }

        public override string ToString()
        {
  
            return $"TriggerId: {Model.Id} ON value {Model.Comparer.ToString().ToUpper()} {Model.Value} with TemplateID {Model.TemplateId}";
        }
    }

    public class SensorSettingsViewModel : NotifyBase
    {
        private Sensor _sensor;
        private SensorTriggerViewModel _selectedTrigger;

        public Sensor Sensor
        {
            get { return _sensor; }
            set { _sensor = value; Notify(); }
        }

        public SensorTriggerViewModel SelectedTrigger
        {
            get { return _selectedTrigger; }
            set { _selectedTrigger = value; Notify(); }
        }

        public List<FaultReportTemplate> Templates { get; set; }

        private FaultReportTemplate _selectedTemplate;
        public FaultReportTemplate SelectedTemplate
        {
            get { return _selectedTemplate; }
            set { _selectedTemplate = value; Notify(); }
        }

        #region Command Bindings

        public ActionCommand<object> SaveCommand { get; set; }
        public ActionCommand<object> CreateTriggerCommand { get; set; }
        public ActionCommand<object> SaveChangesTriggerCommand { get; set; }
        public ActionCommand<object> RemoveTriggerCommand { get; set; }
        #endregion

        private ObservableCollection<SensorTriggerViewModel> _triggers;
        public ObservableCollection<SensorTriggerViewModel> Triggers { get {return _triggers; } set {_triggers = value; Notify(); } }

        public SensorSettingsViewModel()
        {
            SaveCommand = new ActionCommand<object>(SaveCommand_Execute);
            CreateTriggerCommand = new ActionCommand<object>(CreateTriggerCommand_Execute);
            SaveChangesTriggerCommand = new ActionCommand<object>(SaveChangesTriggerCommand_Execute);
            RemoveTriggerCommand = new ActionCommand<object>(RemoveTriggerCommand_Execute);
            SelectedTrigger = new SensorTriggerViewModel(new SensorTrigger());
            Templates = new List<FaultReportTemplate>();
            var triggers = DAL.GetSensorTriggers().Result;
            Triggers = new ObservableCollection<SensorTriggerViewModel>(triggers.Select(t => new SensorTriggerViewModel(t)));
        }


        private void SaveCommand_Execute(object param)
        {
            Sensor.SaveToLocalSettings();
            
            
        }

        private async void CreateTriggerCommand_Execute(object param)
        {
           try
            {
                SelectedTrigger = new SensorTriggerViewModel( new SensorTrigger {});
                //if (await CheckIfTriggerExists(SelectedTrigger.Model) == false)
                //{
                SelectedTrigger.Model = await DAL.SetSensorTrigger(SelectedTrigger.Model);
                Triggers.Add(new SensorTriggerViewModel(SelectedTrigger.Model));
                //}
            }
            catch
            {

            }
            
            
        }
        private async void SaveChangesTriggerCommand_Execute(object param)
        {
            SelectedTrigger.Model = await DAL.SetSensorTrigger(SelectedTrigger.Model);

            var triggers = DAL.GetSensorTriggers().Result;
            Triggers = new ObservableCollection<SensorTriggerViewModel>(triggers.Select(t => new SensorTriggerViewModel(t)));
        }

        private async void RemoveTriggerCommand_Execute(object param)
        {
            try
            {
                SelectedTrigger.Model = await DAL.DeleteSensorTrigger(SelectedTrigger.Model);
                Triggers.Remove(SelectedTrigger);
            }
            catch
            {

            }
        }

        //public async Task<bool> CheckIfTriggerExists(SensorTrigger Trigger)
        //{
        //        var SensorTriggerList = await DAL.GetSensorTriggers();
        //        foreach (var item in SensorTriggerList)
        //        {
        //            if (item.Comparer == Trigger.Comparer && item.Value == Trigger.Value && item.TemplateId == Trigger.TemplateId)
        //                return true;
        //        }
        //        return false;
        //}
    }

    public sealed partial class SensorSettingsPage : Page
    {
        private SensorSettingsViewModel viewModel = new SensorSettingsViewModel();

        public SensorSettingsPage()
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
            this.Loaded += SensorSettingsPage_Loaded;
        }

        private async void SensorSettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            viewModel.Templates = await DAL.GetFaultReportTemplates();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            viewModel.Sensor = e.Parameter as Sensor;
            ShellPage.Current.SetPageTitle("Sensor Overview / " + viewModel.Sensor.Id.ToString());
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            osk.SetTarget(sender as TextBox);
            osk.Visibility = Visibility.Visible;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            osk.SetTarget(null as TextBox);
            osk.Visibility = Visibility.Collapsed;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            var selectedItem = (FaultReportTemplate)cb.SelectedItem;
            viewModel.SelectedTrigger.TemplateId = selectedItem.Id;
        }


        //private void TemplateSelectionChange(object sender, SelectionChangedEventArgs e)
        //{
        //    var cb = (ComboBox)sender;
        //    selectedItem = (ViewModels.FaultReportTemplateViewModel)cb.SelectedItem;

        //}

    }
}
