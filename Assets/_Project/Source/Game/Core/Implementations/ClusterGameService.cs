using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Game.Core
{
    public class ClusterGameService : IClusterGameService
    {
        #region private fields
        private string[,] _grid;
        private int _wordLength;
        private int _wordCount;
        private List<string> _availableClusters;
        private HashSet<string> _targetWords;
        #endregion

        #region public methods

        public void StartLevel(LevelData levelData)
        {
            if (levelData == null)
            {
                Debug.LogError("[ClusterGameService] StartLevel called with null LevelData");
                return;
            }

            _wordLength = levelData.Words.Max(w => w.Solution.Length);
            _wordCount = levelData.Words.Count;
            _grid = new string[_wordCount, _wordLength];
            for (int r = 0; r < _wordCount; r++)
                for (int c = 0; c < _wordLength; c++)
                    _grid[r, c] = null;

            _availableClusters = new List<string>(levelData.Clusters);
            _targetWords = new HashSet<string>(levelData.Words.Select(w => w.Solution));
        }

        public bool TryPlaceCluster(string cluster, int row, int column)
        {
            if (string.IsNullOrEmpty(cluster) || row < 0 || column < 0 || row >= _wordCount || column + cluster.Length > _wordLength)
            {
                Debug.LogError("[ClusterGameService] Invalid cluster placement parameters");
                return false;
            }

            if (!_availableClusters.Contains(cluster))
            {
                Debug.LogError($"[ClusterGameService] Cluster '{cluster}' is not available for placement");
                return false;
            }

            // Check if the placement area is empty
            for (int i = 0; i < cluster.Length; i++)
            {
                if (!string.IsNullOrEmpty(_grid[row, column + i]))
                {
                    Debug.LogError("[ClusterGameService] Placement area is not empty");
                    return false;
                }
            }

            for (int i = 0; i < cluster.Length; i++)
            {
                _grid[row, column + i] = cluster[i].ToString();
            }
            _availableClusters.Remove(cluster);
            return true;
        }

        public bool RemoveCluster(string cluster, int row, int column)
        {
            if (string.IsNullOrEmpty(cluster) || row < 0 || column < 0 || row >= _wordCount || column + cluster.Length > _wordLength)
            {
                Debug.LogError("[ClusterGameService] Invalid cluster removal parameters");
                return false;
            }

            for (int i = 0; i < cluster.Length; i++)
            {
                if (_grid[row, column + i] != cluster[i].ToString())
                {
                    Debug.LogError("[ClusterGameService] Cluster does not match grid content");
                    return false;
                }
            }

            for (int i = 0; i < cluster.Length; i++)
            {
                _grid[row, column + i] = null;
            }
            _availableClusters.Add(cluster);
            return true;
        }

        public void ResetGrid()
        {
            for (int r = 0; r < _wordCount; r++)
                for (int c = 0; c < _wordLength; c++)
                    _grid[r, c] = null;
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

        public string[,] GetGridState()
        {
            var copy = new string[_wordCount, _wordLength];
            for (int r = 0; r < _wordCount; r++)
                for (int c = 0; c < _wordLength; c++)
                    copy[r, c] = _grid[r, c];
            return copy;
        }

        public List<string> GetAvailableClusters()
        {
            return new List<string>(_availableClusters);
        }

        #endregion
    }
}