#tool nuget:?package=GitVersion.CommandLine&version=5.0.0
#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

GitVersion version;
ConvertableDirectoryPath sourceDir;
ConvertableDirectoryPath outputDir;
ConvertableFilePath solution;

Setup(ctx =>
{
   version = GitVersion();

   sourceDir = Directory("./source");
   outputDir = Directory("./output");
   solution = File("./wpf-eye-tracking-experiments.sln");

   Information($"Version: {version.SemVer}");
   Information($"Git branch: {version.BranchName}");
   Information($"Build provider: {BuildSystem.Provider}");

   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean").Does(() =>
{
    CleanDirectory(outputDir);
});

Task("UpdateVersion").Does(() =>
{
    CreateAssemblyInfo(outputDir + File("AssemblyVersion.generated.cs"), new AssemblyInfoSettings {
        Version = version.MajorMinorPatch,
        FileVersion = version.MajorMinorPatch,
        InformationalVersion = version.InformationalVersion,
    });
});

Task("Build")
    .Does(() =>
{
        var latestInstallationPath = VSWhereLatest(new VSWhereLatestSettings { Requires = "Microsoft.Component.MSBuild" });
        var msbuildPath = latestInstallationPath.CombineWithFilePath("MSBuild/current/Bin/MSBuild.exe");
        var settings = new MSBuildSettings
        {
            ToolPath = msbuildPath,
            Configuration = configuration,
        };

        MSBuild(solution, settings.WithTarget("Rebuild"));
});

Task("Test")
    .Does(() =>
{
   NUnit3("./source/**/bin/Release/*.Tests.dll", new NUnit3Settings
   {
       X86 = true,
       Results = new[]
       {
            new NUnit3Result
            {
               FileName = outputDir + File("TestResult.xml")
            }
        },  
   });
});

Task("Default")
   .IsDependentOn("Clean")
   .IsDependentOn("UpdateVersion")
   .IsDependentOn("Build")
   .IsDependentOn("Test")
   ;

RunTarget(target);