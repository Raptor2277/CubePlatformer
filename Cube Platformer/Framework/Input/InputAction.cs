using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace Framework.Input
{
    class InputAction
    {
        public List<Keyboard.Key> Keys { get; private set; }

        public InputAction(List<Keyboard.Key> keys)
        {
            this.Keys = keys;
        }

        public InputAction(Keyboard.Key k)
        {
            this.Keys = new List<Keyboard.Key>();
            Keys.Add(k);
        }

        public bool evaluate()
        {
            foreach (Keyboard.Key k in Keys)
                if (Keyboard.IsKeyPressed(k))
                    return true;
            return false;
        }

        public bool evaluate(Keyboard.Key key)
        {
            foreach (Keyboard.Key k in Keys)
                if (k == key)
                    return true;
            return false;
        }
    }
}
