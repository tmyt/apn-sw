using MvnoSwitcher.MobileConfig;
using System;
using UIKit;

namespace MvnoSwitcher
{
    public partial class MSEditPageViewController : UITableViewController
    {
        public MSEditPageViewController (IntPtr handle) : base (handle)
        {
        }

        public bool IsNew { get; internal set; }
        public ConfigGenerator Config { get; internal set; }
        public int Index { get; internal set; }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NameTextField.Text = Config.Name;
            ApnTextField.Text = Config.Apn;
            UsernameTextField.Text = Config.Username;
            PasswordText.Text = Config.Password;
        }

        partial void SaveTapped(Foundation.NSObject sender)
        {
            Config.Name = NameTextField.Text;
            Config.Apn = ApnTextField.Text;
            Config.Username = UsernameTextField.Text;
            Config.Password = PasswordText.Text;

            int authIndex = (int)AuthenticationTypeSegmentedControl.SelectedSegment;
            Config.AuthenticationType = (authIndex == 1) ? "CHAP" : "PAP";


            var appConfig = (UIApplication.SharedApplication.Delegate as AppDelegate).AppConfig;
            if (IsNew)
            {
                appConfig.Apns.Add(Config);
            }
            appConfig.Save();
        }
    }
}