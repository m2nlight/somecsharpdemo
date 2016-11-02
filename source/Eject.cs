using System;
using System.Runtime.InteropServices;
using System.Threading;

/// <summary>
/// 弹出可移动媒体
/// </summary>
/// <see cref="http://support.microsoft.com/kb/165721"/>
public static class Eject
{
	private static void ReportError(string szMsg)
	{
		const string szErrorFormat = "Error {0}: {1}";
		var error = string.Format(szErrorFormat, GetLastError(), szMsg);
		Console.Error.WriteLine(error);
	}

	private static IntPtr OpenVolume(char driveLetter)
	{
		const string volumeFormat = "\\\\.\\{0}:";
		const string rootFormat = "{0}:\\";

		int accessFlags;

		var rootName = string.Format(rootFormat, driveLetter);
		var driveType = GetDriveTypeW(rootName);
		switch (driveType)
		{
			case DRIVE_REMOVABLE:
				accessFlags = GENERIC_READ | GENERIC_WRITE;
				break;
			case DRIVE_CDROM:
				accessFlags = GENERIC_READ;
				break;
			default:
				Console.Error.WriteLine("Cannot eject. Drive type is incorrect.");
				return new IntPtr(INVALID_HANDLE_VALUE);
		}

		var volumeName = string.Format(volumeFormat, driveLetter);
		var hVolume = CreateFileW(
			volumeName,
			accessFlags,
			FILE_SHARE_READ | FILE_SHARE_WRITE,
			IntPtr.Zero,
			OPEN_EXISTING,
			0,
			IntPtr.Zero);
		if (hVolume == new IntPtr(INVALID_HANDLE_VALUE))
			ReportError("CreateFile");

		return hVolume;
	}

	private static bool CloseVolume(IntPtr hVolume)
	{
		return CloseHandle(hVolume);
	}

	private static bool LockVolume(IntPtr hVolume)
	{
		const int LOCK_TIMEOUT = 10000; //10 Seconds
		const int LOCK_RETRIES = 20;

		var sleepAmount = LOCK_TIMEOUT / LOCK_RETRIES;

		//Do this in a loop until a timeout period has expired
		for (int tryCount = 0; tryCount < LOCK_RETRIES; tryCount++)
		{
			int dwBytesReturned;
			if (DeviceIoControl(
				hVolume,
				FSCTL_LOCK_VOLUME,
				IntPtr.Zero, 0,
				IntPtr.Zero, 0,
				out dwBytesReturned,
				IntPtr.Zero))
				return true; //return
			Thread.Sleep(sleepAmount);
		}

		return false;
	}

	private static bool DismountVolume(IntPtr hVolume)
	{
		int dwBytesReturned;
		return DeviceIoControl(
			hVolume,
			FSCTL_DISMOUNT_VOLUME,
			IntPtr.Zero, 0,
			IntPtr.Zero, 0,
			out dwBytesReturned,
			IntPtr.Zero);
	}

	private static bool PresentRemovalOfVolume(IntPtr hVolume,bool preventRemoval)
	{
		PREVENT_MEDIA_REMOVAL pmrBuffer;
		pmrBuffer.PreventMediaRemoval = preventRemoval;
		
		var size = Marshal.SizeOf(pmrBuffer);
		IntPtr ptr = Marshal.AllocHGlobal(size);
		try
		{
			Marshal.StructureToPtr(pmrBuffer, ptr, false);
			int dwBytesReturned;
			return DeviceIoControl(
				hVolume,
				IOCTL_STORAGE_MEDIA_REMOVAL,
				ptr, (uint) size,
				IntPtr.Zero, 0,
				out dwBytesReturned,
				IntPtr.Zero
				);
		}
		finally
		{
			Marshal.DestroyStructure(ptr, pmrBuffer.GetType());
		}
	}

	private static bool AutoEjectVolume(IntPtr hVolume)
	{
		int dwBytesReturned;
		return DeviceIoControl(
			hVolume,
			IOCTL_STORAGE_EJECT_MEDIA,
			IntPtr.Zero, 0,
			IntPtr.Zero, 0,
			out dwBytesReturned,
			IntPtr.Zero);
	}

	private static bool RemoveVolumeDefinition(string deviceName)
	{
		return DefineDosDeviceW(DDD_REMOVE_DEFINITION, deviceName, null);
	}
	
	public static bool EjectVolume(char driveLetter, bool removeVolumeDefinition)
	{
		var removeSafely = false;
		var autoEject = false;

		//Open the volume.
		var hVolume = OpenVolume(driveLetter);
		if (hVolume == new IntPtr(INVALID_HANDLE_VALUE))
			return false;

		//Lock and dismount the volume.
		if(LockVolume(hVolume)&&DismountVolume(hVolume))
		{
			removeSafely = true;

			//Set prevent removal to false and eject the volume.
			if (PresentRemovalOfVolume(hVolume, false) && AutoEjectVolume(hVolume))
				autoEject = true;
		}

		//Close the volume so other processes can use the drive.
		if (!CloseVolume(hVolume))
			return false;

		if(autoEject)
		{
			Console.Out.WriteLine("Media in Drive {0} has been ejected safely.",driveLetter);
		}
		else
		{
			if (removeSafely)
			{
				Console.Out.WriteLine("Media in Drive {0} can be safely removed.", driveLetter);
			}
			else
			{
				Console.Error.WriteLine("Media in Drive {0} is working, and can't be safely removed.", driveLetter);
				return false;
			}
		}

		if(removeVolumeDefinition) RemoveVolumeDefinition(string.Format("{0}:", driveLetter));
		return true;
	}

	public static void Usage()
	{
		Console.Out.WriteLine("Usage: Eject <drive letter>");
		Console.Out.WriteLine();
	}

	static void Main(string[] args)
	{
		if(args.Length != 1)
		{
			Usage();
			return;
		}
		
		if(!EjectVolume(args[0][0], true))
		{
			Console.Error.WriteLine("Failure ejecting drive {0}.",args[0][0]);
		}
	}

	#region WIN32 API

	/// Return Type: DWORD->unsigned int
	[System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetLastError")]
	public static extern uint GetLastError();

	/// Return Type: UINT->unsigned int
	///lpRootPathName: LPCWSTR->WCHAR*
	[System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetDriveTypeW")]
	public static extern uint GetDriveTypeW([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpRootPathName);

	/// Return Type: HANDLE->void*
	///lpFileName: LPCWSTR->WCHAR*
	///dwDesiredAccess: DWORD->unsigned int
	///dwShareMode: DWORD->unsigned int
	///lpSecurityAttributes: LPSECURITY_ATTRIBUTES->_SECURITY_ATTRIBUTES*
	///dwCreationDisposition: DWORD->unsigned int
	///dwFlagsAndAttributes: DWORD->unsigned int
	///hTemplateFile: HANDLE->void*
	[System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "CreateFileW")]
	public static extern System.IntPtr CreateFileW([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpFileName, int dwDesiredAccess, uint dwShareMode, [System.Runtime.InteropServices.InAttribute()] System.IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, [System.Runtime.InteropServices.InAttribute()] System.IntPtr hTemplateFile);

	/// Return Type: BOOL->int
	///hObject: HANDLE->void*
	[System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "CloseHandle")]
	[return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
	public static extern bool CloseHandle([System.Runtime.InteropServices.InAttribute()] System.IntPtr hObject);

	/// Return Type: BOOL->int
	///hDevice: HANDLE->void*
	///dwIoControlCode: DWORD->unsigned int
	///lpInBuffer: LPVOID->void*
	///nInBufferSize: DWORD->unsigned int
	///lpOutBuffer: LPVOID->void*
	///nOutBufferSize: DWORD->unsigned int
	///lpBytesReturned: LPDWORD->DWORD*
	///lpOverlapped: LPOVERLAPPED->_OVERLAPPED*
	[System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "DeviceIoControl")]
	[return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
	public static extern bool DeviceIoControl([System.Runtime.InteropServices.InAttribute()] System.IntPtr hDevice, uint dwIoControlCode, [System.Runtime.InteropServices.InAttribute()] System.IntPtr lpInBuffer, uint nInBufferSize, System.IntPtr lpOutBuffer, uint nOutBufferSize, out int lpBytesReturned, System.IntPtr lpOverlapped);


	/// DRIVE_REMOVABLE -> 2
	public const int DRIVE_REMOVABLE = 2;

	/// DRIVE_CDROM -> 5
	public const int DRIVE_CDROM = 5;

	/// INVALID_HANDLE_VALUE -> -1
	public const int INVALID_HANDLE_VALUE = -1;

	/// GENERIC_READ -> (0x80000000L)
	public const int GENERIC_READ = -2147483648;

	/// GENERIC_WRITE -> (0x40000000L)
	public const int GENERIC_WRITE = 1073741824;

	/// FILE_SHARE_READ -> 0x00000001
	public const int FILE_SHARE_READ = 1;

	/// FILE_SHARE_WRITE -> 0x00000002
	public const int FILE_SHARE_WRITE = 2;

	/// OPEN_EXISTING -> 3
	public const int OPEN_EXISTING = 3;

	//WinIoCtl.h
	//
	//#define CTL_CODE( DeviceType, Function, Method, Access ) (                 \
	//  ((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method) \
	//)
	private const int FILE_DEVICE_FILE_SYSTEM = 0x00000009;
	private const int METHOD_BUFFERED = 0;
	private const int FILE_ANY_ACCESS = 0;
	//#define FSCTL_LOCK_VOLUME               CTL_CODE(FILE_DEVICE_FILE_SYSTEM,  6, METHOD_BUFFERED, FILE_ANY_ACCESS)
	public const int FSCTL_LOCK_VOLUME = ((FILE_DEVICE_FILE_SYSTEM) << 16) | ((FILE_ANY_ACCESS) << 14) | ((6) << 2) | (METHOD_BUFFERED);
	public const int FSCTL_UNLOCK_VOLUME = ((FILE_DEVICE_FILE_SYSTEM) << 16) | ((FILE_ANY_ACCESS) << 14) | ((7) << 2) | (METHOD_BUFFERED);
	public const int FSCTL_DISMOUNT_VOLUME = ((FILE_DEVICE_FILE_SYSTEM) << 16) | ((FILE_ANY_ACCESS) << 14) | ((8) << 2) | (METHOD_BUFFERED);

	//#define IOCTL_STORAGE_BASE FILE_DEVICE_MASS_STORAGE
	private const int FILE_DEVICE_MASS_STORAGE = 0x0000002d;
	private const int IOCTL_STORAGE_BASE = FILE_DEVICE_MASS_STORAGE;
	//#define FILE_READ_ACCESS          ( 0x0001 )    // file & pipe
	private const int FILE_READ_ACCESS = 0x0001;
	
	//#define IOCTL_STORAGE_MEDIA_REMOVAL           CTL_CODE(IOCTL_STORAGE_BASE, 0x0201, METHOD_BUFFERED, FILE_READ_ACCESS)
	public const int IOCTL_STORAGE_MEDIA_REMOVAL =
		((IOCTL_STORAGE_BASE) << 16) | ((FILE_READ_ACCESS) << 14) | ((0x0201) << 2) | (METHOD_BUFFERED);
	//#define IOCTL_STORAGE_EJECT_MEDIA             CTL_CODE(IOCTL_STORAGE_BASE, 0x0202, METHOD_BUFFERED, FILE_READ_ACCESS)
	public const int IOCTL_STORAGE_EJECT_MEDIA =
		((IOCTL_STORAGE_BASE) << 16) | ((FILE_READ_ACCESS) << 14) | ((0x0202) << 2) | (METHOD_BUFFERED);


	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public struct PREVENT_MEDIA_REMOVAL
	{

		/// BOOLEAN->BYTE->unsigned char
		[MarshalAs(UnmanagedType.I1)]
		public bool PreventMediaRemoval;
	}
	
	#region Remove Volume Definition

	/// DDD_REMOVE_DEFINITION -> 0x00000002
	public const int DDD_REMOVE_DEFINITION = 2;
		
	/// Return Type: BOOL->int
	///dwFlags: DWORD->unsigned int
	///lpDeviceName: LPCWSTR->WCHAR*
	///lpTargetPath: LPCWSTR->WCHAR*
	[System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint="DefineDosDeviceW")]
	[return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
	public static extern  bool DefineDosDeviceW(uint dwFlags, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpDeviceName, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpTargetPath) ;

	#endregion
	
	#endregion
}