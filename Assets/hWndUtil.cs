using System;
using System.Runtime.InteropServices;

public static class hWndUtil
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    
    private static IntPtr hWndPtr;
    public static IntPtr Get() 
    {
        if (hWndPtr == IntPtr.Zero)
        {
            hWndPtr = GetActiveWindow();
        }

        return hWndPtr;
    }
}
