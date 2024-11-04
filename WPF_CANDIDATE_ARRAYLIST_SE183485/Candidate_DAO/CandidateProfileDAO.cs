using Candidate_BusinessObjects;
using System;
using System.Collections;
using System.IO;

namespace Candidate_DAO
{
    public class CandidateProfileDAO
    {
        private readonly string _dataPath;

        public CandidateProfileDAO()
        {
            _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        }

        public ArrayList LoadCandidateProfiles()
        {
            ArrayList candidateProfiles = new ArrayList();
            string filePath = Path.Combine(_dataPath, "CandidateProFile.txt");

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

                    var profile = new CandidateProfile
                    {
                        CandidateId = fields[0],
                        Fullname = fields[1],
                        Birthday = DateTime.Parse(fields[2]),
                        ProfileShortDescription = fields[3] == "NULL" ? null : fields[3],
                        ProfileUrl = fields[4],
                        PostingId = fields[5]
                    };

                    candidateProfiles.Add(profile);
                }
            }
            return candidateProfiles;
        }

        public bool AddCandidate(CandidateProfile candidate)
        {
            try
            {
                var candidates = LoadCandidateProfiles();
                candidates.Add(candidate);
                return SaveCandidates(candidates);
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
                var candidates = LoadCandidateProfiles();
                bool found = false;

                for (int i = 0; i < candidates.Count; i++)
                {
                    var existing = candidates[i] as CandidateProfile;
                    if (existing.CandidateId == candidate.CandidateId)
                    {
                        candidates[i] = candidate;
                        found = true;
                        break;
                    }
                }

                if (!found) return false;
                return SaveCandidates(candidates);
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
                var candidates = LoadCandidateProfiles();
                bool found = false;

                for (int i = candidates.Count - 1; i >= 0; i--)
                {
                    var candidate = candidates[i] as CandidateProfile;
                    if (candidate.CandidateId == candidateId)
                    {
                        candidates.RemoveAt(i);
                        found = true;
                        break;
                    }
                }

                if (!found) return false;
                return SaveCandidates(candidates);
            }
            catch
            {
                return false;
            }
        }
        public CandidateProfile GetCandidateProfileById(string id)
        {
            try
            {
                var profileList = LoadCandidateProfiles();
                foreach (CandidateProfile profile in profileList)
                {
                    if (profile.CandidateId.Equals(id))
                    {
                        return profile;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public ArrayList GetCandidateProfileByNameOrDateTime(string? Name, DateTime? Birthday)
        {
            try
            {
                var allProfiles = LoadCandidateProfiles();
                ArrayList result = new ArrayList();

                // Nếu cả hai tham số đều null/empty
                if (string.IsNullOrWhiteSpace(Name) && !Birthday.HasValue)
                {
                    return allProfiles;
                }

                // Nếu chỉ có Name
                if (!string.IsNullOrWhiteSpace(Name) && !Birthday.HasValue)
                {
                    foreach (CandidateProfile profile in allProfiles)
                    {
                        if (profile.Fullname.Contains(Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.Add(profile);
                        }
                    }
                    return result;
                }

                // Nếu chỉ có Birthday
                if (string.IsNullOrWhiteSpace(Name) && Birthday.HasValue)
                {
                    foreach (CandidateProfile profile in allProfiles)
                    {
                        if (profile.Birthday == Birthday.Value.Date)
                        {
                            result.Add(profile);
                        }
                    }
                    return result;
                }

                // Nếu có cả Name và Birthday
                foreach (CandidateProfile profile in allProfiles)
                {
                    if (profile.Fullname.Contains(Name, StringComparison.OrdinalIgnoreCase) &&
                        profile.Birthday == Birthday.Value.Date)
                    {
                        result.Add(profile);
                    }
                }
                return result;
            }
            catch
            {
                return new ArrayList();
            }
        }
        private bool SaveCandidates(ArrayList candidates)
        {
            try
            {
                string filePath = Path.Combine(_dataPath, "CandidateProFile.txt");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("CandidateID\tFullname\tBirthday\tProfileShortDescription\tProfileURL\tPostingID");
                    foreach (CandidateProfile candidate in candidates)
                    {
                        writer.WriteLine($"{candidate.CandidateId}\t{candidate.Fullname}\t{candidate.Birthday:yyyy-MM-dd HH:mm:ss.fff}\t{candidate.ProfileShortDescription ?? "NULL"}\t{candidate.ProfileUrl}\t{candidate.PostingId}");
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
