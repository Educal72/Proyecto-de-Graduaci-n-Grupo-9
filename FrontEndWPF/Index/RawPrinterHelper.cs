using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;


namespace FrontEndWPF
{
	public class RawPrinterHelper
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public class DOCINFOA
		{
			[MarshalAs(UnmanagedType.LPStr)]
			public string pDocName;
			[MarshalAs(UnmanagedType.LPStr)]
			public string pOutputFile;
			[MarshalAs(UnmanagedType.LPStr)]
			public string pDataType;
		}

		[DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool OpenPrinter(string src, out IntPtr hPrinter, IntPtr pd);

		[DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool ClosePrinter(IntPtr hPrinter);

		[DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

		[DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool EndDocPrinter(IntPtr hPrinter);

		[DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool StartPagePrinter(IntPtr hPrinter);

		[DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool EndPagePrinter(IntPtr hPrinter);

		[DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

		public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, int dwCount)
		{
			IntPtr hPrinter = new IntPtr(0);
			DOCINFOA di = new DOCINFOA();
			bool bSuccess = false;
			int dwWritten = 0;

			di.pDocName = "RAW Document";
			di.pDataType = "RAW";

			if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
			{
				if (StartDocPrinter(hPrinter, 1, di))
				{
					if (StartPagePrinter(hPrinter))
					{
						bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
						EndPagePrinter(hPrinter);
					}
					EndDocPrinter(hPrinter);
				}
				ClosePrinter(hPrinter);
			}

			if (!bSuccess)
			{
				int dwError = Marshal.GetLastWin32Error();
				throw new Win32Exception(dwError);
			}

			return bSuccess;
		}

		public static bool SendStringToPrinter(string szPrinterName, string szString)
		{
			IntPtr pBytes;
			int dwCount;
			dwCount = szString.Length;
			pBytes = Marshal.StringToCoTaskMemAnsi(szString);
			SendBytesToPrinter(szPrinterName, pBytes, dwCount);
			Marshal.FreeCoTaskMem(pBytes);

			return true;
		}
	}
}
