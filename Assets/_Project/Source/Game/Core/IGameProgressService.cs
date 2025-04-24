namespace Source.Game.Core
{
    public interface IGameProgressService
    {
        void SaveProgress(GameProgress progress);
        GameProgress LoadProgress();
    }
}