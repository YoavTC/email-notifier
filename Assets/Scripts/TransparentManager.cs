using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransparentManager : MonoBehaviour
{
    #region External System 32 Methods
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32")]
    private static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);
    
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
    
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    #endregion
    
    //Constants for window style settings operations
    private const int GWL_EXSTYLE = -20;
    private const uint LWA_COLORKEY = 0x00000001;
    private const uint WS_EX_LAYERED = 0x00080000;
    private const uint WS_EX_TOOLWINDOW = 0x00000080;
    private const uint WS_EX_APPWINDOW = 0x00040000;
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_SHOWWINDOW = 0x0040;
    private const uint WS_EX_NOACTIVATE = 0x08000000;
    
    private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

    void Start()
    {
        IntPtr hWnd = hWndUtil.Get();

        Application.runInBackground = true;
        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = (int) Screen.currentResolution.refreshRateRatio.value;
        
        #if !UNITY_EDITOR
        Application.targetFrameRate = 24;
        MakeWindowTransparent(hWnd);
        MakeWindowClickThrough(hWnd);
        MakeWindowStayAtBottom(hWnd);
        #endif
    }

    private void MakeWindowTransparent(IntPtr hWnd)
    {
        MARGINS margins = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);
    }

    private void MakeWindowClickThrough(IntPtr hWnd)
    {
        int style = GetWindowLong(hWnd, GWL_EXSTYLE);

        style |= (int)(WS_EX_LAYERED | WS_EX_TOOLWINDOW);
        style &= ~(int)WS_EX_APPWINDOW;
        
        SetWindowLong(hWnd, GWL_EXSTYLE, style);
        SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);
    }
    
    private void MakeWindowStayAtBottom(IntPtr hWnd)
    {
        // Keep the window always at the bottom of the Z-order and prevent activation
        SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);

        // Modify the window style to prevent it from being activated (popping to the top)
        int style = GetWindowLong(hWnd, GWL_EXSTYLE);
        style |= (int)(WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE); // Add WS_EX_NOACTIVATE to prevent bringing the window to the front
        SetWindowLong(hWnd, GWL_EXSTYLE, style);
    }

    
    private struct MARGINS {public int cxLeftWidth; public int cxRightWidth; public int cyTopHeight; public int cyBottomHeight;}
}
