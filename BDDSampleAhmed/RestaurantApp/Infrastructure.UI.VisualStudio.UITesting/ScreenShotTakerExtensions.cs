using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UITesting;

namespace Infrastructure.UI.VisualStudio.UITesting
{
    public static class ScreenShotTakerExtensions
    {
        /// <summary> Captures a specific window (or control) and save it to a specified file.  </summary>
        /// <param name="control"><see cref="Control"/> to capture</param>
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
        /// <seealso cref="ScreenShotTaker"/>
        public static void SaveImage(this UITestControl control, string filename = null, ImageFormat format = null)
        {
            Capture(control).SaveImage(filename: filename, format: format);
        }

        /// <summary> Captures a .NET UITestControl, etc. </summary>
        /// <param name="control">Object to capture</param>
        /// <returns> Bitmap of control's area </returns>
        public static Bitmap Capture(this UITestControl control)
        {
            return control.Exists
                ? new Bitmap(control.CaptureImage())
                : null;
        }
    }
}