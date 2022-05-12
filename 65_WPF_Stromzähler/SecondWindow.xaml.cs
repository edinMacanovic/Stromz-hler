using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using StromzählerContext;

namespace _65_WPF_Stromzähler
{
    public partial class SecondWindow
    {
        private SqlCommand cmd;
        private Counter currentCounter;
        public MainWindow MainWindow = new();
        private SzContext context = new SzContext();

        public List<Counter> Counters { get; set; } = new();
        public CounterValue CounterValue { get; set; } = new();
        public char Letter { get; set; }

        //Contructor01
        public SecondWindow(string cbSelection = "")
        {
            InitializeComponent();
            calBoxDate.DisplayDate = DateTime.Now;

            foreach (var counter in GetCounterName())
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
                    currentCounter = context.Counters.Select(x => new Counter
                    {
                        ID = x.ID,
                        Name = cbSelection
                    }).FirstOrDefault();
                }
            }
        }

        //Contructor02
        public SecondWindow(Counter data) : this()
        {
            data = context.Counters.Select(x => new Counter
            {
                ID = x.ID,
                Name = x.Name
            }).FirstOrDefault();

            txtBoxCounter.Value = CounterValue.Value;
            calBoxDate.SelectedDate = CounterValue.Date;


            foreach (RadioButton item in radioButtons.Children)
            {
                if (item.Content.Equals(CounterValue.Name))
                {
                    currentCounter = context.Counters.FirstOrDefault(x => x.ID == data.ID);
                    item.IsChecked = true;
                }
            }
        }



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



            context.SaveChanges();

            if (CounterValue.Id != 0)
            {
                context.CounterValues.Update(context.);
            }
            else
            {
                context.CounterValues.Add(new CounterValue
                {
                    Name = MainWindow.cbCounter.SelectedValue.ToString(),
                    CounterId = currentCounter.ID,
                    Value = txtBoxCounter.Value,
                    Date = calBoxDate.SelectedDate,

                });
            }


            //var connection = new Connection();
            //using var con = connection.ConnectionDb();
            //string sqlCmd;


            //if (Data != null)
            //{
            //    sqlCmd = "UPDATE ECounterValue SET Date = @date, Value = @value, CounterId = @counterId " +
            //             "WHERE Id = @id";
            //    cmd = new SqlCommand(sqlCmd, con);
            //    cmd.Parameters.AddWithValue("@date", calBoxDate.SelectedDate);
            //    cmd.Parameters.AddWithValue("@value", txtBoxCounter.Value);
            //    cmd.Parameters.AddWithValue("@id", Data.ID);
            //    cmd.Parameters.AddWithValue("@counterId", currentCounter.ID);
            //    cmd.ExecuteNonQuery();
            //    MessageBox.Show("Ihre Daten wurden Erfolgreich geupdated.");
            //}
            //else
            //{
            //    if (currentCounter != null)
            //    {
            //        sqlCmd = "INSERT INTO ECounterValue (CounterId, Date, Value) VALUES (@counterId, @date, @value);";
            //        cmd = new SqlCommand(sqlCmd, con);
            //        cmd.Parameters.AddWithValue("@counterId", currentCounter.ID);
            //        cmd.Parameters.AddWithValue("@date", calBoxDate.SelectedDate);
            //        cmd.Parameters.AddWithValue("@value", txtBoxCounter.Value);
            //        cmd.ExecuteNonQuery();
            //        MessageBox.Show("Ihre Daten wurden Erfolgreich gespeichert.");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Bitte zuerst einen Stromzähler anlegen");
            //    }
            //}

            Close();
        }


        private void btnAbbrechen(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Methods for this Class
        public List<Counter> GetCounterName()
        {
            return context.Counters.Select(x => new Counter
            {
                ID = x.ID,
                Name = x.Name,
            }).ToList();
        }
    }
}