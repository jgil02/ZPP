using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarRentalApp.Views
{
    public partial class ReservationView : Window
    {
        public ReservationView(string vin) 
        {
            InitializeComponent();

            var viewModel = new ViewModels.ReservationViewModel(vin);
            this.DataContext = viewModel;
            Title = "Rezerwacja auta: " + viewModel.CarFullName;

        }
    }
}