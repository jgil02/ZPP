using System;
using System.Windows;

namespace CarRentalApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            base.OnStartup(e);

            this.DispatcherUnhandledException += (s, ev) =>
            {
                MessageBox.Show($"DispatcherUnhandledException:\n{ev.Exception}", "Unhandled UI exception", MessageBoxButton.OK, MessageBoxImage.Error);
                ev.Handled = true;
            };

            AppDomain.CurrentDomain.UnhandledException += (s, ev) =>
            {
                var ex = ev.ExceptionObject as Exception;
                MessageBox.Show($"UnhandledException:\n{ex}", "Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }
    }
}