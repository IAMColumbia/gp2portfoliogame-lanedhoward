using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInputReaderLibrary
{
    public class HostPackageComparer : IEqualityComparer<IHostPackage>
    {
        bool IEqualityComparer<IHostPackage>.Equals(IHostPackage? x, IHostPackage? y)
        {
            return ((x.LeftRight == y.LeftRight) && (x.UpDown == y.UpDown));
        }

        int IEqualityComparer<IHostPackage>.GetHashCode(IHostPackage obj)
        {
            return obj.GetHashCode();
        }
    }
}
