using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace MGU
{
    public class Sector : System.Windows.Forms.UserControl
    {
        public Sector east, west, north, south, warp;
        public int sector_id;
        public Port port;
        public int[] location;
        public int nroflocations;
        public Planet planet;
        public bool explored;
        public bool fedprotected;
        public Galaxy galaxy;
        public int distance;
        public int status;
        public int enemy_scouts;
        public int enemy_mines;
        public int enemy_cds;
        public int friendly_scouts;
        public int friendly_mines;
        public int friendly_cds;
        public double highlighted;
        public ArrayList force_stacks;

        public Sector()
        {
            east = null;
            west = null;
            north = null;
            south = null;

            sector_id = 0;
            port = null;
            location = new int[20];
            nroflocations = 0;
            Visible = true;
            status = 0;
            highlighted = 0;
            force_stacks = new ArrayList();

            InitializeComponent();
        }

        public void GetSectorInfo(object sender, System.EventArgs e)
        {
            Sector thisSector = (Sector)sender;

            SectorConfig SectorInfo = new SectorConfig(galaxy.game, thisSector);
            SectorInfo.Closed += new EventHandler(SectorClose);
            SectorInfo.ShowDialog(this);
        }

        public void AddLocation(int loc)
        {
            location[nroflocations] = loc;
            nroflocations++;
        }

        public int GetX(int startsector)
        //Returns the horizontal position of the sector in its galaxy
        {
            int result = (sector_id - galaxy.lowestsectorid) % galaxy.galaxy_xsize;
            result -= (startsector - galaxy.lowestsectorid) % galaxy.galaxy_xsize;
            if (result < 0)
                result += galaxy.galaxy_xsize;
            return result;
        }

        public int GetY(int startsector)
        //Returns the vertical position of the sector in its galaxy
        {
            int result = Convert.ToInt16((sector_id - galaxy.lowestsectorid) / galaxy.galaxy_xsize);
            result -= Convert.ToInt16((startsector - galaxy.lowestsectorid) / galaxy.galaxy_xsize);
            if (result < 0)
                result += galaxy.galaxy_ysize;
            return result;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Game currentGame = galaxy.game;
            this.Width = this.Height = galaxy.game.sectorsize;
            this.SuspendLayout();

            //Draw the background of the sector, if hostile in red, if friendly in green, else it depends on the displaystyle
            if(status == 1)
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, (int)(155 + 100 * (highlighted / 100)), 0)), 0, 0, currentGame.sectorsize, currentGame.sectorsize);
            if(status == -1)
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb((int)(155 + 100 * (highlighted/100)), 0, 0)), 0, 0, currentGame.sectorsize, currentGame.sectorsize);
            if (status == 0)
            {
                if (currentGame.hostApplication.displaystyle == 0)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, (int)(155 + 100 * (highlighted / 100)))), 0, 0, currentGame.sectorsize, currentGame.sectorsize);
                if (currentGame.hostApplication.displaystyle == 1)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(20, (int)(100 + 100 * (highlighted / 100)), 20)), 2, 2, currentGame.sectorsize-4, currentGame.sectorsize-4);
                if (currentGame.hostApplication.displaystyle == 2)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, (int)(155 + 100 * (highlighted / 100)))), 5, 5, currentGame.sectorsize - 10, currentGame.sectorsize - 10);
            }
            
            Brush brush;

            //Draw the walls
            if (currentGame.hostApplication.displaystyle == 0 || currentGame.hostApplication.displaystyle == 2)
                brush = Brushes.White;
            else
                brush = Brushes.Yellow;

            if (currentGame.hostApplication.displaystyle == 0)
            {
                if (north == null)
                    e.Graphics.FillRectangle(brush, 0, 0, currentGame.sectorsize, currentGame.sectorsize/66);
                else
                    e.Graphics.FillRectangle(Brushes.Black, 0, 0, currentGame.sectorsize, 0.5f);

                if (west == null)
                    e.Graphics.FillRectangle(brush, 0, 0, currentGame.sectorsize/66, currentGame.sectorsize);
                else
                    e.Graphics.FillRectangle(Brushes.Black, 0, 0, 0.5f, currentGame.sectorsize);

                if (south == null)
                    e.Graphics.FillRectangle(brush, 0, currentGame.sectorsize - currentGame.sectorsize / 66, currentGame.sectorsize, currentGame.sectorsize / 66);
                else
                    e.Graphics.FillRectangle(Brushes.Black, 0, currentGame.sectorsize - 0.5f, currentGame.sectorsize, 0.5f);

                if(east == null)
                    e.Graphics.FillRectangle(brush, currentGame.sectorsize - currentGame.sectorsize / 66, 0, currentGame.sectorsize / 66, currentGame.sectorsize);
                else
                    e.Graphics.FillRectangle(Brushes.Black, currentGame.sectorsize - 0.5f, 0, 0.5f, currentGame.sectorsize);
            }
            
            if (currentGame.hostApplication.displaystyle == 1)
            {
                if (north != null)
                {
                    e.Graphics.FillEllipse(brush, currentGame.sectorsize / 2 - 4, 6, 4, 4);
                    e.Graphics.FillEllipse(brush, currentGame.sectorsize / 2 + 4, 6, 4, 4);
                }

                if (west != null)
                {
                    e.Graphics.FillEllipse(brush, 6, currentGame.sectorsize / 2 - 4, 4, 4);
                    e.Graphics.FillEllipse(brush, 6, currentGame.sectorsize / 2 + 4, 4, 4);
                }

                if (south != null)
                {
                    e.Graphics.FillEllipse(brush, currentGame.sectorsize / 2 - 4, currentGame.sectorsize - 10, 4, 4);
                    e.Graphics.FillEllipse(brush, currentGame.sectorsize / 2 + 4, currentGame.sectorsize - 10, 4, 4);
                }

                if(east != null)
                {
                    e.Graphics.FillEllipse(brush, currentGame.sectorsize - 10, currentGame.sectorsize / 2 - 4, 4, 4);
                    e.Graphics.FillEllipse(brush, currentGame.sectorsize - 10, currentGame.sectorsize / 2 + 4, 4, 4);
                }
            }

            if (currentGame.hostApplication.displaystyle == 2)
            {
                if (north != null)
                    e.Graphics.FillRectangle(brush, currentGame.sectorsize / 2, 0, 3, 5);

                if (west != null)
                    e.Graphics.FillRectangle(brush, 0, currentGame.sectorsize / 2, 5, 3);

                if (south != null)
                    e.Graphics.FillRectangle(brush, currentGame.sectorsize / 2, currentGame.sectorsize - 5, 3, 5);

                if (east != null)
                    e.Graphics.FillRectangle(brush, currentGame.sectorsize - 5, currentGame.sectorsize / 2, 5, 3);
            }

            //Draw the warp
            if (warp != null)
            {
                if (currentGame.hostApplication.displaystyle == 0)
                    e.Graphics.DrawImage(currentGame.Locations.Images[0], currentGame.sectorsize - 15, 5, currentGame.sectorsize / 8, currentGame.sectorsize / 8);
                if (currentGame.hostApplication.displaystyle == 1)
                    e.Graphics.DrawImage(currentGame.Locations.Images[0], currentGame.sectorsize - 17, 8, currentGame.sectorsize / 8, currentGame.sectorsize / 8);
                if (currentGame.hostApplication.displaystyle == 2)
                    e.Graphics.DrawImage(currentGame.Locations.Images[0], currentGame.sectorsize - 20, 8, currentGame.sectorsize / 8, currentGame.sectorsize / 8);
            }

            //Draw forces
            if (enemy_cds > 0 || enemy_mines > 0 || enemy_scouts > 0)
            {
                if (currentGame.hostApplication.displaystyle == 0)
                    e.Graphics.DrawImage(currentGame.Locations.Images[11], currentGame.sectorsize - 15, currentGame.sectorsize - 15, currentGame.sectorsize / 8, currentGame.sectorsize / 8);
                if (currentGame.hostApplication.displaystyle == 1)
                    e.Graphics.DrawImage(currentGame.Locations.Images[11], currentGame.sectorsize - 17, currentGame.sectorsize - 18, currentGame.sectorsize / 8, currentGame.sectorsize / 8);
                if (currentGame.hostApplication.displaystyle == 2)
                    e.Graphics.DrawImage(currentGame.Locations.Images[11], currentGame.sectorsize - 20, currentGame.sectorsize - 18, currentGame.sectorsize / 8, currentGame.sectorsize / 8);
            }

            if (friendly_cds > 0 || friendly_mines > 0 || friendly_scouts > 0)
            {
                if (currentGame.hostApplication.displaystyle == 0)
                    e.Graphics.DrawImage(currentGame.Locations.Images[12], currentGame.sectorsize - (15 + currentGame.sectorsize / 8), currentGame.sectorsize - 15, currentGame.sectorsize / 8, currentGame.sectorsize / 8);
                if (currentGame.hostApplication.displaystyle == 1)
                    e.Graphics.DrawImage(currentGame.Locations.Images[12], currentGame.sectorsize - (18 + currentGame.sectorsize / 8), currentGame.sectorsize - 17, currentGame.sectorsize / 8, currentGame.sectorsize / 8);
                if (currentGame.hostApplication.displaystyle == 2)
                    e.Graphics.DrawImage(currentGame.Locations.Images[12], currentGame.sectorsize - (20 + currentGame.sectorsize / 8), currentGame.sectorsize - 18, currentGame.sectorsize / 8, currentGame.sectorsize / 8);
                
            }

            //Draw the sector number
            if (port != null)
            {
                if (currentGame.hostApplication.displaystyle == 0)
                    e.Graphics.DrawString(this.sector_id.ToString() + " " + currentGame.GetRaceLetter(port.port_race.race_name), new Font("Arial", 8), Brushes.Yellow, 5, 5);
                if (currentGame.hostApplication.displaystyle == 1)
                    e.Graphics.DrawString(this.sector_id.ToString() + " " + currentGame.GetRaceLetter(port.port_race.race_name), new Font("Arial", 8), Brushes.Yellow, 7, 7);
                if (currentGame.hostApplication.displaystyle == 2)
                    e.Graphics.DrawString(this.sector_id.ToString() + " " + currentGame.GetRaceLetter(port.port_race.race_name), new Font("Arial", 8), Brushes.Yellow, 10, 10);
            }
            else
            {
                if (currentGame.hostApplication.displaystyle == 0)
                    e.Graphics.DrawString(this.sector_id.ToString(), new Font("Arial", 8), Brushes.Yellow, 5, 5);
                if (currentGame.hostApplication.displaystyle == 1)
                    e.Graphics.DrawString(this.sector_id.ToString(), new Font("Arial", 8), Brushes.Yellow, 7, 7);
                if (currentGame.hostApplication.displaystyle == 2)
                    e.Graphics.DrawString(this.sector_id.ToString(), new Font("Arial", 8), Brushes.Yellow, 10, 10);
            }
            
            //Draw the planet
            if(planet != null)
                e.Graphics.DrawImage(currentGame.Locations.Images[9], (short)((currentGame.sectorsize - 16) / 2), (short)((currentGame.sectorsize - 16) / 2), (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5));

            if (currentGame.sectorsize > 50)
            {
                //If sector size is sufficiently large, then draw the Port, if there is one
                if (port != null)
                {
                    int totalbuy = 0, totalsell = 0;
                    if (currentGame.hostApplication.displaystyle == 0)
                    {
                        e.Graphics.DrawImage(currentGame.Miscellaneous.Images[7], 5, (currentGame.sectorsize - 4) - (float)(currentGame.sectorsize / 8), 4, (float)(currentGame.sectorsize / 8));
                        e.Graphics.DrawImage(currentGame.Miscellaneous.Images[6], 5, (currentGame.sectorsize - 4) - 2 * (float)(currentGame.sectorsize / 8), 4, (float)(currentGame.sectorsize / 8));
                    }
                    if (currentGame.hostApplication.displaystyle == 1)
                    {
                        e.Graphics.DrawImage(currentGame.Miscellaneous.Images[7], 7, (currentGame.sectorsize - 6) - (float)(currentGame.sectorsize / 8), 4, (float)(currentGame.sectorsize / 8));
                        e.Graphics.DrawImage(currentGame.Miscellaneous.Images[6], 7, (currentGame.sectorsize - 6) - 2 * (float)(currentGame.sectorsize / 8), 4, (float)(currentGame.sectorsize / 8));
                    }
                    if (currentGame.hostApplication.displaystyle == 2)
                    {
                        e.Graphics.DrawImage(currentGame.Miscellaneous.Images[7], 10, (currentGame.sectorsize - 14) - (float)(currentGame.sectorsize / 8), 4, (float)(currentGame.sectorsize / 8));
                        e.Graphics.DrawImage(currentGame.Miscellaneous.Images[6], 10, (currentGame.sectorsize - 14) - 2 * (float)(currentGame.sectorsize / 8), 4, (float)(currentGame.sectorsize / 8));
                    }
                    for (int curgood = 1; curgood < currentGame.hostApplication.Goods.Images.Count-12; curgood += 1)
                    {
                        if (port.port_goods[curgood] == 1)
                        {
                            if (currentGame.good[curgood].goodorevil || currentGame.displayIllegals)
                            {
                                if (currentGame.hostApplication.displaystyle == 0)
                                    e.Graphics.DrawImage(currentGame.hostApplication.Goods.Images[curgood], 10 + (float)(currentGame.sectorsize / 7.5) * totalbuy, (currentGame.sectorsize - 3) - 2 * (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5));
                                if (currentGame.hostApplication.displaystyle == 1)
                                    e.Graphics.DrawImage(currentGame.hostApplication.Goods.Images[curgood], 12 + (float)(currentGame.sectorsize / 7.5) * totalbuy, (currentGame.sectorsize - 5) - 2 * (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5));
                                if (currentGame.hostApplication.displaystyle == 2)
                                    e.Graphics.DrawImage(currentGame.hostApplication.Goods.Images[curgood+12], 15 + (float)(currentGame.sectorsize / 7.5) * totalbuy, (currentGame.sectorsize - 10) - 2 * (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5));
                                totalbuy++;
                            }
                        }
                        else if (port.port_goods[curgood] == -1)
                        {
                            if (currentGame.good[curgood].goodorevil || currentGame.displayIllegals)
                            {
                                if (currentGame.hostApplication.displaystyle == 0)
                                    e.Graphics.DrawImage(currentGame.hostApplication.Goods.Images[curgood], 10 + (float)(currentGame.sectorsize / 7.5) * totalsell, (currentGame.sectorsize - 3) - (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5));
                                if (currentGame.hostApplication.displaystyle == 1)
                                    e.Graphics.DrawImage(currentGame.hostApplication.Goods.Images[curgood], 12 + (float)(currentGame.sectorsize / 7.5) * totalsell, (currentGame.sectorsize - 5) - (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5));
                                if (currentGame.hostApplication.displaystyle == 2)
                                    e.Graphics.DrawImage(currentGame.hostApplication.Goods.Images[curgood+12], 15 + (float)(currentGame.sectorsize / 7.5) * totalsell, (currentGame.sectorsize - 10) - (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5));
                                totalsell++;
                            }
                        }
                    }
                }
            }
            
            //Draw Locations
            int lindex;
            for (int l = 0; l < nroflocations; l++)
            {
                lindex = currentGame.GetLocationIconIndex(currentGame.location[this.location[l]]);
                e.Graphics.DrawImage(currentGame.Locations.Images[lindex], (currentGame.sectorsize / 2) - ((float)(currentGame.sectorsize / 7.5) * (nroflocations / 2)) + (float)(currentGame.sectorsize / 8) * l, currentGame.sectorsize / 2 - (float)(currentGame.sectorsize / 15), (float)(currentGame.sectorsize / 7.5), (float)(currentGame.sectorsize / 7.5));
            }

            //DrawFocus(e.Graphics);
            this.ResumeLayout(false);
        }

        public void RecursiveDistance(int value, bool[] galallowed, bool evade)
        {
            distance = value;

            if (evade && status == -1)
            {
                distance = -1;
                return;
            }

            if (east != null && east.west != null && (east.distance == -1 || east.distance > value + 1))
                east.RecursiveDistance(value + 1, galallowed, evade);
            if (west != null && west.east != null && (west.distance == -1 || west.distance > value + 1))
                west.RecursiveDistance(value + 1, galallowed, evade);
            if (south != null && south.north != null && (south.distance == -1 || south.distance > value + 1))
                south.RecursiveDistance(value + 1, galallowed, evade);
            if (north != null && north.south != null && (north.distance == -1 || north.distance > value + 1))
                north.RecursiveDistance(value + 1, galallowed, evade);
            if (warp != null && warp.warp != null && galallowed[warp.galaxy.galaxy_id] && (warp.distance == -1 || warp.distance > value + 5))
                warp.RecursiveDistance(value + 5, galallowed, evade);
        }

        private void SectorClose(object sender, System.EventArgs e)
        {
            int x, nr;

            //First remove all empty locations
            for (x = nr = 0; x < 5 && nr < 15; x++)
            {
                if (location[x] == 0)
                {
                    for (int i = x + 1; i < 5; i++)
                        location[i - 1] = location[i];
                    x--;
                }

                nr++;
            }

            //Then determine nr of locations

            for (x = nroflocations = 0; x < 5; x++)
            {
                if (location[x] == 0)
                    break;
                else
                    nroflocations++;
            }

            // Make sure the sector is redrawn
            Invalidate();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Sector
            // 
            this.Name = "Sector";
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Sector_MouseDoubleClick);
            //this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Sector_MouseClick);
            this.ResumeLayout(false);
        }

        private void Sector_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Game currentGame = galaxy.game;
            if (e.X > currentGame.sectorsize - 21 && e.X < currentGame.sectorsize - 5 && e.Y > 5 && e.Y < 21)
            {
                currentGame.hostApplication.ChangeGalaxy(currentGame.GetGalaxyIndex(warp.sector_id));
                currentGame.hostApplication.panel1.AutoScrollPosition = new Point(0, 0);
                currentGame.hostApplication.Redraw();
            }
            else
                GetSectorInfo(sender, e);
        }
    }
}