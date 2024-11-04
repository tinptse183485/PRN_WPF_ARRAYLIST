using Candidate_BusinessObjects;
using Candidate_Services;
using CandidateManagement_PHAMTRUNGTIN_SE183485;
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

namespace CandidateManagement_PHAMTRUNGTIN_SE183485
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		private IHRAccountService hRAccountService;
		public LoginWindow()
		{
			InitializeComponent();
			hRAccountService = new HRAccountService();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void BtnLogin_Click(object sender, RoutedEventArgs e)
		{
			Hraccount hraccount = hRAccountService.GetHraccountByEmail(txtEmail.Text);
			if (hraccount != null && hraccount.Password.Equals(txtPassword.Text))
			{
				int? roleID = hraccount.MemberRole;
				this.Hide();
				MenuWindow menuWindow = new MenuWindow(hraccount);
				menuWindow.Show();
			}
			else
			{
				MessageBox.Show("Login Fail");
			}
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
			RegisterWindow registerWindow = new RegisterWindow();
			registerWindow.Show();
			this.Hide();
        }
    }
}