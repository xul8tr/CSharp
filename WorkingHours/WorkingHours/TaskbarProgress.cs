// http://stackoverflow.com/questions/1295890/windows-7-progress-bar-in-taskbar-in-c
// see also: http://stackoverflow.com/questions/1024786/windows-7-taskbar-setoverlayicon-from-wpf-app-doesnt-work
#region Usings
using System;
using System.Runtime.InteropServices;
#endregion

public static class TaskbarProgress
{
    #region Enumeration
    public enum TaskbarStates
    {
        NoProgress = 0,
        Indeterminate = 0x1,
        Normal = 0x2,
        Error = 0x4,
        Paused = 0x8
    }
    #endregion

    #region Interfaces
    [ComImportAttribute()]
    [GuidAttribute("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    private interface ITaskbarList3
    {
        // ITaskbarList
        [PreserveSig]
        void HrInit();
        [PreserveSig]
        void AddTab(IntPtr hwnd);
        [PreserveSig]
        void DeleteTab(IntPtr hwnd);
        [PreserveSig]
        void ActivateTab(IntPtr hwnd);
        [PreserveSig]
        void SetActiveAlt(IntPtr hwnd);

        // ITaskbarList2
        [PreserveSig]
        void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

        // ITaskbarList3
        [PreserveSig]
        void SetProgressValue(IntPtr hwnd, UInt64 ullCompleted, UInt64 ullTotal);
        [PreserveSig]
        void SetProgressState(IntPtr hwnd, TaskbarStates state);
    }
    #endregion

    #region Fields
    private static ITaskbarList3 s_TaskbarInstance = (ITaskbarList3)new TaskbarInstance();
    private static bool s_TaskbarSupported = Environment.OSVersion.Version >= new Version(6, 1);
    #endregion

    #region Nested classes
    [GuidAttribute("56FDF344-FD6D-11d0-958A-006097C9A090")]
    [ClassInterfaceAttribute(ClassInterfaceType.None)]
    [ComImportAttribute()]
    private class TaskbarInstance
    {
    }
    #endregion

    #region Public methods
    public static void SetState(IntPtr windowHandle, TaskbarStates taskbarState)
    {
        if (s_TaskbarSupported)
        {
            s_TaskbarInstance.SetProgressState(windowHandle, taskbarState);
        }
    }

    public static void SetValue(IntPtr windowHandle, double progressValue, double progressMax)
    {
        if (s_TaskbarSupported)
        {
            s_TaskbarInstance.SetProgressValue(windowHandle, (ulong)progressValue, (ulong)progressMax);
        }
    }
    #endregion
}