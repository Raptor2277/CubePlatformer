using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube_Platformer.Framework.Utilities
{
    class Id
    {
        public List<int> PlayerIds { get; private set; }

        public bool isPlayer(int bodyId)
        {
            if(PlayerIds.Contains(bodyId))
                return true;
            return false;
        }
    }
}
