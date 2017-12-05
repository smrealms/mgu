/* Copyright 2009 Robin Langerak
 * Original code by Lasse Johansen
 *
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ToolbarControl
{
	[System.ComponentModel.DefaultEvent("Clicked")]
	public class PictureBar : System.Windows.Forms.Control
	{
		public delegate void EventHandler(int selectedIndex);

		#region Add on - not finished yet
		// I would like that the properties can be adjusted in categories. 
		// So e.g. linecolor category contains normal, over, down.
		private ColorsTemp _lineColor = new ColorsTemp();
		
		[System.ComponentModel.Browsable(false)]
		public ColorsTemp LineColors
		{
			get {return _lineColor;}
			set {_lineColor = value;}
		}

		[System.ComponentModel.TypeConverter(typeof(ColorsTempConverter))]
		public class ColorsTemp
		{
			private System.Drawing.Color _normal = System.Drawing.Color.Black;
			private System.Drawing.Color _over = System.Drawing.Color.Black;
			private System.Drawing.Color _down = System.Drawing.Color.Black;

			public System.Drawing.Color Normal
			{
				get {return _normal;}
				set {_normal = value;}
			}

			public System.Drawing.Color Over
			{
				get {return _over;}
				set {_over = value;}
			}

			public System.Drawing.Color Down
			{
				get {return _down;}
				set {_down = value;}
			}
		}
		internal class ColorsTempConverter : System.ComponentModel.ExpandableObjectConverter 
		{
			public override bool CanConvertFrom(
				System.ComponentModel.ITypeDescriptorContext context, Type t) 
			{

				if (t == typeof(string)) 
				{
					return false;
				}
				return base.CanConvertFrom(context, t);
			}

			public override object ConvertFrom(
				System.ComponentModel.ITypeDescriptorContext context, 
				System.Globalization.CultureInfo info,
				object value) 
			{

				if (value is string) 
				{
					try 
					{
						
						string s = (string) value;
						string[] colorsArray = s.Split('|');

						ColorsTemp colors = new ColorsTemp();
						colors.Normal = System.Drawing.Color.FromName(colorsArray[1]);
						colors.Over = System.Drawing.Color.FromName(colorsArray[2]);
						colors.Down = System.Drawing.Color.FromName(colorsArray[0]);
					}
					catch {}
					// if we got this far, complain that we
					// couldn't parse the string
					//
					throw new ArgumentException(
						"Can not convert '" + (string)value + 
						"' to type ColorsTemp");
				}
				return base.ConvertFrom(context, info, value);
			}
                                 
			public override object ConvertTo(
				System.ComponentModel.ITypeDescriptorContext context, 
				System.Globalization.CultureInfo culture, 
				object value,    
				System.Type destType) 
			{
				if (destType == typeof(string) && value is ColorsTemp) 
				{
					ColorsTemp colors = value as ColorsTemp;
					return string.Join("|", new string[] {colors.Normal.Name, colors.Over.Name, colors.Down.Name});
				}

				return base.ConvertTo(context, culture, value, destType);
			}   
		}

		#endregion

		#region Events
		public event EventHandler Clicked;
		public event EventHandler ItemChanged;
		#endregion

		#region Enums
		private enum State
		{
			Over,
			Down
		}
		#endregion

		#region Size parameters
		private int _sizeOfButtonBorderAndSpace = 1;
		private int _offset = 1;
		private int _lineSize = 1;
		private int _smaller = 0;
		#endregion

		#region Selected parameters
		private int _selectedIndex = -1;
		private State _state;
		#endregion

		#region Color Parameters
		private System.Drawing.Pen _pen = System.Drawing.Pens.Black;
		private System.Drawing.Pen _penOver = System.Drawing.Pens.Black;
		private System.Drawing.Pen _penDown = System.Drawing.Pens.Black;
		
		//Next 3 internal variables only used as cache
		private System.Drawing.Color _brushColor = System.Drawing.Color.Transparent;
		private System.Drawing.Color _brushColorOver = System.Drawing.Color.Transparent;
		private System.Drawing.Color _brushColorDown = System.Drawing.Color.Transparent;

		private System.Drawing.Brush _brush = System.Drawing.Brushes.Transparent;
		private System.Drawing.Brush _brushOver = System.Drawing.Brushes.Transparent;
		private System.Drawing.Brush _brushDown = System.Drawing.Brushes.Transparent;
		#endregion

		#region Other Parameters
		//private Alignment _alignment = Alignment.Top;
		private System.Drawing.ContentAlignment _alignment = System.Drawing.ContentAlignment.TopCenter;
		private System.Windows.Forms.ImageList _iList = new System.Windows.Forms.ImageList();
		#endregion

		public PictureBar()
		{
			this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint|System.Windows.Forms.ControlStyles.DoubleBuffer|System.Windows.Forms.ControlStyles.UserPaint|System.Windows.Forms.ControlStyles.ResizeRedraw|System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
			//this.Dock = System.Windows.Forms.DockStyle.Bottom;
		}

		
		#region Invokers
		private void InvokeItemChanged(int selectedIndex)
		{
			if (this.ItemChanged != null)
				this.ItemChanged(selectedIndex);
		}
		private void InvokeClicked(int selectedIndex)
		{
			if(this.Clicked != null)
				this.Clicked(selectedIndex);
		}
		#endregion

		#region Helpers
		private void CalculateRect(ref System.Drawing.Rectangle smallButtonRect, ref System.Drawing.Rectangle largeButtonRect)
		{
			if(_iList.Images.Count == 0)
			{
				smallButtonRect = new System.Drawing.Rectangle(_offset, _offset, 0, _iList.ImageSize.Height + _lineSize * 2);
				largeButtonRect = new System.Drawing.Rectangle(_offset, _offset, 0, (_iList.ImageSize.Height + _lineSize) * 2);
			}

			int left = 0;
			int smallButtonTop = 0;
			int width = _sizeOfButtonBorderAndSpace * _iList.Images.Count + _iList.ImageSize.Width / 2;

			switch(_alignment)
			{
				case System.Drawing.ContentAlignment.TopLeft:
					left = _offset;
					smallButtonTop = _offset;
					break;
				case System.Drawing.ContentAlignment.TopCenter:
					left = (this.Width - width) / 2;
					smallButtonTop = _offset;
					break;
				case System.Drawing.ContentAlignment.TopRight:
					left = this.Width - width - _offset;
					smallButtonTop = _offset;
					break;
				case System.Drawing.ContentAlignment.MiddleLeft:
					left = _offset;
					smallButtonTop = _iList.ImageSize.Height / 2 + _offset;
					break;
				case System.Drawing.ContentAlignment.MiddleCenter:
					left = (this.Width - width) / 2;
					smallButtonTop = _iList.ImageSize.Height / 2 + _offset;
					break;
				case System.Drawing.ContentAlignment.MiddleRight:
					left = this.Width - width - _offset;
					smallButtonTop = _iList.ImageSize.Height / 2 + _offset;
					break;
				case System.Drawing.ContentAlignment.BottomLeft:
					left = _offset;
					smallButtonTop = _iList.ImageSize.Height + _offset;
					break;
				case System.Drawing.ContentAlignment.BottomCenter:
					left = (this.Width - width) / 2;
					smallButtonTop = _iList.ImageSize.Height + _offset;
					break;
				case System.Drawing.ContentAlignment.BottomRight:
					left = this.Width - width - _offset;
					smallButtonTop = _iList.ImageSize.Height + _offset;
					break;
			}

			smallButtonRect = new System.Drawing.Rectangle(left, smallButtonTop, width, _iList.ImageSize.Height);
			largeButtonRect = new System.Drawing.Rectangle(left, _offset, width, _iList.ImageSize.Height * 2);
		}

		#endregion

		#region Drawing Helper Functions
		private void DrawPicturesAndFrames(System.Drawing.Graphics g, System.Drawing.Rectangle smallButtonRect, System.Drawing.Rectangle largeButtonRect)
		{
			for (int i = 0; i < _iList.Images.Count ; i++)
			{
				int x = i * _sizeOfButtonBorderAndSpace + smallButtonRect.Left + _iList.ImageSize.Width / 2;

				if(_selectedIndex == i) // Draw upfront picture(scaled)
				{
					System.Drawing.Rectangle destRectBB = new System.Drawing.Rectangle(x - _iList.ImageSize.Width / 2 + _smaller, _offset, _iList.ImageSize.Width * 2 + 1 - 2 * _smaller, _iList.ImageSize.Height * 2 + 1 - 2 * _smaller);
					System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x - _iList.ImageSize.Width / 2 + 1 + _smaller , _offset + 1, _iList.ImageSize.Width * 2 - 2 * _smaller, _iList.ImageSize.Height * 2 - 2 * _smaller);
					System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(0, 0, _iList.ImageSize.Width, _iList.ImageSize.Height);

					// Draw border of button
					g.DrawRectangle(_state == State.Over ? _penOver : _penDown, destRectBB);

					// Draw background color
					System.Drawing.Brush brush = null;

					if(_state == State.Over)
					{
						if(_brushColorOver != System.Drawing.Color.Transparent)
							brush = _brushOver;
					}
					else
					{
						if(_brushColorDown != System.Drawing.Color.Transparent)
							brush = _brushDown;
					}

					if(brush != null)
						g.FillRectangle(brush, destRect);

					// Draw scaled image
					g.DrawImage(_iList.Images[i], destRect, sourceRect, System.Drawing.GraphicsUnit.Pixel);
				}
				else // Draw normally
				{
					System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, smallButtonRect.Top, _iList.ImageSize.Width + 1, _iList.ImageSize.Height + 1);
					
					// Draw border of button
					g.DrawRectangle(_pen, destRect);

					// Draw background color
					if(_brushColor != System.Drawing.Color.Transparent)
					{
						destRect = new System.Drawing.Rectangle(x + _lineSize, smallButtonRect.Top + _lineSize, _iList.ImageSize.Width, _iList.ImageSize.Height);
						g.FillRectangle(_brush, destRect);
					}

					// Draw image
					g.DrawImage(_iList.Images[i], x + 1, smallButtonRect.Top + 1);
				}
			}
		}

		private void DrawLine(System.Drawing.Graphics g, System.Drawing.Rectangle smallButtonRect, System.Drawing.Rectangle largeButtonRect)
		{
			int middleLine = smallButtonRect.Top + (smallButtonRect.Bottom - smallButtonRect.Top) / 2;

			g.DrawLine(_pen, 0, middleLine, smallButtonRect.Left, middleLine);
			g.DrawLine(_pen, smallButtonRect.Right, middleLine, this.Width, middleLine);

			// Drawing lines between boxes
			Pen blackPen = new Pen(_pen.Color, _lineSize);
			blackPen.DashPattern = new float[] {_iList.ImageSize.Width / 2, _iList.ImageSize.Width + _lineSize * 2};
			
			if(_selectedIndex != -1)
			{
				if(_selectedIndex != 0) // Draw line until selectedIndex
					g.DrawLine(blackPen, new Point(smallButtonRect.Left, middleLine), new Point(smallButtonRect.Left + _selectedIndex * _sizeOfButtonBorderAndSpace, middleLine));
				
				if(_selectedIndex != (_iList.Images.Count - 1)) // Draw line after selectedIndex
					g.DrawLine(blackPen, new Point(smallButtonRect.Left + (_selectedIndex + 2) * _sizeOfButtonBorderAndSpace, middleLine), new Point(smallButtonRect.Right, middleLine));
			}
			else
			{
				g.DrawLine(blackPen, new Point(smallButtonRect.Left, middleLine), new Point(smallButtonRect.Right, middleLine));
			}
		}

		#endregion

		#region Properties
		[System.ComponentModel.Category("Behavior")]
		public System.Windows.Forms.ImageList ImageList
		{
			get {return _iList;}
			set 
			{
				_iList = value;
				
				if(_iList != null)
				{
					this.Height = (_iList.ImageSize.Height + _lineSize + _offset - _smaller)* 2;
					_sizeOfButtonBorderAndSpace = _iList.ImageSize.Width + _lineSize * 2 + _iList.ImageSize.Width / 2;
				}

				this.Invalidate();
			}
		}

		[System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Where to place the toolbar in the control")]
		public System.Drawing.ContentAlignment ContentAlignment
		{
			get {return _alignment;}
			set 
			{
				_alignment = value;
				this.Invalidate();
			}
		}

		[System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The color of the toolbar items bounding box when it is being pressed")]
		public System.Drawing.Color LineColorDown
		{
			get {return _penDown.Color;}
			set 
			{
				_penDown = new System.Drawing.Pen(value, _lineSize);
				this.Invalidate();
			}
		}

		[System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The color of the toolbar items bounding box when the mouse is over")]
		public System.Drawing.Color LineColorOver
		{
			get {return _penOver.Color;}
			set 
			{
				_penOver = new System.Drawing.Pen(value, _lineSize);
				this.Invalidate();
			}
		}

		[System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The color of all lines by default")]
		public System.Drawing.Color LineColorNormal
		{
			get {return _pen.Color;}
			set 
			{
				_pen = new System.Drawing.Pen(value, _lineSize);
				this.Invalidate();
			}
		}

		[System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The toolbar item background by default. This only shows for pictures with transparent background")]
		public System.Drawing.Color BackgroundColorNormal
		{
			get {return _brushColor;}
			set 
			{
				_brushColor = value;
				_brush = new System.Drawing.SolidBrush(_brushColor);
				this.Invalidate();
			}
		}

		[System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The menu item background color when mouse is over. This only shows for pictures with transparent background")]
		public System.Drawing.Color BackgroundColorOver
		{
			get {return _brushColorOver;}
			set 
			{
				_brushColorOver = value;
				_brushOver = new System.Drawing.SolidBrush(_brushColorOver);
				this.Invalidate();
			}
		}

		[System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The menu item background color when mouse is being pressed. This only shows for pictures with transparent background")]
		public System.Drawing.Color BackgroundColorDown
		{
			get {return _brushColorDown;}
			set 
			{
				_brushColorDown = value;
				_brushDown = new System.Drawing.SolidBrush(_brushColorDown);
				this.Invalidate();
			}
		}
		#endregion

		#region Overrides
		protected override void OnSizeChanged(EventArgs e)
		{
			if(_iList != null)
			{
				int correctHeight = (_iList.ImageSize.Height + _lineSize + _offset - _smaller) * 2;

				if (this.Height != correctHeight)
					this.Height = correctHeight;
			}
			base.OnSizeChanged (e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			System.Drawing.Rectangle smallButtonRect = new System.Drawing.Rectangle(0, 0, 0, 0);
			System.Drawing.Rectangle largeButtonRect = smallButtonRect;

			CalculateRect(ref smallButtonRect, ref largeButtonRect);

			this.DrawLine(e.Graphics, smallButtonRect, largeButtonRect);
			this.DrawPicturesAndFrames(e.Graphics, smallButtonRect, largeButtonRect);
            			
			base.OnPaint (e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			bool invalidate = false;

			System.Drawing.Rectangle smallButtonRect = new System.Drawing.Rectangle(0, 0, 0, 0);
			System.Drawing.Rectangle largeButtonRect = smallButtonRect;

			CalculateRect(ref smallButtonRect, ref largeButtonRect);

			if(smallButtonRect.Contains(e.X, e.Y))
			{
				int mouseXWithoutOffset = e.X - smallButtonRect.Left - _iList.ImageSize.Width / 2;
				int buttonIndex = (int)System.Math.Floor((double)mouseXWithoutOffset / _sizeOfButtonBorderAndSpace);

				if((mouseXWithoutOffset - buttonIndex * _sizeOfButtonBorderAndSpace) <= _iList.ImageSize.Width) // mouse over index = buttonIndex
				{
					invalidate = buttonIndex != _selectedIndex;

					_state = e.Button == System.Windows.Forms.MouseButtons.Left ? State.Down : State.Over;

					_selectedIndex = buttonIndex;
				}
				else // mouse over nothing
				{
					invalidate = _selectedIndex != -1;
					_selectedIndex = -1;
				}
			}
			else // mouse over nothing
			{
				invalidate = _selectedIndex != -1;
				_selectedIndex = -1;
			}

			base.OnMouseMove (e);

			if(invalidate)
			{
				this.InvokeItemChanged(_selectedIndex);
				this.Invalidate();
			}
		}
	
		protected override void OnMouseLeave(EventArgs e)
		{
			bool invalidate = _selectedIndex != -1;
			_selectedIndex = -1;

			base.OnMouseLeave (e);

			if(invalidate)
				this.Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			bool invalidate = false;

			if(_selectedIndex != -1)
			{
				invalidate = true;
				_state = State.Down;
			}

			base.OnMouseDown (e);

			if(invalidate)
				this.Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			bool invalidate = false;

			if(_selectedIndex != -1)
			{
				this.InvokeClicked(_selectedIndex);
				invalidate = true;
				_state = State.Over;
			}

			base.OnMouseUp (e);

			if(invalidate)
				this.Invalidate();
		}
		#endregion
	}
}
