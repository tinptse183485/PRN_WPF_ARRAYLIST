using Candidate_BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Services
{
    public interface IHRAccountService
    {
        public ArrayList GetAllHRAccounts();
        public bool AddHRAccount(Hraccount account);
        public bool UpdateHRAccount(Hraccount account);
        public bool DeleteHRAccount(string email);
        public Hraccount GetHraccountByEmail(string email);
        public ArrayList GetHraccountsByNameOrRole(string? Name, int? Role);
    }
}
