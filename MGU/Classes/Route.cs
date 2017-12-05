using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace MGU
{
    public class Route
	//This is one of the most important classes in the application: it holds the information on route, and optionally for the goods that are traded on that route, and it is used in all MGU functions: plot course, arming routes, trade routes and the location finder
    {
        public int length;
        public ArrayList sectors;
        public int warps;
        public ArrayList waypoints;
        public int sourcegood;
        public int middlegood;
        public int returngood;

	//Variables only used for trade routes
        public int multiplierbuysource, multipliersellsource, multiplierbuyreturn, multipliersellreturn, multiplierbuymiddle, multipliersellmiddle;
        public double cash;
        public double experience;

        public Game currentGame;

        public Route(Game game)
        {
            length = 0;
            warps = 0;
            sectors = new ArrayList();
            waypoints = new ArrayList();

            sourcegood = 0;
            returngood = 0;
            middlegood = 0;
            multiplierbuysource = 0;
            multipliersellsource = 0;
            multiplierbuyreturn = 0;
            multipliersellreturn = 0;
            multiplierbuymiddle = 0;
            multipliersellmiddle = 0;

            currentGame = game;
        }

        public Route(Route oldRoute)
        {
            length = oldRoute.length;
            warps = oldRoute.warps;
            waypoints = (ArrayList) oldRoute.waypoints.Clone();
            sectors = (ArrayList)oldRoute.sectors.Clone();
            waypoints = (ArrayList) oldRoute.waypoints.Clone();

            currentGame = oldRoute.currentGame;
        }

        public void Copy(Route copy)
        {
            length = copy.length;
            warps = copy.warps;
            sectors = (ArrayList) copy.sectors.Clone();
            waypoints = (ArrayList) copy.waypoints.Clone();

            currentGame = copy.currentGame;
        }

        public override bool Equals(object obj)
        {
            Route target = (Route) obj;
            if(this.length != target.length)
                return false;

            if(this.warps != target.warps)
                return false;
            
            //for(int s = 0; s < sectors.Count; s++)
            //    if((int)this.sectors[s] != (int)target.sectors[s])
            //        return false;

            if (Convert.ToInt16(this.sectors[0]) != Convert.ToInt16(target.sectors[0]))
                return false;

            if (Convert.ToInt16(this.sectors[this.sectors.Count - 1]) != Convert.ToInt16(target.sectors[target.sectors.Count - 1]))
                return false;

            for(int w = 0; w < waypoints.Count; w++)
                if(Convert.ToInt16(this.waypoints[w]) != Convert.ToInt16(target.waypoints[w]))
                   return false;

            if(!this.currentGame.Equals(target.currentGame))
                return false;

            if (this.returngood != target.returngood)
                return false;

            if (this.sourcegood != target.sourcegood)
                return false;

            return true;
        }

        public Route AppendRoute(Route routeToAppend)
        //This functions adds the given route to this route and returns the resulting route
        {
            Route newRoute = new Route(this.currentGame);
            newRoute.Copy(this);

            newRoute.length += routeToAppend.length;
            newRoute.warps += routeToAppend.warps;
            for (int i = 1; i < routeToAppend.sectors.Count; i++)
                newRoute.sectors.Add(routeToAppend.sectors[i]);

            return newRoute;
        }

        public void AppendRoute(int waypoint, bool[] galallowed, bool evade)
	    //This function adds to an existing route by adding a new waypoint. The old end sector becomes a waypoint and the given waypoint the new end sector, and the route between waypoint and end sector is added to the route.
        {
            Route newRoute;

            //if the given waypoint is invalid or identical to the current end sector, return
            if(waypoint == Convert.ToInt16(sectors[sectors.Count-1]) || waypoint == -1)
                return;

            //Create a new, empty, route and indicate that it is in the current game
            newRoute = new Route(this.currentGame);

            //Compute the new route as the route between the end sector of this route and the given waypoint
            newRoute.Calculate(Convert.ToInt16(sectors[sectors.Count - 1]), waypoint, galallowed, evade);

            this.length += newRoute.length;
            this.warps += newRoute.warps;
            for(int i = 1; i < newRoute.sectors.Count; i++)
                this.sectors.Add(newRoute.sectors[i]);
            AddWaypoint(waypoint);
        }

        public Route ReverseRoute()
	//This function completely reverses the route, including all waypoints and the source and return good
        {
            Route newRoute = new Route(currentGame);

            newRoute.currentGame = currentGame;
            newRoute.length = length;
            newRoute.warps = warps;

            for(int i = this.sectors.Count-1; i >= 0; i--)
            {
                newRoute.sectors.Add(sectors[i]);
            }

            for (int i = this.waypoints.Count - 1; i >= 0; i--)
            {
                newRoute.waypoints.Add(waypoints[i]);
            }

            newRoute.returngood = sourcegood;
            newRoute.sourcegood = returngood;

            return newRoute;
        }

        public void Calculate(int beginSector, int endSector, bool []galallowed, bool evade)
	    //This function finds a route between the given start and end sector
        {
            Route shortestroute;
            Sector testSector;

            //If the route to be calculated and is given in the shortestroute matrix of the game, return this stored route
            if (currentGame.shortestroutes[beginSector, endSector] != null && currentGame.shortestroutes[beginSector, endSector].sectors.Count > 0)
            {
                this.Copy(currentGame.shortestroutes[beginSector, endSector]);
                for(int i = 0; i < this.sectors.Count - 1; i++)
                {
                    if(currentGame.GetSectorObject(Convert.ToInt16(sectors[i])).warp != null)
                        if(currentGame.GetSectorObject(Convert.ToInt16(sectors[i])).warp.sector_id == Convert.ToInt16(sectors[i+1]))
                            warps++;
                }
                return;
            }

            //If no galallowed is given, then allow all galaxies
            if (galallowed == null)
            {
                galallowed = new bool[currentGame.nrofgalaxies];
                for (int i = 0; i < currentGame.nrofgalaxies; i++)
                    galallowed[i] = true;
            }

            //First clear all distance variables
            for (int g = 0; g < currentGame.nrofgalaxies; g++)
            {
                for (int x = 0; x < currentGame.galaxy[g].galaxy_xsize; x++)
                    for (int y = 0; y < currentGame.galaxy[g].galaxy_ysize; y++)
                    {
                        currentGame.galaxy[g].sector[x, y].distance = -1;
                    }
            }

            //Compute all distances. A brute force, depth-first search is used.
            currentGame.GetSectorObject(endSector).RecursiveDistance(0, galallowed, evade);

            //Find route
            length = currentGame.GetSectorObject(beginSector).distance;
            //If no route has been found, then return
            if (length == -1)
                return;
            sectors = new ArrayList(length+1);
            int nextSector = beginSector;
            while (nextSector != endSector)
            {
                Sector currentSector = currentGame.GetSectorObject(nextSector);
                sectors.Add(nextSector);

                if (currentSector.east != null)
                    if(currentSector.east.distance == currentSector.distance-1)
                    {
                        nextSector = currentSector.east.sector_id;
                        continue;
                    }
                if(currentSector.west != null)
                    if (currentSector.west.distance == currentSector.distance - 1)
                    {
                        nextSector = currentSector.west.sector_id;
                        continue;
                    }
                if(currentSector.south != null)
                    if (currentSector.south.distance == currentSector.distance - 1)
                    {
                        nextSector = currentSector.south.sector_id;
                        continue;
                    }
                if(currentSector.north != null)
                    if (currentSector.north.distance == currentSector.distance - 1)
                    {
                        nextSector = currentSector.north.sector_id;
                        continue;
                    }
                if (currentSector.warp != null)
                    if (currentSector.warp.distance == currentSector.distance - 5)
                    {
                        nextSector = currentSector.warp.sector_id;
                        warps += 1;
                        continue;
                    }
            }
            sectors.Add(endSector);
            
            currentGame.shortestroutes[beginSector, endSector] = this;
            currentGame.shortestroutes[endSector, beginSector] = this.ReverseRoute();
            currentGame.smachanged = true;


            //int e = this.sectors.Count - 1;

            /*for(int s = 1; s < this.sectors.Count; s++)
                //for (int e = s; e < this.sectors.Count; e++)
                {
                    if (Math.Abs(s - e) >= 1)
                    {
                        if(currentGame.shortestroutes[Convert.ToInt16(this.sectors[s]), Convert.ToInt16(this.sectors[e])] == null)
                        {
                            Route newRoute;

                            newRoute = this.SubRoute(s, e);
                            currentGame.shortestroutes[Convert.ToInt16(newRoute.sectors[0]), Convert.ToInt16(newRoute.sectors[newRoute.sectors.Count-1])] = newRoute;
                            currentGame.shortestroutes[Convert.ToInt16(newRoute.sectors[newRoute.sectors.Count - 1]), Convert.ToInt16(newRoute.sectors[0])] = newRoute.ReverseRoute();
                            currentGame.smachanged = true;
                        }
                    }
                }*/
        }

        public int GetFirstLength()
        {
            int result = 0;

            if (waypoints.Count == 0)
                return -1;
            else for(int s = 0; s < sectors.Count; s++)
                if(Convert.ToInt16(sectors[s]) != Convert.ToInt16(waypoints[0]))
                    result++;
                else
                    return result;

            return -1;
        }

        public int GetSecondLength()
        {
            int result = 0;
            int s = 0;

            if (waypoints.Count <= 1)
                return -1;
            else
            {
                for(; s < sectors.Count; s++)
                    if(Convert.ToInt16(sectors[s]) != Convert.ToInt16(waypoints[0]))
                        continue;
                    else
                        break;

                for(; s < sectors.Count; s++)
                    if(Convert.ToInt16(sectors[s]) != Convert.ToInt16(waypoints[1]))
                        result++;
                    else
                        return result;
            }

            return -1;
        }

        public int GetThirdLength()
        {
            int result = 0;
            int s = 0;

            if (waypoints.Count <= 1)
                return -1;
            else
            {
                for (; s < sectors.Count; s++)
                    if (Convert.ToInt16(sectors[s]) != Convert.ToInt16(waypoints[0]))
                        continue;
                    else
                        break;

                for (; s < sectors.Count; s++)
                    if (Convert.ToInt16(sectors[s]) != Convert.ToInt16(waypoints[1]))
                        continue;
                    else
                        break;

                for (; s < sectors.Count; s++)
                    if (Convert.ToInt16(sectors[s]) != Convert.ToInt16(sectors[sectors.Count - 1]))
                        result++;
                    else
                        return result;
            }

            return -1;
        }

        public Route SubRoute(int start, int end)
        {
            Route returnRoute = new Route(this);

            for (int s = 0; s < start; s++)
            {
                returnRoute.length -= 1;
                if (currentGame.GetSectorObject(Convert.ToInt16(returnRoute.sectors[0])).galaxy.galaxy_name != currentGame.GetSectorObject(Convert.ToInt16(returnRoute.sectors[1])).galaxy.galaxy_name)
                {
                    returnRoute.length -= 4;
                    returnRoute.warps -= 1;
                }
                returnRoute.sectors.RemoveAt(0);
            }

            for (int s = end; s < this.sectors.Count-1; s++)
            {
                returnRoute.length -= 1;
                if (currentGame.GetSectorObject(Convert.ToInt16(returnRoute.sectors[end - start])).galaxy.galaxy_name != currentGame.GetSectorObject(Convert.ToInt16(returnRoute.sectors[end - start - 1])).galaxy.galaxy_name)
                {
                    returnRoute.length -= 4;
                    returnRoute.warps -= 1;
                }
                returnRoute.sectors.RemoveAt(end-start);
            }

            return returnRoute;
        }

        public void AddWaypoint(int newwp)
        {
            waypoints.Add(newwp);
        }
    }
}
