using System;
using System.Reflection;
using System.Windows.Forms;

namespace RestaurantApp.Voting.DesktopApp
{
    partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            this.Text = String.Format("Sobre {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Versão  {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                var title = GetAttribute<AssemblyTitleAttribute>(_ => _.Title);

                return string.IsNullOrWhiteSpace(title)
                    ? System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase)
                    : title;
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                return GetAttribute<AssemblyDescriptionAttribute>(_ => _.Description);
            }
        }

        public string AssemblyProduct
        {
            get
            {
                return GetAttribute<AssemblyProductAttribute>(_ => _.Product);
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                return GetAttribute<AssemblyCopyrightAttribute>(_ => _.Copyright);
            }
        }

        public string AssemblyCompany
        {
            get
            {
                return GetAttribute<AssemblyCompanyAttribute>(_ => _.Company);
            }
        }

        public static string GetAttribute<T>(Func<T, string> selector)
        {

            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(T), false);

            if (attributes.Length == 0)
                return "";

            return selector((T)attributes[0]);
        }
        #endregion
    }
}
