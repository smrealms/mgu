/* Copyright 2009 Robin Langerak
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.

 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MGU
{
	public class PlotWindow : System.Windows.Forms.Form
	{
        private MainStuff hostApplication;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox To;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RichTextBox Results;
		private System.Windows.Forms.Button MyCancelButton;
		private System.Windows.Forms.TextBox From;
		private bool _destroyed = false;
		private System.Windows.Forms.GroupBox GalaxyList;
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox checkBox1;
		bool[] allowed;
        private Button HighlightRouteButton;
        bool evade = false;
        public Route plottedRoute = null;

		public bool destroyed
		{
			get { return _destroyed; }
		}

		public PlotWindow(MainStuff host)
		{
            hostApplication = host;
            
			InitializeComponent();

            int nrofgalaxies = hostApplication.games[hostApplication.currentGame].nrofgalaxies;

            //Add the galaxies to the list of allowed galaxies. Do not add the first, Zero, galaxy
            for(int g = 1; g < nrofgalaxies; g++)
                checkedListBox1.Items.Add((object) hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_name);
			for (int x = 0; x < checkedListBox1.Items.Count; x += 1)
				checkedListBox1.SetItemChecked(x, true);
			allowed = new bool[nrofgalaxies];
			for (int x = 0; x < allowed.Length; x += 1)
				allowed[x] = true;
			checkedListBox1.ItemCheck += new ItemCheckEventHandler(ItemCheck_Changed);
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			_destroyed = true;
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.From = new System.Windows.Forms.TextBox();
            this.To = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Results = new System.Windows.Forms.RichTextBox();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.GalaxyList = new System.Windows.Forms.GroupBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.HighlightRouteButton = new System.Windows.Forms.Button();
            this.GalaxyList.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            // 
            // From
            // 
            this.From.Location = new System.Drawing.Point(46, 12);
            this.From.MaxLength = 6;
            this.From.Name = "From";
            this.From.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.From.Size = new System.Drawing.Size(52, 20);
            this.From.TabIndex = 1;
            this.From.TextChanged += new System.EventHandler(this.From_TextChanged);
            // 
            // To
            // 
            this.To.Location = new System.Drawing.Point(126, 12);
            this.To.MaxLength = 6;
            this.To.Name = "To";
            this.To.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.To.Size = new System.Drawing.Size(52, 20);
            this.To.TabIndex = 2;
            this.To.TextChanged += new System.EventHandler(this.To_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(102, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "To:";
            // 
            // Results
            // 
            this.Results.BackColor = System.Drawing.SystemColors.Control;
            this.Results.Location = new System.Drawing.Point(8, 40);
            this.Results.Name = "Results";
            this.Results.ReadOnly = true;
            this.Results.Size = new System.Drawing.Size(295, 232);
            this.Results.TabIndex = 4;
            this.Results.Text = "Please fill in <From> and <To> with sector numbers";
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(397, 284);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(75, 23);
            this.MyCancelButton.TabIndex = 5;
            this.MyCancelButton.Text = "Close";
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // GalaxyList
            // 
            this.GalaxyList.Controls.Add(this.checkedListBox1);
            this.GalaxyList.ForeColor = System.Drawing.SystemColors.ControlText;
            this.GalaxyList.Location = new System.Drawing.Point(309, 12);
            this.GalaxyList.Name = "GalaxyList";
            this.GalaxyList.Size = new System.Drawing.Size(164, 264);
            this.GalaxyList.TabIndex = 20;
            this.GalaxyList.TabStop = false;
            this.GalaxyList.Text = "Allowed gals";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Location = new System.Drawing.Point(8, 31);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(148, 229);
            this.checkedListBox1.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(187, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(120, 16);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "Evade \'red\' sectors";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // HighlightRouteButton
            // 
            this.HighlightRouteButton.Location = new System.Drawing.Point(251, 284);
            this.HighlightRouteButton.Name = "HighlightRouteButton";
            this.HighlightRouteButton.Size = new System.Drawing.Size(140, 23);
            this.HighlightRouteButton.TabIndex = 22;
            this.HighlightRouteButton.Text = "Highlight and go to Route";
            this.HighlightRouteButton.UseVisualStyleBackColor = true;
            this.HighlightRouteButton.Click += new System.EventHandler(this.HighlightRouteButton_Click);
            // 
            // PlotWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(486, 311);
            this.Controls.Add(this.HighlightRouteButton);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.GalaxyList);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.Results);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.To);
            this.Controls.Add(this.From);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlotWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Plot your course";
            this.Load += new System.EventHandler(this.PlotWindow_Load);
            this.GalaxyList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private bool NotNumbered(string text)
		{
			byte[] mynumbers = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
			foreach (char current in mynumbers)
			{
				if (current < '0' || current > '9')
					return true;
			}
			return false;
		}

		private ushort readSectorNr(string sector)
		{
			ushort returnvalue;
			if (sector == "") return 0;
			try 
			{
				if (sector[0] == '#')
					sector = sector.Substring(1, sector.Length - 1);
				returnvalue = ushort.Parse(sector);
				if (returnvalue > hostApplication.games[hostApplication.currentGame].nrofsectors || returnvalue < 0)
					return 0;
				return returnvalue;
			}
			catch (FormatException)
			{
				return 0;
			}
		}

		public void Recalculate()
        //This function calculates a shortest route between the sectors in the inputfields and displays it.
        //Note that there may be more shortest routes: only one is calculated
		{
            Game currentGame = hostApplication.games[hostApplication.currentGame];
			int firstsector, lastsector;

            //Obtain and check start and end sector
			firstsector = readSectorNr(From.Text);
			lastsector = readSectorNr(To.Text);
			if (firstsector == 0 || lastsector == 0)
			{
				Results.Text = "Please fill in <From> and <To> with sector numbers";
				return;
			}

            if (plottedRoute != null)
            {
                int length = plottedRoute.length;
                for (int s = 0; s < plottedRoute.sectors.Count; s++)
                    currentGame.GetSectorObject(Convert.ToInt16(plottedRoute.sectors[s])).highlighted = 0;
            }

            plottedRoute = new Route(hostApplication.games[hostApplication.currentGame]);

            //Do the actual calculation
            plottedRoute.Calculate(firstsector, lastsector, allowed, evade);

			string resultText;
			if (plottedRoute.length == -1)
			{
				Results.Text = "No route has been found between " + firstsector + " and " + lastsector + " with the current settings.";
				return;
			}
            resultText = "Shortest route goes through " + (plottedRoute.sectors.Count - 1).ToString() + " sectors, " + plottedRoute.warps + " warps and it takes " + (plottedRoute.sectors.Count + 4 * plottedRoute.warps - 1).ToString() +" turns.";

            resultText += "\n\n" + hostApplication.games[hostApplication.currentGame].galaxy[hostApplication.games[hostApplication.currentGame].GetGalaxyIndex(Convert.ToInt16(plottedRoute.sectors[0]))].galaxy_name + " galaxy:\n";
            resultText += plottedRoute.sectors[0].ToString();

            if (currentGame.GetSectorObject(Convert.ToInt16(plottedRoute.sectors[0])).warp != null)
            {
                if (currentGame.GetSectorObject(Convert.ToInt16(plottedRoute.sectors[0])).warp.sector_id == Convert.ToInt16(plottedRoute.sectors[1]))
                {
                    resultText += " (warp) \n\n" + hostApplication.games[hostApplication.currentGame].galaxy[hostApplication.games[hostApplication.currentGame].GetGalaxyIndex(Convert.ToInt16(plottedRoute.sectors[0]))].galaxy_name + " galaxy:\n";
                }
            }

			for(int s = 1; s < plottedRoute.sectors.Count; s++)
			{
                if(s < plottedRoute.sectors.Count - 1)
                {
                    if (currentGame.GetSectorObject(Convert.ToInt16(plottedRoute.sectors[s])).warp != null)
                    {
                        if (currentGame.GetSectorObject(Convert.ToInt16(plottedRoute.sectors[s])).warp.sector_id == Convert.ToInt16(plottedRoute.sectors[s + 1]))
                        {
                            resultText += " (warp) \n\n" + hostApplication.games[hostApplication.currentGame].galaxy[hostApplication.games[hostApplication.currentGame].GetGalaxyIndex(Convert.ToInt16(plottedRoute.sectors[0]))].galaxy_name + " galaxy:\n";
                        }
                        else if((currentGame.GetSectorObject(Convert.ToInt16(plottedRoute.sectors[s])).warp.sector_id != Convert.ToInt16(plottedRoute.sectors[s - 1])))
                            resultText += " - ";
                    }
                    else
                        resultText += " - ";
                }
                else
                    resultText += " - ";

                resultText += plottedRoute.sectors[s].ToString();
			}
			Results.Text = resultText;
		}

		private void ItemCheck_Changed(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			allowed[e.Index] = e.NewValue == CheckState.Checked;
            hostApplication.games[hostApplication.currentGame].ResetShortestRoutes();
			Recalculate();
		}

		private void From_TextChanged(object sender, System.EventArgs e)
		{
			Recalculate();
		}

		private void To_TextChanged(object sender, System.EventArgs e)
		{
			Recalculate();
		}

		private void MyCancelButton_Click(object sender, System.EventArgs e)
		{
			this.Hide();
            hostApplication.toolBarButton5.Pushed = false;
		}

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            evade = !evade;
            hostApplication.games[hostApplication.currentGame].ResetShortestRoutes();
            Recalculate();
        }

        private void HighlightRouteButton_Click(object sender, EventArgs e)
        {
            Game currentGame = hostApplication.games[hostApplication.currentGame];

            if (currentGame.highlightedRoute != null)
            {
                int length = currentGame.highlightedRoute.length;
                for (int s = 0; s < length + 1; s++)
                    hostApplication.games[hostApplication.currentGame].GetSectorObject((int)currentGame.highlightedRoute.sectors[s]).highlighted = 0;
            }

            if (Results.Text.StartsWith("Please") || Results.Text.StartsWith("No route"))
            {
                MessageBox.Show("No route has been plotted yet");
                return;
            }

            for(int s = 0; s < plottedRoute.sectors.Count; s++)
                currentGame.GetSectorObject(Convert.ToInt16(plottedRoute.sectors[s])).highlighted = 30 + ((double)s)/((double)plottedRoute.length) * 50;

            hostApplication.games[hostApplication.currentGame].highlightedRoute = plottedRoute;

            if (Results.Text.StartsWith("Please") || Results.Text.StartsWith("No route"))
            {
                MessageBox.Show("No route has been plotted yet");
                return;
            }

            int sectornr = Convert.ToInt16(plottedRoute.sectors[0]);
            int galaxynr = currentGame.GetGalaxyIndex(sectornr);
            currentGame.currentGalaxy = galaxynr;

            //Get number of sectors displayed
            int sectorwidth = this.Width / currentGame.sectorsize - 1;
            int sectorheight = (this.Height - 42) / currentGame.sectorsize - 1;

            currentGame.galaxy[currentGame.currentGalaxy].startsector = sectornr - (int)(0.5 * sectorwidth) - (int)(0.5 * sectorheight) * currentGame.galaxy[currentGame.currentGalaxy].galaxy_xsize;
            if (currentGame.galaxy[currentGame.currentGalaxy].startsector < currentGame.galaxy[currentGame.currentGalaxy].lowestsectorid)
                currentGame.galaxy[currentGame.currentGalaxy].startsector += currentGame.galaxy[currentGame.currentGalaxy].galaxy_xsize * currentGame.galaxy[currentGame.currentGalaxy].galaxy_ysize;
            hostApplication.toolBarButton2.Text = currentGame.galaxy[currentGame.currentGalaxy].galaxy_name;
            hostApplication.Redraw();

            this.Hide();
            hostApplication.toolBarButton5.Pushed = false;
        }

        private void PlotWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
