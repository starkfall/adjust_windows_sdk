using System;
using System.Collections.Generic;

namespace AdjustSdk.NetStandard
{
    public interface IActivityHandler
    {
        string BasePath { get; }

        void Init(AdjustConfig adjustConfig, IDeviceUtil deviceUtil);

        void FinishedTrackingActivity(ResponseData responseData);

        void TrackEvent(AdjustEvent adjustEvent);

        void ApplicationDeactivated();

        void ApplicationActivated();

        void SetEnabled(bool enabled);

        bool IsEnabled();

        bool IsGdprForgotten();

        void SetTrackingStateOptedOut();

        void SetOfflineMode(bool offline);

        void OpenUrl(Uri uri, DateTime clickTime);
        
        void AddSessionCallbackParameter(string key, string value);

        void AddSessionPartnerParameter(string key, string value);

        void RemoveSessionCallbackParameter(string key);

        void RemoveSessionPartnerParameter(string key);

        void ResetSessionCallbackParameters();

        void ResetSessionPartnerParameters();

        void LaunchSessionResponseTasks(SessionResponseData sessionResponseData);

        void LaunchSdkClickResponseTasks(SdkClickResponseData sdkClickResponseData);

        void LaunchAttributionResponseTasks(AttributionResponseData attributionResponseData);

        void SetAskingAttribution(bool askingAttribution);

        ActivityPackage GetAttributionPackage();

        ActivityPackage GetDeeplinkClickPackage(Dictionary<string, string> extraParameters, 
            AdjustAttribution attribution, string deeplink, DateTime clickTime);

        void SendFirstPackages();

        void SetPushToken(string pushToken);

        void SetGdprForgetMe();

        string GetAdid();

        AdjustAttribution GetAttribution();

        void Teardown();
    }
}