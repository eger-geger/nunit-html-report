using System;
using Newtonsoft.Json;

namespace NUnitReporter.EventReport.Events
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ErrorEvent : AbstractActivity
    {
        [JsonProperty("message")]
        private readonly string _message;

        [JsonProperty("stacktrace")]
        private readonly string _stackTrace;

        [JsonConstructor]
        protected ErrorEvent()
        {
            
        }

        public ErrorEvent(IActivity parent, Exception exception) : base(parent)
        {
            _message = exception.Message;
            _stackTrace = exception.StackTrace;
        }

        public string Message
        {
            get { return _message; }
        }

        public string StackTrace
        {
            get { return _stackTrace; }
        }

        protected bool Equals(ErrorEvent other)
        {
            return base.Equals(other)
                   && string.Equals(_message, other._message)
                   && string.Equals(_stackTrace, other._stackTrace);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ErrorEvent) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (_message != null ? _message.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_stackTrace != null ? _stackTrace.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"Message: {_message}, StackTrace: {_stackTrace}";
        }
    }
}