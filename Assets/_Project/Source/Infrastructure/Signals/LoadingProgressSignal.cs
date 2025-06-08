namespace Source.Infrastructure.Signals
{
    public class LoadingProgressSignal
    {
        public float ProgressNomalized { get; }
        public float ProgressPercentage => ProgressNomalized * 100f;

        public LoadingProgressSignal(float progress)
        {
            ProgressNomalized = progress;
        }
    }
}
