using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeLayer
{
    public interface INotificationClient
    {
        Task ReceiveNotification(object notification);
    }
}
