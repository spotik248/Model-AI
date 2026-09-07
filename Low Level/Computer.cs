using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Model;
public static class Computer
{
    public static object[] os = Info();

    public static string Time() => DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");

    public static object[] Info()
    {
        //// Информацию об ОС
        object[] os = new object[13];
        os[0] = Environment.OSVersion.VersionString;
        os[1] = Environment.OSVersion.Version;
        os[2] = Environment.OSVersion.Platform;
        os[3] = Environment.Is64BitOperatingSystem ? 64 : 32;

        //// Оперативная память
        ManagementObjectSearcher ramSearcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
        foreach (ManagementObject obj in ramSearcher.Get())
        {
            ulong totalRAMBytes = Convert.ToUInt64(obj["TotalPhysicalMemory"]);
            double gbRam = Math.Round(totalRAMBytes / (1024.0 * 1024.0 * 1024.0), 2);
            os[4] = gbRam; // TODO: Переделать в из object в object[]
        }

        //// Информация о процессоре
        ManagementObjectSearcher cpuSearcher = new ManagementObjectSearcher("SELECT Name, NumberOfLogicalProcessors FROM Win32_Processor");
        foreach (ManagementObject obj in cpuSearcher.Get())
        {
            os[5] = obj["Name"]; // TODO: Переделать в из object в object[] --\
            os[6] = obj["NumberOfLogicalProcessors"]; //                 <----/
        }

        //// Свободное место на дисках
        ManagementObjectSearcher diskSearcher = new ManagementObjectSearcher("SELECT FreeSpace, Size, Caption FROM Win32_LogicalDisk WHERE DriveType=3");
        foreach (ManagementObject obj in diskSearcher.Get())
        {
            long freeSpaceBytes = Convert.ToInt64(obj["FreeSpace"]);
            long diskSizeBytes = Convert.ToInt64(obj["Size"]);
            string driveLetter = obj["Caption"].ToString() ?? "??";

            double freeGb = Math.Round(freeSpaceBytes / (1024.0 * 1024.0 * 1024.0), 2);
            double totalGb = Math.Round(diskSizeBytes / (1024.0 * 1024.0 * 1024.0), 2);

            os[7] = driveLetter;
            os[8] = totalGb; // TODO: Переделать в из object в object[]
            os[9] = freeGb;
        }

        //// Архитектура CPU и разрядность системы
        os[10] = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") ?? "unknown";

        //// Время работы системы
        DateTime upTime = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(Environment.TickCount));
        os[11] = upTime.ToString("yyyy.MM.dd HH:mm:ss");

        //// Локальный IP адрес
        IPAddress localIpAddress = IPAddress.Parse(GetLocalIP(NetworkInterfaceType.Ethernet) ?? "");
        os[12] = localIpAddress;

        return os;
    }

    public static string? GetLocalIP(NetworkInterfaceType networkInterfaceType)
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == networkInterfaceType &&
               ni.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.Address.ToString();
                    }
                }
            }
        }
        Write.Exc("No network adapters with an IPv4 address in the system!");
        return null;
    }

}