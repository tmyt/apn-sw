using Foundation;
using MvnoSwitcher.Extensions;
using System;
using UIKit;

namespace MvnoSwitcher
{
    public partial class MSProtocolTypeViewController : UITableViewController
    {
        public int Type { get; set; }
        public Action<int> Update { get; set; }

        public MSProtocolTypeViewController(IntPtr handle) : base(handle)
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
            Type = indexPath.Row;
            UpdateSelection();
            Update(Type);
        }

        private void UpdateSelection()
        {
            for (var i = 0; i < 3; ++i)
            {
                var cell = GetCell(TableView, NSIndexPath.FromRowSection(i, 0));
                cell.SetChecked(i == Type);
            }
        }
    }
}