using System;
using System.Threading;
using Newtonsoft.Json;
using NUnitReporter.ActionReport.Actions;

namespace NUnitReporter.ActionReport
{
    public class DefaultActionReport : IActionReport
    {
        [JsonProperty]
        private readonly IAction _rootAction;

        private IAction _currentAction;

        private readonly Mutex _mutex = new Mutex();

        public DefaultActionReport()
        {
            _rootAction = _currentAction = new RootAction();
        }

        public void ActionTaken(string description, params object[] args)
        {
            _mutex.WaitOne();
            _rootAction.AddNested(new SimpleAction(_currentAction, description, args));
            _mutex.ReleaseMutex();
        }

        public string ActionStarted(string description, params object[] args)
        {
            _mutex.WaitOne();

            try
            {
                var newAction = new ComplexAction(_currentAction, description, args);
                _currentAction.AddNested(newAction);
                _currentAction = newAction;

                return newAction.Guid.ToString();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public void ActionFinished(string descriptor)
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

            IAction examinedAction = _currentAction;

            while (examinedAction != null && examinedAction.Guid != actionGuid)
            {
                examinedAction = examinedAction.Parent;
            }

            if (examinedAction != null)
            {
                _currentAction = examinedAction.Parent;
            }

            _mutex.ReleaseMutex();
        }

        public void ImageTaken(string filePath)
        {
            _mutex.WaitOne();
            _rootAction.AddNested(new ScreenshotAction(_rootAction, filePath));
            _mutex.ReleaseMutex();
        }

        public void ErrorThrown(Exception exception)
        {
            ActionTaken($"An error has occurred: {exception.Message}");
        }

        protected bool Equals(DefaultActionReport other)
        {
            return Equals(_rootAction, other._rootAction);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DefaultActionReport) obj);
        }

        public override int GetHashCode()
        {
            return (_rootAction != null ? _rootAction.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"RootAction: {_rootAction}";
        }
    }
}
