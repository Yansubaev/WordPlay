using Cysharp.Threading.Tasks;
using Source.Infrastructure.StateMachine;
using Source.Infrastructure.StateMachine.States;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Source.Infrastructure
{
    /// <summary>
    /// This class is responsible for bootstrapping the game. It initializes the game and sets up the necessary components.
    /// </summary>
    public class ApplicationEntry : MonoBehaviour
    {
        #region private fields
        private GameStateMachine _stateMachine;
        #endregion

        #region public methods

        [Inject]
        public void Inject(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        #endregion

        #region private methods

        private async void Start()
        {
            Debug.Log("ApplicationEntry: Starting the application entry");
            SetOptimalFrameRate();

            var t = await Addressables.InitializeAsync(true);

            // Initialize the state machine with the bootstrap state
            _stateMachine.Enter<BootstrapState>().Forget();
        }

        /// <summary>
        /// Sets the optimal framerate based on device capabilities
        /// </summary>
        private void SetOptimalFrameRate()
        {
            // Get the device's refresh rate (using refreshRateRatio for newer Unity versions)
            int deviceRefreshRate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
            
            // Determine target framerate based on device capabilities
            int targetFrameRate = GetOptimalFrameRate(deviceRefreshRate);
            
            // Set the target framerate
            UnityEngine.Application.targetFrameRate = targetFrameRate;
            
            // Also set VSync if appropriate
            QualitySettings.vSyncCount = ShouldUseVSync(targetFrameRate, deviceRefreshRate) ? 1 : 0;
            
            Debug.Log($"ApplicationEntry: Device refresh rate: {deviceRefreshRate}Hz, Target framerate: {targetFrameRate}fps, VSync: {(QualitySettings.vSyncCount > 0 ? "On" : "Off")}");
        }

        /// <summary>
        /// Determines the optimal framerate based on device refresh rate and platform
        /// </summary>
        /// <param name="deviceRefreshRate">The device's refresh rate</param>
        /// <returns>Optimal target framerate</returns>
        private int GetOptimalFrameRate(int deviceRefreshRate)
        {
            // Platform-specific framerate logic
#if UNITY_ANDROID || UNITY_IOS
            // Mobile devices: Balance performance and battery life
            if (SystemInfo.processorCount >= 8 && SystemInfo.systemMemorySize >= 6000)
            {
                // High-end mobile devices
                return Mathf.Min(deviceRefreshRate, 120);
            }
            else if (SystemInfo.processorCount >= 4 && SystemInfo.systemMemorySize >= 3000)
            {
                // Mid-range mobile devices
                return Mathf.Min(deviceRefreshRate, 60);
            }
            else
            {
                // Low-end mobile devices
                return 30;
            }
#elif UNITY_STANDALONE || UNITY_EDITOR
            // Desktop: Aim for high performance
            if (SystemInfo.graphicsMemorySize >= 4000 && SystemInfo.processorCount >= 4)
            {
                // High-end desktop
                return deviceRefreshRate; // Use full refresh rate
            }
            else if (SystemInfo.graphicsMemorySize >= 2000)
            {
                // Mid-range desktop
                return Mathf.Min(deviceRefreshRate, 144);
            }
            else
            {
                // Low-end desktop
                return 60;
            }
#elif UNITY_WEBGL
            // WebGL: Conservative approach due to browser limitations
            return 60;
#else
            // Default fallback
            return 60;
#endif
        }

        /// <summary>
        /// Determines whether VSync should be used based on target framerate and device refresh rate
        /// </summary>
        /// <param name="targetFrameRate">The target framerate we want to achieve</param>
        /// <param name="deviceRefreshRate">The device's refresh rate</param>
        /// <returns>True if VSync should be enabled</returns>
        private bool ShouldUseVSync(int targetFrameRate, int deviceRefreshRate)
        {
            // Use VSync if target framerate matches device refresh rate or is a clean divisor
            // This helps prevent screen tearing
            if (targetFrameRate == deviceRefreshRate)
                return true;
                
            if (deviceRefreshRate % targetFrameRate == 0)
                return true;
                
            // For mobile devices, prefer VSync to reduce power consumption
#if UNITY_ANDROID || UNITY_IOS
            return true;
#else
            return false;
#endif
        }

        #endregion
    }
}