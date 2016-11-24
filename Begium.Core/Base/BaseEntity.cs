using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Begium.Core.Base
{
    public abstract class BaseEntity : BaseObject
    {
        public override string ToString()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //BindingFlags flags = BindingFlags.Public;
                foreach (PropertyInfo property in this.GetType().GetProperties())
                {
                    var value = property.GetValue(this);
                    if (value != null)
                    {
                        sb.Append(value.ToString() + " ");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return base.ToString();
            }
        }
    }
}
