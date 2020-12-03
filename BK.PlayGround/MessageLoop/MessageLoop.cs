using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace BK.PlayGround.ConsoleApp.MessageLoop
{
	class MessageLoop : IDisposable
	{
		private Thread _thread;
		private GCHandle _hookProcHandle;
		private IntPtr _hook;

		public void Init()
		{
			_thread = new Thread(Start);
			_thread.Start();
		}

		private void Start()
		{
			HookProc hookProc = Proc;
			_hookProcHandle = GCHandle.Alloc(hookProc);
			_hook = SetWindowsHookEx(14, hookProc, IntPtr.Zero, 0);

			if (_hook == IntPtr.Zero)
			{
				throw new InvalidComObjectException("cannot set hook");
			}

			MSG msg;
			int ret;
			while ((ret = GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0)
			{
				if (ret == -1)
				{
					//-1 indicates an error
				}
				else
				{
					// DispatchMessage(ref msg);
				}
			}
		}

		private IntPtr Proc(int code, IntPtr wparam, IntPtr lparam)
		{
			return CallNextHookEx(_hook, code, wparam, lparam);
		}

		[DllImport("user32.dll")]
		static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
			uint wMsgFilterMax);

		public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, uint threadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		public void Dispose()
		{
			_hookProcHandle.Free();
			// todo: unhook 
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MSG
	{
		IntPtr hwnd;
		uint message;
		UIntPtr wParam;
		IntPtr lParam;
		int time;
		POINT pt;
		int lPrivate;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		public int X;
		public int Y;

		public POINT(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public static implicit operator System.Drawing.Point(POINT p) => new System.Drawing.Point(p.X, p.Y);
		public static implicit operator POINT(System.Drawing.Point p) => new POINT(p.X, p.Y);
	}
}
