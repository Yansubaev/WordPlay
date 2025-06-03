using Source.Domain.Entities;

namespace Source.Domain.Serivces
{
    public interface IGameProgressService
    {
        void SaveProgress(GameProgress progress);
        GameProgress LoadProgress();
    }
}