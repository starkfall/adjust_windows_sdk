using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration.Pnp;
using Windows.System;

// from http://attackpattern.com/2013/03/device-information-in-windows-8-store-apps/
namespace AdjustSdk.WinUI3
{
    // Copyright (c) Attack Pattern LLC.  All rights reserved.
    // Licensed under the Apache License, Version 2.0 (the "License");
    // you may not use this file except in compliance with the License.
    // You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

    public class SystemInfoEstimate
    {
        private const string ItemNameKey = "System.ItemNameDisplay";
        private const string ModelNameKey = "System.Devices.ModelName";
        private const string ManufacturerKey = "System.Devices.Manufacturer";
        private const string DeviceClassKey = "{A45C254E-DF1C-4EFD-8020-67D146A850E0},10";
        private const string PrimaryCategoryKey = "{78C34FC8-104A-4ACA-9EA4-524D52996E57},97";
        private const string DeviceDriverVersionKey = "{A8B865DD-2E3D-4094-AD97-E593A70C75D6},3";
        private const string RootContainer = "{00000000-0000-0000-FFFF-FFFFFFFFFFFF}";
        private const string RootQuery = "System.Devices.ContainerId:=\"" + RootContainer + "\"";
        private const string HalDeviceClass = "4d36e966-e325-11ce-bfc1-08002be10318";

        public static async Task<ProcessorArchitecture> GetProcessorArchitectureAsync()
        {
            var halDevice = await GetHalDevice(ItemNameKey);
            if (halDevice != null && halDevice.Properties[ItemNameKey] != null)
            {
                var halName = halDevice.Properties[ItemNameKey].ToString();
                if (halName.Contains("x64")) return ProcessorArchitecture.X64;
                if (halName.Contains("ARM")) return ProcessorArchitecture.Arm;
                return ProcessorArchitecture.X86;
            }
            return ProcessorArchitecture.Unknown;
        }

        public static Task<string> GetDeviceManufacturerAsync()
        {
            return GetRootDeviceInfoAsync(ManufacturerKey);
        }

        public static Task<string> GetDeviceModelAsync()
        {
            return GetRootDeviceInfoAsync(ModelNameKey);
        }

        public static Task<string> GetDeviceCategoryAsync()
        {
            return GetRootDeviceInfoAsync(PrimaryCategoryKey);
        }

        public static async Task<string> GetWindowsVersionAsync()
        {
            // There is no good place to get this.
            // The HAL driver version number should work unless you're using a custom HAL...
            var hal = await GetHalDevice(DeviceDriverVersionKey);
            if (hal == null || !hal.Properties.ContainsKey(DeviceDriverVersionKey))
                return null;

            var versionParts = hal.Properties[DeviceDriverVersionKey].ToString().Split('.');
            return string.Join(".", versionParts.Take(2).ToArray());
        }

        private static async Task<string> GetRootDeviceInfoAsync(string propertyKey)
        {
            var pnp = await PnpObject.CreateFromIdAsync(PnpObjectType.DeviceContainer,
                      RootContainer, new[] { propertyKey });
            return (string)pnp.Properties[propertyKey];
        }

        private static async Task<PnpObject> GetHalDevice(params string[] properties)
        {
            var actualProperties = properties.Concat(new[] { DeviceClassKey });
            var rootDevices = await PnpObject.FindAllAsync(PnpObjectType.Device,
                actualProperties, RootQuery);

            foreach (var rootDevice in rootDevices.Where(d => d.Properties != null && d.Properties.Any()))
            {
                var lastProperty = rootDevice.Properties.Last();
                if (lastProperty.Value != null)
                    if (lastProperty.Value.ToString().Equals(HalDeviceClass))
                        return rootDevice;
            }
            return null;
        }
    }
}
