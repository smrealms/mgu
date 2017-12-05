using System;
using System.Collections.Generic;
using System.Text;

namespace MGU
{
    public class ForceData
    {
        public string owner;

        public int affiliation;

        public int mines;
        public int combat_drones;
        public int scout_drones;

        public int sectorid;

        public ForceData()
        {
            owner = "";

            mines = 0;
            combat_drones = 0;
            scout_drones = 0;
        }
    }
}
