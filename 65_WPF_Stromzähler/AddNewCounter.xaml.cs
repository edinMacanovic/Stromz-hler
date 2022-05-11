using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using StromzählerContext;

namespace _65_WPF_Stromzähler
{
    public partial class AddNewCounter
    {
        //Constructor
        public AddNewCounter()
        {
            InitializeComponent();
            DataContext = this;
            counterTable.ItemsSource =  LoadCounterTable();
            Value = FillECounterValue();
        }

        public int? Value { get; set; }

        public List<Counter> Counters { get; set; } = new();

        private SzContext context = new SzContext();

        public Counter Data { get; set; }

        //Buttons
        private void BtnCounterAnlegen(object sender, RoutedEventArgs e)
        {
            CounterHinzufügen counterHinzufügen = new();
            counterHinzufügen.ShowDialog();
            counterTable.ItemsSource = LoadCounterTable();
        }

        private void BtnCounterLöschen(object sender, RoutedEventArgs e)
        {
            //var connection = new Connection();
            //using var con = connection.ConnectionDb();
            Data = (Counter) counterTable.SelectedValue;

            var test = context.Counters.First(x => x.ID == Data.ID);

            context.Counters.Remove(test);

            //var sqlStringGetAmount = "SELECT count(*) FROM ECounterValue where CounterId = @id";
            //var cmdGetAmount = new SqlCommand(sqlStringGetAmount, con);
            //cmdGetAmount.Parameters.AddWithValue("@id", Data.ID);
            //var reader = cmdGetAmount.ExecuteScalar();

            //if ((int) reader == 0)
            //{
            //    var sqlCmd2 = "Delete from ECounter Where Id = @id";
            //    var cmd = new SqlCommand(sqlCmd2, con);
            //    cmd.Parameters.AddWithValue("@id", Data.ID);
            //    cmd.ExecuteNonQuery();
            //    MessageBox.Show("Ihr Zähler wurde erfolgreich entfernt.");
            //    LoadCounterTable();
            //}
            //else
            //{
            //    var result = MessageBox.Show(
            //        "Wenn Sie einen Zähler löschen werden alle dazugehörigen Zählerständer auch gelöscht. " +
            //        "\n\n Wollen Sie Fortfahren?", "My App", MessageBoxButton.YesNo);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        Data = (Counter) counterTable.SelectedValue;

            //        var sqlCmd1 = "Delete from ECounterValue Where CounterId = @id";
            //        var cmd = new SqlCommand(sqlCmd1, con);
            //        cmd.Parameters.AddWithValue("@id", Data.ID);
            //        cmd.ExecuteNonQuery();

            //        var sqlCmd2 = "Delete from ECounter Where Id = @id";
            //        cmd = new SqlCommand(sqlCmd2, con);
            //        cmd.Parameters.AddWithValue("@id", Data.ID);
            //        cmd.ExecuteNonQuery();
            //        MessageBox.Show("Ihr Zähler wurde erfolgreich entfernt.");
            //        LoadCounterTable();
            //    }
            //}

            context.SaveChanges();
            counterTable.ItemsSource = LoadCounterTable();

        }


        //Methods for this Class
        public List<Counter> LoadCounterTable()
        {
            return context.Counters.Select(x => new Counter
            {
                ID = x.ID,
                Name = x.Name,
            }).ToList();


        }

        private int? FillECounterValue()
        {
            return context.CounterValues.Select(x => x.Id).FirstOrDefault();

        }

        private void Bearbeiten(object sender, MouseButtonEventArgs e)
        {
            if (counterTable.SelectedValue != null)
            {
                var currentSelectedValueFromCounter = (Counter) counterTable.SelectedValue;
                var currentCounterName = currentSelectedValueFromCounter.Name;
                var currentCounterId = currentSelectedValueFromCounter.ID;

                var counterHinzufügen = new CounterHinzufügen(currentCounterName, currentCounterId);
                counterHinzufügen.ShowDialog();
                LoadCounterTable();
            }
        }
    }
}