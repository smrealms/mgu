using System;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 

namespace MGU
{
    public class Good
	//This class currently contains only the base variables. It can in the future be used to generate trade route upgrade scheduled or if the trade system is ever extended.
    {
        public string good_name;
        public string good_image;
        public int good_price;
        public bool goodorevil; //Good = true, evil = false

        public Good()
        {
            good_name = "Undefined";
            good_image = "Undefined";
            good_price = 0;
            goodorevil = true;
        }
    }
}
