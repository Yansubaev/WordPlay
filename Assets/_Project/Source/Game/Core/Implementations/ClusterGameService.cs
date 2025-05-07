using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Game.Core
{
    public class ClusterGameService : IClusterGameService
    {
        private string[,] _grid;
        private int _wordLength;
        private int _wordCount;
        private List<string> _availableClusters;
        private HashSet<string> _targetWords;

        public void StartLevel(LevelData levelData)
        {
            if (levelData == null)
            {
                Debug.LogError("[ClusterGameService] StartLevel called with null LevelData");
                return;
            }

            _wordLength = levelData.TargetWords.Max(w => w.Length);
            _wordCount = levelData.TargetWords.Count;
            _grid = new string[_wordCount, _wordLength];
            _availableClusters = new List<string>(levelData.Clusters);
            _targetWords = levelData.TargetWords.ToHashSet();
        }

        public bool TryPlaceCluster(string cluster, int row, int column)
        {
            if (string.IsNullOrEmpty(cluster) || row < 0 || column < 0 || row >= _wordCount || column + cluster.Length > _wordLength)
            {
                Debug.LogError("[ClusterGameService] Invalid cluster placement parameters");
                return false;
            }

            if (_availableClusters.Contains(cluster))
            {
                for (int i = 0; i < cluster.Length; i++)
                {
                    _grid[row, column + i] = cluster[i].ToString();
                }
                _availableClusters.Remove(cluster);
                return true;
            }

            Debug.LogError($"[ClusterGameService] Cluster '{cluster}' is not available for placement");
            return false;
        }

        public bool Validate(out List<string> matchedWords)
        {
            matchedWords = new List<string>();

            for (int r = 0; r < _wordCount; r++)
            {
                var word = string.Concat(Enumerable.Range(0, _wordLength).Select(c => _grid[r, c] ?? ""));
                if (_targetWords.Contains(word))
                {
                    matchedWords.Add(word);
                }
            }

            return matchedWords.Count == _targetWords.Count && _availableClusters.Count == 0;
        }
    }
}