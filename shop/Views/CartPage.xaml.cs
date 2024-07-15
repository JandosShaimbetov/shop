using shop.Models;
using shop.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace shop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CartPage : Page
    {
        public CartPage()
        {
            this.InitializeComponent();
        }
        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cartItem = button?.DataContext as CartItem;

            if (cartItem != null)
            {
                var viewModel = DataContext as StoreViewModel;
                viewModel?.RemoveFromCart(cartItem);
            }
        }
    }
}
