using Foundation;
using MvnoSwitcher.MobileConfig;
using System;
using UIKit;

namespace MvnoSwitcher
{
    public partial class UIMainPageViewController : UITableViewController
    {
        const string TableCell = "TableCell";

        private UIBarButtonItem _add;
        private UIBarButtonItem _done;
        private UIBarButtonItem _edit;

        public UIMainPageViewController(IntPtr handle) : base(handle)
        {
            _add = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) =>
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var viewController = (MSEditPageViewController)storyboard.InstantiateViewController("EditPage");
                viewController.IsNew = true;
                viewController.Config = new ConfigGenerator();
                viewController.Index = -1;
                NavigationController.PushViewController(viewController, true);
            });
            _done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) =>
            {
                TableView.SetEditing(false, true);
                NavigationItem.LeftBarButtonItem = null;
                NavigationItem.RightBarButtonItem = _edit;
            });
            _edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) =>
            {
                if (TableView.Editing)
                {
                    TableView.SetEditing(false, true);
                }
                TableView.SetEditing(true, true);
                NavigationItem.LeftBarButtonItem = _add;
                NavigationItem.RightBarButtonItem = _done;
            });
            NavigationItem.LeftBarButtonItem = null;
            NavigationItem.RightBarButtonItem = _edit;
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

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    AppDelegate.Current.AppConfig.Apns.RemoveAt(indexPath.Row);
                    AppDelegate.Current.AppConfig.Save();
                    tableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
                    break;
                default:
                    break;
            }
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        {
            return "Delete";
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            if (tableView.Editing)
            {

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

        public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var viewController = (MSEditPageViewController)storyboard.InstantiateViewController("EditPage");
            viewController.IsNew = false;
            viewController.Config = AppDelegate.Current.AppConfig.Apns[indexPath.Row];
            viewController.Index = indexPath.Row;
            NavigationController.PushViewController(viewController, true);
        }
    }
}