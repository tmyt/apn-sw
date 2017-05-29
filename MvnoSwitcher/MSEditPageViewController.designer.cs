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


        [Action ("SaveTapped:")]
        partial void SaveTapped (Foundation.NSObject sender);

        [Action ("NameTextFieldChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NameTextFieldChanged (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (ApnTextField != null) {
                ApnTextField.Dispose ();
                ApnTextField = null;
            }

            if (AuthenticationTypeSegmentedControl != null) {
                AuthenticationTypeSegmentedControl.Dispose ();
                AuthenticationTypeSegmentedControl = null;
            }

            if (NameTextField != null) {
                NameTextField.Dispose ();
                NameTextField = null;
            }

            if (PasswordText != null) {
                PasswordText.Dispose ();
                PasswordText = null;
            }

            if (UsernameTextField != null) {
                UsernameTextField.Dispose ();
                UsernameTextField = null;
            }
        }
    }
}