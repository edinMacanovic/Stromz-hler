using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using StromzählerContext;

namespace _65_WPF_Stromzähler;

public partial class CounterHinzufügen
{
    private readonly SzContext context = new();

    //Constructor
    public CounterHinzufügen()
    {
        InitializeComponent();
        FillCounterList();
    }

    public CounterHinzufügen(string currentCounter, int? currentCounterId) : this()
    {
        newCounterNameHinzufügen.Text = currentCounter;
        CurrentCounterId = currentCounterId;
    }

    public int? CurrentCounterId { get; set; }

    public SqlCommand Cmd { get; set; }

    //Buttons

    private void BtnAbbrechen(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void BtnSpeichern(object sender, RoutedEventArgs e)
    {
        //var connection = new Connection();
        //using var con = connection.ConnectionDb();


        //foreach (var counter in CounterList)
        //{
        //    if (newCounterNameHinzufügen.Text == counter.Name)
        //    {
        //       
        //    }

        //    if (CurrentCounterId == null)
        //    {
        //        if (string.IsNullOrEmpty(newCounterNameHinzufügen.Text))
        //        {
        //            MessageBox.Show("Bitte geben Sie einen gültigen Namen ein.");
        //            return;
        //        }

        //        if (newCounterNameHinzufügen.Text.Length > 15)
        //        {
        //            MessageBox.Show("15 Zeichen ist die maximale Zeichenlänge!");
        //            return;
        //        }
        //    }
        //}

        if (CurrentCounterId == null)
        {
            if (context.Counters.Any(x => x.Name == newCounterNameHinzufügen.Text))
            {
                MessageBox.Show("Dieser Name ist bereits vergeben, bitte versuchen Sie es erneut.");
                return;
            }

            context.Counters.Add(new Counter
            {
                Name = newCounterNameHinzufügen.Text
            });


            context.SaveChanges();
            MessageBox.Show("Ihr Zähler wurde erfolgreich hinzugefügt.");
        }
        else
        {
            context.Counters.Update(context.Counters.FirstOrDefault(x => x.ID == CurrentCounterId));
            context.SaveChanges();

            MessageBox.Show("Ihr Zähler wurde erfolgreich geupdated.");
        }

        Close();
    }

    public void FillCounterList()
    {
        context.Counters.Select(x => new Counter
        {
            Name = x.Name
        }).ToList();
    }
}