using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatTheDungeon
{
    enum DoorType
    {
        preDoor,
        nextDoor
    }
    internal class Door
    {
        //Field-------------
        private bool ifOpen;
        private DoorType type;

        //properties-------------

        public bool IfOpen { get => IfOpen; set => IfOpen = value; }

        //Constructor
        public Door(DoorType type)
        {
            this.type = type;
        }

    }
}
