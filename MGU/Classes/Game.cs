using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
using System.ComponentModel;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace MGU
{
    public class Game
	//This class is the placeholder for all the variables that are of importance for the game except the interface
    {
	//Every game object has a reference to the host MGU application, so it can interface with the GUI
        public MainStuff hostApplication;

        public bool saving;
        public bool smachanged;
        public string address;

        public string game_name;
        public int game_id;
        public Galaxy[] galaxy;
        public ushort nrofgalaxies;
        public Race[] race;
        public bool[] peace;
        public ushort nrofraces;
        public Good[] good;
        public ushort nrofgoods;
        public Location[] location;
        public ushort nroflocations;
        public Ship[] ship;
        public ushort nrofships;
        public Weapon[] weapon;
        public ushort nrofweapons;
        public Technology[] technology;
        public ushort nroftechs;
        public ushort nrofsectors;
        public Route[,] shortestroutes;
        public Route highlightedRoute;
        public ArrayList allianceMembers = new ArrayList();
        public ArrayList seedSectors = new ArrayList();

        //These Imagelists hold the images that are used in the game. Shouldn't these be members of Mainstuff?
        public ImageList Miscellaneous, Goods, Locations;

	//sectorsize determines the size of the sectors in the main viewer. It can be controlled by the user through the style menu
        public int sectorsize;

	//currentGalaxy indicates what galaxy is currently being viewed
        public int currentGalaxy;

	//displayIllegals indicates whether illegal goods should be displayed in the main viewer
        public bool displayIllegals;

        public Game() { }

        public Game(MainStuff host, ImageList Misc, ImageList Locs)
	//The class' main constructor. It 
        {
            hostApplication = host;

	//I think this is obsolete
            string path = Environment.CurrentDirectory + "\\goods\\";
            Image img;

	//The game name and nrofgalaxies are unknown until a .smr file is loaded
            game_name = "Undefined";
            game_id = 0;
            nrofgalaxies = 0;
            sectorsize = 100;

            displayIllegals = false;

            Locations = Locs;
            Miscellaneous = Misc;
        }

        public void InitializeRaces(ushort number, bool loaddefaultraces)
	//This function makes sure that memory is allocated for the given number of races.
        {
            if (loaddefaultraces)
                nrofraces = 9;
            else
                nrofraces = number;
            race = new Race[nrofraces+1];
            peace = new bool[nrofraces + 1];

            for (int i = 0; i < nrofraces + 1; i++)
            {
                race[i] = new Race();
                peace[i] = false;
            }

            peace[1] = true;

            if (loaddefaultraces)
            {
                race[1].race_name = "Neutral";
                race[2].race_name = "Alskant";
                race[3].race_name = "Creonti";
                race[4].race_name = "Ik'Thorne";
                race[5].race_name = "Human";
                race[6].race_name = "Salvene";
                race[7].race_name = "Thevian";
                race[8].race_name = "WQ Human";
                race[9].race_name = "Nijarin";
            }
        }

        public void ComputeGalaxyNeighbours()
        {
            for (int i = 0; i < nrofgalaxies + 1; i++)
            {
                galaxy[i].neighbours.Add(i);

                for (int x = 0; x < galaxy[i].galaxy_xsize; x++)
                    for (int y = 0; y < galaxy[i].galaxy_ysize; y++)
                    {
                        if (galaxy[i].sector[x, y].warp != null)
                        {
                            galaxy[i].neighbours.Add(GetGalaxyIndex(galaxy[i].sector[x, y].warp.sector_id));
                        }
                    }
            }

        }

        public void InitializeGoods(ushort number, bool loaddefaultgoods)
	    //This function makes sure that memory is allocated for the given number of goods.
        {
            if (loaddefaultgoods)
                nrofgoods = 12;
            else
                nrofgoods = number;
            good = new Good[nrofgoods+1];
            for (int i = 0; i < nrofgoods+1; i++)
                good[i] = new Good();

            good[0].good_name = "Nothing";
            good[0].good_price = 0;

            if (loaddefaultgoods)
            {
                good[1].good_name = "Wood"; good[1].good_price = 19; good[1].goodorevil = true;
                good[2].good_name = "Food"; good[2].good_price = 25; good[2].goodorevil = true;
                good[3].good_name = "Ore";  good[3].good_price = 42; good[3].goodorevil = true;
                good[4].good_name = "Precious Metals"; good[4].good_price = 62; good[4].goodorevil = true;
                good[5].good_name = "Slaves"; good[5].good_price = 89; good[5].goodorevil = false;
                good[6].good_name = "Textiles"; good[6].good_price = 112; good[6].goodorevil = true;
                good[7].good_name = "Machinery"; good[7].good_price = 126; good[7].goodorevil = true;
                good[8].good_name = "Circuitry"; good[8].good_price = 141; good[8].goodorevil = true;
                good[9].good_name = "Weapons"; good[9].good_price = 168; good[9].goodorevil = false;
                good[10].good_name = "Computer"; good[10].good_price = 196; good[10].goodorevil = true;
                good[11].good_name = "Luxury Items"; good[11].good_price = 231; good[11].goodorevil = true;
                good[12].good_name = "Narcotics"; good[12].good_price = 259; good[12].goodorevil = false;             
            }
        }

        public void InitializeWeapons(ushort number, bool loaddefaultweapons)
	//This function makes sure that memory is allocated for the given number of weapons.
        {
            if (loaddefaultweapons)
                nrofweapons = 57;
            else
                nrofweapons = number;

            nrofweapons = number;
            weapon = new Weapon[nrofweapons + 1];
            for (int i = 0; i < nrofweapons + 1; i++)
                weapon[i] = new Weapon();
            #region Load Default Weapons
            if (loaddefaultweapons)
            {
                weapon[1].accuracy = 65;
                weapon[1].alignment = 0;
                weapon[1].armor_damage = 40;
                weapon[1].cost = 0;
                weapon[1].name = "Newbie Pulse Laser";
                weapon[1].power = 3;
                weapon[1].shield_damage = 40;
                weapon[1].weapon_race = GetRaceObject(1);
                weapon[1].emp = 0;
                weapon[1].attack_type = 0;
                weapon[1].sold_at = "";

                weapon[2].accuracy = 35;
                weapon[2].alignment = 2;
                weapon[2].armor_damage = 300;
                weapon[2].cost = 352500;
                weapon[2].name = "Nuke";
                weapon[2].power = 5;
                weapon[2].shield_damage = 0;
                weapon[2].weapon_race = GetRaceObject(1);
                weapon[2].emp = 0;
                weapon[2].attack_type = 0;
                weapon[2].sold_at = "";

                weapon[3].accuracy = 35;
                weapon[3].alignment = 1;
                weapon[3].armor_damage = 0;
                weapon[3].cost = 200350;
                weapon[3].name = "Holy Hand Grenade";
                weapon[3].power = 5;
                weapon[3].shield_damage = 300;
                weapon[3].weapon_race = GetRaceObject(1);
                weapon[3].emp = 0;
                weapon[3].attack_type = 0;
                weapon[3].sold_at = "";

                weapon[4].accuracy = 54;
                weapon[4].alignment = 0;
                weapon[4].armor_damage = 45;
                weapon[4].cost = 352500;
                weapon[4].name = "Thevian Rail Gun";
                weapon[4].power = 1;
                weapon[4].shield_damage = 0;
                weapon[4].weapon_race = GetRaceObject(7);
                weapon[4].emp = 0;
                weapon[4].attack_type = 0;
                weapon[4].sold_at = "";

                weapon[5].accuracy = 51;
                weapon[5].alignment = 0;
                weapon[5].armor_damage = 90;
                weapon[5].cost = 86750;
                weapon[5].name = "Thevian Assault Laser";
                weapon[5].power = 3;
                weapon[5].shield_damage = 20;
                weapon[5].weapon_race = GetRaceObject(7);
                weapon[5].emp = 0;
                weapon[5].attack_type = 0;
                weapon[5].sold_at = "";

                weapon[6].accuracy = 54;
                weapon[6].alignment = 0;
                weapon[6].armor_damage = 40;
                weapon[6].cost = 137500;
                weapon[6].name = "Thevian Flux Missile";
                weapon[6].power = 2;
                weapon[6].shield_damage = 40;
                weapon[6].weapon_race = GetRaceObject(7);
                weapon[6].emp = 0;
                weapon[6].attack_type = 0;
                weapon[6].sold_at = "";

                weapon[7].accuracy = 48;
                weapon[7].alignment = 0;
                weapon[7].armor_damage = 0;
                weapon[7].cost = 94000;
                weapon[7].name = "Thevian Shield Disperser";
                weapon[7].power = 4;
                weapon[7].shield_damage = 175;
                weapon[7].weapon_race = GetRaceObject(7);
                weapon[7].emp = 0;
                weapon[7].attack_type = 0;
                weapon[7].sold_at = "";

                weapon[8].accuracy = 58;
                weapon[8].alignment = 0;
                weapon[8].armor_damage = 0;
                weapon[8].cost = 135000;
                weapon[8].name = "Salvene Flux Resonator";
                weapon[8].power = 2;
                weapon[8].shield_damage = 75;
                weapon[8].weapon_race = GetRaceObject(6);
                weapon[8].emp = 0;
                weapon[8].attack_type = 0;
                weapon[8].sold_at = "";

                weapon[9].accuracy = 44;
                weapon[9].alignment = 0;
                weapon[9].armor_damage = 50;
                weapon[9].cost = 187500;
                weapon[9].name = "Salvene EM Flux Cannon";
                weapon[9].power = 3;
                weapon[9].shield_damage = 100;
                weapon[9].weapon_race = GetRaceObject(6);
                weapon[9].emp = 0;
                weapon[9].attack_type = 0;
                weapon[9].sold_at = "";

                weapon[10].accuracy = 54;
                weapon[10].alignment = 0;
                weapon[10].armor_damage = 75;
                weapon[10].cost = 187500;
                weapon[10].name = "Salvene Frag Missile";
                weapon[10].power = 2;
                weapon[10].shield_damage = 0;
                weapon[10].weapon_race = GetRaceObject(6);
                weapon[10].emp = 0;
                weapon[10].attack_type = 0;
                weapon[10].sold_at = "";

                weapon[11].accuracy = 54;
                weapon[11].alignment = 0;
                weapon[11].armor_damage = 40;
                weapon[11].cost = 91250;
                weapon[11].name = "Salvene Chain Laser";
                weapon[11].power = 3;
                weapon[11].shield_damage = 70;
                weapon[11].weapon_race = GetRaceObject(6);
                weapon[11].emp = 0;
                weapon[11].attack_type = 0;
                weapon[11].sold_at = "";

                weapon[12].accuracy = 54;
                weapon[12].alignment = 0;
                weapon[12].armor_damage = 50;
                weapon[12].cost = 140000;
                weapon[12].name = "Ik-Thorne Burst Laser System";
                weapon[12].power = 3;
                weapon[12].shield_damage = 50;
                weapon[12].weapon_race = GetRaceObject(5);
                weapon[12].emp = 0;
                weapon[12].attack_type = 0;
                weapon[12].sold_at = "";

                weapon[13].accuracy = 58;
                weapon[13].alignment = 0;
                weapon[13].armor_damage = 50;
                weapon[13].cost = 137500;
                weapon[13].name = "Ik-Thorne Rapid Fire Cannon";
                weapon[13].power = 1;
                weapon[13].shield_damage = 0;
                weapon[13].weapon_race = GetRaceObject(5);
                weapon[13].emp = 0;
                weapon[13].attack_type = 0;
                weapon[13].sold_at = "";

                weapon[14].accuracy = 51;
                weapon[14].alignment = 0;
                weapon[14].armor_damage = 80;
                weapon[14].cost = 87500;
                weapon[14].name = "Ik-Thorne Cluster Missile";
                weapon[14].power = 2;
                weapon[14].shield_damage = 0;
                weapon[14].weapon_race = GetRaceObject(5);
                weapon[14].emp = 0;
                weapon[14].attack_type = 0;
                weapon[14].sold_at = "";

                weapon[15].accuracy = 62;
                weapon[15].alignment = 0;
                weapon[15].armor_damage = 0;
                weapon[15].cost = 92000;
                weapon[15].name = "Ik-Thorne Accoustic Jammer";
                weapon[15].power = 2;
                weapon[15].shield_damage = 50;
                weapon[15].weapon_race = GetRaceObject(5);
                weapon[15].emp = 0;
                weapon[15].attack_type = 0;
                weapon[15].sold_at = "";

                weapon[16].accuracy = 78;
                weapon[16].alignment = 0;
                weapon[16].armor_damage = 50;
                weapon[16].cost = 90000;
                weapon[16].name = "WQ Human Flechette Cannon";
                weapon[16].power = 2;
                weapon[16].shield_damage = 0;
                weapon[16].weapon_race = GetRaceObject(8);
                weapon[16].emp = 0;
                weapon[16].attack_type = 0;
                weapon[16].sold_at = "";

                weapon[17].accuracy = 38;
                weapon[17].alignment = 0;
                weapon[17].armor_damage = 0;
                weapon[17].cost = 185000;
                weapon[17].name = "WQ Human Shield Vaporizer";
                weapon[17].power = 4;
                weapon[17].shield_damage = 200;
                weapon[17].weapon_race = GetRaceObject(8);
                weapon[17].emp = 0;
                weapon[17].attack_type = 0;
                weapon[17].sold_at = "";

                weapon[18].accuracy = 80;
                weapon[18].alignment = 0;
                weapon[18].armor_damage = 0;
                weapon[18].cost = 98000;
                weapon[18].name = "Human Harmonic Disruptor";
                weapon[18].power = 4;
                weapon[18].shield_damage = 100;
                weapon[18].weapon_race = GetRaceObject(4);
                weapon[18].emp = 0;
                weapon[18].attack_type = 0;
                weapon[18].sold_at = "";

                weapon[19].accuracy = 82;
                weapon[19].alignment = 0;
                weapon[19].armor_damage = 50;
                weapon[19].cost = 195000;
                weapon[19].name = "Human Space Shotgun";
                weapon[19].power = 2;
                weapon[19].shield_damage = 0;
                weapon[19].weapon_race = GetRaceObject(4);
                weapon[19].emp = 0;
                weapon[19].attack_type = 0;
                weapon[19].sold_at = "";

                weapon[20].accuracy = 54;
                weapon[20].alignment = 0;
                weapon[20].armor_damage = 40;
                weapon[20].cost = 47500;
                weapon[20].name = "WQ Human Multi-Phase Laser";
                weapon[20].power = 3;
                weapon[20].shield_damage = 60;
                weapon[20].weapon_race = GetRaceObject(8);
                weapon[20].emp = 0;
                weapon[20].attack_type = 0;
                weapon[20].sold_at = "";

                weapon[21].accuracy = 54;
                weapon[21].alignment = 0;
                weapon[21].armor_damage = 60;
                weapon[21].cost = 78000;
                weapon[21].name = "Human Multi-Phase Laser";
                weapon[21].power = 3;
                weapon[21].shield_damage = 40;
                weapon[21].weapon_race = GetRaceObject(4);
                weapon[21].emp = 0;
                weapon[21].attack_type = 0;
                weapon[21].sold_at = "";

                weapon[22].accuracy = 44;
                weapon[22].alignment = 0;
                weapon[22].armor_damage = 150;
                weapon[22].cost = 78000;
                weapon[22].name = "Photon Torpedo";
                weapon[22].power = 3;
                weapon[22].shield_damage = 0;
                weapon[22].weapon_race = GetRaceObject(1);
                weapon[22].emp = 0;
                weapon[22].attack_type = 0;
                weapon[22].sold_at = "";

                weapon[23].accuracy = 48;
                weapon[23].alignment = 0;
                weapon[23].armor_damage = 150;
                weapon[23].cost = 142500;
                weapon[23].name = "Human Photon Torpedo";
                weapon[23].power = 3;
                weapon[23].shield_damage = 0;
                weapon[23].weapon_race = GetRaceObject(4);
                weapon[23].emp = 0;
                weapon[23].attack_type = 0;
                weapon[23].sold_at = "";

                weapon[24].accuracy = 72;
                weapon[24].alignment = 0;
                weapon[24].armor_damage = 30;
                weapon[24].cost = 142500;
                weapon[24].name = "Creonti Particle Cannon";
                weapon[24].power = 2;
                weapon[24].shield_damage = 30;
                weapon[24].weapon_race = GetRaceObject(3);
                weapon[24].emp = 0;
                weapon[24].attack_type = 0;
                weapon[24].sold_at = "";

                weapon[25].accuracy = 74;
                weapon[25].alignment = 0;
                weapon[25].armor_damage = 110;
                weapon[25].cost = 90500;
                weapon[25].name = "Little Junior Torpedo";
                weapon[25].power = 4;
                weapon[25].shield_damage = 0;
                weapon[25].weapon_race = GetRaceObject(1);
                weapon[25].emp = 0;
                weapon[25].attack_type = 0;
                weapon[25].sold_at = "";

                weapon[26].accuracy = 68;
                weapon[26].alignment = 0;
                weapon[26].armor_damage = 85;
                weapon[26].cost = 156500;
                weapon[26].name = "Creonti Mole Missile";
                weapon[26].power = 3;
                weapon[26].shield_damage = 0;
                weapon[26].weapon_race = GetRaceObject(3);
                weapon[26].emp = 0;
                weapon[26].attack_type = 0;
                weapon[26].sold_at = "";

                weapon[27].accuracy = 68;
                weapon[27].alignment = 0;
                weapon[27].armor_damage = 0;
                weapon[27].cost = 132750;
                weapon[27].name = "Creonti Shield Sucker";
                weapon[27].power = 3;
                weapon[27].shield_damage = 85;
                weapon[27].weapon_race = GetRaceObject(3);
                weapon[27].emp = 0;
                weapon[27].attack_type = 0;
                weapon[27].sold_at = "";

                weapon[28].accuracy = 80;
                weapon[28].alignment = 0;
                weapon[28].armor_damage = 50;
                weapon[28].cost = 96000;
                weapon[28].name = "Alskant Focused Laser";
                weapon[28].power = 4;
                weapon[28].shield_damage = 50;
                weapon[28].weapon_race = GetRaceObject(2);
                weapon[28].emp = 0;
                weapon[28].attack_type = 0;
                weapon[28].sold_at = "";

                weapon[29].accuracy = 54;
                weapon[29].alignment = 0;
                weapon[29].armor_damage = 60;
                weapon[29].cost = 167500;
                weapon[29].name = "Alskant Pulse-Fist Missile";
                weapon[29].power = 2;
                weapon[29].shield_damage = 25;
                weapon[29].weapon_race = GetRaceObject(2);
                weapon[29].emp = 0;
                weapon[29].attack_type = 0;
                weapon[29].sold_at = "";

                weapon[30].accuracy = 62;
                weapon[30].alignment = 0;
                weapon[30].armor_damage = 90;
                weapon[30].cost = 104000;
                weapon[30].name = "Alskant Space Flechette";
                weapon[30].power = 3;
                weapon[30].shield_damage = 0;
                weapon[30].weapon_race = GetRaceObject(2);
                weapon[30].emp = 0;
                weapon[30].attack_type = 0;
                weapon[30].sold_at = "";

                weapon[31].accuracy = 62;
                weapon[31].alignment = 0;
                weapon[31].armor_damage = 0;
                weapon[31].cost = 113500;
                weapon[31].name = "Alskant Anti-Shield System";
                weapon[31].power = 2;
                weapon[31].shield_damage = 65;
                weapon[31].weapon_race = GetRaceObject(2);
                weapon[31].emp = 0;
                weapon[31].attack_type = 0;
                weapon[31].sold_at = "";

                weapon[32].accuracy = 72;
                weapon[32].alignment = 0;
                weapon[32].armor_damage = 50;
                weapon[32].cost = 93000;
                weapon[32].name = "Anti-Ship Missile (Guided)";
                weapon[32].power = 2;
                weapon[32].shield_damage = 0;
                weapon[32].weapon_race = GetRaceObject(1);
                weapon[32].emp = 0;
                weapon[32].attack_type = 0;
                weapon[32].sold_at = "";

                weapon[33].accuracy = 58;
                weapon[33].alignment = 0;
                weapon[33].armor_damage = 50;
                weapon[33].cost = 87500;
                weapon[33].name = "Anti-Ship Missile (Heat-Seeking)";
                weapon[33].power = 1;
                weapon[33].shield_damage = 0;
                weapon[33].weapon_race = GetRaceObject(1);
                weapon[33].emp = 0;
                weapon[33].attack_type = 0;
                weapon[33].sold_at = "";

                weapon[34].accuracy = 48;
                weapon[34].alignment = 0;
                weapon[34].armor_damage = 50;
                weapon[34].cost = 87500;
                weapon[34].name = "Anti-Ship Missile";
                weapon[34].power = 1;
                weapon[34].shield_damage = 0;
                weapon[34].weapon_race = GetRaceObject(1);
                weapon[34].emp = 0;
                weapon[34].attack_type = 0;
                weapon[34].sold_at = "";

                weapon[35].accuracy = 51;
                weapon[35].alignment = 0;
                weapon[35].armor_damage = 85;
                weapon[35].cost = 189750;
                weapon[35].name = "Huge Pulse Laser";
                weapon[35].power = 4;
                weapon[35].shield_damage = 85;
                weapon[35].weapon_race = GetRaceObject(1);
                weapon[35].emp = 0;
                weapon[35].attack_type = 0;
                weapon[35].sold_at = "";

                weapon[36].accuracy = 58;
                weapon[36].alignment = 0;
                weapon[36].armor_damage = 60;
                weapon[36].cost = 189750;
                weapon[36].name = "Large Pulse Laser";
                weapon[36].power = 3;
                weapon[36].shield_damage = 60;
                weapon[36].weapon_race = GetRaceObject(1);
                weapon[36].emp = 0;
                weapon[36].attack_type = 0;
                weapon[36].sold_at = "";

                weapon[37].accuracy = 65;
                weapon[37].alignment = 0;
                weapon[37].armor_damage = 40;
                weapon[37].cost = 111000;
                weapon[37].name = "Pulse Laser";
                weapon[37].power = 3;
                weapon[37].shield_damage = 40;
                weapon[37].weapon_race = GetRaceObject(1);
                weapon[37].emp = 0;
                weapon[37].attack_type = 0;
                weapon[37].sold_at = "";

                weapon[38].accuracy = 58;
                weapon[38].alignment = 0;
                weapon[38].armor_damage = 75;
                weapon[38].cost = 94000;
                weapon[38].name = "Projectile Cannon Lvl 4";
                weapon[38].power = 2;
                weapon[38].shield_damage = 0;
                weapon[38].weapon_race = GetRaceObject(1);
                weapon[38].emp = 0;
                weapon[38].attack_type = 0;
                weapon[38].sold_at = "";

                weapon[39].accuracy = 65;
                weapon[39].alignment = 0;
                weapon[39].armor_damage = 50;
                weapon[39].cost = 71250;
                weapon[39].name = "Projectile Cannon Lvl 3";
                weapon[39].power = 2;
                weapon[39].shield_damage = 0;
                weapon[39].weapon_race = GetRaceObject(1);
                weapon[39].emp = 0;
                weapon[39].attack_type = 0;
                weapon[39].sold_at = "";

                weapon[40].accuracy = 68;
                weapon[40].alignment = 0;
                weapon[40].armor_damage = 35;
                weapon[40].cost = 67500;
                weapon[40].name = "Projectile Cannon Lvl 2";
                weapon[40].power = 1;
                weapon[40].shield_damage = 0;
                weapon[40].weapon_race = GetRaceObject(1);
                weapon[40].emp = 0;
                weapon[40].attack_type = 0;
                weapon[40].sold_at = "";

                weapon[41].accuracy = 72;
                weapon[41].alignment = 0;
                weapon[41].armor_damage = 20;
                weapon[41].cost = 55250;
                weapon[41].name = "Projectile Cannon Lvl 1";
                weapon[41].power = 1;
                weapon[41].shield_damage = 0;
                weapon[41].weapon_race = GetRaceObject(1);
                weapon[41].emp = 0;
                weapon[41].attack_type = 0;
                weapon[41].sold_at = "";

                weapon[42].accuracy = 64;
                weapon[42].alignment = 0;
                weapon[42].armor_damage = 0;
                weapon[42].cost = 72000;
                weapon[42].name = "Advanced Shield Disruptor";
                weapon[42].power = 2;
                weapon[42].shield_damage = 60;
                weapon[42].weapon_race = GetRaceObject(1);
                weapon[42].emp = 0;
                weapon[42].attack_type = 0;
                weapon[42].sold_at = "";

                weapon[43].accuracy = 68;
                weapon[43].alignment = 0;
                weapon[43].armor_damage = 0;
                weapon[43].cost = 46000;
                weapon[43].name = "Shield Disruptor";
                weapon[43].power = 1;
                weapon[43].shield_damage = 30;
                weapon[43].weapon_race = GetRaceObject(1);
                weapon[43].emp = 0;
                weapon[43].attack_type = 0;
                weapon[43].sold_at = "";

                weapon[44].accuracy = 54;
                weapon[44].alignment = 0;
                weapon[44].armor_damage = 60;
                weapon[44].cost = 81000;
                weapon[44].name = "Insanely Large Laser";
                weapon[44].power = 3;
                weapon[44].shield_damage = 60;
                weapon[44].weapon_race = GetRaceObject(1);
                weapon[44].emp = 0;
                weapon[44].attack_type = 0;
                weapon[44].sold_at = "";

                weapon[45].accuracy = 62;
                weapon[45].alignment = 0;
                weapon[45].armor_damage = 40;
                weapon[45].cost = 54000;
                weapon[45].name = "Large Laser";
                weapon[45].power = 2;
                weapon[45].shield_damage = 40;
                weapon[45].weapon_race = GetRaceObject(1);
                weapon[45].emp = 0;
                weapon[45].attack_type = 0;
                weapon[45].sold_at = "";

                weapon[46].accuracy = 68;
                weapon[46].alignment = 0;
                weapon[46].armor_damage = 25;
                weapon[46].cost = 88750;
                weapon[46].name = "Laser";
                weapon[46].power = 2;
                weapon[46].shield_damage = 25;
                weapon[46].weapon_race = GetRaceObject(1);
                weapon[46].emp = 0;
                weapon[46].attack_type = 0;
                weapon[46].sold_at = "";

                weapon[47].accuracy = 72;
                weapon[47].alignment = 0;
                weapon[47].armor_damage = 10;
                weapon[47].cost = 43500;
                weapon[47].name = "Small Laser";
                weapon[47].power = 1;
                weapon[47].shield_damage = 10;
                weapon[47].weapon_race = GetRaceObject(1);
                weapon[47].emp = 0;
                weapon[47].attack_type = 0;
                weapon[47].sold_at = "";

                weapon[48].accuracy = 34;
                weapon[48].alignment = 0;
                weapon[48].armor_damage = 250;
                weapon[48].cost = 251000;
                weapon[48].name = "Creonti Big Daddy";
                weapon[48].power = 4;
                weapon[48].shield_damage = 0;
                weapon[48].weapon_race = GetRaceObject(3);
                weapon[48].emp = 0;
                weapon[48].attack_type = 0;
                weapon[48].sold_at = "";

                weapon[49].accuracy = 40;
                weapon[49].alignment = 0;
                weapon[49].armor_damage = 200;
                weapon[49].cost = 153000;
                weapon[49].name = "Big Momma Torpedo Launcher";
                weapon[49].power = 4;
                weapon[49].shield_damage = 0;
                weapon[49].weapon_race = GetRaceObject(1);
                weapon[49].emp = 0;
                weapon[49].attack_type = 0;
                weapon[49].sold_at = "";

                weapon[50].accuracy = 40;
                weapon[50].alignment = 0;
                weapon[50].armor_damage = 100;
                weapon[50].cost = 55000;
                weapon[50].name = "Torpedo Launcher";
                weapon[50].power = 2;
                weapon[50].shield_damage = 0;
                weapon[50].weapon_race = GetRaceObject(1);
                weapon[50].emp = 0;
                weapon[50].attack_type = 0;
                weapon[50].sold_at = "";

                weapon[51].accuracy = 58;
                weapon[51].alignment = 0;
                weapon[51].armor_damage = 5;
                weapon[51].cost = 85000;
                weapon[51].name = "Nijarin Ion Pulse Phaser";
                weapon[51].power = 1;
                weapon[51].shield_damage = 35;
                weapon[51].weapon_race = GetRaceObject(9);
                weapon[51].emp = 0;
                weapon[51].attack_type = 0;
                weapon[51].sold_at = "";

                weapon[52].accuracy = 50;
                weapon[52].alignment = 0;
                weapon[52].armor_damage = 20;
                weapon[52].cost = 100000;
                weapon[52].name = "Nijarin Ion Disrupter";
                weapon[52].power = 2;
                weapon[52].shield_damage = 80;
                weapon[52].weapon_race = GetRaceObject(9);
                weapon[52].emp = 0;
                weapon[52].attack_type = 0;
                weapon[52].sold_at = "";

                weapon[53].accuracy = 44;
                weapon[53].alignment = 0;
                weapon[53].armor_damage = 20;
                weapon[53].cost = 180000;
                weapon[53].name = "Nijarin Ion Phaser Beam";
                weapon[53].power = 3;
                weapon[53].shield_damage = 130;
                weapon[53].weapon_race = GetRaceObject(9);
                weapon[53].emp = 0;
                weapon[53].attack_type = 0;
                weapon[53].sold_at = "";

                weapon[54].accuracy = 51;
                weapon[54].alignment = 0;
                weapon[54].armor_damage = 170;
                weapon[54].cost = 200000;
                weapon[54].name = "Nijarin Claymore Missile";
                weapon[54].power = 4;
                weapon[54].shield_damage = 0;
                weapon[54].weapon_race = GetRaceObject(9);
                weapon[54].emp = 0;
                weapon[54].attack_type = 0;
                weapon[54].sold_at = "";

                weapon[55].accuracy = 34;
                weapon[55].alignment = 0;
                weapon[55].armor_damage = 150;
                weapon[55].cost = 325500;
                weapon[55].name = "Planetary Pulse Laser";
                weapon[55].power = 5;
                weapon[55].shield_damage = 150;
                weapon[55].weapon_race = GetRaceObject(1);
                weapon[55].emp = 0;
                weapon[55].attack_type = 0;
                weapon[55].sold_at = "";

                weapon[56].accuracy = 66;
                weapon[56].alignment = 2;
                weapon[56].armor_damage = 666;
                weapon[56].cost = 666;
                weapon[56].name = "Hell Blaster";
                weapon[56].power = 0;
                weapon[56].shield_damage = 666;
                weapon[56].weapon_race = GetRaceObject(1);
                weapon[56].emp = 0;
                weapon[56].attack_type = 0;
                weapon[56].sold_at = "";

                weapon[57].accuracy = 10;
                weapon[57].alignment = 0;
                weapon[57].armor_damage = 250;
                weapon[57].cost = 0;
                weapon[57].name = "Port Turret";
                weapon[57].power = 0;
                weapon[57].shield_damage = 250;
                weapon[57].weapon_race = GetRaceObject(1);
                weapon[57].emp = 0;
                weapon[57].attack_type = 0;
                weapon[57].sold_at = "";

                weapon[57].accuracy = 25;
                weapon[57].alignment = 0;
                weapon[57].armor_damage = 250;
                weapon[57].cost = 0;
                weapon[57].name = "Planet Turret";
                weapon[57].power = 0;
                weapon[57].shield_damage = 250;
                weapon[57].weapon_race = GetRaceObject(1);
                weapon[57].emp = 0;
                weapon[57].attack_type = 0;
                weapon[57].sold_at = "";
            }
            #endregion
        }

        public void InitializeTechs(ushort number, bool loaddefaulttechs)
	//This function makes sure that memory is allocated for the given number of technologies.
        {
            if (loaddefaulttechs)
                nroftechs = 11;
            else
                nroftechs = number;
            technology = new Technology[nroftechs + 1];
            for (int i = 0; i < nroftechs + 1; i++)
                technology[i] = new Technology();

            if (loaddefaulttechs)
            {
                technology[0].cost = 350;
                technology[0].name = "Shields";

                technology[1].cost = 250;
                technology[1].name = "Armor";

                technology[2].cost = 350;
                technology[2].name = "Cargo Holds";

                technology[3].cost = 10000;
                technology[3].name = "Combat Drones";

                technology[4].cost = 5000;
                technology[4].name = "Scout Drones";

                technology[5].cost = 20000;
                technology[5].name = "Mines";

                technology[6].cost = 120000;
                technology[6].name = "Scanner";

                technology[7].cost = 500000;
                technology[7].name = "Cloaking Device";

                technology[8].cost = 300000;
                technology[8].name = "Illusion Generator";

                technology[9].cost = 500000;
                technology[9].name = "Jump Drive";

                technology[10].cost = 400000;
                technology[10].name = "Drone Scrambler";
            }

        }

        public void InitializeShips(ushort number, bool loaddefaultships)
	//This function makes sure that memory is allocated for the given number of ships.
        {
            if (loaddefaultships)
                nrofships = 76;
            else
                nrofships = number;
            ship = new Ship[nrofships + 1];
            for (int i = 0; i < nrofships + 1; i++)
                ship[i] = new Ship();

#region Load Default Ships
            if (loaddefaultships)
            {
                ship[0].name = "A Hatchling's Due";
                ship[0].ship_race = GetRaceObject(6);
                ship[0].ship_cost = 0;
                ship[0].ship_speed = 7;
                ship[0].ship_hardpoints = 1;
                ship[0].ship_power = 5;
                ship[0].ship_shields = 150;
                ship[0].ship_armor = 200;
                ship[0].ship_cargo = 60;
                ship[0].ship_CloakAble = false;
                ship[0].ship_DSAble = false;
                ship[0].ship_IGAble = false;
                ship[0].ship_JumpAble = false;
                ship[0].ship_ScanAble = true;
                ship[0].ship_scout_drones = 0;
                ship[0].ship_mines = 0;
                ship[0].ship_combat_drones = 0;
                ship[0].ship_level_needed = 0;
                ship[0].ship_restrictions = 0;
                ship[0].sold_at = "";

                ship[1].name = "Advanced Carrier";
                ship[1].ship_race = GetRaceObject(5);
                ship[1].ship_cost = 3110264;
                ship[1].ship_speed = 9;
                ship[1].ship_hardpoints = 2;
                ship[1].ship_power = 9;
                ship[1].ship_shields = 750;
                ship[1].ship_armor = 100;
                ship[1].ship_cargo = 150;
                ship[1].ship_CloakAble = false;
                ship[1].ship_DSAble = false;
                ship[1].ship_IGAble = false;
                ship[1].ship_JumpAble = false;
                ship[1].ship_ScanAble = true;
                ship[1].ship_scout_drones = 0;
                ship[1].ship_mines = 50;
                ship[1].ship_combat_drones = 20;
                ship[1].ship_level_needed = 0;
                ship[1].ship_restrictions = 0;
                ship[1].sold_at = "";

                ship[2].name = "Advanced Courier Vessel";
                ship[2].ship_race = GetRaceObject(1);
                ship[2].ship_cost = 3560740;
                ship[2].ship_speed = 10;
                ship[2].ship_hardpoints = 2;
                ship[2].ship_power = 9;
                ship[2].ship_shields = 300;
                ship[2].ship_armor = 350;
                ship[2].ship_cargo = 150;
                ship[2].ship_CloakAble = false;
                ship[2].ship_DSAble = false;
                ship[2].ship_IGAble = false;
                ship[2].ship_JumpAble = true;
                ship[2].ship_ScanAble = true;
                ship[2].ship_scout_drones = 0;
                ship[2].ship_mines = 0;
                ship[2].ship_combat_drones = 0;
                ship[2].ship_level_needed = 0;
                ship[2].ship_restrictions = 0;
                ship[2].sold_at = "";

                ship[3].name = "Ambassador";
                ship[3].ship_race = GetRaceObject(4);
                ship[3].ship_cost = 1792187;
                ship[3].ship_speed = 9;
                ship[3].ship_hardpoints = 1;
                ship[3].ship_power = 5;
                ship[3].ship_shields = 500;
                ship[3].ship_armor = 450;
                ship[3].ship_cargo = 150;
                ship[3].ship_CloakAble = false;
                ship[3].ship_DSAble = false;
                ship[3].ship_IGAble = false;
                ship[3].ship_JumpAble = true;
                ship[3].ship_ScanAble = true;
                ship[3].ship_scout_drones = 0;
                ship[3].ship_mines = 0;
                ship[3].ship_combat_drones = 10;
                ship[3].ship_level_needed = 0;
                ship[3].ship_restrictions = 0;
                ship[3].sold_at = "";

                ship[4].name = "Armoured Semi";
                ship[4].ship_race = GetRaceObject(1);
                ship[4].ship_cost = 122152;
                ship[4].ship_speed = 7;
                ship[4].ship_hardpoints = 1;
                ship[4].ship_power = 5;
                ship[4].ship_shields = 225;
                ship[4].ship_armor = 350;
                ship[4].ship_cargo = 60;
                ship[4].ship_CloakAble = false;
                ship[4].ship_DSAble = false;
                ship[4].ship_IGAble = false;
                ship[4].ship_JumpAble = false;
                ship[4].ship_ScanAble = false;
                ship[4].ship_scout_drones = 0;
                ship[4].ship_mines = 0;
                ship[4].ship_combat_drones = 0;
                ship[4].ship_level_needed = 0;
                ship[4].ship_restrictions = 0;
                ship[4].sold_at = "";

                ship[5].name = "Assassin";
                ship[5].ship_race = GetRaceObject(1);
                ship[5].ship_cost = 12483452;
                ship[5].ship_speed = 11;
                ship[5].ship_hardpoints = 4;
                ship[5].ship_power = 16;
                ship[5].ship_shields = 500;
                ship[5].ship_armor = 400;
                ship[5].ship_cargo = 120;
                ship[5].ship_CloakAble = true;
                ship[5].ship_DSAble = false;
                ship[5].ship_IGAble = false;
                ship[5].ship_JumpAble = false;
                ship[5].ship_ScanAble = true;
                ship[5].ship_scout_drones = 0;
                ship[5].ship_mines = 25;
                ship[5].ship_combat_drones = 10;
                ship[5].ship_level_needed = 0;
                ship[5].ship_restrictions = 2;
                ship[5].sold_at = "";

                ship[6].name = "Assault Craft";
                ship[6].ship_race = GetRaceObject(7);
                ship[6].ship_cost = 12789862;
                ship[6].ship_speed = 10;
                ship[6].ship_hardpoints = 6;
                ship[6].ship_power = 22;
                ship[6].ship_shields = 950;
                ship[6].ship_armor = 950;
                ship[6].ship_cargo = 30;
                ship[6].ship_CloakAble = false;
                ship[6].ship_DSAble = false;
                ship[6].ship_IGAble = false;
                ship[6].ship_JumpAble = false;
                ship[6].ship_ScanAble = true;
                ship[6].ship_scout_drones = 0;
                ship[6].ship_mines = 0;
                ship[6].ship_combat_drones = 0;
                ship[6].ship_level_needed = 0;
                ship[6].ship_restrictions = 0;
                ship[6].sold_at = "";

                ship[7].name = "Battle Cruiser";
                ship[7].ship_race = GetRaceObject(1);
                ship[7].ship_cost = 3628134;
                ship[7].ship_speed = 7;
                ship[7].ship_hardpoints = 5;
                ship[7].ship_power = 19;
                ship[7].ship_shields = 525;
                ship[7].ship_armor = 500;
                ship[7].ship_cargo = 0;
                ship[7].ship_CloakAble = false;
                ship[7].ship_DSAble = false;
                ship[7].ship_IGAble = false;
                ship[7].ship_JumpAble = false;
                ship[7].ship_ScanAble = true;
                ship[7].ship_scout_drones = 0;
                ship[7].ship_mines = 100;
                ship[7].ship_combat_drones = 25;
                ship[7].ship_level_needed = 0;
                ship[7].ship_restrictions = 0;
                ship[7].sold_at = "";

                ship[8].name = "Blockade Runner";
                ship[8].ship_race = GetRaceObject(8);
                ship[8].ship_cost = 5131071;
                ship[8].ship_speed = 9;
                ship[8].ship_hardpoints = 3;
                ship[8].ship_power = 13;
                ship[8].ship_shields = 550;
                ship[8].ship_armor = 500;
                ship[8].ship_cargo = 175;
                ship[8].ship_CloakAble = true;
                ship[8].ship_DSAble = false;
                ship[8].ship_IGAble = false;
                ship[8].ship_JumpAble = false;
                ship[8].ship_ScanAble = true;
                ship[8].ship_scout_drones = 0;
                ship[8].ship_mines = 20;
                ship[8].ship_combat_drones = 10;
                ship[8].ship_level_needed = 0;
                ship[8].ship_restrictions = 0;
                ship[8].sold_at = "";

                ship[9].name = "Border Cruiser";
                ship[9].ship_race = GetRaceObject(4);
                ship[9].ship_cost = 6887432;
                ship[9].ship_speed = 8;
                ship[9].ship_hardpoints = 4;
                ship[9].ship_power = 16;
                ship[9].ship_shields = 575;
                ship[9].ship_armor = 600;
                ship[9].ship_cargo = 120;
                ship[9].ship_CloakAble = false;
                ship[9].ship_DSAble = false;
                ship[9].ship_IGAble = false;
                ship[9].ship_JumpAble = true;
                ship[9].ship_ScanAble = true;
                ship[9].ship_scout_drones = 0;
                ship[9].ship_mines = 150;
                ship[9].ship_combat_drones = 50;
                ship[9].ship_level_needed = 0;
                ship[9].ship_restrictions = 0;
                ship[9].sold_at = "";

                ship[10].name = "Bounty Hunter";
                ship[10].ship_race = GetRaceObject(7);
                ship[10].ship_cost = 1768634;
                ship[10].ship_speed = 11;
                ship[10].ship_hardpoints = 3;
                ship[10].ship_power = 13;
                ship[10].ship_shields = 550;
                ship[10].ship_armor = 650;
                ship[10].ship_cargo = 70;
                ship[10].ship_CloakAble = false;
                ship[10].ship_DSAble = false;
                ship[10].ship_IGAble = false;
                ship[10].ship_JumpAble = false;
                ship[10].ship_ScanAble = true;
                ship[10].ship_scout_drones = 0;
                ship[10].ship_mines = 75;
                ship[10].ship_combat_drones = 50;
                ship[10].ship_level_needed = 0;
                ship[10].ship_restrictions = 0;
                ship[10].sold_at = "";

                ship[11].name = "Carapace";
                ship[11].ship_race = GetRaceObject(7);
                ship[11].ship_cost = 6045094;
                ship[11].ship_speed = 11;
                ship[11].ship_hardpoints = 5;
                ship[11].ship_power = 19;
                ship[11].ship_shields = 600;
                ship[11].ship_armor = 700;
                ship[11].ship_cargo = 30;
                ship[11].ship_CloakAble = false;
                ship[11].ship_DSAble = false;
                ship[11].ship_IGAble = false;
                ship[11].ship_JumpAble = false;
                ship[11].ship_ScanAble = true;
                ship[11].ship_scout_drones = 0;
                ship[11].ship_mines = 100;
                ship[11].ship_combat_drones = 50;
                ship[11].ship_level_needed = 0;
                ship[11].ship_restrictions = 0;
                ship[11].sold_at = "";

                ship[12].name = "Celestial Combatant";
                ship[12].ship_race = GetRaceObject(1);
                ship[12].ship_cost = 2856949;
                ship[12].ship_speed = 7;
                ship[12].ship_hardpoints = 4;
                ship[12].ship_power = 16;
                ship[12].ship_shields = 400;
                ship[12].ship_armor = 350;
                ship[12].ship_cargo = 95;
                ship[12].ship_CloakAble = false;
                ship[12].ship_DSAble = false;
                ship[12].ship_IGAble = true;
                ship[12].ship_JumpAble = false;
                ship[12].ship_ScanAble = true;
                ship[12].ship_scout_drones = 0;
                ship[12].ship_mines = 20;
                ship[12].ship_combat_drones = 5;
                ship[12].ship_level_needed = 0;
                ship[12].ship_restrictions = 0;
                ship[12].sold_at = "";

                ship[13].name = "Celestial Mercenary";
                ship[13].ship_race = GetRaceObject(1);
                ship[13].ship_cost = 2865997;
                ship[13].ship_speed = 9;
                ship[13].ship_hardpoints = 3;
                ship[13].ship_power = 13;
                ship[13].ship_shields = 300;
                ship[13].ship_armor = 275;
                ship[13].ship_cargo = 75;
                ship[13].ship_CloakAble = true;
                ship[13].ship_DSAble = false;
                ship[13].ship_IGAble = false;
                ship[13].ship_JumpAble = false;
                ship[13].ship_ScanAble = true;
                ship[13].ship_scout_drones = 0;
                ship[13].ship_mines = 10;
                ship[13].ship_combat_drones = 5;
                ship[13].ship_level_needed = 0;
                ship[13].ship_restrictions = 0;
                ship[13].sold_at = "";

                ship[14].name = "Celestial Trader";
                ship[14].ship_race = GetRaceObject(1);
                ship[14].ship_cost = 435969;
                ship[14].ship_speed = 9;
                ship[14].ship_hardpoints = 1;
                ship[14].ship_power = 5;
                ship[14].ship_shields = 250;
                ship[14].ship_armor = 275;
                ship[14].ship_cargo = 80;
                ship[14].ship_CloakAble = false;
                ship[14].ship_DSAble = false;
                ship[14].ship_IGAble = true;
                ship[14].ship_JumpAble = false;
                ship[14].ship_ScanAble = false;
                ship[14].ship_scout_drones = 0;
                ship[14].ship_mines = 0;
                ship[14].ship_combat_drones = 0;
                ship[14].ship_level_needed = 0;
                ship[14].ship_restrictions = 0;
                ship[14].sold_at = "";

                ship[15].name = "Dark Mirage";
                ship[15].ship_race = GetRaceObject(8);
                ship[15].ship_cost = 10088764;
                ship[15].ship_speed = 9;
                ship[15].ship_hardpoints = 6;
                ship[15].ship_power = 22;
                ship[15].ship_shields = 825;
                ship[15].ship_armor = 825;
                ship[15].ship_cargo = 60;
                ship[15].ship_CloakAble = true;
                ship[15].ship_DSAble = false;
                ship[15].ship_IGAble = false;
                ship[15].ship_JumpAble = false;
                ship[15].ship_ScanAble = true;
                ship[15].ship_scout_drones = 0;
                ship[15].ship_mines = 30;
                ship[15].ship_combat_drones = 0;
                ship[15].ship_level_needed = 0;
                ship[15].ship_restrictions = 0;
                ship[15].sold_at = "";

                ship[16].name = "Deal-Maker";
                ship[16].ship_race = GetRaceObject(2);
                ship[16].ship_cost = 5727059;
                ship[16].ship_speed = 10;
                ship[16].ship_hardpoints = 2;
                ship[16].ship_power = 9;
                ship[16].ship_shields = 350;
                ship[16].ship_armor = 275;
                ship[16].ship_cargo = 150;
                ship[16].ship_CloakAble = true;
                ship[16].ship_DSAble = false;
                ship[16].ship_IGAble = false;
                ship[16].ship_JumpAble = false;
                ship[16].ship_ScanAble = true;
                ship[16].ship_scout_drones = 0;
                ship[16].ship_mines = 15;
                ship[16].ship_combat_drones = 10;
                ship[16].ship_level_needed = 0;
                ship[16].ship_restrictions = 0;
                ship[16].sold_at = "";

                ship[17].name = "Death Cruiser";
                ship[17].ship_race = GetRaceObject(1);
                ship[17].ship_cost = 29890100;
                ship[17].ship_speed = 10;
                ship[17].ship_hardpoints = 5;
                ship[17].ship_power = 19;
                ship[17].ship_shields = 800;
                ship[17].ship_armor = 400;
                ship[17].ship_cargo = 100;
                ship[17].ship_CloakAble = true;
                ship[17].ship_DSAble = false;
                ship[17].ship_IGAble = false;
                ship[17].ship_JumpAble = false;
                ship[17].ship_ScanAble = true;
                ship[17].ship_scout_drones = 0;
                ship[17].ship_mines = 125;
                ship[17].ship_combat_drones = 20;
                ship[17].ship_level_needed = 0;
                ship[17].ship_restrictions = 2;
                ship[17].sold_at = "";

                ship[18].name = "Deep-Spacer";
                ship[18].ship_race = GetRaceObject(2);
                ship[18].ship_cost = 5721889;
                ship[18].ship_speed = 8;
                ship[18].ship_hardpoints = 3;
                ship[18].ship_power = 13;
                ship[18].ship_shields = 500;
                ship[18].ship_armor = 400;
                ship[18].ship_cargo = 240;
                ship[18].ship_CloakAble = false;
                ship[18].ship_DSAble = false;
                ship[18].ship_IGAble = false;
                ship[18].ship_JumpAble = true;
                ship[18].ship_ScanAble = true;
                ship[18].ship_scout_drones = 0;
                ship[18].ship_mines = 25;
                ship[18].ship_combat_drones = 20;
                ship[18].ship_level_needed = 0;
                ship[18].ship_restrictions = 0;
                ship[18].sold_at = "";

                ship[19].name = "Demonica";
                ship[19].ship_race = GetRaceObject(1);
                ship[19].ship_cost = 0;
                ship[19].ship_speed = 10;
                ship[19].ship_hardpoints = 6;
                ship[19].ship_power = 22;
                ship[19].ship_shields = 6666;
                ship[19].ship_armor = 6666;
                ship[19].ship_cargo = 666;
                ship[19].ship_CloakAble = true;
                ship[19].ship_DSAble = true;
                ship[19].ship_IGAble = true;
                ship[19].ship_JumpAble = true;
                ship[19].ship_ScanAble = true;
                ship[19].ship_scout_drones = 0;
                ship[19].ship_mines = 1000;
                ship[19].ship_combat_drones = 1000;
                ship[19].ship_level_needed = 0;
                ship[19].ship_restrictions = 2;
                ship[19].sold_at = "";

                ship[20].name = "Destroyer";
                ship[20].ship_race = GetRaceObject(4);
                ship[20].ship_cost = 13719505;
                ship[20].ship_speed = 7;
                ship[20].ship_hardpoints = 6;
                ship[20].ship_power = 22;
                ship[20].ship_shields = 750;
                ship[20].ship_armor = 750;
                ship[20].ship_cargo = 90;
                ship[20].ship_CloakAble = false;
                ship[20].ship_DSAble = false;
                ship[20].ship_IGAble = false;
                ship[20].ship_JumpAble = true;
                ship[20].ship_ScanAble = true;
                ship[20].ship_scout_drones = 0;
                ship[20].ship_mines = 0;
                ship[20].ship_combat_drones = 0;
                ship[20].ship_level_needed = 0;
                ship[20].ship_restrictions = 0;
                ship[20].sold_at = "";

                ship[21].name = "Devastator";
                ship[21].ship_race = GetRaceObject(3);
                ship[21].ship_cost = 11559082;
                ship[21].ship_speed = 6;
                ship[21].ship_hardpoints = 8;
                ship[21].ship_power = 26;
                ship[21].ship_shields = 300;
                ship[21].ship_armor = 1800;
                ship[21].ship_cargo = 45;
                ship[21].ship_CloakAble = false;
                ship[21].ship_DSAble = false;
                ship[21].ship_IGAble = false;
                ship[21].ship_JumpAble = false;
                ship[21].ship_ScanAble = true;
                ship[21].ship_scout_drones = 0;
                ship[21].ship_mines = 0;
                ship[21].ship_combat_drones = 0;
                ship[21].ship_level_needed = 0;
                ship[21].ship_restrictions = 0;
                ship[21].sold_at = "";

                ship[22].name = "Drudge";
                ship[22].ship_race = GetRaceObject(6);
                ship[22].ship_cost = 875683;
                ship[22].ship_speed = 6;
                ship[22].ship_hardpoints = 2;
                ship[22].ship_power = 9;
                ship[22].ship_shields = 400;
                ship[22].ship_armor = 525;
                ship[22].ship_cargo = 250;
                ship[22].ship_CloakAble = false;
                ship[22].ship_DSAble = false;
                ship[22].ship_IGAble = false;
                ship[22].ship_JumpAble = false;
                ship[22].ship_ScanAble = true;
                ship[22].ship_scout_drones = 0;
                ship[22].ship_mines = 10;
                ship[22].ship_combat_drones = 0;
                ship[22].ship_level_needed = 0;
                ship[22].ship_restrictions = 0;
                ship[22].sold_at = "";

                ship[23].name = "Eater of Souls";
                ship[23].ship_race = GetRaceObject(6);
                ship[23].ship_cost = 13306175;
                ship[23].ship_speed = 7;
                ship[23].ship_hardpoints = 6;
                ship[23].ship_power = 22;
                ship[23].ship_shields = 1050;
                ship[23].ship_armor = 950;
                ship[23].ship_cargo = 30;
                ship[23].ship_CloakAble = false;
                ship[23].ship_DSAble = false;
                ship[23].ship_IGAble = true;
                ship[23].ship_JumpAble = false;
                ship[23].ship_ScanAble = true;
                ship[23].ship_scout_drones = 0;
                ship[23].ship_mines = 25;
                ship[23].ship_combat_drones = 0;
                ship[23].ship_level_needed = 0;
                ship[23].ship_restrictions = 0;
                ship[23].sold_at = "";

                ship[24].name = "Escape Pod";
                ship[24].ship_race = GetRaceObject(1);
                ship[24].ship_cost = 0;
                ship[24].ship_speed = 7;
                ship[24].ship_hardpoints = 0;
                ship[24].ship_power = 0;
                ship[24].ship_shields = 50;
                ship[24].ship_armor = 50;
                ship[24].ship_cargo = 5;
                ship[24].ship_CloakAble = false;
                ship[24].ship_DSAble = false;
                ship[24].ship_IGAble = false;
                ship[24].ship_JumpAble = false;
                ship[24].ship_ScanAble = false;
                ship[24].ship_scout_drones = 0;
                ship[24].ship_mines = 0;
                ship[24].ship_combat_drones = 0;
                ship[24].ship_level_needed = 0;
                ship[24].ship_restrictions = 0;
                ship[24].sold_at = "";

                ship[25].name = "Expediter";
                ship[25].ship_race = GetRaceObject(7);
                ship[25].ship_cost = 1272250;
                ship[25].ship_speed = 9;
                ship[25].ship_hardpoints = 0;
                ship[25].ship_power = 0;
                ship[25].ship_shields = 375;
                ship[25].ship_armor = 475;
                ship[25].ship_cargo = 180;
                ship[25].ship_CloakAble = false;
                ship[25].ship_DSAble = false;
                ship[25].ship_IGAble = false;
                ship[25].ship_JumpAble = false;
                ship[25].ship_ScanAble = true;
                ship[25].ship_scout_drones = 0;
                ship[25].ship_mines = 0;
                ship[25].ship_combat_drones = 0;
                ship[25].ship_level_needed = 0;
                ship[25].ship_restrictions = 0;
                ship[25].sold_at = "";

                ship[26].name = "Favoured Offspring";
                ship[26].ship_race = GetRaceObject(5);
                ship[26].ship_cost = 1986141;
                ship[26].ship_speed = 8;
                ship[26].ship_hardpoints = 1;
                ship[26].ship_power = 5;
                ship[26].ship_shields = 575;
                ship[26].ship_armor = 100;
                ship[26].ship_cargo = 200;
                ship[26].ship_CloakAble = false;
                ship[26].ship_DSAble = false;
                ship[26].ship_IGAble = true;
                ship[26].ship_JumpAble = false;
                ship[26].ship_ScanAble = false;
                ship[26].ship_scout_drones = 0;
                ship[26].ship_mines = 25;
                ship[26].ship_combat_drones = 10;
                ship[26].ship_level_needed = 0;
                ship[26].ship_restrictions = 0;
                ship[26].sold_at = "";

                ship[27].name = "Federal Discovery";
                ship[27].ship_race = GetRaceObject(1);
                ship[27].ship_cost = 3335689;
                ship[27].ship_speed = 11;
                ship[27].ship_hardpoints = 3;
                ship[27].ship_power = 13;
                ship[27].ship_shields = 500;
                ship[27].ship_armor = 400;
                ship[27].ship_cargo = 60;
                ship[27].ship_CloakAble = false;
                ship[27].ship_DSAble = false;
                ship[27].ship_IGAble = false;
                ship[27].ship_JumpAble = true;
                ship[27].ship_ScanAble = true;
                ship[27].ship_scout_drones = 0;
                ship[27].ship_mines = 0;
                ship[27].ship_combat_drones = 30;
                ship[27].ship_level_needed = 0;
                ship[27].ship_restrictions = 1;
                ship[27].sold_at = "";

                ship[28].name = "Federal Ultimatum";
                ship[28].ship_race = GetRaceObject(1);
                ship[28].ship_cost = 43675738;
                ship[28].ship_speed = 8;
                ship[28].ship_hardpoints = 7;
                ship[28].ship_power = 24;
                ship[28].ship_shields = 700;
                ship[28].ship_armor = 600;
                ship[28].ship_cargo = 120;
                ship[28].ship_CloakAble = false;
                ship[28].ship_DSAble = false;
                ship[28].ship_IGAble = false;
                ship[28].ship_JumpAble = true;
                ship[28].ship_ScanAble = true;
                ship[28].ship_scout_drones = 0;
                ship[28].ship_mines = 0;
                ship[28].ship_combat_drones = 10;
                ship[28].ship_level_needed = 0;
                ship[28].ship_restrictions = 1;
                ship[28].sold_at = "";

                ship[29].name = "Federal Warrant";
                ship[29].ship_race = GetRaceObject(1);
                ship[29].ship_cost = 12026598;
                ship[29].ship_speed = 9;
                ship[29].ship_hardpoints = 5;
                ship[29].ship_power = 19;
                ship[29].ship_shields = 600;
                ship[29].ship_armor = 500;
                ship[29].ship_cargo = 90;
                ship[29].ship_CloakAble = false;
                ship[29].ship_DSAble = false;
                ship[29].ship_IGAble = false;
                ship[29].ship_JumpAble = true;
                ship[29].ship_ScanAble = true;
                ship[29].ship_scout_drones = 0;
                ship[29].ship_mines = 0;
                ship[29].ship_combat_drones = 20;
                ship[29].ship_level_needed = 0;
                ship[29].ship_restrictions = 1;
                ship[29].sold_at = "";

                ship[30].name = "Freighter";
                ship[30].ship_race = GetRaceObject(1);
                ship[30].ship_cost = 1791393;
                ship[30].ship_speed = 6;
                ship[30].ship_hardpoints = 1;
                ship[30].ship_power = 5;
                ship[30].ship_shields = 300;
                ship[30].ship_armor = 600;
                ship[30].ship_cargo = 200;
                ship[30].ship_CloakAble = false;
                ship[30].ship_DSAble = false;
                ship[30].ship_IGAble = false;
                ship[30].ship_JumpAble = false;
                ship[30].ship_ScanAble = true;
                ship[30].ship_scout_drones = 0;
                ship[30].ship_mines = 5;
                ship[30].ship_combat_drones = 5;
                ship[30].ship_level_needed = 0;
                ship[30].ship_restrictions = 0;
                ship[30].sold_at = "";

                ship[31].name = "Fury";
                ship[31].ship_race = GetRaceObject(0);
                ship[31].ship_cost = 14724001;
                ship[31].ship_speed = 8;
                ship[31].ship_hardpoints = 7;
                ship[31].ship_power = 24;
                ship[31].ship_shields = 875;
                ship[31].ship_armor = 700;
                ship[31].ship_cargo = 90;
                ship[31].ship_CloakAble = false;
                ship[31].ship_DSAble = true;
                ship[31].ship_IGAble = false;
                ship[31].ship_JumpAble = false;
                ship[31].ship_ScanAble = true;
                ship[31].ship_scout_drones = 0;
                ship[31].ship_mines = 0;
                ship[31].ship_combat_drones = 0;
                ship[31].ship_level_needed = 0;
                ship[31].ship_restrictions = 0;
                ship[31].sold_at = "";

                ship[32].name = "Galactic Semi";
                ship[32].ship_race = GetRaceObject(1);
                ship[32].ship_cost = 0;
                ship[32].ship_speed = 8;
                ship[32].ship_hardpoints = 1;
                ship[32].ship_power = 5;
                ship[32].ship_shields = 150;
                ship[32].ship_armor = 175;
                ship[32].ship_cargo = 60;
                ship[32].ship_CloakAble = false;
                ship[32].ship_DSAble = false;
                ship[32].ship_IGAble = false;
                ship[32].ship_JumpAble = false;
                ship[32].ship_ScanAble = false;
                ship[32].ship_scout_drones = 0;
                ship[32].ship_mines = 0;
                ship[32].ship_combat_drones = 0;
                ship[32].ship_level_needed = 0;
                ship[32].ship_restrictions = 0;
                ship[32].sold_at = "";

                ship[33].name = "Goliath";
                ship[33].ship_race = GetRaceObject(3);
                ship[33].ship_cost = 3380866;
                ship[33].ship_speed = 10;
                ship[33].ship_hardpoints = 4;
                ship[33].ship_power = 16;
                ship[33].ship_shields = 100;
                ship[33].ship_armor = 900;
                ship[33].ship_cargo = 30;
                ship[33].ship_CloakAble = false;
                ship[33].ship_DSAble = false;
                ship[33].ship_IGAble = false;
                ship[33].ship_JumpAble = false;
                ship[33].ship_ScanAble = true;
                ship[33].ship_scout_drones = 0;
                ship[33].ship_mines = 0;
                ship[33].ship_combat_drones = 0;
                ship[33].ship_level_needed = 0;
                ship[33].ship_restrictions = 0;
                ship[33].sold_at = "";

                ship[34].name = "Inter-Stellar Trader";
                ship[34].ship_race = GetRaceObject(1);
                ship[34].ship_cost = 4314159;
                ship[34].ship_speed = 7;
                ship[34].ship_hardpoints = 2;
                ship[34].ship_power = 9;
                ship[34].ship_shields = 375;
                ship[34].ship_armor = 250;
                ship[34].ship_cargo = 300;
                ship[34].ship_CloakAble = false;
                ship[34].ship_DSAble = false;
                ship[34].ship_IGAble = false;
                ship[34].ship_JumpAble = true;
                ship[34].ship_ScanAble = true;
                ship[34].ship_scout_drones = 0;
                ship[34].ship_mines = 25;
                ship[34].ship_combat_drones = 5;
                ship[34].ship_level_needed = 0;
                ship[34].ship_restrictions = 0;
                ship[34].sold_at = "";

                ship[35].name = "Juggernaut";
                ship[35].ship_race = GetRaceObject(3);
                ship[35].ship_cost = 5104130;
                ship[35].ship_speed = 8;
                ship[35].ship_hardpoints = 5;
                ship[35].ship_power = 19;
                ship[35].ship_shields = 150;
                ship[35].ship_armor = 1150;
                ship[35].ship_cargo = 75;
                ship[35].ship_CloakAble = false;
                ship[35].ship_DSAble = false;
                ship[35].ship_IGAble = false;
                ship[35].ship_JumpAble = false;
                ship[35].ship_ScanAble = true;
                ship[35].ship_scout_drones = 0;
                ship[35].ship_mines = 25;
                ship[35].ship_combat_drones = 25;
                ship[35].ship_level_needed = 0;
                ship[35].ship_restrictions = 0;
                ship[35].sold_at = "";

                ship[36].name = "Leviathan";
                ship[36].ship_race = GetRaceObject(3);
                ship[36].ship_cost = 1122251;
                ship[36].ship_speed = 7;
                ship[36].ship_hardpoints = 2;
                ship[36].ship_power = 9;
                ship[36].ship_shields = 50;
                ship[36].ship_armor = 750;
                ship[36].ship_cargo = 240;
                ship[36].ship_CloakAble = false;
                ship[36].ship_DSAble = false;
                ship[36].ship_IGAble = false;
                ship[36].ship_JumpAble = false;
                ship[36].ship_ScanAble = true;
                ship[36].ship_scout_drones = 0;
                ship[36].ship_mines = 100;
                ship[36].ship_combat_drones = 25;
                ship[36].ship_level_needed = 0;
                ship[36].ship_restrictions = 0;
                ship[36].sold_at = "";

                ship[37].name = "Light Carrier";
                ship[37].ship_race = GetRaceObject(1);
                ship[37].ship_cost = 783035;
                ship[37].ship_speed = 9;
                ship[37].ship_hardpoints = 1;
                ship[37].ship_power = 5;
                ship[37].ship_shields = 375;
                ship[37].ship_armor = 50;
                ship[37].ship_cargo = 75;
                ship[37].ship_CloakAble = false;
                ship[37].ship_DSAble = false;
                ship[37].ship_IGAble = false;
                ship[37].ship_JumpAble = false;
                ship[37].ship_ScanAble = false;
                ship[37].ship_scout_drones = 0;
                ship[37].ship_mines = 25;
                ship[37].ship_combat_drones = 10;
                ship[37].ship_level_needed = 0;
                ship[37].ship_restrictions = 0;
                ship[37].sold_at = "";

                ship[38].name = "Light Courier Vessel";
                ship[38].ship_race = GetRaceObject(1);
                ship[38].ship_cost = 2530676;
                ship[38].ship_speed = 10;
                ship[38].ship_hardpoints = 2;
                ship[38].ship_power = 9;
                ship[38].ship_shields = 250;
                ship[38].ship_armor = 300;
                ship[38].ship_cargo = 120;
                ship[38].ship_CloakAble = false;
                ship[38].ship_DSAble = false;
                ship[38].ship_IGAble = false;
                ship[38].ship_JumpAble = true;
                ship[38].ship_ScanAble = true;
                ship[38].ship_scout_drones = 0;
                ship[38].ship_mines = 0;
                ship[38].ship_combat_drones = 0;
                ship[38].ship_level_needed = 0;
                ship[38].ship_restrictions = 0;
                ship[38].sold_at = "";

                ship[39].name = "Light Cruiser";
                ship[39].ship_race = GetRaceObject(1);
                ship[39].ship_cost = 939440;
                ship[39].ship_speed = 7;
                ship[39].ship_hardpoints = 3;
                ship[39].ship_power = 13;
                ship[39].ship_shields = 350;
                ship[39].ship_armor = 275;
                ship[39].ship_cargo = 75;
                ship[39].ship_CloakAble = false;
                ship[39].ship_DSAble = false;
                ship[39].ship_IGAble = false;
                ship[39].ship_JumpAble = false;
                ship[39].ship_ScanAble = false;
                ship[39].ship_scout_drones = 0;
                ship[39].ship_mines = 10;
                ship[39].ship_combat_drones = 0;
                ship[39].ship_level_needed = 0;
                ship[39].ship_restrictions = 0;
                ship[39].sold_at = "";

                ship[40].name = "Light Freighter";
                ship[40].ship_race = GetRaceObject(4);
                ship[40].ship_cost = 0;
                ship[40].ship_speed = 8;
                ship[40].ship_hardpoints = 1;
                ship[40].ship_power = 5;
                ship[40].ship_shields = 150;
                ship[40].ship_armor = 175;
                ship[40].ship_cargo = 60;
                ship[40].ship_CloakAble = false;
                ship[40].ship_DSAble = false;
                ship[40].ship_IGAble = false;
                ship[40].ship_JumpAble = false;
                ship[40].ship_ScanAble = false;
                ship[40].ship_scout_drones = 0;
                ship[40].ship_mines = 0;
                ship[40].ship_combat_drones = 0;
                ship[40].ship_level_needed = 0;
                ship[40].ship_restrictions = 0;
                ship[40].sold_at = "";

                ship[41].name = "Medium Cargo Hulk";
                ship[41].ship_race = GetRaceObject(3);
                ship[41].ship_cost = 0;
                ship[41].ship_speed = 8;
                ship[41].ship_hardpoints = 1;
                ship[41].ship_power = 5;
                ship[41].ship_shields = 150;
                ship[41].ship_armor = 225;
                ship[41].ship_cargo = 60;
                ship[41].ship_CloakAble = false;
                ship[41].ship_DSAble = false;
                ship[41].ship_IGAble = false;
                ship[41].ship_JumpAble = false;
                ship[41].ship_ScanAble = false;
                ship[41].ship_scout_drones = 0;
                ship[41].ship_mines = 0;
                ship[41].ship_combat_drones = 0;
                ship[41].ship_level_needed = 0;
                ship[41].ship_restrictions = 0;
                ship[41].sold_at = "";

                ship[42].name = "Medium Carrier";
                ship[42].ship_race = GetRaceObject(1);
                ship[42].ship_cost = 1630523;
                ship[42].ship_speed = 8;
                ship[42].ship_hardpoints = 1;
                ship[42].ship_power = 5;
                ship[42].ship_shields = 450;
                ship[42].ship_armor = 100;
                ship[42].ship_cargo = 100;
                ship[42].ship_CloakAble = false;
                ship[42].ship_DSAble = false;
                ship[42].ship_IGAble = false;
                ship[42].ship_JumpAble = false;
                ship[42].ship_ScanAble = false;
                ship[42].ship_scout_drones = 0;
                ship[42].ship_mines = 50;
                ship[42].ship_combat_drones = 10;
                ship[42].ship_level_needed = 0;
                ship[42].ship_restrictions = 0;
                ship[42].sold_at = "";

                ship[43].name = "Medium Cruiser";
                ship[43].ship_race = GetRaceObject(1);
                ship[43].ship_cost = 1461380;
                ship[43].ship_speed = 6;
                ship[43].ship_hardpoints = 4;
                ship[43].ship_power = 16;
                ship[43].ship_shields = 400;
                ship[43].ship_armor = 300;
                ship[43].ship_cargo = 115;
                ship[43].ship_CloakAble = false;
                ship[43].ship_DSAble = false;
                ship[43].ship_IGAble = false;
                ship[43].ship_JumpAble = false;
                ship[43].ship_ScanAble = true;
                ship[43].ship_scout_drones = 0;
                ship[43].ship_mines = 20;
                ship[43].ship_combat_drones = 0;
                ship[43].ship_level_needed = 0;
                ship[43].ship_restrictions = 0;
                ship[43].sold_at = "";

                ship[44].name = "Merchant Vessel";
                ship[44].ship_race = GetRaceObject(1);
                ship[44].ship_cost = 825408;
                ship[44].ship_speed = 9;
                ship[44].ship_hardpoints = 2;
                ship[44].ship_power = 9;
                ship[44].ship_shields = 250;
                ship[44].ship_armor = 325;
                ship[44].ship_cargo = 120;
                ship[44].ship_CloakAble = false;
                ship[44].ship_DSAble = false;
                ship[44].ship_IGAble = false;
                ship[44].ship_JumpAble = false;
                ship[44].ship_ScanAble = false;
                ship[44].ship_scout_drones = 0;
                ship[44].ship_mines = 5;
                ship[44].ship_combat_drones = 5;
                ship[44].ship_level_needed = 0;
                ship[44].ship_restrictions = 0;
                ship[44].sold_at = "";

                ship[45].name = "Mother Ship";
                ship[45].ship_race = GetRaceObject(5);
                ship[45].ship_cost = 6266207;
                ship[45].ship_speed = 6;
                ship[45].ship_hardpoints = 1;
                ship[45].ship_power = 5;
                ship[45].ship_shields = 1000;
                ship[45].ship_armor = 50;
                ship[45].ship_cargo = 120;
                ship[45].ship_CloakAble = false;
                ship[45].ship_DSAble = false;
                ship[45].ship_IGAble = false;
                ship[45].ship_JumpAble = false;
                ship[45].ship_ScanAble = true;
                ship[45].ship_scout_drones = 0;
                ship[45].ship_mines = 50;
                ship[45].ship_combat_drones = 0;
                ship[45].ship_level_needed = 0;
                ship[45].ship_restrictions = 0;
                ship[45].sold_at = "";

                ship[46].name = "Negotiator";
                ship[46].ship_race = GetRaceObject(8);
                ship[46].ship_cost = 155234;
                ship[46].ship_speed = 11;
                ship[46].ship_hardpoints = 1;
                ship[46].ship_power = 5;
                ship[46].ship_shields = 200;
                ship[46].ship_armor = 150;
                ship[46].ship_cargo = 40;
                ship[46].ship_CloakAble = false;
                ship[46].ship_DSAble = false;
                ship[46].ship_IGAble = false;
                ship[46].ship_JumpAble = false;
                ship[46].ship_ScanAble = true;
                ship[46].ship_scout_drones = 0;
                ship[46].ship_mines = 0;
                ship[46].ship_combat_drones = 10;
                ship[46].ship_level_needed = 0;
                ship[46].ship_restrictions = 0;
                ship[46].sold_at = "";

                ship[47].name = "Newbie Merchant Vessel";
                ship[47].ship_race = GetRaceObject(1);
                ship[47].ship_cost = 0;
                ship[47].ship_speed = 9;
                ship[47].ship_hardpoints = 2;
                ship[47].ship_power = 9;
                ship[47].ship_shields = 250;
                ship[47].ship_armor = 325;
                ship[47].ship_cargo = 120;
                ship[47].ship_CloakAble = false;
                ship[47].ship_DSAble = false;
                ship[47].ship_IGAble = false;
                ship[47].ship_JumpAble = false;
                ship[47].ship_ScanAble = true;
                ship[47].ship_scout_drones = 0;
                ship[47].ship_mines = 5;
                ship[47].ship_combat_drones = 0;
                ship[47].ship_level_needed = 0;
                ship[47].ship_restrictions = 0;
                ship[47].sold_at = "";

                ship[48].name = "Planetary Freighter";
                ship[48].ship_race = GetRaceObject(1);
                ship[48].ship_cost = 5215028;
                ship[48].ship_speed = 5;
                ship[48].ship_hardpoints = 1;
                ship[48].ship_power = 5;
                ship[48].ship_shields = 400;
                ship[48].ship_armor = 800;
                ship[48].ship_cargo = 400;
                ship[48].ship_CloakAble = false;
                ship[48].ship_DSAble = false;
                ship[48].ship_IGAble = false;
                ship[48].ship_JumpAble = false;
                ship[48].ship_ScanAble = true;
                ship[48].ship_scout_drones = 0;
                ship[48].ship_mines = 15;
                ship[48].ship_combat_drones = 5;
                ship[48].ship_level_needed = 0;
                ship[48].ship_restrictions = 0;
                ship[48].sold_at = "";

                ship[49].name = "Planetary Super Freighter";
                ship[49].ship_race = GetRaceObject(1);
                ship[49].ship_cost = 9035792;
                ship[49].ship_speed = 4;
                ship[49].ship_hardpoints = 1;
                ship[49].ship_power = 5;
                ship[49].ship_shields = 500;
                ship[49].ship_armor = 1000;
                ship[49].ship_cargo = 600;
                ship[49].ship_CloakAble = false;
                ship[49].ship_DSAble = false;
                ship[49].ship_IGAble = false;
                ship[49].ship_JumpAble = false;
                ship[49].ship_ScanAble = true;
                ship[49].ship_scout_drones = 0;
                ship[49].ship_mines = 50;
                ship[49].ship_combat_drones = 5;
                ship[49].ship_level_needed = 0;
                ship[49].ship_restrictions = 0;
                ship[49].sold_at = "";

                ship[50].name = "Planetary Trader";
                ship[50].ship_race = GetRaceObject(1);
                ship[50].ship_cost = 2283335;
                ship[50].ship_speed = 8;
                ship[50].ship_hardpoints = 2;
                ship[50].ship_power = 9;
                ship[50].ship_shields = 325;
                ship[50].ship_armor = 375;
                ship[50].ship_cargo = 180;
                ship[50].ship_CloakAble = false;
                ship[50].ship_DSAble = false;
                ship[50].ship_IGAble = true;
                ship[50].ship_JumpAble = false;
                ship[50].ship_ScanAble = true;
                ship[50].ship_scout_drones = 0;
                ship[50].ship_mines = 15;
                ship[50].ship_combat_drones = 5;
                ship[50].ship_level_needed = 0;
                ship[50].ship_restrictions = 0;
                ship[50].sold_at = "";

                ship[51].name = "Predator";
                ship[51].ship_race = GetRaceObject(6);
                ship[51].ship_cost = 2823300;
                ship[51].ship_speed = 8;
                ship[51].ship_hardpoints = 4;
                ship[51].ship_power = 16;
                ship[51].ship_shields = 525;
                ship[51].ship_armor = 475;
                ship[51].ship_cargo = 30;
                ship[51].ship_CloakAble = false;
                ship[51].ship_DSAble = false;
                ship[51].ship_IGAble = true;
                ship[51].ship_JumpAble = false;
                ship[51].ship_ScanAble = true;
                ship[51].ship_scout_drones = 0;
                ship[51].ship_mines = 10;
                ship[51].ship_combat_drones = 10;
                ship[51].ship_level_needed = 0;
                ship[51].ship_restrictions = 0;
                ship[51].sold_at = "";

                ship[52].name = "Proto Carrier";
                ship[52].ship_race = GetRaceObject(5);
                ship[52].ship_cost = 553810;
                ship[52].ship_speed = 9;
                ship[52].ship_hardpoints = 2;
                ship[52].ship_power = 9;
                ship[52].ship_shields = 400;
                ship[52].ship_armor = 125;
                ship[52].ship_cargo = 120;
                ship[52].ship_CloakAble = false;
                ship[52].ship_DSAble = false;
                ship[52].ship_IGAble = false;
                ship[52].ship_JumpAble = false;
                ship[52].ship_ScanAble = false;
                ship[52].ship_scout_drones = 0;
                ship[52].ship_mines = 0;
                ship[52].ship_combat_drones = 10;
                ship[52].ship_level_needed = 0;
                ship[52].ship_restrictions = 0;
                ship[52].sold_at = "";

                ship[53].name = "Ravager";
                ship[53].ship_race = GetRaceObject(6);
                ship[53].ship_cost = 6707691;
                ship[53].ship_speed = 8;
                ship[53].ship_hardpoints = 5;
                ship[53].ship_power = 19;
                ship[53].ship_shields = 650;
                ship[53].ship_armor = 675;
                ship[53].ship_cargo = 30;
                ship[53].ship_CloakAble = false;
                ship[53].ship_DSAble = false;
                ship[53].ship_IGAble = true;
                ship[53].ship_JumpAble = false;
                ship[53].ship_ScanAble = true;
                ship[53].ship_scout_drones = 0;
                ship[53].ship_mines = 50;
                ship[53].ship_combat_drones = 25;
                ship[53].ship_level_needed = 0;
                ship[53].ship_restrictions = 0;
                ship[53].sold_at = "";

                ship[54].name = "Rebellious Child";
                ship[54].ship_race = GetRaceObject(5);
                ship[54].ship_cost = 273707;
                ship[54].ship_speed = 10;
                ship[54].ship_hardpoints = 1;
                ship[54].ship_power = 5;
                ship[54].ship_shields = 375;
                ship[54].ship_armor = 75;
                ship[54].ship_cargo = 30;
                ship[54].ship_CloakAble = false;
                ship[54].ship_DSAble = false;
                ship[54].ship_IGAble = false;
                ship[54].ship_JumpAble = true;
                ship[54].ship_ScanAble = true;
                ship[54].ship_scout_drones = 0;
                ship[54].ship_mines = 10;
                ship[54].ship_combat_drones = 50;
                ship[54].ship_level_needed = 0;
                ship[54].ship_restrictions = 0;
                ship[54].sold_at = "";

                ship[55].name = "Redeemer";
                ship[55].ship_race = GetRaceObject(0);
                ship[55].ship_cost = 0;
                ship[55].ship_speed = 9;
                ship[55].ship_hardpoints = 1;
                ship[55].ship_power = 5;
                ship[55].ship_shields = 100;
                ship[55].ship_armor = 100;
                ship[55].ship_cargo = 45;
                ship[55].ship_CloakAble = false;
                ship[55].ship_DSAble = false;
                ship[55].ship_IGAble = false;
                ship[55].ship_JumpAble = false;
                ship[55].ship_ScanAble = false;
                ship[55].ship_scout_drones = 0;
                ship[55].ship_mines = 0;
                ship[55].ship_combat_drones = 0;
                ship[55].ship_level_needed = 0;
                ship[55].ship_restrictions = 0;
                ship[55].sold_at = "";

                ship[56].name = "Renaissance";
                ship[56].ship_race = GetRaceObject(4);
                ship[56].ship_cost = 606664;
                ship[56].ship_speed = 8;
                ship[56].ship_hardpoints = 2;
                ship[56].ship_power = 9;
                ship[56].ship_shields = 300;
                ship[56].ship_armor = 250;
                ship[56].ship_cargo = 90;
                ship[56].ship_CloakAble = false;
                ship[56].ship_DSAble = false;
                ship[56].ship_IGAble = false;
                ship[56].ship_JumpAble = false;
                ship[56].ship_ScanAble = false;
                ship[56].ship_scout_drones = 0;
                ship[56].ship_mines = 5;
                ship[56].ship_combat_drones = 10;
                ship[56].ship_level_needed = 0;
                ship[56].ship_restrictions = 0;
                ship[56].sold_at = "";

                ship[57].name = "Resistance";
                ship[57].ship_race = GetRaceObject(8);
                ship[57].ship_cost = 1156265;
                ship[57].ship_speed = 8;
                ship[57].ship_hardpoints = 2;
                ship[57].ship_power = 9;
                ship[57].ship_shields = 300;
                ship[57].ship_armor = 250;
                ship[57].ship_cargo = 90;
                ship[57].ship_CloakAble = true;
                ship[57].ship_DSAble = false;
                ship[57].ship_IGAble = false;
                ship[57].ship_JumpAble = false;
                ship[57].ship_ScanAble = true;
                ship[57].ship_scout_drones = 0;
                ship[57].ship_mines = 10;
                ship[57].ship_combat_drones = 10;
                ship[57].ship_level_needed = 0;
                ship[57].ship_restrictions = 0;
                ship[57].sold_at = "";

                ship[58].name = "Retaliation";
                ship[58].ship_race = GetRaceObject(0);
                ship[58].ship_cost = 435900;
                ship[58].ship_speed = 9;
                ship[58].ship_hardpoints = 2;
                ship[58].ship_power = 9;
                ship[58].ship_shields = 150;
                ship[58].ship_armor = 200;
                ship[58].ship_cargo = 80;
                ship[58].ship_CloakAble = false;
                ship[58].ship_DSAble = true;
                ship[58].ship_IGAble = false;
                ship[58].ship_JumpAble = false;
                ship[58].ship_ScanAble = true;
                ship[58].ship_scout_drones = 0;
                ship[58].ship_mines = 5;
                ship[58].ship_combat_drones = 10;
                ship[58].ship_level_needed = 0;
                ship[58].ship_restrictions = 0;
                ship[58].sold_at = "";

                ship[59].name = "Retribution";
                ship[59].ship_race = GetRaceObject(0);
                ship[59].ship_cost = 3535800;
                ship[59].ship_speed = 9;
                ship[59].ship_hardpoints = 4;
                ship[59].ship_power = 16;
                ship[59].ship_shields = 300;
                ship[59].ship_armor = 300;
                ship[59].ship_cargo = 70;
                ship[59].ship_CloakAble = false;
                ship[59].ship_DSAble = true;
                ship[59].ship_IGAble = false;
                ship[59].ship_JumpAble = false;
                ship[59].ship_ScanAble = true;
                ship[59].ship_scout_drones = 0;
                ship[59].ship_mines = 10;
                ship[59].ship_combat_drones = 20;
                ship[59].ship_level_needed = 0;
                ship[59].ship_restrictions = 0;
                ship[59].sold_at = "";

                ship[60].name = "Rogue";
                ship[60].ship_race = GetRaceObject(8);
                ship[60].ship_cost = 2074860;
                ship[60].ship_speed = 10;
                ship[60].ship_hardpoints = 2;
                ship[60].ship_power = 9;
                ship[60].ship_shields = 425;
                ship[60].ship_armor = 550;
                ship[60].ship_cargo = 120;
                ship[60].ship_CloakAble = true;
                ship[60].ship_DSAble = false;
                ship[60].ship_IGAble = false;
                ship[60].ship_JumpAble = false;
                ship[60].ship_ScanAble = true;
                ship[60].ship_scout_drones = 0;
                ship[60].ship_mines = 50;
                ship[60].ship_combat_drones = 50;
                ship[60].ship_level_needed = 0;
                ship[60].ship_restrictions = 0;
                ship[60].sold_at = "";

                ship[61].name = "Slayer";
                ship[61].ship_race = GetRaceObject(1);
                ship[61].ship_cost = 0;
                ship[61].ship_speed = 10;
                ship[61].ship_hardpoints = 1;
                ship[61].ship_power = 5;
                ship[61].ship_shields = 1000;
                ship[61].ship_armor = 1000;
                ship[61].ship_cargo = 60;
                ship[61].ship_CloakAble = false;
                ship[61].ship_DSAble = false;
                ship[61].ship_IGAble = false;
                ship[61].ship_JumpAble = false;
                ship[61].ship_ScanAble = false;
                ship[61].ship_scout_drones = 0;
                ship[61].ship_mines = 0;
                ship[61].ship_combat_drones = 0;
                ship[61].ship_level_needed = 0;
                ship[61].ship_restrictions = 1;
                ship[61].sold_at = "";

                ship[62].name = "Slip Freighter";
                ship[62].ship_race = GetRaceObject(8);
                ship[62].ship_cost = 0;
                ship[62].ship_speed = 9;
                ship[62].ship_hardpoints = 1;
                ship[62].ship_power = 5;
                ship[62].ship_shields = 150;
                ship[62].ship_armor = 150;
                ship[62].ship_cargo = 60;
                ship[62].ship_CloakAble = false;
                ship[62].ship_DSAble = false;
                ship[62].ship_IGAble = false;
                ship[62].ship_JumpAble = false;
                ship[62].ship_ScanAble = false;
                ship[62].ship_scout_drones = 0;
                ship[62].ship_mines = 10;
                ship[62].ship_combat_drones = 5;
                ship[62].ship_level_needed = 0;
                ship[62].ship_restrictions = 0;
                ship[62].sold_at = "";

                ship[63].name = "Small Escort";
                ship[63].ship_race = GetRaceObject(1);
                ship[63].ship_cost = 520821;
                ship[63].ship_speed = 10;
                ship[63].ship_hardpoints = 2;
                ship[63].ship_power = 9;
                ship[63].ship_shields = 300;
                ship[63].ship_armor = 250;
                ship[63].ship_cargo = 30;
                ship[63].ship_CloakAble = false;
                ship[63].ship_DSAble = false;
                ship[63].ship_IGAble = false;
                ship[63].ship_JumpAble = false;
                ship[63].ship_ScanAble = false;
                ship[63].ship_scout_drones = 0;
                ship[63].ship_mines = 0;
                ship[63].ship_combat_drones = 0;
                ship[63].ship_level_needed = 0;
                ship[63].ship_restrictions = 0;
                ship[63].sold_at = "";

                ship[64].name = "Small-Timer";
                ship[64].ship_race = GetRaceObject(2);
                ship[64].ship_cost = 0;
                ship[64].ship_speed = 9;
                ship[64].ship_hardpoints = 1;
                ship[64].ship_power = 5;
                ship[64].ship_shields = 150;
                ship[64].ship_armor = 150;
                ship[64].ship_cargo = 60;
                ship[64].ship_CloakAble = false;
                ship[64].ship_DSAble = false;
                ship[64].ship_IGAble = false;
                ship[64].ship_JumpAble = false;
                ship[64].ship_ScanAble = false;
                ship[64].ship_scout_drones = 0;
                ship[64].ship_mines = 0;
                ship[64].ship_combat_drones = 0;
                ship[64].ship_level_needed = 0;
                ship[64].ship_restrictions = 0;
                ship[64].sold_at = "";

                ship[65].name = "Spooky Midnight Special";
                ship[65].ship_race = GetRaceObject(1);
                ship[65].ship_cost = 0;
                ship[65].ship_speed = 10;
                ship[65].ship_hardpoints = 10;
                ship[65].ship_power = 30;
                ship[65].ship_shields = 50000;
                ship[65].ship_armor = 21000;
                ship[65].ship_cargo = 1000;
                ship[65].ship_CloakAble = true;
                ship[65].ship_DSAble = false;
                ship[65].ship_IGAble = true;
                ship[65].ship_JumpAble = true;
                ship[65].ship_ScanAble = true;
                ship[65].ship_scout_drones = 0;
                ship[65].ship_mines = 1000;
                ship[65].ship_combat_drones = 1000;
                ship[65].ship_level_needed = 0;
                ship[65].ship_restrictions = 0;
                ship[65].sold_at = "";

                ship[66].name = "Star Ranger";
                ship[66].ship_race = GetRaceObject(7);
                ship[66].ship_cost = 130923;
                ship[66].ship_speed = 12;
                ship[66].ship_hardpoints = 1;
                ship[66].ship_power = 5;
                ship[66].ship_shields = 225;
                ship[66].ship_armor = 200;
                ship[66].ship_cargo = 0;
                ship[66].ship_CloakAble = false;
                ship[66].ship_DSAble = false;
                ship[66].ship_IGAble = false;
                ship[66].ship_JumpAble = false;
                ship[66].ship_ScanAble = true;
                ship[66].ship_scout_drones = 0;
                ship[66].ship_mines = 0;
                ship[66].ship_combat_drones = 10;
                ship[66].ship_level_needed = 0;
                ship[66].ship_restrictions = 0;
                ship[66].sold_at = "";

                ship[67].name = "Stellar Freighter";
                ship[67].ship_race = GetRaceObject(1);
                ship[67].ship_cost = 3508455;
                ship[67].ship_speed = 7;
                ship[67].ship_hardpoints = 1;
                ship[67].ship_power = 5;
                ship[67].ship_shields = 250;
                ship[67].ship_armor = 400;
                ship[67].ship_cargo = 210;
                ship[67].ship_CloakAble = true;
                ship[67].ship_DSAble = false;
                ship[67].ship_IGAble = false;
                ship[67].ship_JumpAble = false;
                ship[67].ship_ScanAble = true;
                ship[67].ship_scout_drones = 0;
                ship[67].ship_mines = 15;
                ship[67].ship_combat_drones = 10;
                ship[67].ship_level_needed = 0;
                ship[67].ship_restrictions = 0;
                ship[67].sold_at = "";

                ship[68].name = "Swift Venture";
                ship[68].ship_race = GetRaceObject(7);
                ship[68].ship_cost = 0;
                ship[68].ship_speed = 9;
                ship[68].ship_hardpoints = 1;
                ship[68].ship_power = 5;
                ship[68].ship_shields = 150;
                ship[68].ship_armor = 150;
                ship[68].ship_cargo = 60;
                ship[68].ship_CloakAble = false;
                ship[68].ship_DSAble = false;
                ship[68].ship_IGAble = false;
                ship[68].ship_JumpAble = false;
                ship[68].ship_ScanAble = false;
                ship[68].ship_scout_drones = 0;
                ship[68].ship_mines = 0;
                ship[68].ship_combat_drones = 0;
                ship[68].ship_level_needed = 0;
                ship[68].ship_restrictions = 0;
                ship[68].sold_at = "";

                ship[69].name = "Thief";
                ship[69].ship_race = GetRaceObject(1);
                ship[69].ship_cost = 16802157;
                ship[69].ship_speed = 12;
                ship[69].ship_hardpoints = 3;
                ship[69].ship_power = 13;
                ship[69].ship_shields = 250;
                ship[69].ship_armor = 300;
                ship[69].ship_cargo = 150;
                ship[69].ship_CloakAble = true;
                ship[69].ship_DSAble = false;
                ship[69].ship_IGAble = false;
                ship[69].ship_JumpAble = false;
                ship[69].ship_ScanAble = true;
                ship[69].ship_scout_drones = 0;
                ship[69].ship_mines = 5;
                ship[69].ship_combat_drones = 0;
                ship[69].ship_level_needed = 0;
                ship[69].ship_restrictions = 2;
                ship[69].sold_at = "";

                ship[70].name = "Tiny Delight";
                ship[70].ship_race = GetRaceObject(5);
                ship[70].ship_cost = 0;
                ship[70].ship_speed = 8;
                ship[70].ship_hardpoints = 1;
                ship[70].ship_power = 5;
                ship[70].ship_shields = 150;
                ship[70].ship_armor = 175;
                ship[70].ship_cargo = 60;
                ship[70].ship_CloakAble = false;
                ship[70].ship_DSAble = false;
                ship[70].ship_IGAble = false;
                ship[70].ship_JumpAble = false;
                ship[70].ship_ScanAble = false;
                ship[70].ship_scout_drones = 0;
                ship[70].ship_mines = 0;
                ship[70].ship_combat_drones = 0;
                ship[70].ship_level_needed = 0;
                ship[70].ship_restrictions = 0;
                ship[70].sold_at = "";

                ship[71].name = "Trade-Master";
                ship[71].ship_race = GetRaceObject(2);
                ship[71].ship_cost = 8095764;
                ship[71].ship_speed = 6;
                ship[71].ship_hardpoints = 1;
                ship[71].ship_power = 5;
                ship[71].ship_shields = 675;
                ship[71].ship_armor = 525;
                ship[71].ship_cargo = 425;
                ship[71].ship_CloakAble = false;
                ship[71].ship_DSAble = false;
                ship[71].ship_IGAble = true;
                ship[71].ship_JumpAble = false;
                ship[71].ship_ScanAble = true;
                ship[71].ship_scout_drones = 0;
                ship[71].ship_mines = 100;
                ship[71].ship_combat_drones = 25;
                ship[71].ship_level_needed = 0;
                ship[71].ship_restrictions = 0;
                ship[71].sold_at = "";

                ship[72].name = "Trip-Maker";
                ship[72].ship_race = GetRaceObject(2);
                ship[72].ship_cost = 1354955;
                ship[72].ship_speed = 9;
                ship[72].ship_hardpoints = 1;
                ship[72].ship_power = 5;
                ship[72].ship_shields = 400;
                ship[72].ship_armor = 550;
                ship[72].ship_cargo = 200;
                ship[72].ship_CloakAble = false;
                ship[72].ship_DSAble = false;
                ship[72].ship_IGAble = false;
                ship[72].ship_JumpAble = false;
                ship[72].ship_ScanAble = true;
                ship[72].ship_scout_drones = 0;
                ship[72].ship_mines = 0;
                ship[72].ship_combat_drones = 0;
                ship[72].ship_level_needed = 0;
                ship[72].ship_restrictions = 0;
                ship[72].sold_at = "";

                ship[73].name = "Unarmed Scout";
                ship[73].ship_race = GetRaceObject(1);
                ship[73].ship_cost = 125251;
                ship[73].ship_speed = 11;
                ship[73].ship_hardpoints = 0;
                ship[73].ship_power = 0;
                ship[73].ship_shields = 150;
                ship[73].ship_armor = 75;
                ship[73].ship_cargo = 30;
                ship[73].ship_CloakAble = false;
                ship[73].ship_DSAble = false;
                ship[73].ship_IGAble = false;
                ship[73].ship_JumpAble = false;
                ship[73].ship_ScanAble = true;
                ship[73].ship_scout_drones = 0;
                ship[73].ship_mines = 0;
                ship[73].ship_combat_drones = 0;
                ship[73].ship_level_needed = 0;
                ship[73].ship_restrictions = 0;
                ship[73].sold_at = "";

                ship[74].name = "Vengeance";
                ship[74].ship_race = GetRaceObject(0);
                ship[74].ship_cost = 1137543;
                ship[74].ship_speed = 8;
                ship[74].ship_hardpoints = 2;
                ship[74].ship_power = 9;
                ship[74].ship_shields = 300;
                ship[74].ship_armor = 300;
                ship[74].ship_cargo = 170;
                ship[74].ship_CloakAble = false;
                ship[74].ship_DSAble = true;
                ship[74].ship_IGAble = false;
                ship[74].ship_JumpAble = false;
                ship[74].ship_ScanAble = true;
                ship[74].ship_scout_drones = 0;
                ship[74].ship_mines = 0;
                ship[74].ship_combat_drones = 5;
                ship[74].ship_level_needed = 0;
                ship[74].ship_restrictions = 0;
                ship[74].sold_at = "";

                ship[75].name = "Vindicator";
                ship[75].ship_race = GetRaceObject(0);
                ship[75].ship_cost = 7600000;
                ship[75].ship_speed = 8;
                ship[75].ship_hardpoints = 6;
                ship[75].ship_power = 22;
                ship[75].ship_shields = 425;
                ship[75].ship_armor = 400;
                ship[75].ship_cargo = 50;
                ship[75].ship_CloakAble = false;
                ship[75].ship_DSAble = true;
                ship[75].ship_IGAble = false;
                ship[75].ship_JumpAble = false;
                ship[75].ship_ScanAble = true;
                ship[75].ship_scout_drones = 0;
                ship[75].ship_mines = 75;
                ship[75].ship_combat_drones = 25;
                ship[75].ship_level_needed = 0;
                ship[75].ship_restrictions = 0;
                ship[75].sold_at = "";

                ship[76].name = "Watchful Eye";
                ship[76].ship_race = GetRaceObject(6);
                ship[76].ship_cost = 135366;
                ship[76].ship_speed = 10;
                ship[76].ship_hardpoints = 1;
                ship[76].ship_power = 5;
                ship[76].ship_shields = 75;
                ship[76].ship_armor = 125;
                ship[76].ship_cargo = 40;
                ship[76].ship_CloakAble = false;
                ship[76].ship_DSAble = false;
                ship[76].ship_IGAble = true;
                ship[76].ship_JumpAble = false;
                ship[76].ship_ScanAble = true;
                ship[76].ship_scout_drones = 100;
                ship[76].ship_mines = 0;
                ship[76].ship_combat_drones = 0;
                ship[76].ship_level_needed = 0;
                ship[76].ship_restrictions = 0;
                ship[76].sold_at = "";
            }
#endregion
        }

        public void InitializeLocations(ushort number, bool loaddefaultlocations)
	//This function makes sure that memory is allocated for the given number of locations.
        {
            if (loaddefaultlocations)
                nroflocations = 89;
            else
                nroflocations = number;

            location = new Location[nroflocations + 1];
            location[0] = new Location();
            location[0].location_name = "Nothing";
            for (int i = 1; i < nroflocations + 1; i++)
                location[i] = new Location();

            if(loaddefaultlocations)
            {
#region Load Default Locations

                location[1].location_name = "Federal Headquarters";
                location[1].location_type = "HQ";
                location[1].nrofsolditems = 0;

                location[2].location_image = "";
                location[2].location_name = "Underground Headquarters";
                location[2].location_type = "UG";
                location[2].nrofsolditems = 0;

                location[3].location_image = "";
                location[3].location_name = "Alskant Headquarters";
                location[3].location_type = "HQ";
                location[3].nrofsolditems = 0;

                location[4].location_image = "";
                location[4].location_name = "Creonti Headquarters";
                location[4].location_type = "HQ";
                location[4].nrofsolditems = 0;

                location[5].location_image = "";
                location[5].location_name = "Human Headquarters";
                location[5].location_type = "HQ";
                location[5].nrofsolditems = 0;

                location[6].location_image = "";
                location[6].location_name = "Ik'Thorne Headquarters";
                location[6].location_type = "HQ";
                location[6].nrofsolditems = 0;

                location[7].location_image = "";
                location[7].location_name = "Salvene Headquarters";
                location[7].location_type = "HQ";
                location[7].nrofsolditems = 0;

                location[8].location_image = "";
                location[8].location_name = "Thevian Headquarters";
                location[8].location_type = "HQ";
                location[8].nrofsolditems = 0;

                location[9].location_image = "";
                location[9].location_name = "WQ Human Headquarters";
                location[9].location_type = "HQ";
                location[9].nrofsolditems = 0;

                location[10].location_image = "";
                location[10].location_name = "Nijarin Headquarters";
                location[10].location_type = "HQ";
                location[10].nrofsolditems = 0;

                location[11].location_image = "";
                location[11].location_name = "Federal Beacon";
                location[11].location_type = "Fed";
                location[11].nrofsolditems = 0;

                location[12].location_image = "";
                location[12].location_name = "Alskant Beacon";
                location[12].location_type = "Fed";
                location[12].nrofsolditems = 0;

                location[13].location_image = "";
                location[13].location_name = "Creonti Beacon";
                location[13].location_type = "Fed";
                location[13].nrofsolditems = 0;

                location[14].location_image = "";
                location[14].location_name = "Human Beacon";
                location[14].location_type = "Fed";
                location[14].nrofsolditems = 0;

                location[15].location_image = "";
                location[15].location_name = "Ik'Thorne Beacon";
                location[15].location_type = "Fed";
                location[15].nrofsolditems = 0;

                location[16].location_image = "";
                location[16].location_name = "Salvene Beacon";
                location[16].location_type = "Fed";
                location[16].nrofsolditems = 0;

                location[17].location_image = "";
                location[17].location_name = "Thevian Beacon";
                location[17].location_type = "Fed";
                location[17].nrofsolditems = 0;

                location[18].location_image = "";
                location[18].location_name = "WQ Human Beacon";
                location[18].location_type = "Fed";
                location[18].nrofsolditems = 0;

                location[19].location_image = "";
                location[19].location_name = "Nijarin Beacon";
                location[19].location_type = "Fed";
                location[19].nrofsolditems = 0;

                location[20].location_image = "";
                location[20].location_name = "Advanced Missile Concepts 1";
                location[20].location_type = "Weapon Shop";
                location[20].nrofsolditems = 5;
                location[20].sells = new int[5];
                location[20].sells[0] = 6;
                location[20].sells[1] = 12;
                location[20].sells[2] = 13;
                location[20].sells[3] = 14;
                location[20].sells[4] = 32;

                location[21].location_image = "";
                location[21].location_name = "Advanced Missile Concepts 2";
                location[21].location_type = "Weapon Shop";
                location[21].nrofsolditems = 4;
                location[21].sells = new int[4];
                location[21].sells[0] = 6;
                location[21].sells[1] = 10;
                location[21].sells[2] = 14;
                location[21].sells[3] = 33;

                location[22].location_image = "";
                location[22].location_name = "Big Guns Inc.";
                location[22].location_type = "Weapon Shop";
                location[22].nrofsolditems = 2;
                location[22].sells = new int[2];
                location[22].sells[0] = 35;
                location[22].sells[1] = 38;

                location[23].location_image = "";
                location[23].location_name = "Cannon-O-Rama";
                location[23].location_type = "Weapon Shop";
                location[23].nrofsolditems = 3;
                location[23].sells = new int[3];
                location[23].sells[0] = 16;
                location[23].sells[1] = 18;
                location[23].sells[2] = 40;

                location[24].location_image = "";
                location[24].location_name = "Creonti Weapons 1";
                location[24].location_type = "Weapon Shop";
                location[24].nrofsolditems = 2;
                location[24].sells = new int[2];
                location[24].sells[0] = 26;
                location[24].sells[1] = 27;

                location[25].location_image = "";
                location[25].location_name = "Creonti Weapons 2";
                location[25].location_type = "Weapon Shop";
                location[25].nrofsolditems = 2;
                location[25].sells = new int[2];
                location[25].sells[0] = 26;
                location[25].sells[1] = 48;

                location[26].location_image = "";
                location[26].location_name = "Flux Systems - Weaponry Division";
                location[26].location_type = "Weapon Shop";
                location[26].nrofsolditems = 2;
                location[26].sells = new int[2];
                location[26].sells[0] = 8;
                location[26].sells[1] = 9;

                location[27].location_image = "";
                location[27].location_name = "Land of Lasers";
                location[27].location_type = "Weapon Shop";
                location[27].nrofsolditems = 5;
                location[27].sells = new int[5];
                location[27].sells[0] = 20;
                location[27].sells[1] = 21;
                location[27].sells[2] = 28;
                location[27].sells[3] = 36;
                location[27].sells[4] = 44;

                location[28].location_image = "";
                location[28].location_name = "Monastery of the Iron Maiden";
                location[28].location_type = "Weapon Shop";
                location[28].nrofsolditems = 1;
                location[28].sells = new int[1];
                location[28].sells[0] = 3;

                location[29].location_image = "";
                location[29].location_name = "No Shields Inc 1";
                location[29].location_type = "Weapon Shop";
                location[29].nrofsolditems = 3;
                location[29].sells = new int[3];
                location[29].sells[0] = 17;
                location[29].sells[1] = 31;
                location[29].sells[2] = 42;

                location[30].location_image = "";
                location[30].location_name = "No Shields Inc 2";
                location[30].location_type = "Weapon Shop";
                location[30].nrofsolditems = 3;
                location[30].sells = new int[3];
                location[30].sells[0] = 7;
                location[30].sells[1] = 14;
                location[30].sells[2] = 42;

                location[31].location_image = "";
                location[31].location_name = "Rapid-Fire Sales Office";
                location[31].location_type = "Weapon Shop";
                location[31].nrofsolditems = 2;
                location[31].sells = new int[2];
                location[31].sells[0] = 12;
                location[31].sells[1] = 13;

                location[32].location_image = "";
                location[32].location_name = "Shotgun Shack";
                location[32].location_type = "Weapon Shop";
                location[32].nrofsolditems = 3;
                location[32].sells = new int[3];
                location[32].sells[0] = 4;
                location[32].sells[1] = 19;
                location[32].sells[2] = 47;

                location[33].location_image = "";
                location[33].location_name = "The General Store - Weapons Division 1";
                location[33].location_type = "Weapon Shop";
                location[33].nrofsolditems = 4;
                location[33].sells = new int[4];
                location[33].sells[0] = 40;
                location[33].sells[1] = 43;
                location[33].sells[2] = 46;
                location[33].sells[3] = 50;

                location[34].location_image = "";
                location[34].location_name = "The General Store - Weapons Division 2";
                location[34].location_type = "Weapon Shop";
                location[34].nrofsolditems = 4;
                location[34].sells = new int[4];
                location[34].sells[0] = 34;
                location[34].sells[1] = 37;
                location[34].sells[2] = 41;
                location[34].sells[3] = 47;

                location[35].location_image = "";
                location[35].location_name = "The General Store - Weapons Division 3";
                location[35].location_type = "Weapon Shop";
                location[35].nrofsolditems = 3;
                location[35].sells = new int[3];
                location[35].sells[0] = 33;
                location[35].sells[1] = 34;
                location[35].sells[2] = 45;

                location[36].location_image = "";
                location[36].location_name = "The General Store - Weapons Division 4";
                location[36].location_type = "Weapon Shop";
                location[36].nrofsolditems = 3;
                location[36].sells = new int[3];
                location[36].sells[0] = 34;
                location[36].sells[1] = 39;
                location[36].sells[2] = 50;

                location[37].location_image = "";
                location[37].location_name = "The One-Stop-Weapons-Shop 1";
                location[37].location_type = "Weapon Shop";
                location[37].nrofsolditems = 3;
                location[37].sells = new int[3];
                location[37].sells[0] = 11;
                location[37].sells[1] = 25;
                location[37].sells[2] = 29;

                location[38].location_image = "";
                location[38].location_name = "The One-Stop-Weapons-Shop 2";
                location[38].location_type = "Weapon Shop";
                location[38].nrofsolditems = 4;
                location[38].sells = new int[4];
                location[38].sells[0] = 5;
                location[38].sells[1] = 8;
                location[38].sells[2] = 33;
                location[38].sells[3] = 37;

                location[39].location_image = "";
                location[39].location_name = "The One-Stop-Weapons-Shop 3";
                location[39].location_type = "Weapon Shop";
                location[39].nrofsolditems = 4;
                location[39].sells = new int[4];
                location[39].sells[0] = 30;
                location[39].sells[1] = 34;
                location[39].sells[2] = 47;
                location[39].sells[3] = 49;

                location[40].location_image = "";
                location[40].location_name = "Torpedo Outlet";
                location[40].location_type = "Weapon Shop";
                location[40].nrofsolditems = 4;
                location[40].sells = new int[4];
                location[40].sells[0] = 22;
                location[40].sells[1] = 23;
                location[40].sells[2] = 25;
                location[40].sells[3] = 50;

                location[41].location_image = "";
                location[41].location_name = "Underground Weapons";
                location[41].location_type = "Weapon Shop";
                location[41].nrofsolditems = 1;
                location[41].sells = new int[1];
                location[41].sells[0] = 2;

                location[42].location_image = "";
                location[42].location_name = "Check Your Pulse";
                location[42].location_type = "Weapon Shop";
                location[42].nrofsolditems = 1;
                location[42].sells = new int[1];
                location[42].sells[0] = 1;

                location[43].location_image = "";
                location[43].location_name = "Nijarin Weaponry";
                location[43].location_type = "Weapon Shop";
                location[43].nrofsolditems = 4;
                location[43].sells = new int[4];
                location[43].sells[0] = 51;
                location[43].sells[1] = 52;
                location[43].sells[2] = 53;
                location[43].sells[3] = 54;

                location[44].location_image = "";
                location[44].location_name = "Pulse of the Universe";
                location[44].location_type = "Weapon Shop";
                location[44].nrofsolditems = 1;
                location[44].sells = new int[1];
                location[44].sells[0] = 55;

                location[45].location_image = "";
                location[45].location_name = "Race Wars Weapons";
                location[45].location_type = "Weapon Shop";
                location[45].nrofsolditems = 55;
                location[45].sells = new int[55];
                location[45].sells[0] = 1;
                location[45].sells[1] = 2;
                location[45].sells[2] = 3;
                location[45].sells[3] = 4;
                location[45].sells[4] = 5;
                location[45].sells[5] = 6;
                location[45].sells[6] = 7;
                location[45].sells[7] = 8;
                location[45].sells[8] = 9;
                location[45].sells[9] = 10;
                location[45].sells[10] = 11;
                location[45].sells[11] = 12;
                location[45].sells[12] = 13;
                location[45].sells[13] = 14;
                location[45].sells[14] = 15;
                location[45].sells[15] = 16;
                location[45].sells[16] = 17;
                location[45].sells[17] = 18;
                location[45].sells[18] = 19;
                location[45].sells[19] = 20;
                location[45].sells[20] = 21;
                location[45].sells[21] = 22;
                location[45].sells[22] = 23;
                location[45].sells[23] = 24;
                location[45].sells[24] = 25;
                location[45].sells[25] = 26;
                location[45].sells[26] = 27;
                location[45].sells[27] = 28;
                location[45].sells[28] = 29;
                location[45].sells[29] = 30;
                location[45].sells[30] = 31;
                location[45].sells[31] = 32;
                location[45].sells[32] = 33;
                location[45].sells[33] = 34;
                location[45].sells[34] = 35;
                location[45].sells[35] = 36;
                location[45].sells[36] = 37;
                location[45].sells[37] = 38;
                location[45].sells[38] = 39;
                location[45].sells[39] = 40;
                location[45].sells[40] = 41;
                location[45].sells[41] = 42;
                location[45].sells[42] = 43;
                location[45].sells[43] = 44;
                location[45].sells[44] = 45;
                location[45].sells[45] = 46;
                location[45].sells[46] = 47;
                location[45].sells[47] = 48;
                location[45].sells[48] = 49;
                location[45].sells[49] = 50;
                location[45].sells[50] = 51;
                location[45].sells[51] = 52;
                location[45].sells[52] = 53;
                location[45].sells[53] = 54;
                location[45].sells[54] = 55;

                location[46].location_image = "";
                location[46].location_name = "Alskant Ship Dealer";
                location[46].location_type = "Ship Shop";
                location[46].nrofsolditems = 5;
                location[46].sells = new int[5];
                location[46].sells[0] = 64;
                location[46].sells[1] = 72;
                location[46].sells[2] = 16;
                location[46].sells[3] = 18;
                location[46].sells[4] = 71;

                location[47].location_image = "";
                location[47].location_name = "Creonti Ship Dealer";
                location[47].location_type = "Ship Shop";
                location[47].nrofsolditems = 5;
                location[47].sells = new int[5];
                location[47].sells[0] = 41;
                location[47].sells[1] = 36;
                location[47].sells[2] = 33;
                location[47].sells[3] = 35;
                location[47].sells[4] = 21;

                location[48].location_image = "";
                location[48].location_name = "Human Ship Dealer";
                location[48].location_type = "Ship Shop";
                location[48].nrofsolditems = 5;
                location[48].sells = new int[5];
                location[48].sells[0] = 40;
                location[48].sells[1] = 3;
                location[48].sells[2] = 56;
                location[48].sells[3] = 9;
                location[48].sells[4] = 20;

                location[49].location_image = "";
                location[49].location_name = "Ik'Thorne Ship Dealer";
                location[49].location_type = "Ship Shop";
                location[49].nrofsolditems = 6;
                location[49].sells = new int[6];
                location[49].sells[0] = 70;
                location[49].sells[1] = 54;
                location[49].sells[2] = 26;
                location[49].sells[3] = 52;
                location[49].sells[4] = 1;
                location[49].sells[5] = 45;

                location[50].location_image = "";
                location[50].location_name = "Salvene Ship Dealer";
                location[50].location_type = "Ship Shop";
                location[50].nrofsolditems = 6;
                location[50].sells = new int[6];
                location[50].sells[0] = 0;
                location[50].sells[1] = 22;
                location[50].sells[2] = 76;
                location[50].sells[3] = 51;
                location[50].sells[4] = 53;
                location[50].sells[5] = 23;

                location[51].location_image = "";
                location[51].location_name = "Thevian Ship Dealer";
                location[51].location_type = "Ship Shop";
                location[51].nrofsolditems = 6;
                location[51].sells = new int[6];
                location[51].sells[0] = 68;
                location[51].sells[1] = 25;
                location[51].sells[2] = 66;
                location[51].sells[3] = 10;
                location[51].sells[4] = 11;
                location[51].sells[5] = 6;

                location[52].location_image = "";
                location[52].location_name = "WQ Human Ship Dealer";
                location[52].location_type = "Ship Shop";
                location[52].nrofsolditems = 6;
                location[52].sells = new int[6];
                location[52].sells[0] = 62;
                location[52].sells[1] = 46;
                location[52].sells[2] = 57;
                location[52].sells[3] = 60;
                location[52].sells[4] = 8;
                location[52].sells[5] = 15;

                location[53].location_image = "";
                location[53].location_name = "Nijarin Ship Dealer";
                location[53].location_type = "Ship Shop";
                location[53].nrofsolditems = 6;
                location[53].sells = new int[6];
                location[53].sells[0] = 55;
                location[53].sells[1] = 58;
                location[53].sells[2] = 74;
                location[53].sells[3] = 59;
                location[53].sells[4] = 75;
                location[53].sells[5] = 31;

                location[54].location_image = "";
                location[54].location_name = "Cheap Used Ships";
                location[54].location_type = "Ship Shop";
                location[54].nrofsolditems = 6;
                location[54].sells = new int[6];
                location[54].sells[0] = 32;
                location[54].sells[1] = 4;
                location[54].sells[2] = 14;
                location[54].sells[3] = 30;
                location[54].sells[4] = 73;
                location[54].sells[5] = 63;

                location[55].location_image = "";
                location[55].location_name = "Cross World Transit Ships";
                location[55].location_type = "Ship Shop";
                location[55].nrofsolditems = 3;
                location[55].sells = new int[3];
                location[55].sells[0] = 38;
                location[55].sells[1] = 2;
                location[55].sells[2] = 34;

                location[56].location_image = "";
                location[56].location_name = "Cruiser Central";
                location[56].location_type = "Ship Shop";
                location[56].nrofsolditems = 3;
                location[56].sells = new int[3];
                location[56].sells[0] = 39;
                location[56].sells[1] = 43;
                location[56].sells[2] = 7;

                location[57].location_image = "";
                location[57].location_name = "Federation Shipyard";
                location[57].location_type = "Ship Shop";
                location[57].nrofsolditems = 3;
                location[57].sells = new int[3];
                location[57].sells[0] = 27;
                location[57].sells[1] = 29;
                location[57].sells[2] = 28;

                location[58].location_image = "";
                location[58].location_name = "Huge Ship Central";
                location[58].location_type = "Ship Shop";
                location[58].nrofsolditems = 4;
                location[58].sells = new int[4];
                location[58].sells[0] = 34;
                location[58].sells[1] = 30;
                location[58].sells[2] = 48;
                location[58].sells[3] = 49;

                location[59].location_image = "";
                location[59].location_name = "Military Vehicle Outlet";
                location[59].location_type = "Ship Shop";
                location[59].nrofsolditems = 3;
                location[59].sells = new int[3];
                location[59].sells[0] = 7;
                location[59].sells[1] = 13;
                location[59].sells[2] = 12;

                location[60].location_image = "";
                location[60].location_name = "Refurbished Ships";
                location[60].location_type = "Ship Shop";
                location[60].nrofsolditems = 4;
                location[60].sells = new int[4];
                location[60].sells[0] = 32;
                location[60].sells[1] = 44;
                location[60].sells[2] = 37;
                location[60].sells[3] = 42;

                location[61].location_image = "";
                location[61].location_name = "Ship-O-Rama";
                location[61].location_type = "Ship Shop";
                location[61].nrofsolditems = 5;
                location[61].sells = new int[5];
                location[61].sells[0] = 4;
                location[61].sells[1] = 50;
                location[61].sells[2] = 67;
                location[61].sells[3] = 73;
                location[61].sells[4] = 7;

                location[62].location_image = "";
                location[62].location_name = "The Smuggler's Craft";
                location[62].location_type = "Ship Shop";
                location[62].nrofsolditems = 3;
                location[62].sells = new int[3];
                location[62].sells[0] = 69;
                location[62].sells[1] = 5;
                location[62].sells[2] = 17;

                location[63].location_image = "";
                location[63].location_name = "Test Shipyard";
                location[63].location_type = "Ship Shop";
                location[63].nrofsolditems = 1;
                location[63].sells = new int[1];
                location[63].sells[0] = 65;

                location[64].location_image = "";
                location[64].location_name = "Semi Wars";
                location[64].location_type = "Ship Shop";
                location[64].nrofsolditems = 1;
                location[64].sells = new int[1];
                location[64].sells[0] = 32;

                location[65].location_image = "";
                location[65].location_name = "Race Wars Ships";
                location[65].location_type = "Ship Shop";
                location[65].nrofsolditems = 74;
                location[65].sells = new int[74];
                location[65].sells[0] = 32;
                location[65].sells[1] = 4;
                location[65].sells[2] = 14;
                location[65].sells[3] = 44;
                location[65].sells[4] = 50;
                location[65].sells[5] = 67;
                location[65].sells[6] = 38;
                location[65].sells[7] = 2;
                location[65].sells[8] = 34;
                location[65].sells[9] = 30;
                location[65].sells[10] = 48;
                location[65].sells[11] = 49;
                location[65].sells[12] = 73;
                location[65].sells[13] = 63;
                location[65].sells[14] = 39;
                location[65].sells[15] = 43;
                location[65].sells[16] = 7;
                location[65].sells[17] = 13;
                location[65].sells[18] = 12;
                location[65].sells[19] = 27;
                location[65].sells[20] = 29;
                location[65].sells[21] = 28;
                location[65].sells[22] = 69;
                location[65].sells[23] = 5;
                location[65].sells[24] = 17;
                location[65].sells[25] = 37;
                location[65].sells[26] = 42;
                location[65].sells[27] = 47;
                location[65].sells[28] = 64;
                location[65].sells[29] = 72;
                location[65].sells[30] = 16;
                location[65].sells[31] = 18;
                location[65].sells[32] = 71;
                location[65].sells[33] = 41;
                location[65].sells[34] = 36;
                location[65].sells[35] = 33;
                location[65].sells[36] = 35;
                location[65].sells[37] = 21;
                location[65].sells[38] = 40;
                location[65].sells[39] = 3;
                location[65].sells[40] = 56;
                location[65].sells[41] = 9;
                location[65].sells[42] = 20;
                location[65].sells[43] = 70;
                location[65].sells[44] = 54;
                location[65].sells[45] = 26;
                location[65].sells[46] = 52;
                location[65].sells[47] = 1;
                location[65].sells[48] = 45;
                location[65].sells[49] = 0;
                location[65].sells[50] = 22;
                location[65].sells[51] = 76;
                location[65].sells[52] = 51;
                location[65].sells[53] = 53;
                location[65].sells[54] = 23;
                location[65].sells[55] = 68;
                location[65].sells[56] = 25;
                location[65].sells[57] = 66;
                location[65].sells[58] = 10;
                location[65].sells[59] = 11;
                location[65].sells[60] = 6;
                location[65].sells[61] = 62;
                location[65].sells[62] = 46;
                location[65].sells[63] = 57;
                location[65].sells[64] = 60;
                location[65].sells[65] = 8;
                location[65].sells[66] = 15;
                location[65].sells[67] = 24;
                location[65].sells[68] = 55;
                location[65].sells[69] = 58;
                location[65].sells[70] = 74;
                location[65].sells[71] = 59;
                location[65].sells[72] = 75;
                location[65].sells[73] = 31;

                location[66].location_image = "";
                location[66].location_name = "Uno Hardware";
                location[66].location_type = "Technology Shop";
                location[66].nrofsolditems = 3;
                location[66].sells = new int[3];
                location[66].sells[0] = 0;
                location[66].sells[1] = 1;
                location[66].sells[2] = 2;

                location[67].location_image = "";
                location[67].location_name = "Combat Accessories";
                location[67].location_type = "Technology Shop";
                location[67].nrofsolditems = 3;
                location[67].sells = new int[3];
                location[67].sells[0] = 3;
                location[67].sells[1] = 4;
                location[67].sells[2] = 5;

                location[68].location_image = "";
                location[68].location_name = "Accelerated Systems";
                location[68].location_type = "Technology Shop";
                location[68].nrofsolditems = 1;
                location[68].sells = new int[1];
                location[68].sells[0] = 9;

                location[69].location_image = "";
                location[69].location_name = "Advanced Communications";
                location[69].location_type = "Technology Shop";
                location[69].nrofsolditems = 1;
                location[69].sells = new int[1];
                location[69].sells[0] = 6;

                location[70].location_image = "";
                location[70].location_name = "Hidden Technology";
                location[70].location_type = "Technology Shop";
                location[70].nrofsolditems = 1;
                location[70].sells = new int[1];
                location[70].sells[0] = 7;

                location[71].location_image = "";
                location[71].location_name = "Image Systems Inc";
                location[71].location_type = "Technology Shop";
                location[71].nrofsolditems = 1;
                location[71].sells = new int[1];
                location[71].sells[0] = 8;

                location[72].location_image = "";
                location[72].location_name = "Race Wars Hardware";
                location[72].location_type = "Technology Shop";
                location[72].nrofsolditems = 11;
                location[72].sells = new int[11];
                location[72].sells[0] = 0;
                location[72].sells[1] = 1;
                location[72].sells[2] = 2;
                location[72].sells[3] = 3;
                location[72].sells[4] = 4;
                location[72].sells[5] = 5;
                location[72].sells[6] = 6;
                location[72].sells[7] = 7;
                location[72].sells[8] = 8;
                location[72].sells[9] = 9;
                location[72].sells[10] = 10;

                location[73].location_image = "";
                location[73].location_name = "Crone Dronfusion";
                location[73].location_type = "Technology Shop";
                location[73].nrofsolditems = 1;
                location[73].sells = new int[1];
                location[73].sells[0] = 10;

                location[74].location_image = "";
                location[74].location_name = "Bank of the Stars";
                location[74].location_type = "Bank";
                location[74].nrofsolditems = 0;

                location[75].location_image = "";
                location[75].location_name = "Last Galactic Bank";
                location[75].location_type = "Bank";
                location[75].nrofsolditems = 0;

                location[76].location_image = "";
                location[76].location_name = "Piggy Bank";
                location[76].location_type = "Bank";
                location[76].nrofsolditems = 0;

                location[77].location_image = "";
                location[77].location_name = "Federal Mint";
                location[77].location_type = "Bank";
                location[77].nrofsolditems = 0;

                location[78].location_image = "";
                location[78].location_name = "The Stellar Dance Club";
                location[78].location_type = "Bar";
                location[78].nrofsolditems = 0;

                location[79].location_image = "";
                location[79].location_name = "Bottoms-Up Bar and Grill";
                location[79].location_type = "Bar";
                location[79].nrofsolditems = 0;

                location[80].location_image = "";
                location[80].location_name = "Chug-A-Mug";
                location[80].location_type = "Bar";
                location[80].nrofsolditems = 0;

                location[81].location_image = "";
                location[81].location_name = "Starlite Saloon";
                location[81].location_type = "Bar";
                location[81].nrofsolditems = 0;

                location[82].location_image = "";
                location[82].location_name = "Alskant Trading Base";
                location[82].location_type = "Technology Shop";
                location[82].nrofsolditems = 4;
                location[82].sells = new int[4];
                location[82].sells[0] = 0;
                location[82].sells[1] = 1;
                location[82].sells[2] = 2;
                location[82].sells[3] = 6;

                location[83].location_image = "";
                location[83].location_name = "Creonti Depot";
                location[83].location_type = "Technology Shop";
                location[83].nrofsolditems = 4;
                location[83].sells = new int[4];
                location[83].sells[0] = 0;
                location[83].sells[1] = 1;
                location[83].sells[2] = 2;
                location[83].sells[3] = 6;

                location[84].location_image = "";
                location[84].location_name = "Human Hardware";
                location[84].location_type = "Technology Shop";
                location[84].nrofsolditems = 5;
                location[84].sells = new int[5];
                location[84].sells[0] = 0;
                location[84].sells[1] = 1;
                location[84].sells[2] = 2;
                location[84].sells[3] = 6;
                location[84].sells[4] = 9;

                location[85].location_image = "";
                location[85].location_name = "Ik'Thorne Drone Farm";
                location[85].location_type = "Technology Shop";
                location[85].nrofsolditems = 5;
                location[85].sells = new int[5];
                location[85].sells[0] = 0;
                location[85].sells[1] = 1;
                location[85].sells[2] = 2;
                location[85].sells[3] = 3;
                location[85].sells[4] = 6;

                location[86].location_image = "";
                location[86].location_name = "Salvene Supply & Plunder";
                location[86].location_type = "Technology Shop";
                location[86].nrofsolditems = 5;
                location[86].sells = new int[5];
                location[86].sells[0] = 0;
                location[86].sells[1] = 1;
                location[86].sells[2] = 2;
                location[86].sells[3] = 6;
                location[86].sells[4] = 8;

                location[87].location_image = "";
                location[87].location_name = "The Thevian Bounty";
                location[87].location_type = "Technology Shop";
                location[87].nrofsolditems = 4;
                location[87].sells = new int[4];
                location[87].sells[0] = 0;
                location[87].sells[1] = 1;
                location[87].sells[2] = 2;
                location[87].sells[3] = 6;

                location[88].location_image = "";
                location[88].location_name = "West-Quadrant Hardware";
                location[88].location_type = "Technology Shop";
                location[88].nrofsolditems = 5;
                location[88].sells = new int[5];
                location[88].sells[0] = 0;
                location[88].sells[1] = 1;
                location[88].sells[2] = 2;
                location[88].sells[3] = 6;
                location[88].sells[4] = 9;

                location[89].location_image = "";
                location[89].location_name = "Nijarin Hardware";
                location[89].location_type = "Technology Shop";
                location[89].nrofsolditems = 5;
                location[89].sells = new int[5];
                location[89].sells[0] = 0;
                location[89].sells[1] = 1;
                location[89].sells[2] = 2;
                location[89].sells[3] = 6;
                location[88].sells[3] = 10;
#endregion
            }
        }

        public Race GetRaceObject(int index)
	//This function returns a race object given an index
        {
            return race[index];
        }

        public Race GetRaceObject(string race_name)
	//This function returns a race object given a string
        {
            for (int i = 1; i <= nrofraces; i++)
            {
                if (race_name == race[i].race_name)
                    return race[i];
            }
            return null;
        }

        public int GetWeaponIndex(string wname)
	//This function returns a weapon index given a string
        {
            for (int i = 0; i < nrofweapons; i++)
                if (weapon[i].name == wname)
                    return i;

            return -1;
        }

        public int GetTechIndex(string tname)
	//This function returns a technology index given a string
        {
            for (int i = 0; i < nroftechs; i++)
                if (technology[i].name == tname)
                    return i;

            return -1;
        }

        public int GetShipIndex(string sname)
	//This function returns a ship index given a string
        {
            for (int i = 0; i < nrofships; i++)
                if (ship[i].name == sname)
                    return i;

            return -1;
        }

        public void InitializeGalaxies(ushort nrofgals)
	    //This function makes sure that memory is allocated for the given number of galaxies and it initialize the first galaxy (at index 0) that contains the 0 sector
        {
            galaxy = new Galaxy[nrofgals+1];
            nrofgalaxies = nrofgals;
            for (int i = 0; i < nrofgals+1; i++)
            {
                galaxy[i] = new Galaxy();
                galaxy[i].game = this;
            }
            galaxy[0].galaxy_name = "Zero";
            galaxy[0].galaxy_xsize = 1;
            galaxy[0].galaxy_ysize = 1;
            galaxy[0].lowestsectorid = 0;
            galaxy[0].InitializeGalaxy(1,1);
        }

        public int GetGalaxyIndex(int sectorid)
	//This function gets the index of a galaxy, given a specific sector id
        {
            int totalnrofsectors = 0;

            for(int i = 0; i < nrofgalaxies; i++)
            {
                if(sectorid >= totalnrofsectors && sectorid < totalnrofsectors+this.galaxy[i].galaxy_xsize*this.galaxy[i].galaxy_ysize)
                    return i;
                else
                    totalnrofsectors += this.galaxy[i].galaxy_xsize*this.galaxy[i].galaxy_ysize;
            }

            return -1;
        }

        public Sector GetSectorObject(int sectorid)
	//This function returns the sector object, given a specific sector id
        {
            int currentgalaxy = GetGalaxyIndex(sectorid);

            if (currentGalaxy == 0)
                return null;
            if (sectorid == -1)
                return null;
            else if (currentgalaxy == -1)
                return null;
            else
                return galaxy[currentgalaxy].sector[(sectorid - galaxy[currentgalaxy].lowestsectorid) % galaxy[currentgalaxy].galaxy_xsize, (sectorid - galaxy[currentgalaxy].lowestsectorid) / galaxy[currentgalaxy].galaxy_xsize];
        }

        public int GetLocationIndex(string locname)
	//This function returns a location index given an location name
        {
            for(int i = 0; i <= nroflocations; i++)
                if(location[i].location_name == locname)
                    return i;

            return -1;
        }

        public int GetItemIndex(Location loc, string itemName)
	//This function returns the itemNameth item index that is sold in the given location.
        {
            if(loc.location_type == "Technology Shop")
                return GetTechIndex(itemName);
            
            if(loc.location_type == "Weapon Shop")
                return GetWeaponIndex(itemName);

            if(loc.location_type == "Ship Shop")
                return GetShipIndex(itemName);

            return -1;
        }

        public int GetRaceIndex(string race_name)
	//his function returns the index of a race, given its name
        {
            for (int i = 1; i <= nrofraces; i++)
            {
                if (race[i].race_name == race_name)
                    return i;
            }

            return -1;
        }

        public string GetRaceLetter(string race_name)
	//This function returns, given a race name, the letter abbreviation that is used both in the display of the route finder and in the main viewer
        {
            if (race_name == "Neutral")
                return "(-)";
            if (race_name == "Alskant")
                return "(A)";
            if (race_name == "Creonti")
                return "(C)";
            if (race_name == "Ik'Thorne")
                return "(I)";
            if (race_name == "Human")
                return "(H)";
            if (race_name == "Salvene")
                return "(S)";
            if (race_name == "Thevian")
                return "(T)";
            if (race_name == "WQ Human")
                return "(W)";
            if (race_name == "Nijarin")
                return "(N)";

            return "";
        }

        public int GetLocationIconIndex(Location l)
	//Where is this function used?
        {
            if (l.location_type == "Weapon Shop")
                return 7;
            if (l.location_type == "Ship Shop")
                return 8;
            if (l.location_type == "Technology Shop")
                return 6;
            if (l.location_type == "Bar")
                return 5;
            if (l.location_type == "Bank")
                return 4;
            if (l.location_type == "UG")
                return 3;
            if (l.location_type == "HQ")
                return 2;
            if (l.location_type == "Fed")
                return 1;

            return -1;
        }

        public ushort ConvertLocation(int loc)
	//This function is used in converting maps to cmf format and is now obsolete
        {
            if (location[loc].location_type == "Bank")
                return 200;
            if (location[loc].location_type == "Bar")
                return 300;
            if (location[loc].location_type == "HQ")
                return 199;
            if (location[loc].location_type == "Fed")
                return 1;
            if (location[loc].location_name == "Alskant Trading Base")
                return 401;
            if (location[loc].location_name == "Creonti Depot")
                return 402;
            if (location[loc].location_name == "Human Hardware")
                return 403;
            if (location[loc].location_name == "Ik'Thorne Drone Farm")
                return 404;
            if (location[loc].location_name == "Nijarin Hardware")
                return 405;
            if (location[loc].location_name == "Salvene Supply")
                return 406;
            if (location[loc].location_name == "The Thevian Bounty")
                return 407;
            if (location[loc].location_name == "West-Quadrant Hardware")
                return 408;
            if (location[loc].location_name == "Accelerated Systems")
                return 409;
            if (location[loc].location_name == "Advanced communications")
                return 410;

            return 0;
        }

        public void ResetShortestRoutes(int sectorid)
        //This function deletes all shortest routes that go through the given sector. Be careful when using this function, for it may go at the cost of computation time. Currently only used when the user marks a sector as hostile
        {
            smachanged = true;
            for (int x = 0; x < nrofsectors; x++)
                for (int y = 0; y < nrofsectors; y++)
                {
                    if(shortestroutes[x, y] != null)
                    {
                        if(shortestroutes[x, y].sectors.Contains(sectorid))
                            shortestroutes[x, y] = null;
                    }
                }
        }

        public void ResetShortestRoutes()
	//This function deletes all shortest route administration. Be careful when using this function, for it may go at the cost of computation time. Currently only used when the user marks a sector as hostile
        {
            smachanged = true;
            for (int x = 0; x < nrofsectors; x++)
                for (int y = 0; y < nrofsectors; y++)
                    shortestroutes[x, y] = null;
        }

        public void SortShips()
        //This function sorts all ships alphabetically
        {
            ArrayList sortedShips = new ArrayList();
            int index;

            //Add ships
            sortedShips.Add(ship[0]);
            for (int s = 1; s < nrofships; s++)
            {
                for (index = 0; index < sortedShips.Count; index++)
                    if (ship[s].name.CompareTo(((Ship)sortedShips[index]).name) < 0)
                    {
                        sortedShips.Insert(index, ship[s]);
                        break;
                    }

                if (index == sortedShips.Count && sortedShips.Count < nrofships)
                    sortedShips.Add(ship[s]);
            }

            for (int i = 0; i < sortedShips.Count; i++)
            {
                ship[i] = (Ship)sortedShips[i];
            }
        }

        public void FindComplementarySector(Sector source, int goodindex, bool[] galallowed, bool evade, bool onegoodroutesallowed)
	    //This function, given a source sector, a specific good index and the list of allowed galaxies, computes the list of all ports that sell the good (if the source port buys it) or buys the good (if the source port sells it). The list is stored in the port object member of the source Sector class
        {
            Sector cs;
            Queue sectorQueue = new Queue();
            bool lookingforaseller = false;
            bool lookingforabuyer = false;

            //First determine whether the source port buys or sells the good. If it does neither, return
            if (source.port.port_goods[goodindex] == 1 || source.port.port_goods[goodindex] == 2)
                lookingforaseller = true;
            if(source.port.port_goods[goodindex] == -1 || source.port.port_goods[goodindex] == 2)
                lookingforabuyer = true;
            if(!lookingforabuyer && !lookingforaseller)
                return;

            //Then clear all distance variables
            for (int g = 0; g < nrofgalaxies; g++)
            {
                for (int x = 0; x < galaxy[g].galaxy_xsize; x++)
                    for (int y = 0; y < galaxy[g].galaxy_ysize; y++)
                    {
                        galaxy[g].sector[x, y].distance = -1;
                    }
            }

		    //This function uses a breadth-first search, so we create a queue and enqueue the source sector
            source.distance = 0;
            sectorQueue.Enqueue(source);

		    //Take a sector out of the queue, test whether it fits the criteria and enqueue all its neighboring sectors that have not yet been visited
            while(sectorQueue.Count > 0)
            {
                cs = (Sector) sectorQueue.Dequeue();

                if (cs.port != null && cs.sector_id != source.sector_id)
                {
                    //If this search concerns the "Nothing" good, then return this sector
                    if (cs.port.port_goods[goodindex] == 2 && source.port.port_goods[goodindex] == 2 && onegoodroutesallowed && source.sector_id != cs.sector_id)
                    {
                        source.port.complementary_sectors[goodindex].Add(cs);
                        source.port.complementary_distances[goodindex].Add(0);
                    }
                    else if ((cs.port.port_goods[goodindex] == 1 && lookingforabuyer) ||
                       (cs.port.port_goods[goodindex] == -1 && lookingforaseller))
                    {
                        source.port.complementary_sectors[goodindex].Add(cs);
                        source.port.complementary_distances[goodindex].Add(cs.distance);
                    }
                }

                if (cs.east != null && cs.east.west != null && (cs.east.distance == -1 || cs.east.distance > cs.distance + 1) && (!evade || cs.east.status != -1))
                {
                    cs.east.distance = cs.distance + 1;
                    sectorQueue.Enqueue(cs.east);
                }
                if (cs.west != null && cs.west.east != null && (cs.west.distance == -1 || cs.west.distance > cs.distance + 1) && (!evade || cs.west.status != -1))
                {
                    cs.west.distance = cs.distance + 1;
                    sectorQueue.Enqueue(cs.west);
                }
                if (cs.south != null && cs.south.north != null && (cs.south.distance == -1 || cs.south.distance > cs.distance + 1) && (!evade || cs.south.status != -1))
                {
                    cs.south.distance = cs.distance + 1;
                    sectorQueue.Enqueue(cs.south);
                }
                if (cs.north != null && cs.north.south != null && (cs.north.distance == -1 || cs.north.distance > cs.distance + 1) && (!evade || cs.north.status != -1))
                {
                    cs.north.distance = cs.distance + 1;
                    sectorQueue.Enqueue(cs.north);
                }
                if (cs.warp != null && cs.warp.warp != null && galallowed[cs.warp.galaxy.galaxy_id] && (cs.warp.distance == -1 || cs.warp.distance > cs.distance + 5) && (!evade || cs.warp.status != -1))
                {
                    if (galaxy[GetGalaxyIndex(source.sector_id)].neighbours.Contains(GetGalaxyIndex(cs.sector_id)))
                    {
                        cs.warp.distance = cs.distance + 5;
                        sectorQueue.Enqueue(cs.warp);
                    }
                }
            }
        }
    }
}
