using System;
using Foundation;
using UIKit;

namespace MvnoSwitcher.Extensions
{
    public static class UIStoryboardSegueExtension
    {
       public static void HandlePrepare(this UIStoryboardSegue segue, NSObject prepareHandler)
        {
            var prepare = (Action<object>)prepareHandler.Unwrap();
            prepare(segue.DestinationViewController);
        }
    }
}
