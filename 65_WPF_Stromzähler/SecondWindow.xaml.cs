using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StromzählerContext;

namespace _65_WPF_Stromzähler
{
    public partial class SecondWindow
    {
        private SqlCommand cmd;
        private Counter currentCounter;
        public MainWindow MainWindow = new();

        //Contructor01
        public SecondWindow(string cbSelection = "")
        {
            InitializeComponent();
            GetCounterName();
            calBoxDate.DisplayDate = DateTime.Now;

            var currentCbSelection = MainWindow.cbCounter.SelectedValue;

            foreach (var counter in Counters)
            {
                var rb = new RadioButton
                {
                    Content = counter.Name
                };
                rb.Margin = new Thickness(5);
                rb.Checked += (s, e) => { currentCounter = counter; };

                if (currentCounter == null)
                {
                    rb.IsChecked = true;
                    currentCounter = counter;
                }

                radioButtons.Children.Add(rb);
            }

            foreach (RadioButton item in radioButtons.Children)
            {
                if (cbSelection == item.Content.ToString())
                {
                    currentCounter = Counters.FirstOrDefault(x => x.Name == cbSelection);
                }
            }
        }

        //Contructor02
        public SecondWindow(SQLRetrievedDataList data) : this()
        {
            Data = data;
            txtBoxCounter.Value = Data.Value;
            calBoxDate.SelectedDate = Data.Date;


            foreach (RadioButton item in radioButtons.Children)
            {
                if (item.Content.Equals(Data.Name))
                {
                    currentCounter = Counters.FirstOrDefault(x => x.ID == Data.CounterId);
                    item.IsChecked = true;
                }
            }
        }

        public List<Counter> Counters { get; set; } = new();
        public SQLRetrievedDataList Data { get; set; }
        public char Letter { get; set; }

        //Buttons
        private void btnSpeichern(object sender, RoutedEventArgs e)
        {
            if (txtBoxCounter.Value % 1 != 0)
            {
                MessageBox.Show("Bitte geben Sie eine Ganzzahl ein.");
                return;
            }

            if (calBoxDate.SelectedDate == null)
            {
                MessageBox.Show("Bitte wählen Sie ein Datum aus.");
                return;
            }

            if (calBoxDate.SelectedDate.Value.Year <= 1754 || calBoxDate.SelectedDate.Value.Year >= 9999)
            {
                MessageBox.Show("Bitte wählen Sie ein gültiges Datum zwischen 01.01.1753 und 31.12.9999 aus.");
                return;
            }

            if (txtBoxCounter.Value == null || txtBoxCounter.Value >= 999999 || txtBoxCounter.Value == 0)
            {
                MessageBox.Show("Bitte geben Sie eine Gültige Zahl ein.");
                return;
            }

            var connection = new Connection();
            using var con = connection.ConnectionDb();
            string sqlCmd;


            if (Data != null)
            {
                sqlCmd = "UPDATE ECounterValue SET Date = @date, Value = @value, CounterId = @counterId " +
                         "WHERE Id = @id";
                cmd = new SqlCommand(sqlCmd, con);
                cmd.Parameters.AddWithValue("@date", calBoxDate.SelectedDate);
                cmd.Parameters.AddWithValue("@value", txtBoxCounter.Value);
                cmd.Parameters.AddWithValue("@id", Data.ID);
                cmd.Parameters.AddWithValue("@counterId", currentCounter.ID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Ihre Daten wurden Erfolgreich geupdated.");
            }
            else
            {
                if (currentCounter != null)
                {
                    sqlCmd = "INSERT INTO ECounterValue (CounterId, Date, Value) VALUES (@counterId, @date, @value);";
                    cmd = new SqlCommand(sqlCmd, con);
                    cmd.Parameters.AddWithValue("@counterId", currentCounter.ID);
                    cmd.Parameters.AddWithValue("@date", calBoxDate.SelectedDate);
                    cmd.Parameters.AddWithValue("@value", txtBoxCounter.Value);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ihre Daten wurden Erfolgreich gespeichert.");
                }
                else
                {
                    MessageBox.Show("Bitte zuerst einen Stromzähler anlegen");
                }
            }

            Close();
        }


        private void btnAbbrechen(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Methods for this Class
        public void GetCounterName()
        {
            var connection = new Connection();
            using var con = connection.ConnectionDb();

            var sqlCmd = "SELECT * FROM ECounter;";
            using var cmd = new SqlCommand(sqlCmd, con);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Counters.Add(new Counter
                {
                    ID = (int) reader["Id"],
                    Name = (string) reader["name"]
                });
            }
        }
    }
}