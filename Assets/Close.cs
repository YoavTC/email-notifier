using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Close : MonoBehaviour
{
    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
    
    public void FullDisplay()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    }

    public void SetMonitor1() => SetMonitor(0);
    public void SetMonitor2() => SetMonitor(1);
    public void SetMonitor3() => SetMonitor(2);

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
        FullDisplay();
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
    //
    
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
}
