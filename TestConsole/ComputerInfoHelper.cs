////////////////////////////////////////////////////////////////////////
//  file name:      ComputerInfoHelper.cs
//  created date:   2015年11月20日
//  created by:     wenkai.wu 
//  version:        0.0.1
//
////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Diagnostics;
using System.Collections;

namespace TestConsole
{
   
    internal class  Info
    {
        public enum Names
        {
            Unknow,
            Cpu_Id,
            Cpu_Name,
            Cpu_Manufacturer,
            Board_SerialNumber,
            Board_Manufacturer,
            Board_Product,
            Bios_SerialNumber,
            Bios_Manufacturer,
            MacAddress,
            DiskId,
            IpAddress,
            TotalPhysicalMemory,
            VideoPnpDeviceID,
            SoundPnpDeviceID,

            LoginUserName,
            ComputerName,
            SystemType,
        }

        public Names name;        
        public List<string> values;
        public object FirstValue
        {
            get
            {
                return values.First();
            }
        }

        public Info(Names name = Names.Unknow, List<string> values = null)
        {
            this.name = name;
            this.values = values;
        }
    }

    internal class Infos
    {
        public Info Cpu_Id;
        public Info Cpu_Name;
        public Info Cpu_Manufacturer;
        public Info DiskId;
        public Info Board_SerialNumber;
        public Info Board_Manufacturer; // 制造商
        public Info Board_Product;      // 型号
        public Info Bios_SerialNumber;
        public Info Bios_Manufacturer;
        public Info MacAddress;
        public Info SystemType;
        public Info TotalPhysicalMemory;
        public Info VideoPnpDeviceID; // 显卡
        public Info SoundPnpDeviceID; // 声卡

        public Infos()
        {
            Cpu_Id = new Info(Info.Names.Cpu_Id, GetValue("Processorid", "win32_Processor"));
            Cpu_Name = new Info(Info.Names.Cpu_Name, GetValue("Name", "win32_Processor"));
            Cpu_Manufacturer = new Info(Info.Names.Cpu_Manufacturer, GetValue("Manufacturer", "win32_Processor"));
            DiskId = new Info(Info.Names.DiskId, GetValue("Model", "win32_DiskDrive"));
            Board_SerialNumber = new Info(Info.Names.Board_SerialNumber, GetValue("SerialNumber", "Win32_baseboard"));
            Board_Manufacturer = new Info(Info.Names.Board_Manufacturer, GetValue("Manufacturer", "Win32_baseboard"));
            Board_Product = new Info(Info.Names.Board_Product, GetValue("Product", "Win32_baseboard"));
            MacAddress = new Info(Info.Names.MacAddress, GetValue("MacAddress", "Win32_NetworkAdapterConfiguration", "where MACAddress is not NULL"));
            SystemType = new Info(Info.Names.SystemType, GetValue("SystemType", "Win32_ComputerSystem"));
            TotalPhysicalMemory = new Info(Info.Names.TotalPhysicalMemory, GetValue("TotalPhysicalMemory", "Win32_ComputerSystem"));
            VideoPnpDeviceID = new Info(Info.Names.VideoPnpDeviceID, GetValue("PNPDeviceID", "Win32_VideoController"));
            SoundPnpDeviceID = new Info(Info.Names.SoundPnpDeviceID, GetValue("PNPDeviceID", "Win32_SoundDevice"));
            Bios_SerialNumber = new Info(Info.Names.Bios_SerialNumber, GetValue("SerialNumber", "Win32_BIOS"));
            Bios_Manufacturer = new Info(Info.Names.Bios_Manufacturer, GetValue("Manufacturer", "Win32_BIOS"));
        }

        public List<string> GetValue(string properity, string key, string condition = null)
        {
            var list = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format("select {0} from {1} {2}", properity, key, condition));
            foreach (ManagementObject mo in searcher.Get())
            {
                try
                {
                    if (mo.Properties != null)
                    {
                        list.Add(mo.Properties[properity].Value.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return list;
        }

        public List<Info> ToList()
        {
            return new List<Info>()
            {
                Cpu_Id,
                Cpu_Name,
                Cpu_Manufacturer,
                DiskId,
                Board_SerialNumber,
                Board_Manufacturer, 
                Board_Product,      
                Bios_SerialNumber,
                Bios_Manufacturer,
                MacAddress,
                SystemType,
                TotalPhysicalMemory,
                VideoPnpDeviceID, 
                SoundPnpDeviceID
            };
        }
    }

    class ComputerInfoHelper
    {
        public Infos Infos = new Infos();
        private static ComputerInfoHelper _helper;
        public static ComputerInfoHelper Helper
        {
            get { return _helper ?? (_helper = new ComputerInfoHelper()); }
        }
        
        public int InfoCount
        {
            get { return ListInfos.Count; }
        }

        public List<Info> ListInfos
        {
            get
            {
                return Infos.ToList();
            }
        }

        public Hashtable HashInfos
        {
            get
            {
                Hashtable hashTb = new Hashtable();
                foreach (Info info in ListInfos)
                {
                    hashTb.Add(info.name, info.FirstValue);
                }
                return hashTb;
            }
        }
    }
}