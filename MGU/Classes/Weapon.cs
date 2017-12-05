using System;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace MGU
{
    public class Weapon : Item
    {
        public Race weapon_race;
        public int cost;
        public int shield_damage;
        public int armor_damage;
        public int accuracy;
        public int power;
        public int emp;
        public int alignment;
        public int attack_type;

        public Weapon()
        {
            weapon_race = null;
            cost = 0;
            shield_damage = 0;
            armor_damage = 0;
            accuracy = 0;
            power = 0;
            emp = 0;
            alignment = 0;
            attack_type = 0;
        }
    }
}
