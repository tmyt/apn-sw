using System;
using Foundation;

namespace MvnoSwitcher.Extensions
{
    public static class NSObjectExtension
    {
        public class Context : NSObject
        {
            public object Value { get; set; }
        }

        public static object Unwrap(this NSObject obj)
        {
            if(obj is Context)
            {
                return ((Context)obj).Value;
            }
            return null;
        }
    }
}
