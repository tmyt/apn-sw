using Foundation;

namespace MvnoSwitcher.Extensions
{
    public static class ObjectExtension
    {
        public static NSObject Wrap(this object source)
        {
            return new NSObjectExtension.Context { Value = source };
        }
    }
}
