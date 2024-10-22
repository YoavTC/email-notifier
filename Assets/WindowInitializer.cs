using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowInitializer : MonoBehaviour
{
    #region External System 32 Methods
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    #endregion
    
    void Start()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        SetMonitor(2);
    }
    
    private void SetMonitor(int index)
    {
        Display display = Display.displays[index];
        
        int monitorWidth = display.systemWidth;
        int monitorHeight = display.systemHeight;

        int xOffset = GetXOffset(index);
        int yOffset = 0;

        IntPtr hWnd = GetActiveWindow();
        
        SetWindowPos(hWnd, IntPtr.Zero, xOffset, yOffset, monitorWidth, monitorHeight, 0x0040);
        Screen.SetResolution(display.systemWidth, display.systemHeight, true);
    }

    private int GetXOffset(int monitorIndex)
    {
        int widthToTarget = 0;
        for (int i = 0; i < monitorIndex; i++)
        {
            widthToTarget += Display.displays[i].systemWidth;
        }

        return widthToTarget;
    }
}
