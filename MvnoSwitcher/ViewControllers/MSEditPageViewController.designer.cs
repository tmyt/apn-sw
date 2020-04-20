// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MvnoSwitcher
{
    [Register ("MSEditPageViewController")]
    partial class MSEditPageViewController
    {
        [Outlet]
        UIKit.UITextField ApnTextField { get; set; }


        [Outlet]
        UIKit.UISegmentedControl AuthenticationTypeSegmentedControl { get; set; }


        [Outlet]
        UIKit.UITextField NameTextField { get; set; }


        [Outlet]
        UIKit.UITextField PasswordText { get; set; }


        [Outlet]
        UIKit.UITextField UsernameTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell AuthenticationTypeCell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell ProtocolTypeCell { get; set; }


        [Action ("SaveTapped:")]
        partial void SaveTapped (Foundation.NSObject sender);

        [Action ("NameTextFieldChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NameTextFieldChanged (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (AuthenticationTypeCell != null) {
                AuthenticationTypeCell.Dispose ();
                AuthenticationTypeCell = null;
            }

            if (ProtocolTypeCell != null) {
                ProtocolTypeCell.Dispose ();
                ProtocolTypeCell = null;
            }
        }
    }
}