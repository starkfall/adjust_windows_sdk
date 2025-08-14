using AdjustSdk.NetStandard;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.UI.Dispatching;
using AdjustSdk.NetStandard.FileSystem;

namespace AdjustSdk.WindowsStoreWinUI3
{
    public class UtilWSWinUI3 : IDeviceUtil
    {
        private readonly DispatcherQueue _dispatcherQueue;
        private DeviceInfo _deviceInfo;
        private readonly ApplicationDataContainer _localSettings;
        private readonly StorageFolder _localFolder;

        private const string PREFS_KEY_INSTALL_TRACKED = "install_tracked";

        private const double PersistValueMaxWaitSeconds = 60;

        public UtilWSWinUI3(DispatcherQueue dispatcherQueue)
        {
            // WinUI3: Use the provided dispatcher queue
            _dispatcherQueue = dispatcherQueue ?? throw new ArgumentNullException(nameof(dispatcherQueue));

            _localSettings = ApplicationData.Current.LocalSettings;
            _localFolder = ApplicationData.Current.LocalFolder;
        }

        public DeviceInfo GetDeviceInfo()
        {
            if (_deviceInfo != null) return _deviceInfo;

            var easClientDeviceInformation = new EasClientDeviceInformation();
            _deviceInfo = new DeviceInfo
            {
                ClientSdk = GetClientSdk(),
                HardwareId = GetHardwareId(),
                NetworkAdapterId = GetNetworkAdapterId(),
                AppDisplayName = GetAppDisplayName(),
                AppVersion = GetAppVersion(),
                AppPublisher = GetAppPublisher(),
                DeviceType = GetDeviceType(),
                DeviceManufacturer = GetDeviceManufacturer(),
                Architecture = GetArchitecture(),
                OsName = GetOsName(),
                OsVersion = GetOsVersion(),
                Language = GetLanguage(),
                Country = GetCountry(),
                ReadWindowsAdvertisingId = ReadWindowsAdvertisingId,
                EasFriendlyName = ExceptionWrap(() => easClientDeviceInformation.FriendlyName),
                EasId = ExceptionWrap(() => easClientDeviceInformation.Id.ToString()),
                EasOperatingSystem = ExceptionWrap(() => easClientDeviceInformation.OperatingSystem),
                EasSystemManufacturer = ExceptionWrap(() => easClientDeviceInformation.SystemManufacturer),
                EasSystemProductName = ExceptionWrap(() => easClientDeviceInformation.SystemProductName),
                EasSystemSku = ExceptionWrap(() => easClientDeviceInformation.SystemSku),
                GetConnectivityType = GetConnectivityType,
                GetNetworkType = GetNetworkType
            };

            return _deviceInfo;
        }

        public Task RunActionInForeground(Action action, Task previousTask = null)
        {
            return RunInForeground(_dispatcherQueue, () => action(), previousTask);
        }

        public void Sleep(int milliseconds)
        {
            SleepAsync(milliseconds).Wait();
        }

        public Task LauchDeeplink(Uri deepLinkUri, Task previousTask = null)
        {
            return RunInForeground(_dispatcherQueue, () => Windows.System.Launcher.LaunchUriAsync(deepLinkUri), previousTask);
        }

        public string ReadWindowsAdvertisingId()
        {
            return GetAdvertisingId();
        }

        public bool ClearSimpleValue(string key)
        {
            return _localSettings.Values.Remove(key);
        }

        public void PersistObject(string key, Dictionary<string, object> objectValuesMap)
        {
            // Implementation for persisting objects
            // This would need to be implemented based on your specific requirements
        }

        public bool PersistValue(string key, string value)
        {
            try
            {
                _localSettings.Values[key] = value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void PersistSimpleValue(string key, string value)
        {
            _localSettings.Values[key] = value;
        }

        public bool TryTakeObject(string key, out Dictionary<string, object> objectValuesMap)
        {
            if (_localSettings.Values.ContainsKey(key))
            {
                objectValuesMap = _localSettings.Values[key] as Dictionary<string, object>;
                return objectValuesMap != null;
            }
            objectValuesMap = null;
            return false;
        }

        public bool TryTakeValue(string key, out string value)
        {
            if (_localSettings.Values.ContainsKey(key))
            {
                value = _localSettings.Values[key] as string;
                return value != null;
            }
            value = null;
            return false;
        }

        public bool TryTakeSimpleValue(string key, out string value)
        {
            if (_localSettings.Values.ContainsKey(key))
            {
                value = _localSettings.Values[key] as string;
                return value != null;
            }
            value = null;
            return false;
        }

        public async Task<IFile> GetLegacyStorageFile(string fileName)
        {
            try
            {
                var file = await _localFolder.GetFileAsync(fileName);
                return new FileSystem.WinRTFile(file);
            }
            catch
            {
                return null;
            }
        }

        public string HashStringUsingSha256(string stringValue)
        {
            return HashString(stringValue, HashAlgorithmNames.Sha256);
        }

        public string HashStringUsingSha512(string stringValue)
        {
            return HashString(stringValue, HashAlgorithmNames.Sha512);
        }

        public string HashStringUsingShaMd5(string stringValue)
        {
            return HashString(stringValue, HashAlgorithmNames.Md5);
        }

        public void SetInstallTracked()
        {
            _localSettings.Values[PREFS_KEY_INSTALL_TRACKED] = true;
        }

        public bool IsInstallTracked()
        {
            return _localSettings.Values.ContainsKey(PREFS_KEY_INSTALL_TRACKED) && 
                   (bool)_localSettings.Values[PREFS_KEY_INSTALL_TRACKED];
        }

        // Helper methods that need to be implemented
        public static string GetClientSdk() => "windows-store-winui3";
        private string GetHardwareId() => "winui3-hardware-id";
        private string GetNetworkAdapterId() => "winui3-network-id";
        private string GetAppDisplayName() => "WinUI3 App";
        private string GetAppVersion() => "1.0.0";
        private string GetAppPublisher() => "Publisher";
        private string GetDeviceType() => "Desktop";
        private string GetDeviceManufacturer() => "Microsoft";
        private string GetArchitecture() => "x64";
        private string GetOsName() => "Windows";
        private string GetOsVersion() => "10.0";
        private string GetLanguage() => "en-US";
        private string GetCountry() => "US";
        private int? GetConnectivityType() => 1; // 1 for WiFi
        private int? GetNetworkType() => 1; // 1 for WiFi
        private string GetAdvertisingId() => "winui3-advertising-id";

        private string HashString(string input, string algorithmName)
        {
            try
            {
                var hashProvider = HashAlgorithmProvider.OpenAlgorithm(algorithmName);
                var hash = hashProvider.HashData(CryptographicBuffer.CreateFromByteArray(
                    System.Text.Encoding.UTF8.GetBytes(input)));
                return CryptographicBuffer.EncodeToHexString(hash);
            }
            catch
            {
                return string.Empty;
            }
        }

        private T ExceptionWrap<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch
            {
                return default(T);
            }
        }

        private async Task RunInForeground(DispatcherQueue dispatcherQueue, Action action, Task previousTask = null)
        {
            if (previousTask != null)
                await previousTask;

            if (dispatcherQueue != null)
            {
                dispatcherQueue.TryEnqueue(() => action());
            }
            else
            {
                action();
            }
        }

        private async Task SleepAsync(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }
    }
}
