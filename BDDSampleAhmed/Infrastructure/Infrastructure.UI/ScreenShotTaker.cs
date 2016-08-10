using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Infrastructure.UI
{
    public enum CaptureMode
    {
        Screen, Window
    }

    public static class ScreenShotTaker
    {

        public const string TimestampVarArg = "Timestamp";
        public const string CurrentUserVarArg = "CurrentUser";
        public const string CurrentUserNameVarArg = "CurrentUserName";
        public const string MachineNameVarArg = "MachineName";
        public const string CurrentUserDomainNameVarArg = "CurrentUserDomainName";

        private static Dictionary<string, Func<string>> PlaceHolderMappings { get; set; }

        static ScreenShotTaker()
        {
            PlaceHolderMappings = new Dictionary<string, Func<string>>{
                {TimestampVarArg            , () => DateTime.Now.ToString("yyyy-MM-dd@hh.mm.ss")},
                {MachineNameVarArg          , () => Environment.MachineName},
                {CurrentUserNameVarArg      , () => Environment.UserName},
                {CurrentUserDomainNameVarArg, () => Environment.UserDomainName},
                {CurrentUserVarArg          , () => Environment.UserDomainName + "@" + Environment.UserName}
            };
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();


        /// <summary> Captures an Active Window, Desktop, Window or Control by hWnd or .NET Contro/Form and save it to a specified file.  </summary>
        /// <param name="filename">Filename.
        /// <param name="filename">Filename.
        /// <para>* If extension is omitted, it's calculated from the type of file</para>
        /// <para>* If path is omitted, defaults to %TEMP%</para>
        /// <para>* Use %Timestamp% to put a timestamp in the filename</para></param>
        /// <para>* Use %CurrentUser% to put the {%CurrentUserDomainName%}@{%CurrentUserName%} in the filename</para></param>
        /// <para>* Use %CurrentUserName% to put the current user name in the filename</para></param>
        /// <para>* Use %CurrentUserDomainName% to put the current user domain name in the filename</para></param>
        /// <para>* Use %MachineName% to put the machine name in the filename</para></param>
        /// <param name="mode">Optional. The default value is CaptureMode.Window.</param>
        /// <param name="format">Optional file save mode.  Default is PNG</param>
        public static void CaptureAndSave(string filename, CaptureMode mode = CaptureMode.Window, ImageFormat format = null)
        {
            SaveImage(Capture(mode), filename, format);
        }

        /// <summary> Captures a specific window (or control) and save it to a specified file.  </summary>
        /// <param name="handle">hWnd (handle) of the window to capture</param>
        /// <param name="filename">Filename.
        /// <param name="filename">Filename.
        /// <para>* If extension is omitted, it's calculated from the type of file</para>
        /// <para>* If path is omitted, defaults to %TEMP%</para>
        /// <para>* Use %Timestamp% to put a timestamp in the filename</para></param>
        /// <para>* Use %CurrentUser% to put the {%CurrentUserDomainName%}@{%CurrentUserName%} in the filename</para></param>
        /// <para>* Use %CurrentUserName% to put the current user name in the filename</para></param>
        /// <para>* Use %CurrentUserDomainName% to put the current user domain name in the filename</para></param>
        /// <para>* Use %MachineName% to put the machine name in the filename</para></param>
        /// <param name="format">Optional file save mode.  Default is PNG</param>
        public static void SaveImage(this IntPtr windowHandle, string filename = null, ImageFormat format = null)
        {
            SaveImage(Capture(windowHandle), filename, format);
        }

        /// <summary> Captures the active window (default) or the desktop and return it as a bitmap </summary>
        /// <param name="mode">Optional. The default value is CaptureMode.Window.</param>
        public static Bitmap Capture(CaptureMode mode = CaptureMode.Window)
        {
            return Capture(mode == CaptureMode.Screen ? GetDesktopWindow() : GetForegroundWindow());
        }

        /// <summary> Capture a specific window and return it as a bitmap </summary>
        /// <param name="handle">hWnd (handle) of the window to capture</param>
        public static Bitmap Capture(this IntPtr handle)
        {
            Rectangle bounds;
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

            var result = new Bitmap(bounds.Width, bounds.Height);
            using (var g = Graphics.FromImage(result))
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);

            return result;
        }


        /// <summary> Saves an image to a specific file </summary>
        /// <param name="image">Image to save.  Usually a BitMap, but can be any
        /// Image.</param>
        /// <param name="filename">Filename.
        /// <para>* If extension is omitted, it's calculated from the type of file</para>
        /// <para>* If path is omitted, defaults to %TEMP%</para>
        /// <para>* Use %Timestamp% to put a timestamp in the filename</para></param>
        /// <para>* Use %CurrentUser% to put the {%CurrentUserDomainName%}@{%CurrentUserName%} in the filename</para></param>
        /// <para>* Use %CurrentUserName% to put the current user name in the filename</para></param>
        /// <para>* Use %CurrentUserDomainName% to put the current user domain name in the filename</para></param>
        /// <para>* Use %MachineName% to put the machine name in the filename</para></param>
        /// <param name="format">Optional file save mode.  Default is PNG</param>
        public static string SaveImage(this Image image, string filename = null, ImageFormat format = null)
        {
            format = format ?? ImageFormat.Png;

            if (string.IsNullOrWhiteSpace(filename))
                filename = Path.GetTempFileName();
            else
            {
                var file = new FileInfo(filename);
                if (string.IsNullOrWhiteSpace(file.Extension))
                    filename = filename.Trim() + "." + format.ToString().ToLower();

                if (!filename.Contains(@"\"))
                    filename = Path.Combine(Environment.GetEnvironmentVariable("TEMP") ?? @"C:\Temp", filename);
            }

            image.Save(FormatString(filename), format);

            return filename;
        }

        /// <summary>
        /// Adds a new placeholder.
        /// </summary>
        /// <param name="name">Case-sensitive name of the place holder</param>
        /// <param name="valueGetter">The factory method for getting values for the placeholder</param>
        public static void AddPlaceHolderMapping(string name, Func<string> valueGetter)
        {
            PlaceHolderMappings[name.Trim()] = valueGetter;
        }

        /// <summary>
        /// Removes a placeholder mapping.
        /// </summary>
        /// <param name="name">Case-sensitive name of the place holder</param>
        public static void RemovePlaceHolderMapping(string name)
        {
            PlaceHolderMappings.Remove(name.Trim());
        }

        /// <summary>
        /// removes all placeholder mappings.
        /// </summary>
        public static void ClearPlaceHolderMappings()
        {
            PlaceHolderMappings.Clear();
        }

        /// <summary>
        /// Formats the string replacing all placeholders with their values.
        /// </summary>
        /// <param name="string"><see cref="string"/> to format</param>
        /// <returns>a formatted <see cref="string"/></returns>
        private static string FormatString(string @string)
        {
            foreach (var mapping in PlaceHolderMappings)
                @string = @string.Replace("%" + mapping.Key + "%", mapping.Value());

            return @string;
        }
    }
}