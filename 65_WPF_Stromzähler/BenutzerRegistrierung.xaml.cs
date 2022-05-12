using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using StromzählerContext;

namespace _65_WPF_Stromzähler
{
    public partial class BenutzerRegistrierung
    {
        private readonly ComputeStringToSha256Hash encrypting = new();

        private SzContext context = new SzContext();

        public UserLogin UserLogin { get; set; } = new();

        public BenutzerRegistrierung()
        {
            InitializeComponent();
        }

        //Buttons for this Class
        private void BtnSpeichern(object sender, RoutedEventArgs e)
        {
            var encodedPassword = encrypting.ComputeStringToSha256HashMethod(txtUserPasswordConfirm.Password);

            var userLogins = context.UserLogins.Where(x => x.Username == TxtUserNameCreate.Text).ToList();


            if (!userLogins.Any())
            {
                UserLogin = context.UserLogins.Select(x => new UserLogin
                {
                    Username = TxtUserNameCreate.Text,
                    Password = encodedPassword,
                }).FirstOrDefault();

                context.UserLogins.Add(UserLogin);

                context.SaveChanges();

                MessageBox.Show("Ihr Benutzer wurde angelegt.");
                Close();

            }
            else
            {
                MessageBox.Show("Dieser Benutzer existiert bereits. Bitte geben Sie einen anderen Benutzernamen ein.");
                TxtUserNameCreate.Clear();
                TxtUserPasswordCreate.Clear();
                txtUserPasswordConfirm.Clear();
                return;
            }

            if (TxtUserPasswordCreate.Password != txtUserPasswordConfirm.Password)
            {
                MessageBox.Show("Ihr Passwort stimmt nicht überein. Bitte wiederholen Sie ihre Eingabe.");
                txtUserPasswordConfirm.Clear();
                TxtUserPasswordCreate.Clear();
                return;
            }
        }

        private void BtnAbbrechen(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}