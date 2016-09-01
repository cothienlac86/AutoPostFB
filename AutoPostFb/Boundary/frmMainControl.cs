namespace Boundary.AutoPostFb
{
    using System;
    using System.Windows.Forms;
    using Facebook;
    using System.Xml.Linq;
    using Newtonsoft.Json.Linq;

    public partial class frmMain : Form
    {
        private const string AppId = "1492851824272497";
        private const string ExtendedPermissions = "email, public_profile, user_friends";
        private string _accessToken;
        //private FacebookClient _fbClient = null;

        public frmMain()
        {
            InitializeComponent();
            HandleLoginButton(true);
            HandleLogoutButton(false);
        }

        private void HandleLoginButton(bool value)
        {
            btnFacebookLogin.BringToFront();
            btnFacebookLogin.Visible = value;
        }
        private void HandleLogoutButton(bool value)
        {
            btnLogout.SendToBack();
            btnLogout.Visible = value;
        }

        private void lnkTutorial_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //System.Diagnostics.Process.Start(lnkTutorial.Text);
        }

        private void btnFacebookLogin_Click(object sender, EventArgs e)
        {
            var fbLoginDialog = new frmLoginDlg(AppId, ExtendedPermissions);
            fbLoginDialog.ShowDialog();
            DisplayAppropriateMessage(fbLoginDialog.FacebookOAuthResult);
        }

        private void DisplayAppropriateMessage(FacebookOAuthResult facebookOAuthResult)
        {
            if (facebookOAuthResult != null)
            {
                if (facebookOAuthResult.IsSuccess)
                {
                    _accessToken = facebookOAuthResult.AccessToken;
                    var _fbClient = new FacebookClient(facebookOAuthResult.AccessToken);

                    dynamic result = _fbClient.Get("/me");
                    var name = string.IsNullOrEmpty(result.name) ? "User not found...!" : result.name;
                    lbInfosHolder.Text = name;
                    HandleLoginButton(false);
                    HandleLogoutButton(true);
                }
                else
                {
                    MessageBox.Show(facebookOAuthResult.ErrorDescription);
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var webBrowser = new WebBrowser();
            var fb = new FacebookClient();
            var logouUrl = fb.GetLogoutUrl(new { access_token = _accessToken, next = "https://www.facebook.com/connect/login_success.html" });
            webBrowser.Navigate(logouUrl);
            btnLogout.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken)) return;
                //ThanhTau86
                var _fbClient = new FacebookClient(_accessToken);
                dynamic result = _fbClient.Get("/1492851824272497?fields=group");
                var name = string.IsNullOrEmpty(result.name) ? "User not found...!" : result.name;
            }
            catch
            {
                throw;
            }
        }
    }
}
