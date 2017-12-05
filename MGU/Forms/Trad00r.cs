/* Copyright 2004 - 2005 Maarten Lankhorst, MGU
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

namespace MGU
{
	public class Trad00r : System.Windows.Forms.Form
	{
        delegate int SetSelectedIndexCallback();
        delegate void SetRtfCallback(string rtf);
        delegate void SetEnabledCallback();
        delegate bool SetRadioButtonCheckedCallback();

		protected System.Windows.Forms.Button CalculateTrigger;
        private System.Windows.Forms.Button MyCancelButton;
        public ComboBox comboBox1;
		private System.Windows.Forms.Label GalLBL;
		private System.Windows.Forms.Label GoodsL;
		private System.Windows.Forms.TextBox Multipliers;
		private System.Windows.Forms.RadioButton ExpRadio;
		private System.Windows.Forms.RadioButton CashRadio;
		private System.Windows.Forms.RadioButton CustomRadio;
		private System.Windows.Forms.TextBox MaxDist;
		private System.Windows.Forms.Label MPLabel;
		private System.Windows.Forms.Label DistLabel;
		private System.Windows.Forms.Label TotalLabel;
		private System.Windows.Forms.TextBox AmountOfRoutes;
		private System.Windows.Forms.Label LabelRoutes;
		private System.Windows.Forms.Label TypeLabel;
		private System.Windows.Forms.RichTextBox Results;
		private System.ComponentModel.IContainer components;
		private portlist MyPorts;
		private int lastgal;
		private bool[] allowed;
		protected System.Windows.Forms.Button button13;
		private bool _destroyed = false;
		private System.Windows.Forms.ImageList imageList3;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ImageList imageList2;
		private ToolbarControl.PictureBar userControl11;
		private ToolbarControl.PictureBar pictureBar1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton CashD;
		private System.Windows.Forms.RadioButton ExpD;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button Neutral;
		private System.Windows.Forms.Button Alskant;
		private System.Windows.Forms.Button Creonti;
		private System.Windows.Forms.Button Human;
		private System.Windows.Forms.Button IkThorne;
		private System.Windows.Forms.Button Salvene;
		private System.Windows.Forms.Button Thevian;
		private System.Windows.Forms.Button WQHuman;
		private System.Windows.Forms.Button Nijarin;
		private System.Windows.Forms.Button Unknown;
		private bool _userfriendly = true;
		private bool[] allowedraces;
		private System.Threading.Mutex mutex = new Mutex();
        private Panel pnlGoodType;
        private RadioButton rdoNeither;
        private RadioButton rdoEvil;
        private RadioButton rdoFed;
		private static Thread BGU = null;
        private Label label3;
        private Panel panel2;
        private Label label2;
        private RadioButton rdoVladdy;
        private RadioButton rdoPage;
        private bool loading = true;

		public bool Userfriendly
		{
			get { return _userfriendly; }
			set 
			{
					_userfriendly = value;
				if (value)
				{
					MPLabel.Text = "Port:";
					Multipliers.Text = "";
					//ExpD.Visible = true;
					//CashD.Visible = true;
				}
				else
				{
					MPLabel.Text = "Multipliers:";
					Multipliers.Text = "";
					//ExpD.Visible = false;
					//CashD.Visible = false;
				}
			}
		}

		public bool destroyed
		{
			get { return _destroyed; }
		}

		public Trad00r()
		{
            StandardInit();
            rdoFed.Checked = false;
            rdoEvil.Checked = false;


            rdoVladdy.Checked = Global.s.RouteDisplayStyle;
            rdoPage.Checked = !Global.s.RouteDisplayStyle;

            loading = false;
            rdoNeither.Checked = true;
		}

		public Trad00r(int galaxy, bool illegals)
		{
			StandardInit();
			if (!illegals)
			{
                foreach (goodEntry good in Database.goods)
                {
                    if (good.restriction < 0)
                        pictureBar1_Clicked(good.name);
                }
            }

            rdoFed.Checked = !illegals;
            rdoEvil.Checked = illegals;
            rdoNeither.Checked = false;

            if (illegals)
            {
                foreach (goodEntry good in Database.goods)
                {
                    if (good.restriction > 0)
                        pictureBar1_Clicked(good.name);
                }
            }
            comboBox1.SelectedIndex = galaxy + 1;

            rdoVladdy.Checked = Global.s.RouteDisplayStyle;
            rdoPage.Checked = !Global.s.RouteDisplayStyle;

            loading = false;
        }

        public Trad00r(int galaxy, bool illegals, bool federals)
        {
            StandardInit();
            if (!illegals)
            {
                foreach (goodEntry good in Database.goods)
                {
                    if (good.restriction < 0)
                        pictureBar1_Clicked(good.name);
                }
            }

            if (!federals)
            {
                foreach (goodEntry good in Database.goods)
                {
                    if (good.restriction > 0)
                        pictureBar1_Clicked(good.name);
                }
            }

            rdoVladdy.Checked = Global.s.RouteDisplayStyle;
            rdoPage.Checked = !Global.s.RouteDisplayStyle;

            if (illegals == federals)
            {
                rdoFed.Checked = false;
                rdoEvil.Checked = false;
                loading = false;
                rdoNeither.Checked = true;
                if (federals)
                {
                    string msg;
                    msg = "Federal & Illegals goods were both selected.";
                    msg += "\nBecause these goods are mutually exclusive, they have both been deselected.";
                    MessageBox.Show(msg);
                }
            }
            else
            {
                rdoFed.Checked = federals;
                rdoEvil.Checked = illegals;
                rdoNeither.Checked = false;
                loading = false;
            }

            comboBox1.SelectedIndex = galaxy + 1;
        }

		protected void StandardInit()
		{
			InitializeComponent();
			this.Results.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang0{\\fonttbl{\\f0\\fswiss\\fcharset0 Lucida Console;}}\\viewkind4\\uc1\\pard\\i\\f0\\fs20 Press the button to calculate routes.";
			ReInitialize();
			this.button13.Visible = false;
            allowed = new bool[Database.goods.Length];

            string path = Environment.CurrentDirectory + "\\goods\\mgu";

            for (int x = 0; x < Database.goods.Length; x += 1)
            {
                if (Database.goods[x].unused)
                {
                    allowed[x] = false;
                    continue;
                }
                else
                    allowed[x] = true;

                if (System.IO.File.Exists(path + Database.goods[x].id + ".gif"))
                {
                    System.Drawing.Image img;
                    try
                    {
                        img = System.Drawing.Image.FromFile(path + Database.goods[x].id + ".gif");
                    }
                    catch (Exception)
                    {
                        img = imageList2.Images[0];
                    }

                    try
                    {
                        imageList1.Images.Add(Database.goods[x].name, img);
                    }
                    catch (Exception)
                    {
                        // The most likely cause for an exception would be:
                        // The user for some ungodly reason modified the database.ini file,
                        // and put the same good name in twice (excluding 'unused' goods);
                    }
                }
            }
			allowedraces = new bool[10];
			for (int x = 0; x < 10; x += 1)
				allowedraces[x] = true;
		}

		public void ReInitialize()
		{
			if (everything.isloaded())
			{
				comboBox1.Items.Clear();
				comboBox1.Items.Add("Every galaxy");
				string[] genome = everything.getgalaxynames();
				foreach (string dna in genome)
					comboBox1.Items.Add(dna);
				comboBox1.SelectedIndex = 0;
				lastgal = -1;
			}
			else
			{
				CalculateTrigger.Enabled = false;
				comboBox1.Enabled = false;
            }

            rdoVladdy.Checked = Global.s.RouteDisplayStyle;
            rdoPage.Checked = !Global.s.RouteDisplayStyle;
		}

		public void Rehash()
		{
			lastgal = -1;
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
			if (BGU != null)
				BGU.Abort();
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		protected void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Trad00r));
            ToolbarControl.PictureBar.ColorsTemp colorsTemp3 = new ToolbarControl.PictureBar.ColorsTemp();
            ToolbarControl.PictureBar.ColorsTemp colorsTemp1 = new ToolbarControl.PictureBar.ColorsTemp();
            this.CalculateTrigger = new System.Windows.Forms.Button();
            this.Results = new System.Windows.Forms.RichTextBox();
            this.MyCancelButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.GalLBL = new System.Windows.Forms.Label();
            this.GoodsL = new System.Windows.Forms.Label();
            this.Multipliers = new System.Windows.Forms.TextBox();
            this.ExpRadio = new System.Windows.Forms.RadioButton();
            this.CashRadio = new System.Windows.Forms.RadioButton();
            this.CustomRadio = new System.Windows.Forms.RadioButton();
            this.MaxDist = new System.Windows.Forms.TextBox();
            this.MPLabel = new System.Windows.Forms.Label();
            this.DistLabel = new System.Windows.Forms.Label();
            this.TotalLabel = new System.Windows.Forms.Label();
            this.AmountOfRoutes = new System.Windows.Forms.TextBox();
            this.LabelRoutes = new System.Windows.Forms.Label();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.button13 = new System.Windows.Forms.Button();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.CashD = new System.Windows.Forms.RadioButton();
            this.ExpD = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.Neutral = new System.Windows.Forms.Button();
            this.Alskant = new System.Windows.Forms.Button();
            this.Creonti = new System.Windows.Forms.Button();
            this.Human = new System.Windows.Forms.Button();
            this.IkThorne = new System.Windows.Forms.Button();
            this.Salvene = new System.Windows.Forms.Button();
            this.Thevian = new System.Windows.Forms.Button();
            this.WQHuman = new System.Windows.Forms.Button();
            this.Nijarin = new System.Windows.Forms.Button();
            this.Unknown = new System.Windows.Forms.Button();
            this.pnlGoodType = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.rdoNeither = new System.Windows.Forms.RadioButton();
            this.rdoEvil = new System.Windows.Forms.RadioButton();
            this.rdoFed = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoVladdy = new System.Windows.Forms.RadioButton();
            this.rdoPage = new System.Windows.Forms.RadioButton();
            this.pictureBar1 = new ToolbarControl.PictureBar();
            this.userControl11 = new ToolbarControl.PictureBar();
            this.panel1.SuspendLayout();
            this.pnlGoodType.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CalculateTrigger
            // 
            this.CalculateTrigger.Location = new System.Drawing.Point(519, 537);
            this.CalculateTrigger.Name = "CalculateTrigger";
            this.CalculateTrigger.Size = new System.Drawing.Size(80, 24);
            this.CalculateTrigger.TabIndex = 1;
            this.CalculateTrigger.Text = "Find routes";
            this.CalculateTrigger.Click += new System.EventHandler(this.CalculateTrigger_Click);
            // 
            // Results
            // 
            this.Results.BackColor = System.Drawing.SystemColors.Control;
            this.Results.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Results.Location = new System.Drawing.Point(7, 189);
            this.Results.Name = "Results";
            this.Results.ReadOnly = true;
            this.Results.Size = new System.Drawing.Size(813, 342);
            this.Results.TabIndex = 2;
            this.Results.Text = "Press the button to calculate routes.";
            // 
            // MyCancelButton
            // 
            this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MyCancelButton.Location = new System.Drawing.Point(611, 537);
            this.MyCancelButton.Name = "MyCancelButton";
            this.MyCancelButton.Size = new System.Drawing.Size(80, 24);
            this.MyCancelButton.TabIndex = 17;
            this.MyCancelButton.Text = "Close";
            this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new System.Drawing.Point(88, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(124, 21);
            this.comboBox1.TabIndex = 18;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // GalLBL
            // 
            this.GalLBL.Location = new System.Drawing.Point(4, 8);
            this.GalLBL.Name = "GalLBL";
            this.GalLBL.Size = new System.Drawing.Size(80, 16);
            this.GalLBL.TabIndex = 19;
            this.GalLBL.Text = "Galaxy:";
            this.GalLBL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GoodsL
            // 
            this.GoodsL.Location = new System.Drawing.Point(4, 133);
            this.GoodsL.Name = "GoodsL";
            this.GoodsL.Size = new System.Drawing.Size(80, 20);
            this.GoodsL.TabIndex = 23;
            this.GoodsL.Text = "Goods:";
            this.GoodsL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Multipliers
            // 
            this.Multipliers.Location = new System.Drawing.Point(88, 64);
            this.Multipliers.MaxLength = 13;
            this.Multipliers.Name = "Multipliers";
            this.Multipliers.ReadOnly = true;
            this.Multipliers.Size = new System.Drawing.Size(88, 20);
            this.Multipliers.TabIndex = 24;
            this.Multipliers.Text = "3x3x 3x3x";
            // 
            // ExpRadio
            // 
            this.ExpRadio.Checked = true;
            this.ExpRadio.Location = new System.Drawing.Point(88, 36);
            this.ExpRadio.Name = "ExpRadio";
            this.ExpRadio.Size = new System.Drawing.Size(124, 16);
            this.ExpRadio.TabIndex = 25;
            this.ExpRadio.TabStop = true;
            this.ExpRadio.Text = "Top Experience";
            this.ExpRadio.CheckedChanged += new System.EventHandler(this.Deactivate_Multipliers);
            // 
            // CashRadio
            // 
            this.CashRadio.Location = new System.Drawing.Point(256, 36);
            this.CashRadio.Name = "CashRadio";
            this.CashRadio.Size = new System.Drawing.Size(156, 16);
            this.CashRadio.TabIndex = 26;
            this.CashRadio.Text = "Top Profit (INACCURATE)";
            this.CashRadio.CheckedChanged += new System.EventHandler(this.Deactivate_Multipliers);
            // 
            // CustomRadio
            // 
            this.CustomRadio.Location = new System.Drawing.Point(496, 36);
            this.CustomRadio.Name = "CustomRadio";
            this.CustomRadio.Size = new System.Drawing.Size(124, 16);
            this.CustomRadio.TabIndex = 27;
            this.CustomRadio.Text = "Defined below:";
            this.CustomRadio.CheckedChanged += new System.EventHandler(this.CustomRadio_CheckedChanged);
            // 
            // MaxDist
            // 
            this.MaxDist.Location = new System.Drawing.Point(364, 64);
            this.MaxDist.MaxLength = 2;
            this.MaxDist.Name = "MaxDist";
            this.MaxDist.ReadOnly = true;
            this.MaxDist.Size = new System.Drawing.Size(40, 20);
            this.MaxDist.TabIndex = 28;
            this.MaxDist.Text = "10";
            // 
            // MPLabel
            // 
            this.MPLabel.Location = new System.Drawing.Point(4, 68);
            this.MPLabel.Name = "MPLabel";
            this.MPLabel.Size = new System.Drawing.Size(80, 16);
            this.MPLabel.TabIndex = 29;
            this.MPLabel.Text = "Multipliers:";
            this.MPLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DistLabel
            // 
            this.DistLabel.Location = new System.Drawing.Point(256, 68);
            this.DistLabel.Name = "DistLabel";
            this.DistLabel.Size = new System.Drawing.Size(104, 16);
            this.DistLabel.TabIndex = 30;
            this.DistLabel.Text = "Maximum Distance:";
            // 
            // TotalLabel
            // 
            this.TotalLabel.Location = new System.Drawing.Point(256, 8);
            this.TotalLabel.Name = "TotalLabel";
            this.TotalLabel.Size = new System.Drawing.Size(60, 16);
            this.TotalLabel.TabIndex = 31;
            this.TotalLabel.Text = "Show best";
            // 
            // AmountOfRoutes
            // 
            this.AmountOfRoutes.Location = new System.Drawing.Point(320, 4);
            this.AmountOfRoutes.MaxLength = 3;
            this.AmountOfRoutes.Name = "AmountOfRoutes";
            this.AmountOfRoutes.Size = new System.Drawing.Size(36, 20);
            this.AmountOfRoutes.TabIndex = 32;
            this.AmountOfRoutes.Text = "30";
            // 
            // LabelRoutes
            // 
            this.LabelRoutes.Location = new System.Drawing.Point(360, 8);
            this.LabelRoutes.Name = "LabelRoutes";
            this.LabelRoutes.Size = new System.Drawing.Size(40, 16);
            this.LabelRoutes.TabIndex = 33;
            this.LabelRoutes.Text = "routes";
            // 
            // TypeLabel
            // 
            this.TypeLabel.Location = new System.Drawing.Point(4, 32);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(80, 32);
            this.TypeLabel.TabIndex = 34;
            this.TypeLabel.Text = "Route Type:";
            this.TypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(7, 537);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(80, 24);
            this.button13.TabIndex = 35;
            this.button13.Text = "Load Maps";
            this.button13.Click += new System.EventHandler(this.LoadSMCMap);
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
            // CashD
            // 
            this.CashD.Checked = true;
            this.CashD.Enabled = false;
            this.CashD.Location = new System.Drawing.Point(4, 4);
            this.CashD.Name = "CashD";
            this.CashD.Size = new System.Drawing.Size(52, 16);
            this.CashD.TabIndex = 38;
            this.CashD.TabStop = true;
            this.CashD.Text = "Cash";
            // 
            // ExpD
            // 
            this.ExpD.Enabled = false;
            this.ExpD.Location = new System.Drawing.Point(120, 4);
            this.ExpD.Name = "ExpD";
            this.ExpD.Size = new System.Drawing.Size(80, 16);
            this.ExpD.TabIndex = 39;
            this.ExpD.Text = "Experience";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CashD);
            this.panel1.Controls.Add(this.ExpD);
            this.panel1.Location = new System.Drawing.Point(492, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 20);
            this.panel1.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 41;
            this.label1.Text = "Allowed races:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Neutral
            // 
            this.Neutral.BackColor = System.Drawing.Color.PaleGreen;
            this.Neutral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Neutral.Location = new System.Drawing.Point(556, 165);
            this.Neutral.Name = "Neutral";
            this.Neutral.Size = new System.Drawing.Size(68, 20);
            this.Neutral.TabIndex = 43;
            this.Neutral.Text = "Neutral";
            this.Neutral.UseVisualStyleBackColor = false;
            this.Neutral.Click += new System.EventHandler(this.Pressy);
            // 
            // Alskant
            // 
            this.Alskant.BackColor = System.Drawing.Color.PaleGreen;
            this.Alskant.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Alskant.Location = new System.Drawing.Point(88, 165);
            this.Alskant.Name = "Alskant";
            this.Alskant.Size = new System.Drawing.Size(52, 20);
            this.Alskant.TabIndex = 44;
            this.Alskant.Text = "Alskant";
            this.Alskant.UseVisualStyleBackColor = false;
            this.Alskant.Click += new System.EventHandler(this.Pressy);
            // 
            // Creonti
            // 
            this.Creonti.BackColor = System.Drawing.Color.PaleGreen;
            this.Creonti.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Creonti.Location = new System.Drawing.Point(140, 165);
            this.Creonti.Name = "Creonti";
            this.Creonti.Size = new System.Drawing.Size(52, 20);
            this.Creonti.TabIndex = 45;
            this.Creonti.Text = "Creonti";
            this.Creonti.UseVisualStyleBackColor = false;
            this.Creonti.Click += new System.EventHandler(this.Pressy);
            // 
            // Human
            // 
            this.Human.BackColor = System.Drawing.Color.PaleGreen;
            this.Human.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Human.Location = new System.Drawing.Point(192, 165);
            this.Human.Name = "Human";
            this.Human.Size = new System.Drawing.Size(52, 20);
            this.Human.TabIndex = 46;
            this.Human.Text = "Human";
            this.Human.UseVisualStyleBackColor = false;
            this.Human.Click += new System.EventHandler(this.Pressy);
            // 
            // IkThorne
            // 
            this.IkThorne.BackColor = System.Drawing.Color.PaleGreen;
            this.IkThorne.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IkThorne.Location = new System.Drawing.Point(244, 165);
            this.IkThorne.Name = "IkThorne";
            this.IkThorne.Size = new System.Drawing.Size(64, 20);
            this.IkThorne.TabIndex = 47;
            this.IkThorne.Text = "Ik\'Thorne";
            this.IkThorne.UseVisualStyleBackColor = false;
            this.IkThorne.Click += new System.EventHandler(this.Pressy);
            // 
            // Salvene
            // 
            this.Salvene.BackColor = System.Drawing.Color.PaleGreen;
            this.Salvene.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Salvene.Location = new System.Drawing.Point(308, 165);
            this.Salvene.Name = "Salvene";
            this.Salvene.Size = new System.Drawing.Size(56, 20);
            this.Salvene.TabIndex = 48;
            this.Salvene.Text = "Salvene";
            this.Salvene.UseVisualStyleBackColor = false;
            this.Salvene.Click += new System.EventHandler(this.Pressy);
            // 
            // Thevian
            // 
            this.Thevian.BackColor = System.Drawing.Color.PaleGreen;
            this.Thevian.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Thevian.Location = new System.Drawing.Point(364, 165);
            this.Thevian.Name = "Thevian";
            this.Thevian.Size = new System.Drawing.Size(60, 20);
            this.Thevian.TabIndex = 49;
            this.Thevian.Text = "Thevian";
            this.Thevian.UseVisualStyleBackColor = false;
            this.Thevian.Click += new System.EventHandler(this.Pressy);
            // 
            // WQHuman
            // 
            this.WQHuman.BackColor = System.Drawing.Color.PaleGreen;
            this.WQHuman.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WQHuman.Location = new System.Drawing.Point(424, 165);
            this.WQHuman.Name = "WQHuman";
            this.WQHuman.Size = new System.Drawing.Size(76, 20);
            this.WQHuman.TabIndex = 50;
            this.WQHuman.Text = "WQ Human";
            this.WQHuman.UseVisualStyleBackColor = false;
            this.WQHuman.Click += new System.EventHandler(this.Pressy);
            // 
            // Nijarin
            // 
            this.Nijarin.BackColor = System.Drawing.Color.PaleGreen;
            this.Nijarin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Nijarin.Location = new System.Drawing.Point(500, 165);
            this.Nijarin.Name = "Nijarin";
            this.Nijarin.Size = new System.Drawing.Size(56, 20);
            this.Nijarin.TabIndex = 51;
            this.Nijarin.Text = "Nijarin";
            this.Nijarin.UseVisualStyleBackColor = false;
            this.Nijarin.Click += new System.EventHandler(this.Pressy);
            // 
            // Unknown
            // 
            this.Unknown.BackColor = System.Drawing.Color.PaleGreen;
            this.Unknown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Unknown.Location = new System.Drawing.Point(624, 165);
            this.Unknown.Name = "Unknown";
            this.Unknown.Size = new System.Drawing.Size(68, 20);
            this.Unknown.TabIndex = 52;
            this.Unknown.Text = "Unknown";
            this.Unknown.UseVisualStyleBackColor = false;
            this.Unknown.Click += new System.EventHandler(this.Pressy);
            // 
            // pnlGoodType
            // 
            this.pnlGoodType.Controls.Add(this.label3);
            this.pnlGoodType.Controls.Add(this.rdoNeither);
            this.pnlGoodType.Controls.Add(this.rdoEvil);
            this.pnlGoodType.Controls.Add(this.rdoFed);
            this.pnlGoodType.Location = new System.Drawing.Point(408, 2);
            this.pnlGoodType.Name = "pnlGoodType";
            this.pnlGoodType.Size = new System.Drawing.Size(284, 28);
            this.pnlGoodType.TabIndex = 53;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "Good Type:";
            // 
            // rdoNeither
            // 
            this.rdoNeither.AutoSize = true;
            this.rdoNeither.Location = new System.Drawing.Point(185, 4);
            this.rdoNeither.Name = "rdoNeither";
            this.rdoNeither.Size = new System.Drawing.Size(59, 17);
            this.rdoNeither.TabIndex = 2;
            this.rdoNeither.TabStop = true;
            this.rdoNeither.Text = "Neither";
            this.rdoNeither.UseVisualStyleBackColor = true;
            this.rdoNeither.CheckedChanged += new System.EventHandler(this.rdoNeither_CheckedChanged);
            // 
            // rdoEvil
            // 
            this.rdoEvil.AutoSize = true;
            this.rdoEvil.Location = new System.Drawing.Point(137, 4);
            this.rdoEvil.Name = "rdoEvil";
            this.rdoEvil.Size = new System.Drawing.Size(42, 17);
            this.rdoEvil.TabIndex = 1;
            this.rdoEvil.TabStop = true;
            this.rdoEvil.Text = "Evil";
            this.rdoEvil.UseVisualStyleBackColor = true;
            this.rdoEvil.CheckedChanged += new System.EventHandler(this.rdoEvil_CheckedChanged);
            // 
            // rdoFed
            // 
            this.rdoFed.AutoSize = true;
            this.rdoFed.Location = new System.Drawing.Point(88, 4);
            this.rdoFed.Name = "rdoFed";
            this.rdoFed.Size = new System.Drawing.Size(43, 17);
            this.rdoFed.TabIndex = 0;
            this.rdoFed.TabStop = true;
            this.rdoFed.Text = "Fed";
            this.rdoFed.UseVisualStyleBackColor = true;
            this.rdoFed.CheckedChanged += new System.EventHandler(this.rdoFed_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.rdoVladdy);
            this.panel2.Controls.Add(this.rdoPage);
            this.panel2.Location = new System.Drawing.Point(32, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(212, 29);
            this.panel2.TabIndex = 54;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Display Type:";
            // 
            // rdoVladdy
            // 
            this.rdoVladdy.Location = new System.Drawing.Point(80, 3);
            this.rdoVladdy.Name = "rdoVladdy";
            this.rdoVladdy.Size = new System.Drawing.Size(66, 23);
            this.rdoVladdy.TabIndex = 38;
            this.rdoVladdy.Text = "Vladdy\'s";
            this.rdoVladdy.CheckedChanged += new System.EventHandler(this.rdoVladdy_CheckedChanged);
            // 
            // rdoPage
            // 
            this.rdoPage.Checked = true;
            this.rdoPage.Location = new System.Drawing.Point(152, 3);
            this.rdoPage.Name = "rdoPage";
            this.rdoPage.Size = new System.Drawing.Size(57, 23);
            this.rdoPage.TabIndex = 39;
            this.rdoPage.TabStop = true;
            this.rdoPage.Text = "Page\'s";
            this.rdoPage.CheckedChanged += new System.EventHandler(this.rdoPage_CheckedChanged);
            // 
            // pictureBar1
            // 
            this.pictureBar1.BackgroundColorDown = System.Drawing.Color.MistyRose;
            this.pictureBar1.BackgroundColorNormal = System.Drawing.Color.WhiteSmoke;
            this.pictureBar1.BackgroundColorOver = System.Drawing.Color.FloralWhite;
            this.pictureBar1.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.pictureBar1.ImageList = this.imageList1;
            this.pictureBar1.LineColorDown = System.Drawing.Color.Black;
            this.pictureBar1.LineColorNormal = System.Drawing.Color.DimGray;
            this.pictureBar1.LineColorOver = System.Drawing.Color.Gray;
            colorsTemp3.Down = System.Drawing.Color.Black;
            colorsTemp3.Normal = System.Drawing.Color.Black;
            colorsTemp3.Over = System.Drawing.Color.Black;
            this.pictureBar1.LineColors = colorsTemp3;
            this.pictureBar1.Location = new System.Drawing.Point(92, 125);
            this.pictureBar1.Name = "pictureBar1";
            this.pictureBar1.Size = new System.Drawing.Size(728, 36);
            this.pictureBar1.TabIndex = 37;
            this.pictureBar1.Text = "pictureBar1";
            this.pictureBar1.Clicked += new ToolbarControl.PictureBar.EventHandler(this.pictureBar1_Clicked);
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
            colorsTemp1.Down = System.Drawing.Color.Black;
            colorsTemp1.Normal = System.Drawing.Color.Black;
            colorsTemp1.Over = System.Drawing.Color.Black;
            this.userControl11.LineColors = colorsTemp1;
            this.userControl11.Location = new System.Drawing.Point(88, 121);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(0, 36);
            this.userControl11.TabIndex = 36;
            // 
            // Trad00r
            // 
            this.AcceptButton = this.CalculateTrigger;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.MyCancelButton;
            this.ClientSize = new System.Drawing.Size(832, 567);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlGoodType);
            this.Controls.Add(this.Unknown);
            this.Controls.Add(this.Nijarin);
            this.Controls.Add(this.WQHuman);
            this.Controls.Add(this.Thevian);
            this.Controls.Add(this.Salvene);
            this.Controls.Add(this.IkThorne);
            this.Controls.Add(this.Human);
            this.Controls.Add(this.Creonti);
            this.Controls.Add(this.Alskant);
            this.Controls.Add(this.Neutral);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBar1);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.TypeLabel);
            this.Controls.Add(this.LabelRoutes);
            this.Controls.Add(this.AmountOfRoutes);
            this.Controls.Add(this.TotalLabel);
            this.Controls.Add(this.DistLabel);
            this.Controls.Add(this.MPLabel);
            this.Controls.Add(this.MaxDist);
            this.Controls.Add(this.CustomRadio);
            this.Controls.Add(this.CashRadio);
            this.Controls.Add(this.ExpRadio);
            this.Controls.Add(this.Multipliers);
            this.Controls.Add(this.GoodsL);
            this.Controls.Add(this.GalLBL);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.MyCancelButton);
            this.Controls.Add(this.Results);
            this.Controls.Add(this.CalculateTrigger);
            this.Controls.Add(this.userControl11);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(704, 558);
            this.Name = "Trad00r";
            this.Text = "Port Negotiator";
            this.panel1.ResumeLayout(false);
            this.pnlGoodType.ResumeLayout(false);
            this.pnlGoodType.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private string GetGoodName(int good, bool DisplayVladdys)
		{
            if (Database.goods.Length > good)
            {
                string r = "";
                    if (Database.goods[good].restriction > 0)
                        r = "\\cf3 ";
                    else if (Database.goods[good].restriction < 0)
                        r = "\\cf4 ";
                    else
                        r = "\\cf5 ";

                    if (DisplayVladdys)
                        r += Database.goods[good].name.PadRight(16);
                    else
                        r += Database.goods[good].name;

                    r += "\\cf0 ";

                return r;
            }
            return null;

            //switch (good)
            //{
            //    case 0: return  "\\cf3 Wood     \\cf0 ";
            //    case 1: return  "\\cf3 Food     \\cf0 ";
            //    case 2: return  "\\cf3 Ore      \\cf0 ";
            //    case 3: return  "\\cf3 Precious \\cf0 ";
            //    case 4: return  "\\cf4 Slaves   \\cf0 ";
            //    case 5: return  "\\cf3 Textiles \\cf0 ";
            //    case 6: return  "\\cf3 Machinery\\cf0 ";
            //    case 7: return  "\\cf3 Circuitry\\cf0 ";
            //    case 8: return  "\\cf4 Weapons  \\cf0 ";
            //    case 9: return  "\\cf3 Computers\\cf0 ";
            //    case 10: return "\\cf3 Luxury   \\cf0 ";
            //    case 11: return "\\cf4 Narcotics\\cf0 ";
            //    default: return null;
            //}
		}

		private string GetRtfRaceNameHeader(int race)
		{
            //if (everything.FileType == galFileType.SMR)
            //    race = everything.switchDefaultPortRace(race);

			switch (race)
			{
				case 0: return "\\cf1 ?\\cf0 ";
				case 1: return "\\cf2 A\\cf0 ";
				case 2: return "\\cf2 C\\cf0 ";
				case 3: return "\\cf2 H\\cf0 ";
				case 4: return "\\cf2 I\\cf0 ";
				case 5: return "\\cf2 S\\cf0 ";
				case 6: return "\\cf2 T\\cf0 ";
				case 7: return "\\cf2 W\\cf0 ";
				case 8: return "\\cf2 N\\cf0 ";
				case 9: return "\\cf1 Ñ\\cf0 ";
				default: return null;
			}
        }

        private string GetRtfRaceName(int race, bool DisplayVladdys)
        {
            if (everything.FileType == galFileType.SMR)
                race = everything.switchDefaultPortRace(race);

            if (DisplayVladdys)
            {
                switch (race)
                {
                    case 0: return "\\cf1 ?\\cf0 ";
                    case 1: return "\\cf2 A\\cf0 ";
                    case 2: return "\\cf2 C\\cf0 ";
                    case 3: return "\\cf2 H\\cf0 ";
                    case 4: return "\\cf2 I\\cf0 ";
                    case 5: return "\\cf2 S\\cf0 ";
                    case 6: return "\\cf2 T\\cf0 ";
                    case 7: return "\\cf2 W\\cf0 ";
                    case 8: return "\\cf2 N\\cf0 ";
                    case 9: return "\\cf1 Ñ\\cf0 ";
                    default: return null;
                }
            }
            else
            {
                switch (race)
                {
                    case 0: return " (Unknown) ";
                    case 1: return " (Alskant) ";
                    case 2: return " (Creonti)";
                    case 3: return " (Human) ";
                    case 4: return " (Ik Thorne) ";
                    case 5: return " (Salvene) ";
                    case 6: return " (Thevian) ";
                    case 7: return " (WQ Human) ";
                    case 8: return " (Nijarin) ";
                    case 9: return " (Neutral) ";
                    default: return null;
                }
            }
        }

		private string Indent(int port)
		{
            if (port < 10) return "    ";
            else if (port < 100) return "   ";
            else if (port < 1000) return "  ";
            else if (port < 10000) return " ";
            else return "";
		}

		private string ReturnData(allroutes thisroute, bool DisplayVladdys)
		{
			string retval;
			int cash = (int)Math.Round(thisroute.money);

            if (DisplayVladdys)
            {
                switch (thisroute.ports.Length)
                {
                    case 2:
                        retval = getexp((int)(100 * Math.Round(thisroute.exp, 2))) + "\\i e\\i0   "
                            + Indent(cash) + cash + "$ " + (thisroute.distance[0] < 10 ? " " : "")
                            + thisroute.distance[0] + "\\i d\\i0  - \\b "
                            + GetRtfRaceName(thisroute.races[0], true) + thisroute.ports[0] + Indent(thisroute.ports[0]) + " "
                            + GetRtfRaceName(thisroute.races[1], true) + thisroute.ports[1] + Indent(thisroute.ports[1])
                            + "\\b0  - \\i "
                            + GetGoodName(thisroute.goods[0], true) + "\\i0  / \\i "
                            + GetGoodName(thisroute.goods[1], true) + "\\i0  : ";
                        if (_userfriendly)
                            retval += thisroute.goodm1[0] + "x" + thisroute.goodm1[1] + "x " + thisroute.goodm2[0] + "x" + thisroute.goodm2[1] + "x";
                        else
                            retval += thisroute.goodm2[1] + "x" + thisroute.goodm1[0] + "x " + thisroute.goodm1[1] + "x" + thisroute.goodm2[0] + "x";
                        return retval;
                    case 3:
                        retval = getexp((int)(100 * Math.Round(thisroute.exp, 2))) + "\\i e\\i0   "
                            + Indent(cash) + cash + "$ " + (thisroute.distance[0] < 10 ? " " : "")
                            + thisroute.distance[0] + "\\i d\\i0  - \\b "
                            + GetRtfRaceName(thisroute.races[0], true) + thisroute.ports[0] + Indent(thisroute.ports[0]) + " "
                            + GetRtfRaceName(thisroute.races[1], true) + thisroute.ports[1] + Indent(thisroute.ports[1]) + " "
                            + GetRtfRaceName(thisroute.races[2], true) + thisroute.ports[2] + Indent(thisroute.ports[2])
                            + "\\b0  - \\i "
                            + GetGoodName(thisroute.goods[0], true) + "\\i0  / \\i "
                            + GetGoodName(thisroute.goods[1], true) + "\\i0  / \\i "
                            + GetGoodName(thisroute.goods[2], true) + "\\i0  : ";
                        if (_userfriendly)
                            retval += thisroute.goodm1[0] + "x" + thisroute.goodm1[1] + "x " + thisroute.goodm2[0] + "x" + thisroute.goodm2[1] + "x" + thisroute.goodm3[0] + "x" + thisroute.goodm3[1] + "x";
                        else
                            retval += thisroute.goodm3[1] + "x" + thisroute.goodm1[0] + "x " + thisroute.goodm1[1] + "x" + thisroute.goodm2[0] + "x " + thisroute.goodm2[1] + "x" + thisroute.goodm3[0] + "x";
                        return retval;
                }
            }
            else
            {
                retval = "Exp: " + getexp((int)(100 * Math.Round(thisroute.exp, 2)))
                    + "\tMoney: $" + cash + " \\par "
                    + "Route: " + thisroute.ports[0] + GetRtfRaceName(thisroute.races[0], false)
                    + " buy " + GetGoodName(thisroute.goods[0], false) + " at " + thisroute.goodm1[0] + "x"
                    + " to sell at (Distance: " + thisroute.distance[0] + ") "
                    + thisroute.ports[1] + GetRtfRaceName(thisroute.races[1], false) + "at "
                    + thisroute.goodm1[1] + "x \\par "
                    + thisroute.ports[1] + GetRtfRaceName(thisroute.races[1], false) + "buy "
                    + GetGoodName(thisroute.goods[1], false) + " at " + thisroute.goodm2[0] + "x to sell at (Distance: "
                    + thisroute.distance[0] + ") " + thisroute.ports[0] + GetRtfRaceName(thisroute.races[0], false)
                    + "at " + thisroute.goodm2[1] + "x \\par \\par";
                return retval;
            }
			return null;
		}

		private void CalculateTrigger_Click(object sender, System.EventArgs e)
		{
			mutex.WaitOne();
			CalculateTrigger.Enabled = false;
			BGU = new Thread(new ThreadStart(DoCalculate));
			BGU.Start();
			mutex.ReleaseMutex();
		}

        private int GetComboBox1SelectIndex()
        {
            if (this.InvokeRequired)
            {
                SetSelectedIndexCallback d = new SetSelectedIndexCallback(GetComboBox1SelectIndex);
                return (int)this.Invoke(d);
            }
            else
            {
                return this.comboBox1.SelectedIndex;
            }
        }

        private void SetResultsRtf(string rtf)
        {
            if (this.InvokeRequired)
            {
                SetRtfCallback d = new SetRtfCallback(SetResultsRtf);
                this.Invoke(d, new object[] { rtf });
            }
            else
            {
                this.Results.Rtf = rtf;
            }
        }

        private void SetCalculateTriggerEnabled()
        {
            if (this.InvokeRequired)
            {
                SetEnabledCallback d = new SetEnabledCallback(SetCalculateTriggerEnabled);
                this.Invoke(d);
            }
            else
                this.CalculateTrigger.Enabled = true;
        }

        private bool GetRadioButtonChecked()
        {
            if (this.InvokeRequired)
            {
                SetRadioButtonCheckedCallback d = new SetRadioButtonCheckedCallback(GetRadioButtonChecked);
                return (bool)this.Invoke(d);
            }
            else
            {
                return this.rdoVladdy.Checked;
            }
        }

		private void DoCalculate()
		{
			mutex.WaitOne();

            bool DisplayVladdys = GetRadioButtonChecked();

			oldstring = null;
			// wee3eee wee3eee
			// First, CALCULATE ALL PORT MULTIPLIERS in the specified galaxy;
            if (lastgal != GetComboBox1SelectIndex())
			{
				int[][] MyGal = everything.enumerate();
                if (GetComboBox1SelectIndex() == 0)
                    MyPorts = everything.xgetports(0);
                else
                    MyPorts = everything.getports(GetComboBox1SelectIndex());

				if (MyPorts == null)
				{
					SetResultsRtf("{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang0{\\fonttbl{\\f0\\fswiss\\fcharset0 Lucida Console;}}\\viewkind4\\uc1\\pard\\b\\f0\\fs20 There are no (usable) ports in this galaxy.\\par}");
					mutex.ReleaseMutex();
					return;
				}
                SetResultsRtf("{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang0{\\fonttbl{\\f0\\fswiss\\fcharset0 Lucida Console;}}\\viewkind4\\uc1\\pard\\i\\f0\\fs20 Stand by, calculating port multipliers...\\par}");
				// Now, lets calculate multipliers, we use same techniques as course plotting, except only length and we got all goods matters..
                MyPorts.galaxy_map = MyGal;
                MyPorts.CalculateGalaxy();
                lastgal = GetComboBox1SelectIndex();
				SetResultsRtf("{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang0{\\fonttbl{\\f0\\fswiss\\fcharset0 Lucida Console;}}\\viewkind4\\uc1\\pard\\i\\f0\\fs20 Multipliers calculated.. initiating possible routes search..\\par}");
			}
			else
				SetResultsRtf("{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang0{\\fonttbl{\\f0\\fswiss\\fcharset0 Lucida Console;}}\\viewkind4\\uc1\\pard\\i\\f0\\fs20 Using the cached stuff.. initiating possible routes search..\\par}");
			int maxroutes = 0;
			try 
			{
				maxroutes = int.Parse(AmountOfRoutes.Text);
			} 
			catch (System.FormatException) 
			{
				MessageBox.Show("Maximum routes is not a numeric value");
				SetResultsRtf("{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang0{\\fonttbl{\\f0\\fswiss\\fcharset0 Lucida Console;}}\\viewkind4\\uc1\\pard\\b\\f0\\fs20 Cancelled.\\par}");
			}
			if (maxroutes == 0)
				maxroutes = 30;
			MyPorts.GetAllRoutes(allowed, allowedraces);
			SetResultsRtf("{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang0{\\fonttbl{\\f0\\fswiss\\fcharset0 Lucida Console;}}\\viewkind4\\uc1\\pard\\i\\f0\\fs20 We found a total of \\ul " + MyPorts.totalroutes + "\\ulnone  routes, processing...\\par");
			int[] toproutes;
			if (ExpRadio.Checked)
				toproutes = CalculateExpRoutes(maxroutes);
			else if (CashRadio.Checked)
				toproutes = CalculateCashRoutes(maxroutes);
			else
				toproutes = CalculateCustomRoutes(maxroutes);
			string rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang0{\\fonttbl{\\f0\\fswiss\\fcharset0 Lucida Console;}}{\\colortbl;\\red127\\green0\\blue0;\\red0\\green127\\blue0;\\red0\\green63\\blue0;\\red191\\green0\\blue0;}\\viewkind4\\uc1\\pard\\i\\f0\\fs20Results found, checked a total of \\ul " + MyPorts.totalroutes + "\\ulnone  routes. \\i0 \\par ";
			if (toproutes == null || toproutes[0] == 0)
			{
				SetResultsRtf(rtf + "\\par\\b No routes found matching selected Criteria\\b0");
				mutex.ReleaseMutex();
				return;
			}
            if (DisplayVladdys)
                rtf += "\\b " + GetRtfRaceNameHeader(1) + "\\b0 lskant \\b " + GetRtfRaceNameHeader(2) + "\\b0 reonti \\b " + GetRtfRaceNameHeader(3) + "\\b0 uman \\b " + GetRtfRaceNameHeader(4) + "\\b0 k'thorne \\b " + GetRtfRaceNameHeader(5) + "\\b0 alvene \\b " + GetRtfRaceNameHeader(6) + "\\b0 hevian \\b " + GetRtfRaceNameHeader(7) + "\\b0 Q Human \\b " + GetRtfRaceNameHeader(8) + "\\b0 ijarin \\b " + GetRtfRaceNameHeader(9) + "\\b0 eutral\\b0\\par ";
			int curpointer;
			string endtext = "";
			for (curpointer = 0; curpointer < maxroutes; curpointer += 1)
			{
				if (toproutes[curpointer] == 0) break;
				endtext += "\\par ";
				endtext += ReturnData(MyPorts.myroutes[toproutes[curpointer]], DisplayVladdys);
			}
			oldstring = null;
			SetResultsRtf(rtf + endtext);
			MyPorts.myroutes = null;
			System.GC.Collect();
			mutex.ReleaseMutex();
			BGU = null;
            SetCalculateTriggerEnabled();
		}

		private string getexp (int exp)
		{
			int msec2 = exp % 10;
			int msec1 = (exp - msec2) % 100 / 10;
			int sec = (exp - msec2 - msec1) / 100;
			return sec.ToString() + "." + msec1.ToString() + msec2.ToString();
		}

		private int[] CalculateExpRoutes(int maxroutes)
		{
			int[] toproutes = new int[maxroutes];
			int curroute;
			int curpointer, altpointer;
			for (curroute = 1; curroute <= MyPorts.totalroutes; curroute += 1)
			{
				if (MyPorts.myroutes[curroute].exp > MyPorts.myroutes[toproutes[maxroutes - 1]].exp)
				{	
					for (curpointer = 0; curpointer < maxroutes; curpointer += 1)
					{
						if (MyPorts.myroutes[curroute].exp > MyPorts.myroutes[toproutes[curpointer]].exp)
						{
							for (altpointer = maxroutes - 1; altpointer > curpointer; altpointer -= 1)
							{
								toproutes[altpointer] = toproutes[altpointer - 1];
							}
							break;
						}
					}
					toproutes[curpointer] = curroute;
				}
			}
			return toproutes;
		}

		private int[] CalculateCustomRoutes(int maxroutes)
		{
			int[] toproutes = new int[maxroutes];
			int curroute;
			int curpointer, altpointer, maxdist;

			try 
			{
				maxdist = int.Parse(MaxDist.Text);
			} 
			catch (System.FormatException)
			{
				MessageBox.Show("Invalid distance supplied");
				return null;
			}

			if (!_userfriendly && Multipliers.Text.IndexOf("x ") != -1)
			{
				string[] AllPliers;
				AllPliers = this.Multipliers.Text.Split("x ".ToCharArray());
				int[] multipliers = new int[4];
				multipliers[0] = int.Parse(AllPliers[0]);
				multipliers[1] = int.Parse(AllPliers[1]);
				multipliers[2] = int.Parse(AllPliers[3]);
				multipliers[3] = int.Parse(AllPliers[4]);

				for (curroute = 1; curroute <= MyPorts.totalroutes; curroute += 1)
				{
					if (MyPorts.myroutes[curroute].distance[0] <= maxdist
						&& MyPorts.myroutes[curroute].distance[0] < MyPorts.myroutes[toproutes[maxroutes - 1]].distance[0])
					{	
						if ( !(MyPorts.myroutes[curroute].goodm1[1] == multipliers[0]
							&& MyPorts.myroutes[curroute].goodm2[0] == multipliers[1]
							&& MyPorts.myroutes[curroute].goodm2[1] == multipliers[2]
							&& MyPorts.myroutes[curroute].goodm1[0] == multipliers[3])
							&& !(MyPorts.myroutes[curroute].goodm2[1] == multipliers[0]
							&& MyPorts.myroutes[curroute].goodm1[0] == multipliers[1]
							&& MyPorts.myroutes[curroute].goodm1[1] == multipliers[2]
							&& MyPorts.myroutes[curroute].goodm2[0] == multipliers[3]))
							continue;
						for (curpointer = 0; curpointer < maxroutes; curpointer += 1)
						{
							if (MyPorts.myroutes[curroute].exp > MyPorts.myroutes[toproutes[curpointer]].exp)
							{
								for (altpointer = maxroutes - 1; altpointer > curpointer; altpointer -= 1)
								{
									toproutes[altpointer] = toproutes[altpointer - 1];
								}
								break;
							}
						}
						toproutes[curpointer] = curroute;
					}
				}
				return toproutes;
			}
			else
			{
				int port;
				try 
				{
					string parseit;
					if (this.Multipliers.Text == "")
					{
						MessageBox.Show("Please pick a port to show the routes of first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return null;
					}
					if (this.Multipliers.Text[0] == '#')
						parseit = this.Multipliers.Text.Substring(1, this.Multipliers.Text.Length - 1);
					else parseit = this.Multipliers.Text;
					port = int.Parse(parseit);
				}
				catch (System.FormatException)
				{
					MessageBox.Show("\"" + this.Multipliers.Text + "\" is not a sector");
					return null;
				}

				if (port < MyPorts.beginsector || port > MyPorts.endsector)
				{
					MessageBox.Show("This port is not in the selected galaxy!\nEven worser is I had to MAKE an error message\n for people that try it anyway!");
					return null;
				}

				if (!everything.getsectorinfo(port).gotport)
				{
					MessageBox.Show("This sector does NOT have a port");
					return null;
				}

				for (curroute = 1; curroute <= MyPorts.totalroutes; curroute += 1)
				{
					if (MyPorts.myroutes[curroute].distance[0] > maxdist ||
						(MyPorts.myroutes[curroute].ports[0] != port &&
						MyPorts.myroutes[curroute].ports[1] != port))
					{
						MyPorts.myroutes[curroute].money = -1;
						MyPorts.myroutes[curroute].exp = -1;
					}
				}
				if (ExpD.Checked)
					return CalculateExpRoutes(maxroutes);
				else return CalculateCashRoutes(maxroutes);
			}
		}

		private int[] CalculateCashRoutes(int maxroutes)
		{
			int[] toproutes = new int[maxroutes];
			int curroute;
			int curpointer, altpointer;
			for (curroute = 1; curroute <= MyPorts.totalroutes; curroute += 1)
			{
				if (MyPorts.myroutes[curroute].money > MyPorts.myroutes[toproutes[maxroutes - 1]].money)
				{	
					for (curpointer = 0; curpointer < maxroutes; curpointer += 1)
					{
						if (MyPorts.myroutes[curroute].money > MyPorts.myroutes[toproutes[curpointer]].money)
						{
							for (altpointer = maxroutes - 1; altpointer > curpointer; altpointer -= 1)
							{
								toproutes[altpointer] = toproutes[altpointer - 1];
							}
							break;
						}
					}
					toproutes[curpointer] = curroute;
				}
			}
			return toproutes;
		}

		private void MyCancelButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void Deactivate_Multipliers(object sender, System.EventArgs e)
		{
			Multipliers.ReadOnly = true;
			MaxDist.ReadOnly = true;
			ExpD.Enabled = false;
			CashD.Enabled = false;
		}

		private void CustomRadio_CheckedChanged(object sender, System.EventArgs e)
		{
			Multipliers.ReadOnly = false;
			MaxDist.ReadOnly = false;
			ExpD.Enabled = true;
			CashD.Enabled = true;
		}

		protected virtual void LoadSMCMap(object sender, System.EventArgs e)
		{
		}

        private void pictureBar1_Clicked(int selectedIndex)
		{
            int i = Database.FindGoodIndex(pictureBar1.ImageList.Images.Keys[selectedIndex]);

            if (i > -1)
            {
                // Imagelist1 is the full set of images
                // Imagelist2 has only 1 image, a cross
                // Imagelist3 is the imagelist we use to construct for the toolbar
                allowed[i] = !allowed[i];
                imageList3 = new ImageList(this.components);
                imageList3.ColorDepth = ColorDepth.Depth8Bit;
                imageList3.ImageSize = new Size(16, 16);
                imageList3.TransparentColor = Color.Transparent;
                for (int x = 0; x < Database.goods.Length; x += 1)
                {
                    if (Database.goods[x].unused) continue;
                    if (allowed[x])
                        imageList3.Images.Add(Database.goods[x].name, imageList1.Images[Database.goods[x].name]);
                    else
                    {
                        int m = imageList3.Images.Add(imageList2.Images[0], Color.White);
                        imageList3.Images.SetKeyName(m, Database.goods[x].name);
                    }
                }
                pictureBar1.ImageList = imageList3;
            }
        }

        private void pictureBar1_Clicked(string key)
        {
            int i = Database.FindGoodIndex(key);
            if (i > -1)
            {
                // Imagelist1 is the full set of images
                // Imagelist2 has only 1 image, a cross
                // Imagelist3 is the imagelist we use to construct for the toolbar
                allowed[i] = !allowed[i];
                imageList3 = new ImageList(this.components);
                imageList3.ColorDepth = ColorDepth.Depth8Bit;
                imageList3.ImageSize = new Size(16, 16);
                imageList3.TransparentColor = Color.Transparent;
                for (int x = 0; x < Database.goods.Length; x += 1)
                {
                    if (Database.goods[x].unused) continue;
                    if (allowed[x])
                        imageList3.Images.Add(Database.goods[x].name, imageList1.Images[Database.goods[x].name]);
                    else
                    {
                        int m = imageList3.Images.Add(imageList2.Images[0], Color.White);
                        imageList3.Images.SetKeyName(m, Database.goods[x].name);
                    }
                }
                pictureBar1.ImageList = imageList3;
            }
        }

		private void Pressy(object sender, System.EventArgs e)
		{
			if (((Button)(sender)).FlatStyle == FlatStyle.Flat)
			{
				((Button)(sender)).FlatStyle = FlatStyle.Standard;
				((Button)(sender)).BackColor = System.Drawing.Color.LightCoral;
			}
			else
			{
				((Button)(sender)).BackColor = System.Drawing.Color.PaleGreen;
				((Button)(sender)).FlatStyle = FlatStyle.Flat;
			}
			if ((Button)sender == Unknown)
			{
				allowedraces[0] = (((Button)(sender)).FlatStyle == FlatStyle.Flat);
				return;
			}
			if ((Button)sender == Alskant)
			{
				allowedraces[1] = (((Button)(sender)).FlatStyle == FlatStyle.Flat);
				return;
			}
			if ((Button)sender == Creonti)
			{
				allowedraces[2] = (((Button)(sender)).FlatStyle == FlatStyle.Flat);
				return;
			}
			if ((Button)sender == Human)
			{
				allowedraces[3] = (((Button)(sender)).FlatStyle == FlatStyle.Flat);
				return;
			}
			if ((Button)sender == IkThorne)
			{
				allowedraces[4] = (((Button)(sender)).FlatStyle == FlatStyle.Flat);
				return;
			}
			if ((Button)sender == Salvene)
			{
				allowedraces[5] = (((Button)(sender)).FlatStyle == FlatStyle.Flat);
				return;
			}
			if ((Button)sender == Thevian)
			{
				allowedraces[6] = (((Button)(sender)).FlatStyle == FlatStyle.Flat);
				return;
			}
			if ((Button)sender == WQHuman)
			{
				allowedraces[7] = (((Button)(sender)).FlatStyle == FlatStyle.Flat);
				return;
			}
			if ((Button)sender == Nijarin)
			{
				allowedraces[8] = (((Button)(sender)).FlatStyle == FlatStyle.Flat);
				return;
			}
			if ((Button)sender == Neutral)
			{
				allowedraces[9] = (((Button)(sender)).FlatStyle == FlatStyle.Flat);
				return;
			}
		}

		private int oldindex = 0;
		private string oldstring = null;
		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (comboBox1.SelectedIndex == 0 && oldindex != 0)
			{
				oldstring = this.Results.Rtf;
				this.Results.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang0{\\fonttbl{\\f0\\fswiss\\fcharset0 Lucida Console;}}{\\colortbl;\\red191\\green0\\blue0;}\\viewkind4\\uc1\\pard\\i\\f0\\fs20\\cf1 Be \\b very\\b0  careful, selecting too much here will result in excessive memory usage..";
			}
			if (oldindex == 0 && comboBox1.SelectedIndex != 0 && oldstring != null)
				this.Results.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang0{\\fonttbl{\\f0\\fswiss\\fcharset0 Lucida Console;}}\\viewkind4\\uc1\\pard\\i\\f0\\fs20 Press the button to calculate routes.";
			oldindex = comboBox1.SelectedIndex;
		}

        private void rdoFed_CheckedChanged(object sender, EventArgs e)
        {
            CheckGoods();
        }

        private void rdoEvil_CheckedChanged(object sender, EventArgs e)
        {
            CheckGoods();
        }

        private void rdoNeither_CheckedChanged(object sender, EventArgs e)
        {
            CheckGoods();
        }

        private void CheckGoods()
        {
            if (loading) return;

            bool illegals = true;
            bool federals = true;

            if (rdoFed.Checked)
                illegals = false;
            else if (rdoEvil.Checked)
                federals = false;
            else if (rdoNeither.Checked)
            {
                illegals = false;
                federals = false;
            }
            
            // Reset all the goods to allowed;
            for (int i = 0; i < allowed.Length; i++)
                allowed[i] = true;

            // If illegals not allowed, deselect them;
            if (!illegals)
            {
                foreach (goodEntry good in Database.goods)
                {
                    if (good.restriction < 0)
                        pictureBar1_Clicked(good.name);
                }
            }

            // If federals not allowed, deselect them;
            if (!federals)
            {
                foreach (goodEntry good in Database.goods)
                {
                    if (good.restriction > 0)
                        pictureBar1_Clicked(good.name);
                }
            }
        }

        private void rdoVladdy_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            Global.s.RouteDisplayStyle = rdoVladdy.Checked;
        }

        private void rdoPage_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            Global.s.RouteDisplayStyle = rdoVladdy.Checked;
        }
	}
}
