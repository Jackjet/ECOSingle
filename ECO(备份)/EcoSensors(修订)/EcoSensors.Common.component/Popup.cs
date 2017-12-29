using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
namespace EcoSensors.Common.component
{
	[System.CLSCompliant(true), ToolboxItem(false)]
	public class Popup : ToolStripDropDown
	{
		private IContainer components;
		private ToolStripControlHost _host;
		private Control _opener;
		private Popup _ownerPopup;
		private Popup _childPopup;
		private bool _resizableTop;
		private bool _resizableLeft;
		private bool _isChildPopupOpened;
		private bool _resizable;
		private bool _nonInteractive;
		private VisualStyleRenderer _sizeGripRenderer;
		public Control Content
		{
			get;
			private set;
		}
		public PopupAnimations ShowingAnimation
		{
			get;
			set;
		}
		public PopupAnimations HidingAnimation
		{
			get;
			set;
		}
		public int AnimationDuration
		{
			get;
			set;
		}
		public bool FocusOnOpen
		{
			get;
			set;
		}
		public bool AcceptAlt
		{
			get;
			set;
		}
		public bool Resizable
		{
			get
			{
				return this._resizable && !this._isChildPopupOpened;
			}
			set
			{
				this._resizable = value;
			}
		}
		public bool NonInteractive
		{
			get
			{
				return this._nonInteractive;
			}
			set
			{
				if (value != this._nonInteractive)
				{
					this._nonInteractive = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}
		public new Size MinimumSize
		{
			get;
			set;
		}
		public new Size MaximumSize
		{
			get;
			set;
		}
		protected override CreateParams CreateParams
		{
			[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 134217728;
				if (this.NonInteractive)
				{
					createParams.ExStyle |= 524448;
				}
				return createParams;
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.components != null)
				{
					this.components.Dispose();
				}
				if (this.Content != null)
				{
					Control content = this.Content;
					this.Content = null;
					content.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.components = new Container();
		}
		public Popup(Control content)
		{
            //Popup <>4__this = this;
			if (content == null)
			{
				throw new System.ArgumentNullException("content");
			}
			this.Content = content;
			this.FocusOnOpen = true;
			this.AcceptAlt = true;
			this.ShowingAnimation = PopupAnimations.SystemDefault;
			this.HidingAnimation = PopupAnimations.None;
			this.AnimationDuration = 100;
			this.InitializeComponent();
			this.AutoSize = false;
			this.DoubleBuffered = true;
			base.ResizeRedraw = true;
			this._host = new ToolStripControlHost(content);
			base.Padding = (base.Margin = (this._host.Padding = (this._host.Margin = Padding.Empty)));
			if (NativeMethods.IsRunningOnMono)
			{
				content.Margin = Padding.Empty;
			}
			this.MinimumSize = content.MinimumSize;
			content.MinimumSize = content.Size;
			this.MaximumSize = content.MaximumSize;
			content.MaximumSize = content.Size;
			base.Size = content.Size;
			if (NativeMethods.IsRunningOnMono)
			{
				this._host.Size = content.Size;
			}
			base.TabStop = (content.TabStop = true);
			content.Location = Point.Empty;
			this.Items.Add(this._host);
			content.Disposed += delegate(object sender, System.EventArgs e)
			{
				content = null;
				this.Dispose(true);
			};
			content.RegionChanged += delegate(object sender, System.EventArgs e)
			{
				this.UpdateRegion();
			};
			content.Paint += delegate(object sender, PaintEventArgs e)
			{
				this.PaintSizeGrip(e);
			};
			this.UpdateRegion();
		}
		protected override void OnVisibleChanged(System.EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (NativeMethods.IsRunningOnMono)
			{
				return;
			}
			if ((base.Visible && this.ShowingAnimation == PopupAnimations.None) || (!base.Visible && this.HidingAnimation == PopupAnimations.None))
			{
				return;
			}
			NativeMethods.AnimationFlags animationFlags = base.Visible ? NativeMethods.AnimationFlags.Roll : NativeMethods.AnimationFlags.Hide;
			PopupAnimations popupAnimations = base.Visible ? this.ShowingAnimation : this.HidingAnimation;
			if (popupAnimations == PopupAnimations.SystemDefault)
			{
				if (SystemInformation.IsMenuAnimationEnabled)
				{
					if (SystemInformation.IsMenuFadeEnabled)
					{
						popupAnimations = PopupAnimations.Blend;
					}
					else
					{
						popupAnimations = (PopupAnimations.Slide | (base.Visible ? PopupAnimations.TopToBottom : PopupAnimations.BottomToTop));
					}
				}
				else
				{
					popupAnimations = PopupAnimations.None;
				}
			}
			if ((popupAnimations & (PopupAnimations.Center | PopupAnimations.Slide | PopupAnimations.Blend | PopupAnimations.Roll)) == PopupAnimations.None)
			{
				return;
			}
			if (this._resizableTop)
			{
				if ((popupAnimations & PopupAnimations.BottomToTop) != PopupAnimations.None)
				{
					popupAnimations = ((popupAnimations & ~PopupAnimations.BottomToTop) | PopupAnimations.TopToBottom);
				}
				else
				{
					if ((popupAnimations & PopupAnimations.TopToBottom) != PopupAnimations.None)
					{
						popupAnimations = ((popupAnimations & ~PopupAnimations.TopToBottom) | PopupAnimations.BottomToTop);
					}
				}
			}
			if (this._resizableLeft)
			{
				if ((popupAnimations & PopupAnimations.RightToLeft) != PopupAnimations.None)
				{
					popupAnimations = ((popupAnimations & ~PopupAnimations.RightToLeft) | PopupAnimations.LeftToRight);
				}
				else
				{
					if ((popupAnimations & PopupAnimations.LeftToRight) != PopupAnimations.None)
					{
						popupAnimations = ((popupAnimations & ~PopupAnimations.LeftToRight) | PopupAnimations.RightToLeft);
					}
				}
			}
			animationFlags |= (NativeMethods.AnimationFlags)((PopupAnimations)1048575 & popupAnimations);
			NativeMethods.SetTopMost(this);
			NativeMethods.AnimateWindow(this, this.AnimationDuration, animationFlags);
		}
		[System.Security.Permissions.UIPermission(System.Security.Permissions.SecurityAction.LinkDemand, Window = System.Security.Permissions.UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (this.AcceptAlt && (keyData & Keys.Alt) == Keys.Alt)
			{
				if ((keyData & Keys.F4) != Keys.F4)
				{
					return false;
				}
				base.Close();
			}
			bool flag = base.ProcessDialogKey(keyData);
			if (!flag && (keyData == Keys.Tab || keyData == (Keys.LButton | Keys.Back | Keys.Shift)))
			{
				bool flag2 = (keyData & Keys.Shift) == Keys.Shift;
				this.Content.SelectNextControl(null, !flag2, true, true, true);
			}
			return flag;
		}
		protected void UpdateRegion()
		{
			if (base.Region != null)
			{
				base.Region.Dispose();
				base.Region = null;
			}
			if (this.Content.Region != null)
			{
				base.Region = this.Content.Region.Clone();
			}
		}
		public void Show(Control control)
		{
			if (control == null)
			{
				throw new System.ArgumentNullException("control");
			}
			this.Show(control, control.ClientRectangle);
		}
		public void Show(Rectangle area)
		{
			this._resizableTop = (this._resizableLeft = false);
			Point position = new Point(area.Left, area.Top + area.Height);
			Rectangle workingArea = Screen.FromControl(this).WorkingArea;
			if (position.X + base.Size.Width > workingArea.Left + workingArea.Width)
			{
				this._resizableLeft = true;
				position.X = workingArea.Left + workingArea.Width - base.Size.Width;
			}
			if (position.Y + base.Size.Height > workingArea.Top + workingArea.Height)
			{
				this._resizableTop = true;
				position.Y -= base.Size.Height + area.Height;
			}
			base.Show(position, ToolStripDropDownDirection.BelowRight);
		}
		public void Show(Control control, Rectangle area)
		{
			if (control == null)
			{
				throw new System.ArgumentNullException("control");
			}
			this.SetOwnerItem(control);
			this._resizableTop = (this._resizableLeft = false);
			Point point = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
			Rectangle workingArea = Screen.FromControl(control).WorkingArea;
			if (point.X + base.Size.Width > workingArea.Left + workingArea.Width)
			{
				this._resizableLeft = true;
				point.X = workingArea.Left + workingArea.Width - base.Size.Width;
			}
			if (point.Y + base.Size.Height > workingArea.Top + workingArea.Height)
			{
				this._resizableTop = true;
				point.Y -= base.Size.Height + area.Height;
			}
			point = control.PointToClient(point);
			base.Show(control, point, ToolStripDropDownDirection.BelowRight);
		}
		private void SetOwnerItem(Control control)
		{
			if (control == null)
			{
				return;
			}
			if (control is Popup)
			{
				Popup popup = control as Popup;
				this._ownerPopup = popup;
				this._ownerPopup._childPopup = this;
				base.OwnerItem = popup.Items[0];
				return;
			}
			if (this._opener == null)
			{
				this._opener = control;
			}
			if (control.Parent != null)
			{
				this.SetOwnerItem(control.Parent);
			}
		}
		protected override void OnSizeChanged(System.EventArgs e)
		{
			if (this.Content != null)
			{
				this.Content.MinimumSize = base.Size;
				this.Content.MaximumSize = base.Size;
				this.Content.Size = base.Size;
				this.Content.Location = Point.Empty;
			}
			base.OnSizeChanged(e);
		}
		protected override void OnLayout(LayoutEventArgs e)
		{
			if (!NativeMethods.IsRunningOnMono)
			{
				base.OnLayout(e);
				return;
			}
			Size preferredSize = this.GetPreferredSize(Size.Empty);
			if (this.AutoSize && preferredSize != base.Size)
			{
				base.Size = preferredSize;
			}
			this.SetDisplayedItems();
			this.OnLayoutCompleted(System.EventArgs.Empty);
			base.Invalidate();
		}
		protected override void OnOpening(CancelEventArgs e)
		{
			if (this.Content.IsDisposed || this.Content.Disposing)
			{
				e.Cancel = true;
				return;
			}
			this.UpdateRegion();
			base.OnOpening(e);
		}
		protected override void OnOpened(System.EventArgs e)
		{
			if (this._ownerPopup != null)
			{
				this._ownerPopup._isChildPopupOpened = true;
			}
			if (this.FocusOnOpen)
			{
				this.Content.Focus();
			}
			base.OnOpened(e);
		}
		protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
		{
			this._opener = null;
			if (this._ownerPopup != null)
			{
				this._ownerPopup._isChildPopupOpened = false;
			}
			base.OnClosed(e);
		}
		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (this.InternalProcessResizing(ref m, false))
			{
				return;
			}
			base.WndProc(ref m);
		}
		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
		public bool ProcessResizing(ref Message m)
		{
			return this.InternalProcessResizing(ref m, true);
		}
		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
		private bool InternalProcessResizing(ref Message m, bool contentControl)
		{
			if (m.Msg == 134 && m.WParam != System.IntPtr.Zero && this._childPopup != null && this._childPopup.Visible)
			{
				this._childPopup.Hide();
			}
			if (!this.Resizable && !this.NonInteractive)
			{
				return false;
			}
			if (m.Msg == 132)
			{
				return this.OnNcHitTest(ref m, contentControl);
			}
			return m.Msg == 36 && this.OnGetMinMaxInfo(ref m);
		}
		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
		private bool OnGetMinMaxInfo(ref Message m)
		{
			NativeMethods.MINMAXINFO mINMAXINFO = (NativeMethods.MINMAXINFO)System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.MINMAXINFO));
			if (!this.MaximumSize.IsEmpty)
			{
				mINMAXINFO.maxTrackSize = this.MaximumSize;
			}
			mINMAXINFO.minTrackSize = this.MinimumSize;
			System.Runtime.InteropServices.Marshal.StructureToPtr(mINMAXINFO, m.LParam, false);
			return true;
		}
		private bool OnNcHitTest(ref Message m, bool contentControl)
		{
			if (this.NonInteractive)
			{
				m.Result = (System.IntPtr)(-1);
				return true;
			}
			int x = Cursor.Position.X;
			int y = Cursor.Position.Y;
			Point pt = base.PointToClient(new Point(x, y));
			GripBounds gripBounds = new GripBounds(contentControl ? this.Content.ClientRectangle : base.ClientRectangle);
			System.IntPtr intPtr = new System.IntPtr(-1);
			if (this._resizableTop)
			{
				if (this._resizableLeft && gripBounds.TopLeft.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((System.IntPtr)13));
					return true;
				}
				if (!this._resizableLeft && gripBounds.TopRight.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((System.IntPtr)14));
					return true;
				}
				if (gripBounds.Top.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((System.IntPtr)12));
					return true;
				}
			}
			else
			{
				if (this._resizableLeft && gripBounds.BottomLeft.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((System.IntPtr)16));
					return true;
				}
				if (!this._resizableLeft && gripBounds.BottomRight.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((System.IntPtr)17));
					return true;
				}
				if (gripBounds.Bottom.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((System.IntPtr)15));
					return true;
				}
			}
			if (this._resizableLeft && gripBounds.Left.Contains(pt))
			{
				m.Result = (contentControl ? intPtr : ((System.IntPtr)10));
				return true;
			}
			if (!this._resizableLeft && gripBounds.Right.Contains(pt))
			{
				m.Result = (contentControl ? intPtr : ((System.IntPtr)11));
				return true;
			}
			return false;
		}
		public void PaintSizeGrip(PaintEventArgs e)
		{
			if (e == null || e.Graphics == null || !this._resizable)
			{
				return;
			}
			Size clientSize = this.Content.ClientSize;
			using (Bitmap bitmap = new Bitmap(16, 16))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					if (Application.RenderWithVisualStyles)
					{
						if (this._sizeGripRenderer == null)
						{
							this._sizeGripRenderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
						}
						this._sizeGripRenderer.DrawBackground(graphics, new Rectangle(0, 0, 16, 16));
					}
					else
					{
						ControlPaint.DrawSizeGrip(graphics, this.Content.BackColor, 0, 0, 16, 16);
					}
				}
				GraphicsState gstate = e.Graphics.Save();
				e.Graphics.ResetTransform();
				if (this._resizableTop)
				{
					if (this._resizableLeft)
					{
						e.Graphics.RotateTransform(180f);
						e.Graphics.TranslateTransform((float)(-(float)clientSize.Width), (float)(-(float)clientSize.Height));
					}
					else
					{
						e.Graphics.ScaleTransform(1f, -1f);
						e.Graphics.TranslateTransform(0f, (float)(-(float)clientSize.Height));
					}
				}
				else
				{
					if (this._resizableLeft)
					{
						e.Graphics.ScaleTransform(-1f, 1f);
						e.Graphics.TranslateTransform((float)(-(float)clientSize.Width), 0f);
					}
				}
				e.Graphics.DrawImage(bitmap, clientSize.Width - 16, clientSize.Height - 16 + 1, 16, 16);
				e.Graphics.Restore(gstate);
			}
		}
	}
}
