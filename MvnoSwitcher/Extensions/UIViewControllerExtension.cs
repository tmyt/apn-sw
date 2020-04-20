using System;
using UIKit;

namespace MvnoSwitcher.Extensions
{
    public static class UIViewControllerExtension
    {
       public static void PerformSegue<T>(this UIViewController viewController, string identifier, Action<T> prepare)
        {
            Action<object> wrapper = obj => prepare((T)obj);
            viewController.PerformSegue(identifier, wrapper.Wrap());
        }
    }
}
