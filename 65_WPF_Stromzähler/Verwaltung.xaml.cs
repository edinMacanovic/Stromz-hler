using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using StromzählerContext;

namespace _65_WPF_Stromzähler
{
    public partial class Verwaltung
    {
        private SzContext context = new SzContext();

        public Verwaltung()
        {
            InitializeComponent();
            userTable.ItemsSource = FillUserTable();
        }

        private void BtnCreateNewUser(object sender, RoutedEventArgs e)
        {
            BenutzerRegistrierung benutzerRegistrierung = new();
            benutzerRegistrierung.ShowDialog();
            FillUserTable();
        }

        //Buttons for this Class
        public List<UserLogin> FillUserTable()
        {
            return context.UserLogins.Select(x => new UserLogin
            {
                Username = x.Username
            }).ToList();

        }
    }
}