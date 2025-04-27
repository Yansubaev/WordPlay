using UnityEngine;
using Zenject;

namespace Source.Infrastructure.UI
{
    public interface ILifecycleOwner
    {
        public GameObject GameObject { get; }

        void Create(SignalBus signalBus);
        void StartLifecycle();
        void ResumeLifecycle();
        void PauseLifecycle();
        void StopLifecycle();
        void Close();
    }
}
