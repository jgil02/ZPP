using CarRentalApp.Data;
using CarRentalApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;

namespace CarRentalApp.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        
        [ObservableProperty]
        private object? _currentView; // Tu lądują ViewModele nowych widoków (np. AddClientViewModel)

        [ObservableProperty] private bool _isCarsVisible = true;
        [ObservableProperty] private bool _isHistoryVisible = false;
        [ObservableProperty] private bool _isModuleVisible = false; // Widoczność dla nowych modułów

        private List<CarFleet> _allCars = new();

        [ObservableProperty] private bool _isClient;
        [ObservableProperty] private bool _isWorker;

        [ObservableProperty] private bool _isComparePanelVisible;
        [ObservableProperty] private bool _isCompareListExpanded;
        public ObservableCollection<CarFleet> CompareQueue { get; } = new();

        public ObservableCollection<CarFleet> Cars { get; } = new();
        public ObservableCollection<ReservationHistoryItem> ReservationsHistory { get; set; } = new();

        
        public ObservableCollection<FilterItem> AvailableBrands { get; } = new();
        public ObservableCollection<FilterItem> AvailableModels { get; } = new();
        public ObservableCollection<FilterItem> AvailableSegments { get; } = new();
        public ObservableCollection<FilterItem> AvailableFuelTypes { get; } = new();
        public ObservableCollection<FilterItem> AvailableBodyTypes { get; } = new();
        public ObservableCollection<FilterItem> AvailableGearboxTypes { get; } = new();

        [ObservableProperty] private string _brandSummary = "Wszystkie";
        [ObservableProperty] private string _modelSummary = "Wszystkie";
        [ObservableProperty] private string _segmentSummary = "Wszystkie";
        [ObservableProperty] private string _fuelSummary = "Wszystkie";
        [ObservableProperty] private string _bodySummary = "Wszystkie";
        [ObservableProperty] private string _gearboxSummary = "Wszystkie";

        [ObservableProperty] private decimal? _priceFrom;
        [ObservableProperty] private decimal? _priceTo;

        public ObservableCollection<string> SortOptions { get; } = new()
        {
            "Nazwa (A-Z)", "Nazwa (Z-A)", "Cena (rosnąco)", "Cena (malejąco)"
        };

        [ObservableProperty] private string _selectedSortOption = "Nazwa (A-Z)";

        partial void OnSelectedSortOptionChanged(string value) => ApplyFilters();

        
        public MainViewModel()
        {
            IsClient = UserSession.CurrentClient != null;
            IsWorker = UserSession.CurrentWorker != null;

            LoadCarsFromDatabase();
        }

        // --- NOWA KOMENDA NAWIGACJI (Obsługuje menu boczne) ---
        [RelayCommand]
        private void Navigate(string target)
        {
            
            IsCarsVisible = false;
            IsHistoryVisible = false;
            IsModuleVisible = false;
            CurrentView = null;

            switch (target)
            {
                case "Samochody":
                    IsCarsVisible = true;
                    break;
                case "Rezerwacje":
                    ShowHistory(); 
                    break;
                case "DodajKlienta":
                    CurrentView = new AddClientViewModel(); // Tworzy nowy moduł
                    IsModuleVisible = true;
                    break;
                case "DodajAuto":
                    // Tu w przyszłości: CurrentView = new AddCarViewModel();
                    // IsModuleVisible = true;
                    break;
                case "Klienci":
                    // Tu w przyszłości: CurrentView = new ClientsListViewModel();
                    // IsModuleVisible = true;
                    break;
            }
        }

        
        private void LoadCarsFromDatabase()
        {
            using (var context = new AppDbContext())
            {
                var data = context.CarFleets.Include(c => c.Car).ToList();
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
            UpdateSummaries();
        }

        private void PopulateFilterList(ObservableCollection<FilterItem> collection, IEnumerable<string> items)
        {
            collection.Clear();
            foreach (var item in items.Distinct().Where(x => !string.IsNullOrEmpty(x)))
            {
                var filterItem = new FilterItem { Name = item };
                filterItem.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(FilterItem.IsSelected)) UpdateSummaries(); };
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

            var filtered = _allCars.AsEnumerable();

            if (selectedBrands.Any()) filtered = filtered.Where(c => selectedBrands.Contains(c.Car.Brand));
            if (selectedModels.Any()) filtered = filtered.Where(c => selectedModels.Contains(c.Car.Model));
            if (selectedSegments.Any()) filtered = filtered.Where(c => selectedSegments.Contains(c.Car.Segment));
            if (selectedFuels.Any()) filtered = filtered.Where(c => selectedFuels.Contains(c.Car.FuelType));
            if (selectedBodies.Any()) filtered = filtered.Where(c => selectedBodies.Contains(c.Car.BodyType));
            if (selectedGearboxes.Any()) filtered = filtered.Where(c => selectedGearboxes.Contains(c.Car.GearboxType));

            if (PriceFrom.HasValue) filtered = filtered.Where(c => c.Car.PricePerDay >= PriceFrom.Value);
            if (PriceTo.HasValue) filtered = filtered.Where(c => c.Car.PricePerDay <= PriceTo.Value);

            if (SelectedSortOption == "Nazwa (A-Z)") filtered = filtered.OrderBy(c => c.Car.Brand).ThenBy(c => c.Car.Model);
            else if (SelectedSortOption == "Nazwa (Z-A)") filtered = filtered.OrderByDescending(c => c.Car.Brand).ThenByDescending(c => c.Car.Model);
            else if (SelectedSortOption == "Cena (rosnąco)") filtered = filtered.OrderBy(c => c.Car.PricePerDay);
            else if (SelectedSortOption == "Cena (malejąco)") filtered = filtered.OrderByDescending(c => c.Car.PricePerDay);

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
            foreach (var b in AvailableBrands) b.IsSelected = false;
            foreach (var b in AvailableModels) b.IsSelected = false;
            foreach (var b in AvailableSegments) b.IsSelected = false;
            foreach (var b in AvailableFuelTypes) b.IsSelected = false;
            foreach (var b in AvailableBodyTypes) b.IsSelected = false;
            foreach (var b in AvailableGearboxTypes) b.IsSelected = false;
            PriceFrom = null; PriceTo = null;
            if (SelectedSortOption != "Nazwa (A-Z)") SelectedSortOption = "Nazwa (A-Z)";
            else ApplyFilters();
            UpdateSummaries();
        }

        [RelayCommand]
        private void ShowCars() => Navigate("Samochody");

        private void ShowHistory()
        {
            if (UserSession.CurrentClient == null) return;
            try
            {
                using (var context = new AppDbContext())
                {
                    var history = context.Reservations
                        .Where(r => r.ClientId == UserSession.CurrentClient.ClientID)
                        .Select(r => new ReservationHistoryItem
                        {
                            CarName = r.CarFleet.Car.Brand + " " + r.CarFleet.Car.Model,
                            Vin = r.CarVin,
                            Dates = r.StartDate.ToString("dd.MM.yyyy") + " - " + r.EndDate.ToString("dd.MM.yyyy"),
                            TotalPrice = r.TotalPrice
                        }).ToList();

                    ReservationsHistory.Clear();
                    foreach (var item in history) ReservationsHistory.Add(item);
                }
                IsHistoryVisible = true;
            }
            catch (System.Exception ex) { MessageBox.Show($"Błąd pobierania historii: {ex.Message}"); }
        }

        [RelayCommand]
        private void OpenReservation(string vin)
        {
            var selectedCar = Cars.FirstOrDefault(c => c.Vin == vin);
            if (selectedCar != null)
            {
                var reservationView = new Views.ReservationView(selectedCar.Vin);
                reservationView.ShowDialog();
            }
        }

        [RelayCommand]
        private void Logout()
        {
            UserSession.CurrentClient = null;
            UserSession.CurrentWorker = null;
            var oldWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is Views.MainView) ?? Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            new Views.LoginView().Show();
            oldWindow?.Close();
        }

        [RelayCommand]
        private void ShowCarDetails(string carId)
        {
            if (!string.IsNullOrEmpty(carId)) new Views.CarDetailsView(carId).ShowDialog();
        }

        public class ReservationHistoryItem
        {
            public string CarName { get; set; } = string.Empty;
            public string Vin { get; set; } = string.Empty;
            public string Dates { get; set; } = string.Empty;
            public decimal TotalPrice { get; set; }
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
                if (CompareQueue.Count >= 3) { MessageBox.Show("Max 3 auta!"); return; }
                CompareQueue.Add(car);
                car.IsSelectedForCompare = true;
            }
            IsComparePanelVisible = CompareQueue.Count > 0;
            if (CompareQueue.Count == 0) IsCompareListExpanded = false;
        }

        [RelayCommand]
        private void ToggleCompareList() => IsCompareListExpanded = !IsCompareListExpanded;

        [RelayCommand]
        private void OpenCompare()
        {
            if (CompareQueue.Count < 2) { MessageBox.Show("Wybierz min. 2 auta."); return; }
            var ids = CompareQueue.Select(q => q.CarId).Distinct().ToList();
            new Views.CompareView(ids).ShowDialog();
        }

    }
}