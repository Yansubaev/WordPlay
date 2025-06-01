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
        private bool[,] _preFilledCells;
        #endregion

        #region public methods

        // Track which cells are pre-filled
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
            _preFilledCells = new bool[_wordCount, _wordLength];

            // Initialize grid
            for (int r = 0; r < _wordCount; r++)
                for (int c = 0; c < _wordLength; c++)
                {
                    _grid[r, c] = null;
                    _preFilledCells[r, c] = false;
                }

            _availableClusters = new List<string>(levelData.Clusters);
            _targetWords = new HashSet<string>(levelData.Words.Select(w => w.Solution));

            // Pre-fill letters that cannot be formed from clusters
            PreFillUnformableLetters(levelData);
        }

        public bool TryPlaceCluster(string cluster, int row, int column)
        {
            Debug.Log($"[ClusterGameService] Attempting to place cluster '{cluster}' at ({row}, {column})");

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

            // Check if the placement area is available (empty or matches pre-filled content)
            for (int i = 0; i < cluster.Length; i++)
            {
                int col = column + i;
                if (_preFilledCells[row, col])
                {
                    // Pre-filled cell must match the cluster character
                    if (_grid[row, col] != cluster[i].ToString())
                    {
                        Debug.LogError("[ClusterGameService] Cluster doesn't match pre-filled letter");
                        return false;
                    }
                }
                else if (!string.IsNullOrEmpty(_grid[row, col]))
                {
                    Debug.LogError("[ClusterGameService] Placement area is not empty");
                    return false;
                }
            }

            // Place the cluster
            for (int i = 0; i < cluster.Length; i++)
            {
                if (!_preFilledCells[row, column + i])
                {
                    _grid[row, column + i] = cluster[i].ToString();
                }
            }
            _availableClusters.Remove(cluster);
            return true;
        }

        public bool TryRemoveCluster(string cluster, int row, int column)
        {
            if (string.IsNullOrEmpty(cluster) || row < 0 || column < 0 || row >= _wordCount || column + cluster.Length > _wordLength)
            {
                Debug.LogError("[ClusterGameService] Invalid cluster removal parameters");
                return false;
            }

            // Verify cluster matches grid content
            for (int i = 0; i < cluster.Length; i++)
            {
                if (_grid[row, column + i] != cluster[i].ToString())
                {
                    Debug.LogError("[ClusterGameService] Cluster does not match grid content");
                    return false;
                }
            }

            // Remove cluster (but keep pre-filled letters)
            for (int i = 0; i < cluster.Length; i++)
            {
                if (!_preFilledCells[row, column + i])
                {
                    _grid[row, column + i] = null;
                }
            }
            _availableClusters.Add(cluster);
            return true;
        }

        public void ResetGrid()
        {
            for (int r = 0; r < _wordCount; r++)
                for (int c = 0; c < _wordLength; c++)
                {
                    if (!_preFilledCells[r, c])
                    {
                        _grid[r, c] = null;
                    }
                }
        }

        public bool Validate(out List<string> matchedWords, out List<int> posInGrid)
        {
            matchedWords = new List<string>();
            posInGrid = new List<int>();

            for (int r = 0; r < _wordCount; r++)
            {
                var word = string.Concat(Enumerable.Range(0, _wordLength).Select(c => _grid[r, c] ?? ""));
                if (_targetWords.Contains(word))
                {
                    matchedWords.Add(word);
                    posInGrid.Add(r);
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

        #region private methods

        private void PreFillUnformableLetters(LevelData levelData)
        {
            var availableClustersCopy = new List<string>(levelData.Clusters);
            var wordCoverage = new List<bool[]>();

            // Initialize coverage arrays for each word
            foreach (var word in levelData.Words)
            {
                wordCoverage.Add(new bool[word.Solution.Length]);
            }

            // Try to cover all words with available clusters using randomized order
            bool foundMatch;
            do
            {
                foundMatch = false;

                // Randomize cluster order for fair distribution
                var shuffledClusters = new List<string>(availableClustersCopy);
                for (int i = 0; i < shuffledClusters.Count; i++)
                {
                    int randomIndex = UnityEngine.Random.Range(i, shuffledClusters.Count);
                    (shuffledClusters[i], shuffledClusters[randomIndex]) = (shuffledClusters[randomIndex], shuffledClusters[i]);
                }

                foreach (var cluster in shuffledClusters)
                {
                    bool clusterUsed = false;

                    // Randomize word order for fair distribution
                    var wordIndices = Enumerable.Range(0, levelData.Words.Count).ToList();
                    for (int i = 0; i < wordIndices.Count; i++)
                    {
                        int randomIndex = UnityEngine.Random.Range(i, wordIndices.Count);
                        (wordIndices[i], wordIndices[randomIndex]) = (wordIndices[randomIndex], wordIndices[i]);
                    }

                    // Try to place this cluster in any word (randomized order)
                    foreach (int wordIndex in wordIndices)
                    {
                        if (clusterUsed) break;

                        var word = levelData.Words[wordIndex].Solution;
                        var coverage = wordCoverage[wordIndex];

                        // Generate randomized position order
                        var positions = Enumerable.Range(0, word.Length - cluster.Length + 1).ToList();
                        for (int i = 0; i < positions.Count; i++)
                        {
                            int randomIndex = UnityEngine.Random.Range(i, positions.Count);
                            (positions[i], positions[randomIndex]) = (positions[randomIndex], positions[i]);
                        }

                        // Try to place cluster at each position (randomized order)
                        foreach (int pos in positions)
                        {
                            if (CanPlaceClusterAt(word, cluster, pos, coverage))
                            {
                                // Mark positions as covered
                                for (int i = 0; i < cluster.Length; i++)
                                {
                                    coverage[pos + i] = true;
                                }

                                // Remove cluster from available list
                                availableClustersCopy.Remove(cluster);
                                foundMatch = true;
                                clusterUsed = true;
                                break;
                            }
                        }
                    }

                    if (clusterUsed) break;
                }
            } while (foundMatch);

            // Pre-fill uncovered positions
            for (int wordIndex = 0; wordIndex < levelData.Words.Count; wordIndex++)
            {
                var word = levelData.Words[wordIndex].Solution;
                var coverage = wordCoverage[wordIndex];

                for (int pos = 0; pos < word.Length; pos++)
                {
                    if (!coverage[pos])
                    {
                        _grid[wordIndex, pos] = word[pos].ToString();
                        _preFilledCells[wordIndex, pos] = true;
                    }
                }
            }
        }

        private List<int> FindUnformableLetters(string word, List<string> clusters)
        {
            var unformablePositions = new List<int>();
            var usedClusters = new List<bool>(new bool[clusters.Count]);

            // Try to cover the word with available clusters
            bool[] covered = new bool[word.Length];

            // Greedy approach: try to place clusters starting from longest
            var sortedClusters = clusters.Select((cluster, index) => new { Cluster = cluster, Index = index })
                                       .OrderByDescending(x => x.Cluster.Length)
                                       .ToList();

            bool foundMatch;
            do
            {
                foundMatch = false;
                foreach (var clusterInfo in sortedClusters)
                {
                    if (usedClusters[clusterInfo.Index]) continue;

                    // Try to place this cluster at each position
                    for (int pos = 0; pos <= word.Length - clusterInfo.Cluster.Length; pos++)
                    {
                        if (CanPlaceClusterAt(word, clusterInfo.Cluster, pos, covered))
                        {
                            // Mark positions as covered
                            for (int i = 0; i < clusterInfo.Cluster.Length; i++)
                            {
                                covered[pos + i] = true;
                            }
                            usedClusters[clusterInfo.Index] = true;
                            foundMatch = true;
                            break;
                        }
                    }
                    if (foundMatch) break;
                }
            } while (foundMatch);

            // Find uncovered positions
            for (int i = 0; i < covered.Length; i++)
            {
                if (!covered[i])
                {
                    unformablePositions.Add(i);
                }
            }

            return unformablePositions;
        }

        private bool CanPlaceClusterAt(string word, string cluster, int position, bool[] covered)
        {
            // Check if cluster matches word at this position and area is not covered
            for (int i = 0; i < cluster.Length; i++)
            {
                if (covered[position + i] || word[position + i] != cluster[i])
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}