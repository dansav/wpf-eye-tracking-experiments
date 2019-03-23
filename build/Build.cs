using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Default);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "source";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    ICollection<Project> ProjectsToBuild => Solution.AllProjects
        .Where(p =>
        {
            var refList = new[]
            {
                "XRayVision",
            };
            return refList.Contains(p.Name);
        })
        .ToArray();

    Target Default => t => t
        .DependsOn(Clean, Restore, Compile);

    Target Clean => t => t
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => t => t
        .After(Clean)
        .Executes(() =>
        {
            foreach (var project in ProjectsToBuild)
            {
                Logger.Info($"Restoring NuGet for: {project.Path}");
                NuGetTasks.NuGetRestore(s => s
                    .SetTargetPath(project.Path)
                    .SetPackagesDirectory(RootDirectory / "packages"));
                Logger.Info($"END");
            }
        });

    Target Compile => t => t
        .After(Restore)
        .Executes(() =>
        {
            foreach (var project in ProjectsToBuild)
            {
                MSBuild(s => s
                    .SetTargetPath(project)
                    .SetTargets("Rebuild")
                    .SetConfiguration(Configuration)
                    .SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())
                    .SetFileVersion(GitVersion.GetNormalizedFileVersion())
                    .SetInformationalVersion(GitVersion.InformationalVersion)
                    .SetMaxCpuCount(Environment.ProcessorCount)
                    .SetNodeReuse(IsLocalBuild));
            }
        });
}