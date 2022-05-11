using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StromzählerContext;

namespace _65_WPF_Stromzähler
{
    public partial class MainWindow
    {
        public List<Counter> Counters = new();

        public LoadTable DisplayTable = new();

        //Constructor
        public MainWindow()
        {
            InitializeComponent();
            RefreshComboBox();
            RefreshTable();
        }

        public List<CounterValue> AllDataList { get; set; }
        public Counter Data { get; set; }

        //Buttons
        private void cbCounter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCounter.SelectedItem == null)
            {
                return;
            }

            RefreshTable();
        }

        private void ClickCounterBearbeiten(object sender, RoutedEventArgs e)
        {
            AddNewCounter addNewCounter = new();
            addNewCounter.ShowDialog();
            RefreshComboBox();
        }

        private void EventAlterCounterData(object sender, MouseButtonEventArgs e)
        {
            if (TableDb.SelectedValue != null)
            {
                var secondWindow = new SecondWindow((SQLRetrievedDataList) TableDb.SelectedValue);
                secondWindow.Title = "Zählerstand bearbeiten";
                secondWindow.ShowDialog();
                RefreshComboBox();
                RefreshTable();
            }
        }

        private void BtnZählerstandAnlegen(object sender, RoutedEventArgs e)
        {
            var secondWindow = new SecondWindow(cbCounter.SelectedValue.ToString());
            secondWindow.Title = "Zählerstand Neu Anlegen";
            secondWindow.ShowDialog();
            RefreshComboBox();
            RefreshTable();
        }

        private void BtnLöschenRow(object sender, RoutedEventArgs e)
        {
            Data = (Counter) TableDb.SelectedValue;

            var connection = new Connection();

            var sqlCmd = "DELETE FROM ECounterValue WHERE ECounterValue.Id = @Id;";

            using var con = connection.ConnectionDb();
            using var cmd = new SqlCommand(sqlCmd, con);
            cmd.Parameters.AddWithValue("@Id", Data.ID);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Ihr Datensatz wurde erfolgreich gelöscht.");
            RefreshTable();
        }

        private void ClickCreateUser(object sender, RoutedEventArgs e)
        {
            Verwaltung verwaltung = new();
            verwaltung.Show();
        }

        private void ClickExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Methods for this Class
        public void RefreshTable()
        {
            var id = Counters.FirstOrDefault(c => c.Name == cbCounter.SelectedItem).ID;

            AllDataList = DisplayTable.loadTable();

            if (id != 0)
            {
                TableDb.ItemsSource = AllDataList.Where(x => x.Id == id);
            }
            else
            {
                TableDb.ItemsSource = AllDataList;
            }
        }

        public void RefreshComboBox()
        {
            Counters.Clear();
            Counters = GetCounterName();
            cbCounter.Items.Clear();

            foreach (var counter in Counters)
            {
                cbCounter.Items.Add(counter.Name);
            }

            cbCounter.SelectedIndex = 0;
        }

        public List<Counter> GetCounterName()
        {
            var result = new List<Counter>();
            var connection = new Connection();
            using var con = connection.ConnectionDb();

            var SqlCmd = "SELECT * FROM ECounter;";
            using var cmd = new SqlCommand(SqlCmd, con);
            using var reader = cmd.ExecuteReader();

            result.Add(new Counter
            {
                ID = 0,
                Name = "Select all"
            });

            while (reader.Read())
            {
                result.Add(new Counter
                {
                    ID = (int) reader["Id"],
                    Name = (string) reader["name"]
                });
            }

            return result;
        }
    }
}