// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
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
		
		void ReleaseDesignerOutlets ()
		{
			if (ApnTextField != null) {
				ApnTextField.Dispose ();
				ApnTextField = null;
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

			if (AuthenticationTypeSegmentedControl != null) {
				AuthenticationTypeSegmentedControl.Dispose ();
				AuthenticationTypeSegmentedControl = null;
			}
		}
	}
}
