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
            var vm = new ViewModels.ReservationViewModel(vin);
            this.DataContext = vm;

            this.Loaded += (s, e) =>
            {

                var startBinding = StartDatePicker.GetBindingExpression(DatePicker.SelectedDateProperty);
                var endBinding = EndDatePicker.GetBindingExpression(DatePicker.SelectedDateProperty);

                StartDatePicker.SelectedDate = null;
                EndDatePicker.SelectedDate = null;

                foreach (var range in vm.OccupiedDates)
                {
                    if (range.Start <= range.End)
                    {
                        try
                        {
                            var blackout = new CalendarDateRange(range.Start.Date, range.End.Date);
                            StartDatePicker.BlackoutDates.Add(blackout);
                            EndDatePicker.BlackoutDates.Add(blackout);
                        }
                        catch (Exception) {}
                    }
                }

                StartDatePicker.SelectedDate = vm.StartDate;
                EndDatePicker.SelectedDate = vm.EndDate;
            };
        }

    }
}