using Candidate_BusinessObjects;
using CandidateManagement_PHAMTRUNGTIN_SE183485;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandidateManagement_PHAMTRUNGTIN_SE183485
{
	/// <summary>
	/// Interaction logic for MenuWindow.xaml
	/// </summary>
	public partial class MenuWindow : Window
	{
		private Hraccount NowAccount = new Hraccount();
		public MenuWindow()
		{
			InitializeComponent();
		}
		public MenuWindow(Hraccount account)
		{
			InitializeComponent();
			this.NowAccount = account;
			WelcomeLabel.Content = $"Welcome, {account.FullName}";
		}

		private void Candidate_Click(object sender, RoutedEventArgs e)
		{
			CandidateProfileWindow candidateProfileWindow = new CandidateProfileWindow(NowAccount);
			candidateProfileWindow.Show();
			this.Close();
		}

		private void Job_Click(object sender, RoutedEventArgs e)
		{
			JobPostingWindow job = new JobPostingWindow(NowAccount);
			job.Show();
			this.Close();
		}

		private void Logoutbtn_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
			LoginWindow log = new LoginWindow();
			log.Show();
		}

        private void btn_hrAccount_Click(object sender, RoutedEventArgs e)
        {
			HRAccountWindow hRAccountWindow = new HRAccountWindow(NowAccount);
			hRAccountWindow.Show();
			this.Close();
        }
    }
}
