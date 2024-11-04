using Candidate_BusinessObjects;
using Candidate_DAO;
using System;
using System.Collections;

namespace Candidate_Services
{
    public class JobPostingService : IJobPostingService
    {
        private readonly JobPostingDAO _jobPostingDAO;

        public JobPostingService()
        {
            _jobPostingDAO = new JobPostingDAO();
        }

        public ArrayList GetAllJobPostings()
        {
            return _jobPostingDAO.LoadJobPostings();
        }

        public bool AddJobPosting(JobPosting posting)
        {
            try
            {
                if (string.IsNullOrEmpty(posting.PostingId) ||
                    string.IsNullOrEmpty(posting.JobPostingTitle) ||
                    string.IsNullOrEmpty(posting.Description))
                    return false;

                var postings = _jobPostingDAO.LoadJobPostings();
                foreach (JobPosting existing in postings)
                {
                    if (existing.PostingId.Equals(posting.PostingId))
                        return false;
                }

                return _jobPostingDAO.AddJobPosting(posting);
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateJobPosting(JobPosting posting)
        {
            try
            {
                if (string.IsNullOrEmpty(posting.PostingId) ||
                    string.IsNullOrEmpty(posting.JobPostingTitle) ||
                    string.IsNullOrEmpty(posting.Description))
                    return false;

                return _jobPostingDAO.UpdateJobPosting(posting);
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteJobPosting(string postingId)
        {
            try
            {
                if (string.IsNullOrEmpty(postingId))
                    return false;

                return _jobPostingDAO.DeleteJobPosting(postingId);
            }
            catch
            {
                return false;
            }
        }
        public JobPosting GetJobPostingById(string id) => _jobPostingDAO.GetJobPostingById(id);
        public ArrayList GetJobPostingByTitleOrPostDate(string? Title, DateTime? Date) => _jobPostingDAO.GetJobPostingByTitleOrPostDate(Title, Date);
    }
}
