using AdjustSdk.NetStandard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Dispatching;
using Windows.ApplicationModel;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.System;
using Windows.System.Profile;

namespace AdjustSdk.WindowsStoreWinUI3
{
    /// <summary>
    ///  The main interface to Adjust.
    ///  Use the methods of this class to tell Adjust about the usage of your app.
    ///  See the README for details.
    /// </summary>
    public class Adjust
    {
        private static bool _isApplicationActive = false;

        private static AdjustInstance _adjustInstance;
        private static AdjustInstance AdjustInstance
        {
            get
            {
                if (_adjustInstance == null)
                    _adjustInstance = new AdjustInstance();
                return _adjustInstance;
            }
            set
            {
                _adjustInstance = value;
            }
        }

        private static IDeviceUtil _deviceUtil;
        private static IDeviceUtil DeviceUtil
        {
            get
            {
                if (_deviceUtil == null)
                    throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                return _deviceUtil;
            }
            set
            {
                _deviceUtil = value;
            }
        }

        private static bool IsInitialized => _deviceUtil != null;

        [Obsolete("Static setup of logging is deprecated! Use AdjustConfig constructor instead.")]
        public static void SetupLogging(Action<String> logDelegate, LogLevel? logLevel = null)
        {
            LogConfig.SetupLogging(logDelegate, logLevel);
        }

        public static bool ApplicationLaunched
        {
            get 
            { 
                if (!IsInitialized)
                    throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                    
                return AdjustInstance.ApplicationLaunched; 
            }
        }

        public static void ApplicationLaunching(AdjustConfig adjustConfig, Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue)
        {
            if (ApplicationLaunched) { return; }
            
            // Create a new DeviceUtil instance with the provided dispatcher queue
            DeviceUtil = new UtilWSWinUI3(dispatcherQueue);
            
            AdjustInstance.ApplicationLaunching(adjustConfig, DeviceUtil);
        }

        /// <summary>
        ///  Tell Adjust that the application is activated (brought to foreground).
        ///
        ///  This is used to keep track of the current session state.
        ///  This should only be used if the VisibilityChanged mechanism doesn't work
        /// </summary>
        public static void ApplicationActivated()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            if (_isApplicationActive) { return; }

            _isApplicationActive = true;
            AdjustInstance.ApplicationActivated();
        }

        /// <summary>
        ///  Tell Adjust that the application is deactivated (sent to background).
        ///
        ///  This is used to calculate session attributes like session length and subsession count.
        ///  This should only be used if the VisibilityChanged mechanism doesn't work
        /// </summary>
        public static void ApplicationDeactivated()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            if (!_isApplicationActive) { return; }

            _isApplicationActive = false;
            AdjustInstance.ApplicationDeactivated();
        }

        /// <summary>
        ///  Tell Adjust that a particular event has happened.
        /// </summary>
        /// <param name="adjustEvent">
        ///  The object that configures the event. <seealso cref="AdjustEvent"/>
        /// </param>
        public static void TrackEvent(AdjustEvent adjustEvent)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.TrackEvent(adjustEvent);
        }
        
        /// <summary>
        /// Enable or disable the adjust SDK
        /// </summary>
        /// <param name="enabled">The flag to enable or disable the adjust SDK</param>
        public static void SetEnabled(bool enabled)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.SetEnabled(enabled);
        }

        /// <summary>
        /// Check if the SDK is enabled or disabled
        /// </summary>
        /// <returns>true if the SDK is enabled, false otherwise</returns>
        public static bool IsEnabled()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            return AdjustInstance.IsEnabled();
        }

        /// <summary>
        /// Puts the SDK in offline or online mode
        /// </summary>
        /// <param name="enabled">The flag to enable or disable the adjust SDK</param>
        public static void SetOfflineMode(bool offlineMode)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.SetOfflineMode(offlineMode);
        }

        /// <summary>
        /// Read the URL that opened the application to search for
        /// an adjust deep link
        /// </summary>
        /// <param name="url">The url that open the application</param>
        public static void AppWillOpenUrl(Uri uri)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.AppWillOpenUrl(uri, DeviceUtil);
        }

        /// <summary>
        /// Get the Windows Advertising Id
        /// </summary>
        public static string GetWindowsAdId()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            return DeviceUtil.ReadWindowsAdvertisingId();
        }

        public static void AddSessionCallbackParameter(string key, string value)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.AddSessionCallbackParameter(key, value);
        }

        public static void AddSessionPartnerParameter(string key, string value)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.AddSessionPartnerParameter(key, value);
        }

        public static void RemoveSessionCallbackParameter(string key)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.RemoveSessionCallbackParameter(key);
        }

        public static void RemoveSessionPartnerParameter(string key)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.RemoveSessionPartnerParameter(key);
        }

        public static void ResetSessionCallbackParameters()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.ResetSessionCallbackParameters();
        }

        public static void ResetSessionPartnerParameters()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.ResetSessionPartnerParameters();
        }

        public static void SendFirstPackages()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.SendFirstPackages();
        }

        public static void SetPushToken(string pushToken)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.SetPushToken(pushToken, DeviceUtil);
        }

        public static string GetAdid()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            return AdjustInstance.GetAdid();
        }

        public static AdjustAttribution GetAttributon()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            return AdjustInstance.GetAttribution();
        }

        /// <summary>
        /// Give user the right to be forgotten in accordance with GDPR law.
        /// </summary>
        public static void GdprForgetMe()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Adjust SDK not initialized. Call ApplicationLaunching first.");
                
            AdjustInstance.GdprForgetMe(DeviceUtil);
        }

        public static string GetSdkVersion()
        {
            return UtilWSWinUI3.GetClientSdk();
        }

#if DEBUG
        public static void SetTestOptions(AdjustSdk.NetStandard.IntegrationTesting.AdjustTestOptions testOptions)
        {
            if (testOptions.Teardown.HasValue && testOptions.Teardown.Value)
            {
                if (AdjustInstance != null)
                {
                    AdjustInstance.Teardown();
                }

                _isApplicationActive = false;
                DeviceUtil = null;
                AdjustInstance = null;
                AdjustFactory.Teardown();

                // check whether to delete state 
                if (testOptions.DeleteState.HasValue && testOptions.DeleteState.Value)
                {
                    ClearAllPersistedObjects();
                    ClearAllPeristedValues();
                }
            }

            if (AdjustInstance == null)
                AdjustInstance = new AdjustInstance();

            AdjustInstance.SetTestOptions(testOptions);
        }

        private static void ClearAllPersistedObjects()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            Task.Run(() =>
            {
                Debug.WriteLine("About to delete local settings. Count: {0}", localSettings.Values.Count);
                localSettings.Values.Clear();
            });
        }

        private static void ClearAllPeristedValues()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            if (localFolder == null)
                return;

            Task.Run(async () =>
            {
                int filesDeletedCount = 0;
                foreach (var file in await localFolder.GetFilesAsync(CommonFileQuery.OrderByName))
                {
                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    filesDeletedCount++;
                }
                Debug.WriteLine("{0} files deleted from local folder.", filesDeletedCount);
            });
        }
#endif
    }
}
