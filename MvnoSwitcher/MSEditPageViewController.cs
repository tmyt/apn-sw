using MvnoSwitcher.MobileConfig;
using System;
using System.Collections.Generic;
using UIKit;

namespace MvnoSwitcher
{
    public partial class MSEditPageViewController : UITableViewController
    {
        private static readonly List<string> AuthTypes = new List<string> { "PAP", "CHAP" };
        private static readonly List<int> Masks = new List<int>{ 1, 2, 3 };

        public MSEditPageViewController(IntPtr handle) : base(handle)
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
            AuthenticationTypeSegmentedControl.SelectedSegment = AuthTypes.IndexOf(Config.AuthenticationType);
            ProtocolTypeSegmentedControl.SelectedSegment = Masks.IndexOf(Config.ProtocolMask);
            Title = string.IsNullOrEmpty(Config.Name) ? "Config" : Config.Name;
        }

        partial void SaveTapped(Foundation.NSObject sender)
        {
            Config.Name = NameTextField.Text;
            Config.Apn = ApnTextField.Text;
            Config.Username = UsernameTextField.Text;
            Config.Password = PasswordText.Text;
            Config.AuthenticationType = AuthTypes[(int)AuthenticationTypeSegmentedControl.SelectedSegment]; 
            Config.ProtocolMask = Masks[(int)ProtocolTypeSegmentedControl.SelectedSegment];

            var appConfig = (UIApplication.SharedApplication.Delegate as AppDelegate).AppConfig;
            if (IsNew)
            {
                appConfig.Apns.Add(Config);
            }
            appConfig.Save();
            NavigationController.PopViewController(true);
        }

        partial void NameTextFieldChanged(UITextField sender)
        {
            var text = NameTextField.Text;
            Title = string.IsNullOrEmpty(text) ? "Config" : text;
        }
    }
}