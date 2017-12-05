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
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;

namespace MGU
{
	public class TradeCalc : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox BuyIn;
		private System.Windows.Forms.Label BuyL;
		private System.Windows.Forms.TextBox BuyOut;
		private System.Windows.Forms.TextBox SellOut;
		private System.Windows.Forms.Label SellL;
		private System.Windows.Forms.TextBox SellIn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox UpperL;
		private System.ComponentModel.Container components = null;

		public TradeCalc()
		{
			InitializeComponent();
			this.nextClipboardViewer = (IntPtr)SetClipboardViewer((int)this.Handle);
		}

		protected override void Dispose( bool disposing )
		{
			ChangeClipboardChain(this.Handle, nextClipboardViewer);
			if( disposing )
			{
				if (components != null) 
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
			this.BuyIn = new System.Windows.Forms.TextBox();
			this.BuyL = new System.Windows.Forms.Label();
			this.BuyOut = new System.Windows.Forms.TextBox();
			this.SellOut = new System.Windows.Forms.TextBox();
			this.SellL = new System.Windows.Forms.Label();
			this.SellIn = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.UpperL = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// BuyIn
			// 
			this.BuyIn.Name = "BuyIn";
			this.BuyIn.Size = new System.Drawing.Size(72, 20);
			this.BuyIn.TabIndex = 0;
			this.BuyIn.Text = "";
			this.BuyIn.TextChanged += new System.EventHandler(this.BuyIn_TextChanged);
			// 
			// BuyL
			// 
			this.BuyL.Location = new System.Drawing.Point(74, 4);
			this.BuyL.Name = "BuyL";
			this.BuyL.Size = new System.Drawing.Size(36, 16);
			this.BuyL.TabIndex = 1;
			this.BuyL.Text = "Buy";
			this.BuyL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// BuyOut
			// 
			this.BuyOut.Location = new System.Drawing.Point(116, 0);
			this.BuyOut.Name = "BuyOut";
			this.BuyOut.ReadOnly = true;
			this.BuyOut.Size = new System.Drawing.Size(72, 20);
			this.BuyOut.TabIndex = 2;
			this.BuyOut.Text = "";
			// 
			// SellOut
			// 
			this.SellOut.Location = new System.Drawing.Point(116, 24);
			this.SellOut.Name = "SellOut";
			this.SellOut.ReadOnly = true;
			this.SellOut.Size = new System.Drawing.Size(72, 20);
			this.SellOut.TabIndex = 5;
			this.SellOut.Text = "";
			// 
			// SellL
			// 
			this.SellL.Location = new System.Drawing.Point(74, 24);
			this.SellL.Name = "SellL";
			this.SellL.Size = new System.Drawing.Size(36, 20);
			this.SellL.TabIndex = 4;
			this.SellL.Text = "Sell";
			this.SellL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SellIn
			// 
			this.SellIn.Location = new System.Drawing.Point(0, 24);
			this.SellIn.Name = "SellIn";
			this.SellIn.Size = new System.Drawing.Size(72, 20);
			this.SellIn.TabIndex = 3;
			this.SellIn.Text = "";
			this.SellIn.TextChanged += new System.EventHandler(this.SellIn_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 32);
			this.label1.TabIndex = 6;
			this.label1.Text = "It\'s a sell if the value in clipboard is bigger then";
			// 
			// UpperL
			// 
			this.UpperL.Location = new System.Drawing.Point(120, 56);
			this.UpperL.Name = "UpperL";
			this.UpperL.Size = new System.Drawing.Size(64, 20);
			this.UpperL.TabIndex = 7;
			this.UpperL.Text = "10000";
			this.UpperL.TextChanged += new System.EventHandler(this.UpperL_TextChanged);
			// 
			// TradeCalc
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(192, 79);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.UpperL,
																		  this.label1,
																		  this.SellOut,
																		  this.SellL,
																		  this.SellIn,
																		  this.BuyOut,
																		  this.BuyL,
																		  this.BuyIn});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TradeCalc";
			this.Text = "Neutral Port Trading Aid";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion

		#region Clippy
		[DllImport("User32.dll")]
		protected static extern int SetClipboardViewer(int hWndNewViewer);

		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

		IntPtr nextClipboardViewer;

		protected override void
			WndProc(ref System.Windows.Forms.Message m)
		{
			const int WM_DRAWCLIPBOARD = 0x308;
			const int WM_CHANGECBCHAIN = 0x030D;

			switch(m.Msg)
			{
				case WM_DRAWCLIPBOARD:
					DoClippy();
					SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
					break;

				case WM_CHANGECBCHAIN:
					if (m.WParam == nextClipboardViewer)
						nextClipboardViewer = m.LParam;
					else SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam); break;

				default: base.WndProc(ref m); break;
			}
		}

		double output = 0;

		private void DoClippy()
		{
			IDataObject iData = Clipboard.GetDataObject();
			if (iData.GetDataPresent(DataFormats.Text))
			{
				try 
				{
					double x = double.Parse((string)iData.GetData(DataFormats.Text));
					if (x <= 0 || x == output)
					{
						return;
					}
					if (x >= upper)
					{
						SellIn.Text = "";
						SellIn.Text = x.ToString();
					}
					else
					{
						BuyIn.Text = "";
						BuyIn.Text = x.ToString();
					}
				}
				catch (System.FormatException)
				{
					output = 0;
				}
			}
		}
		#endregion

		private void BuyIn_TextChanged(object sender, System.EventArgs e)
		{
			double erin, eruit;
			try
			{
				erin = double.Parse(BuyIn.Text);
			}
			catch (System.FormatException)
			{
				BuyOut.Text = "";
				return;
			}
			eruit = erin / 1.225;
			this.output = Math.Round(eruit + 1, 0);
			BuyOut.Text = output.ToString();
			Clipboard.SetDataObject(BuyOut.Text, true);
		}

		private void SellIn_TextChanged(object sender, System.EventArgs e)
		{
			double erin, eruit;
			try
			{
				erin = double.Parse(SellIn.Text);
			}
			catch (System.FormatException)
			{
				SellOut.Text = "";
				return;
			}
			eruit = erin / 0.875;
			this.output = Math.Round(eruit - 1, 0);
			SellOut.Text = output.ToString();
			Clipboard.SetDataObject(SellOut.Text, true);
		}

		private int upper = 10000;

		private void UpperL_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				upper = int.Parse(UpperL.Text);
			}
			catch (System.FormatException)
			{
				UpperL.Text = upper.ToString();
			}
		}
	}
}
