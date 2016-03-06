using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NUnitReporter.EventReport.Events
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Activity : BasicEvent
    {
        [JsonProperty("children")]
        private readonly IList<IActivity> _nested;

        [JsonProperty("endDateTime")]
        private DateTime _endDateTime;

        [JsonProperty("durationInSeconds")]
        private Int32 _durationInSeconds;

        [JsonConstructor]
        protected Activity() : base()
        {
            
        }

        public Activity(IActivity parent, string description, object[] arguments) : base(parent, description, arguments)
        {
            _nested = new List<IActivity>();
        }

        public override IEnumerable<IActivity> Nested
        {
            get
            {
                return _nested;
            }
        }

        public override void AddNested(IActivity activity)
        {
            _nested.Add(activity);
        }

        public override void FinalizeActivity()
        {
            _endDateTime = DateTime.UtcNow;
            _durationInSeconds = Convert.ToInt32(_endDateTime.Subtract(StartDateTime).TotalSeconds);
        }

        protected bool Equals(Activity other)
        {
            return base.Equals(other) && _nested.SequenceEqual(other._nested);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Activity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (_nested != null ? _nested.GetHashCode() : 0);
            }
        }
    }
}