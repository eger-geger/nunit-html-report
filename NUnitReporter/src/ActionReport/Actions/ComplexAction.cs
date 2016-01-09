using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace NUnitReporter.ActionReport.Actions
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ComplexAction : SimpleAction
    {
        [JsonProperty]
        private readonly IList<IAction> _nested;

        [JsonConstructor]
        protected ComplexAction() : base()
        {
            
        }

        public ComplexAction(IAction parent, string description, object[] arguments) : base(parent, description, arguments)
        {
            _nested = new List<IAction>();
        }

        public override IEnumerable<IAction> Nested
        {
            get { return _nested; }
        }

        public override void AddNested(IAction action)
        {
            _nested.Add(action);
        }

        protected bool Equals(ComplexAction other)
        {
            var equal = base.Equals(other);
            var seqEqual = _nested.SequenceEqual(other._nested);

            return equal && seqEqual;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ComplexAction) obj);
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