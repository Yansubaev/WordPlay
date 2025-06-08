using Source.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Source.DI.Installers
{
    public class SignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("<color=green>[ZEN] SignalsInstaller.InstallBindings</color>");

            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<float>().WithId("LoadingProgress").OptionalSubscriber();
            Container.DeclareSignal<RetryLoadingSignal>();
            Container.DeclareSignal<ShowMainMenuSignal>();
            Container.DeclareSignal<OpenSettingsSignal>();
            Container.DeclareSignal<CloseSettingsSignal>();
            Container.DeclareSignal<OpenGameSignal>();
            Container.DeclareSignal<StartGameSignal>();
            Container.DeclareSignal<ExitGameSignal>();
            Container.DeclareSignal<ShowGameplayScreenSignal>();
            Container.DeclareSignal<PauseGameSignal>();
            Container.DeclareSignal<ResumeGameSignal>();
        }
    }
}