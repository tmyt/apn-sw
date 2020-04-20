using Foundation;
using MvnoSwitcher.Extensions;
using System;
using UIKit;

namespace MvnoSwitcher
{
    public partial class MSAuthenticationTypeViewController : UITableViewController
    {
        private static readonly string[] AuthTypes = { "PAP", "CHAP" };

        public string AuthType { get; set; }
        public Action<string> Update { get; set; }

        public MSAuthenticationTypeViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UpdateSelection();
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            TableView.DeselectRow(indexPath, true);
            AuthType = AuthTypes[indexPath.Row];
            UpdateSelection();
            Update(AuthType);
        }

        private void UpdateSelection()
        {
            var current = Array.IndexOf(AuthTypes, AuthType);
            for (var i = 0; i < 2; ++i)
            {
                var cell = GetCell(TableView, NSIndexPath.FromRowSection(i, 0));
                cell.SetChecked(i == current);
            }
        }

        public override void Unwind(UIStoryboardSegue unwindSegue, UIViewController subsequentVC)
        {
            base.Unwind(unwindSegue, subsequentVC);
        }
    }
}