using MasterPlanner.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Timers;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows;
using Npgsql.PostgresTypes;
using System.ComponentModel;



namespace MasterPlanner.Controller
{
    class NotesController : INotifyPropertyChanged
    {
        private int _currentPage = 1;
        private int _totalPages;
        private System.Timers.Timer reminderTimer;
        private bool isReminderCheckInProgress;
        public ObservableCollection<PlannerNote> Items { get; set; }
        public ObservableCollection<PlannerNote> CurrentPageItems { get; set; }
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                    UpdateCurrentPageItems();
                }
            }
        }
        public int TotalPages
        {
            get => _totalPages;
            set
            {
                if (_totalPages != value)
                {
                    _totalPages = value;
                    OnPropertyChanged(nameof(TotalPages));
                }
            }
        }
        public int ItemsPerPage { get; set; } = 15;
        private readonly Dispatcher _dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NotesController()
        {
            Items = new ObservableCollection<PlannerNote>();
            CurrentPageItems = new ObservableCollection<PlannerNote>();
            LoadData();
            CalculateTotalPages();
        }
        public NotesController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            Items = new ObservableCollection<PlannerNote>();
            CurrentPageItems = new ObservableCollection<PlannerNote>();
            LoadData();
            CalculateTotalPages();
        }
        string sort = " ";
        ListSortDirection localDirection = ListSortDirection.Ascending;
        public void LoadData(string sortBy = " ", ListSortDirection direction = ListSortDirection.Ascending)
        {
            using (var context = new PlannerDbContext())
            {
                sort = sortBy;
                localDirection = direction;
                var items = context.Notes.ToList();
                if (sortBy == "Date")
                    items.Sort(new Comparison<PlannerNote>((x, y) => DateTime.Compare(x.Date, y.Date)));
                else if (sortBy == "DateEnd")
                    items.Sort(new Comparison<PlannerNote>((x, y) => DateTime.Compare(x.DateEnd, y.DateEnd)));
                else if (sortBy == "Notes")
                    items.Sort(new Comparison<PlannerNote>((x, y) => string.Compare(x.Notes, y.Notes)));
                if (direction == ListSortDirection.Descending)
                {
                    items.Reverse();
                }
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            UpdateCurrentPageItems();
        }
        public void CalculateTotalPages()
        {
            TotalPages = (int)Math.Ceiling(Items.Count / (double)ItemsPerPage);
        }
        public void UpdateCurrentPageItems()
        {
            CurrentPageItems.Clear();
            var itemsToShow = Items.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);
            foreach (var item in itemsToShow)
            {
                CurrentPageItems.Add(item);
            }
        }

        public void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                //UpdateCurrentPageItems();
            }
        }

        public void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                //UpdateCurrentPageItems();
            }
        }

        public void ClearData()
        {
            Items.Clear();
        }


        public ObservableCollection<PlannerNote> GetItemsByDate(DateTime? date)
        {
            using (var context = new PlannerDbContext())
            {
                DateTime searchDate = DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
                var itemsByDate = context.Notes
                    .Where(x => x.Date.Date == searchDate.Date.Date)
                    .ToList();
                return new ObservableCollection<PlannerNote>(itemsByDate);
            }
        }
        public void AddItem(DateTime date, DateTime dateEnd, string notes)

        {
            using (var context = new PlannerDbContext())
            {
                PlannerNote model = new PlannerNote()
                {
                    Date = date.ToUniversalTime(),
                    DateEnd = dateEnd.ToUniversalTime(),
                    Notes = notes
                };

                context.Notes.Add(model);
                context.SaveChanges();
                Items.Add(model);
                ClearData();
                LoadData(sort, localDirection);
                CalculateTotalPages();
                UpdateCurrentPageItems();
            }
        }
        public void AddItem(PlannerNote model)

        {
            using (var context = new PlannerDbContext())
            {
                context.Notes.Add(model);
                context.SaveChanges();
                Items.Add(model);
                ClearData();
                LoadData(sort, localDirection);
                CalculateTotalPages();
                UpdateCurrentPageItems();
            }
        }

        public void DeleteItem(PlannerNote[] selectedItems)
        {
            using (var context = new PlannerDbContext())
            {
                foreach (var item in selectedItems)
                {
                    context.Entry(item).State = EntityState.Deleted;
                    context.SaveChanges();
                    Items.Remove(item);
                }
                ClearData();
                LoadData(sort, localDirection);
                CalculateTotalPages();
                UpdateCurrentPageItems();
            }
        }
        public void UpdateItem(PlannerNote updateModel)
        {
            using (var context = new PlannerDbContext())
            {
                var item = context.Notes.FirstOrDefault(x => x.Id == updateModel.Id);
                if (item is not null)
                {
                    item.Notes = updateModel.Notes;
                    context.SaveChanges();
                    var itemInCollection = Items.FirstOrDefault(x => x.Id == item.Id);
                    if (itemInCollection is not null)
                    {
                        itemInCollection.Notes = updateModel.Notes;
                    }
                }
            }
        }

        public void InitializeReminder()
        {
            reminderTimer = new System.Timers.Timer();
            reminderTimer.Elapsed += CheckReminderTime;
            reminderTimer.Interval = 1000;
            reminderTimer.Start();
        }

        private void CheckReminderTime(object sender, ElapsedEventArgs e)
        {
            if (isReminderCheckInProgress) return;
            isReminderCheckInProgress = true;
            using (var context = new PlannerDbContext())
            {
                ;
                var now = DateTime.UtcNow;
                var reminders = context.Notes.Where(x => x.ShouldRemind && !x.ReminderShown && x.ReminderDate <= now).ToList();
                if (reminders.Any())
                {
                    reminderTimer.Stop();
                    _dispatcher.Invoke(() =>
                    {
                        foreach (var reminder in reminders)
                        {
                            System.Media.SystemSounds.Exclamation.Play();
                            MessageBox.Show($"Напоминание: {reminder.Notes}");
                            reminder.ReminderShown = true;
                        }

                        context.SaveChanges();
                    });

                    reminderTimer.Start();
                }
                isReminderCheckInProgress = false;
            }
        }
        public void AddNewNote(string noteText, DateTime? selectedDate, DateTime? endDate, bool shouldRemind)
        {
            var model = new PlannerNote
            {
                Notes = noteText,
                Date = selectedDate ?? DateTime.UtcNow,
                DateEnd = endDate ?? DateTime.UtcNow,
                ShouldRemind = shouldRemind
            };

            if (shouldRemind)
            {
                // Если должны установить напоминание, то берем выбранную дату и время и преобразуем в UTC
                model.ReminderDate = selectedDate?.ToUniversalTime() ?? DateTime.UtcNow;
            }

            // Добавление записи через контроллер
            AddItem(model);
        }

    }
}
