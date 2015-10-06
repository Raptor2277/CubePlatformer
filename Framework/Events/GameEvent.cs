using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Events
{
    public class EventTriggeredEventArgs : EventArgs
    {

    }

    public class GameEvent
    {
        public event EventHandler<EventTriggeredEventArgs> EventTriggered;

        public void onEventTriggered()
        {
            if(EventTriggered != null)
                EventTriggered(this, new EventTriggeredEventArgs());
        }
    }
}
