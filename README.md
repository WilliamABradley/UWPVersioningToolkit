# JsonVersioningGenerator
A Rudimentary Generator for Creating In-App Changelogs.

*Requires Compilation and Deployment.*

## Recommended Usage
Copy the VersionLog.cs file to your App, and then when fetching the Changelog, use the Versioning Generator to Create a new Json File to the root directory of your App, and then call this to get an ordered and detailed Changelog. (Allows for Rich Text Formatting, it is recommended to use RichTextBlocks to present result to make the most out of The Presentation features.
```C#
var changelogs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VersionLog>>(await Windows.Storage.FileIO.ReadTextAsync(await Package.Current.InstalledLocation.GetFileAsync("Changelog.json"))).OrderByDescending(v => v.Major).ThenByDescending(v => v.Minor).ThenByDescending(v => v.Build).ThenByDescending(v => v.Revision); 
```
