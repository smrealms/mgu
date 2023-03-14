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
	public class OptimalRoute : System.Windows.Forms.Form
	{
        private MainStuff hostApplication;
        private Game currentGame;
        public Route selectedRoute;

        bool randomroute = true, evade = false;
        long nrofcombinations = 1;

#region Standard Declarations
		private System.Windows.Forms.GroupBox Begin;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TextBox result4;
		private System.Windows.Forms.GroupBox End;
		private System.Windows.Forms.Button CalcButton;
		private System.Windows.Forms.ListBox routeList;
		private System.Windows.Forms.Button Closey;
		private System.Windows.Forms.RichTextBox routeSpecs;
		private System.Windows.Forms.GroupBox Result;
		private System.Windows.Forms.GroupBox Universe;
		private System.Windows.Forms.TextBox StartSectorField;
		private System.Windows.Forms.TextBox EndSectorField;
		private System.Windows.Forms.CheckedListBox GalaxyList;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBox1;
        private CheckBox checkBox3;
        private GroupBox groupBox1;
        private Button DeleteButton;
        private Button AddButton;
        private TextBox NewWayPointBox;
        private ContextMenu TechMenu;
        private ContextMenu WeaponMenu;
        private ContextMenu ShipMenu;
        private ContextMenu OtherMenu;
        private ImageList Locs;
        private ToolBarButton toolBarButton1;
        private ToolBarButton toolBarButton2;
        private ToolBarButton toolBarButton3;
        private ToolBarButton toolBarButton0;
        private ToolBar toolBar;
        //SubMenu Items for weapons per race
        private System.Windows.Forms.MenuItem AnyGun;
        private System.Windows.Forms.MenuItem AlskantW;
        private System.Windows.Forms.MenuItem CreontiW;
        private System.Windows.Forms.MenuItem HumanW;
        private System.Windows.Forms.MenuItem IkThorneW;
        private System.Windows.Forms.MenuItem ThevianW;
        private System.Windows.Forms.MenuItem SalveneW;
        private System.Windows.Forms.MenuItem WQHumanW;
        private System.Windows.Forms.MenuItem NeutralW;
        private System.Windows.Forms.MenuItem NijarinW;
        //Second Submenu Items for weapons per level 
        private System.Windows.Forms.MenuItem L1;
        private System.Windows.Forms.MenuItem L2;
        private System.Windows.Forms.MenuItem L3;
        private System.Windows.Forms.MenuItem L4;
        private System.Windows.Forms.MenuItem L5;
        //Submenu Items for ships per race
        private System.Windows.Forms.MenuItem AlskantS;
        private System.Windows.Forms.MenuItem CreontiS;
        private System.Windows.Forms.MenuItem HumanS;
        private System.Windows.Forms.MenuItem IkThorneS;
        private System.Windows.Forms.MenuItem SalveneS;
        private System.Windows.Forms.MenuItem ThevianS;
        private System.Windows.Forms.MenuItem WQHumanS;
        private System.Windows.Forms.MenuItem NijarinS;
        private System.Windows.Forms.MenuItem NeutralS;
        private System.Windows.Forms.MenuItem Sep1;
        private System.Windows.Forms.MenuItem Sep2;
        private System.Windows.Forms.MenuItem BarMenu;
        private System.Windows.Forms.MenuItem BankMenu;
        private System.Windows.Forms.MenuItem GovernmentMenu;
        private GroupBox WayPoints;
        private ListBox WayPointsList;
        private bool _destroyed = false;
        private TextBox maxRoutesText;
        private Label maxRoutesLabel;
        private Button HighlightRouteButton;
        private Button DeselectAllButton;
        private Button SelectAllButton;
        bool[] allowed;

		public bool destroyed
		{
			get { return _destroyed; }
		}

		public OptimalRoute(MainStuff host)
		{
			InitializeComponent();
            hostApplication = host;
            currentGame = host.games[host.currentGame];

            allowed = new bool[currentGame.nrofgalaxies];
            for (int x = 0; x < allowed.Length; x += 1)
                allowed[x] = true;
            this.GalaxyList.ItemCheck += new ItemCheckEventHandler(ItemCheck_Changed);
            this.maxRoutesText.Text = "10";
		}

		private void OptimalRoute_Load(object sender, System.EventArgs e)
		{
            for (int g = 1; g < hostApplication.games[hostApplication.currentGame].nrofgalaxies; g++)
            {
                GalaxyList.Items.Add(hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_name);
                GalaxyList.SetItemChecked(g-1, true);
            }

            AddGunsToMenu();
            AddTechToMenu();
            AddShipsToMenu();
            AddMiscToMenu();
		}

        #region Menu Filler Functions

        public void AddGunsToMenu()
        {
            AnyGun = new MenuItem("Any Gun");
            AnyGun.RadioCheck = true;
            AnyGun.Click += new EventHandler(ChooseLocation);
            WeaponMenu.MenuItems.Add(0, AnyGun);

            Sep1 = new MenuItem("-");
            WeaponMenu.MenuItems.Add(Sep1);

            //First add level SubMenus to the main menu
            L1 = new MenuItem("Level 1");
            L2 = new MenuItem("Level 2");
            L3 = new MenuItem("Level 3");
            L4 = new MenuItem("Level 4");
            L5 = new MenuItem("Level 5");
            WeaponMenu.MenuItems.Add(2, L1);
            WeaponMenu.MenuItems.Add(3, L2);
            WeaponMenu.MenuItems.Add(4, L3);
            WeaponMenu.MenuItems.Add(5, L4);
            WeaponMenu.MenuItems.Add(6, L5);

            //Fill the weapon submenus with the weapons in the current game
            for (int w = 0; w < hostApplication.games[hostApplication.currentGame].nrofweapons; w++)
            {
                MenuItem newWeap = new MenuItem(hostApplication.games[hostApplication.currentGame].weapon[w].name);
                newWeap.RadioCheck = true;
                newWeap.Click += new EventHandler(ChooseLocation);
                switch (hostApplication.games[hostApplication.currentGame].weapon[w].power)
                {
                    case 1: L1.MenuItems.Add(newWeap); break;
                    case 2: L2.MenuItems.Add(newWeap); break;
                    case 3: L3.MenuItems.Add(newWeap); break;
                    case 4: L4.MenuItems.Add(newWeap); break;
                    case 5: L5.MenuItems.Add(newWeap); break;
                }
            }

            Sep2 = new MenuItem("-");
            WeaponMenu.MenuItems.Add(Sep2);

            //First add race SubMenus to the main menu
            AlskantW = new MenuItem("Alskant");
            CreontiW = new MenuItem("Creonti");
            HumanW = new MenuItem("Human");
            IkThorneW = new MenuItem("Ik'thorne");
            SalveneW = new MenuItem("Salvene");
            ThevianW = new MenuItem("Thevian");
            WQHumanW = new MenuItem("WQ Human");
            NijarinW = new MenuItem("Nijarin");
            NeutralW = new MenuItem("Neutral");
            WeaponMenu.MenuItems.Add(AlskantW);
            WeaponMenu.MenuItems.Add(CreontiW);
            WeaponMenu.MenuItems.Add(HumanW);
            WeaponMenu.MenuItems.Add(IkThorneW);
            WeaponMenu.MenuItems.Add(SalveneW);
            WeaponMenu.MenuItems.Add(ThevianW);
            WeaponMenu.MenuItems.Add(WQHumanW);
            WeaponMenu.MenuItems.Add(NijarinW);
            WeaponMenu.MenuItems.Add(NeutralW);

            //Fill the weapon submenus with the weapons in the current game
            for (int w = 1; w < hostApplication.games[hostApplication.currentGame].nrofweapons; w++)
            {
                MenuItem newWeap = new MenuItem(hostApplication.games[hostApplication.currentGame].weapon[w].name);
                newWeap.RadioCheck = true;
                newWeap.Click += new EventHandler(ChooseLocation);

                switch (hostApplication.games[hostApplication.currentGame].weapon[w].weapon_race.race_name)
                {
                    case "Alskant": AlskantW.MenuItems.Add(newWeap); break;
                    case "Creonti": CreontiW.MenuItems.Add(newWeap); break;
                    case "Human": HumanW.MenuItems.Add(newWeap); break;
                    case "Ik'Thorne": IkThorneW.MenuItems.Add(newWeap); break;
                    case "Salvene": SalveneW.MenuItems.Add(newWeap); break;
                    case "Thevian": ThevianW.MenuItems.Add(newWeap); break;
                    case "WQ Human": WQHumanW.MenuItems.Add(newWeap); break;
                    case "Nijarin": NijarinW.MenuItems.Add(newWeap); break;
                    case "Neutral": NeutralW.MenuItems.Add(newWeap); break;
                }
            }
        }

        public void AddTechToMenu()
        {
            for (int t = 0; t < hostApplication.games[hostApplication.currentGame].nroftechs; t++)
            {
                MenuItem newTech = new MenuItem(hostApplication.games[hostApplication.currentGame].technology[t].name);
                newTech.RadioCheck = true;
                newTech.Click += new EventHandler(ChooseLocation);
                TechMenu.MenuItems.Add(newTech);
            }
        }

        public void AddShipsToMenu()
        {
            //First add race SubMenus to the main menu
            AlskantS = new MenuItem("Alskant");
            CreontiS = new MenuItem("Creonti");
            HumanS = new MenuItem("Human");
            IkThorneS = new MenuItem("Ik'thorne");
            SalveneS = new MenuItem("Salvene");
            ThevianS = new MenuItem("Thevian");
            WQHumanS = new MenuItem("WQ Human");
            NijarinS = new MenuItem("Nijarin");
            NeutralS = new MenuItem("Neutral");
            ShipMenu.MenuItems.Add(AlskantS);
            ShipMenu.MenuItems.Add(CreontiS);
            ShipMenu.MenuItems.Add(HumanS);
            ShipMenu.MenuItems.Add(IkThorneS);
            ShipMenu.MenuItems.Add(SalveneS);
            ShipMenu.MenuItems.Add(ThevianS);
            ShipMenu.MenuItems.Add(WQHumanS);
            ShipMenu.MenuItems.Add(NijarinS);
            ShipMenu.MenuItems.Add(NeutralS);

            //Fill the weapon submenus with the weapons in the current game
            for (int s = 0; s < hostApplication.games[hostApplication.currentGame].nrofships; s++)
            {
                MenuItem newShip = new MenuItem(hostApplication.games[hostApplication.currentGame].ship[s].name);
                newShip.RadioCheck = true;
                newShip.Click += new EventHandler(ChooseLocation);

                switch (hostApplication.games[hostApplication.currentGame].ship[s].ship_race.race_name)
                {
                    case "Alskant": AlskantS.MenuItems.Add(newShip); break;
                    case "Creonti": CreontiS.MenuItems.Add(newShip); break;
                    case "Human": HumanS.MenuItems.Add(newShip); break;
                    case "Ik'Thorne": IkThorneS.MenuItems.Add(newShip); break;
                    case "Salvene": SalveneS.MenuItems.Add(newShip); break;
                    case "Thevian": ThevianS.MenuItems.Add(newShip); break;
                    case "WQ Human": WQHumanS.MenuItems.Add(newShip); break;
                    case "Nijarin": NijarinS.MenuItems.Add(newShip); break;
                    case "Neutral": NeutralS.MenuItems.Add(newShip); break;
                }
            }
        }

        public void AddMiscToMenu()
        {
            //Add the main menu items first
            BarMenu = new MenuItem("Bar");
            OtherMenu.MenuItems.Add(BarMenu);

            //Fill Bar Menu
            MenuItem newItem = new MenuItem("Any Bar");
            newItem.RadioCheck = true;
            newItem.Click += new EventHandler(ChooseLocation);
            BarMenu.MenuItems.Add(newItem);

            //Add the main menu items first
            BankMenu = new MenuItem("Bank");
            OtherMenu.MenuItems.Add(BankMenu);

            //Fill Bank Menu
            newItem = new MenuItem("Any Bank");
            newItem.RadioCheck = true;
            newItem.Click += new EventHandler(ChooseLocation);
            BankMenu.MenuItems.Add(newItem);

            //Add the main menu items first
            GovernmentMenu = new MenuItem("Government");
            OtherMenu.MenuItems.Add(GovernmentMenu);

            //Fill Bar Menu
            newItem = new MenuItem("Federal Beacon");
            newItem.RadioCheck = true;
            newItem.Click += new EventHandler(ChooseLocation);
            GovernmentMenu.MenuItems.Add(newItem);

            for (int l = 0; l < hostApplication.games[hostApplication.currentGame].nroflocations; l++)
            {
                MenuItem newLocation = new MenuItem(hostApplication.games[hostApplication.currentGame].location[l].location_name);
                newLocation.RadioCheck = true;
                newLocation.Click += new EventHandler(ChooseLocation);

                switch (hostApplication.games[hostApplication.currentGame].location[l].location_type)
                {
                    case "Bar": BarMenu.MenuItems.Add(newLocation); break;
                    case "Bank": BankMenu.MenuItems.Add(newLocation); break;
                    case "HQ": GovernmentMenu.MenuItems.Add(newLocation); break;
                    case "UG": GovernmentMenu.MenuItems.Add(newLocation); break;
                }
            }
        }

        #endregion

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				_destroyed = true;
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
#endregion

#region Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MGU.Forms.OptimalRoute));
            this.Begin = new System.Windows.Forms.GroupBox();
            this.StartSectorField = new System.Windows.Forms.TextBox();
            this.result4 = new System.Windows.Forms.TextBox();
            this.EndSectorField = new System.Windows.Forms.TextBox();
            this.End = new System.Windows.Forms.GroupBox();
            this.CalcButton = new System.Windows.Forms.Button();
            this.routeList = new System.Windows.Forms.ListBox();
            this.Closey = new System.Windows.Forms.Button();
            this.routeSpecs = new System.Windows.Forms.RichTextBox();
            this.Result = new System.Windows.Forms.GroupBox();
            this.Universe = new System.Windows.Forms.GroupBox();
            this.GalaxyList = new System.Windows.Forms.CheckedListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.maxRoutesText = new System.Windows.Forms.TextBox();
            this.maxRoutesLabel = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.NewWayPointBox = new System.Windows.Forms.TextBox();
            this.TechMenu = new System.Windows.Forms.ContextMenu();
            this.WeaponMenu = new System.Windows.Forms.ContextMenu();
            this.ShipMenu = new System.Windows.Forms.ContextMenu();
            this.OtherMenu = new System.Windows.Forms.ContextMenu();
            this.Locs = new System.Windows.Forms.ImageList(this.components);
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton0 = new System.Windows.Forms.ToolBarButton();
            this.WayPoints = new System.Windows.Forms.GroupBox();
            this.WayPointsList = new System.Windows.Forms.ListBox();
            this.HighlightRouteButton = new System.Windows.Forms.Button();
            this.DeselectAllButton = new System.Windows.Forms.Button();
            this.SelectAllButton = new System.Windows.Forms.Button();
            this.Begin.SuspendLayout();
            this.End.SuspendLayout();
            this.Result.SuspendLayout();
            this.Universe.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.WayPoints.SuspendLayout();
            this.SuspendLayout();
            // 
            // Begin
            // 
            this.Begin.Controls.Add(this.StartSectorField);
            this.Begin.Location = new System.Drawing.Point(15, 108);
            this.Begin.Name = "Begin";
            this.Begin.Size = new System.Drawing.Size(92, 44);
            this.Begin.TabIndex = 0;
            this.Begin.TabStop = false;
            this.Begin.Text = "Begin sector";
            // 
            // StartSectorField
            // 
            this.StartSectorField.Location = new System.Drawing.Point(8, 16);
            this.StartSectorField.Name = "StartSectorField";
            this.StartSectorField.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartSectorField.Size = new System.Drawing.Size(76, 20);
            this.StartSectorField.TabIndex = 0;
            this.StartSectorField.TextChanged += new System.EventHandler(this.StartSectorField_TextChanged);
            // 
            // result4
            // 
            this.result4.Location = new System.Drawing.Point(8, 88);
            this.result4.Name = "result4";
            this.result4.ReadOnly = true;
            this.result4.Size = new System.Drawing.Size(112, 20);
            this.result4.TabIndex = 8;
            // 
            // EndSectorField
            // 
            this.EndSectorField.Location = new System.Drawing.Point(8, 14);
            this.EndSectorField.Name = "EndSectorField";
            this.EndSectorField.Size = new System.Drawing.Size(76, 20);
            this.EndSectorField.TabIndex = 5;
            // 
            // End
            // 
            this.End.Controls.Add(this.EndSectorField);
            this.End.ForeColor = System.Drawing.SystemColors.ControlText;
            this.End.Location = new System.Drawing.Point(111, 108);
            this.End.Name = "End";
            this.End.Size = new System.Drawing.Size(92, 44);
            this.End.TabIndex = 10;
            this.End.TabStop = false;
            this.End.Text = "End sector";
            // 
            // CalcButton
            // 
            this.CalcButton.Location = new System.Drawing.Point(8, 562);
            this.CalcButton.Name = "CalcButton";
            this.CalcButton.Size = new System.Drawing.Size(72, 24);
            this.CalcButton.TabIndex = 12;
            this.CalcButton.Text = "Calculate";
            this.CalcButton.Click += new System.EventHandler(this.Calculate);
            // 
            // routeList
            // 
            this.routeList.HorizontalScrollbar = true;
            this.routeList.Location = new System.Drawing.Point(8, 16);
            this.routeList.Name = "routeList";
            this.routeList.Size = new System.Drawing.Size(756, 173);
            this.routeList.TabIndex = 14;
            this.routeList.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // Closey
            // 
            this.Closey.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Closey.Location = new System.Drawing.Point(700, 562);
            this.Closey.Name = "Closey";
            this.Closey.Size = new System.Drawing.Size(80, 24);
            this.Closey.TabIndex = 15;
            this.Closey.Text = "Close";
            this.Closey.Click += new System.EventHandler(this.Closey_Click);
            // 
            // routeSpecs
            // 
            this.routeSpecs.BackColor = System.Drawing.SystemColors.Info;
            this.routeSpecs.ForeColor = System.Drawing.SystemColors.InfoText;
            this.routeSpecs.Location = new System.Drawing.Point(8, 188);
            this.routeSpecs.Name = "routeSpecs";
            this.routeSpecs.ReadOnly = true;
            this.routeSpecs.Size = new System.Drawing.Size(756, 180);
            this.routeSpecs.TabIndex = 16;
            this.routeSpecs.Text = "";
            this.routeSpecs.TextChanged += new System.EventHandler(this.routeSpecs_TextChanged);
            // 
            // Result
            // 
            this.Result.Controls.Add(this.routeList);
            this.Result.Controls.Add(this.routeSpecs);
            this.Result.Location = new System.Drawing.Point(8, 184);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(772, 374);
            this.Result.TabIndex = 17;
            this.Result.TabStop = false;
            this.Result.Text = "Results";
            // 
            // Universe
            // 
            this.Universe.Controls.Add(this.GalaxyList);
            this.Universe.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Universe.Location = new System.Drawing.Point(215, 5);
            this.Universe.Name = "Universe";
            this.Universe.Size = new System.Drawing.Size(188, 147);
            this.Universe.TabIndex = 19;
            this.Universe.TabStop = false;
            this.Universe.Text = "Allowed galaxies";
            // 
            // GalaxyList
            // 
            this.GalaxyList.CheckOnClick = true;
            this.GalaxyList.Location = new System.Drawing.Point(8, 16);
            this.GalaxyList.Name = "GalaxyList";
            this.GalaxyList.Size = new System.Drawing.Size(172, 124);
            this.GalaxyList.TabIndex = 0;
            this.GalaxyList.SelectedIndexChanged += new System.EventHandler(this.GalaxyList_SelectedIndexChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(84, 566);
            this.progressBar1.Maximum = 10;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(398, 16);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 20;
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(15, 18);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(171, 20);
            this.checkBox1.TabIndex = 28;
            this.checkBox1.Text = "Evade hostile (red) sectors";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(15, 44);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(171, 20);
            this.checkBox3.TabIndex = 30;
            this.checkBox3.Text = "Use any route order";
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.maxRoutesText);
            this.groupBox1.Controls.Add(this.maxRoutesLabel);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Location = new System.Drawing.Point(9, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 97);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Plot options";
            // 
            // maxRoutesText
            // 
            this.maxRoutesText.Location = new System.Drawing.Point(110, 72);
            this.maxRoutesText.Name = "maxRoutesText";
            this.maxRoutesText.Size = new System.Drawing.Size(76, 20);
            this.maxRoutesText.TabIndex = 6;
            // 
            // maxRoutesLabel
            // 
            this.maxRoutesLabel.AutoSize = true;
            this.maxRoutesLabel.Location = new System.Drawing.Point(12, 75);
            this.maxRoutesLabel.Name = "maxRoutesLabel";
            this.maxRoutesLabel.Size = new System.Drawing.Size(86, 13);
            this.maxRoutesLabel.TabIndex = 31;
            this.maxRoutesLabel.Text = "Max nr or routes:";
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(527, 108);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(106, 22);
            this.DeleteButton.TabIndex = 34;
            this.DeleteButton.Text = "Delete Waypoint";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(419, 108);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(102, 22);
            this.AddButton.TabIndex = 35;
            this.AddButton.Text = "Add Waypoint";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // NewWayPointBox
            // 
            this.NewWayPointBox.Location = new System.Drawing.Point(419, 84);
            this.NewWayPointBox.Name = "NewWayPointBox";
            this.NewWayPointBox.Size = new System.Drawing.Size(214, 20);
            this.NewWayPointBox.TabIndex = 36;
            // 
            // Locs
            // 
            this.Locs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Locs.ImageStream")));
            this.Locs.TransparentColor = System.Drawing.Color.Transparent;
            this.Locs.Images.SetKeyName(0, "");
            this.Locs.Images.SetKeyName(1, "");
            this.Locs.Images.SetKeyName(2, "");
            this.Locs.Images.SetKeyName(3, "");
            this.Locs.Images.SetKeyName(4, "");
            this.Locs.Images.SetKeyName(5, "");
            this.Locs.Images.SetKeyName(6, "");
            this.Locs.Images.SetKeyName(7, "");
            this.Locs.Images.SetKeyName(8, "");
            this.Locs.Images.SetKeyName(9, "");
            // 
            // toolBar
            // 
            this.toolBar.AutoSize = false;
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton1,
            this.toolBarButton3,
            this.toolBarButton2,
            this.toolBarButton0});
            this.toolBar.ButtonSize = new System.Drawing.Size(92, 30);
            this.toolBar.Divider = false;
            this.toolBar.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBar.DropDownArrows = true;
            this.toolBar.ImageList = this.Locs;
            this.toolBar.Location = new System.Drawing.Point(419, 5);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(214, 71);
            this.toolBar.TabIndex = 7;
            this.toolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.DropDownMenu = this.TechMenu;
            this.toolBarButton1.ImageIndex = 6;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton1.Text = "Technology";
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.DropDownMenu = this.ShipMenu;
            this.toolBarButton3.ImageIndex = 8;
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton3.Text = "Ships";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.DropDownMenu = this.WeaponMenu;
            this.toolBarButton2.ImageIndex = 7;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton2.Text = "Weapons ";
            // 
            // toolBarButton0
            // 
            this.toolBarButton0.DropDownMenu = this.OtherMenu;
            this.toolBarButton0.ImageIndex = 5;
            this.toolBarButton0.Name = "toolBarButton0";
            this.toolBarButton0.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton0.Text = "Other";
            // 
            // WayPoints
            // 
            this.WayPoints.Controls.Add(this.WayPointsList);
            this.WayPoints.Location = new System.Drawing.Point(639, 5);
            this.WayPoints.Name = "WayPoints";
            this.WayPoints.Size = new System.Drawing.Size(141, 132);
            this.WayPoints.TabIndex = 37;
            this.WayPoints.TabStop = false;
            this.WayPoints.Text = "Waypoints";
            // 
            // WayPointsList
            // 
            this.WayPointsList.FormattingEnabled = true;
            this.WayPointsList.Location = new System.Drawing.Point(6, 16);
            this.WayPointsList.Name = "WayPointsList";
            this.WayPointsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.WayPointsList.Size = new System.Drawing.Size(127, 108);
            this.WayPointsList.TabIndex = 33;
            // 
            // HighlightRouteButton
            // 
            this.HighlightRouteButton.Location = new System.Drawing.Point(488, 562);
            this.HighlightRouteButton.Name = "HighlightRouteButton";
            this.HighlightRouteButton.Size = new System.Drawing.Size(206, 23);
            this.HighlightRouteButton.TabIndex = 38;
            this.HighlightRouteButton.Text = "Highlight and go to Route";
            this.HighlightRouteButton.UseVisualStyleBackColor = true;
            this.HighlightRouteButton.Click += new System.EventHandler(this.HighlightRouteButton_Click);
            // 
            // DeselectAllButton
            // 
            this.DeselectAllButton.Location = new System.Drawing.Point(314, 155);
            this.DeselectAllButton.Name = "DeselectAllButton";
            this.DeselectAllButton.Size = new System.Drawing.Size(88, 23);
            this.DeselectAllButton.TabIndex = 62;
            this.DeselectAllButton.Text = "Deselect all";
            this.DeselectAllButton.UseVisualStyleBackColor = true;
            this.DeselectAllButton.Click += new System.EventHandler(this.DeselectAllButton_Click);
            // 
            // SelectAllButton
            // 
            this.SelectAllButton.Location = new System.Drawing.Point(215, 155);
            this.SelectAllButton.Name = "SelectAllButton";
            this.SelectAllButton.Size = new System.Drawing.Size(85, 23);
            this.SelectAllButton.TabIndex = 61;
            this.SelectAllButton.Text = "Select all";
            this.SelectAllButton.UseVisualStyleBackColor = true;
            this.SelectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // OptimalRoute
            // 
            this.AcceptButton = this.CalcButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.Closey;
            this.ClientSize = new System.Drawing.Size(786, 589);
            this.Controls.Add(this.DeselectAllButton);
            this.Controls.Add(this.SelectAllButton);
            this.Controls.Add(this.WayPoints);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.HighlightRouteButton);
            this.Controls.Add(this.NewWayPointBox);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Universe);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.Closey);
            this.Controls.Add(this.CalcButton);
            this.Controls.Add(this.Begin);
            this.Controls.Add(this.End);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptimalRoute";
            this.Text = "The best route of the whole universe calculator";
            this.Load += new System.EventHandler(this.OptimalRoute_Load);
            this.Begin.ResumeLayout(false);
            this.Begin.PerformLayout();
            this.End.ResumeLayout(false);
            this.End.PerformLayout();
            this.Result.ResumeLayout(false);
            this.Universe.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.WayPoints.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
#endregion

        ArrayList shortestRoutes;
        ArrayList[] locations;

		private void Calculate(object sender, System.EventArgs e)
		{
            int startSector, endSector, waypoint, maxnrofroutes = 10;
            long nrofroutes = 0;
            ArrayList[] locations = new ArrayList[10];
            int[][] combinations;
            ArrayList newRoutes, routes;
            ArrayList oldRoute;

            //Read maximum nr of routes
            if (!int.TryParse(maxRoutesText.Text, out maxnrofroutes))
            {
                routeSpecs.Text = "Reading of the maximum number of routes failed. Default value 10 is assumed.";
                maxnrofroutes = 10;
            }

            //Read sector numbers
		    if(!int.TryParse(StartSectorField.Text, out startSector))
            {
                routeSpecs.Text = "Reading of the start sector failed: please enter a valid sector number";
                return;
            }
            else if(startSector < 0 || startSector > hostApplication.games[hostApplication.currentGame].nrofsectors)
            {
                routeSpecs.Text = "Start sector number is invalid. Please use an existing sector number.";
                return;
            }
            if(!int.TryParse(EndSectorField.Text, out endSector) && EndSectorField.Text != "")
            {
                routeSpecs.Text = "End sector number is invalid.";
                return;
            }
            else if (EndSectorField.Text == "")
            {
                routeSpecs.Text = "No end sector has been given so none is used for route calculation.";
                endSector = -1;
            }
            else if (endSector < 0 || endSector > hostApplication.games[hostApplication.currentGame].nrofsectors)
            {
                routeSpecs.Text = "End sector number is invalid. No end sector is used for route calculation.";
                endSector = -1;
            }
            if (WayPointsList.Items.Count == 0)
            {
                routeSpecs.Text = "Please select at least one waypoint.";
                return;
            }
            if (!GalaxyList.CheckedIndices.Contains(currentGame.GetGalaxyIndex(startSector)-1))
            {
                routeSpecs.Text = "Starting galaxy not allowed";
                return;
            }
            if (!GalaxyList.CheckedIndices.Contains(currentGame.GetGalaxyIndex(endSector) - 1) && EndSectorField.Text != "")
            {
                routeSpecs.Text = "End galaxy not allowed";
                return;
            }

            //Clear route list
            routeSpecs.Text = "";
            routeList.Items.Clear();

            string locationtype;

            for (int wp = 0; wp < 10; wp++)
            {
                locations[wp] = new ArrayList();
            }

            //Read the waypoints and generate the locations where they are sold
            for (int wp = 0; wp < WayPointsList.SelectedItems.Count; wp++)
            {
                if (int.TryParse(WayPointsList.SelectedItems[wp].ToString(), out waypoint))
                    if (currentGame.GetGalaxyIndex(waypoint) > 0)
                    {
                        locations[wp].Add(currentGame.GetSectorObject(waypoint).sector_id);
                        continue;
                    }

                for (int g = 1; g < hostApplication.games[hostApplication.currentGame].nrofgalaxies; g++)
                    for (int x = 0; x < hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_xsize; x++)
                        for (int y = 0; y < hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_ysize; y++)    
                            for (int l = 0; l < hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].nroflocations; l++)
                            {
                                if (!GalaxyList.GetItemChecked(g-1))
                                    continue;

                                locationtype = hostApplication.games[hostApplication.currentGame].location[hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].location[l]].location_type;
                                for (int s = 0; s < hostApplication.games[hostApplication.currentGame].location[hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].location[l]].nrofsolditems; s++)
                                {
                                    if (locationtype == "Weapon Shop")
                                        if (hostApplication.games[hostApplication.currentGame].weapon[hostApplication.games[hostApplication.currentGame].location[hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].location[l]].sells[s]].name == WayPointsList.SelectedItems[wp].ToString())
                                            locations[wp].Add(hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].sector_id);
                                                                                 
                                    if (locationtype == "Ship Shop")
                                        if (hostApplication.games[hostApplication.currentGame].ship[hostApplication.games[hostApplication.currentGame].location[hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].location[l]].sells[s]].name == WayPointsList.SelectedItems[wp].ToString())
                                            locations[wp].Add(hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].sector_id);

                                    if (locationtype == "Technology Shop")
                                        if (hostApplication.games[hostApplication.currentGame].technology[hostApplication.games[hostApplication.currentGame].location[hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].location[l]].sells[s]].name == WayPointsList.SelectedItems[wp].ToString())
                                            locations[wp].Add(hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].sector_id);
                                }
                                if (locationtype == "Bar" || locationtype == "Bank" || locationtype == "HQ" || locationtype == "HQ" || locationtype == "FED")
                                    if (hostApplication.games[hostApplication.currentGame].location[hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].location[l]].location_name == WayPointsList.SelectedItems[wp].ToString())
                                        locations[wp].Add(hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].sector_id);

                                if (WayPointsList.SelectedItems[wp].ToString() == "Federal Beacon" && locationtype == "Fed")
                                    locations[wp].Add(hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].sector_id);

                                if (WayPointsList.SelectedItems[wp].ToString() == "Any Bar" && locationtype == "Bar")
                                    locations[wp].Add(hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].sector_id);

                                if (WayPointsList.SelectedItems[wp].ToString() == "Any Bank" && locationtype == "Bank")
                                    locations[wp].Add(hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].sector_id);

                                if (WayPointsList.SelectedItems[wp].ToString() == "Any Gun" && locationtype == "Weapon Shop")
                                    locations[wp].Add(hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].sector_id); 
                            }
            }

                            

            //If the exact given order is used, then no permutations are necessary
            nrofcombinations = 1;

            if (randomroute)
            {
                //Compute number of combinations (The Math-library sadly lacks a faculty function)
                for (int fac = 1; fac <= WayPointsList.SelectedIndices.Count; fac++)
                    nrofcombinations *= fac;
            }

            //Retrieve all possible permutations of the waypoints
            combinations = new int[nrofcombinations][];
            for(int i = 0; i < nrofcombinations; i++)
                combinations[i] = new int[WayPointsList.SelectedIndices.Count];

            //First generate all identical orders
            for(int comb = 0; comb < nrofcombinations; comb++)
                for(int perm = 0; perm < WayPointsList.SelectedIndices.Count; perm++)
                    combinations[comb][perm] = perm; 

            //Then swap the orders
            for(int comb = 1; comb < nrofcombinations; comb++)
            {
                int newcomb = comb;
                for (int perm = 1; perm <= WayPointsList.SelectedIndices.Count; perm++)
                {
                    int intermediate = combinations[comb][perm-1];
                    combinations[comb][perm-1] = combinations[comb][(newcomb % perm)];
                    combinations[comb][(newcomb % perm)] = intermediate;
                    newcomb /= perm;
                }
            }

            nrofroutes = nrofcombinations;
            for (int wp = 0; wp < WayPointsList.SelectedIndices.Count; wp++)
               nrofcombinations *= locations[wp].Count;

            //Output the number of combinations
            routeSpecs.Text = "Total combinations possible: " + nrofcombinations.ToString();
            routeSpecs.Text += "\nPlease stand by while routes are being calculated";
            progressBar1.Maximum = (int)nrofcombinations;
            progressBar1.Value = 0;
            shortestRoutes = new ArrayList(maxnrofroutes);

            for (int wp = WayPointsList.SelectedItems.Count; wp < 10; wp++)
            {
                locations[wp].Add(endSector);
            }

            //Fill in locations and generate routes for each order
            for(int c = 0; c < combinations.Length; c++)
                for(int wp0 = 0; wp0 < locations[0].Count; wp0++)
                    for(int wp1 = 0; wp1 < locations[1].Count; wp1++)
                        for(int wp2 = 0; wp2< locations[2].Count; wp2++)
                            for(int wp3 = 0; wp3 < locations[3].Count; wp3++)
                                for(int wp4 = 0; wp4 < locations[4].Count; wp4++)
                                    for(int wp5 = 0; wp5 < locations[5].Count; wp5++)
                                        for (int wp6 = 0; wp6 < locations[6].Count; wp6++)
                                            for (int wp7 = 0; wp7 < locations[7].Count; wp7++)
                                                for (int wp8 = 0; wp8 < locations[8].Count; wp8++)
                                                    for (int wp9 = 0; wp9 < locations[9].Count; wp9++)
                                                    {
                                                        //Create a new, empty, route and indicater that it is a route in this game
                                                        Route newRoute = new Route(hostApplication.games[hostApplication.currentGame]);

                                                        //Start by adding the start sector as indicated by the user
                                                        newRoute.sectors.Add(startSector);

                                                        //Then, for each pair of waypoints, compute the shortest route between the waypoints and add it to the route
                                                        for (int o = 0; o < WayPointsList.SelectedIndices.Count; o++)
                                                        {   
                                                            switch (combinations[c][o])
                                                            {
                                                                case 0:
                                                                    //If the maximum nmber of routes has been reached and this route is already worse than the worst route so far, stop computing the route and skip to the next one
                                                                    //Else, add the route between the start sector and waypoint 1
                                                                    if (shortestRoutes.Count == maxnrofroutes && newRoute.length > ((Route)shortestRoutes[maxnrofroutes - 1]).length) continue;
                                                                    else newRoute.AppendRoute((int)locations[0][wp0], allowed, evade);
                                                                    break;

                                                                case 1:
                                                                    //If the maximum nmber of routes has been reached and this route is already worse than the worst route so far, stop computing the route and skip to the next one
                                                                    //Else, add the route between waypoint 1 and waypoint 2
                                                                    if (shortestRoutes.Count == maxnrofroutes && newRoute.length > ((Route)shortestRoutes[maxnrofroutes - 1]).length) continue;
                                                                    else newRoute.AppendRoute((int)locations[1][wp1], allowed, evade);
                                                                    break;

                                                                case 2:
                                                                    //If the maximum nmber of routes has been reached and this route is already worse than the worst route so far, stop computing the route and skip to the next one
                                                                    //Else, add the route between waypoint 2 and waypoint 3
                                                                    if (shortestRoutes.Count == maxnrofroutes && newRoute.length > ((Route)shortestRoutes[maxnrofroutes - 1]).length) continue;
                                                                    else newRoute.AppendRoute((int)locations[2][wp2], allowed, evade);
                                                                    break;

                                                                case 3:
                                                                    //If the maximum nmber of routes has been reached and this route is already worse than the worst route so far, stop computing the route and skip to the next one
                                                                    //Else, add the route between waypoint 3 and waypoint 4
                                                                    if (shortestRoutes.Count == maxnrofroutes && newRoute.length > ((Route)shortestRoutes[maxnrofroutes - 1]).length) continue;
                                                                    else newRoute.AppendRoute((int)locations[3][wp3], allowed, evade);
                                                                    break;

                                                                case 4:
                                                                    //If the maximum nmber of routes has been reached and this route is already worse than the worst route so far, stop computing the route and skip to the next one
                                                                    //Else, add the route between waypoint 4 and waypoint 5
                                                                    if (shortestRoutes.Count == maxnrofroutes && newRoute.length > ((Route)shortestRoutes[maxnrofroutes - 1]).length) continue;
                                                                    else newRoute.AppendRoute((int)locations[4][wp4], allowed, evade);
                                                                    break;

                                                                case 5:
                                                                    //If the maximum nmber of routes has been reached and this route is already worse than the worst route so far, stop computing the route and skip to the next one
                                                                    //Else, add the route between waypoint 5 and waypoint 6
                                                                    if (shortestRoutes.Count == maxnrofroutes && newRoute.length > ((Route)shortestRoutes[maxnrofroutes - 1]).length) continue;
                                                                    else newRoute.AppendRoute((int)locations[5][wp5], allowed, evade);
                                                                    break;

                                                                case 6:
                                                                    //If the maximum nmber of routes has been reached and this route is already worse than the worst route so far, stop computing the route and skip to the next one
                                                                    //Else, add the route between waypoint 6 and waypoint 7
                                                                    if (shortestRoutes.Count == maxnrofroutes && newRoute.length > ((Route)shortestRoutes[maxnrofroutes - 1]).length) continue;
                                                                    else newRoute.AppendRoute((int)locations[6][wp6], allowed, evade);
                                                                    break;

                                                                case 7:
                                                                    //If the maximum nmber of routes has been reached and this route is already worse than the worst route so far, stop computing the route and skip to the next one
                                                                    //Else, add the route between waypoint 7 and waypoint 8
                                                                    if (shortestRoutes.Count == maxnrofroutes && newRoute.length > ((Route)shortestRoutes[maxnrofroutes - 1]).length) continue;
                                                                    else newRoute.AppendRoute((int)locations[7][wp7], allowed, evade);
                                                                    break;

                                                                case 8:
                                                                    //If the maximum nmber of routes has been reached and this route is already worse than the worst route so far, stop computing the route and skip to the next one
                                                                    //Else, add the route between waypoint 8 and waypoint 9
                                                                    if (shortestRoutes.Count == maxnrofroutes && newRoute.length > ((Route)shortestRoutes[maxnrofroutes - 1]).length) continue;
                                                                    else newRoute.AppendRoute((int)locations[8][wp8], allowed, evade);
                                                                    break;

                                                                case 9:
                                                                    //If the maximum nmber of routes has been reached and this route is already worse than the worst route so far, stop computing the route and skip to the next one
                                                                    //Else, add the route between waypoint 9 and waypoint 10
                                                                    if (shortestRoutes.Count == maxnrofroutes && newRoute.length > ((Route)shortestRoutes[maxnrofroutes - 1]).length) continue;
                                                                    else newRoute.AppendRoute((int)locations[9][wp9], allowed, evade);
                                                                    break;
                                                            }
                                                        }

                                                        //Finally, if an end sector has been given, add the route from the last waypoint to the given end sector
                                                        if (endSector != -1)
                                                            newRoute.AppendRoute(endSector, allowed, evade);

                                                        progressBar1.Value++;

                                                        //If thenew route is flawed for any reason, then do not add but continue with the next route
                                                        if (newRoute.length <= 0)
                                                            break;

                                                        if(shortestRoutes.Count == maxnrofroutes)
                                                        {
                                                            if (newRoute.length < ((Route)shortestRoutes[maxnrofroutes - 1]).length)
                                                                shortestRoutes.RemoveAt(maxnrofroutes - 1);
                                                            else continue;
                                                        }

                                                        if (shortestRoutes.Count == 0)
                                                            shortestRoutes.Add(newRoute);
                                                        else
                                                        {
                                                            for (int i = 0; i <= shortestRoutes.Count; i++)
                                                            {
                                                                if (i == shortestRoutes.Count)
                                                                {
                                                                    shortestRoutes.Add(newRoute);
                                                                    break;
                                                                }
                                                                else if (((Route)shortestRoutes[i]).Equals(newRoute))
                                                                {
                                                                    i = shortestRoutes.Count;
                                                                }
                                                                else if (newRoute.length < ((Route)shortestRoutes[i]).length)
                                                                {
                                                                    shortestRoutes.Insert(i, newRoute);
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }

            //Display all routes
            string displaytext = "";
            if (shortestRoutes.Count == 0)
            {
                routeSpecs.Text = "No routes have been found with these settings";
            }
            else
            {
                routeSpecs.Text = "Total combinations possible: " + nrofcombinations.ToString();
                routeSpecs.Text += "\nRoute calculation finished";
            }
            for (int i = 0; i < shortestRoutes.Count; i++)
            {
                Route currentRoute = (Route)shortestRoutes[i];
                displaytext += currentRoute.length + " turns - #" + currentRoute.sectors[0].ToString() + "(" + hostApplication.games[hostApplication.currentGame].galaxy[hostApplication.games[hostApplication.currentGame].GetGalaxyIndex((int)currentRoute.sectors[0])].galaxy_name + ") ";
                for(int s = 0; s < currentRoute.waypoints.Count; s++)
                {
                    displaytext += "- #";
                    displaytext += currentRoute.waypoints[s].ToString();
                    displaytext += "(";
                    displaytext += hostApplication.games[hostApplication.currentGame].galaxy[hostApplication.games[hostApplication.currentGame].GetGalaxyIndex((int)currentRoute.waypoints[s])].galaxy_name;
                    displaytext += ") ";
                }
                routeList.Items.Add(displaytext);
                displaytext = "";
            }
		}

		public void LocationsChanged()
		{
		}

		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            selectedRoute = (Route) shortestRoutes[routeList.SelectedIndex];
            Sector cs;
            int nextWaypoint = 0, currentGalaxy = 0;
            string resultText = "";
            ArrayList itemsToBuy = new ArrayList();
            bool[] alreadyBought = new bool[WayPointsList.Items.Count];

            //Keep track of what items have been bought along the route, so they won't appear double
            for (int i = 0; i < WayPointsList.Items.Count; i++)
                if (WayPointsList.SelectedIndices.Contains(i))
                    alreadyBought[i] = false;
                else
                    alreadyBought[i] = true;

            routeSpecs.Text = "";

            for (int s = 0; s < selectedRoute.length - selectedRoute.warps * 4 + 1; s++)
            {
                cs = hostApplication.games[hostApplication.currentGame].GetSectorObject(Convert.ToInt16(selectedRoute.sectors[s]));
                int newgalaxy = hostApplication.games[hostApplication.currentGame].GetGalaxyIndex(Convert.ToInt16(selectedRoute.sectors[s]));
                if (newgalaxy != currentGalaxy)
                {
                    if (resultText != "")
                        resultText += "\n\n";
                    resultText += hostApplication.games[hostApplication.currentGame].galaxy[newgalaxy].galaxy_name + " galaxy:\n";
                    currentGalaxy = newgalaxy;
                }
                else if (resultText.EndsWith("here\n"))
                {
                    resultText += "\n";
                }
                else
                    resultText += " - ";
                resultText += selectedRoute.sectors[s].ToString();

                for (int i = 0; i < WayPointsList.Items.Count; i++)
                {
                    if (alreadyBought[i])
                        continue;

                    for (int l = 0; l < cs.nroflocations; l++)
                    {
                        if (currentGame.location[cs.location[l]].Sells(currentGame.GetItemIndex(currentGame.location[cs.location[l]], WayPointsList.Items[i].ToString())))
                        {
                            itemsToBuy.Add(WayPointsList.Items[i]);
                            alreadyBought[i] = true;
                        }
                        if(currentGame.location[cs.location[l]].location_type == "Bank" && (WayPointsList.Items[i].ToString().Contains("Bank") || WayPointsList.Items[i].ToString().Contains("Mint")))
                        {
                            itemsToBuy.Add("Bank");
                            alreadyBought[i] = true;
                        }
                        if (currentGame.location[cs.location[l]].location_type == "Bar" && (WayPointsList.Items[i].ToString().Contains("Bar") || WayPointsList.Items[i].ToString().Contains("Club") || WayPointsList.Items[i].ToString().Contains("Saloon") || WayPointsList.Items[i].ToString().Contains("Chug")))
                        {
                            itemsToBuy.Add("Booze");
                            alreadyBought[i] = true;
                        }
                    } 
                }
                if(itemsToBuy.Count > 0)
                {
                    if(itemsToBuy.Contains("Bank"))
                        resultText += " Bank";
                    
                    if(itemsToBuy.Count > 1)
                        resultText += " and buy ";
                    else if(!resultText.EndsWith("Bank"))
                        resultText += " Buy ";
                }

                for (int i = 0; i < itemsToBuy.Count; i++)
                {
                    if ((string)itemsToBuy[i] != "Bank")
                        resultText += (string)itemsToBuy[i];
                    else
                        continue;

                    if (itemsToBuy.Count - i - 1 > 0)
                        if (itemsToBuy.Count - i - 1 > 1)
                            resultText += ", ";
                        else
                            resultText += " and ";
                }

                if (itemsToBuy.Count > 0)
                    resultText += " here" + "\n";
    
                itemsToBuy.Clear();
            }

            if (currentGame.smachanged)
                SMR16.SaveData(currentGame.address, currentGame);

            routeSpecs.Text = resultText;
		}

        private void Closey_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void ChooseLocation(object sender, System.EventArgs e)
        {
            //Display the target
            NewWayPointBox.Text = ((MenuItem)sender).Text;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (WayPointsList.SelectedItem != null)
                WayPointsList.Items.RemoveAt(WayPointsList.SelectedIndex);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (NewWayPointBox.Text != "")
                WayPointsList.Items.Add(NewWayPointBox.Text);

            WayPointsList.SetSelected(WayPointsList.Items.Count - 1, true);
        }

        private void ItemCheck_Changed(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            allowed[e.Index] = e.NewValue == CheckState.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            randomroute = !randomroute;

            if(!randomroute)
                checkBox3.Text = "Use only given route order";
            else
                checkBox3.Text = "Use any route order";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            evade = !evade;
            hostApplication.games[hostApplication.currentGame].ResetShortestRoutes();
        }

        private void StartSectorField_TextChanged(object sender, EventArgs e)
        {

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

            if (selectedRoute == null)
            {
                MessageBox.Show("No route has been selected yet");
                return;
            }

            for (int s = 0; s < selectedRoute.sectors.Count; s++)
                currentGame.GetSectorObject((int)selectedRoute.sectors[s]).highlighted = 30 + ((double)s) / ((double)selectedRoute.length) * 50;

            for (int w = 0; w < selectedRoute.waypoints.Count; w++)
                currentGame.GetSectorObject((int)selectedRoute.waypoints[w]).highlighted = 100;

            currentGame.highlightedRoute = selectedRoute;

            if (selectedRoute == null)
            {
                MessageBox.Show("No route has been selected yet");
                return;
            }

            int sectornr = (int)selectedRoute.sectors[0];
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
            hostApplication.toolBarButton6.Pushed = false;
        }

        private void routeSpecs_TextChanged(object sender, EventArgs e)
        {

        }

        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= GalaxyList.Items.Count; i++)
            {
                GalaxyList.SetItemChecked(i - 1, true);
            }
        }

        private void DeselectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= GalaxyList.Items.Count; i++)
            {
                GalaxyList.SetItemChecked(i - 1, false);
            }
        }

        private void GalaxyList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
	}
}
