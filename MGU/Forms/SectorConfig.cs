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

namespace MGU
{
	public class SectorConfig : System.Windows.Forms.Form
	{
        private MainStuff hostApplication;
        private Game currentGame;
        private Sector cs;

		private System.Windows.Forms.ImageList Goods;
		private System.Windows.Forms.ImageList Buyies;
		private System.Windows.Forms.ImageList Sellies;
		private System.Windows.Forms.Button westButton;
		private System.Windows.Forms.Button northButton;
		private System.Windows.Forms.Button eastButton;
		private System.Windows.Forms.Button southButton;
		private System.Windows.Forms.TextBox CurrentSectorText;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox warpSectorText;
		private System.Windows.Forms.TextBox warpGalaxyText;
		private System.Windows.Forms.GroupBox PortGroupBox;
		private System.Windows.Forms.Label portLevelLabel;
		private System.Windows.Forms.TextBox portLevelText;
		private System.Windows.Forms.Label buyLabel;
		private System.Windows.Forms.Label sellLabel;
		private System.Windows.Forms.Label portRaceLabel;
		private System.Windows.Forms.ComboBox portRaceSelector;
		private System.Windows.Forms.CheckBox warpCheckBox;
        private System.Windows.Forms.TextBox planetLevelText;
		private System.Windows.Forms.Label planetLevelLabel;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label4;
		private ToolbarControl.PictureBar sellPictureBar;
		private ToolbarControl.PictureBar buyPictureBar;
		private System.Windows.Forms.GroupBox groupBox4;
		private Verlinea.ComboBoxTree.ComboBoxTree locSelecter1;
		private Verlinea.ComboBoxTree.ComboBoxTree locSelecter2;
		private Verlinea.ComboBoxTree.ComboBoxTree locSelecter3;
		private Verlinea.ComboBoxTree.ComboBoxTree locSelecter4;
        private Verlinea.ComboBoxTree.ComboBoxTree locSelecter5;
		private int oldwarp;
		private int[] oldlocs;
		private bool hadport;
		public bool locationsame, sectorsame, portsame;
		private System.Windows.Forms.Label sectorStatusLabel;
        private ImageList imageList2;
        private CheckBox planetCheckBox;
        private Button closeButton;
        private Button RemovePortButton;
        private Button CreatePortButton;
        private GroupBox ForcesGroupBox;
        private Label label6;
        private Label label5;
        private TextBox EnemyCdsField;
        private TextBox EnemyScoutsField;
        private Label label9;
        private Label label8;
        private Label label7;
        private TextBox FriendlyMinesField;
        private TextBox FriendlyCdsField;
        private TextBox FriendlyScoutsField;
        private TextBox EnemyMinesField;
        private Label label10;
        private ListBox OwnerList;
		private System.Windows.Forms.ComboBox sectorStatusSelector;

		private TreeNode[] Fillsubnodes(int id, ref TreeNode Selected)
		{
			TreeNode[] Subnode1, Subnode2, Subnode3, Subnode4, Subnode5, Subnode6, Subnode7, Subnode8;
			ArrayList aSubnode1 = new ArrayList(), aSubnode2 = new ArrayList(),
				aSubnode3 = new ArrayList(), aSubnode4 = new ArrayList(), aSubnode5 = new ArrayList(),
				aSubnode6 = new ArrayList(), aSubnode7 = new ArrayList(), aSubnode8 = new ArrayList();
			bool other = false, foundnode = false;

            TreeNode nothing = new TreeNode("Nothing");
            nothing.Tag = 0;
            aSubnode1.Add(nothing);
            if (id == 0)
            {
                Selected = nothing;
                foundnode = true;
            }

			for (int x = 0; x < currentGame.nroflocations; x += 1)
			{
                TreeNode CurNode = new TreeNode(currentGame.location[x].location_name);
                CurNode.Tag = x;
                
				if (currentGame.location[id].location_name == CurNode.Text && !foundnode)
				{
					Selected = CurNode;
					foundnode = true;
				}
                
				if (currentGame.location[x].location_type == "Fed")
				{
					aSubnode2.Add(CurNode);
				}
                else if (currentGame.location[x].location_type == "HQ")
				{
					aSubnode2.Add(CurNode);
				}
                else if (currentGame.location[x].location_type == "UG")
				{
					aSubnode2.Add(CurNode);
				}
				else if (currentGame.location[x].location_type == "Bank")
				{
					aSubnode3.Add(CurNode);
				}
                else if (currentGame.location[x].location_type == "Bar")
				{
					aSubnode4.Add(CurNode);
				}
                else if (currentGame.location[x].location_type == "Technology Shop")
				{
					 aSubnode5.Add(CurNode);
				}
                else if (currentGame.location[x].location_type == "Weapon Shop")
				{
					aSubnode6.Add(CurNode);
				}
                else if (currentGame.location[x].location_type == "Ship Shop")
                {
                    aSubnode7.Add(CurNode);
                }
				else
				{
					other = true;
					aSubnode8.Add(CurNode);
				}
			}
			Subnode1 = (TreeNode[])aSubnode1.ToArray(typeof(TreeNode));
			Subnode2 = (TreeNode[])aSubnode2.ToArray(typeof(TreeNode));
			Subnode3 = (TreeNode[])aSubnode3.ToArray(typeof(TreeNode));
			Subnode4 = (TreeNode[])aSubnode4.ToArray(typeof(TreeNode));
			Subnode5 = (TreeNode[])aSubnode5.ToArray(typeof(TreeNode));
			Subnode6 = (TreeNode[])aSubnode6.ToArray(typeof(TreeNode));
			Subnode7 = (TreeNode[])aSubnode7.ToArray(typeof(TreeNode));
			Subnode8 = (TreeNode[])aSubnode8.ToArray(typeof(TreeNode));
			TreeNode S2, S3, S4, S5, S6, S7, S8;
			S2 = new TreeNode("Headquarters");
			foreach (TreeNode Node in Subnode2) S2.Nodes.Add(Node);
			S3 = new TreeNode("Banks");
			foreach (TreeNode Node in Subnode3) S3.Nodes.Add(Node);
			S4 = new TreeNode("Bars");
			foreach (TreeNode Node in Subnode4) S4.Nodes.Add(Node);
			S5 = new TreeNode("Technology");
			foreach (TreeNode Node in Subnode5) S5.Nodes.Add(Node);
			S6 = new TreeNode("Weapons");
			foreach (TreeNode Node in Subnode6) S6.Nodes.Add(Node);
			S7 = new TreeNode("Ships");
			foreach (TreeNode Node in Subnode7) S7.Nodes.Add(Node);
			S8 = new TreeNode("Other");
			foreach (TreeNode Node in Subnode8) S8.Nodes.Add(Node);
			ArrayList Returnage = new ArrayList();
			foreach (TreeNode X in Subnode1)
			Returnage.Add(X);
			Returnage.Add(S2);
			Returnage.Add(S3);
			Returnage.Add(S4);
			Returnage.Add(S5);
			Returnage.Add(S6);
			Returnage.Add(S7);
			if (other)
				Returnage.Add(S8);
			return (TreeNode[])(Returnage.ToArray(typeof(TreeNode)));
		}

		public SectorConfig(Game game, Sector sector)
		{
            cs = sector;
            currentGame = game;

			InitializeComponent();

            DrawSectorConfig();

			TreeNode Node = null;
            this.locSelecter1.Nodes.AddRange(Fillsubnodes(cs.location[0], ref Node));
            this.locSelecter1.SelectedNode = Node;
            this.locSelecter2.Nodes.AddRange(Fillsubnodes(cs.location[1], ref Node));
            this.locSelecter2.SelectedNode = Node;
            this.locSelecter3.Nodes.AddRange(Fillsubnodes(cs.location[2], ref Node));
            this.locSelecter3.SelectedNode = Node;
            this.locSelecter4.Nodes.AddRange(Fillsubnodes(cs.location[3], ref Node));
            this.locSelecter4.SelectedNode = Node;
            this.locSelecter5.Nodes.AddRange(Fillsubnodes(cs.location[4], ref Node));
			this.locSelecter5.SelectedNode = Node;
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
			base.Dispose( disposing );
		}

        private void DrawSectorConfig()
		{
            Game currentGame = cs.galaxy.game;

            #region sectornumbers
            //Color the buttons and add the appropriate sector numbers
            if (cs.west != null)
            {
                this.westButton.FlatStyle = FlatStyle.Flat;
                this.westButton.BackColor = System.Drawing.Color.PaleGreen;
                this.westButton.Text = "#" + cs.west.sector_id.ToString();
            }
            else
            {
                this.westButton.FlatStyle = FlatStyle.Standard;
                this.westButton.BackColor = System.Drawing.Color.LightCoral;
            }
            if (cs.north != null)
            {
                this.northButton.FlatStyle = FlatStyle.Flat;
                this.northButton.BackColor = System.Drawing.Color.PaleGreen;
                this.northButton.Text = "#" + cs.north.sector_id.ToString();
            }
            else
            {
                this.northButton.FlatStyle = FlatStyle.Standard;
                this.northButton.BackColor = System.Drawing.Color.LightCoral;
            }
            if (cs.east != null)
            {
                this.eastButton.FlatStyle = FlatStyle.Flat;
                this.eastButton.BackColor = System.Drawing.Color.PaleGreen;
                this.eastButton.Text = "#" + cs.east.sector_id.ToString();
            }
            else
            {
                this.eastButton.FlatStyle = FlatStyle.Standard;
                this.eastButton.BackColor = System.Drawing.Color.LightCoral;
            }
            if (cs.south != null)
            {
                this.southButton.FlatStyle = FlatStyle.Flat;
                this.southButton.BackColor = System.Drawing.Color.PaleGreen;
                this.southButton.Text = "#" + cs.south.sector_id.ToString();
            }
            else
            {
                this.southButton.FlatStyle = FlatStyle.Standard;
                this.southButton.BackColor = System.Drawing.Color.LightCoral;
            }
            
			this.CurrentSectorText.Text = cs.sector_id.ToString();
			this.warpCheckBox.Checked = (cs.warp != null ? true : false);

            if (this.warpCheckBox.Checked)
            {
                this.warpSectorText.Text = cs.warp.sector_id.ToString();
                this.warpGalaxyText.Text = this.currentGame.galaxy[this.currentGame.GetGalaxyIndex(cs.warp.sector_id)].galaxy_name;
            }
			this.label11.Text      = cs.galaxy.galaxy_name;
			this.label12.Text      = cs.galaxy.galaxy_xsize.ToString() + " by " + cs.galaxy.galaxy_ysize.ToString();
            this.label4.Text       = "from #" + cs.galaxy.lowestsectorid.ToString() + " to #" + (cs.galaxy.lowestsectorid+cs.galaxy.galaxy_xsize*cs.galaxy.galaxy_ysize).ToString();
            
#endregion
            #region forces
            EnemyScoutsField.Text = cs.enemy_scouts.ToString();
            EnemyCdsField.Text = cs.enemy_cds.ToString();
            EnemyMinesField.Text = cs.enemy_mines.ToString();
            FriendlyScoutsField.Text = cs.friendly_scouts.ToString();
            FriendlyCdsField.Text = cs.friendly_cds.ToString();
            FriendlyMinesField.Text = cs.friendly_mines.ToString();
            #endregion
            #region port_goods
            this.Invalidate();
           
            if(cs.port != null)
            {
                this.portLevelText.Text = cs.port.port_level.ToString();
                this.portRaceSelector.Items.Clear();
                for(int i = 0; i <= currentGame.nrofraces; i++)
                {
                    this.portRaceSelector.Items.Add(currentGame.race[i].race_name);
                }
                this.portRaceSelector.SelectedIndex = cs.port.port_race.GetRaceNameAsId(currentGame);

                ImageList BuyGoods = new ImageList();
                BuyGoods.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
                BuyGoods.ImageSize = new System.Drawing.Size(16, 16);
                BuyGoods.TransparentColor = System.Drawing.Color.White;

                ImageList SellGoods = new ImageList();
                SellGoods.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
                SellGoods.ImageSize = new System.Drawing.Size(16, 16);
                SellGoods.TransparentColor = System.Drawing.Color.White;

                buyPictureBar.ImageList = BuyGoods;
                sellPictureBar.ImageList = SellGoods;
                
                for (int i = 1; i <= currentGame.nrofgoods; i++)
                {
                    if (cs.port.port_goods[i] == 1)
                    {
                        sellPictureBar.ImageList.Images.Add(imageList2.Images[0]);
                        buyPictureBar.ImageList.Images.Add((System.Drawing.Image)currentGame.hostApplication.Goods.Images[i]);
                    }
                    else if (cs.port.port_goods[i] == -1)
                    {
                        buyPictureBar.ImageList.Images.Add(imageList2.Images[0]);
                        sellPictureBar.ImageList.Images.Add((System.Drawing.Image)currentGame.hostApplication.Goods.Images[i]);
                    }
                    else
                    {
                        buyPictureBar.ImageList.Images.Add(imageList2.Images[0]);
                        sellPictureBar.ImageList.Images.Add(imageList2.Images[0]);
                    }
                }
            }
#endregion
            #region planets
            if (cs.planet != null)
            {
                planetLevelText.Text = cs.planet.level.ToString();
                this.planetCheckBox.Checked = true;
                this.planetLevelText.ReadOnly = !this.planetCheckBox.Checked;
            }
            #endregion
            #region locations
            
#endregion
            #region forces
            OwnerList.Items.Add("Add all forces");
            OwnerList.Items.Add("--------------");
            for (int i = 0; i < cs.force_stacks.Count; i++)
            {
                OwnerList.Items.Add(((ForceData)(cs.force_stacks[i])).owner);
            }
            OwnerList.SetSelected(0, true);
            #endregion
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SectorConfig));
            ToolbarControl.PictureBar.ColorsTemp colorsTemp1 = new ToolbarControl.PictureBar.ColorsTemp();
            ToolbarControl.PictureBar.ColorsTemp colorsTemp2 = new ToolbarControl.PictureBar.ColorsTemp();
            this.westButton = new System.Windows.Forms.Button();
            this.northButton = new System.Windows.Forms.Button();
            this.eastButton = new System.Windows.Forms.Button();
            this.southButton = new System.Windows.Forms.Button();
            this.CurrentSectorText = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.planetCheckBox = new System.Windows.Forms.CheckBox();
            this.sectorStatusSelector = new System.Windows.Forms.ComboBox();
            this.sectorStatusLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.planetLevelText = new System.Windows.Forms.TextBox();
            this.warpCheckBox = new System.Windows.Forms.CheckBox();
            this.planetLevelLabel = new System.Windows.Forms.Label();
            this.warpSectorText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.warpGalaxyText = new System.Windows.Forms.TextBox();
            this.PortGroupBox = new System.Windows.Forms.GroupBox();
            this.RemovePortButton = new System.Windows.Forms.Button();
            this.CreatePortButton = new System.Windows.Forms.Button();
            this.portRaceSelector = new System.Windows.Forms.ComboBox();
            this.portRaceLabel = new System.Windows.Forms.Label();
            this.sellLabel = new System.Windows.Forms.Label();
            this.buyLabel = new System.Windows.Forms.Label();
            this.portLevelText = new System.Windows.Forms.TextBox();
            this.portLevelLabel = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.closeButton = new System.Windows.Forms.Button();
            this.ForcesGroupBox = new System.Windows.Forms.GroupBox();
            this.OwnerList = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.FriendlyMinesField = new System.Windows.Forms.TextBox();
            this.FriendlyCdsField = new System.Windows.Forms.TextBox();
            this.FriendlyScoutsField = new System.Windows.Forms.TextBox();
            this.EnemyMinesField = new System.Windows.Forms.TextBox();
            this.EnemyCdsField = new System.Windows.Forms.TextBox();
            this.EnemyScoutsField = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.locSelecter5 = new Verlinea.ComboBoxTree.ComboBoxTree();
            this.locSelecter4 = new Verlinea.ComboBoxTree.ComboBoxTree();
            this.locSelecter3 = new Verlinea.ComboBoxTree.ComboBoxTree();
            this.locSelecter2 = new Verlinea.ComboBoxTree.ComboBoxTree();
            this.locSelecter1 = new Verlinea.ComboBoxTree.ComboBoxTree();
            this.sellPictureBar = new ToolbarControl.PictureBar();
            this.buyPictureBar = new ToolbarControl.PictureBar();
            this.groupBox1.SuspendLayout();
            this.PortGroupBox.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.ForcesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // westButton
            // 
            this.westButton.Location = new System.Drawing.Point(14, 38);
            this.westButton.Name = "westButton";
            this.westButton.Size = new System.Drawing.Size(58, 20);
            this.westButton.TabIndex = 0;
            this.westButton.Click += new System.EventHandler(this.westButton_Click);
            // 
            // northButton
            // 
            this.northButton.Location = new System.Drawing.Point(72, 18);
            this.northButton.Name = "northButton";
            this.northButton.Size = new System.Drawing.Size(58, 20);
            this.northButton.TabIndex = 1;
            this.northButton.Click += new System.EventHandler(this.northButton_Click);
            // 
            // eastButton
            // 
            this.eastButton.Location = new System.Drawing.Point(128, 38);
            this.eastButton.Name = "eastButton";
            this.eastButton.Size = new System.Drawing.Size(58, 20);
            this.eastButton.TabIndex = 2;
            this.eastButton.Click += new System.EventHandler(this.eastButton_Click);
            // 
            // southButton
            // 
            this.southButton.Location = new System.Drawing.Point(72, 58);
            this.southButton.Name = "southButton";
            this.southButton.Size = new System.Drawing.Size(58, 20);
            this.southButton.TabIndex = 3;
            this.southButton.Click += new System.EventHandler(this.southButton_Click);
            // 
            // CurrentSectorText
            // 
            this.CurrentSectorText.BackColor = System.Drawing.Color.LightYellow;
            this.CurrentSectorText.Cursor = System.Windows.Forms.Cursors.Default;
            this.CurrentSectorText.Location = new System.Drawing.Point(72, 38);
            this.CurrentSectorText.Name = "CurrentSectorText";
            this.CurrentSectorText.ReadOnly = true;
            this.CurrentSectorText.Size = new System.Drawing.Size(58, 20);
            this.CurrentSectorText.TabIndex = 4;
            this.CurrentSectorText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.planetCheckBox);
            this.groupBox1.Controls.Add(this.sectorStatusSelector);
            this.groupBox1.Controls.Add(this.sectorStatusLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.planetLevelText);
            this.groupBox1.Controls.Add(this.warpCheckBox);
            this.groupBox1.Controls.Add(this.planetLevelLabel);
            this.groupBox1.Controls.Add(this.warpSectorText);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.northButton);
            this.groupBox1.Controls.Add(this.CurrentSectorText);
            this.groupBox1.Controls.Add(this.eastButton);
            this.groupBox1.Controls.Add(this.southButton);
            this.groupBox1.Controls.Add(this.westButton);
            this.groupBox1.Controls.Add(this.warpGalaxyText);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(624, 112);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sector Connectivity";
            // 
            // planetCheckBox
            // 
            this.planetCheckBox.Location = new System.Drawing.Point(426, 78);
            this.planetCheckBox.Name = "planetCheckBox";
            this.planetCheckBox.Size = new System.Drawing.Size(65, 24);
            this.planetCheckBox.TabIndex = 19;
            this.planetCheckBox.Text = "Planet";
            this.planetCheckBox.CheckedChanged += new System.EventHandler(this.planetCheckBox_CheckedChanged);
            // 
            // sectorStatusSelector
            // 
            this.sectorStatusSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sectorStatusSelector.Items.AddRange(new object[] {
            "Neutral",
            "Friendly",
            "Hostile"});
            this.sectorStatusSelector.Location = new System.Drawing.Point(90, 86);
            this.sectorStatusSelector.Name = "sectorStatusSelector";
            this.sectorStatusSelector.Size = new System.Drawing.Size(82, 21);
            this.sectorStatusSelector.TabIndex = 18;
            this.sectorStatusSelector.SelectedIndexChanged += new System.EventHandler(this.sectorStatusSelector_SelectedIndexChanged);
            // 
            // sectorStatusLabel
            // 
            this.sectorStatusLabel.Location = new System.Drawing.Point(11, 88);
            this.sectorStatusLabel.Name = "sectorStatusLabel";
            this.sectorStatusLabel.Size = new System.Drawing.Size(75, 18);
            this.sectorStatusLabel.TabIndex = 17;
            this.sectorStatusLabel.Text = "Sector status:";
            this.sectorStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(248, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 16);
            this.label4.TabIndex = 16;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(248, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(156, 16);
            this.label12.TabIndex = 14;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(248, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(156, 16);
            this.label11.TabIndex = 13;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // planetLevelText
            // 
            this.planetLevelText.Location = new System.Drawing.Point(536, 78);
            this.planetLevelText.Name = "planetLevelText";
            this.planetLevelText.ReadOnly = true;
            this.planetLevelText.Size = new System.Drawing.Size(56, 20);
            this.planetLevelText.TabIndex = 1;
            this.planetLevelText.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // warpCheckBox
            // 
            this.warpCheckBox.Location = new System.Drawing.Point(192, 80);
            this.warpCheckBox.Name = "warpCheckBox";
            this.warpCheckBox.Size = new System.Drawing.Size(56, 24);
            this.warpCheckBox.TabIndex = 12;
            this.warpCheckBox.Text = "Warp:";
            this.warpCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // planetLevelLabel
            // 
            this.planetLevelLabel.Location = new System.Drawing.Point(488, 78);
            this.planetLevelLabel.Name = "planetLevelLabel";
            this.planetLevelLabel.Size = new System.Drawing.Size(42, 20);
            this.planetLevelLabel.TabIndex = 4;
            this.planetLevelLabel.Text = "Level:";
            this.planetLevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // warpSectorText
            // 
            this.warpSectorText.Location = new System.Drawing.Point(248, 80);
            this.warpSectorText.MaxLength = 6;
            this.warpSectorText.Name = "warpSectorText";
            this.warpSectorText.Size = new System.Drawing.Size(40, 20);
            this.warpSectorText.TabIndex = 10;
            this.warpSectorText.TextChanged += new System.EventHandler(this.warpSectorText_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(192, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Spread:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(192, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Size:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(192, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "In Galaxy:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // warpGalaxyText
            // 
            this.warpGalaxyText.Location = new System.Drawing.Point(288, 80);
            this.warpGalaxyText.Name = "warpGalaxyText";
            this.warpGalaxyText.ReadOnly = true;
            this.warpGalaxyText.Size = new System.Drawing.Size(120, 20);
            this.warpGalaxyText.TabIndex = 11;
            // 
            // PortGroupBox
            // 
            this.PortGroupBox.Controls.Add(this.RemovePortButton);
            this.PortGroupBox.Controls.Add(this.CreatePortButton);
            this.PortGroupBox.Controls.Add(this.portRaceSelector);
            this.PortGroupBox.Controls.Add(this.portRaceLabel);
            this.PortGroupBox.Controls.Add(this.sellPictureBar);
            this.PortGroupBox.Controls.Add(this.buyPictureBar);
            this.PortGroupBox.Controls.Add(this.sellLabel);
            this.PortGroupBox.Controls.Add(this.buyLabel);
            this.PortGroupBox.Controls.Add(this.portLevelText);
            this.PortGroupBox.Controls.Add(this.portLevelLabel);
            this.PortGroupBox.Location = new System.Drawing.Point(8, 126);
            this.PortGroupBox.Name = "PortGroupBox";
            this.PortGroupBox.Size = new System.Drawing.Size(624, 120);
            this.PortGroupBox.TabIndex = 6;
            this.PortGroupBox.TabStop = false;
            this.PortGroupBox.Text = "Port";
            // 
            // RemovePortButton
            // 
            this.RemovePortButton.Location = new System.Drawing.Point(535, 16);
            this.RemovePortButton.Name = "RemovePortButton";
            this.RemovePortButton.Size = new System.Drawing.Size(83, 23);
            this.RemovePortButton.TabIndex = 9;
            this.RemovePortButton.Text = "Remove Port";
            this.RemovePortButton.UseVisualStyleBackColor = true;
            this.RemovePortButton.Click += new System.EventHandler(this.RemovePortButton_Click);
            // 
            // CreatePortButton
            // 
            this.CreatePortButton.Location = new System.Drawing.Point(455, 16);
            this.CreatePortButton.Name = "CreatePortButton";
            this.CreatePortButton.Size = new System.Drawing.Size(75, 23);
            this.CreatePortButton.TabIndex = 8;
            this.CreatePortButton.Text = "Create Port";
            this.CreatePortButton.UseVisualStyleBackColor = true;
            this.CreatePortButton.Click += new System.EventHandler(this.CreatePortButton_Click);
            // 
            // portRaceSelector
            // 
            this.portRaceSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portRaceSelector.Items.AddRange(new object[] {
            "Alskant",
            "Creonti",
            "Human",
            "Ik\'Thorne",
            "Salvene",
            "Thevian",
            "WQ Human",
            "Nijarin",
            "Neutral"});
            this.portRaceSelector.Location = new System.Drawing.Point(208, 16);
            this.portRaceSelector.Name = "portRaceSelector";
            this.portRaceSelector.Size = new System.Drawing.Size(196, 21);
            this.portRaceSelector.TabIndex = 7;
            this.portRaceSelector.SelectedIndexChanged += new System.EventHandler(this.portRaceSelector_SelectedIndexChanged);
            // 
            // portRaceLabel
            // 
            this.portRaceLabel.Location = new System.Drawing.Point(160, 16);
            this.portRaceLabel.Name = "portRaceLabel";
            this.portRaceLabel.Size = new System.Drawing.Size(40, 24);
            this.portRaceLabel.TabIndex = 6;
            this.portRaceLabel.Text = "Race:";
            this.portRaceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sellLabel
            // 
            this.sellLabel.Location = new System.Drawing.Point(8, 80);
            this.sellLabel.Name = "sellLabel";
            this.sellLabel.Size = new System.Drawing.Size(48, 32);
            this.sellLabel.TabIndex = 3;
            this.sellLabel.Text = "Selling:";
            this.sellLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buyLabel
            // 
            this.buyLabel.Location = new System.Drawing.Point(8, 40);
            this.buyLabel.Name = "buyLabel";
            this.buyLabel.Size = new System.Drawing.Size(48, 32);
            this.buyLabel.TabIndex = 2;
            this.buyLabel.Text = "Buying:";
            this.buyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // portLevelText
            // 
            this.portLevelText.Location = new System.Drawing.Point(56, 16);
            this.portLevelText.Name = "portLevelText";
            this.portLevelText.Size = new System.Drawing.Size(24, 20);
            this.portLevelText.TabIndex = 1;
            this.portLevelText.Text = "9";
            this.portLevelText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.portLevelText.TextChanged += new System.EventHandler(this.portLevelText_TextChanged);
            // 
            // portLevelLabel
            // 
            this.portLevelLabel.Location = new System.Drawing.Point(8, 16);
            this.portLevelLabel.Name = "portLevelLabel";
            this.portLevelLabel.Size = new System.Drawing.Size(48, 20);
            this.portLevelLabel.TabIndex = 0;
            this.portLevelLabel.Text = "Level:";
            this.portLevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.locSelecter5);
            this.groupBox4.Controls.Add(this.locSelecter4);
            this.groupBox4.Controls.Add(this.locSelecter3);
            this.groupBox4.Controls.Add(this.locSelecter2);
            this.groupBox4.Controls.Add(this.locSelecter1);
            this.groupBox4.Location = new System.Drawing.Point(8, 254);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(624, 142);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Locations (Double click to select 1)";
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "");
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(8, 531);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(624, 23);
            this.closeButton.TabIndex = 11;
            this.closeButton.Text = "Close Window";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // ForcesGroupBox
            // 
            this.ForcesGroupBox.Controls.Add(this.OwnerList);
            this.ForcesGroupBox.Controls.Add(this.label10);
            this.ForcesGroupBox.Controls.Add(this.label9);
            this.ForcesGroupBox.Controls.Add(this.label8);
            this.ForcesGroupBox.Controls.Add(this.label7);
            this.ForcesGroupBox.Controls.Add(this.FriendlyMinesField);
            this.ForcesGroupBox.Controls.Add(this.FriendlyCdsField);
            this.ForcesGroupBox.Controls.Add(this.FriendlyScoutsField);
            this.ForcesGroupBox.Controls.Add(this.EnemyMinesField);
            this.ForcesGroupBox.Controls.Add(this.EnemyCdsField);
            this.ForcesGroupBox.Controls.Add(this.EnemyScoutsField);
            this.ForcesGroupBox.Controls.Add(this.label6);
            this.ForcesGroupBox.Controls.Add(this.label5);
            this.ForcesGroupBox.Location = new System.Drawing.Point(8, 404);
            this.ForcesGroupBox.Name = "ForcesGroupBox";
            this.ForcesGroupBox.Size = new System.Drawing.Size(621, 121);
            this.ForcesGroupBox.TabIndex = 13;
            this.ForcesGroupBox.TabStop = false;
            this.ForcesGroupBox.Text = "Forces";
            // 
            // OwnerList
            // 
            this.OwnerList.FormattingEnabled = true;
            this.OwnerList.Location = new System.Drawing.Point(426, 38);
            this.OwnerList.Name = "OwnerList";
            this.OwnerList.Size = new System.Drawing.Size(183, 69);
            this.OwnerList.TabIndex = 12;
            this.OwnerList.SelectedIndexChanged += new System.EventHandler(this.OwnerList_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(476, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(133, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Force owners in this sector";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Mines:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Cds:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Scouts:";
            // 
            // FriendlyMinesField
            // 
            this.FriendlyMinesField.Location = new System.Drawing.Point(242, 91);
            this.FriendlyMinesField.Name = "FriendlyMinesField";
            this.FriendlyMinesField.Size = new System.Drawing.Size(160, 20);
            this.FriendlyMinesField.TabIndex = 7;
            this.FriendlyMinesField.TextChanged += new System.EventHandler(this.FriendlyMinesField_TextChanged);
            // 
            // FriendlyCdsField
            // 
            this.FriendlyCdsField.Location = new System.Drawing.Point(242, 64);
            this.FriendlyCdsField.Name = "FriendlyCdsField";
            this.FriendlyCdsField.Size = new System.Drawing.Size(160, 20);
            this.FriendlyCdsField.TabIndex = 6;
            this.FriendlyCdsField.TextChanged += new System.EventHandler(this.FriendlyCdsField_TextChanged);
            // 
            // FriendlyScoutsField
            // 
            this.FriendlyScoutsField.Location = new System.Drawing.Point(242, 38);
            this.FriendlyScoutsField.Name = "FriendlyScoutsField";
            this.FriendlyScoutsField.Size = new System.Drawing.Size(160, 20);
            this.FriendlyScoutsField.TabIndex = 5;
            this.FriendlyScoutsField.TextChanged += new System.EventHandler(this.FriendlyScoutsField_TextChanged);
            // 
            // EnemyMinesField
            // 
            this.EnemyMinesField.Location = new System.Drawing.Point(63, 91);
            this.EnemyMinesField.Name = "EnemyMinesField";
            this.EnemyMinesField.Size = new System.Drawing.Size(160, 20);
            this.EnemyMinesField.TabIndex = 4;
            this.EnemyMinesField.TextChanged += new System.EventHandler(this.EnemyMinesField_TextChanged);
            // 
            // EnemyCdsField
            // 
            this.EnemyCdsField.Location = new System.Drawing.Point(63, 64);
            this.EnemyCdsField.Name = "EnemyCdsField";
            this.EnemyCdsField.Size = new System.Drawing.Size(160, 20);
            this.EnemyCdsField.TabIndex = 3;
            this.EnemyCdsField.TextChanged += new System.EventHandler(this.EnemyCdsField_TextChanged);
            // 
            // EnemyScoutsField
            // 
            this.EnemyScoutsField.Location = new System.Drawing.Point(63, 38);
            this.EnemyScoutsField.Name = "EnemyScoutsField";
            this.EnemyScoutsField.Size = new System.Drawing.Size(160, 20);
            this.EnemyScoutsField.TabIndex = 2;
            this.EnemyScoutsField.TextChanged += new System.EventHandler(this.EnemyScoutsField_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(282, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Friendly Forces";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(107, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Enemy Forces";
            // 
            // locSelecter5
            // 
            this.locSelecter5.AbsoluteChildrenSelectableOnly = true;
            this.locSelecter5.BranchSeparator = "-";
            this.locSelecter5.Imagelist = null;
            this.locSelecter5.Location = new System.Drawing.Point(8, 115);
            this.locSelecter5.Name = "locSelecter5";
            this.locSelecter5.SelectedNode = null;
            this.locSelecter5.Size = new System.Drawing.Size(610, 21);
            this.locSelecter5.TabIndex = 9;
            // 
            // locSelecter4
            // 
            this.locSelecter4.AbsoluteChildrenSelectableOnly = true;
            this.locSelecter4.BranchSeparator = "-";
            this.locSelecter4.Imagelist = null;
            this.locSelecter4.Location = new System.Drawing.Point(8, 88);
            this.locSelecter4.Name = "locSelecter4";
            this.locSelecter4.SelectedNode = null;
            this.locSelecter4.Size = new System.Drawing.Size(610, 21);
            this.locSelecter4.TabIndex = 8;
            this.locSelecter4.NodeSelected += new Verlinea.ComboBoxTree.ComboBoxTree.MyEvent(this.comboBoxTree4_NodeSelected);
            // 
            // locSelecter3
            // 
            this.locSelecter3.AbsoluteChildrenSelectableOnly = true;
            this.locSelecter3.BranchSeparator = "-";
            this.locSelecter3.Imagelist = null;
            this.locSelecter3.Location = new System.Drawing.Point(8, 64);
            this.locSelecter3.Name = "locSelecter3";
            this.locSelecter3.SelectedNode = null;
            this.locSelecter3.Size = new System.Drawing.Size(610, 21);
            this.locSelecter3.TabIndex = 7;
            this.locSelecter3.NodeSelected += new Verlinea.ComboBoxTree.ComboBoxTree.MyEvent(this.comboBoxTree3_NodeSelected);
            // 
            // locSelecter2
            // 
            this.locSelecter2.AbsoluteChildrenSelectableOnly = true;
            this.locSelecter2.BranchSeparator = "-";
            this.locSelecter2.Imagelist = null;
            this.locSelecter2.Location = new System.Drawing.Point(8, 40);
            this.locSelecter2.Name = "locSelecter2";
            this.locSelecter2.SelectedNode = null;
            this.locSelecter2.Size = new System.Drawing.Size(610, 21);
            this.locSelecter2.TabIndex = 6;
            this.locSelecter2.NodeSelected += new Verlinea.ComboBoxTree.ComboBoxTree.MyEvent(this.comboBoxTree2_NodeSelected);
            // 
            // locSelecter1
            // 
            this.locSelecter1.AbsoluteChildrenSelectableOnly = true;
            this.locSelecter1.BranchSeparator = "-";
            this.locSelecter1.Imagelist = null;
            this.locSelecter1.Location = new System.Drawing.Point(8, 16);
            this.locSelecter1.Name = "locSelecter1";
            this.locSelecter1.SelectedNode = null;
            this.locSelecter1.Size = new System.Drawing.Size(610, 21);
            this.locSelecter1.TabIndex = 5;
            this.locSelecter1.NodeSelected += new Verlinea.ComboBoxTree.ComboBoxTree.MyEvent(this.comboBoxTree1_NodeSelected);
            // 
            // sellPictureBar
            // 
            this.sellPictureBar.BackgroundColorDown = System.Drawing.Color.MintCream;
            this.sellPictureBar.BackgroundColorNormal = System.Drawing.Color.WhiteSmoke;
            this.sellPictureBar.BackgroundColorOver = System.Drawing.Color.FloralWhite;
            this.sellPictureBar.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.sellPictureBar.LineColorDown = System.Drawing.Color.Black;
            this.sellPictureBar.LineColorNormal = System.Drawing.Color.DimGray;
            this.sellPictureBar.LineColorOver = System.Drawing.Color.Gray;
            colorsTemp1.Down = System.Drawing.Color.Black;
            colorsTemp1.Normal = System.Drawing.Color.Black;
            colorsTemp1.Over = System.Drawing.Color.Black;
            this.sellPictureBar.LineColors = colorsTemp1;
            this.sellPictureBar.Location = new System.Drawing.Point(56, 80);
            this.sellPictureBar.Name = "sellPictureBar";
            this.sellPictureBar.Size = new System.Drawing.Size(562, 36);
            this.sellPictureBar.TabIndex = 5;
            this.sellPictureBar.Text = "pictureBar4";
            this.sellPictureBar.Clicked += new ToolbarControl.PictureBar.EventHandler(this.pictureBar2_Clicked);
            // 
            // buyPictureBar
            // 
            this.buyPictureBar.BackgroundColorDown = System.Drawing.Color.MintCream;
            this.buyPictureBar.BackgroundColorNormal = System.Drawing.Color.WhiteSmoke;
            this.buyPictureBar.BackgroundColorOver = System.Drawing.Color.FloralWhite;
            this.buyPictureBar.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.buyPictureBar.LineColorDown = System.Drawing.Color.Black;
            this.buyPictureBar.LineColorNormal = System.Drawing.Color.DimGray;
            this.buyPictureBar.LineColorOver = System.Drawing.Color.Gray;
            colorsTemp2.Down = System.Drawing.Color.Black;
            colorsTemp2.Normal = System.Drawing.Color.Black;
            colorsTemp2.Over = System.Drawing.Color.Black;
            this.buyPictureBar.LineColors = colorsTemp2;
            this.buyPictureBar.Location = new System.Drawing.Point(56, 40);
            this.buyPictureBar.Name = "buyPictureBar";
            this.buyPictureBar.Size = new System.Drawing.Size(562, 36);
            this.buyPictureBar.TabIndex = 4;
            this.buyPictureBar.Clicked += new ToolbarControl.PictureBar.EventHandler(this.pictureBar1_Clicked);
            // 
            // SectorConfig
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(644, 560);
            this.Controls.Add(this.ForcesGroupBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.PortGroupBox);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SectorConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manual sector editor";
            this.Load += new System.EventHandler(this.SectorConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.PortGroupBox.ResumeLayout(false);
            this.PortGroupBox.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ForcesGroupBox.ResumeLayout(false);
            this.ForcesGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void pictureBar1_Clicked(int selectedIndex)
		{
            if(cs.port.port_goods[selectedIndex+1] != 1)
                cs.port.port_goods[selectedIndex+1] = 1;
            else
                cs.port.port_goods[selectedIndex+1] = 0;
            DrawSectorConfig();
		}

		private void pictureBar2_Clicked(int selectedIndex)
        {
            if(cs.port.port_goods[selectedIndex+1] != -1)
                cs.port.port_goods[selectedIndex+1] = -1;
            else
                cs.port.port_goods[selectedIndex+1] = 0;
            DrawSectorConfig();
        }

		private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
		{
            int newWarpSector = 0;

            //if no warp has been given, then do nothing. The text_changed event will handle creating a warp
            if (warpSectorText.Text == "")
                return;

            //If the warp is unchecked, then remove the existing war
            if (warpCheckBox.Checked == false)
            {
                cs.warp.warp = null;
                cs.warp = null;
                warpSectorText.Text = "";
                return;
            }

            //Else, a new warp is being created, and the target sector has been given
            //In this case, check if the target sector is a valid sector

            if (intparse(warpSectorText.Text, ref newWarpSector))
                if (currentGame.GetSectorObject(newWarpSector) == null)
                    MessageBox.Show("Warp sector does not exist.", "Error", MessageBoxButtons.OK);
                else
                {
                    if (currentGame.GetSectorObject(newWarpSector).warp != null)
                        MessageBox.Show("Target warp sector already has a warp. Please change this first.", "Error", MessageBoxButtons.OK);
                    else
                    {
                        cs.warp = currentGame.GetSectorObject(newWarpSector);
                        cs.warp.warp = cs;
                    }
                }
            else
                MessageBox.Show("Warp sector could not be read.", "Error", MessageBoxButtons.OK);
		}

		private void westButton_Click(object sender, System.EventArgs e)
		{
            int west;
            Sector newwest;

            if (cs.west != null)
            {
                cs.west.east = null;
                cs.west = null;

                westButton.FlatStyle = FlatStyle.Standard;
                westButton.BackColor = System.Drawing.Color.LightCoral;
                westButton.Text = "";
            }
            else
            {
                //First find correct sector
                west = cs.sector_id - 1;
                if (west % cs.galaxy.galaxy_xsize == 0)
                    west += cs.galaxy.galaxy_xsize;

                newwest = cs.galaxy.game.GetSectorObject(west);
                cs.west = newwest;
                newwest.east = cs;

                westButton.FlatStyle = FlatStyle.Flat;
                westButton.BackColor = System.Drawing.Color.PaleGreen;
                westButton.Text = "#" + cs.west.sector_id.ToString();
            }
		}

		private void northButton_Click(object sender, System.EventArgs e)
		{
            int north;
            Sector newnorth;

            if (cs.north != null)
            {
                cs.north.south = null;
                cs.north = null;

                northButton.FlatStyle = FlatStyle.Standard;
                northButton.BackColor = System.Drawing.Color.LightCoral;
                northButton.Text = "";
            }
            else
            {
                //First find correct sector
                north = cs.sector_id - cs.galaxy.galaxy_xsize;
                if (north < cs.galaxy.lowestsectorid)
                    north += cs.galaxy.galaxy_xsize * cs.galaxy.galaxy_ysize;

                newnorth = cs.galaxy.game.GetSectorObject(north);
                cs.north = newnorth;
                newnorth.south = cs;

                northButton.FlatStyle = FlatStyle.Flat;
                northButton.BackColor = System.Drawing.Color.PaleGreen;
                northButton.Text = "#" + cs.north.sector_id.ToString();
            }
		}

		private void southButton_Click(object sender, System.EventArgs e)
		{
            int south;
            Sector newsouth;

            if (cs.south != null)
            {
                cs.south.north = null;
                cs.south = null;

                southButton.FlatStyle = FlatStyle.Standard;
                southButton.BackColor = System.Drawing.Color.LightCoral;
                southButton.Text = "";
            }
            else
            {
                //First find correct sector
                south = cs.sector_id + cs.galaxy.galaxy_xsize;
                if (south >= (cs.galaxy.lowestsectorid + cs.galaxy.galaxy_xsize*cs.galaxy.galaxy_ysize))
                    south -= cs.galaxy.galaxy_xsize * cs.galaxy.galaxy_ysize;

                newsouth = cs.galaxy.game.GetSectorObject(south);
                cs.south = newsouth;
                newsouth.north = cs;

                southButton.FlatStyle = FlatStyle.Flat;
                southButton.BackColor = System.Drawing.Color.PaleGreen;
                southButton.Text = "#" + cs.south.sector_id.ToString();
            }
		}

		private void eastButton_Click(object sender, System.EventArgs e)
		{
            int east;
            Sector neweast;

            if (cs.east != null)
            {
                cs.east.west = null;
                cs.east = null;

                eastButton.FlatStyle = FlatStyle.Standard;
                eastButton.BackColor = System.Drawing.Color.LightCoral;
                eastButton.Text = "";
            }
            else
            {
                //First find correct sector
                east = cs.sector_id + 1;
                if (east % cs.galaxy.galaxy_xsize == 1)
                    east -= cs.galaxy.galaxy_xsize;

                neweast = cs.galaxy.game.GetSectorObject(east);
                cs.east = neweast;
                neweast.west = cs;

                eastButton.FlatStyle = FlatStyle.Flat;
                eastButton.BackColor = System.Drawing.Color.PaleGreen;
                eastButton.Text = "#" + cs.east.sector_id.ToString();
            }
		}

		private void textBox5_TextChanged(object sender, System.EventArgs e)
		{
            int newlevel = 0;

            if (intparse(planetLevelText.Text, ref newlevel))
                cs.planet.level = newlevel;
            else
                MessageBox.Show("Planet level could not be read");
		}

		private void comboBoxTree1_NodeSelected(int id)
		{
            cs.location[0] = id;
		}

		private void comboBoxTree2_NodeSelected(int id)
		{
            cs.location[1] = id;
		}

		private void comboBoxTree3_NodeSelected(int id)
		{
            cs.location[2] = id;
		}

		private void comboBoxTree4_NodeSelected(int id)
		{
            cs.location[3] = id;
		}

        private void comboBoxTree5_NodeSelected(int id)
        {
            cs.location[4] = id;
        }

        private void portLevelText_TextChanged(object sender, EventArgs e)
        {
            int newlevel = 0;

            if(intparse(portLevelText.Text, ref newlevel))
                if(newlevel > 0 && newlevel <=9)
                    cs.port.port_level = newlevel;

        }

        private static bool intparse(string toparse, ref int result)
        {
            try { result = int.Parse(toparse); return true; }
            catch (System.FormatException) { return false; }
        }

        private void portRaceSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            cs.port.port_race = currentGame.GetRaceObject(portRaceSelector.Text);
        }

        private void warpSectorText_TextChanged(object sender, EventArgs e)
        {
            int newWarpSector = 0;

            //If warp has not been check yet, then do nothing. The checked event will handle warp creation
            if (!warpCheckBox.Checked)
                return;

            //This occurs when a warp was deleted. The event can be ignored
            if (warpSectorText.Text == "")
                return;

            //This happens when an existing warp is set. Event can be ignored.
            if (cs.warp != null)
                if(warpSectorText.Text == cs.warp.sector_id.ToString())
                    return;

            if (intparse(warpSectorText.Text, ref newWarpSector))
                if (currentGame.GetSectorObject(newWarpSector) == null)
                    MessageBox.Show("Warp sector does not exist.", "Error", MessageBoxButtons.OK);
                else
                {
                    if (currentGame.GetSectorObject(newWarpSector).warp != null)
                        MessageBox.Show("Target warp sector already has a warp. Please change this first.", "Error", MessageBoxButtons.OK);
                    else
                    {
                        if(cs.warp != null)
                            cs.warp.warp = null;
                        cs.warp = currentGame.GetSectorObject(newWarpSector);
                        cs.warp.warp = cs;
                    }
                }
            else
                MessageBox.Show("Warp sector could not be read.", "Error", MessageBoxButtons.OK);
        }

        private void planetCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (planetCheckBox.Checked)
                cs.planet = new Planet();
            else
                cs.planet = null;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sectorStatusSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sectorStatusSelector.SelectedItem == "Friendly")
                cs.status = 1;
            if (sectorStatusSelector.SelectedItem == "Hostile")
            {
                //currentGame.ResetShortestRoutes(cs.sector_id);
                cs.status = -1;
            }
            if (sectorStatusSelector.SelectedItem == "Neutral")
                cs.status = 0;

            cs.Invalidate();
        }

        private void RemovePortButton_Click(object sender, EventArgs e)
        {
            cs.port = null;
            DrawSectorConfig();
        }

        private void CreatePortButton_Click(object sender, EventArgs e)
        {
            cs.port = new Port(currentGame.nrofgoods);
            cs.port.port_race = currentGame.race[1];
            DrawSectorConfig();
        }

        private void SectorConfig_Load(object sender, EventArgs e)
        {

        }

        private void EnemyScoutsField_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(EnemyScoutsField.Text, out cs.enemy_scouts))
                MessageBox.Show("Number of enemy scouts could not be parsed");
            //else if (cs.enemy_scouts > 0)
            //   cs.status = -1;
        }

        private void EnemyCdsField_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(EnemyCdsField.Text, out cs.enemy_cds))
                MessageBox.Show("Number of enemy cds could not be parsed");
            //else if (cs.enemy_cds > 0)
            //    cs.status = -1;
        }

        private void EnemyMinesField_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(EnemyMinesField.Text, out cs.enemy_mines))
                MessageBox.Show("Number of enemy mines could not be parsed");
            else if (cs.enemy_mines > 0)
                cs.status = -1;
        }

        private void FriendlyScoutsField_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(FriendlyScoutsField.Text, out cs.friendly_scouts))
                MessageBox.Show("Number of friendly scouts could not be parsed");
        }

        private void FriendlyCdsField_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(FriendlyCdsField.Text, out cs.friendly_cds))
                MessageBox.Show("Number of friendly cds could not be parsed");
        }

        private void FriendlyMinesField_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(FriendlyMinesField.Text, out cs.friendly_mines))
                MessageBox.Show("Number of friendly mines could not be parsed");
            else if (cs.friendly_mines > 0)
                cs.status = 1;
        }

        private void OwnerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = OwnerList.SelectedIndex;

            if (index == 0)
            {
                //Display the total amount of forces
                int totalenemymines = 0, totalenemyscouts = 0, totalenemycds = 0;
                int totalfriendlymines = 0, totalfriendlyscouts = 0, totalfriendlycds = 0;

                for (int i = 0; i < cs.force_stacks.Count; i++)
                {
                    if (((ForceData)(cs.force_stacks[i])).affiliation == 1)
                    {
                        totalfriendlycds += ((ForceData)(cs.force_stacks[i])).combat_drones;
                        totalfriendlyscouts += ((ForceData)(cs.force_stacks[i])).scout_drones;
                        totalfriendlymines += ((ForceData)(cs.force_stacks[i])).mines;
                    }
                    else
                    {
                        totalenemycds += ((ForceData)(cs.force_stacks[i])).combat_drones;
                        totalenemyscouts += ((ForceData)(cs.force_stacks[i])).scout_drones;
                        totalenemymines += ((ForceData)(cs.force_stacks[i])).mines;
                    }
                }

                EnemyMinesField.Text = totalenemymines.ToString();
                EnemyScoutsField.Text = totalenemyscouts.ToString();
                EnemyCdsField.Text = totalenemycds.ToString();

                FriendlyMinesField.Text = totalfriendlymines.ToString();
                FriendlyScoutsField.Text = totalfriendlyscouts.ToString();
                FriendlyCdsField.Text = totalfriendlycds.ToString();
            }
            else
            {
                int i = index-2;
                int totalenemymines = 0, totalenemyscouts = 0, totalenemycds = 0;
                int totalfriendlymines = 0, totalfriendlyscouts = 0, totalfriendlycds = 0;

                if (((ForceData)(cs.force_stacks[i])).affiliation == 1)
                {
                    totalfriendlycds = ((ForceData)(cs.force_stacks[i])).combat_drones;
                    totalfriendlyscouts = ((ForceData)(cs.force_stacks[i])).scout_drones;
                    totalfriendlymines = ((ForceData)(cs.force_stacks[i])).mines;
                }
                else
                {
                    totalenemycds = ((ForceData)(cs.force_stacks[i])).combat_drones;
                    totalenemyscouts = ((ForceData)(cs.force_stacks[i])).scout_drones;
                    totalenemymines = ((ForceData)(cs.force_stacks[i])).mines;
                }

                EnemyMinesField.Text = totalenemymines.ToString();
                EnemyScoutsField.Text = totalenemyscouts.ToString();
                EnemyCdsField.Text = totalenemycds.ToString();

                FriendlyMinesField.Text = totalfriendlymines.ToString();
                FriendlyScoutsField.Text = totalfriendlyscouts.ToString();
                FriendlyCdsField.Text = totalfriendlycds.ToString();
            }
        }
	}
}
