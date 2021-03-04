using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace SMAutomation
{
	// Token: 0x02000005 RID: 5
	public class SingleProgramInstance : IDisposable
	{
		// Token: 0x0600001D RID: 29
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x0600001E RID: 30
		[DllImport("user32.dll")]
		private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

		// Token: 0x0600001F RID: 31
		[DllImport("user32.dll")]
		private static extern bool IsIconic(IntPtr hWnd);

		// Token: 0x06000020 RID: 32 RVA: 0x0000511C File Offset: 0x0000331C
		public SingleProgramInstance()
		{
			this._processSync = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name, out _owned);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000514E File Offset: 0x0000334E
		public SingleProgramInstance(string identifier)
		{
			this._processSync = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name + identifier, out _owned);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00005188 File Offset: 0x00003388
		~SingleProgramInstance()
		{
			this.Release();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000051B8 File Offset: 0x000033B8
		public bool IsSingleInstance
		{
			get
			{
				return this._owned;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000051D0 File Offset: 0x000033D0
		public void RaiseOtherProcess()
		{
			Process currentProcess = Process.GetCurrentProcess();
			string name = Assembly.GetExecutingAssembly().GetName().Name;
			foreach (Process process in Process.GetProcessesByName(name))
			{
				bool flag = currentProcess.Id != process.Id;
				if (flag)
				{
					IntPtr mainWindowHandle = process.MainWindowHandle;
					bool flag2 = SingleProgramInstance.IsIconic(mainWindowHandle);
					if (flag2)
					{
						SingleProgramInstance.ShowWindowAsync(mainWindowHandle, 9);
					}
					SingleProgramInstance.SetForegroundWindow(mainWindowHandle);
					break;
				}
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00005258 File Offset: 0x00003458
		private void Release()
		{
			bool owned = this._owned;
			if (owned)
			{
				this._processSync.ReleaseMutex();
				this._owned = false;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00005285 File Offset: 0x00003485
		public void Dispose()
		{
			this.Release();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400002A RID: 42
		private const int SW_RESTORE = 9;

		// Token: 0x0400002B RID: 43
		private Mutex _processSync;

		// Token: 0x0400002C RID: 44
		private bool _owned = false;
	}
}
