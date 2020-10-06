using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using assertClicker;
using System.Drawing;

public class EnumDesktopWindowsDemo
{
    const int MAXTITLE = 255;
    static long count = 0;
    static bool isRunning = true;
    static bool exitClicked = false;
    static NotifyIcon trayIcon;

    #region invoke
    private delegate bool EnumDelegate(IntPtr hWnd, int lParam);

    [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
     ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool _EnumDesktopWindows(IntPtr hDesktop,
    EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "GetWindowText",
     ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int _GetWindowText(IntPtr hWnd,
   StringBuilder lpWindowText, int nMaxCount);


    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
   UIntPtr dwExtraInfo);
    [DllImport("user32.dll")]
    static extern short VkKeyScan(char ch);

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);
    
    #endregion

    private static bool EnumWindowsProc(IntPtr hWnd, int lParam)
    {
        string title = GetWindowText(hWnd);
        if (title == "Assertion Failed: Abort=Quit, Retry=Debug, Ignore=Continue")
        {
            count++;
            
            SetForegroundWindow(hWnd);
            PressIKey();
            //trayIcon.ShowBalloonTip(5000, "AssertClicker", count.ToString(), ToolTipIcon.Info);
        }
        return true;
    }

    public static string GetWindowText(IntPtr hWnd)
    {
        StringBuilder title = new StringBuilder(MAXTITLE);
        int titleLength = _GetWindowText(hWnd, title, title.Capacity + 1);
        title.Length = titleLength;
        return title.ToString();
    }

    public static void ClickAsserts()
    {
        EnumDelegate enumfunc = new EnumDelegate(EnumWindowsProc);
        IntPtr hDesktop = IntPtr.Zero; // current desktop
        bool success = _EnumDesktopWindows(hDesktop, enumfunc, IntPtr.Zero);
    }

    public static void PressIKey()
    {
        const int KEYEVENTF_KEYUP = 0x2;
        const int KEYEVENTF_KEYDOWN = 0x0;
        
        keybd_event((byte) VkKeyScan('i'), 0, KEYEVENTF_KEYDOWN, (UIntPtr) 0);
        keybd_event((byte)VkKeyScan('i'), 0, KEYEVENTF_KEYUP, (UIntPtr)0);
    }

    static void Main()
    {
        trayIcon = new NotifyIcon();
        trayIcon.Text = "WcfMigrationTest";
        trayIcon.Icon = Resource1.started;
        trayIcon.MouseDoubleClick += new MouseEventHandler(trayIcon_MouseDoubleClick);
        trayIcon.MouseClick += new MouseEventHandler(trayIcon_MouseClick);
        

        trayIcon.Visible = true;

        while (!exitClicked)
        {
            if (isRunning)
            {
                ClickAsserts();
            }
            Thread.Sleep(100);
            Application.DoEvents();
        }

        trayIcon.Visible = false;
    }

    static void trayIcon_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            isRunning = !isRunning;
            if (isRunning)
            {
                trayIcon.Icon = Resource1.started;
            }
            else
            {
                trayIcon.Icon = Resource1.stoppped;
            }
        }
        else
        {
            trayIcon.ShowBalloonTip(5000, "AssertClicker", count.ToString(), ToolTipIcon.Info);
        }
    }

    static void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        DialogResult dr = MessageBox.Show("Do you want to exit?",
                      "AssertClicker", MessageBoxButtons.YesNo);
        if (dr == DialogResult.Yes)
        {
            exitClicked = true;
        }
       
    }
}
