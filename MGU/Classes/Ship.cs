using System;
using System.Collections.Generic;
using System.Text;

namespace MGU
{
    public class Ship : Item
    {
        public Race ship_race;
        public string ship_class;
        public long ship_cost;
        public int ship_speed;
        public int ship_hardpoints;
        public int ship_power;
        public int ship_shields;
        public int ship_armor;
        public int ship_cargo;
        public int ship_scout_drones;
        public int ship_combat_drones;
        public int ship_mines;
        public int ship_level_needed;
        public bool ship_JumpAble;
        public bool ship_CloakAble;
        public bool ship_IGAble;
        public bool ship_ScanAble;
        public bool ship_DSAble;
        public int ship_restrictions;

        public Ship()
        {

        }
    }
}
