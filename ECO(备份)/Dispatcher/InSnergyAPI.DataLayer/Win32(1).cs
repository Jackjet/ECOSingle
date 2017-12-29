using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
namespace InSnergyAPI.DataLayer
{
	public class Win32
	{
		[Flags]
		public enum FileProtection : uint
		{
			PAGE_NOACCESS = 1u,
			PAGE_READONLY = 2u,
			PAGE_READWRITE = 4u,
			PAGE_WRITECOPY = 8u,
			PAGE_EXECUTE = 16u,
			PAGE_EXECUTE_READ = 32u,
			PAGE_EXECUTE_READWRITE = 64u,
			PAGE_EXECUTE_WRITECOPY = 128u,
			PAGE_GUARD = 256u,
			PAGE_NOCACHE = 512u,
			PAGE_WRITECOMBINE = 1024u,
			SEC_FILE = 8388608u,
			SEC_IMAGE = 16777216u,
			SEC_RESERVE = 67108864u,
			SEC_COMMIT = 134217728u,
			SEC_NOCACHE = 268435456u
		}
		[Flags]
		public enum FileMapAccess
		{
			FILE_MAP_COPY = 1,
			FILE_MAP_WRITE = 2,
			FILE_MAP_READ = 4,
			FILE_MAP_ALL_ACCESS = 983071,
			PAGE_READONLY = 2,
			PAGE_READWRITE = 4,
			PAGE_WRITECOPY = 8,
			PAGE_EXECUTE = 16,
			PAGE_EXECUTE_READ = 32,
			PAGE_EXECUTE_READWRITE = 64
		}
		public struct SECURITY_DESCRIPTOR
		{
			public byte revision;
			public byte size;
			public short control;
			public IntPtr owner;
			public IntPtr group;
			public IntPtr sacl;
			public IntPtr dacl;
		}
		internal struct SYSTEM_INFO
		{
			internal Win32._PROCESSOR_INFO_UNION uProcessorInfo;
			public uint dwPageSize;
			public IntPtr lpMinimumApplicationAddress;
			public IntPtr lpMaximumApplicationAddress;
			public IntPtr dwActiveProcessorMask;
			public uint dwNumberOfProcessors;
			public uint dwProcessorType;
			public uint dwAllocationGranularity;
			public ushort dwProcessorLevel;
			public ushort dwProcessorRevision;
		}
		[StructLayout(LayoutKind.Explicit)]
		internal struct _PROCESSOR_INFO_UNION
		{
			[FieldOffset(0)]
			internal uint dwOemId;
			[FieldOffset(0)]
			internal ushort wProcessorArchitecture;
			[FieldOffset(2)]
			internal ushort wReserved;
		}
		public struct SECURITY_ATTRIBUTES
		{
			public int nLength;
			public IntPtr lpSecurityDescriptor;
			public bool bInheritHandle;
		}
		public const int WAIT_OBJECT_0 = 0;
		public const uint INFINITE = 4294967295u;
		public const uint SECURITY_DESCRIPTOR_REVISION = 1u;
		public const uint SECTION_MAP_READ = 4u;
		public const uint ERROR_ALREADY_EXISTS = 183u;
		public const uint INVALID_HANDLE_VALUE = 4294967295u;
		public const int SECURITY_MAX_SID_SIZE = 68;
		public const int SDDL_REVISION_1 = 1;
		public const int PAGE_READWRITE = 4;
		public const int FILE_MAP_WRITE = 2;
		public const int FILE_MAP_ALL_ACCESS = 983071;
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hHandle);
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern uint GetLastError();
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool InitializeSecurityDescriptor(out IntPtr pSecurityDescriptor, uint dwRevision);
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool ConvertStringSidToSid(string StringSid, ref IntPtr ptrSid);
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool SetSecurityDescriptorDacl(ref IntPtr pSecurityDescriptor, bool daclPresent, IntPtr dacl, bool daclDefaulted);
		[DllImport("kernel32.dll")]
		internal static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref Win32.SYSTEM_INFO lpSystemInfo);
		public static IntPtr MarshalToPointer(object data)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(data));
			Marshal.StructureToPtr(data, intPtr, false);
			return intPtr;
		}
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr CreateBoundaryDescriptor([In] string Name, [In] int Flags);
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool CreateWellKnownSid([In] WellKnownSidType WellKnownSidType, [In] IntPtr DomainSid = default(IntPtr), [In] IntPtr pSid, [In] [Out] ref int cbSid);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool AddSIDToBoundaryDescriptor([In] [Out] ref IntPtr BoundaryDescriptor, [In] IntPtr RequiredSid);
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor([In] string StringSecurityDescriptor, [In] int StringSDRevision, out IntPtr SecurityDescriptor, [Out] IntPtr SecurityDescriptorSize);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr LocalFree([In] IntPtr hMem);
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr CreatePrivateNamespace([In] ref Win32.SECURITY_ATTRIBUTES lpPrivateNamespaceAttributes = null, [In] IntPtr lpBoundaryDescriptor, [In] string lpAliasPrefix);
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr CreateFileMapping([In] uint hFile, [In] ref Win32.SECURITY_ATTRIBUTES lpAttributes = null, [In] int flProtect, [In] int dwMaximumSizeHigh, [In] int dwMaximumSizeLow, [In] string lpName = null);
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern IntPtr MapViewOfFile([In] IntPtr hFileMappingObject, [In] int dwDesiredAccess, [In] int dwFileOffsetHigh, [In] int dwFileOffsetLow, [In] int dwNumberOfBytesToMap);
		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "memcpy")]
		public static extern IntPtr MemCopy(IntPtr dest, IntPtr src, uint count);
	}
}
