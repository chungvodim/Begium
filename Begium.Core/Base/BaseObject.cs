using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Begium.Core
{
    public abstract class BaseObject
    {
        private ILog _log;
        protected virtual ILog log {
            get
            {
                if (_log == null)
                {
                    _log = log4net.LogManager.GetLogger("BaseObject");
                }
                return _log;
            }
            set
            {
                _log = value;
            }
        }
    }
}
