namespace UWPVersioningToolkit.Models
{
    /// <summary>
    /// V1 VersionLog for Parsing and Update Reasons
    /// </summary>
    public class V1VersionLog
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }
        public string Log { get; set; }
    }
}