using Newtonsoft.Json;

namespace NUnitReporter.EventReport.Events
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ScreenshotEvent : AbstractActivity
    {
        [JsonProperty]
        private readonly string _filePath;

        [JsonConstructor]
        protected ScreenshotEvent()
        {
            
        }

        public ScreenshotEvent(IActivity parent, string imageFilePath) : base(parent)
        {
            _filePath = imageFilePath;
        }

        public string FilePath
        {
            get { return _filePath; }
        }

        protected bool Equals(ScreenshotEvent other)
        {
            return base.Equals(other) 
                && string.Equals(_filePath, other._filePath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ScreenshotEvent) obj);
        }

        public override int GetHashCode()
        {
            var hashcode = base.GetHashCode();

            hashcode = (hashcode * 397) ^ (_filePath?.GetHashCode() ?? 0);

            return hashcode;
        }
    }
}