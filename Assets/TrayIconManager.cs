using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Application = UnityEngine.Application;

public class TrayIconManager : MonoBehaviour
{
    #region External System 32 Methods
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern int Shell_NotifyIcon(uint dwMessage, ref NOTIFYICONDATA lpData);
    
    [DllImport("user32.dll")]
    private static extern bool DestroyIcon(IntPtr hIcon);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr LoadImage(IntPtr hInst, string lpszName, uint uType, int cx, int cy, uint uFlags);
    #endregion

    //Constants for system tray operations
    private const uint NIM_ADD = 0x00000000;
    private const uint NIM_DELETE = 0x00000002;
    private const uint NIF_MESSAGE = 0x00000001;
    private const uint NIF_ICON = 0x00000002;
    private const uint NIF_TIP = 0x00000004;

    //Constants for window messages
    private const uint WM_LBUTTONDOWN = 0x0201; //Left-click message
    private const uint WM_RBUTTONDOWN = 0x0204; //Right-click message
    private const uint WM_USER = 0x0400; //Custom user message

    //Constants for icon loading
    private const uint IMAGE_ICON = 1;
    private const uint LR_LOADFROMFILE = 0x00000010;

    //Icon data and window procedure delegate
    private NOTIFYICONDATA notifyIconData;
    private IntPtr hIcon;
    private WndProcDelegate newWndProc;
    private IntPtr oldWndProc;

    //Path to the icon file
    [Header("Settings")]
    [SerializeField] private string hoverTooltipText = "Desktop Calendar\n \nPress to open settings menu";

    //Delegate for handling window messages
    private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    private void Start()
    {
        string iconPath = Application.persistentDataPath + "/icon.ico";
        
        //Save ico file to persistentDataPath
        if (!File.Exists(iconPath))
        {
            TextAsset icoData = Resources.Load<TextAsset>("icon");
            Debug.Log(icoData);
            Debug.Log(icoData.bytes);
            File.WriteAllBytes(iconPath, icoData.bytes);
        }
        
        //Create the NotifyIconData
        notifyIconData = new NOTIFYICONDATA();
        notifyIconData.cbSize = (uint)Marshal.SizeOf(typeof(NOTIFYICONDATA));
        notifyIconData.hWnd = hWndUtil.Get();
        notifyIconData.uID = 1;
        notifyIconData.uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP;
        notifyIconData.szTip = hoverTooltipText;
        notifyIconData.uCallbackMessage = WM_USER + 1;

        //Load icon from file using WinAPI
        hIcon = LoadIconFromFile(iconPath);
        notifyIconData.hIcon = hIcon; //Set the icon

        //Add the icon to the system tray
        Shell_NotifyIcon(NIM_ADD, ref notifyIconData);

        //Set up WndProc to intercept messages
        newWndProc = new WndProcDelegate(CustomWndProc);
        oldWndProc = SetWindowLongPtr(hWndUtil.Get(), -4, Marshal.GetFunctionPointerForDelegate(newWndProc));
    }

    //Remove the icon from the system tray
    private void OnApplicationQuit()
    {
        Shell_NotifyIcon(NIM_DELETE, ref notifyIconData);
        DestroyIcon(hIcon);
        SetWindowLongPtr(hWndUtil.Get(), -4, oldWndProc);
    }

    //Load icon from file using WinAPI
    private IntPtr LoadIconFromFile(string path)
    {
        return LoadImage(IntPtr.Zero, path, IMAGE_ICON, 0, 0, LR_LOADFROMFILE);
    }

    //Handle tray icon interactions
    private IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        //Check for tray icon click events
        if (msg == WM_USER + 1)
        {
            if ((uint)lParam == WM_LBUTTONDOWN || (uint)lParam == WM_RBUTTONDOWN)
            {
                UIManager.Instance.ShowSettingsMenu();
            }
        }

        //Call the original WndProc for other messages
        return CallWindowProc(oldWndProc, hWnd, msg, wParam, lParam);
    }

    //Define the NOTIFYICONDATA structure
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct NOTIFYICONDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uID;
        public uint uFlags;
        public uint uCallbackMessage;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;
    }
}
