using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;

namespace lab5
{
    public class PcInfo
    {
        public static string PcName()
        {
			string nameinfo="";
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT Name FROM Win32_ComputerSystem");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    nameinfo+="Getting your computer name.........\n";
					nameinfo += "Your computer name is stated below:\n";
					nameinfo += "Name: "+queryObj["Name"]+ "\n";
					nameinfo += "...................................\n";
					nameinfo += "...................................\n";
				}
				return nameinfo;
            }
            catch (ManagementException e)
            {
                Console.WriteLine("Klaida");
				return nameinfo;
			}
        }

        public static string PcHard()
        {
			string hardinfo = "";
			hardinfo+="Getting your computer hard drive information..."+ "\n";
			hardinfo += "..............................................." + "\n";
			hardinfo += "..............................................." + "\n";
			var driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
			foreach (ManagementObject d in driveQuery.Get())
			{
				var deviceId = d.Properties["DeviceId"].Value;
				//Console.WriteLine("Device");
				//Console.WriteLine(d);
				var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath);
				var partitionQuery = new ManagementObjectSearcher(partitionQueryText);
				foreach (ManagementObject p in partitionQuery.Get())
				{
					//Console.WriteLine("Partition");
					//Console.WriteLine(p);
					var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", p.Path.RelativePath);
					var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
					foreach (ManagementObject ld in logicalDriveQuery.Get())
					{
						//Console.WriteLine("Logical drive");
						//Console.WriteLine(ld);

						var physicalName = Convert.ToString(d.Properties["Name"].Value); // \\.\PHYSICALDRIVE2
						var diskName = Convert.ToString(d.Properties["Caption"].Value); // WDC WD5001AALS-xxxxxx
						var diskModel = Convert.ToString(d.Properties["Model"].Value); // WDC WD5001AALS-xxxxxx
						var diskInterface = Convert.ToString(d.Properties["InterfaceType"].Value); // IDE
						var capabilities = (UInt16[])d.Properties["Capabilities"].Value; // 3,4 - random access, supports writing
						var mediaLoaded = Convert.ToBoolean(d.Properties["MediaLoaded"].Value); // bool
						var mediaType = Convert.ToString(d.Properties["MediaType"].Value); // Fixed hard disk media
						var mediaSignature = Convert.ToUInt32(d.Properties["Signature"].Value); // int32
						var mediaStatus = Convert.ToString(d.Properties["Status"].Value); // OK

						var driveName = Convert.ToString(ld.Properties["Name"].Value); // C:
						var driveId = Convert.ToString(ld.Properties["DeviceId"].Value); // C:
						var driveCompressed = Convert.ToBoolean(ld.Properties["Compressed"].Value);
						var driveType = Convert.ToUInt32(ld.Properties["DriveType"].Value); // C: - 3
						var fileSystem = Convert.ToString(ld.Properties["FileSystem"].Value); // NTFS
						var freeSpace = Convert.ToUInt64(ld.Properties["FreeSpace"].Value); // in bytes
						var totalSpace = Convert.ToUInt64(ld.Properties["Size"].Value); // in bytes
						var driveMediaType = Convert.ToUInt32(ld.Properties["MediaType"].Value); // c: 12
						var volumeName = Convert.ToString(ld.Properties["VolumeName"].Value); // System
						var volumeSerial = Convert.ToString(ld.Properties["VolumeSerialNumber"].Value); // 12345678

						hardinfo += "PhysicalName: "+physicalName+"\n";
						hardinfo += "DiskName: "+diskName + "\n";
						hardinfo+="DiskModel: "+diskModel + "\n";
						hardinfo+="DiskInterface: "+diskInterface + "\n";
						hardinfo+="Capabilities: "+capabilities + "\n";
						hardinfo+="MediaLoaded: "+mediaLoaded + "\n";
						hardinfo+="MediaType: "+mediaType+"\n";
						hardinfo+="MediaSignature: "+ mediaSignature+"\n";
						hardinfo+="MediaStatus: "+mediaStatus+"\n";
						
						hardinfo+="DriveName: "+driveName+"\n";
						hardinfo+="DriveId: "+driveId+"\n";
						hardinfo+="DriveCompressed: "+driveCompressed+"\n";
						hardinfo+="DriveType: "+driveType+"\n";
						hardinfo+="FileSystem: "+fileSystem+"\n";
						hardinfo+="FreeSpace: "+freeSpace+"\n";
						hardinfo+="TotalSpace: "+totalSpace+"\n";
						hardinfo+="DriveMediaType: "+driveMediaType+"\n";
						hardinfo+="VolumeName: "+ volumeName+"\n";
						hardinfo+="VolumeSerial: "+volumeSerial+"\n";
						
						hardinfo += new string('-', 79)+"\n";
					}
				}
			}
			hardinfo += "Hard drive information found successfully"+"\n";
			hardinfo += "........................................."+"\n";
			hardinfo += "........................................."+"\n";
			return hardinfo;
		}

		public static string PcCpu()
        {
			string Cpuinfo = "";
			PerformanceCounter cpuCounter = new PerformanceCounter();
			cpuCounter.CategoryName = "Processor";
			cpuCounter.CounterName = "% Processor Time";
			cpuCounter.InstanceName = "_Total";

			dynamic firstValue = cpuCounter.NextValue();
			System.Threading.Thread.Sleep(1000);

			dynamic secondValue = cpuCounter.NextValue();

			Cpuinfo+="Calculating overall CPU usage..." + "\n";
			Cpuinfo+="CPU usage is: " + secondValue + "%" + "\n";
			Cpuinfo+="................................" + "\n";
			Cpuinfo += "................................" + "\n";
			return Cpuinfo;

		}

		public static string PcMemory()
        {
			string memoryinfo = "";
			ManagementObjectSearcher search = new ManagementObjectSearcher("root\\CIMV2", "Select TotalVisibleMemorySize, FreePhysicalMemory from Win32_OPeratingSystem");
			memoryinfo+="Getting computer total and free memory" + "\n";
			memoryinfo += "......................................" + "\n";
			foreach (ManagementObject x in search.Get())
			{
				ulong totalMemory = (ulong)x["TotalVisibleMemorySize"];
				ulong freeMemory = (ulong)x["FreePhysicalMemory"];
				memoryinfo += "Total memory in MB is: " + totalMemory + "\n";
				memoryinfo += "Total free memory in MB is: " + freeMemory + "\n";
			}
			memoryinfo += "................................." + "\n";
			memoryinfo += "................................." + "\n";
			return memoryinfo;
		}

		public static string PcAllInfo()
        {
			string allinformation = "";
			allinformation+= PcName();
			allinformation += PcHard();
			allinformation += PcCpu();
			allinformation += PcMemory();
			return allinformation;
        }
    }

	public class AllEvents
	{
		public static string LastEvents()
		{
			string lastevents = "";
			lastevents+="Searching for last 10 Event Logs..." + "\n";
			lastevents += "..................................." + "\n";
			EventLog[] remoteEventLogs;

			remoteEventLogs = EventLog.GetEventLogs();

			lastevents += "Number of logs on computer: " + remoteEventLogs.Length + "\n";

			foreach (EventLog log in remoteEventLogs)
			{
				lastevents += "Log: " + log.Log+ "\n";
			}
			return lastevents;
		}
	}
}
 