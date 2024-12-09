using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class WindowMover : MonoBehaviour
{
    #region External System32 Methods
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        uint uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool EnumDisplayMonitors(
        IntPtr hdc,
        IntPtr lprcClip,
        MonitorEnumProc lpfnEnum,
        IntPtr dwData);

    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_NOACTIVATE = 0x0010;

    [SerializeField] private TMP_Text buttonDisplay;

    private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    #endregion

    private int currentMonitorIndex = 0;
    private Rect[] monitorBounds;

    private void Start()
    {
        CollectMonitorBounds();
        #if !UNITY_EDITOR
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        #endif
    }

    public void MoveToNextMonitor()
    {
        if (monitorBounds == null || monitorBounds.Length <= 1)
        {
            Debug.LogWarning("Only one monitor detected. Cannot move to the next monitor.");
            return;
        }

        // Cycle to the next monitor
        currentMonitorIndex = (currentMonitorIndex + 1) % monitorBounds.Length;
        MoveWindowToMonitor(currentMonitorIndex);
        
        // Update button display
        buttonDisplay.text = $"Monitor: {currentMonitorIndex}";
    }

    private void CollectMonitorBounds()
    {
        var bounds = new System.Collections.Generic.List<Rect>();

        EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData) =>
        {
            bounds.Add(lprcMonitor);
            return true; // Continue enumeration
        }, IntPtr.Zero);

        monitorBounds = bounds.ToArray();
    }

    private void MoveWindowToMonitor(int index)
    {
        if (index < 0 || index >= monitorBounds.Length)
        {
            Debug.LogError("Monitor index out of range.");
            return;
        }

        Rect targetMonitor = monitorBounds[index];
        IntPtr hWnd = GetActiveWindow();

        if (hWnd == IntPtr.Zero)
        {
            Debug.LogError("Failed to get active window handle.");
            return;
        }

        int width = targetMonitor.Right - targetMonitor.Left;
        int height = targetMonitor.Bottom - targetMonitor.Top;

        // Calculate center position of the monitor
        int centerX = targetMonitor.Left + (width / 2) - (Screen.width / 2);
        int centerY = targetMonitor.Top + (height / 2) - (Screen.height / 2);

        // Move the window
        bool result = SetWindowPos(
            hWnd,
            IntPtr.Zero,
            centerX,
            centerY,
            Screen.width,
            Screen.height,
            SWP_NOZORDER | SWP_NOACTIVATE
        );

        if (!result)
        {
            Debug.LogError("Failed to set window position.");
        }
    }
}
