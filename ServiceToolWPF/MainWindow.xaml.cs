﻿using ServiceToolWPF.Classes;
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
using Microsoft.Win32;
using System.IO;
using System.Drawing;

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
            if (btnLogin.Content.ToString() == "Login")
            {
                UserLogin();
            }
            else
            {
                UserLogout();
            }
        }
        #endregion
        #region Login
        private void UserLogin()
        {
            if (txbUserName.Text != "" && txbPassword.Password != "")
            {
                WriteLog($"Login: {txbUserName.Text}");
                lbxLog.Items.Refresh();
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
                            WriteLog("Succesfull login!");
                            WriteLog($"Logged in user: {txbUserName.Text} token: {MainWindow.loggedInUser.Token.ToString()}");
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
                if (MainWindow.loggedIn)
                {
                    txbUserName.Focusable = false;
                    txbPassword.Focusable = false;
                    btnLogin.Content = "Logout";
                }
                else
                {
                    WriteLog("Incorrect username or password!");
                }
            }
        }
        #endregion
        #region Logout
        public void UserLogout()
        {
            txbUserName.Focusable = true;
            txbPassword.Focusable = true;
            btnLogin.Content = "Login";
        }
        #endregion 
        #region LogWindow
        public void WriteLog(string line)
        {
            lbxLog.Items.Add($"{DateTime.Now.ToString()}> {line}");
        }
        private void btnClearLog_Click(object sender, RoutedEventArgs e)
        {
            lbxLog.Items.Clear();
        }
        private void btnSaveLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "txt | *.txt";
                saveFileDialog1.Title = "Save log";
                saveFileDialog1.FileName = "log " + DateTime.Now.ToString();
                if (saveFileDialog1.ShowDialog() == true)
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                    foreach (string line in lbxLog.Items)
                    {
                        sw.WriteLine(line);
                    }
                    sw.Close();
                }
            }
            catch
            {
                WriteLog("Save log error!");
            }
        }
        #endregion
        #region Registration input mask settings
        private void txbRegUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            RegUsernameTextCheck();
        }
        private void txbRegUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegUsernameTextCheck();
        }
        private void RegUsernameTextCheck()
        {
            if (txbRegUserName.Text == "")
            {
                txbRegUserName.Background = null;
            }
            else
            {
                txbRegUserName.Background = txbUserNameBackgrnd.Background;
            }
        }
        private void txbRegPassword1_GotFocus(object sender, RoutedEventArgs e)
        {
            RegPassword1TextCheck();
            RegPassword2TextCheck();
        }
        private void txbRegPassword1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            RegPassword1TextCheck();
            RegPassword2TextCheck();
        }
        private void RegPassword1TextCheck()
        {
            if (txbRegPassword1.Password == "")
            {
                txbRegPassword1.Background = null;
            }
            else
            {
                txbRegPassword1.Background = txbPasswordBackgrnd.Background;
            }
        }
        private void txbRegPassword2_GotFocus(object sender, RoutedEventArgs e)
        {
            RegPassword2TextCheck();
        }
        private void txbRegPassword2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            RegPassword2TextCheck();
        }
        private void RegPassword2TextCheck()
        {
            if (txbRegPassword2.Password == "")
            {
                txbRegPassword2.Background = null;
            }
            else
            {
                if (txbRegPassword1.Password == txbRegPassword2.Password)
                {
                    txbRegPassword2.Background = txbUserNameBackgrnd.Background;
                }
                else
                {
                    txbRegPassword2.Background = Brushes.LightPink;
                }
            }
        }

        private void txbRegName_GotFocus(object sender, RoutedEventArgs e)
        {
            RegNameTextCheck();
        }

        private void txbRegName_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegNameTextCheck();
        }
        private void RegNameTextCheck()
        {
            if (txbRegName.Text == "")
            {
                txbRegName.Background = null;
            }
            else
            {
                txbRegName.Background = txbUserNameBackgrnd.Background;
            }
        }

        private void txbRegEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            RegEmailTextCheck();
        }

        private void txbRegEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegEmailTextCheck();
        }

        private void RegEmailTextCheck()
        {
            if (txbRegEmail.Text == "")
            {
                txbRegEmail.Background = null;
            }
            else
            {
                txbRegEmail.Background = txbUserNameBackgrnd.Background;
            }
        }
        private void txbRegPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            RegEmailPhoneCheck();
        }

        private void txbRegPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegEmailPhoneCheck();
        }

        private void RegEmailPhoneCheck()
        {
            if (txbRegPhone.Text == "")
            {
                txbRegPhone.Background = null;
            }
            else
            {
                txbRegPhone.Background = txbUserNameBackgrnd.Background;
            }
        }

        #endregion
        #region Registration trigger events
        private void txbRegUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void txbRegPassword1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void txbRegPassword2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void txbRegName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void txbRegEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void txbRegPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            Registration();
        }
        #endregion
        #region Registration
        private void Registration() 
        {
            if (txbRegUserName.Text != "" && txbRegPassword1.Password != "" && txbRegPassword1.Password == txbRegPassword2.Password && txbRegName.Text != "" && txbRegEmail.Text != "" && txbRegPhone.Text != "")
            {




            }
        }
        #endregion


    }
}