using System;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace MGU
{
    public class Race
	//This class holds the information for race
    {
        public string race_name;
        public int relations;

        public Race()
        {
            race_name = "Neutral";
            relations = 0;
        }

        public Race(string newname)
        {
            race_name = newname;
            relations = 0;
        }

        public ushort GetRaceNameAsId(Game currentGame)
	//This function returns the index of a race within a given game
        {
            for (int i = 0; i <= currentGame.nrofraces; i++)
                if (currentGame.race[i].race_name == this.race_name)
                    return (ushort) i;
            
            return 0;
        }
    }
}
