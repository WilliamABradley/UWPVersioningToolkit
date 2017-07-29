namespace UWPVersioningToolkit.Models
{
    public class VersionLog
    {
        /// <summary>
        /// Version Major Number.
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        /// Version Minor Number.
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        /// Version Build Number.
        /// </summary>
        public int Build { get; set; }

        /// <summary>
        /// Version Revision Number <para/>It is recommended you don't change this, as it is used internally by the Windows Store.
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// What is New this Version.
        /// </summary>
        public string New { get; set; }

        /// <summary>
        /// What is Fixed this Version.
        /// </summary>
        public string Fixed { get; set; }

        /// <summary>
        /// Special Versioning for the Store (Max 1500 Chars)
        /// </summary>
        public string StoreVersionSummary { get; set; }
    }
}