using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Lab8.Models; // Не забудьте додати простір імен вашого контексту бази даних

namespace Lab8
{
    public partial class MainWindow : Window
    {
        private FootballDbContext _context;
        private IDbContextTransaction _currentTransaction;

        public ObservableCollection<Player> Stadia { get; set; }

        public Player SelectedPlayer { get; set; }

        private int _currentPage = 1;  // Поточна сторінка
        private const int _pageSize = 5;  // Кількість елементів на сторінці
        private int _totalPages = 1; // Загальна кількість сторінок

        public MainWindow()
        {
            InitializeComponent();

            Stadia = new ObservableCollection<Player>();

            _context = new FootballDbContext();

            LoadStadiums();

            DataGridStadia.ItemsSource = Stadia;
        }

        private void LoadStadiums()
        {
            var players = _context.Players.ToList();

            foreach (var player in players)
            {
                Stadia.Add(player);
            }

            DataGridStadia.ItemsSource = Stadia;
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            string position = PositionTextBox.Text.Trim();
            string salaryText = Salary.Text.Trim();

            decimal? salary = string.IsNullOrEmpty(salaryText) ? (decimal?)null : decimal.Parse(salaryText);

            Stadia.Clear();

            var playersQuery = _context.Players.AsQueryable();

            if (!string.IsNullOrEmpty(position))
            {
                playersQuery = playersQuery.Where(p => p.Position.Contains(position));
            }

            if (salary.HasValue)
            {
                playersQuery = playersQuery.Where(p => p.Salary > salary.Value);
            }

            // Розраховуємо кількість сторінок тільки для відфільтрованих даних
            int totalItems = playersQuery.Count();
            _totalPages = (int)Math.Ceiling((double)totalItems / _pageSize);

            // Завантажуємо першу сторінку результатів
            LoadPage(_currentPage);
        }

        private void LoadPage(int pageNumber)
        {
            Stadia.Clear();

            var playersQuery = _context.Players.AsQueryable();

            string position = PositionTextBox.Text.Trim();
            string salaryText = Salary.Text.Trim();
            decimal? salary = string.IsNullOrEmpty(salaryText) ? (decimal?)null : decimal.Parse(salaryText);

            if (!string.IsNullOrEmpty(position))
            {
                playersQuery = playersQuery.Where(p => p.Position.Contains(position));
            }

            if (salary.HasValue)
            {
                playersQuery = playersQuery.Where(p => p.Salary > salary.Value);
            }


            var playersOnPage = playersQuery
                .Skip((pageNumber - 1) * _pageSize)  // Пропускаємо елементи для попередніх сторінок
                .Take(_pageSize)                      // Беремо елементи для поточної сторінки
                .ToList();

            foreach (var player in playersOnPage)
            {
                Stadia.Add(player);
            }

            DataGridStadia.ItemsSource = Stadia;

            // Оновлюємо відображення кнопок навігації
            UpdatePaginationButtons();
        }

        private void UpdatePaginationButtons()
        {
            // Відключаємо/включаємо кнопки в залежності від поточної сторінки
            PreviousPageButton.IsEnabled = _currentPage > 1;
            NextPageButton.IsEnabled = _currentPage < _totalPages;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            // Зменшуємо поточну сторінку і завантажуємо її
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadPage(_currentPage);
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            // Збільшуємо поточну сторінку і завантажуємо її
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadPage(_currentPage);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _context.Dispose(); // Закриття контексту бази даних
            base.OnClosing(e);
        }



        /////////////////////////////////////////////////////////////////////////////////


        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

            // Зчитування даних з полів
            string name = NameTextBox.Text.Trim();
            string surname = SurnameTextBox.Text.Trim();
            DateTime? birthday = BirthdayPicker.SelectedDate;
            string nationality = NationalityTextBox.Text.Trim();
            string position = PositionTextBoxAdd.Text.Trim();
            string salaryText = SalaryTextBox.Text.Trim();
            string teamIdText = TeamIdTextBox.Text.Trim();

            // Перевірка на пусті поля
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || birthday == null ||
                string.IsNullOrEmpty(nationality) || string.IsNullOrEmpty(position) ||
                string.IsNullOrEmpty(salaryText) || string.IsNullOrEmpty(teamIdText))
            {
                MessageBox.Show("All fields are required. Please fill out all the fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Перевірка коректності зарплати (має бути числом)
            if (!decimal.TryParse(salaryText, out decimal salary) || salary <= 0)
            {
                MessageBox.Show("Salary must be a positive number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Перевірка коректності Team ID (має бути цілим числом)
            if (!int.TryParse(teamIdText, out int teamId) || teamId <= 0)
            {
                MessageBox.Show("Team ID must be a positive integer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Додавання нового гравця
            Player newPlayer = new Player
            {
                Name = name,
                Surname = surname,
                Birthday = DateOnly.FromDateTime(birthday.Value),
                Nationality = nationality,
                Position = position,
                Salary = salary,
                TeamId = teamId
            };

            // Імітація збереження (додайте вашу логіку додавання до бази даних або колекції)
            SavePlayer(newPlayer);

            // Повідомлення про успіх
            MessageBox.Show($"Player {name} {surname} has been added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Очищення полів після додавання
            ClearFields();
        }

        // Метод для збереження гравця (додайте свою логіку)
        private void SavePlayer(Player player)
        {
            Stadia.Add(player);
            _context.Players.Add(player);
           // _context.SaveChanges();
        }

        // Метод для очищення полів
        private void ClearFields()
        {
            NameTextBox.Text = string.Empty;
            SurnameTextBox.Text = string.Empty;
            BirthdayPicker.SelectedDate = null;
            NationalityTextBox.Text = string.Empty;
            PositionTextBoxAdd.Text = string.Empty;
            SalaryTextBox.Text = string.Empty;
            TeamIdTextBox.Text = string.Empty;
        }

        private void UpdateAdd_Click(object sender, RoutedEventArgs e)
        {
            // Отримання вибраного гравця
            Player selectedPlayer = (Player)DataGridStadia.SelectedItem;

            if (selectedPlayer == null)
            {
                MessageBox.Show("Please select a player to update.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Оновлення гравця в базі даних
            try
            {
                Player playerToUpdate = _context.Players.FirstOrDefault(p => p.PlayerId == selectedPlayer.PlayerId);

                if (playerToUpdate != null)
                {
                    // Оновлення даних гравця
                    playerToUpdate.Name = selectedPlayer.Name;
                    playerToUpdate.Surname = selectedPlayer.Surname;
                    playerToUpdate.Birthday = selectedPlayer.Birthday;
                    playerToUpdate.Nationality = selectedPlayer.Nationality;
                    playerToUpdate.Position = selectedPlayer.Position;
                    playerToUpdate.Salary = selectedPlayer.Salary;
                    playerToUpdate.TeamId = selectedPlayer.TeamId;

                  //  _context.SaveChanges();

                    MessageBox.Show($"Player {playerToUpdate.Name} {playerToUpdate.Surname} has been updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                   
                }
                else
                {
                    MessageBox.Show("Player not found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the player: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IncreaseSalary_Click(object sender, RoutedEventArgs e)
        {
            IncreaseSalariesForTopTeam(16, "2018/2019", -4);
        }

        public void IncreaseSalariesForTopTeam(int tournamentId, string season, decimal percentageIncrease)
        {
            try
            {
                // Виклик процедури
                _context.Database.ExecuteSqlRaw(
                    "CALL IncreaseSalariesForTopTeam({0}, {1}, {2})",
                    tournamentId, season, percentageIncrease);

                MessageBox.Show("Salaries successfully updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Обробка помилок на основі тексту повідомлення
                if (ex.Message.Contains("Percentage increase must be non-negative"))
                {
                    MessageBox.Show("The percentage increase cannot be negative. Please provide a valid value.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ex.Message.Contains("No team found for the given tournament and season"))
                {
                    MessageBox.Show("No team was found for the given tournament and season. Please check your input.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Інші неочікувані помилки
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void StartTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTransaction == null)
            {
                _currentTransaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
                MessageBox.Show("Транзакцію розпочато.");
            }
            else
            {
                MessageBox.Show("Транзакція вже розпочата.");
            }
        }

        private void CommitTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTransaction != null)
            {
                try
                {
                    _context.SaveChanges();
                    _currentTransaction.Commit();
                    _currentTransaction = null;
                    MessageBox.Show("Транзакцію підтверджено.");
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Помилка, блокування ресурсу іншою транзакцією: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Немає активної транзакції.");
            }
        }

        private void RollbackTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Rollback();
                _currentTransaction = null;
                MessageBox.Show("Транзакцію скасовано.");
            }
            else
            {
                MessageBox.Show("Немає активної транзакції.");
            }
        }


    }




}
