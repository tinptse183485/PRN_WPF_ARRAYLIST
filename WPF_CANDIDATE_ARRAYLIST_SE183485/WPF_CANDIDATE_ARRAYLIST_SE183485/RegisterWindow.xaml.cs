using Candidate_BusinessObjects;
using Candidate_Services;
using CandidateManagement_PHAMTRUNGTIN_SE183485;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly IHRAccountService hRAccountService;
        public RegisterWindow()
        {
            InitializeComponent();
            hRAccountService = new HRAccountService();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtFullname.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("All fields are required!");
                return;
            }
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(txtEmail.Text, emailPattern))
            {
                MessageBox.Show("Invalid email format!");
                return;
            }
            if (txtFullname.Text.Length <= 6 || !IsFullNameCapitalized(txtFullname.Text))
            {
                MessageBox.Show("Full Name must be greater than 6 characters and each word must begin with a capital letter!");
                return;
            }
            Hraccount newAccount = new Hraccount
            {
                Email = txtEmail.Text,
                FullName = txtFullname.Text,
                Password = txtPassword.Text,
                MemberRole =3
            };
            if (hRAccountService.AddHRAccount(newAccount))
            {
                MessageBox.Show("Create account successful!");
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Create account unsuccessful!");
            }
        }
        private bool IsFullNameCapitalized(string fullName)
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
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
