using Windows.UI.Xaml.Controls;

namespace shop.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(StorePage));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                switch (selectedItem.Tag)
                {
                    case "StorePage":
                        ContentFrame.Navigate(typeof(StorePage));
                        break;

                    case "CartPage":
                        ContentFrame.Navigate(typeof(CartPage));
                        break;
                }
            }
        }
    }
}
