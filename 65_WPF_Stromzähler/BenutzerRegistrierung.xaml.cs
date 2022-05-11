using System.Data.SqlClient;
using System.Windows;

namespace _65_WPF_Stromzähler
{
    public partial class BenutzerRegistrierung
    {
        private readonly ComputeStringToSha256Hash encrypting = new();

        public BenutzerRegistrierung()
        {
            InitializeComponent();
        }

        //Buttons for this Class
        private void BtnSpeichern(object sender, RoutedEventArgs e)
        {
            var encodedPassword = encrypting.ComputeStringToSha256HashMethod(txtUserPasswordConfirm.Password);

            var sqlCmd = "SELECT * FROM UserLogin WHERE UserName = @name";

            var connection = new Connection();
            using var con = connection.ConnectionDb();
            using var cmd = new SqlCommand(sqlCmd, con);
            cmd.Parameters.AddWithValue("@name", TxtUserNameCreate.Text);
            var affectedDataSet = cmd.ExecuteScalar();


            if (affectedDataSet != null)
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

            if (affectedDataSet == null)
            {
                sqlCmd = "INSERT UserLogin (UserName, Password) VALUES(@userName,@pwd)";
                var cmd2 = new SqlCommand(sqlCmd, con);
                cmd2.Parameters.AddWithValue("@userName", TxtUserNameCreate.Text);
                cmd2.Parameters.AddWithValue("@pwd", encodedPassword);
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Ihr Benutzer wurde angelegt.");
                Close();
            }
        }

        private void BtnAbbrechen(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}