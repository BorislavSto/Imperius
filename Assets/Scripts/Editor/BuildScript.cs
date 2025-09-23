using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;

public class BuildScript
{
    public static void BuildWindows()
    {
        string buildPath = "Builds/Windows";
        CreateDictionary(buildPath);

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetEnabledScenes(),
            locationPathName = $"{buildPath}/TestGame.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };
        
        BuildPipeline.BuildPlayer(options);
        ZipBuild(buildPath);
    }

    public static void CreateDictionary(string path)
    {
        if (Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private static string[] GetEnabledScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

    private static void ZipBuild(string buildPath)
    {
        string zipPath = $"{buildPath}.zip";

        if (File.Exists(zipPath))
            File.Delete(zipPath);

        ZipFile.CreateFromDirectory(buildPath, zipPath);
    }
}
