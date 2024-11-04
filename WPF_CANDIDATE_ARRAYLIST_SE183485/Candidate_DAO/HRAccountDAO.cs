using Candidate_BusinessObjects;
using System;
using System.Collections;
using System.IO;
using System.Xml.Linq;

namespace Candidate_DAO
{
    public class HRAccountDAO
    {
        private readonly string _dataPath;

        public HRAccountDAO()
        {
            _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        }

        public ArrayList LoadHRAccounts()
        {
            ArrayList hrAccounts = new ArrayList();
            string filePath = Path.Combine(_dataPath, "HRAccount.txt");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Cannot find file: {filePath}");
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                // Skip header line
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var fields = line.Split('\t');

                    var account = new Hraccount
                    {
                        Email = fields[0],
                        Password = fields[1],
                        FullName = fields[2],
                        MemberRole = int.Parse(fields[3])
                    };

                    hrAccounts.Add(account);
                }
            }
            return hrAccounts;
        }

        public bool AddHRAccount(Hraccount account)
        {
            try
            {
                var accounts = LoadHRAccounts();
                accounts.Add(account);
                SaveHRAccounts(accounts);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateHRAccount(Hraccount account)
        {
            try
            {
                var accounts = LoadHRAccounts();
                bool found = false;

                for (int i = 0; i < accounts.Count; i++)
                {
                    var existing = accounts[i] as Hraccount;
                    if (existing.Email == account.Email)
                    {
                        accounts[i] = account;
                        found = true;
                        break;
                    }
                }

                if (!found) return false;

                SaveHRAccounts(accounts);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteHRAccount(string email)
        {
            try
            {
                var accounts = LoadHRAccounts();
                bool found = false;

                for (int i = accounts.Count - 1; i >= 0; i--)
                {
                    var account = accounts[i] as Hraccount;
                    if (account.Email == email)
                    {
                        accounts.RemoveAt(i);
                        found = true;
                        break;
                    }
                }

                if (!found) return false;

                SaveHRAccounts(accounts);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public ArrayList GetHraccountsByNameOrRole(string? Name, int? Role)
        {
            try
            {
                var allHrAccount = LoadHRAccounts();
                ArrayList result = new ArrayList();

                // Nếu cả hai tham số đều null/empty
                if (string.IsNullOrWhiteSpace(Name) && Role==null)
                {
                    return allHrAccount;
                }

                // Nếu chỉ có Name
                if (!string.IsNullOrWhiteSpace(Name) && Role == null)
                {
                    foreach (Hraccount hraccount in allHrAccount)
                    {
                        if (hraccount.FullName.Contains(Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.Add(hraccount);
                        }
                    }
                    return result;
                }

                // Nếu chỉ có Birthday
                if (string.IsNullOrWhiteSpace(Name) && Role != null)
                {
                    foreach (Hraccount hraccount in allHrAccount)
                    {
                        if (hraccount.MemberRole == Role)
                        {
                            result.Add(hraccount);
                        }
                    }
                    return result;
                }

                // Nếu có cả Name và Birthday
                foreach (Hraccount hraccount in allHrAccount)
                {
                    if (hraccount.FullName.Contains(Name, StringComparison.OrdinalIgnoreCase) &&
                        hraccount.MemberRole == Role)
                    {
                        result.Add(hraccount);
                    }
                }
                return result;
            }
            catch
            {
                return new ArrayList();
            }
        }
        private bool SaveHRAccounts(ArrayList accounts)
        {
            try
            {
                string filePath = Path.Combine(_dataPath, "HRAccount.txt");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Email\tPassword\tFullName\tMemberRole");
                    foreach (Hraccount account in accounts)
                    {
                        writer.WriteLine($"{account.Email}\t{account.Password}\t{account.FullName}\t{account.MemberRole}");
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Hraccount GetHraccountByEmail(string email)
        {
            try
            {
                var accounts = LoadHRAccounts();
                foreach (Hraccount account in accounts)
                {
                    if (account.Email.Equals(email))
                    {
                        return account;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
