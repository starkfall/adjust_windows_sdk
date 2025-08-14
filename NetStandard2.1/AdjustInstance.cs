using System;
using System.Collections.Generic;
using static AdjustSdk.NetStandard.Constants;

namespace AdjustSdk.NetStandard
{
    public class AdjustInstance
    {
        private IActivityHandler _activityHandler;
        private readonly ILogger _logger = AdjustFactory.Logger;
        private List<Action<ActivityHandler>> _preLaunchActions;
        private bool? _startEnabled = null;
        private bool _startOffline = false;
        private string _basePath;
        private string _gdprPath;

        public bool ApplicationLaunched => _activityHandler != null;

        public AdjustInstance()
        {
        }

        public void ApplicationLaunching(AdjustConfig adjustConfig, IDeviceUtil deviceUtil)
        {
            adjustConfig.PreLaunchActions = _preLaunchActions;
            adjustConfig.StartEnabled = _startEnabled;
            adjustConfig.StartOffline = _startOffline;
            adjustConfig.BasePath = _basePath;
            adjustConfig.GdprPath = _gdprPath;

            AdjustConfig.String2Sha256Func = deviceUtil.HashStringUsingSha256;
            AdjustConfig.String2Sha512Func = deviceUtil.HashStringUsingSha512;
            AdjustConfig.String2Md5Func = deviceUtil.HashStringUsingShaMd5;
            
            _activityHandler = ActivityHandler.GetInstance(adjustConfig, deviceUtil);
        }

        public void TrackEvent(AdjustEvent adjustEvent)
        {
            if (!CheckActivityHandler()) { return; }
            _activityHandler.TrackEvent(adjustEvent);
        }

        public void ApplicationActivated()
        {
            if (!CheckActivityHandler()) { return; }
            _activityHandler.ApplicationActivated();
        }

        public void ApplicationDeactivated()
        {
            if (!CheckActivityHandler()) { return; }
            _activityHandler.ApplicationDeactivated();
        }

        public void SetEnabled(bool enabled)
        {
            _startEnabled = enabled;

            if(CheckActivityHandler(enabled, "enabled mode", "disabled mode"))
            {
                _activityHandler.SetEnabled(enabled);
            }
        }

        public bool IsEnabled()
        {
            if (!CheckActivityHandler()) { return false; }
            return _activityHandler.IsEnabled();
        }

        public void SetOfflineMode(bool offlineMode)
        {
            if (!CheckActivityHandler(offlineMode, "offline mode", "online mode"))
            {
                _startOffline = offlineMode;
            }
            else
            {
                _activityHandler.SetOfflineMode(offlineMode);
            }
        }
        
        public void AppWillOpenUrl(Uri uri, IDeviceUtil deviceUtil)
        {
            var clickTime = DateTime.Now;

            if (!CheckActivityHandler())
            {
                deviceUtil.PersistSimpleValue(DEEPLINK_URL, uri.AbsoluteUri);
                deviceUtil.PersistSimpleValue(DEEPLINK_CLICK_TIME, clickTime.Ticks.ToString());
                return;
            }
            
            _activityHandler.OpenUrl(uri, clickTime);
        }

        public AdjustAttribution GetAttribution()
        {
            if (!CheckActivityHandler()) { return null; }
            return _activityHandler.GetAttribution();
        }

        /// <summary>
        /// Check if ActivityHandler instance is set or not.
        /// </summary>
        /// <param name="status">Is SDK enabled or not</param>
        /// <param name="trueMessage">Log message to display in case SDK is enabled</param>
        /// <param name="falseMessage">Log message to display in case SDK is disabled</param>
        /// <returns>boolean indicating whether ActivityHandler instance is set or not</returns>
        private bool CheckActivityHandler(bool status, string trueMessage, string falseMessage)
        {
            return CheckActivityHandler(status ? trueMessage : falseMessage);
        }

        /// <summary>
        /// Check if ActivityHandler instance is set or not.
        /// </summary>
        /// <param name="savedForLaunchWarningSuffixMessage">Log message to indicate action that was asked when SDK was disabled</param>
        /// <returns>boolean indicating whether ActivityHandler instance is set or not</returns>
        private bool CheckActivityHandler(string savedForLaunchWarningSuffixMessage = null)
        {
            if (_activityHandler == null)
            {
                if (!string.IsNullOrEmpty(savedForLaunchWarningSuffixMessage))
                {
                    _logger.Warn($"Adjust not initialized, but {savedForLaunchWarningSuffixMessage} saved for launch");
                }
                else
                {
                    _logger.Error("Adjust not initialized correctly");
                }

                return false;
            }

            return true;
        }

        public void AddSessionCallbackParameter(string key, string value)
        {
            if (_activityHandler != null)
            {
                _activityHandler.AddSessionCallbackParameter(key, value);
                return;
            }

            if (_preLaunchActions == null)
            {
                _preLaunchActions = new List<Action<ActivityHandler>>();
            }

            _preLaunchActions.Add((activityHandler) =>
            {
                activityHandler.AddSessionCallbackParameterI(key, value);
            });
        }

        public void AddSessionPartnerParameter(string key, string value)
        {
            if (_activityHandler != null)
            {
                _activityHandler.AddSessionPartnerParameter(key, value);
                return;
            }

            if (_preLaunchActions == null)
            {
                _preLaunchActions = new List<Action<ActivityHandler>>();
            }

            _preLaunchActions.Add((activityHandler) =>
            {
                activityHandler.AddSessionPartnerParameterI(key, value);
            });
        }

        public void RemoveSessionCallbackParameter(string key)
        {
            if (_activityHandler != null)
            {
                _activityHandler.RemoveSessionCallbackParameter(key);
                return;
            }

            if (_preLaunchActions == null)
            {
                _preLaunchActions = new List<Action<ActivityHandler>>();
            }

            _preLaunchActions.Add((activityHandler) =>
            {
                activityHandler.RemoveSessionCallbackParameterI(key);
            });
        }

        public void RemoveSessionPartnerParameter(string key)
        {
            if (_activityHandler != null)
            {
                _activityHandler.RemoveSessionPartnerParameter(key);
                return;
            }

            if (_preLaunchActions == null)
            {
                _preLaunchActions = new List<Action<ActivityHandler>>();
            }

            _preLaunchActions.Add((activityHandler) =>
            {
                activityHandler.RemoveSessionPartnerParameterI(key);
            });
        }

        public void ResetSessionCallbackParameters()
        {
            if (_activityHandler != null)
            {
                _activityHandler.ResetSessionCallbackParameters();
                return;
            }

            if (_preLaunchActions == null)
            {
                _preLaunchActions = new List<Action<ActivityHandler>>();
            }

            _preLaunchActions.Add((activityHandler) =>
            {
                activityHandler.ResetSessionCallbackParametersI();
            });
        }

        public void ResetSessionPartnerParameters()
        {
            if (_activityHandler != null)
            {
                _activityHandler.ResetSessionPartnerParameters();
                return;
            }

            if (_preLaunchActions == null)
            {
                _preLaunchActions = new List<Action<ActivityHandler>>();
            }

            _preLaunchActions.Add((activityHandler) =>
            {
                activityHandler.ResetSessionPartnerParametersI();
            });
        }

        public void SetPushToken(string pushToken, IDeviceUtil deviceUtil)
        {
            deviceUtil.PersistSimpleValue(ADJUST_PUSH_TOKEN, pushToken);

            if (CheckActivityHandler())
            {
                if (_activityHandler.IsEnabled())
                {
                    _activityHandler.SetPushToken(pushToken);
                }
            }
        }

        public void SendFirstPackages()
        {
            if (!CheckActivityHandler()) { return; }
            _activityHandler.SendFirstPackages();
        }

        public string GetAdid()
        {
            if (!CheckActivityHandler()) { return null; }
            return _activityHandler.GetAdid();
        }

        public void GdprForgetMe(IDeviceUtil deviceUtil)
        {
            Util.MarkGdprForgotten(deviceUtil);

            if (CheckActivityHandler("GDPR forget me"))
            {
                if (_activityHandler.IsEnabled())
                {
                    _activityHandler.SetGdprForgetMe();
                }
            }
        }

#if DEBUG
        public void SetTestOptions(IntegrationTesting.AdjustTestOptions testOptions)
        {
            if (testOptions.BasePath != null)
            {
                _basePath = testOptions.BasePath;
            }

            if (testOptions.GdprPath != null)
            {
                _gdprPath = testOptions.GdprPath;
            }

            if (testOptions.BaseUrl != null)
            {
                AdjustFactory.BaseUrl = testOptions.BaseUrl;
            }

            if (testOptions.GdprUrl != null)
            {
                AdjustFactory.GdprUrl = testOptions.GdprUrl;
            }

            if (testOptions.TimerIntervalInMilliseconds.HasValue)
            {
                var intervalMillis = testOptions.TimerIntervalInMilliseconds.Value;
                if (intervalMillis == -1)
                    AdjustFactory.SetTimerInterval(TimeSpan.FromMinutes(1));
                else
                    AdjustFactory.SetTimerInterval(TimeSpan.FromMilliseconds(intervalMillis));
            }

            if (testOptions.TimerStartInMilliseconds.HasValue)
            {
                var timerStartMillis = testOptions.TimerStartInMilliseconds.Value;
                if (timerStartMillis == -1)
                    AdjustFactory.SetTimerStart(TimeSpan.FromMinutes(1));
                else
                    AdjustFactory.SetTimerStart(TimeSpan.FromMilliseconds(timerStartMillis));
            }

            if (testOptions.SessionIntervalInMilliseconds.HasValue)
            {
                var sessionIntervalMillis = testOptions.SessionIntervalInMilliseconds.Value;
                if (sessionIntervalMillis == -1)
                    AdjustFactory.SetSessionInterval(TimeSpan.FromMinutes(30));
                else
                    AdjustFactory.SetSessionInterval(TimeSpan.FromMilliseconds(sessionIntervalMillis));
            }

            if (testOptions.SubsessionIntervalInMilliseconds.HasValue)
            {
                var subSessionIntervalMillis = testOptions.SubsessionIntervalInMilliseconds.Value;
                if (subSessionIntervalMillis == -1)
                    AdjustFactory.SetSubsessionInterval(TimeSpan.FromSeconds(1));
                else
                    AdjustFactory.SetSubsessionInterval(TimeSpan.FromMilliseconds(subSessionIntervalMillis));
            }

            if (testOptions.NoBackoffWait.HasValue && testOptions.NoBackoffWait.Value)
            {
                AdjustFactory.SetPackageHandlerBackoffStrategy(BackoffStrategy.NoWait);
                AdjustFactory.SetSdkClickHandlerBackoffStrategy(BackoffStrategy.NoWait);
            }
        }
#endif

        public void Teardown()
        {
            if (!CheckActivityHandler()) { return; }

            _activityHandler.Teardown();
            _activityHandler = null;
        }
    }
}
