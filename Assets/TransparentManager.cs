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
    #endregion
    
    //Constants for window style settings operations
    private const int GWL_EXSTYLE = -20;
    private const uint LWA_COLORKEY = 0x00000001;
    private const uint WS_EX_LAYERED = 0x00080000;
    private const uint WS_EX_TOOLWINDOW = 0x00000080;
    private const uint WS_EX_APPWINDOW = 0x00040000;
    // private const uint WS_EX_TRANSPARENT = 0x00000020;

    void Start()
    {
        IntPtr hWnd = hWndUtil.Get();

        Application.runInBackground = true;
        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = (int) Screen.currentResolution.refreshRateRatio.value;
        Application.targetFrameRate = 24;
        
        MakeWindowTransparent(hWnd);
        #if !UNITY_EDITOR
        MakeWindowClickThrough(hWnd);
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
    
    private struct MARGINS {public int cxLeftWidth; public int cxRightWidth; public int cyTopHeight; public int cyBottomHeight;}
}
