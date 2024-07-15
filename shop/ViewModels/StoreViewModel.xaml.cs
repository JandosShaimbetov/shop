using shop.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace shop.ViewModels
{
    public sealed partial class StoreViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();
        public ObservableCollection<CartItem> Cart { get; private set; } = new ObservableCollection<CartItem>();

        private string _sortBy;
        private bool _isAscending;

        public StoreViewModel()
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await LoadProductsAsync();
            await LoadCartAsync();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await localFolder.GetFileAsync("products.json");
                string json = await FileIO.ReadTextAsync(file);

                var products = JsonSerializer.Deserialize<ObservableCollection<Product>>(json);

                foreach (var product in products)
                {
                    product.FullImagePath = Path.Combine(localFolder.Path, product.ImagePath);
                    Products.Add(product);
                }
            }
            catch (FileNotFoundException ex)
            {
                await ShowErrorMessageAsync("Файл продуктов не найден. Пожалуйста, убедитесь, что файл существует.");
            }
            catch (Exception ex)
            {
                await ShowErrorMessageAsync($"Произошла ошибка при загрузке продуктов");
            }
        }

        private async Task LoadCartAsync()
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await localFolder.GetFileAsync("cart.json");
                string json = await FileIO.ReadTextAsync(file);

                var cart = JsonSerializer.Deserialize<ObservableCollection<CartItem>>(json);

                foreach (var item in cart)
                {
                    item.Product = Products.FirstOrDefault(i => i.Id == item.ProductId);
                    Cart.Add(item);
                }
            }
            catch (FileNotFoundException ex)
            {
                await ShowErrorMessageAsync("Файл корзины не найден. Пожалуйста, убедитесь, что файл существует.");
            }
            catch (Exception ex)
            {
                await ShowErrorMessageAsync($"Произошла ошибка при загрузке корзины");
            }
        }


        public void AddToCart(Product product)
        {
            var item = Cart.FirstOrDefault(i => i.ProductId == product.Id);
            if (item == null)
            {
                Cart.Add(new CartItem { Id = Guid.NewGuid().ToString(), Product = product, Quantity = 1, ProductId = product.Id });
            }
            else
            {
                item.Quantity++;
            }
            SaveCart();
            OnPropertyChanged(nameof(Cart));
            OnPropertyChanged(nameof(TotalPrice));
        }

        public void RemoveFromCart(CartItem item)
        {
            if (item.Quantity > 1)
            {
                item.Quantity--;
                OnPropertyChanged(nameof(item.Quantity));
            }
            else
            {
                Cart.Remove(item);
            }
            SaveCart();
            OnPropertyChanged(nameof(Cart));
            OnPropertyChanged(nameof(TotalPrice));
        }

        private async void SaveCart()
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await localFolder.CreateFileAsync("cart.json", CreationCollisionOption.ReplaceExisting);
                string json = JsonSerializer.Serialize(Cart);
                await FileIO.WriteTextAsync(file, json);
            }
            catch (Exception ex)
            {
                await ShowErrorMessageAsync($"Произошла ошибка при сохранении корзины: {ex.Message}");
            }
        }

        public double TotalPrice => Cart.Sum(i => i.TotalPrice);

        private async Task ShowErrorMessageAsync(string message)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog(message, "Ошибка");
            await messageDialog.ShowAsync();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
