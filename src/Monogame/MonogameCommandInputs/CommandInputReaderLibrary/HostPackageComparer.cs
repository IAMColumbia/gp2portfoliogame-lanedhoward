using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInputReaderLibrary
{
    public class HostPackageComparer : EqualityComparer<IHostPackage>
    {
        public override bool Equals(IHostPackage? x, IHostPackage? y)
        {
            if (x == null && y == null) return true;
            if (x == null ^ y == null) return false; // XOR
            return ((x.LeftRight == y.LeftRight) && (x.UpDown == y.UpDown));
        }

        public override int GetHashCode(IHostPackage obj)
        {
            return obj.GetHashCode();
        }
    }
}
