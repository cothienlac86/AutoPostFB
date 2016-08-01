
namespace FirstFacebookApplication
{
    using System;
    using System.Windows.Forms;
    using Facebook;

    public partial class frmMain : Form
    {
        private const string AppId = "1571870129775473";
        private const string ExtendedPermissions = "user_friends, public_profile, user_managed_groups";
        private string _accessToken;

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
            var fbLoginDialog = new FacebookLoginDialog(AppId, ExtendedPermissions);
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
                    var fb = new FacebookClient(facebookOAuthResult.AccessToken);

                    dynamic result = fb.Get("/me");
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
    }
}
