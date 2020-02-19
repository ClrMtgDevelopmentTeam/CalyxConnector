using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalyxSDKConn.Core
{
    public abstract partial class BaseEntity
    {
        public DateTime? CreatedON { get; set; }
        public DateTime? UpdatedON { get; set; }
    }
}
