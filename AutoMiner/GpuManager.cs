using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MSI.Afterburner;

namespace AutoMiner
{
    public class GpuManager
    {
        public static List<GPU> GetGPUs()
        {
            HardwareMonitor mahm = new HardwareMonitor();
            ControlMemory macm = new ControlMemory();
            List<GPU> gpus = new List<GPU>();
            for (int i = 0; i < mahm.Header.GpuEntryCount; i++)
            {
                GpuInfoWrapper gpuInfoWrapper = extractGpuInfo(macm.GpuEntries[i]);
                OcProfile ocProfile = ConfigService.getOcProfile(mahm.GpuEntries[i].GpuId);
                if (ocProfile == null)
                {
                    ocProfile = new OcProfile
                    {
                        miningCoreClockBoost = gpuInfoWrapper.coreClockBoost.def,
                        notMiningCoreClockBoost = gpuInfoWrapper.coreClockBoost.def,
                        miningMemoryClockBoost = gpuInfoWrapper.memoryClockBoost.def,
                        notMiningMemoryClockBoost = gpuInfoWrapper.memoryClockBoost.def,
                        miningPowerLimit = gpuInfoWrapper.powerLimit.def,
                        notMiningPowerLimit = gpuInfoWrapper.powerLimit.def,
                        gpuId = mahm.GpuEntries[i].GpuId
                    };
                    ConfigService.appConfig.ocProfiles.Add(ocProfile);
                    ConfigService.saveConfig();
                }
                gpus.Add(new GPU { 
                    name = mahm.GpuEntries[i].Device, 
                    index = i ,
                    gpuInfo = gpuInfoWrapper,
                    ocProfile = ocProfile
                });
            }
            return gpus;
        }

        public static void applyMiningOcParams()
        {
            HardwareMonitor mahm = new HardwareMonitor();
            ControlMemory macm = new ControlMemory();
            bool changes = false;
            for (int i = 0; i < mahm.Header.GpuEntryCount; i++)
            {
                OcProfile profile = ConfigService.getOcProfile(mahm.GpuEntries[i].GpuId);
                if (profile != null)
                {
                    GpuInfoWrapper gpu = extractGpuInfo(macm.GpuEntries[i]);
                    if (gpu.coreClockBoost.current != profile.miningCoreClockBoost)
                    {
                        changes = true;
                        macm.GpuEntries[i].CoreClockBoostCur = profile.miningCoreClockBoost;
                    }
                    if (gpu.memoryClockBoost.current != profile.miningMemoryClockBoost)
                    {
                        changes = true;
                        macm.GpuEntries[i].MemoryClockBoostCur = profile.miningMemoryClockBoost;
                    }
                    if (gpu.powerLimit.current != profile.miningPowerLimit)
                    {
                        changes = true;
                        macm.GpuEntries[i].PowerLimitCur = profile.miningPowerLimit;
                    }
                }
            }
            if (changes)
            {
                Trace.WriteLine("Applying Oc Parames");
                macm.CommitChanges();
            }
        }

        public static void applyNotMiningOcParams()
        {
            HardwareMonitor mahm = new HardwareMonitor();
            ControlMemory macm = new ControlMemory();
            bool changes = false;
            for (int i = 0; i < mahm.Header.GpuEntryCount; i++)
            {
                OcProfile profile = ConfigService.getOcProfile(mahm.GpuEntries[i].GpuId);
                if (profile != null)
                {
                    GpuInfoWrapper gpu = extractGpuInfo(macm.GpuEntries[i]);
                    if (gpu.coreClockBoost.current != profile.notMiningCoreClockBoost)
                    {
                        changes = true;
                        macm.GpuEntries[i].CoreClockBoostCur = profile.notMiningCoreClockBoost;
                    }
                    if (gpu.memoryClockBoost.current != profile.notMiningMemoryClockBoost)
                    {
                        changes = true;
                        macm.GpuEntries[i].MemoryClockBoostCur = profile.notMiningMemoryClockBoost;
                    }
                    if (gpu.powerLimit.current != profile.notMiningPowerLimit)
                    {
                        changes = true;
                        macm.GpuEntries[i].PowerLimitCur = profile.notMiningPowerLimit;
                    }
                }
            }
            if (changes)
            {
                Trace.WriteLine("Applying Oc Parames");
                macm.CommitChanges();
            }
        }

        private static GpuInfoWrapper extractGpuInfo(ControlMemoryGpuEntry entry)
        {
            return new GpuInfoWrapper
            {
                powerLimit = new OcInfoWrapper
                {
                    current = entry.PowerLimitCur,
                    def = entry.PowerLimitDef,
                    max = entry.PowerLimitMax,
                    min = entry.PowerLimitMin
                },
                coreClockBoost = new OcInfoWrapper
                {
                    current = entry.CoreClockBoostCur,
                    def = entry.CoreClockBoostDef,
                    max = entry.CoreClockBoostMax,
                    min = entry.CoreClockBoostMin,
                },
                memoryClockBoost = new OcInfoWrapper
                {
                    current = entry.MemoryClockBoostCur,
                    def = entry.MemoryClockBoostDef,
                    max = entry.MemoryClockBoostMax,
                    min = entry.MemoryClockBoostMin
                },
                fanSpeed = new OcInfoWrapper
                {
                    current = (int)entry.FanSpeedCur,
                    def = (int)entry.FanSpeedDef,
                    max = (int)entry.FanSpeedMax,
                    min = (int)entry.FanSpeedMin
                }
            };
        }
    }

    public class GPU
    {
        public string name { get; set; }
        public int index { get; set; }
        public GpuInfoWrapper gpuInfo { get; set; }
        public OcProfile ocProfile { get; set; }
    }

    public class GpuInfoWrapper
    {
        public OcInfoWrapper powerLimit { get; set; }
        public OcInfoWrapper coreClockBoost { get; set; }
        public OcInfoWrapper memoryClockBoost { get; set; }
        public OcInfoWrapper fanSpeed { get; set; }
    }

    public class OcInfoWrapper
    {
        public int current { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int def { get; set; }
    }

    public class OcProfile
    {
        public string gpuId { get; set; }
        public int miningPowerLimit { get; set; }
        public int miningCoreClockBoost { get; set; }
        public int miningMemoryClockBoost { get; set; }
        public int notMiningPowerLimit { get; set; }
        public int notMiningCoreClockBoost { get; set; }
        public int notMiningMemoryClockBoost { get; set; }

    }
    
}
