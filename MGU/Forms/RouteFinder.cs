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
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace MGU
{
	public class RouteFinder : System.Windows.Forms.Form
	{
        MainStuff hostApplication;
        Game currentGame;
        ArrayList tradeRoutes = new ArrayList();
        ArrayList sortedRoutes = new ArrayList();
        ArrayList threeWayPartialRoutes = new ArrayList();
        ArrayList allowedPorts = new ArrayList();
        TimeSpan allowedPortsTime, complementarySectorTime, generateRoutesTime, threeWayRoutesTime, cashXpTime, sortRoutesTime, displayRoutesTime;

        private bool[] goodsAllowed;
        private bool[] racesAllowed;
        private bool[] galaxiesAllowed;
        private bool OneGoodRoutesAllowed = false;
        private bool ThreeGoodRoutesAllowed = false;

		protected System.Windows.Forms.Button CalculateTrigger;
        private System.Windows.Forms.Button MyCancelButton;
        private System.Windows.Forms.Label GoodsL;
        private System.Windows.Forms.RadioButton ExpRadio;
		private System.Windows.Forms.Label TypeLabel;
		private System.Windows.Forms.RichTextBox Result;
		private System.ComponentModel.IContainer components;
		private int lastgal;
		private bool _destroyed = false;
		private System.Windows.Forms.ImageList imageList3;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ImageList imageList2;
		private ToolbarControl.PictureBar userControl11;
        private ToolbarControl.PictureBar goodsPictureBar;
		private System.Windows.Forms.Label label1;
		private ArrayList raceButtons;
        private ArrayList relationLabels;
        private ArrayList relationFields;
        private System.Threading.Mutex mutex = new Mutex();
        private static Thread BGU = null;
        private GroupBox GalaxyList;
        private CheckedListBox GalaxyListBox;
        private CheckBox allowIllegals;
        private RadioButton CashRadio;
        private bool loading = true;
        private Label label3;
        private ComboBox shipSelecter;
        private Button SelectAllButton;
        private Button DeselectAllButton;
        private int goal = 1;
        private bool useJump = false;
        private ProgressBar progressBar1;
        private Panel panel1;
        private RadioButton jumpNoRadio;
        private RadioButton jumpYesRadio;
        private Label label5;
        private CheckBox Allow1GoodRoutesCheckBox;
        private CheckBox Allow3GoodRoutesCheckBox;
        private ListBox routeList;
        private Button HighlightRouteButton;
        protected Button RoutesThroughSectorButton;
        private RichTextBox sectorField;
        private Label label2;
        private Button AdjustRelationsButton;
        private Label label4;
        private RichTextBox AllowedRatioTextBox;
        private int displayType = 0;

		public bool destroyed
		{
			get { return _destroyed; }
		}

		public RouteFinder(MainStuff host)
		{
            int index;
            hostApplication = host;
            currentGame = host.games[host.currentGame];
            InitializeComponent();

            galaxiesAllowed = new bool[currentGame.nrofgalaxies + 1];
            for (int g = 1; g < hostApplication.games[hostApplication.currentGame].nrofgalaxies; g++)
                GalaxyListBox.Items.Add(hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_name);

            this.GalaxyListBox.SetItemChecked(currentGame.currentGalaxy-1, true);
            this.galaxiesAllowed[currentGame.currentGalaxy] = true;

            goodsAllowed = new bool[currentGame.nrofgoods+1];
            goodsAllowed[0] = true;
            for (int i = 1; i <= currentGame.nrofgoods; i++)
            {
                goodsAllowed[i] = true;
            }

            allowIllegals.Checked = currentGame.displayIllegals;

            Allow1GoodRoutesCheckBox.Checked = false;
            OneGoodRoutesAllowed = false;
            Allow3GoodRoutesCheckBox.Checked = false;
            ThreeGoodRoutesAllowed = false;
            
            ToggleIllegalGoods();

#region Add race buttons
            racesAllowed = new bool[currentGame.nrofraces + 1];
            for (int i = 1; i <= currentGame.nrofraces; i++)
            {
                if(currentGame.peace[i])
                    this.racesAllowed[i] = true;
                else
                    this.racesAllowed[i] = false;
            }

            raceButtons = new ArrayList();
            raceButtons.Add(new System.Windows.Forms.Button());
            for (int x = 1; x < currentGame.nrofraces + 1; x++)
            {
                raceButtons.Add(new System.Windows.Forms.Button());

                System.Windows.Forms.Button currentRace = (System.Windows.Forms.Button)raceButtons[x];

                if(currentGame.peace[x])
                    currentRace.BackColor = System.Drawing.Color.PaleGreen;
                else
                    currentRace.BackColor = System.Drawing.Color.LightCoral;
                currentRace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                currentRace.Name = "Race" + x.ToString();
                currentRace.Size = new System.Drawing.Size(666 / currentGame.nrofraces, 21);
                currentRace.Font = new Font("Microsoft Sans Serif", (float)8.25 - currentGame.nrofraces / 9);
                currentRace.Location = new System.Drawing.Point(30 + 666 / currentGame.nrofraces * x, 248);

                currentRace.Text = currentGame.race[x].race_name;
                currentRace.UseVisualStyleBackColor = false;
                currentRace.Click += new System.EventHandler(this.raceButton_Click);

                this.Controls.Add((System.Windows.Forms.Button)raceButtons[x]);
            }
#endregion

#region Add relation buttons and field
            //Create new containers for the relation labels and fields
            relationLabels = new ArrayList();
            relationFields = new ArrayList();

            //Fill the 0 position so you can start at 1
            relationLabels.Add(new System.Windows.Forms.Label());
            relationFields.Add(new System.Windows.Forms.TextBox());

            for (int x = 1; x < currentGame.nrofraces + 1; x++)
            {
                relationLabels.Add(new System.Windows.Forms.Label());
                ((System.Windows.Forms.Label)relationLabels[x]).Name = "Race"+x.ToString();
                ((System.Windows.Forms.Label)relationLabels[x]).Size = new System.Drawing.Size(60, 20);
                ((System.Windows.Forms.Label)relationLabels[x]).Font = new Font("Microsoft Sans Serif", (float)8.25 - currentGame.nrofraces / 9);
                ((System.Windows.Forms.Label)relationLabels[x]).Location = new System.Drawing.Point(259, 20 + 160 / currentGame.nrofraces * x);

                ((System.Windows.Forms.Label)relationLabels[x]).Text = currentGame.race[x].race_name;

                this.Controls.Add((System.Windows.Forms.Label)relationLabels[x]);

                relationFields.Add(new System.Windows.Forms.TextBox());
                ((System.Windows.Forms.TextBox)relationFields[x]).Name = "Race" + x.ToString();
                ((System.Windows.Forms.TextBox)relationFields[x]).Size = new System.Drawing.Size(60, 20);
                ((System.Windows.Forms.TextBox)relationFields[x]).Font = new Font("Microsoft Sans Serif", (float)8.25 - currentGame.nrofraces / 9);
                ((System.Windows.Forms.TextBox)relationFields[x]).Location = new System.Drawing.Point(319, 20 + 160 / currentGame.nrofraces * x);
                //((System.Windows.Forms.TextBox)relationFields[x]).TextChanged += new System.EventHandler(this.relationField_TextChanged);

                ((System.Windows.Forms.TextBox)relationFields[x]).Text = currentGame.race[x].relations.ToString();

                this.Controls.Add((System.Windows.Forms.TextBox)relationFields[x]);
            }
#endregion

            //Add ships
            for (int s = 0; s < currentGame.nrofships; s++)
            {
                shipSelecter.Items.Add(currentGame.ship[s].name);
            }
            shipSelecter.SelectedIndex = 30;

            AllowedRatioTextBox.Text = "1.2";
        }

        protected override void Dispose(bool disposing)
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			_destroyed = true;
			if (BGU != null)
				BGU.Abort();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		protected void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MGU.Forms.RouteFinder));
            ToolbarControl.PictureBar.ColorsTemp colorsTemp1 = new ToolbarControl.PictureBar.ColorsTemp();
            ToolbarControl.PictureBar.ColorsTemp colorsTemp2 = new ToolbarControl.PictureBar.ColorsTemp();
            this.CalculateTrigger = new System.Windows.Forms.Button();
            this.Result = new System.Windows.Forms.RichTextBox();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.GoodsL = new System.Windows.Forms.Label();
            this.ExpRadio = new System.Windows.Forms.RadioButton();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.GalaxyList = new System.Windows.Forms.GroupBox();
            this.GalaxyListBox = new System.Windows.Forms.CheckedListBox();
            this.allowIllegals = new System.Windows.Forms.CheckBox();
            this.CashRadio = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.shipSelecter = new System.Windows.Forms.ComboBox();
            this.SelectAllButton = new System.Windows.Forms.Button();
            this.DeselectAllButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.jumpNoRadio = new System.Windows.Forms.RadioButton();
            this.jumpYesRadio = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.Allow1GoodRoutesCheckBox = new System.Windows.Forms.CheckBox();
            this.Allow3GoodRoutesCheckBox = new System.Windows.Forms.CheckBox();
            this.routeList = new System.Windows.Forms.ListBox();
            this.HighlightRouteButton = new System.Windows.Forms.Button();
            this.RoutesThroughSectorButton = new System.Windows.Forms.Button();
            this.sectorField = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.goodsPictureBar = new ToolbarControl.PictureBar();
            this.userControl11 = new ToolbarControl.PictureBar();
            this.AdjustRelationsButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.AllowedRatioTextBox = new System.Windows.Forms.RichTextBox();
            this.GalaxyList.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CalculateTrigger
            // 
            this.CalculateTrigger.Location = new System.Drawing.Point(12, 636);
            this.CalculateTrigger.Name = "CalculateTrigger";
            this.CalculateTrigger.Size = new System.Drawing.Size(80, 24);
            this.CalculateTrigger.TabIndex = 1;
            this.CalculateTrigger.Text = "Find routes";
            this.CalculateTrigger.Click += new System.EventHandler(this.CalculateTrigger_Click);
            // 
            // Result
            // 
            this.Result.BackColor = System.Drawing.SystemColors.Control;
            this.Result.Font = new System.Drawing.Font("Lucida Console", 7.5F);
            this.Result.Location = new System.Drawing.Point(7, 497);
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.Size = new System.Drawing.Size(813, 133);
            this.Result.TabIndex = 2;
            this.Result.Text = "Press the button to calculate routes.";
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(740, 634);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(80, 24);
            this.MyCancelButton.TabIndex = 17;
            this.MyCancelButton.Text = "Close";
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // GoodsL
            // 
            this.GoodsL.Location = new System.Drawing.Point(4, 215);
            this.GoodsL.Name = "GoodsL";
            this.GoodsL.Size = new System.Drawing.Size(44, 17);
            this.GoodsL.TabIndex = 23;
            this.GoodsL.Text = "Goods:";
            this.GoodsL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ExpRadio
            // 
            this.ExpRadio.Checked = true;
            this.ExpRadio.Location = new System.Drawing.Point(88, 12);
            this.ExpRadio.Name = "ExpRadio";
            this.ExpRadio.Size = new System.Drawing.Size(82, 16);
            this.ExpRadio.TabIndex = 25;
            this.ExpRadio.TabStop = true;
            this.ExpRadio.Text = "Experience";
            this.ExpRadio.CheckedChanged += new System.EventHandler(this.ExpRadio_CheckedChanged);
            // 
            // TypeLabel
            // 
            this.TypeLabel.Location = new System.Drawing.Point(4, 2);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(80, 32);
            this.TypeLabel.TabIndex = 34;
            this.TypeLabel.Text = "Route Type:";
            this.TypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // imageList3
            // 
            this.imageList3.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList3.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "");
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 243);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 18);
            this.label1.TabIndex = 41;
            this.label1.Text = "Allowed races:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GalaxyList
            // 
            this.GalaxyList.Controls.Add(this.GalaxyListBox);
            this.GalaxyList.ForeColor = System.Drawing.SystemColors.ControlText;
            this.GalaxyList.Location = new System.Drawing.Point(418, 4);
            this.GalaxyList.Name = "GalaxyList";
            this.GalaxyList.Size = new System.Drawing.Size(402, 174);
            this.GalaxyList.TabIndex = 55;
            this.GalaxyList.TabStop = false;
            this.GalaxyList.Text = "Allowed galaxies";
            // 
            // GalaxyListBox
            // 
            this.GalaxyListBox.CheckOnClick = true;
            this.GalaxyListBox.Location = new System.Drawing.Point(8, 16);
            this.GalaxyListBox.MultiColumn = true;
            this.GalaxyListBox.Name = "GalaxyListBox";
            this.GalaxyListBox.Size = new System.Drawing.Size(388, 154);
            this.GalaxyListBox.TabIndex = 0;
            this.GalaxyListBox.SelectedIndexChanged += new System.EventHandler(this.GalaxyListBox_SelectedIndexChanged);
            // 
            // allowIllegals
            // 
            this.allowIllegals.AutoSize = true;
            this.allowIllegals.Checked = true;
            this.allowIllegals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allowIllegals.Location = new System.Drawing.Point(710, 219);
            this.allowIllegals.Name = "allowIllegals";
            this.allowIllegals.Size = new System.Drawing.Size(93, 17);
            this.allowIllegals.TabIndex = 56;
            this.allowIllegals.Text = "Toggle illegals";
            this.allowIllegals.UseVisualStyleBackColor = true;
            this.allowIllegals.CheckedChanged += new System.EventHandler(this.allowIllegals_CheckedChanged);
            // 
            // CashRadio
            // 
            this.CashRadio.Location = new System.Drawing.Point(168, 12);
            this.CashRadio.Name = "CashRadio";
            this.CashRadio.Size = new System.Drawing.Size(53, 16);
            this.CashRadio.TabIndex = 26;
            this.CashRadio.Text = " Profit";
            this.CashRadio.CheckedChanged += new System.EventHandler(this.CashRadio_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 32);
            this.label3.TabIndex = 57;
            this.label3.Text = "Ship:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // shipSelecter
            // 
            this.shipSelecter.FormattingEnabled = true;
            this.shipSelecter.Location = new System.Drawing.Point(88, 40);
            this.shipSelecter.Name = "shipSelecter";
            this.shipSelecter.Size = new System.Drawing.Size(160, 21);
            this.shipSelecter.TabIndex = 58;
            this.shipSelecter.SelectedIndexChanged += new System.EventHandler(this.shipSelecter_SelectedIndexChanged);
            // 
            // SelectAllButton
            // 
            this.SelectAllButton.Location = new System.Drawing.Point(418, 184);
            this.SelectAllButton.Name = "SelectAllButton";
            this.SelectAllButton.Size = new System.Drawing.Size(196, 23);
            this.SelectAllButton.TabIndex = 59;
            this.SelectAllButton.Text = "Select all galaxies";
            this.SelectAllButton.UseVisualStyleBackColor = true;
            this.SelectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // DeselectAllButton
            // 
            this.DeselectAllButton.Location = new System.Drawing.Point(629, 184);
            this.DeselectAllButton.Name = "DeselectAllButton";
            this.DeselectAllButton.Size = new System.Drawing.Size(191, 23);
            this.DeselectAllButton.TabIndex = 60;
            this.DeselectAllButton.Text = "Deselect all galaxies";
            this.DeselectAllButton.UseVisualStyleBackColor = true;
            this.DeselectAllButton.Click += new System.EventHandler(this.DeselectAllButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 664);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(808, 23);
            this.progressBar1.TabIndex = 61;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.jumpNoRadio);
            this.panel1.Controls.Add(this.jumpYesRadio);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(12, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(212, 29);
            this.panel1.TabIndex = 65;
            // 
            // jumpNoRadio
            // 
            this.jumpNoRadio.AutoSize = true;
            this.jumpNoRadio.Checked = true;
            this.jumpNoRadio.Location = new System.Drawing.Point(151, 6);
            this.jumpNoRadio.Name = "jumpNoRadio";
            this.jumpNoRadio.Size = new System.Drawing.Size(39, 17);
            this.jumpNoRadio.TabIndex = 67;
            this.jumpNoRadio.TabStop = true;
            this.jumpNoRadio.Text = "No";
            this.jumpNoRadio.UseVisualStyleBackColor = true;
            this.jumpNoRadio.CheckedChanged += new System.EventHandler(this.jumpNoRadio_CheckedChanged);
            // 
            // jumpYesRadio
            // 
            this.jumpYesRadio.AutoSize = true;
            this.jumpYesRadio.Location = new System.Drawing.Point(80, 6);
            this.jumpYesRadio.Name = "jumpYesRadio";
            this.jumpYesRadio.Size = new System.Drawing.Size(43, 17);
            this.jumpYesRadio.TabIndex = 66;
            this.jumpYesRadio.Text = "Yes";
            this.jumpYesRadio.UseVisualStyleBackColor = true;
            this.jumpYesRadio.CheckedChanged += new System.EventHandler(this.jumpYesRadio_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 22);
            this.label5.TabIndex = 65;
            this.label5.Text = "Use Jump:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Allow1GoodRoutesCheckBox
            // 
            this.Allow1GoodRoutesCheckBox.AutoSize = true;
            this.Allow1GoodRoutesCheckBox.Checked = true;
            this.Allow1GoodRoutesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Allow1GoodRoutesCheckBox.Location = new System.Drawing.Point(12, 101);
            this.Allow1GoodRoutesCheckBox.Name = "Allow1GoodRoutesCheckBox";
            this.Allow1GoodRoutesCheckBox.Size = new System.Drawing.Size(119, 17);
            this.Allow1GoodRoutesCheckBox.TabIndex = 66;
            this.Allow1GoodRoutesCheckBox.Text = "Allow 1-good routes";
            this.Allow1GoodRoutesCheckBox.UseVisualStyleBackColor = true;
            this.Allow1GoodRoutesCheckBox.CheckedChanged += new System.EventHandler(this.Allow1GoodRoutesCheckBox_CheckedChanged);
            // 
            // Allow3GoodRoutesCheckBox
            // 
            this.Allow3GoodRoutesCheckBox.AutoSize = true;
            this.Allow3GoodRoutesCheckBox.Checked = true;
            this.Allow3GoodRoutesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Allow3GoodRoutesCheckBox.Location = new System.Drawing.Point(12, 124);
            this.Allow3GoodRoutesCheckBox.Name = "Allow3GoodRoutesCheckBox";
            this.Allow3GoodRoutesCheckBox.Size = new System.Drawing.Size(119, 17);
            this.Allow3GoodRoutesCheckBox.TabIndex = 67;
            this.Allow3GoodRoutesCheckBox.Text = "Allow 3-good routes";
            this.Allow3GoodRoutesCheckBox.UseVisualStyleBackColor = true;
            this.Allow3GoodRoutesCheckBox.CheckedChanged += new System.EventHandler(this.Allow3GoodRoutesCheckBox_CheckedChanged);
            // 
            // routeList
            // 
            this.routeList.Font = new System.Drawing.Font("Lucida Console", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routeList.HorizontalScrollbar = true;
            this.routeList.ItemHeight = 10;
            this.routeList.Location = new System.Drawing.Point(7, 272);
            this.routeList.Name = "routeList";
            this.routeList.Size = new System.Drawing.Size(813, 204);
            this.routeList.TabIndex = 68;
            this.routeList.SelectedIndexChanged += new System.EventHandler(this.routeList_SelectedIndexChanged);
            // 
            // HighlightRouteButton
            // 
            this.HighlightRouteButton.Location = new System.Drawing.Point(474, 637);
            this.HighlightRouteButton.Name = "HighlightRouteButton";
            this.HighlightRouteButton.Size = new System.Drawing.Size(140, 23);
            this.HighlightRouteButton.TabIndex = 69;
            this.HighlightRouteButton.Text = "Highlight and go to Route";
            this.HighlightRouteButton.UseVisualStyleBackColor = true;
            this.HighlightRouteButton.Click += new System.EventHandler(this.HighlightRouteButton_Click);
            // 
            // RoutesThroughSectorButton
            // 
            this.RoutesThroughSectorButton.Location = new System.Drawing.Point(98, 636);
            this.RoutesThroughSectorButton.Name = "RoutesThroughSectorButton";
            this.RoutesThroughSectorButton.Size = new System.Drawing.Size(150, 24);
            this.RoutesThroughSectorButton.TabIndex = 70;
            this.RoutesThroughSectorButton.Text = "Find routes through sector:";
            this.RoutesThroughSectorButton.Click += new System.EventHandler(this.RoutesThroughSectorButton_Click);
            // 
            // sectorField
            // 
            this.sectorField.Location = new System.Drawing.Point(254, 637);
            this.sectorField.Name = "sectorField";
            this.sectorField.Size = new System.Drawing.Size(89, 22);
            this.sectorField.TabIndex = 71;
            this.sectorField.Text = "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(259, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 18);
            this.label2.TabIndex = 72;
            this.label2.Text = "Relations:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // goodsPictureBar
            // 
            this.goodsPictureBar.BackgroundColorDown = System.Drawing.Color.MistyRose;
            this.goodsPictureBar.BackgroundColorNormal = System.Drawing.Color.WhiteSmoke;
            this.goodsPictureBar.BackgroundColorOver = System.Drawing.Color.FloralWhite;
            this.goodsPictureBar.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.goodsPictureBar.ImageList = this.imageList1;
            this.goodsPictureBar.LineColorDown = System.Drawing.Color.Black;
            this.goodsPictureBar.LineColorNormal = System.Drawing.Color.DimGray;
            this.goodsPictureBar.LineColorOver = System.Drawing.Color.Gray;
            colorsTemp1.Down = System.Drawing.Color.Black;
            colorsTemp1.Normal = System.Drawing.Color.Black;
            colorsTemp1.Over = System.Drawing.Color.Black;
            this.goodsPictureBar.LineColors = colorsTemp1;
            this.goodsPictureBar.Location = new System.Drawing.Point(54, 209);
            this.goodsPictureBar.Name = "goodsPictureBar";
            this.goodsPictureBar.Size = new System.Drawing.Size(638, 36);
            this.goodsPictureBar.TabIndex = 37;
            this.goodsPictureBar.Text = "goodsPictureBar";
            this.goodsPictureBar.Clicked += new ToolbarControl.PictureBar.EventHandler(this.goodsPictureBar_Clicked_1);
            // 
            // userControl11
            // 
            this.userControl11.BackgroundColorDown = System.Drawing.Color.MintCream;
            this.userControl11.BackgroundColorNormal = System.Drawing.Color.WhiteSmoke;
            this.userControl11.BackgroundColorOver = System.Drawing.Color.FloralWhite;
            this.userControl11.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.userControl11.ImageList = this.imageList1;
            this.userControl11.LineColorDown = System.Drawing.Color.Black;
            this.userControl11.LineColorNormal = System.Drawing.Color.DimGray;
            this.userControl11.LineColorOver = System.Drawing.Color.Gray;
            colorsTemp2.Down = System.Drawing.Color.Black;
            colorsTemp2.Normal = System.Drawing.Color.Black;
            colorsTemp2.Over = System.Drawing.Color.Black;
            this.userControl11.LineColors = colorsTemp2;
            this.userControl11.Location = new System.Drawing.Point(88, 79);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(0, 36);
            this.userControl11.TabIndex = 36;
            // 
            // AdjustRelationsButton
            // 
            this.AdjustRelationsButton.Location = new System.Drawing.Point(317, 13);
            this.AdjustRelationsButton.Name = "AdjustRelationsButton";
            this.AdjustRelationsButton.Size = new System.Drawing.Size(75, 23);
            this.AdjustRelationsButton.TabIndex = 73;
            this.AdjustRelationsButton.Text = "Adjust";
            this.AdjustRelationsButton.UseVisualStyleBackColor = true;
            this.AdjustRelationsButton.Click += new System.EventHandler(this.AdjustRelationsButton_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(137, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 17);
            this.label4.TabIndex = 74;
            this.label4.Text = "Allowed ratio";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AllowedRatioTextBox
            // 
            this.AllowedRatioTextBox.Location = new System.Drawing.Point(140, 121);
            this.AllowedRatioTextBox.Name = "AllowedRatioTextBox";
            this.AllowedRatioTextBox.Size = new System.Drawing.Size(89, 20);
            this.AllowedRatioTextBox.TabIndex = 75;
            this.AllowedRatioTextBox.Text = "";
            // 
            // RouteFinder
            // 
            this.AcceptButton = this.CalculateTrigger;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(832, 698);
            this.Controls.Add(this.AllowedRatioTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AdjustRelationsButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sectorField);
            this.Controls.Add(this.RoutesThroughSectorButton);
            this.Controls.Add(this.HighlightRouteButton);
            this.Controls.Add(this.routeList);
            this.Controls.Add(this.Allow3GoodRoutesCheckBox);
            this.Controls.Add(this.Allow1GoodRoutesCheckBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.DeselectAllButton);
            this.Controls.Add(this.SelectAllButton);
            this.Controls.Add(this.shipSelecter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.allowIllegals);
            this.Controls.Add(this.GalaxyList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.goodsPictureBar);
            this.Controls.Add(this.TypeLabel);
            this.Controls.Add(this.CashRadio);
            this.Controls.Add(this.ExpRadio);
            this.Controls.Add(this.GoodsL);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.CalculateTrigger);
            this.Controls.Add(this.userControl11);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(704, 558);
            this.Name = "RouteFinder";
            this.Text = "Port Negotiator";
            this.Load += new System.EventHandler(this.RouteFinder_Load);
            this.GalaxyList.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
       
        private void ExpRadio_CheckedChanged(object sender, EventArgs e)
        {
            //Set goal of our port search to experience
            goal = 1;
            AllowedRatioTextBox.Text = "1.2";
            ReSort();
        }

        private void CashRadio_CheckedChanged(object sender, EventArgs e)
        {
            //Set goal of our port search to profit
            goal = 0;
            AllowedRatioTextBox.Text = "3";
            ReSort();
        }

        private void goodsPictureBar_Clicked(int selectedIndex)
        {
            GoodsRefresh();
        }

        public void GoodsRefresh()
        {
            ImageList Goods = new ImageList();
            Goods.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            Goods.ImageSize = new System.Drawing.Size(16, 16);
            Goods.TransparentColor = System.Drawing.Color.White;

            goodsPictureBar.ImageList = Goods;

            for (int i = 1; i <= currentGame.nrofgoods; i++)
            {
                if(goodsAllowed[i])
                    goodsPictureBar.ImageList.Images.Add((System.Drawing.Image)currentGame.hostApplication.Goods.Images[i]);
                else
                    goodsPictureBar.ImageList.Images.Add(imageList2.Images[0]);
            }
        }

        private void goodsPictureBar_Clicked_1(int selectedIndex)
        {
            goodsAllowed[selectedIndex+1] = !goodsAllowed[selectedIndex+1];
            GoodsRefresh();
        }

        private void allowIllegals_CheckedChanged(object sender, EventArgs e)
        {
            currentGame.displayIllegals = !currentGame.displayIllegals;
            ToggleIllegalGoods();
        }

        public void ToggleIllegalGoods()
        {
            if (currentGame.displayIllegals)
            {
                for (int i = 0; i < currentGame.nrofgoods + 1; i++)
                    if (!currentGame.good[i].goodorevil)
                        goodsAllowed[i] = false;
            }
            else
            {
                for (int i = 0; i < currentGame.nrofgoods + 1; i++)
                    goodsAllowed[i] = true;
            }

            GoodsRefresh();
        }

        private void raceButton_Click(object sender, EventArgs e)
        {
            int racenr = currentGame.GetRaceIndex(((System.Windows.Forms.Button) sender).Text);

            System.Windows.Forms.Button currentButton = (System.Windows.Forms.Button)raceButtons[racenr];
            if (racesAllowed[racenr])
            {
                currentButton.FlatStyle = FlatStyle.Standard;
                currentButton.BackColor = System.Drawing.Color.LightCoral;
                currentGame.peace[racenr] = false;
                racesAllowed[racenr] = false;
                if(sortedRoutes.Count > 0)
                    ReDisplay();
            }
            else
            {
                currentButton.FlatStyle = FlatStyle.Flat;
                currentButton.BackColor = System.Drawing.Color.PaleGreen;
                currentGame.peace[racenr] = true;
                racesAllowed[racenr] = true;
                if (sortedRoutes.Count > 0)
                    ReDisplay();
            }

        }

        private void GalaxyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 1; i <= currentGame.nrofgalaxies; i++)
            {
                galaxiesAllowed[i] = GalaxyListBox.CheckedIndices.Contains(i-1);
            }
        }

        private void CalculateTrigger_Click(object sender, EventArgs e)
        {
            for(int r = 1; r <= currentGame.nrofraces; r++)
            {
                if(currentGame.race[r].relations < -300 && currentGame.peace[r] == true)
                {
                    DialogResult result = MessageBox.Show("You have indicated that you want to include " + currentGame.race[r].race_name + " in your search, but according to your relations with this race you can't trade their ports. Continue anyway?", "Relation mismatch detected", MessageBoxButtons.YesNo);
                    if(result == DialogResult.No)
                        return;
                }
            }

            ComputeRoutes();
            ReSort();
        }

        private void ComputeRoutes()
        {
            DateTime startTime, endTime;
            Result.Text = "";
            bool founddouble = false;
            Sector source = new Sector(), complement = new Sector(), start = new Sector(), end = new Sector(), middle = new Sector();
            Route newRoute, reverseRoute;
            double allowedRatio;
            if(AllowedRatioTextBox.Text != "")
                allowedRatio = Convert.ToDouble(AllowedRatioTextBox.Text.Replace('.', ','));
            else
                allowedRatio = 1.2;

            //First find all allowed ports
            tradeRoutes = new ArrayList();
            sortedRoutes = new ArrayList();
            threeWayPartialRoutes.Clear();
            allowedPorts.Clear();

            Result.Text = "Calculating allowed ports";
            Result.Invalidate();
            startTime = DateTime.Now;

            for(int g = 1; g < currentGame.nrofgalaxies; g++)
            {
                if (!galaxiesAllowed[g])
                    continue;
                else
                {
                    for(int x = 0; x < currentGame.galaxy[g].galaxy_xsize; x++)
                        for (int y = 0; y < currentGame.galaxy[g].galaxy_ysize; y++)
                        {
                            if (currentGame.galaxy[g].sector[x, y].port == null)
                                continue;
                            else
                                if (racesAllowed[currentGame.galaxy[g].sector[x, y].port.port_race.GetRaceNameAsId(currentGame)])
                                    allowedPorts.Add(currentGame.galaxy[g].sector[x, y]);
                        }
                }
            }

            //Reset all complementary goods
            for (int p = 0; p < allowedPorts.Count; p++)
            {
                ((Sector)allowedPorts[p]).port.Reset();
            }

            endTime = DateTime.Now;
            allowedPortsTime = endTime - startTime;

            Result.Text = allowedPorts.Count.ToString() + " ports were found, now calculating port multipliers";
            Result.Invalidate();
            startTime = DateTime.Now;

            //For each port that was found, and for each good on the port, compute the distance to the port that sells or buys the good
            for (int p = 0; p < allowedPorts.Count; p++)
            {
                for (int g = 0; g <= currentGame.nrofgoods; g++)
                {
                    if(goodsAllowed[g])
                        currentGame.FindComplementarySector((Sector)allowedPorts[p], g, galaxiesAllowed, true, OneGoodRoutesAllowed || ThreeGoodRoutesAllowed);
                }
            }

            endTime = DateTime.Now;
            complementarySectorTime = endTime - startTime;

            Result.Text = "Generating possible trade routes";
            Result.Invalidate();
            
            if(!ThreeGoodRoutesAllowed)
                progressBar1.Maximum = allowedPorts.Count * (currentGame.nrofgoods + 1);
            else
                progressBar1.Maximum = allowedPorts.Count * allowedPorts.Count * (currentGame.nrofgoods + 1);
            progressBar1.Value = 0;
            startTime = DateTime.Now;

            //For each port and each good, find complementary goods and generate Trade Routes
            for (int p = 0; p < allowedPorts.Count; p++)
            {
                source = (Sector)allowedPorts[p];

                for (int g = 0; g <= currentGame.nrofgoods; g++)
                {
                    progressBar1.Value++;

                    if (source.port.complementary_sectors[g].Count == 0 || source.port.port_goods[g] == 1 || !goodsAllowed[g])
                        continue;

                    for (int c = 0; c < source.port.complementary_sectors[g].Count; c++)
                    {
                        //Cut off the search if the distance to the complementary port is x*longer than the Distance index for the good
                        if ((int) source.port.complementary_distances[g][c] > allowedRatio * GetMinimum(source.port.complementary_distances[g]))
                            break;

                        complement = (Sector)source.port.complementary_sectors[g][c];

                        for(int rg = 0; rg <= currentGame.nrofgoods; rg++)
                        {
                            if(g==rg || !goodsAllowed[rg])
                                continue;

                            if (complement.port.port_goods[rg] == 1 || source.port.port_goods[rg] == 0)
                                continue;

                            for(int rc = 0; rc < complement.port.complementary_sectors[rg].Count; rc++)
                            {
                                if(complement.port.complementary_sectors[rg][rc].Equals(source))
                                {
                                    newRoute = new Route(currentGame);
                                    newRoute.Calculate(source.sector_id, complement.sector_id, galaxiesAllowed, false);
                                    newRoute.sourcegood = g;
                                    newRoute.returngood = rg;

                                    newRoute.multipliersellsource = GetMinimum(complement.port.complementary_distances[g]);
                                    newRoute.multiplierbuysource = GetMinimum(source.port.complementary_distances[g]);
                                    newRoute.multiplierbuyreturn = GetMinimum(complement.port.complementary_distances[rg]);
                                    newRoute.multipliersellreturn = GetMinimum(source.port.complementary_distances[rg]);

                                    tradeRoutes.Add(newRoute);
                                    if (newRoute.returngood == 0)
                                        threeWayPartialRoutes.Add(newRoute);
                                }
                            }
                        }
                    }
                }
            }

            endTime = DateTime.Now;
            generateRoutesTime = endTime - startTime;

            Result.Text = "Generating three-way routes";
            Result.Invalidate();
            startTime = DateTime.Now;

            if (ThreeGoodRoutesAllowed)
            {
                Route firstRoute;
                Route secondRoute;
                Route thirdRoute;

                int count = threeWayPartialRoutes.Count;

                for (int i = 0; i < count; i++)
                    for (int j = 0; j < count; j++)
                    {
                        firstRoute = (Route)threeWayPartialRoutes[i];
                        secondRoute = (Route)threeWayPartialRoutes[j];

                        if (Convert.ToInt16(firstRoute.sectors[firstRoute.sectors.Count - 1]) != Convert.ToInt16(secondRoute.sectors[0]))
                            continue;

                        if (i == j)
                            continue;

                        for (int k = 0; k < count; k++)
                        {
                            if (i == k || j == k)
                                continue;

                            if(progressBar1.Value < progressBar1.Maximum)
                                progressBar1.Value++;

                            Route mergedRoute = new Route(currentGame);
                            thirdRoute = (Route)threeWayPartialRoutes[k];

                            if (Convert.ToInt16(secondRoute.sectors[secondRoute.sectors.Count - 1]) == (Convert.ToInt16(thirdRoute.sectors[0])) &&
                               (Convert.ToInt16(thirdRoute.sectors[thirdRoute.sectors.Count - 1])) == (Convert.ToInt16(firstRoute.sectors[0])))
                            {
                                mergedRoute = new Route(firstRoute);
                                mergedRoute = mergedRoute.AppendRoute(secondRoute);
                                mergedRoute = mergedRoute.AppendRoute(thirdRoute);
                                mergedRoute.sourcegood = firstRoute.sourcegood;
                                mergedRoute.middlegood = secondRoute.sourcegood;
                                mergedRoute.returngood = thirdRoute.sourcegood;
                                mergedRoute.multiplierbuysource = firstRoute.multiplierbuysource;
                                mergedRoute.multipliersellsource = firstRoute.multipliersellsource;
                                mergedRoute.multiplierbuymiddle = secondRoute.multiplierbuysource;
                                mergedRoute.multipliersellmiddle = secondRoute.multipliersellsource;
                                mergedRoute.multiplierbuyreturn = thirdRoute.multiplierbuysource;
                                mergedRoute.multipliersellreturn = thirdRoute.multipliersellsource;
                                mergedRoute.AddWaypoint((Convert.ToInt16(secondRoute.sectors[0])));
                                mergedRoute.AddWaypoint((Convert.ToInt16(thirdRoute.sectors[0])));
                                tradeRoutes.Add(mergedRoute);
                            }
                            else
                                continue;
                        }
                    }
            }

            endTime = DateTime.Now;
            threeWayRoutesTime = endTime - startTime;
            startTime = DateTime.Now;

            Result.Text = "Computing cash and profit for each route";
            Result.Invalidate();
            //Compute xp and profit for each route

            Route currentRoute;
            int speed = currentGame.ship[shipSelecter.SelectedIndex].ship_speed;
            double holdsspecificxp = (Math.Round(currentGame.ship[shipSelecter.SelectedIndex].ship_cargo/30.0) + 1) * 2;
            double sourcerelations = 1000, returnrelations = 1000, middlerelations = 1000;
            int routelength = 0;

            for (int i = 0; i < tradeRoutes.Count; i++)
            {
                currentRoute = (Route)tradeRoutes[i];
                routelength = currentRoute.sectors.Count + currentRoute.warps * 4 - 1;
                start = currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[0]));
                end = currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[((Route)tradeRoutes[i]).sectors.Count - 1]));
                if(currentRoute.middlegood != 0)
                    middle = currentGame.GetSectorObject((int)currentRoute.waypoints[0]);

                sourcerelations = start.port.port_race.relations;
                returnrelations = end.port.port_race.relations;
                if (currentRoute.middlegood != 0)
                    middlerelations = middle.port.port_race.relations;

                if (useJump && currentRoute.warps <= 1 && (currentRoute.length * 2 + 4) > 34)
                {
                    routelength = 34;
                }
                else
                {
                    if (currentRoute.middlegood != 0)
                        routelength = routelength + 6;
                    else if (currentRoute.returngood != 0 && currentRoute.sourcegood != 0)
                        routelength = routelength * 2 + 4;
                    else
                        routelength = routelength * 2 + 2;
                }

                if (Convert.ToInt16(currentRoute.sectors[0]) == 255 && Convert.ToInt16(currentRoute.sectors[currentRoute.sectors.Count -1]) == 247)
                    holdsspecificxp = holdsspecificxp;

                if (holdsspecificxp == 2)
                    currentRoute.experience = 0;
                else if (currentRoute.middlegood == 0)
                {
                    currentRoute.experience = currentRoute.multiplierbuyreturn * holdsspecificxp +
                                              currentRoute.multiplierbuysource * holdsspecificxp +
                                              currentRoute.multipliersellreturn * holdsspecificxp +
                                              currentRoute.multipliersellsource * holdsspecificxp;        //totalmultipliers
                    currentRoute.experience /= routelength;
                    currentRoute.experience *= speed;
                    currentRoute.experience = Math.Round(currentRoute.experience, 0);
                }
                else
                {
                    currentRoute.experience = currentRoute.multiplierbuyreturn * holdsspecificxp +
                                              currentRoute.multiplierbuysource * holdsspecificxp +
                                              currentRoute.multipliersellreturn * holdsspecificxp +
                                              currentRoute.multipliersellsource * holdsspecificxp +
                                              currentRoute.multipliersellmiddle * holdsspecificxp +
                                              currentRoute.multiplierbuymiddle * holdsspecificxp;
                    currentRoute.experience /= routelength;
                    currentRoute.experience *= speed;
                    currentRoute.experience = Math.Round(currentRoute.experience, 0);
                }

                currentRoute.cash = -Math.Round(currentGame.ship[shipSelecter.SelectedIndex].ship_cargo * currentGame.good[currentRoute.sourcegood].good_price * 0.5 * Math.Pow(currentRoute.multiplierbuysource, 1.84) * (2 - (sourcerelations + 50) / 850) * ((sourcerelations + 350) / 1500) / 38.56 * (1 + (10 - start.port.port_level) / 50) * 1.5);
                currentRoute.cash += Math.Round(0.75 * currentGame.ship[shipSelecter.SelectedIndex].ship_cargo * currentGame.good[currentRoute.sourcegood].good_price * 0.6 * Math.Pow(currentRoute.multipliersellsource + 0.5, 1.8) * 1.5 * ((returnrelations + 350) / 8415));
                currentRoute.cash -= Math.Round(currentGame.ship[shipSelecter.SelectedIndex].ship_cargo * currentGame.good[currentRoute.returngood].good_price * 0.5 * Math.Pow(currentRoute.multiplierbuyreturn, 1.84) * (2 - (returnrelations + 50) / 850) * ((returnrelations + 350) / 1500) / 38.56 * (1 + (10 - end.port.port_level) / 50) * 1.5);
                currentRoute.cash += Math.Round(0.75 * currentGame.ship[shipSelecter.SelectedIndex].ship_cargo * currentGame.good[currentRoute.returngood].good_price * 0.6 * Math.Pow(currentRoute.multipliersellreturn + 0.5, 1.8) * 1.5 * ((sourcerelations + 350) / 8415));
                
                if (currentRoute.middlegood != 0)
                {
                    currentRoute.cash = -Math.Round(currentGame.ship[shipSelecter.SelectedIndex].ship_cargo * currentGame.good[currentRoute.sourcegood].good_price * 0.5 * Math.Pow(currentRoute.multiplierbuysource, 1.84) * (2 - (sourcerelations + 50) / 850) * ((sourcerelations + 350) / 1500) / 38.56 * (1 + (10 - start.port.port_level) / 50) * 1.5);
                    currentRoute.cash += Math.Round(0.75 * currentGame.ship[shipSelecter.SelectedIndex].ship_cargo * currentGame.good[currentRoute.sourcegood].good_price * 0.6 * Math.Pow(currentRoute.multipliersellsource + 0.5, 1.8) * 1.5 * ((middlerelations + 350) / 8415));
                    currentRoute.cash -= Math.Round(currentGame.ship[shipSelecter.SelectedIndex].ship_cargo * currentGame.good[currentRoute.middlegood].good_price * 0.5 * Math.Pow(currentRoute.multiplierbuymiddle, 1.84) * (2 - (middlerelations + 50) / 850) * ((middlerelations + 350) / 1500) / 38.56 * (1 + (10 - middle.port.port_level) / 50) * 1.5);
                    currentRoute.cash += Math.Round(0.75 * currentGame.ship[shipSelecter.SelectedIndex].ship_cargo * currentGame.good[currentRoute.middlegood].good_price * 0.6 * Math.Pow(currentRoute.multipliersellmiddle + 0.5, 1.8) * 1.5 * ((returnrelations + 350) / 8415));
                    currentRoute.cash -= Math.Round(currentGame.ship[shipSelecter.SelectedIndex].ship_cargo * currentGame.good[currentRoute.returngood].good_price * 0.5 * Math.Pow(currentRoute.multiplierbuyreturn, 1.84) * (2 - (returnrelations + 50) / 850) * ((returnrelations + 350) / 1500) / 38.56 * (1 + (10 - end.port.port_level) / 50) * 1.5);
                    currentRoute.cash += Math.Round(0.75 * currentGame.ship[shipSelecter.SelectedIndex].ship_cargo * currentGame.good[currentRoute.returngood].good_price * 0.6 * Math.Pow(currentRoute.multipliersellreturn + 0.5, 1.8) * 1.5 * ((sourcerelations + 350) / 8415));
                }
                currentRoute.cash /= routelength;
                currentRoute.cash *= speed;
                currentRoute.cash = Math.Round(currentRoute.cash, 0);

                endTime = DateTime.Now;
                cashXpTime = endTime - startTime;
            }

            Result.Text = "Sorting trade routes";

            if(currentGame.smachanged)
                SMR16.SaveData(currentGame.hostApplication.games[currentGame.hostApplication.currentGame].address, currentGame);
        }

        public int GetMinimum(ArrayList target)
        {
            int minimum = 10000;

            for (int i = 0; i < target.Count; i++)
            {
                if ((int)target[i] < minimum)
                    minimum = (int)target[i];
            }

            return minimum;
        }

        private static bool intparse(string toparse, ref int result)
        {
            try { result = int.Parse(toparse); return true; }
            catch (System.FormatException) { return false; }
        }

        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            for(int i = 1; i <= GalaxyListBox.Items.Count; i++)
            {
                GalaxyListBox.SetItemChecked(i-1, true);
                galaxiesAllowed[i] = true;
            }
        }

        private void DeselectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < GalaxyListBox.Items.Count; i++)
            {
                GalaxyListBox.SetItemChecked(i, false);
                galaxiesAllowed[i] = false;
            }
        }

        private void ChangeDisplaytoVladdy_CheckedChanged(object sender, EventArgs e)
        {
            displayType = 0;
            ReDisplay();
        }

        private void ChangeDisplayToPage_CheckedChanged(object sender, EventArgs e)
        {
            displayType = 1;
            ReDisplay();
        }

        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReSort()
        {
            DateTime startTime = DateTime.Now, endTime;
            bool founddouble = false;
            string line = "";
            int r;

            for (int i = 0; i < tradeRoutes.Count; i++)
            {
                if (sortedRoutes.Count == 0)
                {
                    sortedRoutes.Add(tradeRoutes[i]);
                    continue;
                }
                else
                {
                    for (r = 0; r < sortedRoutes.Count; r++)
                    {
                        founddouble = false;

                        //If trade route already exists, then do not add and skip to next route
                        if (((Route)sortedRoutes[r]).Equals((Route)tradeRoutes[i]))
                        {
                            founddouble = true;
                            break;
                        }
                        if((((Route)sortedRoutes[r]).ReverseRoute()).Equals((Route)tradeRoutes[i]))
                        {
                            founddouble = true;
                            break;
                        }

                        if (((Route)sortedRoutes[r]).middlegood != 0 && ((Route)tradeRoutes[i]).middlegood != 0 && Convert.ToInt16(((Route)sortedRoutes[r]).sectors[0]) == Convert.ToInt16(((Route)tradeRoutes[i]).waypoints[0]) && ((Route)sortedRoutes[r]).sourcegood == ((Route)tradeRoutes[i]).middlegood && Convert.ToInt16(((Route)sortedRoutes[r]).waypoints[0]) == Convert.ToInt16(((Route)tradeRoutes[i]).waypoints[1]) && ((Route)sortedRoutes[r]).middlegood == ((Route)tradeRoutes[i]).returngood && Convert.ToInt16(((Route)sortedRoutes[r]).waypoints[1]) == Convert.ToInt16(((Route)tradeRoutes[i]).sectors[0]) && ((Route)sortedRoutes[r]).returngood == ((Route)tradeRoutes[i]).sourcegood)
                        {
                            founddouble = true;
                            break;
                        }
                        if (((Route)sortedRoutes[r]).middlegood != 0 && ((Route)tradeRoutes[i]).middlegood != 0 && Convert.ToInt16(((Route)sortedRoutes[r]).sectors[0]) == Convert.ToInt16(((Route)tradeRoutes[i]).waypoints[1]) && ((Route)sortedRoutes[r]).sourcegood == ((Route)tradeRoutes[i]).returngood && Convert.ToInt16(((Route)sortedRoutes[r]).waypoints[0]) == Convert.ToInt16(((Route)tradeRoutes[i]).sectors[0]) && ((Route)sortedRoutes[r]).middlegood == ((Route)tradeRoutes[i]).sourcegood && Convert.ToInt16(((Route)sortedRoutes[r]).waypoints[1]) == Convert.ToInt16(((Route)tradeRoutes[i]).waypoints[0]) && ((Route)sortedRoutes[r]).returngood == ((Route)tradeRoutes[i]).middlegood)
                        {
                            founddouble = true;
                            break;
                        }
                        if (((Route)sortedRoutes[r]).middlegood != 0 && ((Route)tradeRoutes[i]).middlegood != 0 && Convert.ToInt16(((Route)sortedRoutes[r]).sectors[0]) == Convert.ToInt16(((Route)tradeRoutes[i]).sectors[1]) && ((Route)sortedRoutes[r]).sourcegood == ((Route)tradeRoutes[i]).sourcegood && Convert.ToInt16(((Route)sortedRoutes[r]).waypoints[1]) == Convert.ToInt16(((Route)tradeRoutes[i]).waypoints[0]) && ((Route)sortedRoutes[r]).middlegood == ((Route)tradeRoutes[i]).middlegood && Convert.ToInt16(((Route)sortedRoutes[r]).waypoints[0]) == Convert.ToInt16(((Route)tradeRoutes[i]).sectors[0]) && ((Route)sortedRoutes[r]).returngood == ((Route)tradeRoutes[i]).returngood)
                        {
                            founddouble = true;
                            break;
                        }
                        if (((Route)sortedRoutes[r]).middlegood != 0 && ((Route)tradeRoutes[i]).middlegood != 0 && ((Route)sortedRoutes[r]).sectors[0].Equals(((Route)tradeRoutes[i]).waypoints[0]) && ((Route)sortedRoutes[r]).waypoints[1].Equals(((Route)tradeRoutes[i]).sectors[0]) && ((Route)sortedRoutes[r]).waypoints[0].Equals(((Route)tradeRoutes[i]).waypoints[1]))
                        {
                            founddouble = true;
                            break;
                        }

                        if (goal == 1 && ((Route)tradeRoutes[i]).experience > ((Route)sortedRoutes[r]).experience)
                        {
                            sortedRoutes.Insert(r, tradeRoutes[i]);
                            if(sortedRoutes.Count > 50)
                                sortedRoutes.RemoveAt(50);
                            break;
                        }
                        if (goal == 0 && ((Route)tradeRoutes[i]).cash > ((Route)sortedRoutes[r]).cash)
                        {
                            sortedRoutes.Insert(r, tradeRoutes[i]);
                            if(sortedRoutes.Count > 50)
                                sortedRoutes.RemoveAt(50);
                            break;
                        }
                    }

                    if (r == sortedRoutes.Count && !founddouble)
                        sortedRoutes.Add(tradeRoutes[i]);
                }
            }

            endTime = DateTime.Now;
            sortRoutesTime = endTime - startTime;

            ReDisplay();
        }

        private void ReDisplay()
        {
            DateTime startTime = DateTime.Now, endTime;
            Route currentRoute;
            Sector end, start, middle = new Sector();
            string line = "", sectors = "", goods = "";
            int nroftrades = 0;

            //determine the maximum nr of routes to display
            int nrofroutes = 50;

            routeList.Items.Clear();

            //Display routes
            Result.Text = "Results found after checking a total of " + allowedPorts.Count.ToString() + " ports and " + tradeRoutes.Count.ToString() + " two-way routes:\n";
            if(OneGoodRoutesAllowed && !ThreeGoodRoutesAllowed)
                Result.Text = "Results found after checking a total of " + allowedPorts.Count.ToString() + " ports, " + threeWayPartialRoutes.Count.ToString() + " one-port routes and " + tradeRoutes.Count.ToString() + " two-port routes.\n";
            if(OneGoodRoutesAllowed && ThreeGoodRoutesAllowed)
                Result.Text = "Results found after checking a total of " + allowedPorts.Count.ToString() + " ports, " + threeWayPartialRoutes.Count.ToString() + " one-port routes, " + tradeRoutes.Count.ToString() + " two-port routes and " + Math.Pow(threeWayPartialRoutes.Count, 3).ToString() + " three-port routes.\n";
            if(!OneGoodRoutesAllowed && ThreeGoodRoutesAllowed)
                Result.Text = "Results found after checking a total of " + allowedPorts.Count.ToString() + " ports, " + tradeRoutes.Count.ToString() + " two-port routes and " + Math.Pow(threeWayPartialRoutes.Count, 3).ToString() + " three-port routes.\n";

            Result.Text += "\nDisplayed values for experience and cash are per hour for the selected ship";
            Result.Text += "\nRoute distances are given per round trip\n\n";

            routeList.Items.Add("");
            routeList.Items.Add("(A)lskant (C)reonti (H)uman (I)k'thorne (S)alvene (T)hevian (W)Q Human (N)ijarin (-)Neutral");
            routeList.Items.Add("");

            for (int i = 0; i < nrofroutes && i < sortedRoutes.Count; i++)
            {
                currentRoute = (Route)sortedRoutes[i];
                start = currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[0]));
                end = currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[currentRoute.sectors.Count - 1]));
                line = "";
                goods = "";
                sectors = "";

                nroftrades = 0;
                if (currentRoute.sourcegood != 0)
                    nroftrades += 2;
                if (currentRoute.middlegood != 0)
                    nroftrades += 2;
                if (currentRoute.returngood != 0)
                    nroftrades += 2;

                if (currentRoute.middlegood != 0)
                {
                    middle = currentGame.GetSectorObject((int)currentRoute.waypoints[0]);
                    end = currentGame.GetSectorObject((int)currentRoute.waypoints[1]);
                }

                //Check if race is still allowed
                if (!racesAllowed[currentGame.GetRaceIndex(start.port.port_race.race_name)] || !racesAllowed[currentGame.GetRaceIndex(end.port.port_race.race_name)])
                    continue;

                line += (currentRoute.experience.ToString() + "e ").PadRight(8, ' ') + (currentRoute.cash.ToString() + "$  ").PadRight(11, ' ');
                if (currentRoute.length > 15 && useJump && currentRoute.middlegood == 0)
                {
                    line += (15 + "d");

                    if (currentRoute.warps >= 1)
                        line += " (" + currentRoute.warps.ToString() + "w)";
                }
                else if (currentRoute.middlegood != 0)
                {
                    line += (currentRoute.GetFirstLength().ToString() + "x" + currentRoute.GetSecondLength().ToString() + "x" + currentRoute.GetThirdLength().ToString() + "d");
                }
                else
                {
                    line += ((currentRoute.sectors.Count - 1).ToString() + "d");

                    if (currentRoute.warps >= 1)
                        line += "(" + currentRoute.warps.ToString() + "w)";
                }

                line = line.PadRight(28, ' ');
                line += " - ";

                if (currentRoute.middlegood != 0)
                {
                    sectors = (currentGame.GetRaceLetter(start.port.port_race.race_name) + " " + start.sector_id).PadRight(9, ' ') +
                                    (currentGame.GetRaceLetter(middle.port.port_race.race_name) + " " + middle.sector_id).ToString().PadRight(9, ' ') +
                                    (currentGame.GetRaceLetter(end.port.port_race.race_name) + " " + end.sector_id).PadRight(9, ' ');
                }
                else
                {
                    sectors += (currentGame.GetRaceLetter(start.port.port_race.race_name) + " " + start.sector_id).PadRight(9, ' ') +
                                    (currentGame.GetRaceLetter(end.port.port_race.race_name) + " " + end.sector_id).PadRight(9, ' ');
                }
                
                line += sectors.PadRight(27, ' ');

                if (currentRoute.middlegood != 0)
                {
                    goods = (currentGame.good[currentRoute.sourcegood].good_name).PadRight(15, ' ') + " \\ " +
                            (currentGame.good[currentRoute.middlegood].good_name).PadRight(15, ' ') + " \\ " +
                            (currentGame.good[currentRoute.returngood].good_name).PadRight(15, ' ');
                }
                else
                {
                    goods = (currentGame.good[currentRoute.sourcegood].good_name).PadRight(15, ' ') + " \\ " +
                            (currentGame.good[currentRoute.returngood].good_name).PadRight(15, ' ');
                }

                line += goods.PadRight(52, ' ') + " : ";

                if (currentRoute.middlegood != 0)
                {
                    line += currentRoute.multipliersellsource.ToString() + "x" +
                            currentRoute.multiplierbuysource.ToString() + " " +
                            currentRoute.multipliersellmiddle.ToString() + "x" +
                            currentRoute.multiplierbuymiddle.ToString() + " " +
                            currentRoute.multipliersellreturn.ToString() + "x" +
                            currentRoute.multiplierbuyreturn.ToString();
                }
                else
                {
                    line += currentRoute.multipliersellsource.ToString() + "x" +
                            currentRoute.multiplierbuysource.ToString() + " " +
                            currentRoute.multipliersellreturn.ToString() + "x" +
                            currentRoute.multiplierbuyreturn.ToString();
                }

                routeList.Items.Add(line);
                Result.Text += line + "\n";
            }

            endTime = DateTime.Now;
            displayRoutesTime = endTime - startTime;

            Result.Text += "\n";
            Result.Text += "Calculating allowed ports took " + allowedPortsTime.Milliseconds + " Milliseconds\n";
            Result.Text += "Calculating complementary sectors took " + complementarySectorTime.TotalMilliseconds + " Milliseconds\n";
            Result.Text += "Calculating routes took " + generateRoutesTime.TotalMilliseconds + " Milliseconds\n";
            Result.Text += "Calculating three-way routes took " + threeWayRoutesTime.TotalMilliseconds + " Milliseconds\n";
            Result.Text += "Sorting routes took " + sortRoutesTime.TotalMilliseconds + " Milliseconds\n";
            Result.Text += "Displaying routes took " + displayRoutesTime.TotalMilliseconds + " Milliseconds\n";
        }

        private void jumpYesRadio_CheckedChanged(object sender, EventArgs e)
        {
            useJump = true;
            ReSort();
        }

        private void jumpNoRadio_CheckedChanged(object sender, EventArgs e)
        {
            useJump = false;
            ReSort();
        }

        private void Allow1GoodRoutesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OneGoodRoutesAllowed = !OneGoodRoutesAllowed;
        }

        private void Allow3GoodRoutesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ThreeGoodRoutesAllowed = !ThreeGoodRoutesAllowed;
            if (ThreeGoodRoutesAllowed)
                AllowedRatioTextBox.Text = "3";
            else
                AllowedRatioTextBox.Text = "1.2";
        }

        private void routeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int routenr;
            Sector end, start;

            if (routeList.SelectedIndex < 3)
            {
                Result.Text = "No route selected";
                return;
            }

            routenr = routeList.SelectedIndex - 3;
            Route currentRoute = (Route) sortedRoutes[routenr];
            start = currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[0]));
            end = currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[currentRoute.sectors.Count - 1]));
            while (!racesAllowed[currentGame.GetRaceIndex(start.port.port_race.race_name)] || !racesAllowed[currentGame.GetRaceIndex(end.port.port_race.race_name)])
            {
                routenr++;
                currentRoute = (Route)sortedRoutes[routenr];
                start = currentGame.GetSectorObject((int)currentRoute.sectors[0]);
                end = currentGame.GetSectorObject((int)currentRoute.sectors[currentRoute.sectors.Count - 1]);
            }

            Result.Text = "";
            Result.Text += "Experience: " + ((int)(currentRoute.experience / currentGame.ship[shipSelecter.SelectedIndex].ship_speed)).ToString().PadRight(7, ' ') + " Money: $ " + ((int)currentRoute.cash / currentGame.ship[shipSelecter.SelectedIndex].ship_speed).ToString().PadRight(9, ' ') + "per turn\n";
            Result.Text += "Experience: " + currentRoute.experience.ToString().PadRight(7, ' ') + " Money: $ " + currentRoute.cash.ToString().PadRight(9, ' ') + "per hour,  \n";
            //Result.Text += "Experience: " + (currentRoute.experience * (4000/currentGame.ship[shipSelecter.SelectedIndex].ship_cargo)).ToString().PadRight(7, ' ') + " Money: $ " + (currentRoute.cash * (4000/currentGame.ship[shipSelecter.SelectedIndex].ship_cargo)).ToString().PadRight(9, ' ') + "per full port,  \n";
            Result.Text += "\n";

            if (currentRoute.middlegood == 0)
            {
                Result.Text += "Route: \n";
                Result.Text += "Buy " + currentGame.good[currentRoute.sourcegood].good_name + " in sector " + currentRoute.sectors[0].ToString() + " (" + currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[0])).port.port_race.race_name + ") at " + currentRoute.multiplierbuysource.ToString() + "x and sell it in sector " + currentRoute.sectors[currentRoute.sectors.Count - 1].ToString() + " (" + currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[currentRoute.sectors.Count - 1])).port.port_race.race_name + ") at " + currentRoute.multipliersellsource.ToString() + "x\n";
                Result.Text += "And on the way back buy " + currentGame.good[currentRoute.returngood].good_name + " in sector " + currentRoute.sectors[currentRoute.sectors.Count - 1].ToString() + " (" + currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[currentRoute.sectors.Count - 1])).port.port_race.race_name + ") at " + currentRoute.multiplierbuyreturn.ToString() + "x and sell it in sector " + currentRoute.sectors[0].ToString() + " (" + currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[0])).port.port_race.race_name + ") at " + currentRoute.multipliersellreturn.ToString() + "x\n";
            }
            else
            {
                Result.Text += "Route: \n";
                Result.Text += "Buy " + currentGame.good[currentRoute.sourcegood].good_name + " in sector " + currentRoute.sectors[0].ToString() + " (" + currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[0])).port.port_race.race_name + ") at " + currentRoute.multipliersellsource.ToString() + "x and sell it in sector " + currentRoute.waypoints[0].ToString() + " (" + currentGame.GetSectorObject(Convert.ToInt16(currentRoute.waypoints[0])).port.port_race.race_name + ") at " + currentRoute.multiplierbuysource.ToString() + "x\n";
                Result.Text += "Then, buy " + currentGame.good[currentRoute.middlegood].good_name + " in sector " + currentRoute.waypoints[0].ToString() + " (" + currentGame.GetSectorObject(Convert.ToInt16(currentRoute.waypoints[0])).port.port_race.race_name + ") at " + currentRoute.multipliersellmiddle.ToString() + "x and sell it in sector " + currentRoute.waypoints[1].ToString() + " (" + currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[currentRoute.sectors.Count - 1])).port.port_race.race_name + ") at " + currentRoute.multiplierbuymiddle.ToString() + "x\n";
                Result.Text += "Finally, buy " + currentGame.good[currentRoute.returngood].good_name + " in sector " + currentRoute.waypoints[1].ToString() + " (" + currentGame.GetSectorObject(Convert.ToInt16(currentRoute.waypoints[1])).port.port_race.race_name + ") at " + currentRoute.multipliersellreturn.ToString() + "x and sell it in sector " + currentRoute.sectors[0].ToString() + " (" + currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[0])).port.port_race.race_name + ") at " + currentRoute.multiplierbuyreturn.ToString() + "x\n";
            }
            
        }

        private void HighlightRouteButton_Click(object sender, EventArgs e)
        {
            Game currentGame = hostApplication.games[hostApplication.currentGame];

            if (currentGame.highlightedRoute != null)
            {
                int length = currentGame.highlightedRoute.length;
                for (int s = 0; s < currentGame.highlightedRoute.sectors.Count; s++)
                    hostApplication.games[hostApplication.currentGame].GetSectorObject(Convert.ToInt16(currentGame.highlightedRoute.sectors[s])).highlighted = 0;
            }

            if (routeList.SelectedIndex < 3 || sortedRoutes[routeList.SelectedIndex - 3] == null)
            {
                MessageBox.Show("No route has been selected yet");
                return;
            }

            Route currentRoute = (Route)sortedRoutes[routeList.SelectedIndex - 3];

            for (int s = 0; s < currentRoute.sectors.Count; s++)
                currentGame.GetSectorObject(Convert.ToInt16(currentRoute.sectors[s])).highlighted = 30 + ((double)s) / ((double)currentRoute.length) * 50;

            for (int w = 0; w < currentRoute.waypoints.Count; w++)
                currentGame.GetSectorObject((int)currentRoute.waypoints[w]).highlighted = 100;

            currentGame.highlightedRoute = currentRoute;

            if (routeList.SelectedIndex < 3 || sortedRoutes[routeList.SelectedIndex - 3] == null)
            {
                MessageBox.Show("No route has been selected yet");
                return;
            }

            int sectornr = Convert.ToInt16(currentRoute.sectors[0]);
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
            hostApplication.toolBarButton7.Pushed = false;
        }

        private void RouteFinder_Load(object sender, EventArgs e)
        {

        }

        private void RoutesThroughSectorButton_Click(object sender, EventArgs e)
        {
            int sectorToCompareWith, galaxynr;
            bool hasSector = false;

            if(!int.TryParse(sectorField.Text, out sectorToCompareWith))
            {
                MessageBox.Show("Sector could not be read");
                return;
            }

            galaxynr = currentGame.GetGalaxyIndex(sectorToCompareWith);
            if (galaxynr == -1)
            {
                MessageBox.Show("Not a valid sector for the currently loaded game!");
                return;
            }

            ComputeRoutes();

            for (int i = 0; i < tradeRoutes.Count; i++)
            {
                hasSector = false;

                for(int j = 0; j < ((Route)tradeRoutes[i]).sectors.Count; j++)
                {
                    if (Convert.ToInt16(((Route)tradeRoutes[i]).sectors[j]) == sectorToCompareWith)
                    {
                        hasSector = true;
                    }
                }

                if(!hasSector)
                    tradeRoutes.RemoveAt(i);
            }

            ReSort();
        }

        private void shipSelecter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void relationField_TextChanged(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AdjustRelationsButton_Click(object sender, EventArgs e)
        {
            int newrelation;

            for (int r = 1; r <= currentGame.nrofraces; r++)
            {
                if (!int.TryParse(((System.Windows.Forms.TextBox)relationFields[r]).Text, out newrelation))
                {
                    MessageBox.Show("Relation number could not be read");
                    return;
                }

                int x = Convert.ToInt16(((((System.Windows.Forms.TextBox)relationFields[r]).Name).Substring(4)));
                if (x < -2000)
                    x = -2000;
                if (x > 2000)
                    x = 2000;

                currentGame.race[x].relations = newrelation;
                ((System.Windows.Forms.TextBox)relationFields[r]).Text = newrelation.ToString();
            }
        }
    }
}
