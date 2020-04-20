using Foundation;
using MvnoSwitcher.Extensions;
using MvnoSwitcher.MobileConfig;
using System;
using System.Collections.Generic;
using UIKit;

namespace MvnoSwitcher
{
    public partial class MSMainPageViewController : UITableViewController
    {
        const string TableCell = "TableCell";

#if false
        private bool _isManaged = true;
#else
        private bool _isManaged = false;
#endif
        private List<ConfigGenerator> _managedConfigs = new List<ConfigGenerator>{
            new ConfigGenerator
            {
                Name = "Internal APN",
                Apn = "internal.example.com",
            },
        };

        private bool _editMode;
        private UIBarButtonItem _addButton;
        private UIBarButtonItem _doneButton;
        private UIBarButtonItem _editButton;

        public MSMainPageViewController(IntPtr handle) : base(handle)
        {
            _addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) =>
            {
                CreateNew(); // new
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

        public override nint NumberOfSections(UITableView tableView)
        {
            return _isManaged ? 2 : 1;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            if (_isManaged && section == 0) return _managedConfigs.Count;
            return AppDelegate.Current.AppConfig.Apns.Count;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            if (!_isManaged) return null;
            return section == 0 ? "組織管理" : "ユーザー定義";
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(TableCell);
            var apn = GetConfigForIndexPath(indexPath);
            cell.TextLabel.Text = apn.Name;
            cell.DetailTextLabel.Text = string.IsNullOrEmpty(apn.Apn) ? "(none)" : apn.Apn;
            return cell;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (_isManaged && indexPath.Section == 0) return false;
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
                if (_isManaged && indexPath.Section == 0) return;
                // open details
                OpenDetails(indexPath.Row);
            }
            else
            {
                // open safari
                var apn = GetConfigForIndexPath(indexPath);
                var argString = apn.GetQueryString(AppDelegate.Current.HttpServer.Token);
                var url = new NSUrl($"http://127.0.0.1:18080/ondemand?{argString}");
                UIApplication.SharedApplication.OpenUrl(url);
            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            segue.HandlePrepare(sender);
        }

        private void OpenDetails(int index)
        {
            this.PerformSegue<MSEditPageViewController>("EditAPN", vc =>
            {
                vc.IsNew = false;
                vc.Config = AppDelegate.Current.AppConfig.Apns[index];
                vc.Index = index;
            });
        }

        private void CreateNew()
        {
            this.PerformSegue<UINavigationController>("NewAPN", nav =>
            {
                var vc = (MSEditPageViewController)nav.TopViewController;
                vc.IsNew = true;
                vc.Config = new ConfigGenerator();
                vc.Index = -1;
            });
        }

        private ConfigGenerator GetConfigForIndexPath(NSIndexPath indexPath)
        {
            return GetConfigsForSection(indexPath.Section)[indexPath.Row];
        }

        private List<ConfigGenerator> GetConfigsForSection(int section)
        {
            var apns = AppDelegate.Current.AppConfig.Apns;
            if (!_isManaged) return apns;
            return section == 0 ? _managedConfigs : apns;
        }
    }
}
