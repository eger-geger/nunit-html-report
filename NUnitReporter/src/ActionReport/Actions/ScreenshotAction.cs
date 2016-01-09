using Newtonsoft.Json;

namespace NUnitReporter.ActionReport.Actions
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ScreenshotAction : SimpleAction
    {
        private const string ActionDescription = "The screen-shot was taken";

        [JsonProperty]
        private readonly string _filePath;

        [JsonConstructor]
        protected ScreenshotAction() : base()
        {
            
        }

        public ScreenshotAction(IAction parent, string imageFilePath)
            : base(parent, ActionDescription, new[] {imageFilePath})
        {
            _filePath = imageFilePath;
        }

        public string FilePath
        {
            get { return _filePath; }
        }

        protected bool Equals(ScreenshotAction other)
        {
            return base.Equals(other) 
                && string.Equals(FilePath, other.FilePath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ScreenshotAction) obj);
        }

        public override int GetHashCode()
        {
            var hashcode = base.GetHashCode();

            hashcode = (hashcode * 397) ^ (FilePath?.GetHashCode() ?? 0);

            return hashcode;
        }
    }
}