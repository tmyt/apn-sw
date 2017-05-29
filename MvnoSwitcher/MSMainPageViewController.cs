using Foundation;
using MvnoSwitcher.MobileConfig;
using System;
using UIKit;

namespace MvnoSwitcher
{
    public partial class MSMainPageViewController : UITableViewController
    {
        const string TableCell = "TableCell";

        private bool _editMode;
        private UIBarButtonItem _addButton;
        private UIBarButtonItem _doneButton;
        private UIBarButtonItem _editButton;

        public MSMainPageViewController(IntPtr handle) : base(handle)
        {
            _addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) =>
            {
                OpenDetails(-1); // new
            });
            _doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) =>
            {
                TableView.SetEditing(false, true);
                NavigationItem.LeftBarButtonItem = _addButton;
                NavigationItem.RightBarButtonItem = _editButton;
                _editMode = false;
            });
            _editButton = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) =>
            {
                if (TableView.Editing)
                {
                    TableView.SetEditing(false, true);
                }
                TableView.SetEditing(true, true);
                NavigationItem.LeftBarButtonItem = null;
                NavigationItem.RightBarButtonItem = _doneButton;
                _editMode = true;
            });
            NavigationItem.LeftBarButtonItem = _addButton;
            NavigationItem.RightBarButtonItem = _editButton;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            TableView.ReloadData();
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return AppDelegate.Current.AppConfig.Apns.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(TableCell);
            var apn = AppDelegate.Current.AppConfig.Apns[indexPath.Row];
            cell.TextLabel.Text = apn.Name;
            cell.DetailTextLabel.Text = apn.Apn;
            return cell;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var del = UITableViewRowAction.Create(UITableViewRowActionStyle.Destructive, "Delete", (_, i) =>
            {
                AppDelegate.Current.AppConfig.Apns.RemoveAt(i.Row);
                AppDelegate.Current.AppConfig.Save();
                tableView.DeleteRows(new[] { i }, UITableViewRowAnimation.Left);
            });
            var edit = UITableViewRowAction.Create(UITableViewRowActionStyle.Normal, "Edit", (_, i) =>
            {
                OpenDetails(i.Row);
            });
            return _editMode
                ? new[] { del }
                : new[] { del, edit };
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            if (tableView.Editing)
            {
                // open details
                OpenDetails(indexPath.Row);
            }
            else
            {
                // open safari
                var apn = AppDelegate.Current.AppConfig.Apns[indexPath.Row];
                var argString = apn.GetQueryString(AppDelegate.Current.HttpServer.Token);
                var url = new NSUrl($"http://127.0.0.1:18080/ondemand?{argString}");
                UIApplication.SharedApplication.OpenUrl(url);
            }
        }

        private void OpenDetails(int index)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var viewController = (MSEditPageViewController)storyboard.InstantiateViewController("EditPage");
            viewController.IsNew = index < 0;
            viewController.Config = index < 0 ? new ConfigGenerator() : AppDelegate.Current.AppConfig.Apns[index];
            viewController.Index = index;
            NavigationController.PushViewController(viewController, true);
        }
    }
}