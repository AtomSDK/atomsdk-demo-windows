using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atom.VPN.Demo.Extensions
{
    public static class Extensions
    {
        public static bool Equals<T>(this Type type)
        {
            return typeof(T).Equals(type);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> value)
        {
            if (value != null)
                return new ObservableCollection<T>(value);
            return null;
        }
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> value)
        {
            if (value != null)
                return new ObservableCollection<T>(value);
            return null;
        }
    }
}
