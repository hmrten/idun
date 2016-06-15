﻿using IDUNv2.DataAccess;
using IDUNv2.Models;
using IDUNv2.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class DeviceSettingsPage : Page
    {
        private DeviceSettingsViewModel viewModel = new DeviceSettingsViewModel();

        public ICollection<CmdBarItem> CmdBarItems { get; private set; }


        private void NavigationItems()
        {
            CmdBarItems = new CmdBarItem[]
            {
                new CmdBarItem(Symbol.Save, "Save", SaveDeviceSettings)
            };
        }

        private async void SaveDeviceSettings(object param)
        {
            ShellPage.SetSpinner(LoadingState.Loading);
            var status = await DAL.AuthenticateAuthorization();
            viewModel.ConnectionStatus = !status;
            if (status)
                viewModel.AuthorisationMessage = "Log in Successful!";
            else 
                viewModel.AuthorisationMessage = "Authorisation Failed. Please Enter Valid details or check your Internet Connection!";
            ShellPage.SetSpinner(LoadingState.Finished);
        }

        public DeviceSettingsPage()
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DAL.ShowOSK(sender as TextBox);
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DAL.ShowOSK(sender as PasswordBox);
        }

        private void TBLostFocus(object sender, RoutedEventArgs e)
        {
            DAL.ShowOSK(null);
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (viewModel.ObjectID.Length == 0 ||
                viewModel.SystemID.Length == 0 ||
                viewModel.Username.Length == 0 ||
                viewModel.Password.Length <= 3 ||
                viewModel.URL.Length == 0)
            {
                ShellPage.Current.AddNotificatoin(Models.NotificationType.Error, "Server Settings Error", "Please check that all information is entered correctly and try again!");
            }
            else
            {
                ShellPage.Current.AddNotificatoin(Models.NotificationType.Information, "Server Settings Saved", "Server settings have been saved locally.");
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var status = await DAL.AuthenticateAuthorization();
            viewModel.ConnectionStatus = !status;
            NavigationItems();
            DAL.SetCmdBarItems(CmdBarItems);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DAL.SetCmdBarItems(null);
        }

    }
}
