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
            FillUserTable();
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
            var test = context.UserLogins.Select(x => x).ToList();

            var listeUser = new List<UserLogin>();

            foreach (var user in test)
            {
                listeUser.Add(new UserLogin
                {
                    Username = user.Username
                });
            }

            return (List<UserLogin>) (userTable.ItemsSource = listeUser);
        }
    }
}