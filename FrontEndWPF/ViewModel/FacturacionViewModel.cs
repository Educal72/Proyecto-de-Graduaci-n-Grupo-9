using FrontEndWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FrontEndWPF.ViewModel
{
    public class FacturacionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<Producto> _productosCarrito;
        public ObservableCollection<Producto> ProductosCarrito
        {
            get => _productosCarrito;
            set
            {
                _productosCarrito = value;
                OnPropertyChanged();
            }
        }

        public FacturacionViewModel()
        {
            
        }
    }
}
