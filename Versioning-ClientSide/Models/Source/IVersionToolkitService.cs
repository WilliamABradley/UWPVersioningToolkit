using System.Threading.Tasks;

namespace UWPVersioningToolkit.Models.Source
{
    /// <summary>
    /// The Interface to Change where to Access the Changelog Information. Implement this interface to load from other sources, such as online.
    /// </summary>
    public interface IVersionToolkitService
    {
        /// <summary>
        /// Fetches the Changelog data.
        /// </summary>
        /// <returns>Changelog Data</returns>
        Task<Changelog> GetChangelog();

        /// <summary>
        /// Gets the App's Last Used Version.
        /// </summary>
        /// <returns>The Version of App that the User had used last.</returns>
        VersionCheck GetPreviousVersion();

        /// <summary>
        /// Stores the new Version of App that the User used last.
        /// </summary>
        /// <param name="version"></param>
        void StoreVersion(VersionCheck version);
    }
}