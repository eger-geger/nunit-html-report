using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NUnitReporter.ActionReport.Actions
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class SimpleAction : IAction
    {
        [JsonProperty]
        private readonly IList<object> _arguments;

        [JsonProperty]
        private readonly string _description;

        [JsonProperty]
        private readonly Guid _guid;

        [JsonConstructor]
        protected SimpleAction()
        {
            _arguments = new List<object>();
        }

        public SimpleAction(IAction parent, string description, IEnumerable<object> arguments)
        {
            Parent = parent;
            _description = description;
            _arguments = arguments.ToList();
            _guid = Guid.NewGuid();
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        public IAction Parent { get; }

        public string Description
        {
            get { return _description; }
        }

        public IEnumerable<object> Arguments
        {
            get { return _arguments; }
        }

        public virtual IEnumerable<IAction> Nested
        {
            get { return Enumerable.Empty<IAction>(); }
        }

        public virtual void AddNested(IAction action)
        {
            Parent.AddNested(action);
        }

        protected bool Equals(SimpleAction other)
        {
            return Guid.Equals(other.Guid)
                   && string.Equals(Description, other.Description)
                   && _arguments.SequenceEqual(other.Arguments);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SimpleAction) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Guid.GetHashCode();
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Arguments != null ? Arguments.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Parent != null ? Parent.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return
                $"Guid: {Guid}, Description: {Description}, Arguments: [{string.Join(",", Arguments)}], Nested: [<{string.Join(">,<", Nested)}>]";
        }
    }
}