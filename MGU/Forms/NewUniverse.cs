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
	public class NewUniverse : System.Windows.Forms.Form
	{
        int largestsector;
        MainStuff hostApplication;
        Game currentGame;

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView GalaxyList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox NameField;
		private System.Windows.Forms.TextBox WidthField;
        private System.Windows.Forms.TextBox HeightField;
		private System.Windows.Forms.Label HeightLabel;
		private System.Windows.Forms.Button AddGalaxyButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox DefaultRaceBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button RemoveButton;
		private System.Windows.Forms.Button ModifyButton;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox ModifyNameField;
		private System.Windows.Forms.TextBox ModifyWidthField;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox ModifyHeightField;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Label label8;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button CreateUniverseButton;
		private System.Windows.Forms.Button button5;
        private Label WidthLabel;
        private Button MoveUpButton;
        private Button MoveDownButton;
		private ArrayList GalaxyInfo;

		public NewUniverse(MainStuff host)
		{
            hostApplication = host;
            currentGame = host.games[host.currentGame];
            largestsector = 0;
			InitializeComponent();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewUniverse));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MoveDownButton = new System.Windows.Forms.Button();
            this.MoveUpButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ModifyHeightField = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ModifyWidthField = new System.Windows.Forms.TextBox();
            this.ModifyNameField = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ModifyButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.DefaultRaceBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.AddGalaxyButton = new System.Windows.Forms.Button();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.HeightField = new System.Windows.Forms.TextBox();
            this.WidthField = new System.Windows.Forms.TextBox();
            this.NameField = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GalaxyList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.CreateUniverseButton = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MoveDownButton);
            this.groupBox1.Controls.Add(this.MoveUpButton);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.GalaxyList);
            this.groupBox1.Location = new System.Drawing.Point(4, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(584, 340);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Galaxy layout";
            // 
            // MoveDownButton
            // 
            this.MoveDownButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveDownButton.Image")));
            this.MoveDownButton.Location = new System.Drawing.Point(368, 111);
            this.MoveDownButton.Name = "MoveDownButton";
            this.MoveDownButton.Size = new System.Drawing.Size(56, 45);
            this.MoveDownButton.TabIndex = 4;
            this.MoveDownButton.UseVisualStyleBackColor = true;
            this.MoveDownButton.Click += new System.EventHandler(this.MoveDownButton_Click);
            // 
            // MoveUpButton
            // 
            this.MoveUpButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveUpButton.Image")));
            this.MoveUpButton.Location = new System.Drawing.Point(368, 63);
            this.MoveUpButton.Name = "MoveUpButton";
            this.MoveUpButton.Size = new System.Drawing.Size(56, 45);
            this.MoveUpButton.TabIndex = 3;
            this.MoveUpButton.UseVisualStyleBackColor = true;
            this.MoveUpButton.Click += new System.EventHandler(this.MoveUpButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ModifyHeightField);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.ModifyWidthField);
            this.groupBox3.Controls.Add(this.ModifyNameField);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.ModifyButton);
            this.groupBox3.Controls.Add(this.RemoveButton);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.comboBox2);
            this.groupBox3.Location = new System.Drawing.Point(430, 200);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(148, 132);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Modify selected galaxy";
            // 
            // ModifyHeightField
            // 
            this.ModifyHeightField.Location = new System.Drawing.Point(94, 36);
            this.ModifyHeightField.Name = "ModifyHeightField";
            this.ModifyHeightField.Size = new System.Drawing.Size(46, 20);
            this.ModifyHeightField.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(76, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 16);
            this.label7.TabIndex = 6;
            this.label7.Text = "H:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(4, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "W:";
            // 
            // ModifyWidthField
            // 
            this.ModifyWidthField.Location = new System.Drawing.Point(24, 36);
            this.ModifyWidthField.Name = "ModifyWidthField";
            this.ModifyWidthField.Size = new System.Drawing.Size(46, 20);
            this.ModifyWidthField.TabIndex = 4;
            // 
            // ModifyNameField
            // 
            this.ModifyNameField.Location = new System.Drawing.Point(44, 16);
            this.ModifyNameField.Name = "ModifyNameField";
            this.ModifyNameField.Size = new System.Drawing.Size(96, 20);
            this.ModifyNameField.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "Name:";
            // 
            // ModifyButton
            // 
            this.ModifyButton.Location = new System.Drawing.Point(8, 104);
            this.ModifyButton.Name = "ModifyButton";
            this.ModifyButton.Size = new System.Drawing.Size(68, 23);
            this.ModifyButton.TabIndex = 1;
            this.ModifyButton.Text = "Modify";
            this.ModifyButton.Click += new System.EventHandler(this.ModifyButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(80, 104);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(64, 23);
            this.RemoveButton.TabIndex = 0;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.Click += new System.EventHandler(this.RemoveGalClick);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 16);
            this.label8.TabIndex = 12;
            this.label8.Text = "Default port owner:";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Items.AddRange(new object[] {
            "Alskant",
            "Creonti",
            "Human",
            "Ik\'Thorne",
            "Salvene",
            "Thevian",
            "WQ Human",
            "Nijarin",
            "Neutral"});
            this.comboBox2.Location = new System.Drawing.Point(8, 80);
            this.comboBox2.MaxDropDownItems = 12;
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(136, 21);
            this.comboBox2.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.WidthLabel);
            this.groupBox2.Controls.Add(this.DefaultRaceBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.AddGalaxyButton);
            this.groupBox2.Controls.Add(this.HeightLabel);
            this.groupBox2.Controls.Add(this.HeightField);
            this.groupBox2.Controls.Add(this.WidthField);
            this.groupBox2.Controls.Add(this.NameField);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(430, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(148, 166);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add galaxy";
            // 
            // WidthLabel
            // 
            this.WidthLabel.Location = new System.Drawing.Point(16, 55);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(60, 16);
            this.WidthLabel.TabIndex = 12;
            this.WidthLabel.Text = "Width";
            // 
            // DefaultRaceBox
            // 
            this.DefaultRaceBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DefaultRaceBox.Items.AddRange(new object[] {
            "Alskant",
            "Creonti",
            "Human",
            "Ik\'Thorne",
            "Salvene",
            "Thevian",
            "WQ Human",
            "Nijarin",
            "Neutral"});
            this.DefaultRaceBox.Location = new System.Drawing.Point(8, 111);
            this.DefaultRaceBox.MaxDropDownItems = 12;
            this.DefaultRaceBox.Name = "DefaultRaceBox";
            this.DefaultRaceBox.Size = new System.Drawing.Size(132, 21);
            this.DefaultRaceBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Default port race:";
            // 
            // AddGalaxyButton
            // 
            this.AddGalaxyButton.Location = new System.Drawing.Point(8, 139);
            this.AddGalaxyButton.Name = "AddGalaxyButton";
            this.AddGalaxyButton.Size = new System.Drawing.Size(132, 20);
            this.AddGalaxyButton.TabIndex = 9;
            this.AddGalaxyButton.Text = "Add galaxy";
            this.AddGalaxyButton.Click += new System.EventHandler(this.AddGalaxyButton_Click);
            // 
            // HeightLabel
            // 
            this.HeightLabel.Location = new System.Drawing.Point(80, 56);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(60, 16);
            this.HeightLabel.TabIndex = 8;
            this.HeightLabel.Text = "Height";
            // 
            // HeightField
            // 
            this.HeightField.Location = new System.Drawing.Point(80, 72);
            this.HeightField.Name = "HeightField";
            this.HeightField.Size = new System.Drawing.Size(60, 20);
            this.HeightField.TabIndex = 3;
            // 
            // WidthField
            // 
            this.WidthField.Location = new System.Drawing.Point(16, 72);
            this.WidthField.Name = "WidthField";
            this.WidthField.Size = new System.Drawing.Size(60, 20);
            this.WidthField.TabIndex = 2;
            // 
            // NameField
            // 
            this.NameField.Location = new System.Drawing.Point(4, 32);
            this.NameField.Name = "NameField";
            this.NameField.Size = new System.Drawing.Size(136, 20);
            this.NameField.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name of the galaxy:";
            // 
            // GalaxyList
            // 
            this.GalaxyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.GalaxyList.FullRowSelect = true;
            this.GalaxyList.GridLines = true;
            this.GalaxyList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.GalaxyList.HideSelection = false;
            this.GalaxyList.Location = new System.Drawing.Point(8, 16);
            this.GalaxyList.MultiSelect = false;
            this.GalaxyList.Name = "GalaxyList";
            this.GalaxyList.Size = new System.Drawing.Size(354, 316);
            this.GalaxyList.TabIndex = 0;
            this.GalaxyList.UseCompatibleStateImageBehavior = false;
            this.GalaxyList.View = System.Windows.Forms.View.Details;
            this.GalaxyList.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Galaxy name";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Height";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 45;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Width";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 45;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Begins in";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 55;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Ends at";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 55;
            // 
            // CreateUniverseButton
            // 
            this.CreateUniverseButton.Location = new System.Drawing.Point(372, 358);
            this.CreateUniverseButton.Name = "CreateUniverseButton";
            this.CreateUniverseButton.Size = new System.Drawing.Size(76, 24);
            this.CreateUniverseButton.TabIndex = 2;
            this.CreateUniverseButton.Text = "OK";
            this.CreateUniverseButton.Click += new System.EventHandler(this.CreateUniverseButton_Click);
            // 
            // button5
            // 
            this.button5.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button5.Location = new System.Drawing.Point(456, 358);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(76, 24);
            this.button5.TabIndex = 3;
            this.button5.Text = "Cancel";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // NewUniverse
            // 
            this.AcceptButton = this.CreateUniverseButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.button5;
            this.ClientSize = new System.Drawing.Size(600, 385);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.CreateUniverseButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewUniverse";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Universe Creation";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
		{
			
		}

		private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            MoveUpButton.Enabled = true;
            MoveDownButton.Enabled = true;

            if(GalaxyList.SelectedIndices.Count > 0)
            {
                ModifyNameField.Text = GalaxyList.SelectedItems[0].SubItems[0].Text;
                ModifyHeightField.Text = GalaxyList.SelectedItems[0].SubItems[1].Text;
                ModifyWidthField.Text = GalaxyList.SelectedItems[0].SubItems[2].Text;

                if (GalaxyList.SelectedIndices[0] == 0)
                    MoveUpButton.Enabled = false;
                if (GalaxyList.SelectedIndices[0] == GalaxyList.Items.Count-1)
                    MoveDownButton.Enabled = false;
            }
		}

		private void RemoveGalClick(object sender, System.EventArgs e)
		{
            if (GalaxyList.SelectedIndices.Count > 0)
            {
                GalaxyList.SelectedItems[0].Remove();
            }
		}

		private void RedrawInfo()
		{
			
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void AddGalaxyButton_Click(object sender, EventArgs e)
        {
            string[] infoItems = new string[5];
            int height, width;

            infoItems[0] = NameField.Text;
            if (!int.TryParse(HeightField.Text, out height))
            {
                MessageBox.Show("Reading of the height failed: please enter a valid number");
                return;
            }
            else
                infoItems[1] = height.ToString();
            if (!int.TryParse(WidthField.Text, out width))
            {
                MessageBox.Show("Reading of the width failed: please enter a valid number");
                return;
            }
            else
                infoItems[2] = width.ToString();

            infoItems[3] = (largestsector + 1).ToString();
            infoItems[4] = (largestsector + height*width).ToString();

            largestsector += height*width;

            GalaxyList.Items.Add(new ListViewItem(infoItems));
        }

        private void ModifyButton_Click(object sender, EventArgs e)
        {
            if (GalaxyList.SelectedIndices.Count > 0)
            {
                GalaxyList.SelectedItems[0].SubItems[0].Text = ModifyNameField.Text;
                GalaxyList.SelectedItems[0].SubItems[1].Text = ModifyHeightField.Text;
                GalaxyList.SelectedItems[0].SubItems[2].Text = ModifyWidthField.Text;
            }
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            string[] subItems = new string[5];
            if (GalaxyList.SelectedIndices.Count > 0)
            {
                ListViewItem temp = new ListViewItem(subItems);
                temp.SubItems[0] = GalaxyList.SelectedItems[0].SubItems[0];
                temp.SubItems[1] = GalaxyList.SelectedItems[0].SubItems[1];
                temp.SubItems[2] = GalaxyList.SelectedItems[0].SubItems[2];
                temp.SubItems[3] = GalaxyList.SelectedItems[0].SubItems[3];
                temp.SubItems[4] = GalaxyList.SelectedItems[0].SubItems[4];
                GalaxyList.Items[GalaxyList.SelectedIndices[0]].SubItems[0] = GalaxyList.Items[GalaxyList.SelectedIndices[0]-1].SubItems[0];
                GalaxyList.Items[GalaxyList.SelectedIndices[0]].SubItems[1] = GalaxyList.Items[GalaxyList.SelectedIndices[0] - 1].SubItems[1];
                GalaxyList.Items[GalaxyList.SelectedIndices[0]].SubItems[2] = GalaxyList.Items[GalaxyList.SelectedIndices[0] - 1].SubItems[2];
                GalaxyList.Items[GalaxyList.SelectedIndices[0]].SubItems[3] = GalaxyList.Items[GalaxyList.SelectedIndices[0] - 1].SubItems[3];
                GalaxyList.Items[GalaxyList.SelectedIndices[0]].SubItems[4] = GalaxyList.Items[GalaxyList.SelectedIndices[0] - 1].SubItems[4];
                GalaxyList.Items[GalaxyList.SelectedIndices[0] - 1].SubItems[0] = temp.SubItems[0];
                GalaxyList.Items[GalaxyList.SelectedIndices[0] - 1].SubItems[1] = temp.SubItems[1];
                GalaxyList.Items[GalaxyList.SelectedIndices[0] - 1].SubItems[2] = temp.SubItems[2];
                GalaxyList.Items[GalaxyList.SelectedIndices[0] - 1].SubItems[3] = temp.SubItems[3];
                GalaxyList.Items[GalaxyList.SelectedIndices[0] - 1].SubItems[4] = temp.SubItems[4];
            }
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            string[] subItems = new string[5];
            if (GalaxyList.SelectedIndices.Count > 0)
            {
                ListViewItem temp = new ListViewItem(subItems);
                temp.SubItems[0] = GalaxyList.SelectedItems[0].SubItems[0];
                temp.SubItems[1] = GalaxyList.SelectedItems[0].SubItems[1];
                temp.SubItems[2] = GalaxyList.SelectedItems[0].SubItems[2];
                temp.SubItems[3] = GalaxyList.SelectedItems[0].SubItems[3];
                temp.SubItems[4] = GalaxyList.SelectedItems[0].SubItems[4];
                GalaxyList.Items[GalaxyList.SelectedIndices[0]].SubItems[0] = GalaxyList.Items[GalaxyList.SelectedIndices[0] + 1].SubItems[0];
                GalaxyList.Items[GalaxyList.SelectedIndices[0]].SubItems[1] = GalaxyList.Items[GalaxyList.SelectedIndices[0] + 1].SubItems[1];
                GalaxyList.Items[GalaxyList.SelectedIndices[0]].SubItems[2] = GalaxyList.Items[GalaxyList.SelectedIndices[0] + 1].SubItems[2];
                GalaxyList.Items[GalaxyList.SelectedIndices[0]].SubItems[3] = GalaxyList.Items[GalaxyList.SelectedIndices[0] + 1].SubItems[3];
                GalaxyList.Items[GalaxyList.SelectedIndices[0]].SubItems[4] = GalaxyList.Items[GalaxyList.SelectedIndices[0] + 1].SubItems[4];
                GalaxyList.Items[GalaxyList.SelectedIndices[0] + 1].SubItems[0] = temp.SubItems[0];
                GalaxyList.Items[GalaxyList.SelectedIndices[0] + 1].SubItems[1] = temp.SubItems[1];
                GalaxyList.Items[GalaxyList.SelectedIndices[0] + 1].SubItems[2] = temp.SubItems[2];
                GalaxyList.Items[GalaxyList.SelectedIndices[0] + 1].SubItems[3] = temp.SubItems[3];
                GalaxyList.Items[GalaxyList.SelectedIndices[0] + 1].SubItems[4] = temp.SubItems[4];
            }
        }

        private void CreateUniverseButton_Click(object sender, EventArgs e)
        {
            if(GalaxyList.Items.Count == 0)
            {
                MessageBox.Show("Creating a universe failed because no galaxies have been defined");
                return;
            }

            this.hostApplication.games[hostApplication.currentGame].InitializeGalaxies((ushort)(GalaxyList.Items.Count+1));

            for (int g = 1; g <= GalaxyList.Items.Count; g++)
            {
                this.hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_id = g;
                this.hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_xsize = Convert.ToInt16(GalaxyList.Items[g-1].SubItems[2].Text);
                this.hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_ysize = Convert.ToInt16(GalaxyList.Items[g-1].SubItems[1].Text);
                this.hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_type = "Racial";
                this.hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_name = Convert.ToString(GalaxyList.Items[g-1].SubItems[0].Text);
                this.hostApplication.games[hostApplication.currentGame].galaxy[g].lowestsectorid = Convert.ToInt16(GalaxyList.Items[g-1].SubItems[3].Text);
                this.hostApplication.games[hostApplication.currentGame].galaxy[g].InitializeGalaxy(currentGame.galaxy[g].galaxy_xsize, currentGame.galaxy[g].galaxy_ysize);
            }

            this.hostApplication.games[hostApplication.currentGame].nrofsectors = (ushort)(Convert.ToInt16(GalaxyList.Items[GalaxyList.Items.Count-1].SubItems[4].Text)+1);
            this.hostApplication.games[hostApplication.currentGame].currentGalaxy = 1;
            currentGame.shortestroutes = new Route[currentGame.nrofsectors, currentGame.nrofsectors];

            //This is where we set panel1 as the parent control of all sector objects, so that the OnPaint message of panle1 gets passed on to all sectors.
            for (int g = 1; g < this.hostApplication.games[hostApplication.currentGame].nrofgalaxies; g++)
                for (int x = 0; x < this.hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_xsize; x++)
                    for (int y = 0; y < this.hostApplication.games[hostApplication.currentGame].galaxy[g].galaxy_ysize; y++)
                        this.hostApplication.games[hostApplication.currentGame].galaxy[g].sector[x, y].Parent = this.hostApplication.panel1;

            hostApplication.goLeftButton.Enabled = hostApplication.goRightButton.Enabled = hostApplication.goDownButton.Enabled = hostApplication.goUpButton.Enabled = hostApplication.toolBarButton1.Enabled = hostApplication.toolBarButton2.Enabled = hostApplication.toolBarButton3.Enabled = hostApplication.toolBarButton5.Enabled = hostApplication.toolBarButton6.Enabled = hostApplication.toolBarButton7.Enabled = hostApplication.toolBarButton8.Enabled = hostApplication.toolBarButton1.Enabled = true;
            currentGame.nrofgoods = 12;
            currentGame.InitializeRaces(9, true);
            currentGame.InitializeGoods(12, true);
            currentGame.InitializeShips(76, true);
            currentGame.InitializeWeapons(57, true);
            currentGame.InitializeLocations(57, true);
            
            //Add the galaxies to the menu
            this.hostApplication.AddGalaxies();
            this.hostApplication.Redraw();
            this.Close();
        }
	}
}
