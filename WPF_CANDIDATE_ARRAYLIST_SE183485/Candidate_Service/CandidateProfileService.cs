using Candidate_BusinessObjects;
using Candidate_DAO;
using System;
using System.Collections;

namespace Candidate_Services
{
    public class CandidateProfileService : ICandidateProfileService
    {
        private readonly CandidateProfileDAO _candidateProfileDAO;

        public CandidateProfileService()
        {
            _candidateProfileDAO = new CandidateProfileDAO();
        }

        public ArrayList GetAllCandidateProfiles()
        {
            return _candidateProfileDAO.LoadCandidateProfiles();
        }

        public bool AddCandidate(CandidateProfile candidate)
        {
            try
            {
                if (string.IsNullOrEmpty(candidate.CandidateId) || string.IsNullOrEmpty(candidate.Fullname) ||
                    string.IsNullOrEmpty(candidate.PostingId))
                    return false;

                var candidates = _candidateProfileDAO.LoadCandidateProfiles();
                foreach (CandidateProfile existing in candidates)
                {
                    if (existing.CandidateId.Equals(candidate.CandidateId))
                        return false;
                }

                return _candidateProfileDAO.AddCandidate(candidate);
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCandidate(CandidateProfile candidate)
        {
            try
            {
                if (string.IsNullOrEmpty(candidate.CandidateId) || 
                    string.IsNullOrEmpty(candidate.Fullname) ||
                    string.IsNullOrEmpty(candidate.PostingId))
                    return false;

                return _candidateProfileDAO.UpdateCandidate(candidate);
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCandidate(string candidateId)
        {
            try
            {
                if (string.IsNullOrEmpty(candidateId))
                    return false;

                return _candidateProfileDAO.DeleteCandidate(candidateId);
            }
            catch
            {
                return false;
            }
        }

        public CandidateProfile GetCandidateProfileById(string id) => _candidateProfileDAO.GetCandidateProfileById(id);
        public ArrayList GetCandidateProfileByNameOrDateTime(string? Name, DateTime? Birthday) => _candidateProfileDAO.GetCandidateProfileByNameOrDateTime(Name, Birthday);
    }
}
