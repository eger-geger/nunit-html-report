using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NUnitReporter.ActionReport.Actions
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class RootAction : IAction
    {
        [JsonProperty]
        private readonly IList<IAction> _nested;

        [JsonProperty]
        private readonly Guid _guid;

        public RootAction()
        {
            _nested = new List<IAction>();
            _guid = Guid.NewGuid();
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        public IAction Parent
        {
            get { return null; }
        }

        public string Description
        {
            get { return string.Empty; }
        }

        public IEnumerable<object> Arguments
        {
            get { return Enumerable.Empty<object>(); }
        }

        public IEnumerable<IAction> Nested
        {
            get { return _nested ?? Enumerable.Empty<IAction>(); }
        }

        public void AddNested(IAction action)
        {
            _nested.Add(action);
        }

        protected bool Equals(RootAction other)
        {
            var guidEqual = Guid.Equals(other.Guid); 
            var sequenceEqual= _nested.SequenceEqual(other._nested);

            return guidEqual && sequenceEqual;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RootAction) obj);
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