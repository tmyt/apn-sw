using Foundation;
using MvnoSwitcher.MobileConfig;
using MvnoSwitcher.Extensions;
using System;
using System.Collections.Generic;
using UIKit;

namespace MvnoSwitcher
{
    public partial class MSEditPageViewController : UITableViewController
    {
        private static readonly List<string> AuthTypes = new List<string> { "PAP", "CHAP" };
        private static readonly List<string> Masks = new List<string> { "IPv4", "IPv6", "IPv4v6" };

        private UIBarButtonItem _cancelButton;
        private string _authType;
        private int _mask;


        public bool IsNew { get; internal set; }
        public ConfigGenerator Config { get; internal set; }
        public int Index { get; internal set; }

        public MSEditPageViewController(IntPtr handle) : base(handle)
        {
            _cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (s, e) =>
            {
                Dismiss();
            });
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _authType = Config.AuthenticationType;
            _mask = Config.ProtocolMask - 1;
            if (IsNew)
            {
                NavigationItem.LeftBarButtonItem = _cancelButton;
                var vc = ((UINavigationController)PresentingViewController).TopViewController;
                var @delegate = new AdaptivePresentationControllerDelegate((MSMainPageViewController)vc);
                NavigationController.PresentationController.Delegate = @delegate;
            }
            // load values from config
            NameTextField.Text = Config.Name;
            ApnTextField.Text = Config.Apn;
            UsernameTextField.Text = Config.Username;
            PasswordText.Text = Config.Password;
            Title = string.IsNullOrEmpty(Config.Name) ? "Config" : Config.Name;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            AuthenticationTypeCell.DetailTextLabel.Text = _authType;
            ProtocolTypeCell.DetailTextLabel.Text = Masks[_mask];
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            segue.HandlePrepare(sender);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            TableView.DeselectRow(indexPath, true);
            switch (indexPath.Row)
            {
                case 4:// Authentication
                    this.PerformSegue<MSAuthenticationTypeViewController>("EditAuthentication", vc =>
                    {
                        vc.AuthType = _authType;
                        vc.Update = (x) => _authType = x;
                    });
                    break;
                case 5:// Protocol
                    this.PerformSegue<MSProtocolTypeViewController>("EditProtocol", vc =>
                    {
                        vc.Type = _mask;
                        vc.Update = (x) => _mask = x;
                    });
                    break;
            }
        }

        partial void SaveTapped(Foundation.NSObject sender)
        {
            Config.Name = NameTextField.Text;
            Config.Apn = ApnTextField.Text;
            Config.Username = UsernameTextField.Text;
            Config.Password = PasswordText.Text;
            Config.AuthenticationType = _authType;
            Config.ProtocolMask = _mask + 1;

            var appConfig = (UIApplication.SharedApplication.Delegate as AppDelegate).AppConfig;
            if (IsNew)
            {
                appConfig.AddConfig(Config);
                Dismiss();
            }
            else
            {
                appConfig.Save();
                NavigationController.PopViewController(true);
            }
        }

        partial void NameTextFieldChanged(UITextField sender)
        {
            var text = NameTextField.Text;
            Title = string.IsNullOrEmpty(text) ? "Config" : text;
        }

        private void Dismiss()
        {
            var controller = NavigationController.PresentationController;
            var @delegate = controller.Delegate;
            DismissViewController(true, () => {
                @delegate.DidDismiss(controller);
            });

        }

        public class AdaptivePresentationControllerDelegate : UIAdaptivePresentationControllerDelegate
        {
            private MSMainPageViewController _viewController;

            public AdaptivePresentationControllerDelegate(MSMainPageViewController vc)
            {
                this._viewController = vc;
            }

            public override void DidDismiss(UIPresentationController presentationController)
            {
                _viewController.TableView.ReloadData();
            }
        }
    }
}