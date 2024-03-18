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



namespace MasterPlanner.Controller
{
    class TestC
    {
        public ObservableCollection<TestModel> Items { get; set; }
        public TestC()
        {
            Items = new ObservableCollection<TestModel>();
            LoadData();
        }

        public void LoadData()
        {
            using (var context = new TestDbContext())
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
        public void AddItem(DateTime date, string notes)
        {
            using (var context = new TestDbContext())
            {
                TestModel model = new TestModel()
                {
                    Date = date.ToUniversalTime(),
                    Notes = notes
                };

                context.Notes.Add(model);
                context.SaveChanges();
                Items.Add(model);
                ClearData();
                LoadData();
            }
        }

        public void DeleteItem(TestModel selectedItems)
        {
            using (var context = new TestDbContext())
            {
                context.Entry(selectedItems).State = EntityState.Deleted;
                context.SaveChanges();
                Items.Remove(selectedItems);
            }
        }
        public void UpdateItem(TestModel updateModel)
        {
            using (var context = new TestDbContext())
            {
                var item = context.Notes.FirstOrDefault(x=>x.Id == updateModel.Id);
                if(item is not null)
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

    }
}
