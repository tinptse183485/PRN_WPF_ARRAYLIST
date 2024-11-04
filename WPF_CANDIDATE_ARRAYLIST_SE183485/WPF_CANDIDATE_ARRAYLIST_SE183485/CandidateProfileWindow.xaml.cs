using Candidate_BusinessObjects;
using Candidate_Services;
using CandidateManagement_PHAMTRUNGTIN_SE183485;
using System;
using System.Collections.Generic;
using System.Linq;
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
	/// Interaction logic for CandidateProfileWindow.xaml
	/// </summary>
	public partial class CandidateProfileWindow : Window
	{
		private Hraccount Nowaccount = new Hraccount();
		private ICandidateProfileService profileService;
		private IJobPostingService jobPostingService;
		public CandidateProfileWindow()
		{
			InitializeComponent();
			profileService = new CandidateProfileService();
			jobPostingService = new JobPostingService();
		}

		public CandidateProfileWindow(Hraccount account)
		{
			InitializeComponent();
			profileService = new CandidateProfileService();
			jobPostingService = new JobPostingService();
			this.Nowaccount = account;
			switch (Nowaccount.MemberRole)
			{
				case 1:
					break;
				case 2:
					bntDelete.IsEnabled = false;
					break;
				case 3:
					bntDelete.IsEnabled = false;
					bntUpdate.IsEnabled = false;
					break;
				default:
					this.Close();
					break;
			}
		}
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataInit();
        }
  

		

		private void bntClose_Click(object sender, RoutedEventArgs e)
		{
			MenuWindow window = new MenuWindow(Nowaccount);
			window.Show();
			this.Close();
		}

		private void bntDelete_Click(object sender, RoutedEventArgs e)
		{
			string candidateID = CandidateIDtxt.Text;
			if(candidateID == "")
			{
				MessageBox.Show("Choose 1 CandidateID to delete !");
				return;
			}
			MessageBoxResult result = MessageBox.Show("Make sure you want to delete this Candidate Profile",
													  "Confirm Delete",
													  MessageBoxButton.YesNo,
													  MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				if (candidateID.Length > 0 && profileService.DeleteCandidate(candidateID))
				{
					MessageBox.Show("Delete successfully");
					loadDataInit();
				}
				else
				{
					MessageBox.Show("Delete unsuccessfully !");
				}
			}
		}

		private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataGrid dataGrid = sender as DataGrid;
			DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
			if (row != null)
			{
				DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
				string id = ((TextBlock)RowColumn.Content).Text;
				CandidateProfile profile = profileService.GetCandidateProfileById(id);
				if (profile != null)
				{
					CandidateIDtxt.Text = profile.CandidateId;
					FullNametxt.Text = profile.Fullname;
					BirthDay.SelectedDate = profile.Birthday;
					ImageURLtxt.Text = profile.ProfileUrl;
					cmbPostID.SelectedValue = profile.PostingId;
					Descriptiontxt.Text = profile.ProfileShortDescription;
				}
			}
		}

		
		private void loadDataInit() {
			this.DataGrid.ItemsSource = profileService.GetAllCandidateProfiles();
			this.cmbPostID.ItemsSource = jobPostingService.GetAllJobPostings();
			this.cmbPostID.DisplayMemberPath = "JobPostingTitle";
			this.cmbPostID.SelectedValuePath = "PostingId";

			CandidateIDtxt.Text = "";
			FullNametxt.Text = "";
			BirthDay.Text ="";
			ImageURLtxt.Text = "";
			cmbPostID.Text = "";
			Descriptiontxt.Text = "";
			BirthDay.SelectedDate = null;

		}
		private void JobPostingID_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			
		}

		private void bntAdd_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(CandidateIDtxt.Text) ||string.IsNullOrWhiteSpace(FullNametxt.Text) ||
				string.IsNullOrWhiteSpace(BirthDay.Text) ||string.IsNullOrWhiteSpace(ImageURLtxt.Text) ||
				cmbPostID.SelectedValue == null ||string.IsNullOrWhiteSpace(Descriptiontxt.Text))
			{
				MessageBox.Show("All fields are required!");
				return;
			}

			if (FullNametxt.Text.Length <= 8 || !IsCapitalized(FullNametxt.Text))
			{
				MessageBox.Show("Full Name must be greater than 8 characters and each word must begin with a capital letter!");
				return;
			}

			if (Descriptiontxt.Text.Length < 12 || Descriptiontxt.Text.Length > 200)
			{
				MessageBox.Show("Description must be between 12 and 200 characters!");
				return;
			}

			try
			{
				CandidateProfile candidate = new CandidateProfile
				{
					CandidateId = CandidateIDtxt.Text,
					Fullname = FullNametxt.Text,
					Birthday = DateTime.Parse(BirthDay.Text),
					ProfileUrl = ImageURLtxt.Text,
					PostingId = cmbPostID.SelectedValue.ToString(),
					ProfileShortDescription = Descriptiontxt.Text
				};
				if (profileService.AddCandidate(candidate))
				{
					MessageBox.Show("Add successfully!");
					loadDataInit();
				}
				else
				{
					MessageBox.Show("CandidateId must not be duplicated! Change it !");
				}
			}
			catch (FormatException)
			{
				MessageBox.Show("Invalid date format for Birthday. Please enter a valid date.");
			}
		}

		private bool IsCapitalized(string fullName)
		{
			string[] words = fullName.Split(' ');
			foreach (string word in words)
			{
				if (string.IsNullOrEmpty(word) || !char.IsUpper(word[0]))
				{
					return false;
				}
			}
			return true;
		}


		private void bntUpdate_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(CandidateIDtxt.Text) || string.IsNullOrWhiteSpace(FullNametxt.Text) ||
				string.IsNullOrWhiteSpace(BirthDay.Text) || string.IsNullOrWhiteSpace(ImageURLtxt.Text) ||
				cmbPostID.SelectedValue == null || string.IsNullOrWhiteSpace(Descriptiontxt.Text))
			{
				MessageBox.Show("All fields are required!");
				return;
			}

			if (FullNametxt.Text.Length <= 8 || !IsCapitalized(FullNametxt.Text))
			{
				MessageBox.Show("Full Name must be greater than 8 characters and each word must begin with a capital letter!");
				return;
			}

			if (Descriptiontxt.Text.Length < 12 || Descriptiontxt.Text.Length > 200)
			{
				MessageBox.Show("Description must be between 12 and 200 characters!");
				return;
			}
			try
			{
				CandidateProfile candidate = new CandidateProfile();
                candidate.Fullname = FullNametxt.Text;
                candidate.CandidateId = CandidateIDtxt.Text;
				candidate.Birthday = DateTime.Parse(BirthDay.Text);
				candidate.ProfileUrl = ImageURLtxt.Text;
				candidate.PostingId = cmbPostID.SelectedValue.ToString();
				candidate.ProfileShortDescription = Descriptiontxt.Text;
				if (profileService.UpdateCandidate(candidate))
				{
					MessageBox.Show("Update successfully !");
					loadDataInit();
				}
				else
				{
					MessageBox.Show("Update unsuccessfully !");
				}
			}
			catch (FormatException)
			{
				MessageBox.Show("Invalid date format for Birthday. Please enter a valid date.");
			}
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			string name = FullNametxt.Text;
			DateTime ? date = BirthDay.SelectedDate;
			var listCandidateProfile = profileService.GetCandidateProfileByNameOrDateTime(name, date);
			this.DataGrid.ItemsSource = listCandidateProfile;
		}
	}
}
