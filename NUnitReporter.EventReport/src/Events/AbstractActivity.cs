using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NUnitReporter.EventReport.Events
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class AbstractActivity : IActivity
    {
        [JsonProperty]
        private readonly Guid _guid;

        [JsonProperty("startDateTime")]
        protected readonly DateTime StartDateTime;

        private readonly IActivity _parent;

        protected AbstractActivity()
        {
            
        }

        protected AbstractActivity(IActivity parent)
        {
            _parent = parent;
            _guid = Guid.NewGuid();
            StartDateTime = DateTime.UtcNow;
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        public virtual IActivity Parent
        {
            get { return _parent; }
        }

        public virtual void FinalizeActivity()
        {
        }

        public virtual IEnumerable<IActivity> Nested
        {
            get
            {
                return Enumerable.Empty<IActivity>();
            }
        }

        public virtual void AddNested(IActivity activity)
        {
            Parent.AddNested(activity);
        }

        protected bool Equals(AbstractActivity other)
        {
            return _guid.Equals(other._guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AbstractActivity) obj);
        }

        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }
    }
}