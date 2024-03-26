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



namespace MasterPlanner.Controller
{
    class NotesController
    {
        private System.Timers.Timer reminderTimer;
        private bool isReminderCheckInProgress;
        public ObservableCollection<PlannerNote> Items { get; set; }
        private readonly Dispatcher _dispatcher;

        public NotesController()
        {
            Items = new ObservableCollection<PlannerNote>();
            LoadData();
        }
        public NotesController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            Items = new ObservableCollection<PlannerNote>();
            LoadData();
        }

        public void LoadData()
        {
            using (var context = new PlannerDbContext())
            {
                var items = context.Notes.ToList();
                foreach (var item in items)
                {
                    Items.Add(item);
                }

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
                    .Where(x => x.Date == searchDate.Date)
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
                LoadData();
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
                LoadData();
            }
        }

        public void DeleteItem(PlannerNote selectedItems)
        {
            using (var context = new PlannerDbContext())
            {
                context.Entry(selectedItems).State = EntityState.Deleted;
                context.SaveChanges();
                Items.Remove(selectedItems);
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
