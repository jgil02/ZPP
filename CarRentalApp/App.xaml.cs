using System.Windows;

namespace CarRentalApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Pe³na kwalifikacja eliminuje problem z brakiem using/typu
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            base.OnStartup(e);
        }
    }
}