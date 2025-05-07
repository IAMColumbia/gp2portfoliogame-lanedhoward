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

            if ((x.LeftRight == y.LeftRight) && (x.UpDown == y.UpDown))
            {
                if (x.Buttons.Count == y.Buttons.Count)
                {
                    if ((x.Buttons.All(b => y.Buttons.Any(c => (c.GetType() == b.GetType()/* && c.State == b.State*/)))) 
                        && (y.Buttons.All(b => x.Buttons.Any(c => (c.GetType() == b.GetType()/* && c.State == b.State*/)))))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override int GetHashCode(IHostPackage obj)
        {
            return obj.GetHashCode();
        }
    }
}
