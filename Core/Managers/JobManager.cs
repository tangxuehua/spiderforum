using System.Collections;

namespace System.Web.Core
{
    public class JobManager
    {
        #region Private Members

        private static readonly JobManager _jobManager = null;
        private Hashtable jobList = new Hashtable();

        #endregion

        #region Constructors

        static JobManager()
        {
            _jobManager = new JobManager();
        }

        #endregion

        #region Public Properties

        public static JobManager Instance
        {
            get
            {
                return _jobManager;
            }
        }

        public Hashtable JobList
        {
            get { return jobList; }
        }

        #endregion

        #region Public Methods

        public void StartAllJobs()
        {
            foreach (Job job in Configuration.Instance.Jobs.Values)
            {
                job.Start();
            }
        }

        public void StartJob(string jobName)
        {
            Job job = GetJob(jobName);
            if (job != null)
            {
                job.Start();
            }
        }

        public void StopAllJobs()
        {
            if (jobList != null)
            {
                foreach (Job job in jobList.Values)
                {
                    job.Stop();
                }
            }
        }

        public void StopJob(string jobName)
        {
            Job job = GetJob(jobName);
            if (job != null)
            {
                job.Stop();
            }
        }

        public Job GetJob(string jobName)
        {
            return jobList[jobName] as Job;
        }

        public bool IsJobEnabled(string jobName)
        {
            Job job = GetJob(jobName);
            if (job == null)
            {
                return false;
            }
            return job.Enabled;
        }

        #endregion
    }
}