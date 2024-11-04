using Candidate_BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Services
{
    public interface ICandidateProfileService
    {
        public ArrayList GetAllCandidateProfiles();
        public bool AddCandidate(CandidateProfile candidate);
        public bool UpdateCandidate(CandidateProfile candidate);
        public bool DeleteCandidate(string candidateId);
        public CandidateProfile GetCandidateProfileById(string id);
        public ArrayList GetCandidateProfileByNameOrDateTime(string? Name, DateTime? Birthday);
    }
}
