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

namespace MGU
{
	public class Pl0tt00r : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox To;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RichTextBox Results;
		private System.Windows.Forms.Button MyCancelButton;
		private System.Windows.Forms.TextBox From;
		private bool _destroyed = false;
		private System.Windows.Forms.GroupBox Heelal;
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox checkBox1;
		bool[] toegelaten;

		public bool destroyed
		{
			get { return _destroyed; }
		}

		public Pl0tt00r(int sector)
		{
			InitializeComponent();
			checkedListBox1.Items.AddRange((object[]) everything.getgalaxynames());
			for (int x = 0; x < checkedListBox1.Items.Count; x += 1)
				checkedListBox1.SetItemChecked(x, true);
			toegelaten = new bool[everything.gettotalgalaxynum()];
			for (int x = 0; x < toegelaten.Length; x += 1)
				toegelaten[x] = true;
			checkedListBox1.ItemCheck += new ItemCheckEventHandler(ItemCheck_Changed);
			From.Text = "#" + sector;
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
			this.Heelal = new System.Windows.Forms.GroupBox();
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.Heelal.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "From:";
			// 
			// From
			// 
			this.From.Location = new System.Drawing.Point(44, 12);
			this.From.MaxLength = 6;
			this.From.Name = "From";
			this.From.Size = new System.Drawing.Size(52, 20);
			this.From.TabIndex = 1;
			this.From.Text = "";
			this.From.TextChanged += new System.EventHandler(this.From_TextChanged);
			// 
			// To
			// 
			this.To.Location = new System.Drawing.Point(124, 12);
			this.To.MaxLength = 6;
			this.To.Name = "To";
			this.To.Size = new System.Drawing.Size(52, 20);
			this.To.TabIndex = 2;
			this.To.Text = "";
			this.To.TextChanged += new System.EventHandler(this.To_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(100, 16);
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
			this.Results.Size = new System.Drawing.Size(280, 232);
			this.Results.TabIndex = 4;
			this.Results.Text = "Please fill in <From> and <To> with sector numbers";
			// 
			// MyCancelButton
			// 
			this.MyCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.MyCancelButton.Location = new System.Drawing.Point(388, 284);
			this.MyCancelButton.Name = "MyCancelButton";
			this.MyCancelButton.TabIndex = 5;
			this.MyCancelButton.Text = "Close";
			this.MyCancelButton.Click += new System.EventHandler(this.MyCancelButton_Click);
			// 
			// Heelal
			// 
			this.Heelal.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.checkedListBox1});
			this.Heelal.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Heelal.Location = new System.Drawing.Point(300, 8);
			this.Heelal.Name = "Heelal";
			this.Heelal.Size = new System.Drawing.Size(164, 268);
			this.Heelal.TabIndex = 20;
			this.Heelal.TabStop = false;
			this.Heelal.Text = "Allowed nrofgals";
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.CheckOnClick = true;
			this.checkedListBox1.Location = new System.Drawing.Point(8, 16);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(148, 244);
			this.checkedListBox1.TabIndex = 0;
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(180, 16);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(120, 16);
			this.checkBox1.TabIndex = 21;
			this.checkBox1.Text = "Evade \'red\' sectors";
			this.checkBox1.CheckedChanged += new System.EventHandler(this.From_TextChanged);
			// 
			// Pl0tt00r
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.MyCancelButton;
			this.ClientSize = new System.Drawing.Size(466, 311);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.checkBox1,
																		  this.Heelal,
																		  this.MyCancelButton,
																		  this.Results,
																		  this.label2,
																		  this.To,
																		  this.From,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Pl0tt00r";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Plot your course";
			this.Heelal.ResumeLayout(false);
			this.ResumeLayout(false);

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

		private ushort lees(string sector)
		{
			ushort teruggave;
			if (sector == "") return 0;
			try 
			{
				if (sector[0] == '#')
					sector = sector.Substring(1, sector.Length - 1);
				teruggave = ushort.Parse(sector);
				if (teruggave > everything.lastsector || teruggave < 0)
					return 0;
				return teruggave;
			}
			catch (FormatException)
			{
				return 0;
			}
		}

		public void Recalculate()
		{
			int firstsector, lastsector;
			firstsector = lees(From.Text);
			lastsector = lees(To.Text);
			if (firstsector == 0 || lastsector == 0)
			{
				Results.Text = "Please fill in <From> and <To> with sector numbers";
				return;
			}
			route MyRoute = plotter.plot_course(firstsector, lastsector, toegelaten, !checkBox1.Checked);
			string Resulttext;
			if (MyRoute == null)
			{
				Results.Text = "No route has been found between " + firstsector + " and " + lastsector + " with the current settings.";
				return;
			}
			Resulttext = "Shortest route goes through " + MyRoute.length + " sectors, " + MyRoute.warps + " warps and it takes " + MyRoute.totalturns + " turns.";
			int curgalaxy = 0;
			foreach (int sector in MyRoute.sectors)
			{
				if (everything.getgalaxynum(sector) != curgalaxy)
				{
					Resulttext += "\n\n" + everything.getgalaxyname(everything.getgalaxynum(sector)) + " galaxy:\n";
					curgalaxy = everything.getgalaxynum(sector);
				}
				else
					Resulttext += " - ";
				Resulttext += sector.ToString();
			}
			Results.Text = Resulttext;
		}

		private void ItemCheck_Changed(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			toegelaten[e.Index] = e.NewValue == CheckState.Checked;
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
			this.Close();
		}
	}
}
