using System;

/// <summary>
/// Summary description for Class1
/// </summary>


namespace MGU
{
    public class Location
	//This function contains variables for the storage of locations and what they sell
    {
        public string location_name;
        public string location_image;
        public string location_type;
        public int[] sells;
        public int nrofsolditems;

        public Location()
        {
            location_name = "";
            location_type = "";
            location_image = "";
            nrofsolditems = 0;
        }

        public void Initialize(int numberofitems)
	    //This function returns the number of items that are being sold at the location
        {
            sells = new int[numberofitems];
            nrofsolditems = numberofitems;
        }

        public bool Sells(int item)
    	//This function returns a boolean that indicates if the given item nr is sold.
        {
            for (int i = 0; i < nrofsolditems; i++)
                if (sells[i] == item)
                    return true;

            return false;
        }
    }
}