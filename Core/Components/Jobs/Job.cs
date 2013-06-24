using System;
using System.Threading;
using System.Xml;

namespace System.Web.Core
{
    public class Job
    {
        #region Private Members

        private IJob _ijob;
        private bool _enabled = true;
        private Type _jobType;
        private string _name;
        private int _minutes = 15;
        private Timer _timer = null;
        private XmlNode _node = null;
        private bool _isRunning;

        #endregion

        #region Constructors

        public Job(Type ijob, XmlNode node)
        {
            _node = node;
            _jobType = ijob;
            XmlAttribute att = null;

            att = node.Attributes["name"];
            if (att != null)
            {
                this._name = att.Value;
            }
            att = node.Attributes["enabled"];
            if (att != null && !string.IsNullOrEmpty(att.Value))
            {
                try
                {
                    this._enabled = bool.Parse(att.Value);
                }
                catch
                {
                    this._enabled = true;
                }
            }
            att = node.Attributes["minutes"];
            if (att != null && !string.IsNullOrEmpty(att.Value))
            {
                try
                {
                    this._minutes = Int32.Parse(att.Value);
                }
                catch
                {
                    this._minutes = 15;
                }
            }
        }

        #endregion

        #region Public Properities

        public string Name
        {
            get { return this._name; }
        }

        public bool Enabled
        {
            get { return this._enabled; }
        }

        public int Minutes
        {
            get { return _minutes; }
            set { _minutes = value; }
        }

        public int Interval
        {
            get
            {
                return Minutes * 60000;
            }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public Type JobType
        {
            get { return this._jobType; }
        }

        #endregion

        #region Public Events

        public event EventHandler PreJob;
        public event EventHandler PostJob;

        #endregion

        #region Public Methods

        public void Start()
        {
            if (Enabled && _timer == null)
            {
                _timer = new Timer(new TimerCallback(timer_Callback), null, Interval, Interval);
                Execute();
            }
        }

        public void Stop()
        {
            if (_timer != null)
            {
                lock (this)
                {
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }

        #endregion

        #region Private Methods

        private void Execute()
        {
            OnPreJob();
            _isRunning = true;

            if (_ijob == null)
            {
                _ijob = CreateJobInstance();
            }
            try
            {
                _ijob.Execute(this._node);
            }
            catch
            {
            }

            _isRunning = false;
            OnPostJob();
        }
        private void timer_Callback(object state)
        {
            if (!Enabled)
            {
                return;
            }
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            Execute();
            if (Enabled)
            {
                _timer.Change(Interval, Interval);
            }
            else
            {
                this.Stop();
            }
        }
        private void OnPreJob()
        {
            try
            {
                if (PreJob != null)
                {
                    PreJob(this, EventArgs.Empty);
                }
            }
            catch
            {
            }
        }
        private void OnPostJob()
        {
            try
            {
                if (PostJob != null)
                {
                    PostJob(this, EventArgs.Empty);
                }
            }
            catch
            {
            }
        }
        private IJob CreateJobInstance()
        {
            if (_jobType != null)
            {
                _ijob = Activator.CreateInstance(_jobType) as IJob;
            }
            _enabled = (_ijob != null);
            if (!_enabled)
            {
                this.Stop();
            }
            return _ijob;
        }

        #endregion
    }
}
