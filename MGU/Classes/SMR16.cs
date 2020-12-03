using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MGU
{
    class SMR16
    {
        #region Reading and Writing of SMR files
        public static bool LoadData(string SMRFile, Game currentGame)
        {
            string[] sectors;
            TextReader additionaldata;
            bool addloaded = false;
            int startsector, endsector; 
            int currentsectorid = 0, currentgalaxyid = 0, currentsectorx, currentsectory, raceNumber = 0, goodsNumber = 0, goodsPrice = 0, portlevel = 0;

            //Open selected file
            FileStream fs, fsa;
            try
            {
                fs = new FileStream(SMRFile, FileMode.Open, FileAccess.Read);
            }
            catch (System.IO.IOException)
            {
                return false;
            }
            TextReader data = new StreamReader(fs);

            string SMRAdditional = Path.Combine(Path.GetDirectoryName(SMRFile), Path.GetFileNameWithoutExtension(SMRFile) + ".sma");
            if(File.Exists(SMRAdditional))
            {
                try
                {
                    fsa = new FileStream(SMRAdditional, FileMode.Open, FileAccess.Read);
                    addloaded = true;
                }
                catch (System.IO.IOException)
                {
                    return false;
                }

                additionaldata = new StreamReader(fsa);
            }
            else
            {
                fsa = new FileStream(SMRAdditional, FileMode.Create, FileAccess.ReadWrite);
                additionaldata = new StreamReader(fsa);
            }

            //Check if the file is an SMR file and check the version
            string fileheader = data.ReadLine();
            if(fileheader == null)
                return false;

            if (!fileheader.StartsWith(";SMR1.6 Sectors File v", StringComparison.InvariantCultureIgnoreCase))
            {
                MessageBox.Show("This file is not a SMR file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            currentGame.address = SMRFile;

            //Split the entire file into individual lines
            char[] delim = { '\n', '\r' };
            ArrayList lines = new ArrayList(data.ReadToEnd().Split(delim, StringSplitOptions.RemoveEmptyEntries));
            ForceData forces;

            int i, j, nrofcomments = 0;
            
            for(i = 0; i < lines.Count; i++)
            {
                switch(lines[i].ToString())
                {
                    case "[Game]":
                        //Move past game header
                        i++;
                        //Retrieve game name
                        delim = new char[] { '=' };
                        string[] galname = lines[i].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                        currentGame.game_name = galname[1];
                        break;
                    case "[Races]":
                        //Reset the number of detected comments
                        nrofcomments = 0;
                        //Move past Races header and comments
                        i++;
                        //Determine the number of races
                        j = i;
                        while(!(lines[j].ToString().StartsWith("[") && lines[j].ToString().EndsWith("]")))
                        {
                            if(lines[j].ToString().StartsWith(";"))
                                nrofcomments++;
                            j++;
                        }
                        ushort numberofraces = (ushort) (j-i-nrofcomments);

                        //Allocate space for all races
                        currentGame.InitializeRaces(numberofraces, false);

                        //Retrieve race names
                        for(; i < j; i++)
                        {
                            if(!lines[i].ToString().StartsWith(";"))
                            {
                                delim = new char[] { '=', ',' };
                                string[] raceNames = lines[i].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                int.TryParse(raceNames[1], out raceNumber);
                                currentGame.race[raceNumber].race_name = raceNames[0];
                            }
                        }
                        //Move one line back to prevent missing a header
                        i--;
                        break;
                    case "[Goods]":
                        //Reset the number of detected comments
                        nrofcomments = 0;
                        //Move past goods header
                        i++;
                        //Determine the number of goods
                        j = i;
                        while (!(lines[j].ToString().StartsWith("[") && lines[j].ToString().EndsWith("]")))
                        {
                            if (lines[j].ToString().StartsWith(";"))
                                nrofcomments++;
                            j++;
                        }
                        ushort numberofgoods = (ushort) (j - i - nrofcomments);

                        //Allocate space for all goods
                        currentGame.InitializeGoods(numberofgoods, false);

                        //Retrieve goods data
                        for (; i < j; i++)
                        {
                            if (!lines[i].ToString().StartsWith(";"))
                            {
                                delim = new char[] { '=', ',' };
                                string[] goodsNames = lines[i].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                int.TryParse(goodsNames[0], out goodsNumber);
                                int.TryParse(goodsNames[2], out goodsPrice);
                                currentGame.good[goodsNumber].good_name = goodsNames[1];
                                currentGame.good[goodsNumber].good_price = goodsPrice;
                                if (currentGame.good[goodsNumber].good_name == "Slaves" ||
                                   currentGame.good[goodsNumber].good_name == "Weapons" ||
                                   currentGame.good[goodsNumber].good_name == "Narcotics")
                                    currentGame.good[goodsNumber].goodorevil = false;
                            }
                        }
                        //Move one line back to prevent missing a header
                        i--;
                        break;
                    case "[Weapons]":
                        //Reset the number of detected comments
                        nrofcomments = 0;
                        //Move past weapons header
                        i++;
                        //Determine the number of weapons
                        j = i;
                        while (!(lines[j].ToString().StartsWith("[") && lines[j].ToString().EndsWith("]")))
                        {
                            if (lines[j].ToString().StartsWith(";"))
                                nrofcomments++;
                            j++;
                        }
                        ushort numberofguns = (ushort) (j - i - nrofcomments);

                        //Allocate space for all weapons
                        currentGame.InitializeWeapons(numberofguns, false);
                        int weaponnr = 1;

                        //Retrieve weapon data
                        for (; i < j; i++)
                        {
                            if (!lines[i].ToString().StartsWith(";"))
                            {
                                delim = new char[] { '=', ',' };
                                string[] weaponData = lines[i].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                currentGame.weapon[weaponnr].name = weaponData[0];
                                currentGame.weapon[weaponnr].weapon_race = currentGame.GetRaceObject(weaponData[1]);
                                int.TryParse(weaponData[2], out currentGame.weapon[weaponnr].cost);
                                int.TryParse(weaponData[3], out currentGame.weapon[weaponnr].shield_damage);
                                int.TryParse(weaponData[4], out currentGame.weapon[weaponnr].armor_damage);
                                int.TryParse(weaponData[5], out currentGame.weapon[weaponnr].accuracy);
                                int.TryParse(weaponData[6], out currentGame.weapon[weaponnr].power);
                                int.TryParse(weaponData[7], out currentGame.weapon[weaponnr].alignment);
                                weaponnr++;
                            }
                        }
                        //Move one line back to prevent missing a header
                        i--;
                        break;
                    case "[ShipEquipment]":
                        //Reset the number of detected comments
                        nrofcomments = 0;
                        //Move past ShipEquipment header
                        i++;
                        //Determine the number of technologies
                        j = i;
                        while (!(lines[j].ToString().StartsWith("[") && lines[j].ToString().EndsWith("]")))
                        {
                            if (lines[j].ToString().StartsWith(";"))
                                nrofcomments++;
                            j++;
                        }
                        ushort numberoftechs = (ushort) (j - i - nrofcomments);

                        //Allocate space for all technology
                        currentGame.InitializeTechs(numberoftechs, false);
                        int technr = 0;

                        //Retrieve technology data
                        for (; i < j; i++)
                        {
                            if (!lines[i].ToString().StartsWith(";"))
                            {
                                delim = new char[] { '=', ',' };
                                string[] techData = lines[i].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                currentGame.technology[technr].name = techData[0];
                                long.TryParse(techData[1], out currentGame.technology[technr].cost);
                                technr++;
                            }
                        }
                        //Move one line back to prevent missing a header
                        i--;
                        break;
                    case "[Ships]":
                        //Reset the number of detected comments
                        nrofcomments = 0;
                        //Move past ships header
                        i++;
                        //Determine the number of ships
                        j = i;
                        while (!(lines[j].ToString().StartsWith("[") && lines[j].ToString().EndsWith("]")))
                        {
                            if (lines[j].ToString().StartsWith(";"))
                                nrofcomments++;
                            j++;
                        }
                        ushort numberofships = (ushort) (j - i - nrofcomments);

                        //Allocate space for all ships
                        currentGame.InitializeShips(numberofships, false);
                        int shipnr = 0;

                        //Retrieve ship data
                        for (; i < j; i++)
                        {
                            if (!lines[i].ToString().StartsWith(";"))
                            {
                                delim = new char[] { ',' };
                                string[] shipData = lines[i].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                delim = new char[] { '=' };
                                string[] basicData = shipData[0].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                currentGame.ship[shipnr].name = basicData[0];
                                currentGame.ship[shipnr].ship_race = currentGame.GetRaceObject(basicData[1]);
                                long.TryParse(shipData[1], out currentGame.ship[shipnr].ship_cost);
                                int.TryParse(shipData[2], out currentGame.ship[shipnr].ship_speed);
                                int.TryParse(shipData[3], out currentGame.ship[shipnr].ship_hardpoints);
                                int.TryParse(shipData[4], out currentGame.ship[shipnr].ship_power);
                                currentGame.ship[shipnr].ship_class = shipData[5];
                                delim = new char[] { '=', ';' };
                                string[] shipTech = shipData[6].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                for(int st = 0; st < shipTech.Length; st++)
                                {
                                    switch(shipTech[st])
                                    {
                                        case "Shields":
                                            int.TryParse(shipTech[st+1], out currentGame.ship[shipnr].ship_shields);
                                            break;
                                        case "Armor":
                                            int.TryParse(shipTech[st + 1], out currentGame.ship[shipnr].ship_armor);
                                            break;
                                        case "Cargo Holds":
                                            int.TryParse(shipTech[st + 1], out currentGame.ship[shipnr].ship_cargo);
                                            break;
                                        case "Combat Drones":
                                            int.TryParse(shipTech[st + 1], out currentGame.ship[shipnr].ship_combat_drones);
                                            break;
                                        case "Scout Drones":
                                            int.TryParse(shipTech[st + 1], out currentGame.ship[shipnr].ship_combat_drones);
                                            break;
                                        case "Mines":
                                            int.TryParse(shipTech[st + 1], out currentGame.ship[shipnr].ship_mines);
                                            break;
                                        case "Scanner":
                                            if(int.Parse(shipTech[st + 1]) == 1)
                                                currentGame.ship[shipnr].ship_ScanAble = true;
                                            break;
                                        case "Cloaking Device":
                                            if(int.Parse(shipTech[st + 1]) == 1)
                                                currentGame.ship[shipnr].ship_CloakAble = true;
                                            break;
                                        case "Illusion Generator":
                                            if(int.Parse(shipTech[st + 1]) == 1)
                                                currentGame.ship[shipnr].ship_IGAble = true;
                                            break;
                                        case "Jump Drive":
                                            if (int.Parse(shipTech[st + 1]) == 1)
                                                currentGame.ship[shipnr].ship_JumpAble = true;
                                            break;
                                        case "Drone Scrambler":
                                            if (int.Parse(shipTech[st + 1]) == 1)
                                                currentGame.ship[shipnr].ship_DSAble = true;
                                            break;
                                    } 
                                }
                                delim = new char[] { '=' };
                                basicData = shipData[7].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                int.TryParse(basicData[1], out currentGame.ship[shipnr].ship_restrictions);
                                shipnr++;

                                //When all Ships have been read, sort them
                                if(i==j-1)
                                    currentGame.SortShips();
                            }
                        }
                        //Move one line back to prevent missing a header
                        i--;
                        break;
                    case "[Locations]":
                        //Reset the number of detected comments
                        nrofcomments = 0;
                        //Move past locations header
                        i++;
                        //Determine the number of locations
                        j = i;
                        while (!(lines[j].ToString().StartsWith("[") && lines[j].ToString().EndsWith("]")))
                        {
                            if (lines[j].ToString().StartsWith(";"))
                                nrofcomments++;
                            j++;
                        }
                        ushort numberoflocs = (ushort) (j - i - nrofcomments);

                        //Allocate space for all locations
                        currentGame.InitializeLocations(numberoflocs, false);
                        int locnr = 1;

                        //Retrieve location data
                        for (; i < j; i++)
                        {
                            if (!lines[i].ToString().StartsWith(";"))
                            {
                                delim = new char[] { '=', ';' };
                                string[] locData = lines[i].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                currentGame.location[locnr].location_name = locData[0];
                                for (int loci = 1; loci < locData.Length; loci++)
                                {
                                    switch (locData[loci])
                                    {
                                        case "Weapons":
                                            currentGame.location[locnr].location_type = "Weapon Shop";
                                            currentGame.location[locnr].Initialize(locData.Length - 2);
                                            for (int weapnr = 2; weapnr < locData.Length; weapnr++)
                                                currentGame.location[locnr].sells[weapnr - 2] = currentGame.GetWeaponIndex(locData[weapnr]);
                                            break;
                                        case "ShipEquipment":
                                            currentGame.location[locnr].location_type = "Technology Shop";
                                            currentGame.location[locnr].Initialize(locData.Length - 2);
                                            for (technr = 2; technr < locData.Length; technr++)
                                                currentGame.location[locnr].sells[technr - 2] = currentGame.GetTechIndex(locData[technr]);
                                            break;
                                        case "Ships":
                                            currentGame.location[locnr].location_type = "Ship Shop";
                                            currentGame.location[locnr].Initialize(locData.Length - 2);
                                            for (shipnr = 2; shipnr < locData.Length; shipnr++)
                                                currentGame.location[locnr].sells[shipnr - 2] = currentGame.GetShipIndex(locData[shipnr]);
                                            break;
                                        case "HQ":
                                            currentGame.location[locnr].location_type = "HQ";
                                            break;
                                        case "UG":
                                            currentGame.location[locnr].location_type = "UG";
                                            break;
                                        case "Fed":
                                            currentGame.location[locnr].location_type = "Fed";
                                            break;
                                        case "Bank":
                                            currentGame.location[locnr].location_type = "Bank";
                                            break;
                                        case "Bar":
                                            currentGame.location[locnr].location_type = "Bar";
                                            break;
                                    }
                                }
                                if (currentGame.location[locnr].nrofsolditems == 0)
                                {
                                    currentGame.location[locnr].Initialize(0);
                                    if(locData.Length > 1)
                                        currentGame.location[locnr].location_type = locData[1];
                                }
                                locnr++;
                            }
                        }
                        //Move one line back to prevent missing a header
                        i--;
                        break;
                    case "[Galaxies]":
                        //Reset the number of detected comments
                        nrofcomments = 0;
                        //Move past galaxies header
                        i++;
                        //Determine the number of galaxies
                        j = i;
                        while (!(lines[j].ToString().StartsWith("[") && lines[j].ToString().EndsWith("]")))
                        {
                            if (lines[j].ToString().StartsWith(";"))
                                nrofcomments++;
                            j++;
                        }
                        ushort numberofgals = (ushort) (j - i - nrofcomments);

                        //Allocate space for all locations
                        currentGame.InitializeGalaxies((ushort) (numberofgals+1));
                        ushort galnr = 1;
                        ushort totalnrofsectors = 0;

                        //Retrieve galaxy data
                        for (; i < j; i++)
                        {
                            if (!lines[i].ToString().StartsWith(";"))
                            {
                                delim = new char[] {'=', ',' };
                                string[] galData = lines[i].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                currentGame.galaxy[galnr].galaxy_id = int.Parse(galData[0]);
                                currentGame.galaxy[galnr].galaxy_xsize = int.Parse(galData[1]);
                                currentGame.galaxy[galnr].galaxy_ysize = int.Parse(galData[2]);
                                currentGame.galaxy[galnr].galaxy_type = galData[3];
                                currentGame.galaxy[galnr].galaxy_name = galData[4];
                                currentGame.galaxy[galnr].lowestsectorid = currentGame.galaxy[galnr].startsector = totalnrofsectors + 1;
                                totalnrofsectors += (ushort) (currentGame.galaxy[galnr].galaxy_xsize*currentGame.galaxy[galnr].galaxy_ysize);
                                currentGame.galaxy[galnr].InitializeGalaxy(currentGame.galaxy[galnr].galaxy_xsize, currentGame.galaxy[galnr].galaxy_ysize);
                            }
                            galnr++;
                        }
                        currentGame.nrofsectors = (ushort) (totalnrofsectors+1);
                        currentGame.currentGalaxy = 1;

                        //Move one line back to prevent missing a header
                        i--;
                        break;
                    default:
                        if(lines[i].ToString().Contains("Sector"))
                            goto Sectors;
                        break;
                }
            }
        Sectors:        
            for (j = i; j < lines.Count; j++)
                if ((string)lines[j] == "[Sector=1]")
                    break;

            int west = 0, east = 0, south = 0, north = 0, warp = 0, portrace = 0, goodid = 0;

            for (i = 1; i < currentGame.nrofsectors; i++)
            {
                //First break down the line to be read into a header and field value
                delim = new char[] { '=', ']' };
                string[] line = lines[j].ToString().Split(delim);

                //Determine galaxy and index of current sector
                currentsectorid = int.Parse(line[1]);
                currentgalaxyid = currentGame.GetGalaxyIndex(currentsectorid);
                currentsectorx = (currentsectorid - currentGame.galaxy[currentgalaxyid].lowestsectorid) % currentGame.galaxy[currentgalaxyid].galaxy_xsize;
                currentsectory = (currentsectorid - currentGame.galaxy[currentgalaxyid].lowestsectorid) / currentGame.galaxy[currentgalaxyid].galaxy_xsize;

                //Set location of sector (important for drawing)
                int x = currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].GetX(currentGame.galaxy[currentgalaxyid].lowestsectorid);
                int y = currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].GetY(currentGame.galaxy[currentgalaxyid].lowestsectorid);
                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].Location = new Point(x * currentGame.sectorsize,y * currentGame.sectorsize);

                j++;
                if (j >= lines.Count)
                    break;

                while (!lines[j].ToString().Contains("Sector"))
                {
                    //First break down the line to be read into a header and field value
                    delim = new char[] { '=' };
                    line = lines[j].ToString().Split(delim);

                    //Distinguish between different line headers
                    switch (line[0])
                    {
                        case "Left":
                            //If the left sector number cannot be parsed, then create an empty sector
                            if (intparse(line[1], ref west))
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].west = currentGame.GetSectorObject(west);
                            break;
                        case "Right":
                            if (intparse(line[1], ref east))
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].east = currentGame.GetSectorObject(east);
                            break;
                        case "Up":
                            if (intparse(line[1], ref north))
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].north = currentGame.GetSectorObject(north);
                            break;
                        case "Down":
                            if (intparse(line[1], ref south))
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].south = currentGame.GetSectorObject(south);
                            break;
                        case "Planet":
                            currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].planet = new Planet();
                            break;
                        case "Port Level":
                            currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].port = new Port(currentGame.nrofgoods);
                            if (intparse(line[1], ref portlevel))
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].port.port_level = portlevel;
                            break;
                        case "Port Race":
                            if (!intparse(line[1], ref portrace)) portrace = 0;
                            currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].port.port_race = currentGame.race[portrace];
                            break;
                        case "Buys":
                            delim = new char[] { ',', ']' };
                            line = line[1].ToString().Split(delim);
                            for (int pg = 1; pg < line.Length + 1; pg++)
                            {
                                if (!intparse(line[pg - 1], ref goodid))
                                    continue;
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].port.port_goods[goodid] = 1;
                            }
                            break;
                        case "Sells":
                            delim = new char[] { ',', ']' };
                            line = line[1].ToString().Split(delim);
                            for (int pg = 1; pg < line.Length + 1; pg++)
                            {
                                if (!intparse(line[pg - 1], ref goodid))
                                    continue;
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].port.port_goods[goodid] = -1;
                            }
                            break;
                        case "Locations":
                            delim = new char[] { ',' };
                            line = line[1].ToString().Split(delim);
                            for (int ll = 0; ll < line.Length; ll++)
                            {
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].AddLocation(currentGame.GetLocationIndex(line[ll]));
                            }
                            break;
                        
                        case "Warp":
                            if (intparse(line[1], ref warp))
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].warp = currentGame.GetSectorObject(warp);
                            break;

                        case "EnemyForces":
                            delim = new char[] { '=', ';', ',' };
                            line = lines[j].ToString().Split(delim);
                            for (int ll = 1; ll < line.Length - 6; ll += 7)
                            {
                                forces = new ForceData();

                                forces.owner = line[ll];
                                forces.affiliation = -1;
                                forces.sectorid = currentsectorid;

                                for (int lll = ll + 1; lll < ll + 7; lll++)
                                    switch (line[lll])
                                    {
                                        case "Mines":
                                            if (currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].status == 0 && currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].friendly_mines > 0)
                                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].status = 1;

                                            int.TryParse(line[lll + 1], out forces.mines);
                                            if(forces.mines > 0)
                                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].status = -1;
                                            lll++;
                                            break;
                                        case "Scout Drones":
                                            int.TryParse(line[lll + 1], out forces.scout_drones);
                                            lll++;
                                            break;
                                        case "Combat Drones":
                                            int.TryParse(line[lll + 1], out forces.combat_drones);
                                            lll++;
                                            break;
                                    }

                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].force_stacks.Add(forces);
                            }
                            break;

                        case "FriendlyForces":
                            delim = new char[] { '=', ';', ',' };
                            line = lines[j].ToString().Split(delim);
                            for (int ll = 1; ll < line.Length-6; ll += 7)
                            {
                                forces = new ForceData();

                                forces.owner = line[ll];
                                forces.affiliation = 1;
                                forces.sectorid = currentsectorid;

                                for(int lll = ll+1; lll < ll+7; lll++)
                                    switch (line[lll])
                                    {
                                        case "Mines":
                                            if (currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].status == 0 && currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].friendly_mines > 0)
                                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].status = 1;

                                            int.TryParse(line[lll + 1], out forces.mines);
                                            if (forces.mines > 0)
                                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].status = 1;
                                            lll++;
                                            break;
                                        case "Scout Drones":
                                            int.TryParse(line[lll + 1], out forces.scout_drones);
                                            lll++;
                                            break;
                                        case "Combat Drones":
                                            int.TryParse(line[lll + 1], out forces.combat_drones);
                                            lll++;
                                            break;
                                    }

                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].force_stacks.Add(forces);
                            }
                            break;

                        case "Status":
                            if (line[1] == "Hostile")
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].status = -1;
                            else if (line[1] == "Friendly")
                                currentGame.galaxy[currentgalaxyid].sector[currentsectorx, currentsectory].status = 1;
                            break;

                        default:
                            break;
                    }

                    j++;
                    if (j >= lines.Count)
                        break;
                    else
                        if (lines[j].ToString().Contains("[Routes]"))
                            break;
                }
            }

            currentGame.shortestroutes = new Route[currentGame.nrofsectors, currentGame.nrofsectors];

            if (File.Exists(SMRAdditional) && addloaded)
            {
                string[] line;
                delim = new char[] { '\n', '\r' };
                lines = new ArrayList(additionaldata.ReadToEnd().Split(delim, StringSplitOptions.RemoveEmptyEntries));

                for (i = 0; i < lines.Count && !((string)lines[i]).Contains("Alliance Members"); i++)
                {
                    if (lines[i].ToString().Contains("[Routes]"))
                    {
                        j = i+1;
                        while (j < lines.Count && !((string)lines[j]).Contains("Alliance Members"))
                        {
                            delim = new char[] { '-' };
                            sectors = lines[j].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);

                            startsector = Convert.ToInt16(sectors[0]);
                            endsector = Convert.ToInt16(sectors[sectors.Length - 1]);

                            //Split sectorlist into individual sectors
                            currentGame.shortestroutes[startsector, endsector] = new Route(currentGame);

                            //Add sectors to the route
                            for (int s = 0; s < sectors.Length; s++)
                            {
                                currentGame.shortestroutes[startsector, endsector].sectors.Add(Convert.ToInt16(sectors[s]));
                                if (s >= 1)
                                {
                                    if (currentGame.GetGalaxyIndex(Convert.ToInt16(sectors[s])) == currentGame.GetGalaxyIndex(Convert.ToInt16(sectors[s - 1])))
                                        currentGame.shortestroutes[startsector, endsector].length += 1;
                                    else
                                    {
                                        currentGame.shortestroutes[startsector, endsector].length += 5;
                                        currentGame.shortestroutes[startsector, endsector].warps += 1;
                                    }
                                }
                                else
                                    currentGame.shortestroutes[startsector, endsector].length += 1;
                            }

                            j++;
                        }
                    }
                }

                for (; i < lines.Count && !((string)lines[i]).Contains("Seed Sectors"); i++)
                {
                    if (i < lines.Count)
                    {
                        if (lines[i].ToString().Contains("[Alliance Members]"))
                        {
                            j=i+1;
                            while (j < lines.Count && !((string)lines[j]).Contains("Seed Sectors"))
                            {
                                currentGame.allianceMembers.Add(lines[j]);
                                j++;
                            }
                        }
                    }
                }

                for (; i < lines.Count && !((string)lines[i]).Contains("Peace"); i++)
                {
                    if (i < lines.Count)
                    {
                        if (lines[i].ToString().Contains("[Seed Sectors]"))
                        {
                            j = i + 1;
                            while (j < lines.Count && !((string)lines[j]).Contains("Peace"))
                            {
                                currentGame.seedSectors.Add(Convert.ToInt16(lines[j]));
                                j++;
                            }
                        }
                    }
                }

                for (; i < lines.Count && !((string)lines[i]).Contains("Relations"); i++)
                {
                    if (i < lines.Count)
                    {
                        if (lines[i].ToString().Contains("[Peace]"))
                        {
                            j = i + 1;
                            while (j < lines.Count && !((string)lines[j]).Contains("Relations"))
                            {
                                delim = new char[] { '=' };
                                line = lines[j].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                if (line[1].ToString().Contains("War"))
                                {
                                    currentGame.peace[currentGame.GetRaceIndex(line[0].ToString())] = false;
                                }
                                else if (line[1].ToString().Contains("Peace"))
                                    currentGame.peace[currentGame.GetRaceIndex(line[0].ToString())] = true;
                                j++;
                            }
                        }
                    }
                }

                for (; i < lines.Count; i++)
                {
                    if (i < lines.Count)
                    {
                        if (lines[i].ToString().Contains("[Relations]"))
                        {
                            j = i + 1;
                            while (j < lines.Count)
                            {
                                delim = new char[] { '=' };
                                line = lines[j].ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                                currentGame.race[currentGame.GetRaceIndex(line[0].ToString())].relations = Convert.ToInt16(line[1]);
                                j++;
                            }
                        }
                    }
                }
            }

            currentGame.ComputeGalaxyNeighbours();

            fs.Close();
            data.Close();
            if(addloaded)
            {
                fsa.Close();
                additionaldata.Close();
            }
            return true;
        }

        public static bool SaveData(string SMRFile, Game currentGame)
        {
            FileStream fs, fsa;

            try
            {
                fs = new FileStream(SMRFile, FileMode.Create, FileAccess.Write, FileShare.Read);
            }
            catch (System.IO.IOException)
            {
                return false;
            }

            currentGame.saving = true;

            string SMRAdditional = Path.Combine(Path.GetDirectoryName(SMRFile), Path.GetFileNameWithoutExtension(SMRFile) + ".sma");
            try
            {
                fsa = new FileStream(SMRAdditional, FileMode.Create, FileAccess.Write, FileShare.Read);
            }
            catch (System.IO.IOException)
            {
                return false;
            }

            StreamWriter data = new StreamWriter(fs);
            StreamWriter additionaldata = new StreamWriter(fsa);

            data.WriteLine(";SMR1.6 Sectors File v1.07 (modified by MGU)");

            //First we write the races to file
            data.WriteLine("[Races]");
            data.WriteLine("; Name = ID");
            for (int r = 1; r <= currentGame.nrofraces; r++)
                data.WriteLine(currentGame.race[r].race_name + "=" + r.ToString());

            //Done with races, now we write the goods
            data.WriteLine("[Goods]");
            data.WriteLine("; ID = Name, BasePrice");

            for (int g = 1; g <= currentGame.nrofgoods; g++)
                data.WriteLine(g.ToString() + "=" + currentGame.good[g].good_name + "," + currentGame.good[g].good_price.ToString());

            //Done with goods, now we write the weapons
            data.WriteLine("[Weapons]");
            data.WriteLine("; Weapon = Race,Cost,Shield,Armour,Accuracy,Power level,Restriction");
            data.WriteLine("; Restriction: 0=none, 1=good, 2=evil");
            data.WriteLine("; Attack: 0=none, 1=raid");
            for(int w = 1; w <= currentGame.nrofweapons; w++)
                data.WriteLine(currentGame.weapon[w].name + "=" + currentGame.weapon[w].weapon_race.race_name + "," +
                                                                  currentGame.weapon[w].cost.ToString() + "," +
                                                                  currentGame.weapon[w].shield_damage.ToString() + "," + 
                                                                  currentGame.weapon[w].armor_damage.ToString() + "," +
                                                                  currentGame.weapon[w].accuracy.ToString() + "," +
                                                                  currentGame.weapon[w].power.ToString() + "," +
                                                                  currentGame.weapon[w].alignment.ToString());

            //Done with weapons, now technology
            data.WriteLine("[ShipEquipment]");
            data.WriteLine("; Name = Cost");
            for(int t = 0; t < currentGame.nroftechs; t++)
                data.WriteLine(currentGame.technology[t].name + "=" + currentGame.technology[t].cost.ToString());

            //Done with technology, on to ships
            data.WriteLine("[Ships]");
            data.WriteLine("; Name = Race,Cost,TPH,Hardpoints,Power,Class,+Equipment (Optional),+Restrictions(Optional)");
            data.WriteLine("; Restrictions:Align(Integer)");

            for(int s = 0; s < currentGame.nrofships; s++)
            {
                int scan, cloak, ig, ds, jump;
                if (currentGame.ship[s].ship_ScanAble) scan = 1; else scan = 0;
                if (currentGame.ship[s].ship_IGAble) ig = 1; else ig = 0;
                if (currentGame.ship[s].ship_JumpAble) jump = 1; else jump = 0;
                if (currentGame.ship[s].ship_CloakAble) cloak = 1; else cloak = 0;
                if (currentGame.ship[s].ship_DSAble) ds = 1; else ds = 0;

                data.WriteLine(currentGame.ship[s].name + "=" + currentGame.ship[s].ship_race.race_name + "," +
                                                                  currentGame.ship[s].ship_cost + "," + 
                                                                  currentGame.ship[s].ship_speed.ToString() + "," +
                                                                  currentGame.ship[s].ship_hardpoints.ToString() + "," +
                                                                  currentGame.ship[s].ship_power.ToString() + "," +
                                                                  currentGame.ship[s].ship_class + "," +
                                                                  "ShipEquipment=Shields=" + currentGame.ship[s].ship_shields.ToString() + ";" +
                                                                  "Armor=" + currentGame.ship[s].ship_armor.ToString() + ";" +
                                                                  "Cargo Holds=" + currentGame.ship[s].ship_cargo.ToString() + ";" +
                                                                  "Combat Drones=" + currentGame.ship[s].ship_combat_drones.ToString() + ";" +
                                                                  "Scout Drones=" + currentGame.ship[s].ship_scout_drones.ToString() + ";" +
                                                                  "Mines=" + currentGame.ship[s].ship_mines.ToString() + ";" +
                                                                  "Scanner=" + scan.ToString() + ";" +
                                                                  "Cloaking Device=" + cloak.ToString() + ";" +
                                                                  "Illusion Generator=" + ig.ToString() + ";" +
                                                                  "Jump Drive=" + jump.ToString() + ";" +
                                                                  "Drone Scrambler=" + ds.ToString() + "," +
                                                                  "Restrictions=" + currentGame.ship[s].ship_restrictions.ToString());
            }

            //Done with ships, now locations
            data.WriteLine("[Locations]");
            data.WriteLine("; Name = +Sells");
            for (int l = 1; l <= currentGame.nroflocations; l++)
            {
                string locline = "";

                locline += currentGame.location[l].location_name + "=";
                switch(currentGame.location[l].location_type)
                {
                    case "Weapon Shop":
                        locline += "Weapons=";
                        for (int i = 0; i < currentGame.location[l].nrofsolditems; i++)
                        {
                            locline += currentGame.weapon[currentGame.location[l].sells[i]].name;
                            if (i != (currentGame.location[l].nrofsolditems - 1))
                                locline += ";";
                        }
                        break;

                    case "Ship Shop":
                        locline += "Ships=";
                        for (int i = 0; i < currentGame.location[l].nrofsolditems; i++)
                        {
                            locline += currentGame.ship[currentGame.location[l].sells[i]].name;
                            if (i != (currentGame.location[l].nrofsolditems - 1))
                                locline += ";";
                        }
                        break;

                    case "Technology Shop":
                        locline += "ShipEquipment=";
                        for (int i = 0; i < currentGame.location[l].nrofsolditems; i++)
                        {
                            locline += currentGame.technology[currentGame.location[l].sells[i]].name;
                            if (i != (currentGame.location[l].nrofsolditems - 1))
                                locline += ";";
                        }
                        break;

                    case "HQ":
                        locline += "HQ=";
                        break;

                    case "UG":
                        locline += "UG=";
                        break;

                    case "Fed":
                        locline += "Fed=";
                        break;

                    case "Bank":
                        locline += "Bank=";
                        break;

                    case "Bar":
                        locline += "Bar=";
                        break;
                }

                data.WriteLine(locline);
            }

            //Locations done, now write the game name and the galaxies
            data.WriteLine("[Game]");
            data.WriteLine("Name=" + currentGame.game_name);
            data.WriteLine("[Galaxies]");
            for (int g = 1; g < currentGame.nrofgalaxies; g++)
                data.WriteLine(g.ToString() + "=" + currentGame.galaxy[g].galaxy_xsize.ToString() + "," +
                                                    currentGame.galaxy[g].galaxy_ysize.ToString() + "," +
                                                    currentGame.galaxy[g].galaxy_type + "," +
                                                    currentGame.galaxy[g].galaxy_name);

            //Done, now finally: the sectors
            for(int g = 1; g < currentGame.nrofgalaxies; g++)
                for (int y = 0; y < currentGame.galaxy[g].galaxy_ysize; y++)
                    for (int x = 0; x < currentGame.galaxy[g].galaxy_xsize; x++)
                    {
                        data.WriteLine("[Sector=" + currentGame.galaxy[g].sector[x, y].sector_id.ToString() + "]");
                        if (currentGame.galaxy[g].sector[x, y].north != null)
                            data.WriteLine("Up=" + currentGame.galaxy[g].sector[x, y].north.sector_id.ToString());
                        if (currentGame.galaxy[g].sector[x, y].south != null)
                            data.WriteLine("Down=" + currentGame.galaxy[g].sector[x, y].south.sector_id.ToString());
                        if (currentGame.galaxy[g].sector[x, y].west != null)
                            data.WriteLine("Left=" + currentGame.galaxy[g].sector[x, y].west.sector_id.ToString());
                        if(currentGame.galaxy[g].sector[x,y].east != null)
                            data.WriteLine("Right=" + currentGame.galaxy[g].sector[x, y].east.sector_id.ToString());
                        if (currentGame.galaxy[g].sector[x, y].warp != null)
                            data.WriteLine("Warp=" + currentGame.galaxy[g].sector[x, y].warp.sector_id.ToString());
                        if (currentGame.galaxy[g].sector[x, y].port != null)
                        {
                            data.WriteLine("Port Level=" + currentGame.galaxy[g].sector[x, y].port.port_level.ToString());
                            data.WriteLine("Port Race=" + currentGame.galaxy[g].sector[x, y].port.port_race.GetRaceNameAsId(currentGame).ToString());
                            string sellLine = "", buyLine = "";
                            for (int good = 0; good < currentGame.good.Length; good++)
                            {
                                if (currentGame.galaxy[g].sector[x, y].port.port_goods[good] == 1)
                                    buyLine += good.ToString() + ",";
                                if (currentGame.galaxy[g].sector[x, y].port.port_goods[good] == -1)
                                    sellLine += good.ToString() + ",";
                            }

                            //Remove the redundant comma at the end
                            buyLine = buyLine.TrimEnd(',');
                            sellLine = sellLine.TrimEnd(',');

                            data.WriteLine("Buys=" + buyLine);
                            data.WriteLine("Sells=" + sellLine);
                        }
                        if (currentGame.galaxy[g].sector[x, y].nroflocations > 0)
                        {
                            string locLine = "Locations=";
                            for (int l = 0; l < currentGame.galaxy[g].sector[x, y].nroflocations; l++)
                                locLine += currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]].location_name + ",";

                            locLine = locLine.TrimEnd(',');
                            data.WriteLine(locLine);
                        }
                        if (currentGame.galaxy[g].sector[x, y].enemy_mines > 0 || currentGame.galaxy[g].sector[x, y].enemy_scouts > 0 || currentGame.galaxy[g].sector[x, y].enemy_cds > 0) 
                        {
                            string forcesLine = "EnemyForces=Owner=";
                            if (currentGame.galaxy[g].sector[x, y].enemy_mines > 0)
                                forcesLine += "Mines=" + currentGame.galaxy[g].sector[x, y].enemy_mines.ToString();
                            if (currentGame.galaxy[g].sector[x, y].enemy_scouts > 0 || currentGame.galaxy[g].sector[x, y].enemy_cds > 0)
                                forcesLine += ";";
                            if (currentGame.galaxy[g].sector[x, y].enemy_scouts > 0)
                                forcesLine += "Scout Drones=" + currentGame.galaxy[g].sector[x, y].enemy_scouts.ToString();
                            if(currentGame.galaxy[g].sector[x, y].enemy_cds > 0)
                                forcesLine += ";";
                            if (currentGame.galaxy[g].sector[x, y].enemy_cds > 0)
                                forcesLine += "Combat Drones=" + currentGame.galaxy[g].sector[x, y].enemy_cds.ToString();

                            data.WriteLine(forcesLine);
                        }

                        if (currentGame.galaxy[g].sector[x, y].friendly_mines > 0 || currentGame.galaxy[g].sector[x, y].friendly_scouts > 0 || currentGame.galaxy[g].sector[x, y].friendly_cds > 0)
                        {
                            string forcesLine = "FriendlyForces=Owner=";
                            if (currentGame.galaxy[g].sector[x, y].friendly_mines > 0)
                                forcesLine += "Mines=" + currentGame.galaxy[g].sector[x, y].friendly_mines.ToString();
                            if (currentGame.galaxy[g].sector[x, y].friendly_scouts > 0 || currentGame.galaxy[g].sector[x, y].friendly_cds > 0)
                                forcesLine += ";";
                            if (currentGame.galaxy[g].sector[x, y].friendly_scouts > 0)
                                forcesLine += "Scout Drones=" + currentGame.galaxy[g].sector[x, y].friendly_scouts.ToString();
                            if (currentGame.galaxy[g].sector[x, y].friendly_cds > 0)
                                forcesLine += ";";
                            if (currentGame.galaxy[g].sector[x, y].friendly_cds > 0)
                                forcesLine += "Combat Drones=" + currentGame.galaxy[g].sector[x, y].friendly_cds.ToString();

                            data.WriteLine(forcesLine);
                        }
                        if (currentGame.galaxy[g].sector[x, y].status == 1)
                            data.WriteLine("Status=Friendly");
                        if (currentGame.galaxy[g].sector[x, y].status == -1)
                            data.WriteLine("Status=Hostile");
                    }

            additionaldata.WriteLine("[Routes]");
            for (int r = 0; r < currentGame.nrofsectors; r++)
                for (int r2 = 0; r2 < currentGame.nrofsectors; r2++)
                {
                    string route = "";
                    if (currentGame.shortestroutes[r, r2] != null)
                    {
                        if (currentGame.shortestroutes[r, r2].sectors.Count > 0)
                        {
                            for (int s = 0; s < currentGame.shortestroutes[r, r2].sectors.Count; s++)
                            {
                                route += currentGame.shortestroutes[r, r2].sectors[s].ToString();
                                if(s != currentGame.shortestroutes[r, r2].sectors.Count - 1)
                                    route += "-";
                                
                            }
                            additionaldata.WriteLine(route);
                        }
                    }
                }

            additionaldata.WriteLine("[Alliance Members]");
            foreach (string am in currentGame.allianceMembers)
            {
                additionaldata.WriteLine(am);
            }

            additionaldata.WriteLine("[Seed Sectors]");
            foreach (int ss in currentGame.seedSectors)
            {
                additionaldata.WriteLine(ss.ToString());
            }

            additionaldata.WriteLine("[Peace]");
            for (int i = 1; i < currentGame.nrofraces+1; i++ )
            {
                if(currentGame.peace[i])
                    additionaldata.WriteLine(currentGame.race[i].race_name + "=Peace");
                else
                    additionaldata.WriteLine(currentGame.race[i].race_name + "=War");
            }
            additionaldata.WriteLine("[Relations]");
            for (int i = 1; i < currentGame.nrofraces + 1; i++)
            {
                additionaldata.WriteLine(currentGame.race[i].race_name + "=" + currentGame.race[i].relations.ToString());
            }

            data.Close();
            additionaldata.Close();
            fs.Close();
            fsa.Close();
            currentGame.saving = false;
            return true;
        }

        #endregion SMR INI Reading and Writing

        public static bool intparse(string toparse, ref int result)
        {
            try { result = int.Parse(toparse); return true; }
            catch (System.FormatException) { return false; }
        }

        private static bool intparse(string toparse, ref ushort result)
        {
            try { result = ushort.Parse(toparse); return true; }
            catch (System.FormatException) { return false; }
        }
    }
}
