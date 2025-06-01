using System.Collections.Generic;

namespace Source.Game.Core
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