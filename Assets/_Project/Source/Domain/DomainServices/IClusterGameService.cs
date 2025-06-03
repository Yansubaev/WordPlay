using System.Collections.Generic;
using Source.Domain.Entities;

namespace Source.Domain.Serivces
{
    public interface IClusterGameService
    {
        void StartLevel(LevelData levelData);
        bool TryPlaceCluster(string cluster, int row, int column);
        bool TryRemoveCluster(string cluster, int row, int column);
        bool Validate(out List<string> matchedWords, out List<int> posInGrid);
        string[,] GetGridState();
        List<string> GetAvailableClusters();
    }
}