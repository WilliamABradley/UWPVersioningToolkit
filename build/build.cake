#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=GitVersion.CommandLine"

using System;
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var Project = "..\\Versioning-ClientSide\\Versioning-ClientSide.csproj";
var Source = "..\\Versioning-ClientSide\\bin";
var OutputDir = ".\\bin";
var ProjectNuspec = ".\\Versioning-ClientSide.nuspec";
var PackgeOutput = ".\\nupkg";

GitVersion Version = null;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    //Wipe Copied Directory
    CleanDirectory(Directory(OutputDir));

    Information("\nCleaning Project");
    MSBuild(Project, configurator =>
        configurator.SetConfiguration(configuration)
            .SetVerbosity(Verbosity.Quiet)
            .SetMSBuildPlatform(MSBuildPlatform.x86)
            .WithTarget("Clean"));

        Information("\n");
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(Project);
});

Task("VersionUpdate")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    Version = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true
    });
    Information("New Version is: " + Version.NuGetVersionV2);
});

Task("Build")
    .IsDependentOn("VersionUpdate")
    .Does(() =>
{
    Information("\nBuilding Project");
    MSBuild(Project, configurator =>
        configurator.SetConfiguration(configuration)
            .SetVerbosity(Verbosity.Quiet)
            .SetMSBuildPlatform(MSBuildPlatform.x86)
            .WithTarget("Build")
            .WithProperty("GenerateProjectSpecificOutputFolder", "true")
            .WithProperty("GenerateLibraryLayout", "true"));

    Information("\nCopying to output Directory");
    CopyDirectory(Source, OutputDir);
});

Task("Nuget-Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    var nuGetPackSettings = new NuGetPackSettings
    {
        OutputDirectory = PackgeOutput,
        Symbols = true,
        Properties = new Dictionary<string, string>
        {
            { "binaries", OutputDir }
        }
    };

    if(Version != null){
        nuGetPackSettings.Version = Version.NuGetVersionV2;
    }

    NuGetPack(ProjectNuspec, nuGetPackSettings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Nuget-Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
