using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Source.Editor
{
    #region public classes

    public class LevelValidationData
    {
        public string levelId;
    }

    [Serializable]
    public class LevelChainData
    {
        public string[] chain;

        public LevelChainData(string[] levelIds)
        {
            chain = levelIds;
        }
    }

    public class LevelChainGenerator
    {
        #region private constants
        private const string DEFAULT_LEVELS_PATH = "Assets/_Project/Levels/En";
        private const string OUTPUT_PATH = "Assets/_Project/Levels/level_chain.json";
        #endregion

        #region public methods

        [MenuItem("Tools/Wordplay/Create Level Chain")]
        public static void CreateLevelChain()
        {
            string selectedPath = SelectLevelsDirectory();
            if (string.IsNullOrEmpty(selectedPath))
                return;

            GenerateLevelChain(selectedPath);
        }

        [MenuItem("Tools/Wordplay/Create Level Chain from En Directory")]
        public static void CreateLevelChainFromEn()
        {
            if (!Directory.Exists(DEFAULT_LEVELS_PATH))
            {
                EditorUtility.DisplayDialog(
                    "Directory Not Found",
                    $"Default En directory not found: {DEFAULT_LEVELS_PATH}",
                    "OK"
                );
                return;
            }

            GenerateLevelChain(DEFAULT_LEVELS_PATH);
        }

        #endregion

        #region private methods

        private static string SelectLevelsDirectory()
        {
            string initialDirectory = Path.GetFullPath(DEFAULT_LEVELS_PATH);

            // Check if default directory exists
            if (!Directory.Exists(initialDirectory))
            {
                initialDirectory = Path.GetFullPath("Assets/_Project/Levels");
            }

            string selectedPath = EditorUtility.OpenFolderPanel(
                "Select Levels Directory",
                initialDirectory,
                ""
            );

            if (string.IsNullOrEmpty(selectedPath))
            {
                Debug.Log("Level chain generation cancelled.");
                return null;
            }

            // Convert absolute path to relative Unity path
            string relativePath = GetRelativePath(selectedPath);
            if (string.IsNullOrEmpty(relativePath))
            {
                EditorUtility.DisplayDialog(
                    "Invalid Directory",
                    "Selected directory must be within the Unity project.",
                    "OK"
                );
                return null;
            }

            return relativePath;
        }

        private static void GenerateLevelChain(string levelsDirectory)
        {
            try
            {
                // Get all JSON files in the selected directory
                string[] jsonFiles = Directory.GetFiles(levelsDirectory, "*.json", SearchOption.TopDirectoryOnly);

                if (jsonFiles.Length == 0)
                {
                    EditorUtility.DisplayDialog(
                        "No JSON Files Found",
                        $"No JSON files found in directory: {levelsDirectory}",
                        "OK"
                    );
                    return;
                }                // Extract level IDs from filenames and sort them
                List<string> levelIds = new List<string>();
                List<string> invalidFiles = new List<string>();

                foreach (string filePath in jsonFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);

                    // Skip the level_chain.json file itself if it exists in the directory
                    if (fileName.Equals("level_chain", StringComparison.OrdinalIgnoreCase))
                        continue;

                    // Validate that this is a proper level file by checking for levelId
                    if (ValidateLevelFile(filePath, fileName))
                    {
                        levelIds.Add(fileName);
                    }
                    else
                    {
                        invalidFiles.Add(fileName);
                    }
                }

                // Show warning for invalid files
                if (invalidFiles.Count > 0)
                {
                    Debug.LogWarning($"Skipped {invalidFiles.Count} invalid level files: {string.Join(", ", invalidFiles)}");
                }                // Sort level IDs naturally (level_1, level_2, level_10, etc.)
                levelIds.Sort(NaturalStringComparer);

                // Create the level chain object
                var levelChain = new LevelChainData(levelIds.ToArray());

                // Serialize to JSON with proper formatting
                string json = JsonUtility.ToJson(levelChain, true);

                // Write to output file
                string outputFullPath = Path.GetFullPath(OUTPUT_PATH);
                Directory.CreateDirectory(Path.GetDirectoryName(outputFullPath));
                File.WriteAllText(outputFullPath, json);

                // Refresh the asset database
                AssetDatabase.Refresh();

                Debug.Log($"Level chain generated successfully with {levelIds.Count} levels!");
                Debug.Log($"Output file: {OUTPUT_PATH}");
                Debug.Log($"Levels: {string.Join(", ", levelIds)}");

                // Ping the created file in the Project window
                var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(OUTPUT_PATH);
                if (asset != null)
                {
                    EditorGUIUtility.PingObject(asset);
                }
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog(
                    "Error",
                    $"Failed to generate level chain: {e.Message}",
                    "OK"
                );
                Debug.LogError($"Level chain generation failed: {e}");
            }
        }

        private static bool ValidateLevelFile(string filePath, string fileName)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);

                // Simple validation - check if the file contains levelId property
                if (jsonContent.Contains("\"levelId\""))
                {
                    // Try to parse as JSON to ensure it's valid
                    var levelData = JsonUtility.FromJson<LevelValidationData>(jsonContent);
                    return !string.IsNullOrEmpty(levelData.levelId);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private static string GetRelativePath(string absolutePath)
        {
            string projectPath = Path.GetFullPath(Application.dataPath).Replace('\\', '/');
            absolutePath = absolutePath.Replace('\\', '/');

            if (!absolutePath.StartsWith(projectPath))
                return null;

            string relativePath = "Assets" + absolutePath.Substring(projectPath.Length);
            return relativePath;
        }

        private static int NaturalStringComparer(string x, string y)
        {
            // Extract numeric part for proper sorting (level_1, level_2, level_10)
            string[] xParts = x.Split('_');
            string[] yParts = y.Split('_');

            // If both have the same prefix and numeric suffix
            if (xParts.Length >= 2 && yParts.Length >= 2 &&
                xParts[0] == yParts[0])
            {
                if (int.TryParse(xParts[1], out int xNum) &&
                    int.TryParse(yParts[1], out int yNum))
                {
                    return xNum.CompareTo(yNum);
                }
            }

            // Fallback to string comparison
            return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }

    #endregion 
}