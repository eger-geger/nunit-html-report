using System;
using System.Threading;
using Newtonsoft.Json;
using NUnitReporter.EventReport.Events;

namespace NUnitReporter.EventReport
{

    /// <summary>
    ///     Hierarchical report starting from root element. Thread-safe.
    /// </summary>
    /// <seealso cref="NUnitReporter.EventReport.IEventReport" />
    public class DefaultEventReport : IEventReport
    {
        [JsonProperty("root")]
        private readonly IActivity _rootActivity;
        
        private IActivity _currentActivity;

        private readonly Mutex _mutex = new Mutex();

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultEventReport"/> class.
        /// </summary>
        public DefaultEventReport()
        {
            _rootActivity = _currentActivity = new RootActivity();
        }

        internal IActivity RootActivity
        {
            get
            {
                return _rootActivity;
            }
        }


        /// <summary>
        ///     Record atomic activity which cannot host other activities
        /// </summary>
        /// <param name="description">Activity description</param>
        /// <param name="args">Activity details (input arguments)</param>
        public void RecordEvent(string description, params object[] args)
        {
            RecordEvent(new BasicEvent(_currentActivity, description, args));
        }


        /// <summary>
        ///     Record error atomic event
        /// </summary>
        /// <param name="exception">Error to record</param>
        public void RecordError(Exception exception)
        {
            RecordEvent(new ErrorEvent(_currentActivity, exception));
        }


        /// <summary>
        ///     Record atomic activity which cannot host other activities
        /// </summary>
        /// <param name="event">Activity to be recorded</param>
        public void RecordEvent(IActivity @event)
        {
            _mutex.WaitOne();
            _currentActivity.AddNested(@event);
            _mutex.ReleaseMutex();
        }


        /// <summary>
        ///     Record starting point of the complex activity.
        ///     All activities reported after current method call will be considered it's "children"
        ///     until activity endpoint will be recorder with <see cref="RecordActivityFinished" />
        /// </summary>
        /// <param name="description">Activity description</param>
        /// <param name="args">Activity details (input arguments)</param>
        /// <returns>
        ///     Activity identifier which should be used with <see cref="RecordActivityFinished" />
        /// </returns>
        public string RecordActivityStarted(string description, params object[] args)
        {
            _mutex.WaitOne();

            try
            {
                var newAction = new Activity(_currentActivity, description, args);
                _currentActivity.AddNested(newAction);
                _currentActivity = newAction;

                return newAction.Guid.ToString();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }


        /// <summary>
        ///     Record the point when activity identified with descriptor has finished.
        ///     It will have no effect when wrong activity identifier was given.
        /// </summary>
        /// <param name="descriptor">Activity identifier created by <see cref="RecordActivityStarted" /></param>
        public void RecordActivityFinished(string descriptor)
        {
            if (String.IsNullOrEmpty(descriptor))
            {
                return;
            }

            Guid actionGuid;

            try
            {
                actionGuid = Guid.Parse(descriptor);
            }
            catch (FormatException)
            {
                return;
            }

            _mutex.WaitOne();

            IActivity examinedActivity = _currentActivity;

            while (examinedActivity != null && examinedActivity.Guid != actionGuid)
            {
                examinedActivity.FinalizeActivity();
                examinedActivity = examinedActivity.Parent;
            }

            if (examinedActivity != null)
            {
                examinedActivity.FinalizeActivity();
                _currentActivity = examinedActivity.Parent;
            }

            _mutex.ReleaseMutex();
        }

        protected bool Equals(DefaultEventReport other)
        {
            return Equals(_rootActivity, other._rootActivity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DefaultEventReport) obj);
        }

        public override int GetHashCode()
        {
            return (_rootActivity != null ? _rootActivity.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"RootAction: {_rootActivity}";
        }
    }
}
