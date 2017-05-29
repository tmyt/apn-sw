using MvnoSwitcher.MobileConfig;
using System;
using UIKit;

namespace MvnoSwitcher
{
    public partial class MSEditPageViewController : UITableViewController
    {
        public MSEditPageViewController (IntPtr handle) : base (handle)
        {
        }

        public bool IsNew { get; internal set; }
        public ConfigGenerator Config { get; internal set; }
        public int Index { get; internal set; }

        
    }
}