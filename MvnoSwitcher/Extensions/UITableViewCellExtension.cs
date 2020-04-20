using UIKit;

namespace MvnoSwitcher.Extensions
{
    public static class UITableViewCellExtension
    {
        public static void SetChecked(this UITableViewCell cell, bool @checked)
        {
            cell.Accessory = @checked ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
        }
    }
}
