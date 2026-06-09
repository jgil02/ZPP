using CarRentalApp.Data;
using CarRentalApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CarRentalApp.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        [ObservableProperty] private object? _currentView;
        [ObservableProperty] private bool _isCarsVisible = true;
        [ObservableProperty] private bool _isHistoryVisible = false;
        [ObservableProperty] private bool _isModuleVisible = false;
        [ObservableProperty] private bool _isClientsVisible = false;
        [ObservableProperty] private string _currentPage = "Samochody";

        private List<CarFleet> _allCars = new();

        [ObservableProperty] private bool _isClient;
        [ObservableProperty] private bool _isWorker;

        [ObservableProperty] private bool _isComparePanelVisible;
        [ObservableProperty] private bool _isCompareListExpanded;

        public ObservableCollection<CarFleet> CompareQueue { get; } = new();
        public ObservableCollection<CarFleet> Cars { get; } = new();
        public ObservableCollection<ReservationHistoryItem> ReservationsHistory { get; set; } = new();

        // FILTRY
        public ObservableCollection<FilterItem> AvailableBrands { get; } = new();
        public ObservableCollection<FilterItem> AvailableModels { get; } = new();
        public ObservableCollection<FilterItem> AvailableSegments { get; } = new();
        public ObservableCollection<FilterItem> AvailableFuelTypes { get; } = new();
        public ObservableCollection<FilterItem> AvailableBodyTypes { get; } = new();
        public ObservableCollection<FilterItem> AvailableGearboxTypes { get; } = new();
        public ObservableCollection<FilterItem> AvailableStatuses { get; } = new();
        public ObservableCollection<Client> AllClients { get; } = new();
        public ObservableCollection<Client> FilteredClients { get; } = new();

        [ObservableProperty] private string _brandSummary = "Wszystkie";
        [ObservableProperty] private string _modelSummary = "Wszystkie";
        [ObservableProperty] private string _segmentSummary = "Wszystkie";
        [ObservableProperty] private string _fuelSummary = "Wszystkie";
        [ObservableProperty] private string _bodySummary = "Wszystkie";
        [ObservableProperty] private string _gearboxSummary = "Wszystkie";
        [ObservableProperty] private string _statusSummary = "Wszystkie";

        [ObservableProperty] private decimal? _priceFrom;
        [ObservableProperty] private decimal? _priceTo;
        [ObservableProperty] private string _searchClientText = "";

        public ObservableCollection<string> SortOptions { get; } = new()
        {
            "Nazwa (A-Z)", "Nazwa (Z-A)", "Cena (rosnąco)", "Cena (malejąco)"
        };

        [ObservableProperty] private string _selectedSortOption = "Nazwa (A-Z)";

        partial void OnSelectedSortOptionChanged(string value) => ApplyFilters();
        partial void OnSearchClientTextChanged(string value) => ApplyClientFilter();

        public MainViewModel()
        {
            IsClient = UserSession.CurrentClient != null;
            IsWorker = UserSession.CurrentWorker != null;
            LoadCarsFromDatabase();
        }

        [RelayCommand]
        private void Navigate(string target)
        {
            IsCarsVisible = false;
            IsHistoryVisible = false;
            IsModuleVisible = false;
            IsClientsVisible = false;
            CurrentView = null;
            CurrentPage = target;

            switch (target)
            {
                case "Samochody":
                    LoadCarsFromDatabase();
                    IsCarsVisible = true;
                    break;
                case "Rezerwacje":
                    ShowHistory();
                    break;
                case "Klienci":
                    FetchClientsFromDb();
                    IsClientsVisible = true;
                    break;
                case "DodajKlienta":
                    CurrentView = new AddClientViewModel();
                    IsModuleVisible = true;
                    break;
                case "DodajAuto":
                    CurrentView = new AddCarViewModel();
                    IsModuleVisible = true;
                    break;
            }
        }

        private void LoadCarsFromDatabase()
        {
            using (var context = new AppDbContext())
            {
                var data = context.CarFleets.AsNoTracking().Include(c => c.Car).ToList();
                var today = DateTime.Now.Date;

                var activeReservations = context.Reservations
                    .Where(r => today >= r.StartDate.Date && today <= r.EndDate.Date)
                    .ToList();

                foreach (var fleet in data)
                {
                    bool isBusyToday = activeReservations.Any(r => r.CarVin.Trim() == fleet.Vin.Trim());

                    if (isBusyToday)
                    {
                        fleet.CurrentStatus = "Wypożyczony";
                        fleet.StatusColor = "#E74C3C";
                        fleet.IsAvailable = false;
                    }
                    else
                    {
                        fleet.CurrentStatus = "Dostępny";
                        fleet.StatusColor = "#27AE60";
                        fleet.IsAvailable = true;
                    }
                }

                _allCars = data;
                Cars.Clear();
                foreach (var item in data) Cars.Add(item);
            }
            InitializeFilters();
        }

        private void InitializeFilters()
        {
            PopulateFilterList(AvailableBrands, _allCars.Select(c => c.Car.Brand));
            PopulateFilterList(AvailableModels, _allCars.Select(c => c.Car.Model));
            PopulateFilterList(AvailableSegments, _allCars.Select(c => c.Car.Segment));
            PopulateFilterList(AvailableFuelTypes, _allCars.Select(c => c.Car.FuelType));
            PopulateFilterList(AvailableBodyTypes, _allCars.Select(c => c.Car.BodyType));
            PopulateFilterList(AvailableGearboxTypes, _allCars.Select(c => c.Car.GearboxType));

            AvailableStatuses.Clear();
            string[] statuses = { "Dostępny", "Wypożyczony" };
            foreach (var s in statuses)
            {
                var item = new FilterItem { Name = s };
                item.PropertyChanged += (sender, e) => { if (e.PropertyName == nameof(FilterItem.IsSelected)) UpdateSummaries(); };
                AvailableStatuses.Add(item);
            }
            UpdateSummaries();
        }

        private void PopulateFilterList(ObservableCollection<FilterItem> collection, IEnumerable<string> items)
        {
            collection.Clear();
            foreach (var item in items.Distinct().Where(x => !string.IsNullOrEmpty(x)))
            {
                var filterItem = new FilterItem { Name = item };
                filterItem.PropertyChanged += (sender, e) => { if (e.PropertyName == nameof(FilterItem.IsSelected)) UpdateSummaries(); };
                collection.Add(filterItem);
            }
        }

        private void UpdateSummaries()
        {
            BrandSummary = GetSummaryText(AvailableBrands);
            ModelSummary = GetSummaryText(AvailableModels);
            SegmentSummary = GetSummaryText(AvailableSegments);
            FuelSummary = GetSummaryText(AvailableFuelTypes);
            BodySummary = GetSummaryText(AvailableBodyTypes);
            GearboxSummary = GetSummaryText(AvailableGearboxTypes);
            StatusSummary = GetSummaryText(AvailableStatuses);
        }

        private string GetSummaryText(ObservableCollection<FilterItem> collection)
        {
            var selectedCount = collection.Count(x => x.IsSelected);
            if (selectedCount == 0) return "Wszystkie";
            if (selectedCount == 1) return collection.First(x => x.IsSelected).Name;
            return $"{selectedCount} wybrane";
        }

        [RelayCommand]
        private void ApplyFilters()
        {
            var selectedBrands = AvailableBrands.Where(x => x.IsSelected).Select(x => x.Name).ToList();
            var selectedModels = AvailableModels.Where(x => x.IsSelected).Select(x => x.Name).ToList();
            var selectedSegments = AvailableSegments.Where(x => x.IsSelected).Select(x => x.Name).ToList();
            var selectedFuels = AvailableFuelTypes.Where(x => x.IsSelected).Select(x => x.Name).ToList();
            var selectedBodies = AvailableBodyTypes.Where(x => x.IsSelected).Select(x => x.Name).ToList();
            var selectedGearboxes = AvailableGearboxTypes.Where(x => x.IsSelected).Select(x => x.Name).ToList();
            var selectedStatuses = AvailableStatuses.Where(x => x.IsSelected).Select(x => x.Name).ToList();

            var filtered = _allCars.AsEnumerable();

            if (selectedBrands.Any()) filtered = filtered.Where(c => selectedBrands.Contains(c.Car.Brand));
            if (selectedModels.Any()) filtered = filtered.Where(c => selectedModels.Contains(c.Car.Model));
            if (selectedSegments.Any()) filtered = filtered.Where(c => selectedSegments.Contains(c.Car.Segment));
            if (selectedFuels.Any()) filtered = filtered.Where(c => selectedFuels.Contains(c.Car.FuelType));
            if (selectedBodies.Any()) filtered = filtered.Where(c => selectedBodies.Contains(c.Car.BodyType));
            if (selectedGearboxes.Any()) filtered = filtered.Where(c => selectedGearboxes.Contains(c.Car.GearboxType));

            if (selectedStatuses.Any() && selectedStatuses.Count < 2)
            {
                filtered = filtered.Where(c => selectedStatuses.Contains(c.CurrentStatus));
            }

            if (PriceFrom.HasValue) filtered = filtered.Where(c => c.Car.PricePerDay >= PriceFrom.Value);
            if (PriceTo.HasValue) filtered = filtered.Where(c => c.Car.PricePerDay <= PriceTo.Value);

            if (SelectedSortOption == "Nazwa (A-Z)")
                filtered = filtered.OrderBy(c => c.Car.Brand).ThenBy(c => c.Car.Model);
            else if (SelectedSortOption == "Nazwa (Z-A)")
                filtered = filtered.OrderByDescending(c => c.Car.Brand).ThenByDescending(c => c.Car.Model);
            else if (SelectedSortOption == "Cena (rosnąco)")
                filtered = filtered.OrderBy(c => c.Car.PricePerDay);
            else if (SelectedSortOption == "Cena (malejąco)")
                filtered = filtered.OrderByDescending(c => c.Car.PricePerDay);

            Cars.Clear();
            foreach (var item in filtered)
            {
                item.IsSelectedForCompare = CompareQueue.Any(q => q.Vin == item.Vin);
                Cars.Add(item);
            }
        }

        [RelayCommand]
        private void ClearFilters()
        {
            var allLists = new[] { AvailableBrands, AvailableModels, AvailableSegments,
                           AvailableFuelTypes, AvailableBodyTypes, AvailableGearboxTypes, AvailableStatuses };

            foreach (var list in allLists)
            {
                foreach (var item in list) item.IsSelected = false;
            }

            PriceFrom = null;
            PriceTo = null;
            SelectedSortOption = "Nazwa (A-Z)";

            UpdateSummaries();
            ApplyFilters();
        }

        [RelayCommand] private void ShowCars() => Navigate("Samochody");
        [RelayCommand] private void ShowClients() => Navigate("Klienci");

        private void ShowHistory()
        {
            if (UserSession.CurrentClient == null) return;
            try
            {
                using (var context = new AppDbContext())
                {
                    var today = DateTime.Now.Date;

                    var reservations = context.Reservations
                        .Include(r => r.CarFleet)
                            .ThenInclude(cf => cf.Car)
                        .Where(r => r.ClientId == UserSession.CurrentClient.ClientID)
                        .OrderByDescending(r => r.StartDate)
                        .ToList();

                    var history = reservations.Select(r =>
                    {
                        var next = reservations
                            .Where(nr => nr.CarVin.Trim() == r.CarVin.Trim() && nr.Id != r.Id && nr.StartDate.Date > r.EndDate.Date)
                            .OrderBy(nr => nr.StartDate)
                            .FirstOrDefault();

                        DateTime? maxAllowed = next != null ? next.StartDate.Date.AddDays(-1) : (DateTime?)null;

                        bool isFinished = r.EndDate.Date < today;
                        bool cannotExtendBecauseMaxIsBeforeToday = maxAllowed.HasValue && today > maxAllowed.Value;

                        bool showChange = !isFinished && (!maxAllowed.HasValue || today <= maxAllowed.Value);

                        return new ReservationHistoryItem
                        {
                            Id = r.Id,
                            CarName = r.CarFleet?.Car != null ? r.CarFleet.Car.Brand + " " + r.CarFleet.Car.Model : r.CarVin,
                            Vin = r.CarVin,
                            Dates = r.StartDate.ToString("dd.MM.yyyy") + " - " + r.EndDate.ToString("dd.MM.yyyy"),
                            TotalPrice = r.TotalPrice,
                            StartDate = r.StartDate,
                            EndDate = r.EndDate,
                            MaxAllowedEndDate = maxAllowed,
                            ShowChangeButton = showChange
                        };
                    }).ToList();

                    ReservationsHistory.Clear();
                    foreach (var item in history) ReservationsHistory.Add(item);
                }
                IsHistoryVisible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
            }
        }

        [RelayCommand]
        private void OpenChangeEndDate(ReservationHistoryItem item)
        {
            if (item == null) return;

            try
            {
                var owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is Views.MainView);
                var wnd = new Views.ChangeEndDateView(item.Id, item.Vin, item.EndDate);
                if (owner != null) wnd.Owner = owner;
                wnd.ShowDialog();
                ShowHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening change-date window:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FetchClientsFromDb()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var data = context.Clients.ToList();
                    AllClients.Clear();
                    foreach (var c in data) AllClients.Add(c);
                    ApplyClientFilter();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pobierania klientów: " + ex.Message);
            }
        }

        private void ApplyClientFilter()
        {
            var filtered = AllClients.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchClientText))
            {
                string search = SearchClientText.ToLower();
                filtered = filtered.Where(c =>
                    c.FirstName.ToLower().Contains(search) ||
                    c.LastName.ToLower().Contains(search) ||
                    c.Username.ToLower().Contains(search) ||
                    c.Email.ToLower().Contains(search));
            }

            FilteredClients.Clear();
            foreach (var c in filtered) FilteredClients.Add(c);
        }

        [RelayCommand]
        private void OpenReservation(string vin)
        {
            var selectedCar = Cars.FirstOrDefault(c => c.Vin == vin);
            if (selectedCar != null)
            {
                var reservationView = new Views.ReservationView(selectedCar.Vin);
                reservationView.ShowDialog();
                LoadCarsFromDatabase();
            }
        }

        [RelayCommand]
        private void Logout()
        {
            UserSession.CurrentClient = null; UserSession.CurrentWorker = null;
            new Views.LoginView().Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is Views.MainView)?.Close();
        }

        [RelayCommand]
        private void ShowCarDetails(string carId)
        {
            if (!string.IsNullOrEmpty(carId)) new Views.CarDetailsView(carId).ShowDialog();
        }

        [RelayCommand]
        private void ToggleCompare(CarFleet car)
        {
            if (car == null) return;
            if (car.IsSelectedForCompare)
            {
                var inQueue = CompareQueue.FirstOrDefault(q => q.Vin == car.Vin);
                if (inQueue != null) CompareQueue.Remove(inQueue);
                car.IsSelectedForCompare = false;
            }
            else
            {
                if (CompareQueue.Count >= 3) return;
                CompareQueue.Add(car);
                car.IsSelectedForCompare = true;
            }
            IsComparePanelVisible = CompareQueue.Count > 0;
        }

        [RelayCommand] private void ToggleCompareList() => IsCompareListExpanded = !IsCompareListExpanded;
        [RelayCommand] private void OpenCompare() => new Views.CompareView(CompareQueue.Select(q => q.CarId).ToList()).ShowDialog();

        [ObservableProperty] private bool _isDarkMode = false;

        [RelayCommand]
        private void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            string themeName = IsDarkMode ? "DarkTheme" : "LightTheme";
            var uri = new Uri($"Themes/{themeName}.xaml", UriKind.Relative);
            var resources = Application.Current.Resources;

            var oldTheme = resources.MergedDictionaries.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme.xaml"));
            if (oldTheme != null)
            {
                resources.MergedDictionaries.Remove(oldTheme);
            }
            resources.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
        }

        public class ReservationHistoryItem
        {
            public int Id { get; set; }
            public string CarName { get; set; } = string.Empty;
            public string Vin { get; set; } = string.Empty;
            public string Dates { get; set; } = string.Empty;
            public decimal TotalPrice { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime? MaxAllowedEndDate { get; set; }
            public bool ShowChangeButton { get; set; } = false;
        }
    }
}