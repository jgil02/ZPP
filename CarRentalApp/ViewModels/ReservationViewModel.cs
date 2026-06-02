using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarRentalApp.Data;
using CarRentalApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace CarRentalApp.ViewModels
{
    public partial class ReservationViewModel : BaseViewModel
    {
        private readonly AppDbContext _context = new();
        private readonly decimal _pricePerDay;

        public string CarVin { get; }
        public string CarFullName { get; private set; } = string.Empty;

        [ObservableProperty]
        private DateTime _startDate = DateTime.Now;

        [ObservableProperty]
        private DateTime _endDate = DateTime.Now.AddDays(3); // Domyślnie na 3 dni

        [ObservableProperty]
        private decimal _totalPrice;

        public ReservationViewModel(string vin)
        {
            CarVin = vin;

            var carFleet = _context.CarFleets.Include(c => c.Car).FirstOrDefault(c => c.Vin == vin);
            if (carFleet != null)
            {
                _pricePerDay = carFleet.Car.PricePerDay;
                CarFullName = $"{carFleet.Car.Brand} {carFleet.Car.Model}";
            }

            CalculatePrice();
        }

        partial void OnStartDateChanged(DateTime value) => CalculatePrice();
        partial void OnEndDateChanged(DateTime value) => CalculatePrice();

        private void CalculatePrice()
        {
            int days = (EndDate.Date - StartDate.Date).Days + 1;

            if (days < 1) days = 1;

            TotalPrice = days * _pricePerDay;
        }

        [RelayCommand]
        private void ConfirmReservation(Window window)
        {
            if (UserSession.CurrentClient == null && UserSession.CurrentWorker == null)
            {
                MessageBox.Show("Błąd sesji: Nie jesteś zalogowany!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (StartDate.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Data odbioru nie może być w przeszłości!", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (EndDate.Date < StartDate.Date)
            {
                MessageBox.Show("Data zwrotu nie może być wcześniejsza niż data odbioru!", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool isAlreadyReserved = _context.Reservations.Any(r =>
                    r.CarVin == CarVin &&
                    StartDate.Date <= r.EndDate.Date &&
                    EndDate.Date >= r.StartDate.Date);

                if (isAlreadyReserved)
                {
                    MessageBox.Show("Niestety, to auto jest już zarezerwowane w wybranym terminie. Wybierz inne daty.",
                                    "Brak dostępności", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var reservation = new Reservation
                {
                    CarVin = CarVin,
                    ClientId = UserSession.CurrentClient?.ClientID ?? 1,
                    WorkerId = UserSession.CurrentWorker?.WorkerID ?? 1,
                    StartDate = StartDate.Date,
                    EndDate = EndDate.Date,
                    TotalPrice = TotalPrice
                };

                _context.Reservations.Add(reservation);

                _context.SaveChanges();

                // Generowanie PDF po zapisaniu rezerwacji
                GenerateReservationPdf(reservation);

                MessageBox.Show("Rezerwacja zakończona sukcesem!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                window?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisu do bazy: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateReservationPdf(Reservation reservation)
        {
            try
            {
                var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var folder = Path.Combine(docs, "CarRental", "Reservations");
                Directory.CreateDirectory(folder);

                var fileName = $"Reservation_{reservation.Id}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                var path = Path.Combine(folder, fileName);

                var client = _context.Clients.Find(reservation.ClientId);
                var worker = _context.Workers.Find(reservation.WorkerId);

                // Pobierz dane samochodu i ścieżkę do obrazka
                var carFleet = _context.CarFleets.Include(cf => cf.Car).FirstOrDefault(cf => cf.Vin == reservation.CarVin);
                var car = carFleet?.Car;
                string? imagePathRaw = car?.ImagePath;

                // Spróbuj zlokalizować plik obrazu (kilka wariantów ścieżki)
                string? imageFullPath = null;
                if (!string.IsNullOrWhiteSpace(imagePathRaw))
                {
                    var trimmed = imagePathRaw.TrimStart('/', '\\');
                    var baseDir = AppDomain.CurrentDomain.BaseDirectory;

                    var candidate1 = Path.Combine(baseDir, trimmed);
                    var candidate2 = Path.Combine(baseDir, "Images", Path.GetFileName(trimmed));
                    var candidate3 = Path.Combine(Directory.GetCurrentDirectory(), "Images", Path.GetFileName(trimmed));

                    if (File.Exists(candidate1)) imageFullPath = candidate1;
                    else if (File.Exists(candidate2)) imageFullPath = candidate2;
                    else if (File.Exists(candidate3)) imageFullPath = candidate3;
                }

                byte[]? imageBytes = null;
                if (!string.IsNullOrEmpty(imageFullPath))
                    imageBytes = File.ReadAllBytes(imageFullPath);

                var clientInfo = client != null
                    ? $"{client.FirstName} {client.LastName}\nEmail: {client.Email}\nTelefon: {client.Phone}\n{client.City}, {client.Street} {client.HouseNumber}"
                    : "Klient: —";

                var workerInfo = worker != null
                    ? $"{worker.FirstName} {worker.LastName}\nPozycja: {worker.Position}"
                    : "Pracownik: —";

                var carDisplay = car != null
                    ? $"{car.Brand} {car.Model} ({reservation.CarVin})"
                    : $"{reservation.CarVin}";

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.DefaultTextStyle(x => x.FontSize(12));
                        page.Header().Text("Potwierdzenie rezerwacji").FontSize(20).SemiBold().AlignCenter();

                        page.Content().PaddingTop(10).Column(column =>
                        {
                            column.Spacing(10);

                            // Górny wiersz: zdjęcie po lewej, podsumowanie po prawej
                            column.Item().Row(row =>
                            {
                                row.ConstantColumn(180).Height(120).Element(el =>
                                {
                                    if (imageBytes != null)
                                    {
                                        el.AlignMiddle().AlignLeft().Image(imageBytes, ImageScaling.FitArea);
                                    }
                                    else
                                    {
                                        el.AlignMiddle().AlignLeft().Text("[Brak zdjęcia]").Italic().FontSize(10);
                                    }
                                });

                                row.RelativeColumn().Column(right =>
                                {
                                    right.Item().Text("Szczegóły rezerwacji").SemiBold().FontSize(14);
                                    right.Item().Text($"Rezerwacja ID: {reservation.Id}").FontSize(12);
                                    right.Item().Text($"Samochód: {carDisplay}").FontSize(12);
                                    right.Item().Text($"Data odbioru: {reservation.StartDate:yyyy-MM-dd}").FontSize(12);
                                    right.Item().Text($"Data zwrotu: {reservation.EndDate:yyyy-MM-dd}").FontSize(12);
                                    right.Item().Text($"Cena całkowita: {reservation.TotalPrice:C}").FontSize(12).SemiBold();
                                });
                            });

                            column.Item().PaddingTop(8).LineHorizontal(1);

                            // Sekcja: Dane klienta
                            column.Item().PaddingTop(6).Text("Dane klienta").SemiBold().FontSize(14);
                            column.Item().Text(clientInfo).FontSize(12);

                            // Sekcja: Dane pracownika
                            column.Item().PaddingTop(6).Text("Dane pracownika").SemiBold().FontSize(14);
                            column.Item().Text(workerInfo).FontSize(12);

                            // Stopka z metadanymi
                            column.Item().PaddingTop(12).Text($"Wygenerowano: {DateTime.Now:yyyy-MM-dd HH:mm}").FontSize(10).AlignLeft();
                        });

                        page.Footer().AlignCenter().Text("Potwierdzenie wygenerowane przez CarRentalApp");
                    });
                });

                document.GeneratePdf(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas generowania PDF: {ex.Message}", "Błąd PDF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}