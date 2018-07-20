using Microsoft.UI.Xaml.Controls;
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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<ListItemData> Items { get; set; } = new ObservableCollection<ListItemData>();

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            ListView1.ItemsSource = Items;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainPage_Loaded;
            RefreshContainer.RefreshRequested += RefreshContainer_RefreshRequested;
            RefreshContainer.Visualizer.RefreshStateChanged += Visualizer_RefreshStateChanged;

            // Add some initial content to the list.
            await FetchAndInsertItemsAsync(2);
        }

        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            RefreshContainer.RequestRefresh();
        }

        private async void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            // Respond to a request by performing a refresh and using the deferral object.
            using (var RefreshCompletionDeferral = args.GetDeferral())
            {
                // Do some async operation to refresh the content

                await FetchAndInsertItemsAsync(3);

                // The 'using' statement ensures the deferral is marked as complete.
                // Otherwise, you'd call
                // RefreshCompletionDeferral.Complete();
                // RefreshCompletionDeferral.Dispose();
            }
        }

        private void Visualizer_RefreshStateChanged(RefreshVisualizer sender, RefreshStateChangedEventArgs args)
        {
            // Respond to visualizer state changes.
            // Disable the refresh button if the visualizer is refreshing.
            if (args.NewState == RefreshVisualizerState.Refreshing)
            {
                RefreshButton.IsEnabled = false;
            }
            else
            {
                RefreshButton.IsEnabled = true;
            }
        }

        // App specific code to get fresh data.
        private async Task FetchAndInsertItemsAsync(int updateCount)
        {
            for (int i = 0; i < updateCount; ++i)
            {
                // Simulate delay while we go fetch new items.
                await Task.Delay(1000);
                Items.Insert(0, GetNextItem());
            }
        }

        private ListItemData GetNextItem()
        {
            return new ListItemData()
            {
                Header = "Header " + DateTime.Now.Second.ToString(),
                Date = DateTime.Now.ToString(),
                Body = DateTime.Now.ToString()
            };
        }
    }

    

}
