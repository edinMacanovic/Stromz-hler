﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using StromzählerContext;
using System.Linq;

namespace _65_WPF_Stromzähler
{
    public partial class Login
    {
        private readonly ComputeStringToSha256Hash encrypting = new();
        private readonly MainWindow mainWindow = new();
        private SzContext context = new();

        public List<UserLogin> UserList = new();

        public Login()
        {
            InitializeComponent();
        }

        //Buttons for this Class
        private void BtnAnmelden(object sender, RoutedEventArgs e)
        {
            var encodedPassword = encrypting.ComputeStringToSha256HashMethod(txtUserPassword.Password);

            var userLogin = context.UserLogins.Where(x => x.Username == TxtUserName.Text && x.Password == encodedPassword).ToList();

            if (userLogin.Count() != 0)
            {
                Close();
                mainWindow.Show();
                return;
            }

            TxtUserName.Clear();
            txtUserPassword.Clear();
            MessageBox.Show("Bitte geben Sie einen gültigen Benutzernamen oder ein gültiges Passwort ein.");
        }

        private void BtnAbbrechen(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}