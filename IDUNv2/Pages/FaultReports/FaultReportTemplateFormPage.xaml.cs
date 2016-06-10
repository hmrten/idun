﻿using IDUNv2.DataAccess;
using IDUNv2.Models;
using IDUNv2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace IDUNv2.Pages
{
    public sealed partial class FaultReportTemplateFormPage : Page
    {
        private FaultReportTemplateFormViewModel viewModel = new FaultReportTemplateFormViewModel();

        private FaultReportTemplateViewModel selectedItem;

        public FaultReportTemplateFormPage()
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
            this.Loaded += Templates_Loaded;
        }

        private async void Templates_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.InitAsync();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            osk.SetTarget(sender as TextBox);
            osk.Visibility = Visibility.Visible;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            osk.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // set the list of buttons
            DAL.SetCmdBarItems(viewModel.CmdBarItems);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // use null to clear the list
            DAL.SetCmdBarItems(null);

            int dirtyTemplatesNr = 0;
            if (viewModel.Templates == null)
                return;
            foreach (var item in viewModel.Templates)
            {
                if (item.Dirty)
                    ++dirtyTemplatesNr;
            }
            if (dirtyTemplatesNr >= 1)
            {
                ShellPage.Current.AddNotificatoin(Models.NotificationType.Error, "Unsaved Templates", "Changed templates data and/or new templates were present and not saved. All changes and newly generated templates were discarded! ");

            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (ListBox)sender;
            selectedItem = (FaultReportTemplateViewModel)list.SelectedItem;
        }
    }
}
