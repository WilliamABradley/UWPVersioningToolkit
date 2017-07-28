namespace UWPVersioningToolkit.Models
{
    public class VersionLog
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }

        public string New { get; set; }
        public string Fixed { get; set; }

        /// <summary>
        /// Special Versioning for the Store (Max 1500 Chars)
        /// </summary>
        public string StoreVersionSummary { get; set; }
    }
}