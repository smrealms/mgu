using System;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace MGU
{
    public class Item
    //This class functions as a superclass for technology, ship and weapon.
    {
	    //Name of the item
        public string name;

	    //The name of the shop that it is being sold at. This is not being used at the moment and should point directly to the shop class, rather than be a string
        public string sold_at;

	    //Cost of the item
        public long cost;

        public Item()
        {
            name = "";
            sold_at = "";
        }
    }
}
