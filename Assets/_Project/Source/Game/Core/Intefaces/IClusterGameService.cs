using System.Collections.Generic;

namespace Source.Game.Core
{
    public interface IClusterGameService
    {
        void StartLevel(LevelData levelData);
        bool TryPlaceCluster(string cluster, int row, int column);
        bool Validate(out List<string> matchedWords);
        string[,] GetGridState();
        List<string> GetAvailableClusters();
    }
}