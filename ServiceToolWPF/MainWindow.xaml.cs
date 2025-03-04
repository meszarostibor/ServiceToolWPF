using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiceToolWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            //            if (e.Key == Key.Enter) { UserLogin(); }
        }
        private void txbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            //            if (e.Key == Key.Enter) { UserLogin(); }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //            UserLogin();
        }
        #endregion
        #region Logout trigger event
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}