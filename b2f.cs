//
// Bring application to the front of the Z-order in Windows.
// To run the application, pass the name of the window title as the parameter:
//
//    b2f "Visual Studio Code"
//
// To compile the application, no special parameters are needed:
//
//    csc b2f.cs
//
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BringToFront
{
    public class Program
    {
        [DllImport("user32")]
        public static extern IntPtr GetWindow(IntPtr hwnd, int wCmd);
        [DllImport("user32")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, Int32 cch);
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hwnd);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr handle, int nCmdShow);

        private const int GWL_ID = (-12);
        private const int GW_HWNDNEXT = 2;
        private const int GW_CHILD = 5;
        private const int SW_RESTORE = 9;

        private static IntPtr FindWindow(string windowName)
        {
            IntPtr hwnd = GetWindow(GetDesktopWindow(), GW_CHILD);

            while(hwnd != IntPtr.Zero)
            {
                StringBuilder text = new StringBuilder(255);
                int rtn = GetWindowText(hwnd, text, 255);
                string windowText = text.ToString();
                windowText = windowText.Substring(0, rtn);

                if(windowText.Length > 0 && windowText.Contains(windowName))
                {
                    return hwnd;
                }

                hwnd = GetWindow(hwnd, GW_HWNDNEXT);
            }
            return IntPtr.Zero;
        }

        public static void bringToFront(string title)
        {
            // Get a handle to the application.
            IntPtr handle = FindWindow(title);

            // Verify that it is a running process.
            if(IntPtr.Zero == handle)
            {
                Console.WriteLine("Could not find app.");
                return;
            }

            if(IsIconic(handle))
            {
                ShowWindow(handle, SW_RESTORE);
            }
            SetForegroundWindow(handle);
        }

        public static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                bringToFront(args[0]);
            }
            else
            {
                Console.WriteLine("Specify app window title");
            }
        }
    }
}
