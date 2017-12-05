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
using System.Collections;
using System.Windows.Forms;

namespace MGU
{
	public class OptimalPicker : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.ComponentModel.Container components = null;
		TreeNode Nothing, Weapons, Ships, Bars, Banks, Governments, Other, Technology;
		TreeNode AnyGun, AnyBar, AnyBank, AnyFed;
		TreeNode L0, L1, L2, L3, L4, L5, L6, L7, L8, L9;
		TreeNode AlskantS, CreontiS, HumanS, IkThorneS, SalveneS, ThevianS, WQHumanS, NijarinS, RestrictS, NeutralS, OtherS;
		TextBox AbuseMe;

		private TreeNode[] Fillsubnodes(int id, ref TreeNode Selected)
		{
			locationEntry[] locations = Database.locations;
			Nothing = new TreeNode(Database.GetInfo(0).name);
			Nothing.Tag = (object) 0;
			if (id == 0)
				Selected = Nothing;
			bool isitem = false;
			if (id < 0)
				isitem = true;
			Governments = new TreeNode("Governments");
			Banks = new TreeNode("Banks");
			Bars = new TreeNode("Bars");
			Other = new TreeNode("Other");
			AnyGun = new TreeNode("Any weapon shop");
			AnyGun.Tag = (object) 1073741824;
			AnyBar = new TreeNode("Any bar");
			AnyBar.Tag = (object) 1073741825;
			AnyBank = new TreeNode("Any bank");
			AnyBank.Tag = (object) 1073741826;
			AnyFed = new TreeNode("Any protection");
			AnyFed.Tag = (object) 1073741827;
			Banks.Nodes.Add(AnyBank);
			Bars.Nodes.Add(AnyBar);
			Governments.Nodes.Add(AnyFed);
			for (int x = 0; x < locations.Length; x += 1)
			{
				if (locations[x].id < 100 || !locations[x].used) continue;
				TreeNode CurNode = new TreeNode(locations[x].name);
				CurNode.Tag = (object) locations[x].id;
				if (locations[x].id == id && !isitem)
					Selected = CurNode;
				if (locations[x].id < 200)
				{
					if (locations[x].icon != -1)
						Governments.Nodes.Add(CurNode);
				}
				else if (locations[x].id < 300)
				{
					Banks.Nodes.Add(CurNode);
				}
				else if (locations[x].id < 400)
				{
					Bars.Nodes.Add(CurNode);
				}
				else if (locations[x].id >= 700)
				{
					Other.Nodes.Add(CurNode);
				}
			}
			if (isitem)
				id *= -1;
			Weapons = new TreeNode("Weapons");
			Technology = new TreeNode("Technology");
			Weapons.Nodes.Add(AnyGun);
			Ships = new TreeNode("Ships");
			itemEntry[] items = Database.items;
			AlskantS = new TreeNode("Alskant");
			CreontiS = new TreeNode("Creonti");
			HumanS   = new TreeNode("Human");
			IkThorneS= new TreeNode("Ik'Thorne");
			SalveneS = new TreeNode("Salvene");
			ThevianS = new TreeNode("Thevian");
			WQHumanS = new TreeNode("WQ Human");
			NijarinS = new TreeNode("Nijarin");
			NeutralS = new TreeNode("Neutral");
			RestrictS = new TreeNode("Restricted");
			OtherS = new TreeNode("Other");
			L0 = new TreeNode("Level 0");
			L1 = new TreeNode("Level 1");
			L2 = new TreeNode("Level 2");
			L3 = new TreeNode("Level 3");
			L4 = new TreeNode("Level 4");
			L5 = new TreeNode("Level 5");
			L6 = new TreeNode("Level 6");
			L7 = new TreeNode("Level 7");
			L8 = new TreeNode("Level 8");
			L9 = new TreeNode("Level 9");
			for (int x = 0; x < items.Length; x += 1)
			{
				if (!items[x].used) continue;
				TreeNode CurNode = new TreeNode(items[x].name);
				if (isitem && x + 1 == id)
					Selected = CurNode;
				CurNode.Tag = (object) (-1 - x);
				switch (items[x].type)
				{
					case ItemType.Ship: if (items[x].restriction != 0)
											RestrictS.Nodes.Add(CurNode);
										else
											switch (items[x].race.ToLower())
											{
												case "alskant": AlskantS.Nodes.Add(CurNode); break;
												case "creonti": CreontiS.Nodes.Add(CurNode); break;
												case "human": HumanS.Nodes.Add(CurNode); break;
												case "ik'thorne": IkThorneS.Nodes.Add(CurNode); break;
												case "salvene": SalveneS.Nodes.Add(CurNode); break;
												case "thevian": ThevianS.Nodes.Add(CurNode); break;
												case "wq human": WQHumanS.Nodes.Add(CurNode); break;
												case "nijarin": NijarinS.Nodes.Add(CurNode); break;
												case "neutral": NeutralS.Nodes.Add(CurNode); break;
												default: OtherS.Nodes.Add(CurNode); break;
											}
										break;
					case ItemType.Tech: Technology.Nodes.Add(CurNode); break;
					case ItemType.Weapon: switch (items[x].powerlevel)
										  {
											  case 0: L0.Nodes.Add(CurNode); break;
											  case 1: L1.Nodes.Add(CurNode); break;
											  case 2: L2.Nodes.Add(CurNode); break;
											  case 3: L3.Nodes.Add(CurNode); break;
											  case 4: L4.Nodes.Add(CurNode); break;
											  case 5: L5.Nodes.Add(CurNode); break;
											  case 6: L6.Nodes.Add(CurNode); break;
											  case 7: L7.Nodes.Add(CurNode); break;
											  case 8: L8.Nodes.Add(CurNode); break;
											  case 9: L9.Nodes.Add(CurNode); break;
										  }
										break;
				}
			}
			ArrayList Returnage = new ArrayList();
			if (L0.Nodes.Count != 0)
				Weapons.Nodes.Add(L0);
			if (L1.Nodes.Count != 0)
				Weapons.Nodes.Add(L1);
			if (L2.Nodes.Count != 0)
				Weapons.Nodes.Add(L2);
			if (L3.Nodes.Count != 0)
				Weapons.Nodes.Add(L3);
			if (L4.Nodes.Count != 0)
				Weapons.Nodes.Add(L4);
			if (L5.Nodes.Count != 0)
				Weapons.Nodes.Add(L5);
			if (L6.Nodes.Count != 0)
				Weapons.Nodes.Add(L6);
			if (L7.Nodes.Count != 0)
				Weapons.Nodes.Add(L7);
			if (L8.Nodes.Count != 0)
				Weapons.Nodes.Add(L8);
			if (L9.Nodes.Count != 0)
				Weapons.Nodes.Add(L9);
			if (AlskantS.Nodes.Count != 0)
				Ships.Nodes.Add(AlskantS);
			if (CreontiS.Nodes.Count != 0)
				Ships.Nodes.Add(CreontiS);
			if (HumanS.Nodes.Count != 0)
				Ships.Nodes.Add(HumanS);
			if (IkThorneS.Nodes.Count != 0)
				Ships.Nodes.Add(IkThorneS);
			if (SalveneS.Nodes.Count != 0)
				Ships.Nodes.Add(SalveneS);
			if (ThevianS.Nodes.Count != 0)
				Ships.Nodes.Add(ThevianS);
			if (WQHumanS.Nodes.Count != 0)
				Ships.Nodes.Add(WQHumanS);
			if (NijarinS.Nodes.Count != 0)
				Ships.Nodes.Add(NijarinS);
			if (RestrictS.Nodes.Count != 0)
				Ships.Nodes.Add(RestrictS);
			if (NeutralS.Nodes.Count != 0)
				Ships.Nodes.Add(NeutralS);
			if (OtherS.Nodes.Count != 0)
				Ships.Nodes.Add(AlskantS);
			Returnage.Add(Nothing);
			if (Weapons.Nodes.Count != 0)
				Returnage.Add(Weapons);
			if (Ships.Nodes.Count != 0)
				Returnage.Add(Ships);
			Returnage.Add(Bars);
			Returnage.Add(Banks);
			Returnage.Add(Governments);
			if (Technology.Nodes.Count != 0)
				Returnage.Add(Technology);
			if (Other.Nodes.Count != 0)
				Returnage.Add(Other);
			return (TreeNode[]) Returnage.ToArray(typeof(TreeNode));
		}

		public OptimalPicker()
		{
			InitializeComponent();
		}

		public OptimalPicker(TextBox Boxy)
		{
			InitializeComponent();
			AbuseMe = Boxy;
			TreeNode m33p = null;
			treeView1.Nodes.AddRange(Fillsubnodes((int)Boxy.Tag, ref m33p));
			treeView1.SelectedNode = m33p;
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
		private void InitializeComponent()
		{
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.HideSelection = false;
			this.treeView1.ImageIndex = -1;
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = -1;
			this.treeView1.Size = new System.Drawing.Size(296, 240);
			this.treeView1.TabIndex = 0;
			this.treeView1.DoubleClick += new System.EventHandler(this.ClickClick);
			this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.SelectWantsToChange);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(4, 244);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(220, 244);
			this.button2.Name = "button2";
			this.button2.TabIndex = 2;
			this.button2.Text = "Cancel";
			// 
			// OptimalPicker
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(298, 271);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button2,
																		  this.button1,
																		  this.treeView1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptimalPicker";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Location Picker";
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			AbuseMe.Tag = treeView1.SelectedNode.Tag;
			AbuseMe.Text = treeView1.SelectedNode.Text;
			this.Close();
		}

		private void SelectWantsToChange(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if (e.Node.Nodes.Count != 0)
				e.Cancel = true;
			if (!e.Node.IsExpanded)
				e.Node.Expand();
			else e.Node.Collapse();
		}

		private void ClickClick(object sender, System.EventArgs e)
		{
			if (treeView1.SelectedNode.Nodes.Count != 0) return;
			button1_Click(sender, e);
		}
	}
}
