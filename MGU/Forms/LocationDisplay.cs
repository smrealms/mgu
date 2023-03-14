/* Copyright 2009-2011 Robin Langerak
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
	public class LocationDisplay : System.Windows.Forms.Form
    {
        private MainStuff hostApplication;
        private Game currentGame;
        ArrayList sortedsectornumbers = new ArrayList(100);

        bool evade = false;

#region Form Elements
        private System.Windows.Forms.ListBox LocationList;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList Locs;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.Windows.Forms.ContextMenu TechMenu;
		private System.Windows.Forms.ContextMenu WeaponMenu;
		private System.Windows.Forms.ContextMenu ShipMenu;
		private System.Windows.Forms.ContextMenu OtherMenu;
        private System.Windows.Forms.ContextMenu GoodsMenu;
		private System.Windows.Forms.Label SectorLabel;
		private System.Windows.Forms.ToolBarButton toolBarButton0;
        //Main Menu Items
		private System.Windows.Forms.MenuItem[] Ship = new MenuItem[0], Bar = new MenuItem[0], Bank = new MenuItem[0], Gov = new MenuItem[0], Other = new MenuItem[0];
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
		private System.Windows.Forms.MenuItem RestrictedS;
		private System.Windows.Forms.MenuItem NeutralS;
		private System.Windows.Forms.MenuItem OtherS;
		private System.Windows.Forms.MenuItem Sep1;
		private System.Windows.Forms.MenuItem Sep2;
		private System.Windows.Forms.MenuItem BarMenu;
		private System.Windows.Forms.MenuItem BankMenu;
		private System.Windows.Forms.MenuItem GovernmentMenu;
		private System.Windows.Forms.MenuItem OtherW;
		private System.Windows.Forms.Label TargetLabel;
		private System.Windows.Forms.TextBox TargetBox;
		private System.Windows.Forms.TextBox SectorBox;
		private System.Windows.Forms.MenuItem Sep3;
		private System.Windows.Forms.MenuItem OtherM;
		private System.Windows.Forms.MenuItem Sep4;
        private GroupBox GalaxyList;
        private CheckedListBox checkedListBox1;
		private System.Windows.Forms.Button CloseButton;
        private CheckBox checkBox1;
        private ToolBarButton toolBarButton5;
        private ToolBarButton Goods;
        private Button PlotButton;
        bool[] allowed;
#endregion
		 
		public LocationDisplay(MainStuff host)
		{
            hostApplication = host;
            currentGame = host.games[host.currentGame];
			InitializeComponent();

            int nrofgalaxies = currentGame.nrofgalaxies;

            //Add the galaxies to the list of allowed galaxies. Do not add the first, Zero, galaxy
            for (int g = 1; g < nrofgalaxies; g++)
                checkedListBox1.Items.Add((object)currentGame.galaxy[g].galaxy_name);
            for (int x = 0; x < checkedListBox1.Items.Count; x += 1)
                checkedListBox1.SetItemChecked(x, true);
            allowed = new bool[nrofgalaxies];
            for (int x = 0; x < allowed.Length; x += 1)
                allowed[x] = true;
            checkedListBox1.ItemCheck += new ItemCheckEventHandler(ItemCheck_Changed);

			LocationsChanged();
		}

		public void LocationsChanged()
        //This function refills the menu of the location finder
		{
			AddGunsToMenu();
			AddTechToMenu();
			AddShipsToMenu();
			AddMiscToMenu();
            AddGoodsToMenu();
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
            for (int w = 0; w < currentGame.nrofweapons; w++)
            {
                MenuItem newWeap = new MenuItem(currentGame.weapon[w].name);
                newWeap.RadioCheck = true;
                newWeap.Click += new EventHandler(ChooseLocation);
                switch (currentGame.weapon[w].power)
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
            for (int w = 1; w < currentGame.nrofweapons; w++)
            {
                MenuItem newWeap = new MenuItem(currentGame.weapon[w].name);
                newWeap.RadioCheck = true;
                newWeap.Click += new EventHandler(ChooseLocation);

                switch (currentGame.weapon[w].weapon_race.race_name)
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
            for (int t = 0; t < currentGame.nroftechs; t++)
            {
                MenuItem newTech = new MenuItem(currentGame.technology[t].name);
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
            for (int s = 0; s < currentGame.nrofships; s++)
            {
                MenuItem newShip = new MenuItem(currentGame.ship[s].name);
                newShip.RadioCheck = true;
                newShip.Click += new EventHandler(ChooseLocation);

                switch (currentGame.ship[s].ship_race.race_name)
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

        public void AddGoodsToMenu()
        {
            MenuItem sellGood = new MenuItem("Selling:");
            MenuItem buyGood = new MenuItem("Buying:");

            GoodsMenu.MenuItems.Add(sellGood);
            GoodsMenu.MenuItems.Add(buyGood);

            for(int i = 1; i <= currentGame.nrofgoods; i++)
            {
                MenuItem newsellGood = new MenuItem(currentGame.good[i].good_name);
                newsellGood.RadioCheck = true;
                newsellGood.Click += new EventHandler(ChooseLocation);
                sellGood.MenuItems.Add(newsellGood);
                MenuItem newbuyGood = new MenuItem(currentGame.good[i].good_name);
                newbuyGood.RadioCheck = true;
                newbuyGood.Click += new EventHandler(ChooseLocation);
                buyGood.MenuItems.Add(newbuyGood);
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

            for (int l = 0; l < currentGame.nroflocations; l++)
            {
                MenuItem newLocation = new MenuItem(currentGame.location[l].location_name);
                newLocation.RadioCheck = true;
                newLocation.Click += new EventHandler(ChooseLocation);

                switch (currentGame.location[l].location_type)
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
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MGU.Forms.LocationDisplay));
            this.LocationList = new System.Windows.Forms.ListBox();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.TechMenu = new System.Windows.Forms.ContextMenu();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.WeaponMenu = new System.Windows.Forms.ContextMenu();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.ShipMenu = new System.Windows.Forms.ContextMenu();
            this.toolBarButton0 = new System.Windows.Forms.ToolBarButton();
            this.OtherMenu = new System.Windows.Forms.ContextMenu();
            this.Goods = new System.Windows.Forms.ToolBarButton();
            this.GoodsMenu = new System.Windows.Forms.ContextMenu();
            this.Locs = new System.Windows.Forms.ImageList(this.components);
            this.AnyGun = new System.Windows.Forms.MenuItem();
            this.Sep2 = new System.Windows.Forms.MenuItem();
            this.L1 = new System.Windows.Forms.MenuItem();
            this.L2 = new System.Windows.Forms.MenuItem();
            this.L3 = new System.Windows.Forms.MenuItem();
            this.L4 = new System.Windows.Forms.MenuItem();
            this.L5 = new System.Windows.Forms.MenuItem();
            this.Sep1 = new System.Windows.Forms.MenuItem();
            this.AlskantW = new System.Windows.Forms.MenuItem();
            this.CreontiW = new System.Windows.Forms.MenuItem();
            this.HumanW = new System.Windows.Forms.MenuItem();
            this.IkThorneW = new System.Windows.Forms.MenuItem();
            this.SalveneW = new System.Windows.Forms.MenuItem();
            this.ThevianW = new System.Windows.Forms.MenuItem();
            this.WQHumanW = new System.Windows.Forms.MenuItem();
            this.NijarinW = new System.Windows.Forms.MenuItem();
            this.NeutralW = new System.Windows.Forms.MenuItem();
            this.OtherW = new System.Windows.Forms.MenuItem();
            this.AlskantS = new System.Windows.Forms.MenuItem();
            this.CreontiS = new System.Windows.Forms.MenuItem();
            this.HumanS = new System.Windows.Forms.MenuItem();
            this.IkThorneS = new System.Windows.Forms.MenuItem();
            this.SalveneS = new System.Windows.Forms.MenuItem();
            this.ThevianS = new System.Windows.Forms.MenuItem();
            this.WQHumanS = new System.Windows.Forms.MenuItem();
            this.NijarinS = new System.Windows.Forms.MenuItem();
            this.RestrictedS = new System.Windows.Forms.MenuItem();
            this.NeutralS = new System.Windows.Forms.MenuItem();
            this.OtherS = new System.Windows.Forms.MenuItem();
            this.BarMenu = new System.Windows.Forms.MenuItem();
            this.BankMenu = new System.Windows.Forms.MenuItem();
            this.GovernmentMenu = new System.Windows.Forms.MenuItem();
            this.Sep3 = new System.Windows.Forms.MenuItem();
            this.OtherM = new System.Windows.Forms.MenuItem();
            this.Sep4 = new System.Windows.Forms.MenuItem();
            this.SectorLabel = new System.Windows.Forms.Label();
            this.TargetBox = new System.Windows.Forms.TextBox();
            this.TargetLabel = new System.Windows.Forms.Label();
            this.SectorBox = new System.Windows.Forms.TextBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.GalaxyList = new System.Windows.Forms.GroupBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.PlotButton = new System.Windows.Forms.Button();
            this.GalaxyList.SuspendLayout();
            this.SuspendLayout();
            // 
            // LocationList
            // 
            this.LocationList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LocationList.Location = new System.Drawing.Point(0, 52);
            this.LocationList.Name = "LocationList";
            this.LocationList.Size = new System.Drawing.Size(345, 290);
            this.LocationList.TabIndex = 12;
            this.LocationList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.LocationList_DrawItem);
            this.LocationList.SelectedIndexChanged += new System.EventHandler(this.LocationList_SelectedIndexChanged);
            // 
            // toolBar1
            // 
            this.toolBar1.AutoSize = false;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton1,
            this.toolBarButton2,
            this.toolBarButton3,
            this.toolBarButton0,
            this.Goods});
            this.toolBar1.Divider = false;
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.Locs;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(527, 25);
            this.toolBar1.TabIndex = 7;
            this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.toolBar1.Wrappable = false;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.DropDownMenu = this.TechMenu;
            this.toolBarButton1.ImageIndex = 6;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton1.Text = "Technology";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.DropDownMenu = this.WeaponMenu;
            this.toolBarButton2.ImageIndex = 7;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton2.Text = "Weapons";
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.DropDownMenu = this.ShipMenu;
            this.toolBarButton3.ImageIndex = 8;
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton3.Text = "Ships";
            // 
            // toolBarButton0
            // 
            this.toolBarButton0.DropDownMenu = this.OtherMenu;
            this.toolBarButton0.ImageIndex = 5;
            this.toolBarButton0.Name = "toolBarButton0";
            this.toolBarButton0.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton0.Text = "Other";
            // 
            // Goods
            // 
            this.Goods.DropDownMenu = this.GoodsMenu;
            this.Goods.ImageIndex = 10;
            this.Goods.Name = "Goods";
            this.Goods.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.Goods.Tag = "5";
            this.Goods.Text = "Goods";
            // 
            // GoodsMenu
            // 
            this.GoodsMenu.Popup += new System.EventHandler(this.GoodsMenu_Popup);
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
            this.Locs.Images.SetKeyName(10, "10 luxury items.gif");
            // 
            // AnyGun
            // 
            this.AnyGun.Index = -1;
            this.AnyGun.Text = "";
            // 
            // Sep2
            // 
            this.Sep2.Index = -1;
            this.Sep2.Text = "";
            // 
            // L1
            // 
            this.L1.Index = -1;
            this.L1.Text = "";
            // 
            // L2
            // 
            this.L2.Index = -1;
            this.L2.Text = "";
            // 
            // L3
            // 
            this.L3.Index = -1;
            this.L3.Text = "";
            // 
            // L4
            // 
            this.L4.Index = -1;
            this.L4.Text = "";
            // 
            // L5
            // 
            this.L5.Index = -1;
            this.L5.Text = "";
            // 
            // Sep1
            // 
            this.Sep1.Index = -1;
            this.Sep1.Text = "";
            // 
            // AlskantW
            // 
            this.AlskantW.Index = -1;
            this.AlskantW.Text = "";
            // 
            // CreontiW
            // 
            this.CreontiW.Index = -1;
            this.CreontiW.Text = "";
            // 
            // HumanW
            // 
            this.HumanW.Index = -1;
            this.HumanW.Text = "";
            // 
            // IkThorneW
            // 
            this.IkThorneW.Index = -1;
            this.IkThorneW.Text = "";
            // 
            // SalveneW
            // 
            this.SalveneW.Index = -1;
            this.SalveneW.Text = "";
            // 
            // ThevianW
            // 
            this.ThevianW.Index = -1;
            this.ThevianW.Text = "";
            // 
            // WQHumanW
            // 
            this.WQHumanW.Index = -1;
            this.WQHumanW.Text = "";
            // 
            // NijarinW
            // 
            this.NijarinW.Index = -1;
            this.NijarinW.Text = "";
            // 
            // NeutralW
            // 
            this.NeutralW.Index = -1;
            this.NeutralW.Text = "";
            // 
            // OtherW
            // 
            this.OtherW.Index = -1;
            this.OtherW.Text = "";
            // 
            // AlskantS
            // 
            this.AlskantS.Index = -1;
            this.AlskantS.Text = "";
            // 
            // CreontiS
            // 
            this.CreontiS.Index = -1;
            this.CreontiS.Text = "";
            // 
            // HumanS
            // 
            this.HumanS.Index = -1;
            this.HumanS.Text = "";
            // 
            // IkThorneS
            // 
            this.IkThorneS.Index = -1;
            this.IkThorneS.Text = "";
            // 
            // SalveneS
            // 
            this.SalveneS.Index = -1;
            this.SalveneS.Text = "";
            // 
            // ThevianS
            // 
            this.ThevianS.Index = -1;
            this.ThevianS.Text = "";
            // 
            // WQHumanS
            // 
            this.WQHumanS.Index = -1;
            this.WQHumanS.Text = "";
            // 
            // NijarinS
            // 
            this.NijarinS.Index = -1;
            this.NijarinS.Text = "";
            // 
            // RestrictedS
            // 
            this.RestrictedS.Index = -1;
            this.RestrictedS.Text = "";
            // 
            // NeutralS
            // 
            this.NeutralS.Index = -1;
            this.NeutralS.Text = "";
            // 
            // OtherS
            // 
            this.OtherS.Index = -1;
            this.OtherS.Text = "";
            // 
            // BarMenu
            // 
            this.BarMenu.Index = -1;
            this.BarMenu.Text = "";
            // 
            // BankMenu
            // 
            this.BankMenu.Index = -1;
            this.BankMenu.Text = "";
            // 
            // GovernmentMenu
            // 
            this.GovernmentMenu.Index = -1;
            this.GovernmentMenu.Text = "";
            // 
            // Sep3
            // 
            this.Sep3.Index = -1;
            this.Sep3.Text = "";
            // 
            // OtherM
            // 
            this.OtherM.Index = -1;
            this.OtherM.Text = "";
            // 
            // Sep4
            // 
            this.Sep4.Index = -1;
            this.Sep4.Text = "";
            // 
            // SectorLabel
            // 
            this.SectorLabel.Location = new System.Drawing.Point(4, 28);
            this.SectorLabel.Name = "SectorLabel";
            this.SectorLabel.Size = new System.Drawing.Size(64, 20);
            this.SectorLabel.TabIndex = 8;
            this.SectorLabel.Text = "My Sector:";
            this.SectorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TargetBox
            // 
            this.TargetBox.Location = new System.Drawing.Point(207, 28);
            this.TargetBox.Name = "TargetBox";
            this.TargetBox.ReadOnly = true;
            this.TargetBox.Size = new System.Drawing.Size(138, 20);
            this.TargetBox.TabIndex = 10;
            this.TargetBox.TextChanged += new System.EventHandler(this.TargetBox_TextChanged);
            // 
            // TargetLabel
            // 
            this.TargetLabel.Location = new System.Drawing.Point(155, 28);
            this.TargetLabel.Name = "TargetLabel";
            this.TargetLabel.Size = new System.Drawing.Size(48, 20);
            this.TargetLabel.TabIndex = 11;
            this.TargetLabel.Text = "Target:";
            this.TargetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SectorBox
            // 
            this.SectorBox.Location = new System.Drawing.Point(65, 28);
            this.SectorBox.MaxLength = 6;
            this.SectorBox.Name = "SectorBox";
            this.SectorBox.Size = new System.Drawing.Size(84, 20);
            this.SectorBox.TabIndex = 13;
            this.SectorBox.TextChanged += new System.EventHandler(this.SectorBox_TextChanged);
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(237, 348);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(275, 24);
            this.CloseButton.TabIndex = 14;
            this.CloseButton.Text = "Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // GalaxyList
            // 
            this.GalaxyList.Controls.Add(this.checkedListBox1);
            this.GalaxyList.ForeColor = System.Drawing.SystemColors.ControlText;
            this.GalaxyList.Location = new System.Drawing.Point(356, 52);
            this.GalaxyList.Name = "GalaxyList";
            this.GalaxyList.Size = new System.Drawing.Size(164, 290);
            this.GalaxyList.TabIndex = 21;
            this.GalaxyList.TabStop = false;
            this.GalaxyList.Text = "Allowed gals";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Location = new System.Drawing.Point(8, 31);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.Size = new System.Drawing.Size(148, 259);
            this.checkedListBox1.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(356, 26);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(164, 20);
            this.checkBox1.TabIndex = 29;
            this.checkBox1.Text = "Evade hostile (red) sectors";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // PlotButton
            // 
            this.PlotButton.Location = new System.Drawing.Point(12, 348);
            this.PlotButton.Name = "PlotButton";
            this.PlotButton.Size = new System.Drawing.Size(219, 23);
            this.PlotButton.TabIndex = 30;
            this.PlotButton.Text = "Plot to Location";
            this.PlotButton.UseVisualStyleBackColor = true;
            this.PlotButton.Click += new System.EventHandler(this.PlotButton_Click);
            // 
            // LocationDisplay
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(527, 377);
            this.Controls.Add(this.PlotButton);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.GalaxyList);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.SectorBox);
            this.Controls.Add(this.TargetLabel);
            this.Controls.Add(this.TargetBox);
            this.Controls.Add(this.SectorLabel);
            this.Controls.Add(this.toolBar1);
            this.Controls.Add(this.LocationList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LocationDisplay";
            this.Text = "Location Displayer";
            this.GalaxyList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private void LocationList_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            if (this.LocationList.Items.Count == 0)
                return;

            e.DrawBackground();
            Brush myBrush = Brushes.Black;

            string test = e.ToString();
            if(this.LocationList.Items[e.Index].ToString().Contains("UNREACHABLE"))
                myBrush = Brushes.Red;

            e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, myBrush,e.Bounds,StringFormat.GenericDefault);
            e.DrawFocusRectangle();

            //Set 'home' galaxy to allowed
            int targetsector;
            if (int.TryParse(SectorBox.Text, out targetsector))
                checkedListBox1.SetItemChecked(currentGame.GetSectorObject(targetsector).galaxy.GetGalaxyID() - 1, true);
        }

		#endregion

        private void ItemCheck_Changed(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            int targetsector;
            if (int.TryParse(SectorBox.Text, out targetsector) )
                if (e.CurrentValue == CheckState.Checked && e.Index == currentGame.GetSectorObject(targetsector).galaxy.GetGalaxyID() - 1)
                {
                    MessageBox.Show("Cannot deselect the galaxy that your target sector is in!");
                }
            else
                allowed[e.Index] = e.NewValue == CheckState.Checked;
            FindSectors();
        }

        private void ChooseLocation(object sender, System.EventArgs e)
        {
            TargetBox.Text = "";

            if (((MenuItem)sender).Text == "Food" || ((MenuItem)sender).Text == "Wood" || ((MenuItem)sender).Text == "Ore" || ((MenuItem)sender).Text == "Precious Metals"
                || ((MenuItem)sender).Text == "Textiles" || ((MenuItem)sender).Text == "Slaves" || ((MenuItem)sender).Text == "Machinery"
                || ((MenuItem)sender).Text == "Circuitry" || ((MenuItem)sender).Text == "Weapons" || ((MenuItem)sender).Text == "Computer"
                || ((MenuItem)sender).Text == "Luxury Items" || ((MenuItem)sender).Text == "Narcotics")
            {
                if (((MenuItem)((MenuItem)sender).Parent).Text == "Selling:")
                    TargetBox.Text = "Ports selling ";
                if (((MenuItem)((MenuItem)sender).Parent).Text == "Buying:")
                    TargetBox.Text = "Ports buying ";
            }

            //Display the target
            TargetBox.Text += ((MenuItem)sender).Text;

            FindSectors();
        }

        private void FindSectors()
        {
            int targetsector;

            //Generate a list of locations being searched
            ArrayList locations = new ArrayList(100);
            ArrayList sectornumbers = new ArrayList(100);
            int distancelist;
            string locationtype;
            
            if (SectorBox.Text == "")
                return;

            sortedsectornumbers.Clear();

            for (int g = 0; g < currentGame.nrofgalaxies; g++)
                for (int x = 0; x < currentGame.galaxy[g].galaxy_xsize; x++)
                    for (int y = 0; y < currentGame.galaxy[g].galaxy_ysize; y++)
                    {
                        if (currentGame.galaxy[g].sector[x, y].port != null)
                        {
                            for (int i = 1; i <= currentGame.nrofgoods; i++)
                            {
                                if ("Ports selling " + currentGame.good[i].good_name == TargetBox.Text && currentGame.galaxy[g].sector[x, y].port.port_goods[i] == -1)
                                {
                                    sectornumbers.Add(currentGame.galaxy[g].sector[x, y].sector_id);
                                    locations.Add(null);
                                }
                                if ("Ports buying " + currentGame.good[i].good_name == TargetBox.Text && currentGame.galaxy[g].sector[x, y].port.port_goods[i] == 1)
                                {
                                    sectornumbers.Add(currentGame.galaxy[g].sector[x, y].sector_id);
                                    locations.Add(null);
                                }
                            }
                        }

                        for (int l = 0; l < currentGame.galaxy[g].sector[x, y].nroflocations; l++)
                        {
                            locationtype = currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]].location_type;
                            for (int s = 0; s < currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]].nrofsolditems; s++)
                            {
                                if (locationtype == "Weapon Shop")
                                    if (currentGame.weapon[currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]].sells[s]].name == TargetBox.Text)
                                    {
                                        locations.Add(currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]]);
                                        sectornumbers.Add(currentGame.galaxy[g].sector[x, y].sector_id);
                                    }
                                if (locationtype == "Ship Shop")
                                    if (currentGame.ship[currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]].sells[s]].name == TargetBox.Text)
                                    {
                                        locations.Add(currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]]);
                                        sectornumbers.Add(currentGame.galaxy[g].sector[x, y].sector_id);
                                    }
                                if (locationtype == "Technology Shop")
                                    if (currentGame.technology[currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]].sells[s]].name == TargetBox.Text)
                                    {
                                        locations.Add(currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]]);
                                        sectornumbers.Add(currentGame.galaxy[g].sector[x, y].sector_id);
                                    }
                            }
                            if (locationtype == "Bar" || locationtype == "Bank" || locationtype == "HQ" || locationtype == "UG" || locationtype == "Fed")
                                if (currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]].location_name == TargetBox.Text)
                                {
                                    locations.Add(currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]]);
                                    sectornumbers.Add(currentGame.galaxy[g].sector[x, y].sector_id);
                                }
                            if (TargetBox.Text == "Federal Beacon" && locationtype == "Fed")
                            {
                                locations.Add(currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]]);
                                sectornumbers.Add(currentGame.galaxy[g].sector[x, y].sector_id);
                            }
                            if (TargetBox.Text == "Any Bar" && locationtype == "Bar")
                            {
                                locations.Add(currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]]);
                                sectornumbers.Add(currentGame.galaxy[g].sector[x, y].sector_id);
                            }
                            if (TargetBox.Text == "Any Bank" && locationtype == "Bank")
                            {
                                locations.Add(currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]]);
                                sectornumbers.Add(currentGame.galaxy[g].sector[x, y].sector_id);
                            }
                            if (TargetBox.Text == "Any Gun" && locationtype == "Weapon Shop")
                            {
                                locations.Add(currentGame.location[currentGame.galaxy[g].sector[x, y].location[l]]);
                                sectornumbers.Add(currentGame.galaxy[g].sector[x, y].sector_id);
                            }
                        }
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

            //Compute all distances
            if(int.TryParse(SectorBox.Text, out targetsector))
                if(targetsector > 0 && targetsector < currentGame.nrofsectors)  
                    currentGame.GetSectorObject(targetsector).RecursiveDistance(0, allowed, evade);

            //If no locations have been found, then display message and return
            if (sectornumbers.Count == 0)
            {
                LocationList.Items.Clear();
                LocationList.Items.Add("No locations found");
                return;
            }

            //First insert a location as reference
            LocationList.Items.Clear();
            if (allowed[currentGame.GetSectorObject((int)sectornumbers[0]).galaxy.GetGalaxyID() - 1] && currentGame.GetSectorObject((int)sectornumbers[0]).distance != 100000 && currentGame.GetSectorObject((int)sectornumbers[0]).distance > 0)
            {
                if ((Location)locations[0] != null)
                    LocationList.Items.Add(currentGame.GetSectorObject((int)sectornumbers[0]).distance.ToString() + " Turns - #" + ((int)sectornumbers[0]).ToString() + " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[0])].galaxy_name + ") " + ((Location)locations[0]).location_name);
                else
                    LocationList.Items.Add(currentGame.GetSectorObject((int)sectornumbers[0]).distance.ToString() + " Turns - #" + ((int)sectornumbers[0]).ToString() + " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[0])].galaxy_name + ") ");
            }
            else
            {
                if ((Location)locations[0] != null)
                    LocationList.Items.Add("UNREACHABLE - #" + ((int)sectornumbers[0]).ToString() +  " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[0])].galaxy_name + ") " + ((Location)locations[0]).location_name);
                else
                    LocationList.Items.Add("UNREACHABLE - #" + ((int)sectornumbers[0]).ToString() +  " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[0])].galaxy_name + ") ");
            }
            sortedsectornumbers.Add((int)sectornumbers[0]);

            Sector targetSector, itemSector;
            //For each location in the list, insert it ordered by distance
            for (int target = 1; target < locations.Count; target++)
            {
                targetSector = currentGame.GetSectorObject((int)sectornumbers[target]);
                if (targetSector.distance == -1 || !allowed[targetSector.galaxy.GetGalaxyID() - 1])
                    targetSector.distance = 100000;
                for (int item = 0; item < LocationList.Items.Count; item++)
                {
                    itemSector = currentGame.GetSectorObject((int)sortedsectornumbers[item]);
                    if (targetSector.distance < itemSector.distance)
                    {
                        if (allowed[targetSector.galaxy.GetGalaxyID() - 1] && targetSector.distance != 100000)
                            if ((Location)locations[0] != null)
                                LocationList.Items.Insert(item, targetSector.distance.ToString() + " Turns - #" + ((int)sectornumbers[target]).ToString() + " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[target])].galaxy_name + ") " + ((Location)locations[target]).location_name);
                            else
                                LocationList.Items.Insert(item, targetSector.distance.ToString() + " Turns - #" + ((int)sectornumbers[target]).ToString() + " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[target])].galaxy_name + ") ");
                        else
                        {
                            if ((Location)locations[0] != null)
                                LocationList.Items.Insert(item, "UNREACHABLE - #" + ((int)sectornumbers[target]).ToString() + " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[target])].galaxy_name + ") " + ((Location)locations[target]).location_name);
                            else
                                LocationList.Items.Insert(item, "UNREACHABLE - #" + ((int)sectornumbers[target]).ToString() + " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[target])].galaxy_name + ") ");
                        }
                        sortedsectornumbers.Insert(item, (int)sectornumbers[target]);
                        break;
                    }
                    if(item == LocationList.Items.Count-1)
                    {
                        if (allowed[targetSector.galaxy.GetGalaxyID()-1] && targetSector.distance != 100000)
                            if ((Location)locations[0] != null)
                                LocationList.Items.Insert(item+1, targetSector.distance.ToString() + " Turns - #" + ((int)sectornumbers[target]).ToString() + " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[target])].galaxy_name + ") " + ((Location)locations[target]).location_name);
                            else
                                LocationList.Items.Insert(item + 1, targetSector.distance.ToString() + " Turns - #" + ((int)sectornumbers[target]).ToString() + " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[target])].galaxy_name + ") ");
                        else
                        {
                            if ((Location)locations[0] != null)
                                LocationList.Items.Insert(item + 1, "UNREACHABLE - #" + ((int)sectornumbers[target]).ToString() + " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[target])].galaxy_name + ") " + ((Location)locations[target]).location_name);
                            else
                                LocationList.Items.Insert(item + 1, "UNREACHABLE - #" + ((int)sectornumbers[target]).ToString() + " (" + currentGame.galaxy[currentGame.GetGalaxyIndex((int)sectornumbers[target])].galaxy_name + ") ");
                        }
                        sortedsectornumbers.Insert(item + 1, (int)sectornumbers[target]);
                        break;
                    }
                }
            }
        }

		private void TargetBox_TextChanged(object sender, System.EventArgs e)
		{
            LocationList.Items.Clear();
            if(SectorBox.Text != "")
    			FindSectors();
		}

		private void SectorBox_TextChanged(object sender, System.EventArgs e)
		{
            int dummy = 0;

            if(!intparse(SectorBox.Text, ref dummy))
                MessageBox.Show("Sector number could not be read");
            else
            {
                LocationList.Items.Clear();
                if(TargetBox.Text != "")
                    FindSectors();
            }
		}

		private void CloseButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            evade = !evade;
        }

        private static bool intparse(string toparse, ref int result)
        {
            try { result = int.Parse(toparse); return true; }
            catch (System.FormatException) { return false; }
        }

        private void PrintShipsButton_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(@"C:\Test.txt", FileMode.Create, FileAccess.Write, FileShare.Write);

            StreamWriter writer = new StreamWriter(fs);
            
            for(int i = 0; i < 76; i++)
            {
                writer.WriteLine("ship[" + i.ToString() + "].name = \"" + currentGame.ship[i].name + "\";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_race = GetRaceObject(" + currentGame.ship[i].ship_race.GetRaceNameAsId(currentGame) + ");");
                writer.WriteLine("ship[" + i.ToString() + "].ship_cost = " + currentGame.ship[i].ship_cost.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_speed = " + currentGame.ship[i].ship_speed.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_hardpoints = " + currentGame.ship[i].ship_hardpoints.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_power = " + currentGame.ship[i].ship_power.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_shields = " + currentGame.ship[i].ship_shields.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_armor = " + currentGame.ship[i].ship_armor.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_cargo = " + currentGame.ship[i].ship_cargo.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_CloakAble = " + currentGame.ship[i].ship_CloakAble.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_DSAble = " + currentGame.ship[i].ship_DSAble.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_IGAble = " + currentGame.ship[i].ship_IGAble.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_JumpAble = " + currentGame.ship[i].ship_JumpAble.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_ScanAble = " + currentGame.ship[i].ship_ScanAble.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_scout_drones = " + currentGame.ship[i].ship_scout_drones.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_mines =  " + currentGame.ship[i].ship_mines.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_combat_drones =  " + currentGame.ship[i].ship_combat_drones.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_level_needed =  " + currentGame.ship[i].ship_level_needed.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].ship_restrictions =  " + currentGame.ship[i].ship_restrictions.ToString() + ";");
                writer.WriteLine("ship[" + i.ToString() + "].sold_at = \"\"" + ";");
                writer.WriteLine("");
            }

            fs.Close();
        }

        private void PrintWeaponsButton_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(@"C:\Test.txt", FileMode.Create, FileAccess.Write, FileShare.Write);

            StreamWriter writer = new StreamWriter(fs);

            for (int i = 1; i < 59; i++)
            {
                writer.WriteLine("weapon[" + i.ToString() + "].accuracy = " + currentGame.weapon[i].accuracy.ToString() + ";");
                writer.WriteLine("weapon[" + i.ToString() + "].alignment = " + currentGame.weapon[i].alignment.ToString() + ";");
                writer.WriteLine("weapon[" + i.ToString() + "].armor_damage = " + currentGame.weapon[i].armor_damage.ToString() + ";");
                writer.WriteLine("weapon[" + i.ToString() + "].cost = " + currentGame.weapon[i].cost.ToString() + ";");
                writer.WriteLine("weapon[" + i.ToString() + "].name = \"" + currentGame.weapon[i].name + "\";");
                writer.WriteLine("weapon[" + i.ToString() + "].power = " + currentGame.weapon[i].power.ToString() + ";");
                writer.WriteLine("weapon[" + i.ToString() + "].shield_damage = " + currentGame.weapon[i].shield_damage.ToString() + ";");
                writer.WriteLine("weapon[" + i.ToString() + "].race = GetRaceObject(" + currentGame.weapon[i].weapon_race.GetRaceNameAsId(currentGame).ToString() + ");");
                writer.WriteLine("weapon[" + i.ToString() + "].sold_at = \"\"" + ";");
                writer.WriteLine("");
            }

            fs.Close();
        }

        private void PrintLocationsButton_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(@"C:\Test.txt", FileMode.Create, FileAccess.Write, FileShare.Write);

            StreamWriter writer = new StreamWriter(fs);

            for (int i = 85; i < 89; i++)
            {
                writer.WriteLine("location[" + i.ToString() + "].location_image = \"" + currentGame.location[i].location_image + "\";");
                writer.WriteLine("location[" + i.ToString() + "].location_name = \"" + currentGame.location[i].location_name + "\";");
                writer.WriteLine("location[" + i.ToString() + "].location_type = \"" + currentGame.location[i].location_type + "\";");
                writer.WriteLine("location[" + i.ToString() + "].nrofsolditems = " + currentGame.location[i].nrofsolditems.ToString() + ";");
                if (currentGame.location[i].nrofsolditems > 0)
                    writer.WriteLine("location[" + i.ToString() + "].sells = new int[" + currentGame.location[i].nrofsolditems.ToString() + "];");             
                for(int l =  0; l < currentGame.location[i].nrofsolditems; l++)
                    writer.WriteLine("location[" + i.ToString() + "].sells[" + l.ToString() + "] = " + currentGame.location[i].sells[l].ToString() + ";");
                writer.WriteLine("");
            }

            fs.Close();
        }

        private void LocationList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PlotButton_Click(object sender, EventArgs e)
        {
            if (hostApplication.Plotter == null)
            {
                hostApplication.Plotter = new PlotWindow(hostApplication);
                hostApplication.Plotter.Disposed += new EventHandler(hostApplication.PlotDispose);
                hostApplication.Plotter.Show();
                hostApplication.toolBarButton5.Pushed = true;
            }
            else hostApplication.Plotter.Activate();

            if (LocationList.SelectedIndex < 0)
                return;

            if(sortedsectornumbers[LocationList.SelectedIndex] != null)
                hostApplication.Plotter.To.Text = sortedsectornumbers[LocationList.SelectedIndex].ToString();
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {

        }

        private void GoodsMenu_Popup(object sender, EventArgs e)
        {

        }
	}
}
