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

#region Assembly Information
using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Reflection;

[assembly: AssemblyTitle("MGU " + MGU.MainStuff.version)]
[assembly: AssemblyDescription("This program is created to help out the lazy SMRealms player")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Merchants Guide to the Universe")]
[assembly: AssemblyCopyright("Copyright Robin Langerak, 2009 - 2010")]
[assembly: AssemblyVersion(MGU.MainStuff.AssemblyVersion)]
#endregion

namespace MGU
{
	public class MainStuff : System.Windows.Forms.Form
	{
        //This is the main class of the program, which contains all elements of the graphical user interface.
        //Most changes to the functionality will take place in other classes

        //This variable contains the index of the currently loaded game
        public int currentGame;

        //This variable contains a list of all games
        public Game[] games = new Game[5];
        public int nrofgames = 0;

        //This variable contains the string representation of the address of the current program.
        public string currentAddress;

        //This variable stores the display style of the sectors (MGU, in-game or SMC)
        public int displaystyle = 0;

        //This variable indicates if a map has been loaded
        private bool loaded = false;

        #region Version Info
		public const string version = "v1.6.1.8";
        private ToolBarButton Sep9;
        private ToolBarButton toolBarButton10;
        public ToolBarButton toolBarButton0;
        private ToolBarButton Sep0;
        private ContextMenu contextMenu0;
		public const string AssemblyVersion = "1.6.1.8";
        #endregion

		[STAThread]
		static void Main(string[] args)
		{
            Application.Run(new MainStuff());
		}

        public MainStuff()
        {
            string nextline;
            string[] lineparts;

            //First initialize all GUI components
            InitializeComponent();

            for(int x =0; x < 5; x++)
                games[x] = new Game(this, Cons, Locs);

            //Load the startup file if it exists
            if (File.Exists(Directory.GetCurrentDirectory() + @"\startup.txt"))
            {
                FileStream fs = new FileStream(Directory.GetCurrentDirectory() + @"\startup.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader reader = new StreamReader(fs);
                nextline = reader.ReadLine();
                char[] delim = { '=' };
                lineparts = nextline.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                while (lineparts[0].StartsWith("LoadedMap") && lineparts[1] != " ")
                {
                    if (SMR16.LoadData(lineparts[1], games[nrofgames]))
                    {
                        currentGame = nrofgames;
                        games[currentGame].address = lineparts[1];
                        nrofgames++;

                        MenuItem GameItem = new System.Windows.Forms.MenuItem();
                        GameItem.Index = nrofgames - 1;
                        GameItem.Text = games[currentGame].game_name;
                        GameItem.RadioCheck = true;
                        GameItem.Click += new System.EventHandler(this.ChangeGame);
                        contextMenu0.MenuItems.Add(GameItem);

                        toolBarButton0.Text = games[currentGame].game_name;

                        if (nrofgames >= 2)
                            for (int g = 0; g < games[currentGame - 1].nrofgalaxies; g++)
                                for (int x = 0; x < games[currentGame - 1].galaxy[g].galaxy_xsize; x++)
                                    for (int y = 0; y < games[currentGame - 1].galaxy[g].galaxy_ysize; y++)
                                    {
                                        games[currentGame - 1].galaxy[g].sector[x, y].Parent = null;
                                        games[currentGame - 1].galaxy[g].sector[x, y].Visible = false;
                                    }

                        //This is where we set panel1 as the parent control of all sector objects, so that the OnPaint message of panle1 gets passed on to all sectors.
                        for (int g = 0; g < games[currentGame].nrofgalaxies; g++)
                            for (int x = 0; x < games[currentGame].galaxy[g].galaxy_xsize; x++)
                                for (int y = 0; y < games[currentGame].galaxy[g].galaxy_ysize; y++)
                                    games[currentGame].galaxy[g].sector[x, y].Parent = panel1;

                        //Add the galaxies to the menu
                        AddGalaxies();
                        Redraw();

                        goLeftButton.Enabled = goRightButton.Enabled = goDownButton.Enabled = goUpButton.Enabled = toolBarButton1.Enabled = toolBarButton2.Enabled = toolBarButton3.Enabled = toolBarButton5.Enabled = this.toolBarButton6.Enabled = this.toolBarButton7.Enabled = this.toolBarButton8.Enabled = this.toolBarButton1.Enabled = true;
                    }
                    nextline = reader.ReadLine();
                    lineparts = nextline.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                }

                if (lineparts[0].StartsWith("Style"))
                    this.displaystyle = Convert.ToInt16(lineparts[1]);
                
                nextline = reader.ReadLine();
                if (reader.EndOfStream)
                    return;

                reader.Close();
                fs.Close();
            }
            else
            {
                //Set all buttons to disable, because no map has been loaded yet
                toolBarButton0.Enabled = false;
                toolBarButton1.Enabled = false;
                toolBarButton2.Enabled = false;
                toolBarButton3.Enabled = false;
                toolBarButton5.Enabled = false;
                toolBarButton6.Enabled = false;
                toolBarButton7.Enabled = false;
                toolBarButton8.Enabled = false;
                toolBarButton9.Enabled = false;
                SaveUniverse.Enabled = CloseUniverse.Enabled = false;
            }
        }

        #region Windows Form Designer generated code
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button goToButton;
        private System.Windows.Forms.RichTextBox goToField;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.ToolBar toolBar1;
        public System.Windows.Forms.ToolBarButton toolBarButton1;
        public System.Windows.Forms.ToolBarButton toolBarButton2;
        public System.Windows.Forms.ToolBarButton toolBarButton3;
        public System.Windows.Forms.ToolBarButton toolBarButton5;
        public System.Windows.Forms.ToolBarButton toolBarButton6;
        public System.Windows.Forms.ToolBarButton toolBarButton7;
        public System.Windows.Forms.ToolBarButton toolBarButton8;
        public System.Windows.Forms.ToolBarButton toolBarButton9;
		public System.Windows.Forms.Button goLeftButton;
        public System.Windows.Forms.Button goRightButton;
        public System.Windows.Forms.Button goDownButton;
        public System.Windows.Forms.Button goUpButton;
        public System.Windows.Forms.MenuItem menuItem1;
        public System.Windows.Forms.MenuItem mgu;
        public System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem game;
		private System.Windows.Forms.MenuItem smc;
		private System.Windows.Forms.MenuItem zoom75;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem zoom100;
		private System.Windows.Forms.MenuItem readMeLink;
		private System.Windows.Forms.MenuItem CreateUniverse;
		private System.Windows.Forms.MenuItem OpenUniverse;
        private System.Windows.Forms.MenuItem CloseUniverse;
        private System.Windows.Forms.MenuItem Quit;
		public System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.ContextMenu contextMenu2;
        private System.Windows.Forms.ImageList Locs;
        public ImageList Goods;
		private System.Windows.Forms.ImageList Cons;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBarButton Sep4;
		private System.Windows.Forms.ToolBarButton Sep1;
		private System.Windows.Forms.ToolBarButton Sep2;
		private System.Windows.Forms.ToolBarButton Sep3;
		private System.Windows.Forms.ToolBarButton Sep5;
		private System.Windows.Forms.ToolBarButton Sep6;
		private System.Windows.Forms.ToolBarButton Sep7;
		private System.Windows.Forms.MenuItem SaveUniverse;
		private System.Windows.Forms.MenuItem gameLink;
        private System.Windows.Forms.MenuItem aboutLink;
		private System.Windows.Forms.ImageList SMCGoods;
		private System.Windows.Forms.MenuItem zoom50;
        private System.Windows.Forms.ImageList SMRFocus;
        private System.Windows.Forms.MenuItem menuItem37;
		private System.Windows.Forms.ToolBarButton Sep8;
		private System.Windows.Forms.ToolBarButton TradeCalculaterButton;
		private System.Windows.Forms.MenuItem[] GalaxyItems;

		protected override void Dispose( bool disposing )
		{
            FileStream fs;
            string address = Directory.GetCurrentDirectory() + @"\startup.txt";

            try
            {
                fs = new FileStream(address, FileMode.Create, FileAccess.Write, FileShare.Read);
            }
            catch (IOException)
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                base.Dispose(disposing);
                return;
            }

            StreamWriter writer = new StreamWriter(fs);

            for(int g = 1; g < nrofgames+1; g++)
                writer.WriteLine("LoadedMap = " + games[g].address);
            writer.WriteLine("Style = " + displaystyle.ToString());

            writer.Close();
            fs.Close();

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainStuff));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.CreateUniverse = new System.Windows.Forms.MenuItem();
            this.OpenUniverse = new System.Windows.Forms.MenuItem();
            this.SaveUniverse = new System.Windows.Forms.MenuItem();
            this.CloseUniverse = new System.Windows.Forms.MenuItem();
            this.menuItem37 = new System.Windows.Forms.MenuItem();
            this.Quit = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.readMeLink = new System.Windows.Forms.MenuItem();
            this.gameLink = new System.Windows.Forms.MenuItem();
            this.aboutLink = new System.Windows.Forms.MenuItem();
            this.mgu = new System.Windows.Forms.MenuItem();
            this.game = new System.Windows.Forms.MenuItem();
            this.smc = new System.Windows.Forms.MenuItem();
            this.zoom75 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.zoom100 = new System.Windows.Forms.MenuItem();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButton0 = new System.Windows.Forms.ToolBarButton();
            this.contextMenu0 = new System.Windows.Forms.ContextMenu();
            this.Sep0 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.Sep1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.Sep2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.contextMenu2 = new System.Windows.Forms.ContextMenu();
            this.zoom50 = new System.Windows.Forms.MenuItem();
            this.Sep3 = new System.Windows.Forms.ToolBarButton();
            this.Sep4 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
            this.Sep5 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
            this.Sep6 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
            this.Sep7 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
            this.Sep8 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
            this.Sep9 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton10 = new System.Windows.Forms.ToolBarButton();
            this.Locs = new System.Windows.Forms.ImageList(this.components);
            this.Cons = new System.Windows.Forms.ImageList(this.components);
            this.Goods = new System.Windows.Forms.ImageList(this.components);
            this.goLeftButton = new System.Windows.Forms.Button();
            this.goRightButton = new System.Windows.Forms.Button();
            this.goDownButton = new System.Windows.Forms.Button();
            this.goUpButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SMCGoods = new System.Windows.Forms.ImageList(this.components);
            this.SMRFocus = new System.Windows.Forms.ImageList(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.goToButton = new System.Windows.Forms.Button();
            this.goToField = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem5});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.CreateUniverse,
            this.OpenUniverse,
            this.SaveUniverse,
            this.CloseUniverse,
            this.menuItem37,
            this.Quit});
            this.menuItem1.Text = "&Game";
            // 
            // CreateUniverse
            // 
            this.CreateUniverse.Index = 0;
            this.CreateUniverse.Text = "&Create new universe";
            this.CreateUniverse.Click += new System.EventHandler(this.CreateNewUniverse_Click);
            // 
            // OpenUniverse
            // 
            this.OpenUniverse.Index = 1;
            this.OpenUniverse.Text = "Open game universe";
            this.OpenUniverse.Click += new System.EventHandler(this.OpenUniverse_Click);
            // 
            // SaveUniverse
            // 
            this.SaveUniverse.Index = 2;
            this.SaveUniverse.Text = "&Save game universe";
            this.SaveUniverse.Click += new System.EventHandler(this.SaveMap);
            // 
            // CloseUniverse
            // 
            this.CloseUniverse.Index = 3;
            this.CloseUniverse.Text = "&Close game universe";
            this.CloseUniverse.Click += new System.EventHandler(this.CloseMap);
            // 
            // menuItem37
            // 
            this.menuItem37.Index = 4;
            this.menuItem37.Text = "-";
            // 
            // Quit
            // 
            this.Quit.Index = 5;
            this.Quit.Text = "&Exit";
            this.Quit.Click += new System.EventHandler(this.CloseProgram);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.readMeLink,
            this.gameLink,
            this.aboutLink});
            this.menuItem5.Text = "&Help";
            // 
            // readMeLink
            // 
            this.readMeLink.Index = 0;
            this.readMeLink.Text = "View readme";
            this.readMeLink.Click += new System.EventHandler(this.ViewReadme);
            // 
            // gameLink
            // 
            this.gameLink.Index = 1;
            this.gameLink.Text = "Go to game";
            this.gameLink.Click += new System.EventHandler(this.WebsiteView);
            // 
            // aboutLink
            // 
            this.aboutLink.Index = 2;
            this.aboutLink.Text = "About";
            this.aboutLink.Click += new System.EventHandler(this.ViewAbout);
            // 
            // mgu
            // 
            this.mgu.Index = 0;
            this.mgu.RadioCheck = true;
            this.mgu.Text = "MGU";
            this.mgu.Click += new System.EventHandler(this.StyleClick);
            // 
            // game
            // 
            this.game.Index = 1;
            this.game.RadioCheck = true;
            this.game.Text = "Game";
            this.game.Click += new System.EventHandler(this.StyleClick);
            // 
            // smc
            // 
            this.smc.Index = 2;
            this.smc.RadioCheck = true;
            this.smc.Text = "SMC";
            this.smc.Click += new System.EventHandler(this.StyleClick);
            // 
            // zoom75
            // 
            this.zoom75.Index = 5;
            this.zoom75.RadioCheck = true;
            this.zoom75.Text = "75%";
            this.zoom75.Click += new System.EventHandler(this.ZoomChange);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 3;
            this.menuItem10.Text = "-";
            // 
            // zoom100
            // 
            this.zoom100.Index = 4;
            this.zoom100.RadioCheck = true;
            this.zoom100.Text = "100%";
            this.zoom100.Click += new System.EventHandler(this.ZoomChange);
            // 
            // toolBar1
            // 
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton0,
            this.Sep0,
            this.toolBarButton1,
            this.Sep1,
            this.toolBarButton2,
            this.Sep2,
            this.toolBarButton3,
            this.Sep3,
            this.Sep4,
            this.toolBarButton5,
            this.Sep5,
            this.toolBarButton6,
            this.Sep6,
            this.toolBarButton7,
            this.Sep7,
            this.toolBarButton8,
            this.Sep8,
            this.toolBarButton9,
            this.Sep9,
            this.toolBarButton10});
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.Locs;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(921, 50);
            this.toolBar1.TabIndex = 0;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.MenuButtonClicked);
            // 
            // toolBarButton0
            // 
            this.toolBarButton0.DropDownMenu = this.contextMenu0;
            this.toolBarButton0.Name = "toolBarButton0";
            this.toolBarButton0.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton0.Text = "Game";
            // 
            // Sep0
            // 
            this.Sep0.Name = "Sep0";
            this.Sep0.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.ImageIndex = 3;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton1.Text = "Illegals";
            // 
            // Sep1
            // 
            this.Sep1.Name = "Sep1";
            this.Sep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.DropDownMenu = this.contextMenu1;
            this.toolBarButton2.ImageIndex = 0;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton2.Text = "Galaxy";
            // 
            // Sep2
            // 
            this.Sep2.Name = "Sep2";
            this.Sep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.DropDownMenu = this.contextMenu2;
            this.toolBarButton3.ImageIndex = 6;
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButton3.Text = "Style";
            // 
            // contextMenu2
            // 
            this.contextMenu2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mgu,
            this.game,
            this.smc,
            this.menuItem10,
            this.zoom100,
            this.zoom75,
            this.zoom50});
            this.contextMenu2.Popup += new System.EventHandler(this.contextMenu2_Popup);
            // 
            // zoom50
            // 
            this.zoom50.Index = 6;
            this.zoom50.RadioCheck = true;
            this.zoom50.Text = "50%";
            this.zoom50.Click += new System.EventHandler(this.ZoomChange);
            // 
            // Sep3
            // 
            this.Sep3.Name = "Sep3";
            this.Sep3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // Sep4
            // 
            this.Sep4.Name = "Sep4";
            this.Sep4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton5
            // 
            this.toolBarButton5.ImageIndex = 8;
            this.toolBarButton5.Name = "toolBarButton5";
            this.toolBarButton5.Text = "Plot Course";
            // 
            // Sep5
            // 
            this.Sep5.Name = "Sep5";
            this.Sep5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton6
            // 
            this.toolBarButton6.ImageIndex = 7;
            this.toolBarButton6.Name = "toolBarButton6";
            this.toolBarButton6.Text = "Plot Arming";
            // 
            // Sep6
            // 
            this.Sep6.Name = "Sep6";
            this.Sep6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton7
            // 
            this.toolBarButton7.ImageIndex = 4;
            this.toolBarButton7.Name = "toolBarButton7";
            this.toolBarButton7.Text = "Find Route";
            // 
            // Sep7
            // 
            this.Sep7.Name = "Sep7";
            this.Sep7.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton8
            // 
            this.toolBarButton8.ImageIndex = 5;
            this.toolBarButton8.Name = "toolBarButton8";
            this.toolBarButton8.Text = "Find Locs";
            // 
            // Sep8
            // 
            this.Sep8.Name = "Sep8";
            this.Sep8.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton9
            // 
            this.toolBarButton9.ImageIndex = 10;
            this.toolBarButton9.Name = "toolBarButton9";
            this.toolBarButton9.Text = "Trade Calc";
            // 
            // Sep9
            // 
            this.Sep9.Name = "Sep9";
            this.Sep9.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton10
            // 
            this.toolBarButton10.ImageIndex = 12;
            this.toolBarButton10.Name = "toolBarButton10";
            this.toolBarButton10.Text = "Force Manager";
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
            this.Locs.Images.SetKeyName(10, "");
            this.Locs.Images.SetKeyName(11, "enemy_forces.GIF");
            this.Locs.Images.SetKeyName(12, "friendly_forces.GIF");
            // 
            // Cons
            // 
            this.Cons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Cons.ImageStream")));
            this.Cons.TransparentColor = System.Drawing.Color.White;
            this.Cons.Images.SetKeyName(0, "");
            this.Cons.Images.SetKeyName(1, "");
            this.Cons.Images.SetKeyName(2, "");
            this.Cons.Images.SetKeyName(3, "");
            this.Cons.Images.SetKeyName(4, "");
            this.Cons.Images.SetKeyName(5, "");
            this.Cons.Images.SetKeyName(6, "");
            this.Cons.Images.SetKeyName(7, "");
            // 
            // Goods
            // 
            this.Goods.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Goods.ImageStream")));
            this.Goods.TransparentColor = System.Drawing.Color.Transparent;
            this.Goods.Images.SetKeyName(0, "23 planet.gif");
            this.Goods.Images.SetKeyName(1, "00 wood.gif");
            this.Goods.Images.SetKeyName(2, "01 food.gif");
            this.Goods.Images.SetKeyName(3, "02 ore.gif");
            this.Goods.Images.SetKeyName(4, "03 precious metals.gif");
            this.Goods.Images.SetKeyName(5, "04 slaves.gif");
            this.Goods.Images.SetKeyName(6, "05 textiles.gif");
            this.Goods.Images.SetKeyName(7, "06 machinery.gif");
            this.Goods.Images.SetKeyName(8, "07 circuitry.gif");
            this.Goods.Images.SetKeyName(9, "08 weapons.gif");
            this.Goods.Images.SetKeyName(10, "09 computers.gif");
            this.Goods.Images.SetKeyName(11, "10 luxury items.gif");
            this.Goods.Images.SetKeyName(12, "11 narcotics.gif");
            this.Goods.Images.SetKeyName(13, "smc 00 wood.GIF");
            this.Goods.Images.SetKeyName(14, "smc 01 food.GIF");
            this.Goods.Images.SetKeyName(15, "smc 02 ore.GIF");
            this.Goods.Images.SetKeyName(16, "smc 03 precious metals.GIF");
            this.Goods.Images.SetKeyName(17, "smc 04 slaves.gif");
            this.Goods.Images.SetKeyName(18, "smc 05 textiles.GIF");
            this.Goods.Images.SetKeyName(19, "smc 06 machinery.gif");
            this.Goods.Images.SetKeyName(20, "smc 07 circuitry.GIF");
            this.Goods.Images.SetKeyName(21, "smc 08 weapons.GIF");
            this.Goods.Images.SetKeyName(22, "smc 09 computers.GIF");
            this.Goods.Images.SetKeyName(23, "smc 10 luxury items.GIF");
            this.Goods.Images.SetKeyName(24, "smc 11 narcotics.GIF");
            // 
            // goLeftButton
            // 
            this.goLeftButton.BackColor = System.Drawing.Color.Goldenrod;
            this.goLeftButton.ImageIndex = 2;
            this.goLeftButton.ImageList = this.Cons;
            this.goLeftButton.Location = new System.Drawing.Point(847, 24);
            this.goLeftButton.Name = "goLeftButton";
            this.goLeftButton.Size = new System.Drawing.Size(20, 20);
            this.goLeftButton.TabIndex = 1;
            this.goLeftButton.UseVisualStyleBackColor = false;
            this.goLeftButton.Click += new System.EventHandler(this.goLeftButton_Click);
            // 
            // goRightButton
            // 
            this.goRightButton.BackColor = System.Drawing.Color.Goldenrod;
            this.goRightButton.ImageIndex = 4;
            this.goRightButton.ImageList = this.Cons;
            this.goRightButton.Location = new System.Drawing.Point(887, 24);
            this.goRightButton.Name = "goRightButton";
            this.goRightButton.Size = new System.Drawing.Size(20, 20);
            this.goRightButton.TabIndex = 2;
            this.goRightButton.UseVisualStyleBackColor = false;
            this.goRightButton.Click += new System.EventHandler(this.goRightButton_Click);
            // 
            // goDownButton
            // 
            this.goDownButton.BackColor = System.Drawing.Color.Goldenrod;
            this.goDownButton.ImageIndex = 5;
            this.goDownButton.ImageList = this.Cons;
            this.goDownButton.Location = new System.Drawing.Point(867, 24);
            this.goDownButton.Name = "goDownButton";
            this.goDownButton.Size = new System.Drawing.Size(20, 20);
            this.goDownButton.TabIndex = 4;
            this.goDownButton.UseVisualStyleBackColor = false;
            this.goDownButton.Click += new System.EventHandler(this.goDownButton_Click);
            // 
            // goUpButton
            // 
            this.goUpButton.BackColor = System.Drawing.Color.Goldenrod;
            this.goUpButton.ImageIndex = 3;
            this.goUpButton.ImageList = this.Cons;
            this.goUpButton.Location = new System.Drawing.Point(867, 4);
            this.goUpButton.Name = "goUpButton";
            this.goUpButton.Size = new System.Drawing.Size(20, 20);
            this.goUpButton.TabIndex = 3;
            this.goUpButton.UseVisualStyleBackColor = false;
            this.goUpButton.Click += new System.EventHandler(this.goUpButton_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(921, 503);
            this.panel1.TabIndex = 7;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(917, 0);
            this.panel5.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(0, 499);
            this.panel4.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(917, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(0, 499);
            this.panel3.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 499);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(917, 0);
            this.panel2.TabIndex = 0;
            // 
            // SMCGoods
            // 
            this.SMCGoods.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.SMCGoods.ImageSize = new System.Drawing.Size(16, 16);
            this.SMCGoods.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // SMRFocus
            // 
            this.SMRFocus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SMRFocus.ImageStream")));
            this.SMRFocus.TransparentColor = System.Drawing.Color.Transparent;
            this.SMRFocus.Images.SetKeyName(0, "");
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            // 
            // goToButton
            // 
            this.goToButton.BackColor = System.Drawing.Color.Transparent;
            this.goToButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.goToButton.ForeColor = System.Drawing.Color.Transparent;
            this.goToButton.Location = new System.Drawing.Point(745, 2);
            this.goToButton.Name = "goToButton";
            this.goToButton.Size = new System.Drawing.Size(90, 16);
            this.goToButton.TabIndex = 8;
            this.goToButton.Text = "Go to sector:";
            this.goToButton.UseVisualStyleBackColor = false;
            this.goToButton.Click += new System.EventHandler(this.goToButton_Click);
            // 
            // goToField
            // 
            this.goToField.Location = new System.Drawing.Point(746, 20);
            this.goToField.Name = "goToField";
            this.goToField.Size = new System.Drawing.Size(89, 22);
            this.goToField.TabIndex = 9;
            this.goToField.Text = "";
            this.goToField.TextChanged += new System.EventHandler(this.goToField_TextChanged);
            // 
            // MainStuff
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(921, 553);
            this.Controls.Add(this.goToField);
            this.Controls.Add(this.goToButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.goDownButton);
            this.Controls.Add(this.goUpButton);
            this.Controls.Add(this.goRightButton);
            this.Controls.Add(this.goLeftButton);
            this.Controls.Add(this.toolBar1);
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(784, 86);
            this.Name = "MainStuff";
            this.Text = "Merchants Guide to the Universe Version 1.6.1.8";
            this.Load += new System.EventHandler(this.MainStuff_Load);
            this.SizeChanged += new System.EventHandler(this.DoResize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
#endregion

        #region Galaxy Movement
        //This region contains the functionality of the buttons to scroll through a galaxy.
        //Each button does this by changing the startsector of the currently loaded game,
        //i.e. the upperleft sector and then redrawing the screen
        
		private void goLeftButton_Click(object sender, System.EventArgs e)
		{
            games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector -= 1;
            if (games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector - games[currentGame].galaxy[games[currentGame].currentGalaxy].lowestsectorid + 1 % games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize == 0)
                games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector += games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize;
            Redraw();
		}

		private void goDownButton_Click(object sender, System.EventArgs e)
		{
            games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector += games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize;
            if (games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector > games[currentGame].galaxy[games[currentGame].currentGalaxy].lowestsectorid + games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize * games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_ysize)
                games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector -= games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize * games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_ysize;
            Redraw();
		}

		private void goRightButton_Click(object sender, System.EventArgs e)
		{
            games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector += 1;
            if (games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector % games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize == 1)
                games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector -= games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize;
            Redraw();
		}

		private void goUpButton_Click(object sender, System.EventArgs e)
		{
            games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector -= games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize;
            if (games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector < games[currentGame].galaxy[games[currentGame].currentGalaxy].lowestsectorid)
                games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector += games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize * games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_ysize;
            Redraw();
		}

        private void DoResize(object sender, System.EventArgs e)
        {
            this.panel1.Size = new Size(this.Width - this.panel1.Location.X - 8, this.Height - this.panel1.Location.Y - 46);
        }

#endregion
        
        #region Toolbar Invoked
		private void MenuButtonClicked(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		//Check what button was clicked and take action accordingly
        {
			if (e.Button == toolBarButton1)
			{
                games[currentGame].displayIllegals = e.Button.Pushed;
                Redraw();
			}

			if (e.Button == toolBarButton2)
				contextMenu1.Show(this, new Point(toolBarButton2.Rectangle.X, toolBarButton2.Rectangle.Y + toolBarButton2.Rectangle.Height + 2));
			if (e.Button == toolBarButton3)
				contextMenu2.Show(this, new Point(toolBarButton3.Rectangle.X, toolBarButton3.Rectangle.Y + toolBarButton3.Rectangle.Height + 2));
			if (e.Button == toolBarButton5)
				DoPlotCourse();
			if (e.Button == toolBarButton6)
				DoArmRoute();
			if (e.Button == toolBarButton7)
				FindTrade();
			if (e.Button == toolBarButton8)
				FindLocation();
			if (e.Button == TradeCalculaterButton)
				OpenTradeCalc();
            if (e.Button == toolBarButton10)
                //MessageBox.Show("This function has not been implemented yet");
                OpenForceManager();
		}

		private void ChangeGalaxy(object sender, System.EventArgs e)
        //This function is invoked when the user changes the galaxy to be viewed
		{
            games[currentGame].currentGalaxy = ((MenuItem)sender).Index + 1;
            toolBarButton2.Text = games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_name;
			Redraw();
		}

        public void ChangeGalaxy(int newIndex)
        //This function changes the currently displayed galaxy to that with the given index
        {
            games[currentGame].currentGalaxy = newIndex;
            toolBarButton2.Text = games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_name;
            Redraw();
        }

        private void ChangeGame(object sender, System.EventArgs e)
        //This function is invoked when the user changes the galaxy to be viewed
        {
            int previousgame = currentGame;

            currentGame = ((MenuItem)sender).Index+1;
            toolBarButton0.Text = games[currentGame].game_name;
            if (nrofgames >= 2)
                for (int g = 0; g < games[previousgame].nrofgalaxies; g++)
                    for (int x = 0; x < games[previousgame].galaxy[g].galaxy_xsize; x++)
                        for (int y = 0; y < games[previousgame].galaxy[g].galaxy_ysize; y++)
                        {
                            games[previousgame].galaxy[g].sector[x, y].Parent = null;
                            games[previousgame].galaxy[g].sector[x, y].Visible = false;
                        }

            //This is where we set panel1 as the parent control of all sector objects, so that the OnPaint message of panle1 gets passed on to all sectors.
            for (int g = 0; g < games[currentGame].nrofgalaxies; g++)
                for (int x = 0; x < games[currentGame].galaxy[g].galaxy_xsize; x++)
                    for (int y = 0; y < games[currentGame].galaxy[g].galaxy_ysize; y++)
                        games[currentGame].galaxy[g].sector[x, y].Parent = panel1;

            //Add the galaxies to the menu
            AddGalaxies();
            Redraw();
        }

        public void ChangeGame(int newIndex)
        //This function changes the currently displayed galaxy to that with the given index
        {
            int previousgame = currentGame;

            currentGame = newIndex;
            toolBarButton0.Text = games[currentGame].game_name;
            if (nrofgames >= 2)
                for (int g = 0; g < games[previousgame].nrofgalaxies; g++)
                    for (int x = 0; x < games[previousgame].galaxy[g].galaxy_xsize; x++)
                        for (int y = 0; y < games[previousgame].galaxy[g].galaxy_ysize; y++)
                        {
                            games[previousgame].galaxy[g].sector[x, y].Parent = null;
                            games[previousgame].galaxy[g].sector[x, y].Visible = false;
                        }

            //This is where we set panel1 as the parent control of all sector objects, so that the OnPaint message of panle1 gets passed on to all sectors.
            for (int g = 0; g < games[currentGame].nrofgalaxies; g++)
                for (int x = 0; x < games[currentGame].galaxy[g].galaxy_xsize; x++)
                    for (int y = 0; y < games[currentGame].galaxy[g].galaxy_ysize; y++)
                        games[currentGame].galaxy[g].sector[x, y].Parent = panel1;

            //Add the galaxies to the menu
            AddGalaxies();
            Redraw();
        }

		private void StyleClick(object sender, System.EventArgs e)
        //This function changes the sector display style, as chosen by the user
		{
            smc.Checked = false;
            game.Checked = false;
            mgu.Checked = false;

            if ((MenuItem)sender == this.mgu)
            {
                if (displaystyle == 2)
                    games[currentGame].sectorsize -= 10;
                displaystyle = 0;
                mgu.Checked = true;
            }
            if ((MenuItem)sender == this.game)
            {
                if (displaystyle == 2)
                    games[currentGame].sectorsize -= 10;
                displaystyle = 1;
                game.Checked = true;
            }
            if ((MenuItem)sender == this.smc)
            {
                if(displaystyle != 2)
                    games[currentGame].sectorsize += 10;
                displaystyle = 2;
                smc.Checked = true;
            }
            Redraw();
        }

		private void ZoomChange(object sender, System.EventArgs e)
        //This function changes the sector size
		{
            zoom100.Checked = false;
            zoom75.Checked = false;
            zoom50.Checked = false;

            if ((MenuItem)sender == zoom100)
            {
                games[currentGame].sectorsize = 100;
                zoom100.Checked = true;
            }
            if ((MenuItem)sender == zoom75)
            {
                games[currentGame].sectorsize = 75;
                zoom75.Checked = true;
            }
            if ((MenuItem)sender == zoom50)
            {
                games[currentGame].sectorsize = 50;
                zoom50.Checked = true;
            }

            Redraw();
		}
#endregion

        #region Menu Actions

        //Define all the GUI elements of the application
		public PlotWindow Plotter = null;
		private RouteFinder Trader = null;
		public OptimalRoute Optim = null;
		private LocationDisplay LD = null;
        private NewUniverse newuni = null;
        public ForceManager forceMan = null;

		private void DoPlotCourse()
        //This function creates a new course plotter or displays an existing one. The code for the course plotter can be found in PlotWindow.cs
		{
			if (Plotter == null)
			{
				Plotter = new PlotWindow(this);
				Plotter.Disposed += new EventHandler(PlotDispose);
				Plotter.Show();
				toolBarButton5.Pushed = true;
			}
			else Plotter.Activate();
		}

		private void DoArmRoute()
        //This function creates a new arming route plotter or displays an existing one. The code for the arming route plotter can be found in OptimalRoute.cs
		{
			if (Optim == null)
			{
				Optim = new OptimalRoute(this);
				Optim.Disposed += new EventHandler(ArmDispose);
				Optim.Show();
				toolBarButton6.Pushed = true;
			}
			else Optim.Activate();
		}

		private void FindTrade()
        //This function creates a new trade route or displays an existing one. The code for the course plotter can be found in PlotWindow.cs
		{
			if (Trader == null)
			{
				Trader = new RouteFinder(this);
				Trader.Disposed += new EventHandler(TradeDispose);
				Trader.Show();
				toolBarButton7.Pushed = true;
			}
			else
                Trader.Show();
		}

		private void FindLocation()
		{
			if (LD == null)
			{
				LD = new LocationDisplay(this);
				LD.Disposed += new EventHandler(LocationDispose);
				LD.Show();
				toolBarButton8.Pushed = true;
			}
			else LD.Activate();
		}

		private void OpenTradeCalc()
		{
		}

        private void OpenForceManager()
        {
            if (forceMan == null)
            {
                forceMan = new ForceManager(this);
                forceMan.Disposed += new EventHandler(ForceManagerDispose);
                forceMan.Show();
                toolBarButton10.Pushed = true;
            }
            else forceMan.Activate();
        }

		public void PlotDispose(object sender, System.EventArgs e)
		{
			Plotter = null;
			toolBarButton5.Pushed = false;
		}

		private void ArmDispose(object sender, System.EventArgs e)
		{
			Optim = null;
			toolBarButton6.Pushed = false;
		}

		private void TradeDispose(object sender, System.EventArgs e)
		{
			Trader = null;
			toolBarButton7.Pushed = false;
		}

        private void ForceManagerDispose(object sender, System.EventArgs e)
        {
            forceMan = null;
            toolBarButton10.Pushed = false;
        }

		private void LocationDispose(object sender, System.EventArgs e)
		{
			LD = null;
			toolBarButton8.Pushed = false;
		}

        private void newUniDispose(object sender, System.EventArgs e)
        {
            newuni = null;
        }

		private void TradeCalcClosed(object sender, System.EventArgs e)
		{

		}

		private void CreateNewUniverse_Click(object sender, System.EventArgs e)
		{
            if (newuni == null)
            {
                newuni = new NewUniverse(this);
                newuni.Disposed += new EventHandler(newUniDispose);
                newuni.Show();
            }
            else newuni.Activate();
		}

		private void CloseMap(object sender, System.EventArgs e)
        {
            FileStream fs;
            DialogResult userResponse = MessageBox.Show("Do you want to save the current map before closing? If you choose not to, all your modifications will be lost", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (userResponse == DialogResult.Yes)
                SaveMap(sender, e);

            if (games[currentGame].saving)
                MessageBox.Show("Waiting for map to be saved");

            while (games[currentGame].saving) ;

            ClearGame();

            for (int g = currentGame; g < nrofgames; g++)
                games[g] = games[g + 1];

            if (nrofgames > 1)
            {
                nrofgames--;
                if (currentGame > nrofgames)
                    currentGame--;

                ChangeGame(currentGame);
            }
            else
            {
                nrofgames--;
                currentGame = -1;
                toolBarButton0.Text = "";
                SaveUniverse.Enabled = CloseUniverse.Enabled = false;
                toolBarButton0.Enabled = false;
                toolBarButton1.Enabled = false;
                toolBarButton2.Enabled = false;
                toolBarButton3.Enabled = false;

                toolBarButton5.Enabled = false;
                toolBarButton6.Enabled = false;
                toolBarButton7.Enabled = false;
                toolBarButton8.Enabled = false;
                toolBarButton9.Enabled = false;
                toolBarButton10.Enabled = false;
            }
        }

		private void SaveMap(object sender, System.EventArgs e)
		{
			SaveFileDialog saveMap = new SaveFileDialog();
            saveMap.Filter = "SMR Files (*.smr)|*.smr";
			
			saveMap.FilterIndex = 1;
			saveMap.RestoreDirectory = true;
			if (saveMap.ShowDialog() == DialogResult.Cancel) return;
			if (!saveMap.FileName.EndsWith(".smr"))
				saveMap.FileName += ".smr";
			string except = "";
            if (SMR16.SaveData(saveMap.FileName, games[currentGame])) return;
			if (except != "")
				MessageBox.Show("Unable to save to " + saveMap.FileName + ": " + except);
		}

		private void ViewReadme(object sender, System.EventArgs e)
		{
            MessageBox.Show("This function is not implemented at the moment");
            return;
			try 
			{
				System.Diagnostics.Process erf = new System.Diagnostics.Process();
				erf.StartInfo.FileName = "readme.txt";
				erf.Start();
			}
			catch (System.ComponentModel.Win32Exception)
			{}
		}

		private void WebsiteView(object sender, System.EventArgs e)
		{
			try 
			{
				System.Diagnostics.Process.Start("iexplore.exe", "http://mgu.smrealms.de");
			}
			catch (System.ComponentModel.Win32Exception)
			{}
		}

		private void ViewAbout(object sender, System.EventArgs e)
		{
			(new About()).ShowDialog(this);
			this.Activate();
		}

		private void CloseProgram(object sender, System.EventArgs e)
		{
            FileStream fs;

            //Save settings in temp file
            string address = Directory.GetCurrentDirectory() + @"\startup.txt";

            SMR16.SaveData(address, games[currentGame]);

            fs = new FileStream(address, FileMode.Create, FileAccess.Write, FileShare.Read);

            StreamWriter writer = new StreamWriter(fs);

            writer.WriteLine("LoadedMap=" + games[currentGame].address);
            writer.WriteLine("Style=" + displaystyle.ToString());
            for (int i = 0; i < games[currentGame].allianceMembers.Count; i++)
                writer.WriteLine("Alliance Member=" + games[currentGame].allianceMembers[i].ToString());

            writer.Close();
            fs.Close();

			this.Close();
		}

	
#endregion
#region Loading, Saving, Opening and Closing
		private bool Save(string to, ref string except)
		{
			try 
			{
				return Save(to);
			}
			catch (System.IO.IOException exc)
			{
				except = exc.ToString();
				return false;
			}
		}

		private bool Save(string to)
		{
			//if (!loaded) return true;
            if (to.EndsWith(".smr"))
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                RegistryKey newkey1 = key.CreateSubKey("^V^ Productions");
                RegistryKey newkey = newkey1.CreateSubKey("Merchants Guide to the Universe");
                return true;
            }
			return false;
		}
        				
#endregion

        #region HelpFunctions
        public void AddGalaxies()
        {
            contextMenu1.Dispose();
            contextMenu1 = new ContextMenu();

            MenuItem GalaxyItem = new MenuItem();
            for (int x = 1; x < games[currentGame].nrofgalaxies; x++)
            {
                GalaxyItem = new System.Windows.Forms.MenuItem();
                GalaxyItem.Index = x-1;
                GalaxyItem.Text = games[currentGame].galaxy[x].galaxy_name;
                GalaxyItem.RadioCheck = true;
                GalaxyItem.Click += new System.EventHandler(this.ChangeGalaxy);
                contextMenu1.MenuItems.Add(GalaxyItem);
            }

            toolBarButton2.Text = games[currentGame].galaxy[1].galaxy_name;
            toolBarButton2.Enabled = true;
        }

        public void Redraw()
        {
            panel1.SuspendLayout();
            panel1.Visible = false;

            if (currentGame == -1)
                return;

            for (int i = 0; i < games[currentGame].nrofgalaxies; i++)
            {
                for (int x = 0; x < games[currentGame].galaxy[i].galaxy_xsize; x++)
                    for (int y = 0; y < games[currentGame].galaxy[i].galaxy_ysize; y++)
                    {
                        if (games[currentGame].currentGalaxy != i)
                            games[currentGame].galaxy[i].sector[x, y].Visible = false;
                        else
                        {
                            games[currentGame].galaxy[i].sector[x, y].Location = new Point(games[currentGame].galaxy[i].sector[x, y].GetX(games[currentGame].galaxy[i].startsector) * games[currentGame].sectorsize + panel1.AutoScrollPosition.X, games[currentGame].galaxy[i].sector[x, y].GetY(games[currentGame].galaxy[i].startsector) * games[currentGame].sectorsize + panel1.AutoScrollPosition.Y);
                            games[currentGame].galaxy[i].sector[x, y].Visible = true;
                        }
                    }

                for (int x = 0; x < games[currentGame].galaxy[i].galaxy_xsize; x++)
                    for (int y = 0; y < games[currentGame].galaxy[i].galaxy_ysize; y++)
                    {
                        if (games[currentGame].galaxy[i].sector[x, y].Visible)
                            games[currentGame].galaxy[i].sector[x, y].Invalidate();
                    }
            }
            panel1.ResumeLayout(false);

            if (games[currentGame].nrofgalaxies > 0)
            {
                    panel1.Visible = true;
                    return;
            }
        }

        #endregion

        private void OpenUniverse_Click(object sender, EventArgs e)
        {
            if (nrofgames >= 5)
                MessageBox.Show("Maximum number of maps reached, please close a map first.");

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "SMR Files (*.smr)|*.smr";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName.ToString();
                if (File.Exists(Directory.GetCurrentDirectory() + @"\startup.txt"))
                {
                    FileStream fs = new FileStream(Directory.GetCurrentDirectory() + @"\startup.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                    StreamReader reader = new StreamReader(fs);
                    string nextline = reader.ReadLine();
                    char[] delim = { '=' };
                    string[] lineparts = nextline.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    if (lineparts[0].StartsWith("LoadedMap") && lineparts[1] == filename)
                    {
                        nextline = reader.ReadLine();
                        lineparts = nextline.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                        if (lineparts[0].StartsWith("Style"))
                            this.displaystyle = Convert.ToInt16(lineparts[1]);

                        nextline = reader.ReadLine();
                        if (reader.EndOfStream)
                            return;

                        while (nextline.StartsWith("Alliance Member"))
                        {
                            lineparts = nextline.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                            games[currentGame].allianceMembers.Add(lineparts[1]);
                            if (!reader.EndOfStream)
                                nextline = reader.ReadLine();
                            else
                                break;
                        }
                    }

                    reader.Close();
                    fs.Close();
                }
                if (filename.Trim().ToLower().EndsWith(".smr"))
                {
                    panel1.SuspendLayout();
                    try
                    {
                        if (SMR16.LoadData(filename, games[nrofgames+1]))
                        {
                            nrofgames++;
                            currentGame = nrofgames;

                            MenuItem GameItem = new System.Windows.Forms.MenuItem();
                            GameItem.Index = nrofgames - 1;
                            GameItem.Text = games[currentGame].game_name;
                            GameItem.RadioCheck = true;
                            GameItem.Click += new System.EventHandler(this.ChangeGame);
                            contextMenu0.MenuItems.Add(GameItem);

                            toolBarButton0.Text = games[currentGame].game_name;

                            if (nrofgames >= 2)
                                for (int g = 0; g < games[currentGame - 1].nrofgalaxies; g++)
                                    for (int x = 0; x < games[currentGame - 1].galaxy[g].galaxy_xsize; x++)
                                        for (int y = 0; y < games[currentGame - 1].galaxy[g].galaxy_ysize; y++)
                                        {
                                            games[currentGame - 1].galaxy[g].sector[x, y].Parent = null;
                                            games[currentGame - 1].galaxy[g].sector[x, y].Visible = false;
                                        }

                            //This is where we set panel1 as the parent control of all sector objects, so that the OnPaint message of panle1 gets passed on to all sectors.
                            for (int g = 0; g < games[currentGame].nrofgalaxies; g++)
                                for (int x = 0; x < games[currentGame].galaxy[g].galaxy_xsize; x++)
                                    for (int y = 0; y < games[currentGame].galaxy[g].galaxy_ysize; y++)
                                        games[currentGame].galaxy[g].sector[x, y].Parent = panel1;

                            //Add the galaxies to the menu
                            AddGalaxies();
                            Redraw();

                            goLeftButton.Enabled = goRightButton.Enabled = goDownButton.Enabled = goUpButton.Enabled = toolBarButton1.Enabled = toolBarButton2.Enabled = toolBarButton3.Enabled = toolBarButton5.Enabled = this.toolBarButton6.Enabled = this.toolBarButton7.Enabled = this.toolBarButton8.Enabled = this.toolBarButton1.Enabled = toolBarButton9.Enabled = toolBarButton10.Enabled = true;
                            CloseUniverse.Enabled = SaveUniverse.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Error while reading maps.\nMaps NOT loaded");
                            loaded = false;
                            CloseUniverse.Enabled = SaveUniverse.Enabled = false;
                        }
                    }
                    catch (Exception Except0r)
                    {
                        MessageBox.Show("Error while reading maps: " + Except0r.Message + "\nMaps NOT loaded");
                        loaded = false;
                    }
                    panel1.ResumeLayout(false);
                }
            }
        }

        public void ClearGame()
        {
            contextMenu1.MenuItems.Clear();
            contextMenu0.MenuItems.RemoveAt(currentGame-1);

            //Remove all sector objects
            for (int g = 0; g < games[currentGame].nrofgalaxies; g++)
                for (int x = 0; x < games[currentGame].galaxy[g].galaxy_xsize; x++)
                    for (int y = 0; y < games[currentGame].galaxy[g].galaxy_ysize; y++)
                    {
                        games[currentGame].galaxy[g].sector[x, y].Dispose();
                    }

            toolBarButton2.Text = "";
        }

        private void contextMenu2_Popup(object sender, EventArgs e)
        {

        }

        private void goToButton_Click(object sender, EventArgs e)
        {
            int sectornr = 0, galaxynr = 0;

            if (!intparse(goToField.Text, ref sectornr))
                MessageBox.Show("Sector number could not be read!");
            else
            {
                galaxynr = games[currentGame].GetGalaxyIndex(sectornr);
                if (galaxynr == -1)
                {
                    MessageBox.Show("Not a valid sector for the currently loaded game!");
                }
                else
                {
                    games[currentGame].currentGalaxy = galaxynr;
                    //Get number of sectors displayed
                    int sectorwidth = this.Width / games[currentGame].sectorsize - 1;
                    int sectorheight = (this.Height - 42) / games[currentGame].sectorsize - 1;

                    games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector = sectornr - (int)(0.5 * sectorwidth) - (int)(0.5 * sectorheight) * games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize;
                    if (games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector < games[currentGame].galaxy[games[currentGame].currentGalaxy].lowestsectorid)
                        games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector += games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize * games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_ysize;
                    toolBarButton2.Text = games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_name;

                    this.panel1.AutoScrollPosition = new Point(0, 0);
                    this.panel1.Invalidate();
                    Redraw();
                    
                }
            }
        }

        private static bool intparse(string toparse, ref int result)
        {
            try { result = int.Parse(toparse); return true; }
            catch (System.FormatException) { return false; }
        }

        private void goToField_TextChanged(object sender, EventArgs e)
        {
            if(goToField.Text.Contains("\n"))
            {
                int sectornr = 0, galaxynr = 0;

                if (!intparse(goToField.Text, ref sectornr))
                    MessageBox.Show("Sector number could not be read!");
                else
                {
                    galaxynr = games[currentGame].GetGalaxyIndex(sectornr);
                    if (galaxynr == -1)
                    {
                        MessageBox.Show("Not a valid sector for the currently loaded game!");
                    }
                    else
                    {
                        games[currentGame].currentGalaxy = galaxynr;
                        //Get number of sectors displayed
                        int sectorwidth = this.Width / games[currentGame].sectorsize - 1;
                        int sectorheight = (this.Height - 42) / games[currentGame].sectorsize - 1;

                        games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector = sectornr - (int)(0.5 * sectorwidth) - (int)(0.5 * sectorheight) * games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize;
                        if (games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector < games[currentGame].galaxy[games[currentGame].currentGalaxy].lowestsectorid)
                            games[currentGame].galaxy[games[currentGame].currentGalaxy].startsector += games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_xsize * games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_ysize;
                        toolBarButton2.Text = games[currentGame].galaxy[games[currentGame].currentGalaxy].galaxy_name;
                        Redraw();
                    }
                }

                goToField.Text = goToField.Text.Remove(goToField.Text.Length - 1);
            }
        }

        private void MainStuff_Load(object sender, EventArgs e)
        {

        }
	}
}
