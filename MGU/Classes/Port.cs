using System;
using System.Collections;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace MGU
{
    public class Port
	//This class contains port information. Besides the obvious variables, it also keeps track of the complementary ports (and distance to those ports) for each good. For example, if this port buys wood, then complementary_sectors[0] will contain a reference to the closest sector that sells wood, and complementary_distances[0] holds the distance to that port.
	//Port references are Sector objects, while distances are integers.
    {
        public int port_level;
        public Race port_race;
        public int[] port_goods;
        public ArrayList[] complementary_sectors;
        public ArrayList[] complementary_distances;

	    public Port()
        {
        }

        public Port(int nrofgoods)
		//This function allocates memory for the given nrofgoods
        {
            port_goods = new int[nrofgoods+1];
            complementary_distances = new ArrayList[nrofgoods + 1];
            complementary_sectors = new ArrayList[nrofgoods + 1];

            for (int index = 0; index <= nrofgoods; index++)
            {
                port_goods[index] = 0;
                complementary_distances[index] = new ArrayList();
                complementary_sectors[index] = new ArrayList();
            }

            port_goods[0] = 2;
        }

        public void Reset()
	//This function removes all the info on complementary distances
        {
            int nrofgoods = port_goods.Length;
            complementary_distances = new ArrayList[nrofgoods + 1];
            complementary_sectors = new ArrayList[nrofgoods + 1];

            for (int index = 0; index <= nrofgoods; index++)
            {
                complementary_distances[index] = new ArrayList();
                complementary_sectors[index] = new ArrayList();
            }
        }
    }
}
