using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace System.Web.Core
{
    public class PerformanceTester
    {
        #region Private Members

        private static PerformanceTester m_instance = new PerformanceTester();
        private ActionList m_clsListAction = new ActionList();
        private string m_fileToSave = null;
        private bool m_append = true;
        private Nullable<DateTime> m_applicationStartDate = null;
        private bool m_bEnable = true;

        #endregion

        #region Constructors

        private PerformanceTester()
        {
        }

        #endregion

        #region Public Properties

        public static PerformanceTester Instance
        {
            get { return m_instance; }
        }
        public string FileToSave
        {
            get { return m_fileToSave; }
            set { m_fileToSave = value; }
        }
        public bool Append
        {
            get { return m_append; }
            set { m_append = value; }
        }
        public bool Enable
        {
            get { return m_bEnable; }
            set { m_bEnable = value; }
        }

        #endregion

        #region Public Methods

        public void Start(DateTime applicationStartDate)
        {
            if (string.IsNullOrEmpty(FileToSave))
            {
                throw new Exception("You must set the 'FileToSave' property before you start the PerformanceTester.");
            }
            if (!File.Exists(FileToSave))
            {
                FileStream fileStream = File.Create(FileToSave);
                fileStream.Close();
                fileStream.Dispose();
            }
            if (m_applicationStartDate.HasValue)
            {
                throw new Exception("You have started the PerformanceTest.");
            }
            this.m_applicationStartDate = applicationStartDate;
            StreamWriter reportWriter = new StreamWriter(FileToSave, Append);

            try
            {
                reportWriter.WriteLine("");
                reportWriter.WriteLine("");
                reportWriter.WriteLine("");
                reportWriter.WriteLine(string.Format("应用程序开始:{0}", this.m_applicationStartDate.Value));
            }
            finally
            {
                reportWriter.Close();
                reportWriter.Dispose();
            }
        }
        public void End()
        {
            this.m_applicationStartDate = null;
            this.m_clsListAction.Clear();
        }
        public Action GetAction(string actionName)
        {
            if (!m_applicationStartDate.HasValue)
            {
                throw new Exception("You must start the PerformanceTest first.");
            }
            if (string.IsNullOrEmpty(actionName))
            {
                throw new Exception("The action name can not be null.");
            }

            Action action = null;

            foreach (Action a in m_clsListAction)
            {
                if (a.Name == actionName)
                {
                    action = a;
                    break;
                }
            }

            if (action == null)
            {
                action = new Action(actionName);
                action.FileToSave = this.FileToSave;
                this.m_clsListAction.Add(action);
            }

            return action;
        }
        public void Save(Action action)
        {
            if (!Enable) return;
            if (action == null) return;
            if (string.IsNullOrEmpty(this.FileToSave)) return;

            action.FileToSave = this.FileToSave;
            action.Append = this.Append;
            action.Save();
        }
        public void Save(Action action, string description)
        {
            if (!Enable) return;
            if (action == null) return;
            if (string.IsNullOrEmpty(this.FileToSave)) return;

            action.FileToSave = this.FileToSave;
            action.Append = this.Append;
            action.Save(description);
        }

        #endregion
    }
    public class Action
    {
        #region Private Members

        //the name of the action
        private string m_name = null;
        //the description information of the action
        private string m_description = null;
        //how many time this action been recorded
        private int m_recordCount = 0;
        //whether the action is running now
        private bool m_isRunning = false;
        //the start time of the action
        private DateTime m_startTime;
        //the end time of the action
        private DateTime m_endTime;
        //a list which contains all the small actions
        private SmallActionList clsListmallAction = new SmallActionList();
        //used to build a formated report string
        private StringBuilder clsReportBuilder = new StringBuilder();
        //a stopwatch to record every small action's startticks and endticks
        private Stopwatch m_stopWatch = new Stopwatch();
        //a stopwatch to record action's startticks and endticks
        private Stopwatch m_myselfStopWatch = new Stopwatch();
        //a flag explains whether we should generate the details of the small action information
        private bool m_generateDetails = true;
        //a file used to report the action's information
        private string m_fileToSave = null;
        //if we should append the report information in the file
        private bool m_append = true;
        //specify how to show the specific time information
        private OutputTimeStyle m_eOutputTimeStyle = OutputTimeStyle.DateTime;

        #endregion

        #region Constructors

        public Action(string name)
        {
            if (name == null)
            {
                throw new Exception("the Parameter 'name' can not be null.");
            }
            this.m_name = name;
        }

        #endregion

        #region Public Methods

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_name))
                {
                    return string.Empty;
                }
                return this.m_name;
            }
        }
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_description))
                {
                    return string.Empty;
                }
                return this.m_description;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }
                this.m_description = value;
            }
        }
        public string FileToSave
        {
            get { return m_fileToSave; }
            set { m_fileToSave = value; }
        }
        public bool Append
        {
            get { return m_append; }
            set { m_append = value; }
        }
        public bool GenerateDetails
        {
            get { return this.m_generateDetails; }
            set { this.m_generateDetails = value; }
        }
        public OutputTimeStyle OutputTimeStyle
        {
            get { return m_eOutputTimeStyle; }
            set { m_eOutputTimeStyle = value; }
        }

        public SmallAction CreateSmallAction(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new Exception("The description is null.");
            }
            if (this.m_isRunning == false) return null;

            SmallAction newSmallAction = new SmallAction(this);
            newSmallAction.StartTicks = GetCurrentTicks();
            newSmallAction.StartTime = DateTime.Now;
            newSmallAction.Description = description;

            return newSmallAction;
        }
        public void SaveSmallAction(SmallAction smallAction)
        {
            if (smallAction == null) return;

            smallAction.EndTime = DateTime.Now;
            smallAction.EndTicks = GetCurrentTicks();
            this.clsListmallAction.Add(smallAction);
        }
        public void Start()
        {
            if (this.m_isRunning == false) this.m_recordCount += 1;

            this.clsReportBuilder = new StringBuilder();
            this.clsListmallAction = new SmallActionList();
            this.m_stopWatch.Reset();
            this.m_stopWatch.Start();
            this.m_myselfStopWatch.Reset();
            this.m_myselfStopWatch.Start();
            this.m_startTime = DateTime.Now;

            this.m_isRunning = true;
        }
        public void Save()
        {
            Save(Description);
        }
        public void Save(string description)
        {
            if (string.IsNullOrEmpty(this.FileToSave)) return;

            if (this.m_isRunning == false) return;


            this.m_stopWatch.Stop();
            this.m_myselfStopWatch.Stop();
            this.m_endTime = DateTime.Now;
            this.Description = description;

            if (!File.Exists(this.FileToSave))
            {
                FileStream fileStream = File.Create(this.FileToSave);
                fileStream.Close();
                fileStream.Dispose();
            }

            StreamWriter reportWriter = new StreamWriter(this.FileToSave, this.Append);

            this.clsReportBuilder.AppendLine("------------------------------------------------------------------------------------------------------------------------------------");
            this.clsReportBuilder.AppendLine(string.Format("过程名称:{0}  过程描述:{1}(第{2}次){3}", this.Name, this.Description, this.m_recordCount.ToString(), Environment.NewLine));
            this.clsReportBuilder.AppendLine(string.Format("过程总时间:{0}ms;  子过程总时间:{1}ms;  开始时间:{2};  结束时间:{3};{4}", (this.m_myselfStopWatch.Elapsed.Ticks / 10000).ToString(), (GetTotalTicks() / 10000).ToString(), this.m_startTime.ToString(), this.m_endTime.ToString(), Environment.NewLine));
            if (this.m_generateDetails)
            {
                this.clsReportBuilder.AppendLine("子过程明细:" + Environment.NewLine);
                this.clsReportBuilder.AppendLine(GenerateTreeReport());
                this.clsReportBuilder.AppendLine("结论:");
            }
            this.clsReportBuilder.AppendLine("------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine);

            try
            {
                reportWriter.Write(this.clsReportBuilder.ToString());
                this.m_isRunning = false;
            }
            finally
            {
                reportWriter.Close();
                reportWriter.Dispose();
            }
        }

        #endregion

        #region Private Methods

        private long GetCurrentTicks()
        {
            this.m_stopWatch.Stop();
            long currentTicks = this.m_stopWatch.Elapsed.Ticks;
            this.m_stopWatch.Start();
            return currentTicks;
        }
        private string GenerateTreeReport()
        {
            string totalString = string.Empty;
            string leftSpace = "  ";
            string unitIndentString = "    ";
            List<string> actionTimeStrings = new List<string>();
            SmallAction rootVirtualNode = new SmallAction(null);
            SmallActionList topLevelSmallActions = null;

            rootVirtualNode.TreeNodeDeepLevel = 0;
            topLevelSmallActions = GetTopLevelSmallActions();

            foreach (SmallAction smallAction in topLevelSmallActions)
            {
                smallAction.TreeNodeDeepLevel = 1;
                smallAction.ParentSmallAction = rootVirtualNode;
                rootVirtualNode.ChildSmallActions.Add(smallAction);
            }

            foreach (SmallAction smallAction in topLevelSmallActions)
            {
                BuildChildSmallActionTree(smallAction);
            }

            foreach (SmallAction smallAction in rootVirtualNode.ChildSmallActions)
            {
                totalString += string.Join(Environment.NewLine, GetSmallActionTimeStrings(smallAction, leftSpace, unitIndentString, actionTimeStrings)) + Environment.NewLine;
                actionTimeStrings.Clear();
            }


            return totalString;
        }
        private void BuildChildSmallActionTree(SmallAction parentAction)
        {
            SmallActionList childSmallActions = GetChildSmallActions(parentAction);
            foreach (SmallAction smallAction in childSmallActions)
            {
                BuildChildSmallActionTree(smallAction);
                parentAction.ChildSmallActions.Add(smallAction);
            }
        }
        private long GetTotalTicks()
        {
            if (this.clsListmallAction.Count == 0) return 0;
            long total = 0;
            foreach (SmallAction action in GetTopLevelSmallActions())
            {
                total = total + action.TotalTicks;
            }
            return total;
        }
        private bool IsTopLevelSmallAction(SmallAction smallAction)
        {
            if (smallAction == null)
            {
                return false;
            }
            foreach (SmallAction sa in this.clsListmallAction)
            {
                if (sa.SmallActionID == smallAction.SmallActionID)
                {
                    continue;
                }
                if (sa.StartTicks < smallAction.StartTicks && sa.EndTicks > smallAction.EndTicks)
                {
                    return false;
                }
            }
            return true;
        }
        private SmallActionList GetTopLevelSmallActions()
        {
            SmallActionList topLevelSmallActions = new SmallActionList();
            foreach (SmallAction smallAction in this.clsListmallAction)
            {
                if (IsTopLevelSmallAction(smallAction))
                {
                    topLevelSmallActions.Add(smallAction);
                }
            }
            return topLevelSmallActions;
        }
        private SmallAction GetDirectParent(SmallAction smallAction)
        {
            if (smallAction == null)
            {
                return null;
            }
            foreach (SmallAction sa in this.clsListmallAction)
            {
                if (smallAction.SmallActionID == sa.SmallActionID)
                {
                    continue;
                }
                if (sa.StartTicks < smallAction.StartTicks && sa.EndTicks > smallAction.EndTicks)
                {
                    return sa;
                }
            }
            return null;
        }
        private SmallActionList GetChildSmallActions(SmallAction parentSmallAction)
        {
            if (parentSmallAction == null)
            {
                return new SmallActionList();
            }
            SmallActionList childSmallActions = new SmallActionList();
            foreach (SmallAction smallAction in this.clsListmallAction)
            {
                if (smallAction.SmallActionID == parentSmallAction.SmallActionID)
                {
                    continue;
                }
                if (smallAction.StartTicks > parentSmallAction.StartTicks && smallAction.EndTicks < parentSmallAction.EndTicks)
                {
                    SmallAction directParent = GetDirectParent(smallAction);
                    if (directParent != null && directParent.SmallActionID == parentSmallAction.SmallActionID)
                    {
                        smallAction.TreeNodeDeepLevel = parentSmallAction.TreeNodeDeepLevel + 1;
                        smallAction.ParentSmallAction = parentSmallAction;
                        childSmallActions.Add(smallAction);
                    }
                }
            }
            return childSmallActions;
        }
        private string[] GetSmallActionTimeStrings(SmallAction smallAction, string leftSpace, string unitIndentString, List<string> smallActionTimeStrings)
        {
            string smallActionTimeStringFormat = "{0}{1}({2})  {3}  {4}  {5}";
            string smallActionTimeLeftSpaceString = leftSpace;
            for (int i = 0; i <= smallAction.TreeNodeDeepLevel - 1; i++)
            {
                smallActionTimeLeftSpaceString += unitIndentString;
            }
            if (smallAction.Action.OutputTimeStyle == OutputTimeStyle.DateTime)
            {
                smallActionTimeStrings.Add(string.Format(smallActionTimeStringFormat, new object[] { smallActionTimeLeftSpaceString, (smallAction.TotalTicks / 10000).ToString() + "ms", GetTimePercent(smallAction), smallAction.Description, smallAction.StartTime.ToString() + ":" + smallAction.StartTime.Millisecond.ToString(), smallAction.EndTime.ToString() + ":" + smallAction.EndTime.Millisecond.ToString() }));
            }
            else if (smallAction.Action.OutputTimeStyle == OutputTimeStyle.Ticks)
            {
                smallActionTimeStrings.Add(string.Format(smallActionTimeStringFormat, new object[] { smallActionTimeLeftSpaceString, (smallAction.TotalTicks / 10000).ToString() + "ms", GetTimePercent(smallAction), smallAction.Description, smallAction.StartTicks.ToString(), smallAction.EndTicks.ToString() }));
            }
            else
            {
                smallActionTimeStrings.Add(string.Format(smallActionTimeStringFormat, new object[] { smallActionTimeLeftSpaceString, (smallAction.TotalTicks / 10000).ToString() + "ms", GetTimePercent(smallAction), smallAction.Description, string.Empty, string.Empty }));
            }
            foreach (SmallAction childSmallAction in smallAction.ChildSmallActions)
            {
                GetSmallActionTimeStrings(childSmallAction, leftSpace, unitIndentString, smallActionTimeStrings);
            }
            return smallActionTimeStrings.ToArray();
        }
        private string GetTimePercent(SmallAction smallAction)
        {
            if (smallAction.TreeNodeDeepLevel == 1)
            {
                if (GetTotalTicks() == 0)
                {
                    return "0.00%";
                }
                else
                {
                    return (smallAction.TotalTicks / GetTotalTicks()).ToString("##.##%");
                }
            }
            else if (smallAction.TreeNodeDeepLevel >= 2)
            {
                if (smallAction.ParentSmallAction.TotalTicks == 0)
                {
                    return "0.00%";
                }
                else
                {
                    return (smallAction.TotalTicks / smallAction.ParentSmallAction.TotalTicks).ToString("##.##%");
                }
            }
            return "0.00%";
        }

        #endregion
    }
    public class ActionList : List<Action>
    {
    }
    public class SmallAction
    {
        #region Private Members

        private Action m_action;
        //the unique ID of the action,attention: it is not necessary
        private string m_smallActionID = null;
        //the start time of the action
        private DateTime m_startTime;
        //the end time of the action
        private DateTime m_endTime;
        //the related description information of the action
        private string m_description = null;
        //the parent small action of the current small action
        private SmallAction m_parentSmallAction = null;
        //contains all the child small actions
        private SmallActionList m_childSmallActions = new SmallActionList();
        //represents the indent level
        private int m_treeNodeDeepLevel = 0;
        //the relative startticks from the whole action
        private long m_startTicks = 0;
        //the relative endticks from the whole action
        private long m_endTicks = 0;

        #endregion

        #region Constructors

        public SmallAction(Action action)
        {
            this.m_action = action;
            m_smallActionID = Guid.NewGuid().ToString();
        }

        #endregion

        #region Public Properties

        public Action Action
        {
            get { return this.m_action; }
            set { this.m_action = value; }
        }
        /// <summary>
        /// return the unique ID of the action,important: it is not necessary for a small action,that means you can leave it with null value,
        /// but if you have assigned a value to it,the value must be different from other small action.
        /// </summary>
        public string SmallActionID
        {
            get { return m_smallActionID; }
            set { m_smallActionID = value; }
        }
        /// <summary>
        /// get or set the start time of the action
        /// </summary>
        public DateTime StartTime
        {
            get { return m_startTime; }
            set { m_startTime = value; }
        }
        /// <summary>
        /// get or set the end time of the action
        /// </summary>
        public DateTime EndTime
        {
            get { return m_endTime; }
            set { m_endTime = value; }
        }
        /// <summary>
        /// get or set the description information of the action
        /// </summary>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }
        /// <summary>
        /// the parent small action of the current small action
        /// </summary>
        public SmallAction ParentSmallAction
        {
            get { return m_parentSmallAction; }
            set { m_parentSmallAction = value; }
        }
        /// <summary>
        /// contains all the child small actions
        /// </summary>
        public SmallActionList ChildSmallActions
        {
            get { return m_childSmallActions; }
        }
        /// <summary>
        /// represents the tree node deep level
        /// </summary>
        public int TreeNodeDeepLevel
        {
            get { return m_treeNodeDeepLevel; }
            set { m_treeNodeDeepLevel = value; }
        }
        //the relative startticks from the whole action
        public long StartTicks
        {
            get { return this.m_startTicks; }
            set { this.m_startTicks = value; }
        }
        //the relative endticks from the whole action
        public long EndTicks
        {
            get { return this.m_endTicks; }
            set { this.m_endTicks = value; }
        }
        /// <summary>
        /// get the time span of the action
        /// </summary>
        public long TotalTicks
        {
            get { return EndTicks - StartTicks; }
        }

        #endregion
    }
    public class SmallActionList : List<SmallAction>
    {
    }
    public enum OutputTimeStyle
    {
        DateTime,
        Ticks,
        None
    }
}