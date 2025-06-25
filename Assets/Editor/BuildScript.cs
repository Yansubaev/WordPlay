using UnityEditor;

public static class BuildScript
{
    public static void BuildAndroid()
    {
        string buildPath = "Build/Android/app.apk";
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath, BuildTarget.Android, BuildOptions.None);
    }

    public static void BuildWebGL()
    {
        string buildPath = "Build/WebGL";
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath, BuildTarget.WebGL, BuildOptions.None);
    }
}