using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.About
{
    partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
            var pos = this.PointToScreen(lblTitle.Location);
            pos = pbLogo.PointToClient(pos);
            lblTitle.Parent = pbLogo;
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = AssemblyProduct;
            lblTitle.BackColor = Color.Transparent;

            pos = this.PointToScreen(lblVersion.Location);
            pos = pbLogo.PointToClient(pos);
            lblVersion.Parent = pbLogo;
            lblVersion.ForeColor = Color.White;
            lblVersion.Text = String.Format("Version {0}", AssemblyVersion);
            lblVersion.BackColor = Color.Transparent;

            this.lblDistLic.Text = Properties.Resources.AboutDistMsg;

            this.Text = String.Format("About {0}", AssemblyProduct);
            this.labelCopyright.Text = AssemblyCopyright;
            this.textBoxDescription.Text = AssemblyDescription;

            LinkLabel.Link DonateLink = new LinkLabel.Link();
            DonateLink.LinkData = AppCommon.Instance.UrlDonate;
            llblDonate.Links.Add(DonateLink);

            LinkLabel.Link LicenseLink = new LinkLabel.Link();
            LicenseLink.LinkData = @"https://opensource.org/licenses/GPL-3.0";
            llblLicense.Links.Add(LicenseLink);

        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
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
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }


        #endregion

        private void llblDonate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Send the URL to the operating system.
            Process.Start(e.Link.LinkData as string);
        }

        private void llblLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Send the URL to the operating system.
            Process.Start(e.Link.LinkData as string);
        }
    }
}
