using System;
using System.Threading;
using Newtonsoft.Json;
using NUnitReporter.EventReport.Events;

namespace NUnitReporter.EventReport
{
    public class DefaultEventReport : IEventReport
    {
        [JsonProperty("root")]
        private readonly IActivity _rootActivity;

        private IActivity _currentActivity;

        private readonly Mutex _mutex = new Mutex();

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

        public void RecordEvent(string description, params object[] args)
        {
            RecordEvent(new BasicEvent(_currentActivity, description, args));
        }

        public void RecordScreenshot(string filePath)
        {
            RecordEvent(new ScreenshotEvent(_currentActivity, filePath));
        }

        public void RecordError(Exception exception)
        {
            RecordEvent(new ErrorEvent(_currentActivity, exception));
        }

        public void RecordEvent(IActivity @event)
        {
            _mutex.WaitOne();
            _currentActivity.AddNested(@event);
            _mutex.ReleaseMutex();
        }

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
