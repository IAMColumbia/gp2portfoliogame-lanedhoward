using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameCommandInputs
{
    public abstract class InputHost : IInputHost
    {
        public virtual IHostPackage GetCurrentInputs()
        {
            IHostPackage package = new HostPackage();

            return package;
        }
    }
}