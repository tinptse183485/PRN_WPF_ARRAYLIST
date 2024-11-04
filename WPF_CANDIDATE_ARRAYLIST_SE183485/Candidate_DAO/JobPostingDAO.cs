using Candidate_BusinessObjects;
using System;
using System.Collections;
using System.IO;

namespace Candidate_DAO
{
    public class JobPostingDAO
    {
        private readonly string _dataPath;

        public JobPostingDAO()
        {
            _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        }

        public ArrayList LoadJobPostings()
        {
            ArrayList jobPostings = new ArrayList();
            string filePath = Path.Combine(_dataPath, "JobPosting.txt");

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

                    var posting = new JobPosting
                    {
                        PostingId = fields[0],
                        JobPostingTitle = fields[1],
                        Description = fields[2],
                        PostedDate = DateTime.Parse(fields[3])
                    };

                    jobPostings.Add(posting);
                }
            }
            return jobPostings;
        }

        public bool AddJobPosting(JobPosting posting)
        {
            try
            {
                var postings = LoadJobPostings();
                postings.Add(posting);
                return SaveJobPostings(postings);
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
                var postings = LoadJobPostings();
                bool found = false;

                for (int i = 0; i < postings.Count; i++)
                {
                    var existing = postings[i] as JobPosting;
                    if (existing.PostingId == posting.PostingId)
                    {
                        postings[i] = posting;
                        found = true;
                        break;
                    }
                }

                if (!found) return false;
                return SaveJobPostings(postings);
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
                var postings = LoadJobPostings();
                bool found = false;

                for (int i = postings.Count - 1; i >= 0; i--)
                {
                    var posting = postings[i] as JobPosting;
                    if (posting.PostingId == postingId)
                    {
                        postings.RemoveAt(i);
                        found = true;
                        break;
                    }
                }

                if (!found) return false;
                return SaveJobPostings(postings);
            }
            catch
            {
                return false;
            }
        }
        public JobPosting GetJobPostingById(string id)
        {
            try
            {
                var ListJobPosting = LoadJobPostings();
                foreach (JobPosting jobposting in ListJobPosting)
                {
                    if (jobposting.PostingId.Equals(id))
                    {
                        return jobposting;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public ArrayList GetJobPostingByTitleOrPostDate(string? Title, DateTime? Date)
        {
            try
            {
                var allJobPosting = LoadJobPostings();
                ArrayList result = new ArrayList();

                if (string.IsNullOrWhiteSpace(Title) && !Date.HasValue)
                {
                    return allJobPosting;
                }
                if (!string.IsNullOrWhiteSpace(Title) && !Date.HasValue)
                {
                    foreach (JobPosting job in allJobPosting)
                    {
                        if (job.JobPostingTitle.Contains(Title, StringComparison.OrdinalIgnoreCase))
                        {
                            result.Add(job);
                        }
                    }
                    return result;
                }
                if (string.IsNullOrWhiteSpace(Title) && Date.HasValue)
                {
                    foreach (JobPosting job in allJobPosting)
                    {
                        if (job.PostedDate == Date.Value.Date)
                        {
                            result.Add(job);
                        }
                    }
                    return result;
                }
                foreach (JobPosting job in allJobPosting)
                {
                    if (job.JobPostingTitle.Contains(Title, StringComparison.OrdinalIgnoreCase) &&
                        job.PostedDate == Date.Value.Date)
                    {
                        result.Add(job);
                    }
                }
                return result;
            }
            catch
            {
                return new ArrayList();
            }
        }
        private bool SaveJobPostings(ArrayList postings)
        {
            try
            {
                string filePath = Path.Combine(_dataPath, "JobPosting.txt");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("PostingID\tJobPostingTitle\tDescription\tPostedDate");
                    foreach (JobPosting posting in postings)
                    {
                        writer.WriteLine($"{posting.PostingId}\t{posting.JobPostingTitle}\t{posting.Description}\t{posting.PostedDate:yyyy-MM-dd HH:mm:ss.fff}");
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
