using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NUnitReporter.EventReport.Events
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RootActivity : IActivity
    {
        [JsonProperty("children")]
        private readonly IList<IActivity> _nested;

        [JsonProperty]
        private readonly Guid _guid;

        public RootActivity()
        {
            _nested = new List<IActivity>();
            _guid = Guid.NewGuid();
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        public IActivity Parent
        {
            get { return null; }
        }

        public IEnumerable<IActivity> Nested
        {
            get { return _nested ?? Enumerable.Empty<IActivity>(); }
        }

        public void AddNested(IActivity activity)
        {
            _nested.Add(activity);
        }

        public void FinalizeActivity()
        {
        }

        protected bool Equals(RootActivity other)
        {
            return Guid.Equals(other.Guid) && _nested.SequenceEqual(other._nested);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RootActivity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Guid.GetHashCode()*397) ^ (_nested != null ? _nested.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"Guid: {Guid}, Nested: [<{String.Join(">,<", Nested)}>]";
        }
    }
}