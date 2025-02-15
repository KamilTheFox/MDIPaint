using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIPaint
{
    public interface IReceiverSize
    {
        void SetSeze(IRequesterSize requester);

        void SetDefaultSize(IRequesterSize requester);
    }
}
