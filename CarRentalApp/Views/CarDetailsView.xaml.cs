using System.Windows;

namespace CarRentalApp.Views
{
    public partial class CarDetailsView : Window
    {
        public CarDetailsView(string idCar) 
        {
            InitializeComponent();
            this.DataContext = new ViewModels.CarDetailsViewModel(idCar);
        }
    }
}