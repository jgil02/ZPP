using System.Collections.Generic;
using System.Windows;

namespace CarRentalApp.Views
{
    public partial class CompareView : Window
    {
        public CompareView(List<string> carIds)
        {
            InitializeComponent();

            this.DataContext = new ViewModels.CompareViewModel(carIds);
        }
    }
}