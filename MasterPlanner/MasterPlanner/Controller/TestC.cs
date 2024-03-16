using MasterPlanner.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Configuration;



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
            using(var context = new TestDbContext())
            {
                var items = context.Notes.ToList();
                foreach(var item in items)
                {
                    Items.Add(item);
                }

            }
        }
    }
}
