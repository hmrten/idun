﻿using IDUNv2.Models;
using IDUNv2.ViewModels;
using System;
using System.Linq;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using IDUNv2.DataAccess;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace IDUNv2.Pages
{
    public sealed partial class ShellPage : Page
    {
        public static ShellPage Current;

        #region FIelds

        private ShellViewModel viewModel = new ShellViewModel();

        #endregion

        #region Properties

        /// <summary>
        /// Expose CmdBar's primary commands so each page can add/remove buttons to it
        /// </summary>
        public IObservableVector<ICommandBarElement> CmdBarPrimaryCommands
        {
            get { return CmdBar.PrimaryCommands; }
        }

        #endregion

        #region Constructors

        public ShellPage()
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
            this.Loaded += MainPage_Loaded;
            Current = this;
            viewModel.NotificationList.CollectionChanged += NotificationList_CollectionChanged;
        }

        #endregion

        #region Event Handlers

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DeviceSettings.HasSettings())
            {
                viewModel.SetNavListSettings();
                viewModel.SelectMainMenu(ContentFrame, viewModel.NavList.FirstOrDefault());
            }
            else
            {
                viewModel.SetNavListFull();
                var first = viewModel.NavList.First();
                viewModel.SelectMainMenu(ContentFrame, first);
                var status = await DAL.ConnectToCloud();
                if (DAL.FaultReportAccess.LiveSystem)
                {
                    if (!status)
                        AddNotificatoin(
                            NotificationType.Warning,
                            "Authorization Failed!",
                            "Please enter valid IFS Cloud Log In details or check your Internet Connection!\nCloud Services are not available.");
                }
                else
                {
                    AddNotificatoin(
                        NotificationType.Information,
                        "Non-LiveSystem",
                        "Not connected to real cloud, using in-memory test data");
                }
            }
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            var image = (Image)sender;
            image.Source = new BitmapImage(new Uri(BaseUri, "Assets/loadinggif.gif"));
        }

        private void Timer_Tick(object sender, object e)
        {
            NotificationButton.Visibility = Visibility.Collapsed;
            var timer = (DispatcherTimer)sender;
            timer.Stop();
        }

        #endregion

        #region Spinner

        public static void SetSpinner(LoadingState state)
        {
            if (Current.SpinnerPanel == null)
                return;
            switch (state)
            {
                case LoadingState.Loading:
                    Current.SpinnerPanel.Visibility = Visibility.Visible;
                    break;
                default:
                    Current.SpinnerPanel.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        #endregion

        #region Navigation

        public void EnableFullNavList()
        {
            viewModel.SetNavListFull();
        }

        private void NavMenu_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var lv = sender as ListView;
            var item = lv.SelectedItem as NavMenuItem;
            viewModel.SelectMainMenu(ContentFrame, item);
        }

        private void NavMenuExpand_Click(object sender, RoutedEventArgs e)
        {
            viewModel.IsPaneOpen = !viewModel.IsPaneOpen;
        }

        public void ContentNavigate(Type pageType, object param = null)
        {
            ContentFrame.Navigate(pageType, param);
        }

        public void PushNavLink(NavLinkItem item)
        {
            var last = viewModel.NavLinks.LastOrDefault();
            if (last != null && last.Title == item.Title)
                return;
            viewModel.NavLinks.Add(item);
        }

        public void PopNavLink()
        {
            viewModel.NavLinks.RemoveAt(viewModel.NavLinks.Count - 1);
        }

        private void NavLink_Click(object sender, RoutedEventArgs e)
        {
            var hyper = sender as HyperlinkButton;
            var item = hyper.Tag as NavLinkItem;
            PopNavLinksTo(item);
            ContentFrame.Navigate(item.PageType, item.Param);
        }

        private void PopNavLinksTo(NavLinkItem item)
        {
            var links = viewModel.NavLinks;
            int i = links.IndexOf(item);
            if (i != -1)
            {
                int n = viewModel.NavLinks.Count - 1 - i;
                while (n > 0)
                {
                    links.RemoveAt(links.Count - 1);
                    --n;
                }
            }
        }

        public static void Reload()
        {
            Current.Frame.Navigate(typeof(ShellPage));
        }

        #endregion

        #region Notifications

        private void NotificationList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(2500);
                timer.Tick += Timer_Tick;
                timer.Start();
                NotificationButton.Visibility = Visibility.Visible;
            }
        }

        public void AddNotificatoin(NotificationType Type, string ShortDescription, string LongDescription)
        {
            DateTime Date = new DateTime();
            Date = DateTime.Now;
            viewModel.NotificationList.Insert(0, new Notification
            {
                Type = Type,
                ShortDescription = ShortDescription,
                LongDescription = LongDescription,
                Date = Date.ToString("dd/MM/yyyy HH:mm:ss")
            });
        }

        private void NotificationViewed_Click(object sender, RoutedEventArgs e)
        {
            viewModel.NotificationList.Remove(viewModel.SelectedNotificationItem);
            if (NotificationFlyOutList.Items.Count == 0)
            {
                NotificationListPanel.Visibility = Visibility.Collapsed;
            }
            NotificationFlyOutList.SelectedItem = NotificationFlyOutList.Items.FirstOrDefault();
        }

        private void NotificationButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotificationListPanel.Visibility == Visibility.Visible)
                NotificationListPanel.Visibility = Visibility.Collapsed;
            else
            {
                NotificationListPanel.Visibility = Visibility.Visible;
                NotificationFlyOutList.SelectedItem = NotificationFlyOutList.Items.FirstOrDefault();
            }
        }

        private void NotificationIconTap(object sender, RoutedEventArgs e)
        {
            NotificationButton.Visibility = Visibility.Collapsed;
            NotificationListPanel.Visibility = Visibility.Visible;
            NotificationFlyOutList.SelectedItem = NotificationFlyOutList.Items.FirstOrDefault();
        }

        private void CloseNotification_Click(object sender, RoutedEventArgs e)
        {
            NotificationListPanel.Visibility = Visibility.Collapsed;
        }

        private void NotificationFlyOutList_Loaded(object sender, RoutedEventArgs e)
        {
            var list = (ListView)sender;
            list.SelectedItem = list.Items.FirstOrDefault();
        }

        private void NotificationALLViewed_Click(object sender, RoutedEventArgs e)
        {
            viewModel.NotificationList.Clear();
        }

        #endregion

        #region OSK

        public void ShowOSK(Control target)
        {
            if (target != null)
            {
                osk.SetTarget(target);
                osk.Visibility = Visibility.Visible;
                CmdBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                osk.Visibility = Visibility.Collapsed;
                CmdBar.Visibility = Visibility.Visible;
            }
        }

        public void ShowNumPad(Control target)
        {
            if (target != null)
            {
                NumPad.SetTarget(target);
                NumPad.Visibility = Visibility.Visible;
            }
            else
            {
                NumPad.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region WebView

        private void CloseWeb_Click(object sender, RoutedEventArgs e)
        {
            Current.WebPageContentFrame.Visibility = Visibility.Collapsed;
            Current.CmdBar.Visibility = Visibility.Visible;
        }

        private void NavDocButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            Current.ShowWeb(new Uri("ms-appx-web:///Assets/IDUNDocumentation.html#" + btn.Tag.ToString()));
        }
        private void NavTutButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            Current.ShowWeb(new Uri("ms-appx-web:///Assets/IDUNTutorial.html#" + btn.Tag.ToString()));
        }

        public void ShowWeb(Uri adress)
        {
            viewModel.localWebPage = adress;
            if (adress.ToString().Contains("IDUNDocumentation"))
            {
                DocumentationNavigation.Visibility = Visibility.Visible;
                TutorialNavigation.Visibility = Visibility.Collapsed;
            }
            else if (adress.ToString().Contains("IDUNTutorial"))
            {
                DocumentationNavigation.Visibility = Visibility.Collapsed;
                TutorialNavigation.Visibility = Visibility.Visible;
            }
            else
            {
                DocumentationNavigation.Visibility = Visibility.Collapsed;
                TutorialNavigation.Visibility = Visibility.Collapsed;
            }
            Current.WebPageContentFrame.Visibility = Visibility.Visible;
            Current.CmdBar.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}
