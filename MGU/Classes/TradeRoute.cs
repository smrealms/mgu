using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace MGU
{
    public class TradeRoute : EdgeRoute
    {
        int sourcegood, returngood;

        public TradeRoute(int newsource, int newreturn)
        {
            sourcegood = newsource;
            returngood = newreturn;
        }
    }
}
