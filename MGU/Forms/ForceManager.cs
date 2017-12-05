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
using System.Collections;
using System.Windows.Forms;
using System.IO;

namespace MGU
{
    public class ForceManager : System.Windows.Forms.Form
    {
        private MainStuff hostApplication;
        private Button RemoveAllianceMemberButton;
        private Button AddAllianceMemberButton;
        private ListBox AllianceMemberListBox;
        private GroupBox AllianceMembersGroupBox;
        private GroupBox SectorsToSeedGroupBox;
        private ListBox SeedSectorListBox;
        private Button AddSeedSectorButton;
        private Button RemoveSeedSectorButton;
        private TextBox NewSeedSectorTextBox;
        private Button CreateForceReportButton;
        private Button CreateSeedReportButton;
        private Button GenerateSeedRouteButton;
        private Button GenerateRefreshRouteButton;
        private RichTextBox ReportTextBox;
        private Button DeselectAllMembersButton;
        private Button SelectAllMembersButton;
        private Button DeselectAllSectorsButton;
        private Button SelectAllSectorsButton;
        private Game currentGame;

        public ForceManager(MainStuff newHost)
        {
            hostApplication = newHost;

            InitializeComponent();

            for (int i = 0; i < hostApplication.games[hostApplication.currentGame].allianceMembers.Count; i++)
            {
                AllianceMemberListBox.Items.Add(hostApplication.games[hostApplication.currentGame].allianceMembers[i].ToString());
            }

            for (int i = 0; i < hostApplication.games[hostApplication.currentGame].seedSectors.Count; i++)
            {
                SeedSectorListBox.Items.Add(hostApplication.games[hostApplication.currentGame].seedSectors[i].ToString());
            }
        }

        private void InitializeComponent()
        {
            this.AddAllianceMemberButton = new System.Windows.Forms.Button();
            this.NewAllianceMemberTextBox = new System.Windows.Forms.TextBox();
            this.RemoveAllianceMemberButton = new System.Windows.Forms.Button();
            this.AllianceMemberListBox = new System.Windows.Forms.ListBox();
            this.AllianceMembersGroupBox = new System.Windows.Forms.GroupBox();
            this.DeselectAllMembersButton = new System.Windows.Forms.Button();
            this.SelectAllMembersButton = new System.Windows.Forms.Button();
            this.SectorsToSeedGroupBox = new System.Windows.Forms.GroupBox();
            this.DeselectAllSectorsButton = new System.Windows.Forms.Button();
            this.SelectAllSectorsButton = new System.Windows.Forms.Button();
            this.SeedSectorListBox = new System.Windows.Forms.ListBox();
            this.AddSeedSectorButton = new System.Windows.Forms.Button();
            this.RemoveSeedSectorButton = new System.Windows.Forms.Button();
            this.NewSeedSectorTextBox = new System.Windows.Forms.TextBox();
            this.CreateForceReportButton = new System.Windows.Forms.Button();
            this.CreateSeedReportButton = new System.Windows.Forms.Button();
            this.GenerateSeedRouteButton = new System.Windows.Forms.Button();
            this.GenerateRefreshRouteButton = new System.Windows.Forms.Button();
            this.ReportTextBox = new System.Windows.Forms.RichTextBox();
            this.AllianceMembersGroupBox.SuspendLayout();
            this.SectorsToSeedGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddAllianceMemberButton
            // 
            this.AddAllianceMemberButton.Location = new System.Drawing.Point(192, 19);
            this.AddAllianceMemberButton.Name = "AddAllianceMemberButton";
            this.AddAllianceMemberButton.Size = new System.Drawing.Size(153, 23);
            this.AddAllianceMemberButton.TabIndex = 1;
            this.AddAllianceMemberButton.Text = "Add Alliance Member";
            this.AddAllianceMemberButton.UseVisualStyleBackColor = true;
            this.AddAllianceMemberButton.Click += new System.EventHandler(this.AddAllianceMemberButton_Click);
            // 
            // NewAllianceMemberTextBox
            // 
            this.NewAllianceMemberTextBox.Location = new System.Drawing.Point(192, 50);
            this.NewAllianceMemberTextBox.Name = "NewAllianceMemberTextBox";
            this.NewAllianceMemberTextBox.Size = new System.Drawing.Size(153, 20);
            this.NewAllianceMemberTextBox.TabIndex = 2;
            this.NewAllianceMemberTextBox.TextChanged += new System.EventHandler(this.NewAllianceMemberTextBox_TextChanged);
            this.NewAllianceMemberTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckKeys);
            // 
            // RemoveAllianceMemberButton
            // 
            this.RemoveAllianceMemberButton.Location = new System.Drawing.Point(192, 77);
            this.RemoveAllianceMemberButton.Name = "RemoveAllianceMemberButton";
            this.RemoveAllianceMemberButton.Size = new System.Drawing.Size(153, 37);
            this.RemoveAllianceMemberButton.TabIndex = 3;
            this.RemoveAllianceMemberButton.Text = "Remove Selected Alliance Member(s)";
            this.RemoveAllianceMemberButton.UseVisualStyleBackColor = true;
            this.RemoveAllianceMemberButton.Click += new System.EventHandler(this.RemoveAllianceMemberButton_Click);
            // 
            // AllianceMemberListBox
            // 
            this.AllianceMemberListBox.FormattingEnabled = true;
            this.AllianceMemberListBox.Location = new System.Drawing.Point(8, 19);
            this.AllianceMemberListBox.Name = "AllianceMemberListBox";
            this.AllianceMemberListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.AllianceMemberListBox.Size = new System.Drawing.Size(178, 95);
            this.AllianceMemberListBox.TabIndex = 4;
            // 
            // AllianceMembersGroupBox
            // 
            this.AllianceMembersGroupBox.Controls.Add(this.DeselectAllMembersButton);
            this.AllianceMembersGroupBox.Controls.Add(this.SelectAllMembersButton);
            this.AllianceMembersGroupBox.Controls.Add(this.AllianceMemberListBox);
            this.AllianceMembersGroupBox.Controls.Add(this.AddAllianceMemberButton);
            this.AllianceMembersGroupBox.Controls.Add(this.RemoveAllianceMemberButton);
            this.AllianceMembersGroupBox.Controls.Add(this.NewAllianceMemberTextBox);
            this.AllianceMembersGroupBox.Location = new System.Drawing.Point(4, 4);
            this.AllianceMembersGroupBox.Name = "AllianceMembersGroupBox";
            this.AllianceMembersGroupBox.Size = new System.Drawing.Size(350, 154);
            this.AllianceMembersGroupBox.TabIndex = 5;
            this.AllianceMembersGroupBox.TabStop = false;
            this.AllianceMembersGroupBox.Text = "Alliance members";
            // 
            // DeselectAllMembersButton
            // 
            this.DeselectAllMembersButton.Location = new System.Drawing.Point(193, 121);
            this.DeselectAllMembersButton.Name = "DeselectAllMembersButton";
            this.DeselectAllMembersButton.Size = new System.Drawing.Size(151, 23);
            this.DeselectAllMembersButton.TabIndex = 6;
            this.DeselectAllMembersButton.Text = "Deselect All";
            this.DeselectAllMembersButton.UseVisualStyleBackColor = true;
            this.DeselectAllMembersButton.Click += new System.EventHandler(this.DeselectAllMembersButton_Click);
            // 
            // SelectAllMembersButton
            // 
            this.SelectAllMembersButton.Location = new System.Drawing.Point(8, 121);
            this.SelectAllMembersButton.Name = "SelectAllMembersButton";
            this.SelectAllMembersButton.Size = new System.Drawing.Size(178, 23);
            this.SelectAllMembersButton.TabIndex = 5;
            this.SelectAllMembersButton.Text = "Select All";
            this.SelectAllMembersButton.UseVisualStyleBackColor = true;
            this.SelectAllMembersButton.Click += new System.EventHandler(this.SelectAllMembersButton_Click);
            // 
            // SectorsToSeedGroupBox
            // 
            this.SectorsToSeedGroupBox.Controls.Add(this.DeselectAllSectorsButton);
            this.SectorsToSeedGroupBox.Controls.Add(this.SelectAllSectorsButton);
            this.SectorsToSeedGroupBox.Controls.Add(this.SeedSectorListBox);
            this.SectorsToSeedGroupBox.Controls.Add(this.AddSeedSectorButton);
            this.SectorsToSeedGroupBox.Controls.Add(this.RemoveSeedSectorButton);
            this.SectorsToSeedGroupBox.Controls.Add(this.NewSeedSectorTextBox);
            this.SectorsToSeedGroupBox.Location = new System.Drawing.Point(360, 4);
            this.SectorsToSeedGroupBox.Name = "SectorsToSeedGroupBox";
            this.SectorsToSeedGroupBox.Size = new System.Drawing.Size(265, 154);
            this.SectorsToSeedGroupBox.TabIndex = 6;
            this.SectorsToSeedGroupBox.TabStop = false;
            this.SectorsToSeedGroupBox.Text = "Sectors to seed";
            // 
            // DeselectAllSectorsButton
            // 
            this.DeselectAllSectorsButton.Location = new System.Drawing.Point(107, 120);
            this.DeselectAllSectorsButton.Name = "DeselectAllSectorsButton";
            this.DeselectAllSectorsButton.Size = new System.Drawing.Size(150, 23);
            this.DeselectAllSectorsButton.TabIndex = 8;
            this.DeselectAllSectorsButton.Text = "Deselect All";
            this.DeselectAllSectorsButton.UseVisualStyleBackColor = true;
            this.DeselectAllSectorsButton.Click += new System.EventHandler(this.DeselectAllSectorsButton_Click);
            // 
            // SelectAllSectorsButton
            // 
            this.SelectAllSectorsButton.Location = new System.Drawing.Point(8, 120);
            this.SelectAllSectorsButton.Name = "SelectAllSectorsButton";
            this.SelectAllSectorsButton.Size = new System.Drawing.Size(91, 23);
            this.SelectAllSectorsButton.TabIndex = 7;
            this.SelectAllSectorsButton.Text = "Select All";
            this.SelectAllSectorsButton.UseVisualStyleBackColor = true;
            this.SelectAllSectorsButton.Click += new System.EventHandler(this.SelectAllSectorsButton_Click);
            // 
            // SeedSectorListBox
            // 
            this.SeedSectorListBox.FormattingEnabled = true;
            this.SeedSectorListBox.Location = new System.Drawing.Point(8, 19);
            this.SeedSectorListBox.Name = "SeedSectorListBox";
            this.SeedSectorListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.SeedSectorListBox.Size = new System.Drawing.Size(91, 95);
            this.SeedSectorListBox.TabIndex = 4;
            // 
            // AddSeedSectorButton
            // 
            this.AddSeedSectorButton.Location = new System.Drawing.Point(107, 19);
            this.AddSeedSectorButton.Name = "AddSeedSectorButton";
            this.AddSeedSectorButton.Size = new System.Drawing.Size(153, 23);
            this.AddSeedSectorButton.TabIndex = 1;
            this.AddSeedSectorButton.Text = "Add Seed Sector";
            this.AddSeedSectorButton.UseVisualStyleBackColor = true;
            this.AddSeedSectorButton.Click += new System.EventHandler(this.AddSeedSectorButton_Click);
            // 
            // RemoveSeedSectorButton
            // 
            this.RemoveSeedSectorButton.Location = new System.Drawing.Point(107, 77);
            this.RemoveSeedSectorButton.Name = "RemoveSeedSectorButton";
            this.RemoveSeedSectorButton.Size = new System.Drawing.Size(153, 37);
            this.RemoveSeedSectorButton.TabIndex = 3;
            this.RemoveSeedSectorButton.Text = "Remove Selected Seed Sector(s)";
            this.RemoveSeedSectorButton.UseVisualStyleBackColor = true;
            this.RemoveSeedSectorButton.Click += new System.EventHandler(this.RemoveSeedSectorButton_Click);
            // 
            // NewSeedSectorTextBox
            // 
            this.NewSeedSectorTextBox.Location = new System.Drawing.Point(107, 50);
            this.NewSeedSectorTextBox.Name = "NewSeedSectorTextBox";
            this.NewSeedSectorTextBox.Size = new System.Drawing.Size(153, 20);
            this.NewSeedSectorTextBox.TabIndex = 2;
            this.NewSeedSectorTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckKeys);
            // 
            // CreateForceReportButton
            // 
            this.CreateForceReportButton.Location = new System.Drawing.Point(4, 163);
            this.CreateForceReportButton.Name = "CreateForceReportButton";
            this.CreateForceReportButton.Size = new System.Drawing.Size(133, 23);
            this.CreateForceReportButton.TabIndex = 7;
            this.CreateForceReportButton.Text = "Create Force Report";
            this.CreateForceReportButton.UseVisualStyleBackColor = true;
            this.CreateForceReportButton.Click += new System.EventHandler(this.CreateForceReportButton_Click);
            // 
            // CreateSeedReportButton
            // 
            this.CreateSeedReportButton.Location = new System.Drawing.Point(143, 163);
            this.CreateSeedReportButton.Name = "CreateSeedReportButton";
            this.CreateSeedReportButton.Size = new System.Drawing.Size(115, 23);
            this.CreateSeedReportButton.TabIndex = 8;
            this.CreateSeedReportButton.Text = "Create Seed Report";
            this.CreateSeedReportButton.UseVisualStyleBackColor = true;
            this.CreateSeedReportButton.Click += new System.EventHandler(this.CreateSeedReportButton_Click);
            // 
            // GenerateSeedRouteButton
            // 
            this.GenerateSeedRouteButton.Location = new System.Drawing.Point(264, 163);
            this.GenerateSeedRouteButton.Name = "GenerateSeedRouteButton";
            this.GenerateSeedRouteButton.Size = new System.Drawing.Size(160, 23);
            this.GenerateSeedRouteButton.TabIndex = 9;
            this.GenerateSeedRouteButton.Text = "Generate Optimal Seed Route";
            this.GenerateSeedRouteButton.UseVisualStyleBackColor = true;
            this.GenerateSeedRouteButton.Click += new System.EventHandler(this.GenerateSeedRouteButton_Click);
            // 
            // GenerateRefreshRouteButton
            // 
            this.GenerateRefreshRouteButton.Location = new System.Drawing.Point(431, 163);
            this.GenerateRefreshRouteButton.Name = "GenerateRefreshRouteButton";
            this.GenerateRefreshRouteButton.Size = new System.Drawing.Size(194, 23);
            this.GenerateRefreshRouteButton.TabIndex = 10;
            this.GenerateRefreshRouteButton.Text = "Generate Optimal Refresh Route";
            this.GenerateRefreshRouteButton.UseVisualStyleBackColor = true;
            this.GenerateRefreshRouteButton.Click += new System.EventHandler(this.GenerateRefreshRouteButton_Click);
            // 
            // ReportTextBox
            // 
            this.ReportTextBox.Location = new System.Drawing.Point(4, 192);
            this.ReportTextBox.Name = "ReportTextBox";
            this.ReportTextBox.ReadOnly = true;
            this.ReportTextBox.Size = new System.Drawing.Size(621, 253);
            this.ReportTextBox.TabIndex = 11;
            this.ReportTextBox.Text = "";
            // 
            // ForceManager
            // 
            this.ClientSize = new System.Drawing.Size(631, 452);
            this.Controls.Add(this.ReportTextBox);
            this.Controls.Add(this.GenerateRefreshRouteButton);
            this.Controls.Add(this.GenerateSeedRouteButton);
            this.Controls.Add(this.CreateSeedReportButton);
            this.Controls.Add(this.CreateForceReportButton);
            this.Controls.Add(this.SectorsToSeedGroupBox);
            this.Controls.Add(this.AllianceMembersGroupBox);
            this.Name = "ForceManager";
            this.Load += new System.EventHandler(this.ForceManager_Load);
            this.AllianceMembersGroupBox.ResumeLayout(false);
            this.AllianceMembersGroupBox.PerformLayout();
            this.SectorsToSeedGroupBox.ResumeLayout(false);
            this.SectorsToSeedGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if(sender == this.NewAllianceMemberTextBox)
                    AddAllianceMemberButton_Click(sender, e);
                else if(sender == this.NewSeedSectorTextBox)
                    AddSeedSectorButton_Click(sender, e);
            } 
        }

        private TextBox NewAllianceMemberTextBox;

        private void AddAllianceMemberButton_Click(object sender, EventArgs e)
        {
            if(NewAllianceMemberTextBox.Text != "")
            {
                AllianceMemberListBox.Items.Add(NewAllianceMemberTextBox.Text);
                hostApplication.games[hostApplication.currentGame].allianceMembers.Add(NewAllianceMemberTextBox.Text);
            }
        }

        private void RemoveAllianceMemberButton_Click(object sender, EventArgs e)
        {
            for (int index = AllianceMemberListBox.SelectedIndices.Count-1; index >= 0; index--)
            {
                AllianceMemberListBox.Items.RemoveAt(AllianceMemberListBox.SelectedIndices[index]);
                hostApplication.games[hostApplication.currentGame].allianceMembers.Remove(AllianceMemberListBox.Text);
            }
        }

        private void NewAllianceMemberTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddSeedSectorButton_Click(object sender, EventArgs e)
        {
            int sectornr = 0;

            if (NewSeedSectorTextBox.Text != "")
                if (intparse(NewSeedSectorTextBox.Text, ref sectornr))
                {
                    hostApplication.games[hostApplication.currentGame].seedSectors.Add(sectornr);
                    SeedSectorListBox.Items.Add(NewSeedSectorTextBox.Text);
                }
                else
                    MessageBox.Show("Sector number could not be parsed");
        }

        private void RemoveSeedSectorButton_Click(object sender, EventArgs e)
        {
            for (int index = SeedSectorListBox.SelectedIndices.Count - 1; index >= 0; index--)
            {
                SeedSectorListBox.Items.RemoveAt(SeedSectorListBox.SelectedIndices[index]);
            }
        }

        public static bool intparse(string toparse, ref int result)
        {
            try { result = int.Parse(toparse); return true; }
            catch (System.FormatException) { return false; }
        }

        private void CreateForceReportButton_Click(object sender, EventArgs e)
        {
            int             currentgalaxyid, currentsectorx, currentsectory;
            Sector          cs; 
            ArrayList[]     forcesperplayer = new ArrayList[AllianceMemberListBox.Items.Count];

            if (AllianceMemberListBox.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Please select alliance members for whom to generate a force report");
                return;
            }
            else
            {
                ReportTextBox.Text = "";
            }
            for(int i = 0; i < AllianceMemberListBox.Items.Count; i++)
                forcesperplayer[i] = new ArrayList();
            
            //Create a container that contains, for each player, their complete forcedata
            for (int i = 1; i < hostApplication.games[hostApplication.currentGame].nrofsectors; i++)
            {
                //Determine galaxy and index of current sector
                currentgalaxyid = hostApplication.games[hostApplication.currentGame].GetGalaxyIndex(i);
                currentsectorx = (i - hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].lowestsectorid) % hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].galaxy_xsize;
                currentsectory = (i - hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].lowestsectorid) / hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].galaxy_xsize;
                cs = hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].sector[currentsectorx, currentsectory];

                //For all players in alliance member list, see if they have force data in this sector
                for (int owner = 0; owner < AllianceMemberListBox.Items.Count; owner++)
                {
                    for(int forcesInSector = 0; forcesInSector < cs.force_stacks.Count; forcesInSector++)
                        if (((ForceData)cs.force_stacks[forcesInSector]).owner.ToString() == AllianceMemberListBox.Items[owner].ToString())
                            forcesperplayer[owner].Add(cs.force_stacks[forcesInSector]);
                }
            }

            //Only for the selected alliance members, show the report
            foreach (int index in AllianceMemberListBox.SelectedIndices)
            {
                if (forcesperplayer[index].Count == 0)
                    ReportTextBox.Text += AllianceMemberListBox.Items[index] + " has no forces in the entire galaxy.\n\n";
                else
                {
                    ReportTextBox.Text += AllianceMemberListBox.Items[index] + " has forces in the following sectors:\n\n";

                    foreach (ForceData fd in forcesperplayer[index])
                    {
                        ReportTextBox.Text += fd.mines.ToString() + " mines, " + fd.combat_drones.ToString() + " combat_drones, and " + fd.scout_drones.ToString() + " scout drones in sector " + fd.sectorid.ToString() + "\n\n";
                    }
                }
            }
        }

        private void SelectAllMembersButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < AllianceMemberListBox.Items.Count; i++)
            {
                AllianceMemberListBox.SetSelected(i, true);
            }
        }

        private void DeselectAllMembersButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < AllianceMemberListBox.Items.Count; i++)
            {
                AllianceMemberListBox.SetSelected(i, false);
            }
        }

        private void SelectAllSectorsButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SeedSectorListBox .Items.Count; i++)
            {
                SeedSectorListBox.SetSelected(i, true);
            }
        }

        private void DeselectAllSectorsButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SeedSectorListBox.Items.Count; i++)
            {
                SeedSectorListBox.SetSelected(i, false);
            }
        }

        private void ForceManager_Load(object sender, EventArgs e)
        {

        }

        private void CreateSeedReportButton_Click(object sender, EventArgs e)
        {
            int currentgalaxyid, currentsectorx, currentsectory;
            Sector cs;
            ArrayList[] forcesperplayer = new ArrayList[AllianceMemberListBox.Items.Count];
            int sectornr = 0;

            if (AllianceMemberListBox.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Please select alliance members for whom to generate a force report");
                return;
            }
            else
            {
                ReportTextBox.Text = "";
            }

            for (int i = 0; i < AllianceMemberListBox.Items.Count; i++)
                forcesperplayer[i] = new ArrayList();

            //Create a container that contains, for each player, their complete forcedata
            for (int i = 1; i < hostApplication.games[hostApplication.currentGame].nrofsectors; i++)
            {
                //Determine galaxy and index of current sector
                currentgalaxyid = hostApplication.games[hostApplication.currentGame].GetGalaxyIndex(i);
                currentsectorx = (i - hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].lowestsectorid) % hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].galaxy_xsize;
                currentsectory = (i - hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].lowestsectorid) / hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].galaxy_xsize;
                cs = hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].sector[currentsectorx, currentsectory];

                //For all players in alliance member list, see if they have force data in this sector
                for (int owner = 0; owner < AllianceMemberListBox.Items.Count; owner++)
                {
                    for (int forcesInSector = 0; forcesInSector < cs.force_stacks.Count; forcesInSector++)
                        if (((ForceData)cs.force_stacks[forcesInSector]).owner.ToString() == AllianceMemberListBox.Items[owner].ToString())
                            forcesperplayer[owner].Add(cs.force_stacks[forcesInSector]);
                }
            }

            //Only for the selected alliance members, show the report
            foreach (int index in AllianceMemberListBox.SelectedIndices)
            {
                ReportTextBox.Text += AllianceMemberListBox.Items[index] + " still needs to seed the following sectors:\n\n";

                for (int index2 = 0; index2 < SeedSectorListBox.Items.Count; index2++ )
                {
                    if (!intparse(SeedSectorListBox.Items[index2].ToString(), ref sectornr) || sectornr > hostApplication.games[hostApplication.currentGame].nrofsectors)
                    {
                        MessageBox.Show("Invalid Seed Sector");
                        ReportTextBox.Text = "";
                        return;
                    }

                    currentgalaxyid = hostApplication.games[hostApplication.currentGame].GetGalaxyIndex(sectornr);
                    currentsectorx = (sectornr - hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].lowestsectorid) % hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].galaxy_xsize;
                    currentsectory = (sectornr - hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].lowestsectorid) / hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].galaxy_xsize;
                    cs = hostApplication.games[hostApplication.currentGame].galaxy[currentgalaxyid].sector[currentsectorx, currentsectory];

                    bool ownerfound = false;
                    foreach (ForceData fd in cs.force_stacks)
                    {
                        if (fd.owner == AllianceMemberListBox.Items[index])
                            ownerfound = true;
                    }

                    if (!ownerfound)
                        ReportTextBox.Text += cs.sector_id.ToString();
                    
                    if(index2 != SeedSectorListBox.Items.Count - 1)
                        ReportTextBox.Text += ",";
                }
                
                ReportTextBox.Text += "\n\n";
            }
        }

        private void GenerateSeedRouteButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function has not been implemented yet");
        }

        private void GenerateRefreshRouteButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function has not been implemented yet");
        }
    }
}