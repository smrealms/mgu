using System;
using System.Collections;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace MGU
{
    public class Galaxy
	//This class is the placeholder for all the sectors within the galaxy. In addition, there is an upward pointing reference to the game in which the galaxy exists
    {
	    //Basic galaxy variables
        public string galaxy_name;
        public int galaxy_xsize, galaxy_ysize;
        public int galaxy_id;
        public string galaxy_type;
        public ArrayList neighbours;

	    //The sector variable is a two-dimenional array of sector objects
        public Sector[,] sector;

	    //The class has three sector indicators. Lowestsectorid is the smallest sector number available in the galaxy. The first galaxy has a lowestsectorid of 1.
	    //The variable startsector indicates what sector is the topleft one in the viewing screen. Therefore it changes as a user scrolls through the galaxy.
	    //The currentsector variable indicates the sector that is currently being viewed/edited
        public int startsector;
        public int lowestsectorid;
        public int currentsector;

	    //The game variable references the game class to which this galaxy belongs
        public Game game;

        public Galaxy()
        {
            neighbours = new ArrayList();
            galaxy_name = "Undefined";
            galaxy_xsize = 0;
            galaxy_ysize = 0;
        }

        public void ClearGalaxy()
        //This function clears all sectors and other variables from the galaxy
        {
            for (int x = 0; x < galaxy_xsize; x++)
                for (int y = 0; y < galaxy_ysize; y++)
                {
                    sector[x, y] = null;
                }

            galaxy_name = "";
            galaxy_xsize = galaxy_ysize = 0;
            galaxy_id = -1;
            startsector = lowestsectorid = currentsector = -1;
        }

        public void InitializeGalaxy(int xsize, int ysize)
	    //This constructor creates a new galaxy based on the given size and fills it with sectors
        {
            sector = new Sector[xsize, ysize];

            for (int x = 0; x < xsize; x++)
                for (int y = 0; y < ysize; y++)
                {
                    sector[x, y] = new Sector();
                    sector[x, y].sector_id = lowestsectorid + y * xsize + x;
                    sector[x, y].galaxy = this;
                }

            startsector = lowestsectorid;
        }

        public short GetGalaxyID()
	//This function determines what, in the list of galaxies for the game, is the index of this specific galaxy. It assumes that no duplicate galaxy names exist.
        {
            for (short i = 0; i < game.nrofgalaxies; i++)
            {
                if (game.galaxy[i].galaxy_name == this.galaxy_name)
                    return i;
            }

            return -1;
        }
    }
}