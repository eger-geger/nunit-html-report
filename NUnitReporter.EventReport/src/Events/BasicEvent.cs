using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NUnitReporter.EventReport.Events
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BasicEvent : AbstractActivity
    {
        [JsonProperty("arguments")]
        private readonly IList<String> _arguments;

        [JsonProperty("description")]
        private readonly string _description;

        [JsonConstructor]
        protected BasicEvent()
        {
            _arguments = new List<String>();
        }

        public BasicEvent(IActivity parent, string description, IEnumerable<Object> arguments) : base(parent)
        {
            _description = description;
            _arguments = arguments.Select(ConvertArgumentToString).ToList();
        }

        public string Description
        {
            get { return _description; }
        }

        public IList<String> Arguments
        {
            get { return _arguments; }
        }

        protected bool Equals(BasicEvent other)
        {
            return base.Equals(other)
                   && _arguments.SequenceEqual(other._arguments)
                   && string.Equals(_description, other._description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BasicEvent) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (_arguments != null ? _arguments.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_description != null ? _description.GetHashCode() : 0);
                return hashCode;
            }
        }

        private static String ConvertArgumentToString(Object argument)
        {
            if (argument is String)
            {
                return (String) argument;
            }

            if (argument is IEnumerable)
            {
                return String.Format("< {0} >", String.Join(", ", (argument as IEnumerable).Cast<Object>().ToArray())) ;
            }

            return argument.ToString();
        }

        public override string ToString()
        {
            return $"Guid: {Guid}, Description: {_description}, Arguments: [{string.Join(",", _arguments)}]";
        }
    }
}