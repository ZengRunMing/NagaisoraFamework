﻿using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System;

namespace NagaisoraFamework
{
	public enum AccessFlag : uint
	{
		GenericRead = 0x80000000,
		GenericWrite = 0x40000000
	}

	public enum ShareMode : uint
	{
		FileShareRead = 0x00000001,
		FileShareWrite = 0x00000002,
		FileShareDelete = 0x00000004
	}

	public enum CreateMode : uint
	{
		CreateNew = 1,
		CreateAlways = 2,
		OpenExisting = 3
	}

	public enum EMoveMethod : uint
	{
		Begin = 0,
		Current = 1,
		End = 2
	}

	public enum WMIPath
	{
		// 硬件 
		Win32_Processor,         // CPU 处理器 
		Win32_PhysicalMemory,    // 物理内存条 
		Win32_Keyboard,          // 键盘 
		Win32_PointingDevice,    // 点输入设备，包括鼠标。 
		Win32_FloppyDrive,       // 软盘驱动器 
		Win32_DiskDrive,         // 硬盘驱动器 
		Win32_CDROMDrive,        // 光盘驱动器 
		Win32_BaseBoard,         // 主板 
		Win32_BIOS,              // BIOS 芯片 
		Win32_ParallelPort,      // 并口 
		Win32_SerialPort,        // 串口 
		Win32_SerialPortConfiguration, // 串口配置 
		Win32_SoundDevice,       // 多媒体设置，一般指声卡。 
		Win32_SystemSlot,        // 主板插槽 (ISA & PCI & AGP) 
		Win32_USBController,     // USB 控制器 
		Win32_NetworkAdapter,    // 网络适配器 
		Win32_NetworkAdapterConfiguration, // 网络适配器设置 
		Win32_Printer,           // 打印机 
		Win32_PrinterConfiguration, // 打印机设置 
		Win32_PrintJob,          // 打印机任务 
		Win32_TCPIPPrinterPort,  // 打印机端口 
		Win32_POTSModem,         // MODEM 
		Win32_POTSModemToSerialPort, // MODEM 端口 
		Win32_DesktopMonitor,    // 显示器 
		Win32_DisplayConfiguration, // 显卡 
		Win32_DisplayControllerConfiguration, // 显卡设置 
		Win32_VideoController,  // 显卡细节。 
		Win32_VideoSettings,    // 显卡支持的显示模式。 

		// 操作系统 
		Win32_TimeZone,         // 时区 
		Win32_SystemDriver,     // 驱动程序 
		Win32_DiskPartition,    // 磁盘分区 
		Win32_LogicalDisk,      // 逻辑磁盘 
		Win32_LogicalDiskToPartition,     // 逻辑磁盘所在分区及始末位置。 
		Win32_LogicalMemoryConfiguration, // 逻辑内存配置 
		Win32_PageFile,         // 系统页文件信息 
		Win32_PageFileSetting,  // 页文件设置 
		Win32_BootConfiguration, // 系统启动配置 
		Win32_ComputerSystem,   // 计算机信息简要 
		Win32_OperatingSystem,  // 操作系统信息 
		Win32_StartupCommand,   // 系统自动启动程序 
		Win32_Service,          // 系统安装的服务 
		Win32_Group,            // 系统管理组 
		Win32_GroupUser,        // 系统组帐号 
		Win32_UserAccount,      // 用户帐号 
		Win32_Process,          // 系统进程 
		Win32_Thread,           // 系统线程 
		Win32_Share,            // 共享 
		Win32_NetworkClient,    // 已安装的网络客户端 
		Win32_NetworkProtocol,  // 已安装的网络协议 
	}

	public static class WindowsAPI
	{
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern SafeFileHandle CreateFile(string FileName, AccessFlag accessFlag, ShareMode shareMode, IntPtr securityAttr, CreateMode createMode, uint flagsAndAttributes, IntPtr templateFile);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern uint SetFilePointer([In] SafeFileHandle hFile, [In] long lDistanceToMove, IntPtr lpDistanceToMoveHigh, [In] EMoveMethod dwMoveMethod);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetFilePointerEx([In] SafeFileHandle hFile, [In] long lDistanceToMove, IntPtr lpDistanceToMoveHigh, [In] EMoveMethod dwMoveMethod);
		[DllImport("kernel32.dll")]
		public static extern SafeFileHandle OpenProcess(uint flag, bool ihh, int processid);
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(SafeFileHandle handle, int address, int[] buffer, int size, int[] nor);
		[DllImport("kernel32.dll")]
		public static extern SafeFileHandle WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, int size, out IntPtr lpNumberOfBytesWritten);
		[DllImport("kernel32", EntryPoint = "CreateRemoteThread")]
		public static extern int CreateRemoteThread(int hProcess, int lpThreadAttributes, int dwStackSize, int lpStartAddress, int lpParameter, int dwCreationFlags, ref int lpThreadId);
		[DllImport("Kernel32.dll")]
		public static extern SafeFileHandle VirtualAllocEx(IntPtr hProcess, int lpAddress, int dwSize, short flAllocationType, short flProtect);
		[DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
		public static extern int CloseHandle(int hObject);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		public const uint GENERIC_READ = 0x80000000;
		public const uint GENERIC_WRITE = 0x40000000;
		public const uint FILE_SHARE_READ = 0x00000001;
		public const uint FILE_SHARE_WRITE = 0x00000002;
		public const uint OPEN_EXISTING = 3;
		public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
	}

}
