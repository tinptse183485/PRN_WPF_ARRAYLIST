using Candidate_BusinessObjects;
using Candidate_Services;
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
using static System.Net.Mime.MediaTypeNames;

namespace CandidateManagement_PHAMTRUNGTIN_SE183485
{
	/// <summary>
	/// Interaction logic for JobPostingWindow.xaml
	/// </summary>
	public partial class JobPostingWindow : Window
	{
		private readonly IJobPostingService _jobPostingService;
		private Hraccount NowAccount = new Hraccount();
		public JobPostingWindow()
		{
			InitializeComponent();
		}
		public JobPostingWindow(Hraccount account)
		{
			InitializeComponent();
			_jobPostingService = new JobPostingService();
			this.NowAccount = account;
			switch (NowAccount.MemberRole)
			{
				case 1:
					break;
				case 2:
					btnDelete.IsEnabled = false;
					break;
				case 3:
					btnDelete.IsEnabled = false;
					btnUpdate.IsEnabled = false;
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
		private void loadDataInit()
		{
			this.DataGridJob.ItemsSource = _jobPostingService.GetAllJobPostings();

			PostingID.Text = "";
			JobPostingTitle.Text = "";
			JobPostingTitle.Text = "";
			Description.Text = "";
			PostingDate.SelectedDate = null;
		}
		private void Description_TextChanged(object sender, TextChangedEventArgs e)
		{

        }

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(PostingID.Text) ||string.IsNullOrWhiteSpace(JobPostingTitle.Text) ||PostingDate.SelectedDate == null ||string.IsNullOrWhiteSpace(Description.Text))
			{
				MessageBox.Show("All fields are required!");
				return;
			}

			if (Description.Text.Length < 12 || Description.Text.Length > 200)
			{
				MessageBox.Show("Description must be between 12 and 200 characters!");
				return;
			}
			JobPosting jobPosting = new JobPosting();
			jobPosting.PostingId = PostingID.Text ;
			jobPosting.JobPostingTitle = JobPostingTitle.Text ;
			jobPosting.PostedDate = PostingDate.SelectedDate ;
			jobPosting.Description = Description.Text;
			if (_jobPostingService.AddJobPosting(jobPosting))
			{
				MessageBox.Show("Add successful !");
				loadDataInit();
			}
			else
			{
				MessageBox.Show("Add unsuccessful !");
			}
		}
        private void DataGridJob_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
            if (row != null)
            {
                DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string id = ((TextBlock)RowColumn.Content).Text;
                JobPosting jobPosting = _jobPostingService.GetJobPostingById(id);
                if (jobPosting != null)
                {
                    PostingID.Text = jobPosting.PostingId;
                    JobPostingTitle.Text = jobPosting.JobPostingTitle;
                    PostingDate.SelectedDate = jobPosting.PostedDate;
                    Description.Text = jobPosting.Description;
                }
            }
        }
        private void Button_Close(object sender, RoutedEventArgs e)
		{
			MenuWindow window = new MenuWindow(NowAccount);
			window.Show();
			this.Close();
		}

		

		private void btnUpdate_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(PostingID.Text) || string.IsNullOrWhiteSpace(JobPostingTitle.Text) || PostingDate.SelectedDate == null || string.IsNullOrWhiteSpace(Description.Text))
			{
				MessageBox.Show("All fields are required!");
				return;
			}

			if (Description.Text.Length < 12 || Description.Text.Length > 200)
			{
				MessageBox.Show("Description must be between 12 and 200 characters!");
				return;
			}
			JobPosting jobPosting = new JobPosting();
			jobPosting.PostingId = PostingID.Text;
			jobPosting.JobPostingTitle = JobPostingTitle.Text;
			jobPosting.PostedDate = PostingDate.SelectedDate;
			jobPosting.Description = Description.Text;
			if (_jobPostingService.UpdateJobPosting(jobPosting))
			{
				MessageBox.Show("Update successful !");
				loadDataInit();
			}
			else
			{
				MessageBox.Show("Update unsuccessful !");
			}
		}
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string Title = JobPostingTitle.Text;
            DateTime? day = PostingDate.SelectedDate;
            var listJobPosting = _jobPostingService.GetJobPostingByTitleOrPostDate(Title, day);
            this.DataGridJob.ItemsSource = listJobPosting;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			string jobpostingID = PostingID.Text;
			if (string.IsNullOrWhiteSpace(jobpostingID))
			{
				MessageBox.Show("Choose 1 job posting to delete!");
				return;
			}

			MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this job posting?",
													  "Confirm Delete",
													  MessageBoxButton.YesNo,
													  MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				if (_jobPostingService.DeleteJobPosting(jobpostingID))
				{
					MessageBox.Show("Delete successful!");
					loadDataInit();
				}
				else
				{
					MessageBox.Show("Something went wrong! Unable to delete.");
				}
			}
		}

		
	}
}
