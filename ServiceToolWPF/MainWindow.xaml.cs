using ServiceToolWPF.Classes;
using ServiceToolWPF.Services;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.Net.Http;

namespace ServiceToolWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Declarations
        public static bool loggedIn = false;
        public static HttpClient? sharedClient = new()
        {
            BaseAddress = new Uri("http://localhost:5174/"),
        };
        public static LoggedInUserDTO? loggedInUser;
        #endregion
        #region Constuctor
        public MainWindow()
        {
            InitializeComponent();
            btnLogOut.IsEnabled = false;
            lbxLog.Items.Add("1.sor");
            lbxLog.Items.Add("2.sor");
            lbxLog.Items.Add("3.sor");
            lbxLog.Items.Add("4.sor");
            lbxLog.Items.Add("5.sor");
            lbxLog.Items.Add("6.sor");

        }
        #endregion
        #region Generate Salt/Hash
        static int SaltLength = 64;
        public static string GenerateSalt()
        {
            Random random = new Random();
            string karakterek = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string salt = "";
            for (int i = 0; i < SaltLength; i++)
            {
                salt += karakterek[random.Next(karakterek.Length)];
            }
            return salt;
        }
        public static string CreateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
        #endregion

        #region UserName/Password textbox mask settings
        private void txbUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            unTextCheck();
        }
        private void txbUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            unTextCheck();
        }
        private void unTextCheck()
        {
            if (txbUserName.Text == "")
            {
                txbUserName.Background = null;
            }
            else
            {
                txbUserName.Background = txbUserNameBackgrnd.Background;
            }
        }

        private void txbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            passTextCheck();
        }
        private void txbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passTextCheck();
        }
        private void passTextCheck()
        {
            if (txbPassword.Password == "")
            {
                txbPassword.Background = null;
            }
            else
            {
                txbPassword.Background = txbPasswordBackgrnd.Background;
            }
        }
        #endregion
        #region Login trigger events
        private void txbUserName_KeyDown(object sender, KeyEventArgs e)
        {
                        if (e.Key == Key.Enter) { UserLogin(); }
        }
        private void txbPassword_KeyDown(object sender, KeyEventArgs e)
        {
                        if (e.Key == Key.Enter) { UserLogin(); }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
                        UserLogin();
        }
        #endregion
        #region Logout trigger event
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        #region Login
        private void UserLogin()
        {
            if (txbUserName.Text != "" && txbPassword.Password != "")
            {
                var salt = LoginService.GetSalt(ServiceToolWPF.MainWindow.sharedClient, txbUserName.Text);
                if (salt != "" && salt != "Error")
                {
                    string tmpHash = MainWindow.CreateSHA256(txbPassword.Password + salt);
                    try
                    {
                        MainWindow.loggedInUser = JsonSerializer.Deserialize<LoggedInUserDTO>(LoginService.Login(ServiceToolWPF.MainWindow.sharedClient, txbUserName.Text, tmpHash));
                        if (MainWindow.loggedInUser?.Token != "")
                        {
                            MainWindow.loggedIn = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
                if (!MainWindow.loggedIn)
                {
                        MessageBox.Show("Incorrect username or password!");
                }
            }
        }
        #endregion

    }
}