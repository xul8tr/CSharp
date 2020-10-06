using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Permissions;

namespace ShoWinCS
{
    /// <summary>
    /// Summary description for SpyWindow.
    /// </summary>
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class SpyWindow : System.Windows.Forms.Form
    {			
        //public event DisplayImageEventHandler ImageReadyForDisplay;
        private bool _realOnly;
        private bool _childSearch;
        private bool _capturing;
        private Image _finderHome;
        private Image _finderGone;		
        private Cursor _cursorDefault;
        private Cursor _cursorFinder;
        private IntPtr _hPreviousWindow;
        private IntPtr _parentHandle;
        private IntPtr _childHandle;

        private System.Windows.Forms.Button _buttonOK;
        private System.Windows.Forms.Button _buttonCancel;
        private System.Windows.Forms.Button _buttonCapture;
        private System.Windows.Forms.PictureBox _pictureBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _textBoxRect;
        private System.Windows.Forms.TextBox _textBoxStyle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _textBoxHandle;
        private System.Windows.Forms.TextBox _textBoxClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _textBoxText;
        private System.Windows.Forms.Label label2;
        private Button SetCaption;
        private Button button_Enable;
        private CheckBox checkBox_Child;
        private Label label8;
        private TextBox textBox1;
        private CheckBox checkBox_RealOnly;
        private Label label9;
        private TextBox textBox_Message;
        private Button button_Send;
        private Label label10;
        private TextBox textBox_L;
        private Label label11;
        private TextBox textBox_W;
        private ComboBox comboBox_WmMessages;		
        private System.ComponentModel.Container components = null;
        private TextBox textBoxResult;
        private Label label12;
        private System.Collections.Specialized.HybridDictionary myHibridDictionary = new System.Collections.Specialized.HybridDictionary();

        /// <summary>
        /// Initializes a new instance of the SpyWindow class
        /// </summary>
        public SpyWindow()
        {
            UIPermission myPermission = new UIPermission(UIPermissionWindow.AllWindows);
            myPermission.Demand();
            this.InitializeComponent();	
            
            _cursorDefault = Cursor.Current;
            _cursorFinder = EmbeddedResources.LoadCursor(EmbeddedResources.Finder);
            _finderHome = EmbeddedResources.LoadImage(EmbeddedResources.FinderHome);
            _finderGone = EmbeddedResources.LoadImage(EmbeddedResources.FinderGone);
            
            _pictureBox.Image = _finderHome;
            _pictureBox.MouseDown += new MouseEventHandler(OnFinderToolMouseDown);	
            _buttonOK.Click += new EventHandler(OnButtonOKClicked);
            _buttonCancel.Click += new EventHandler(OnButtonCancelClicked);
            _buttonCapture.Click += new EventHandler(OnButtonCaptureClicked);
            _buttonCapture.Enabled = false;
            _textBoxHandle.TextChanged += new EventHandler(OnTextBoxHandleTextChanged);

            //this.AcceptButton = _buttonOK;
            this.CancelButton = _buttonCancel;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();

                    if (_capturing)
                        this.CaptureMouse(false);
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
            this._buttonOK = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._buttonCapture = new System.Windows.Forms.Button();
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._textBoxRect = new System.Windows.Forms.TextBox();
            this._textBoxStyle = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._textBoxHandle = new System.Windows.Forms.TextBox();
            this._textBoxClass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._textBoxText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SetCaption = new System.Windows.Forms.Button();
            this.button_Enable = new System.Windows.Forms.Button();
            this.checkBox_Child = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox_RealOnly = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_Message = new System.Windows.Forms.TextBox();
            this.button_Send = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_L = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_W = new System.Windows.Forms.TextBox();
            this.comboBox_WmMessages = new System.Windows.Forms.ComboBox();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _buttonOK
            // 
            this._buttonOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._buttonOK.Location = new System.Drawing.Point(476, 263);
            this._buttonOK.Name = "_buttonOK";
            this._buttonOK.Size = new System.Drawing.Size(75, 23);
            this._buttonOK.TabIndex = 7;
            this._buttonOK.Text = "OK";
            // 
            // _buttonCancel
            // 
            this._buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._buttonCancel.Location = new System.Drawing.Point(556, 263);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 23);
            this._buttonCancel.TabIndex = 8;
            this._buttonCancel.Text = "Cancel";
            // 
            // _buttonCapture
            // 
            this._buttonCapture.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._buttonCapture.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._buttonCapture.Location = new System.Drawing.Point(290, 123);
            this._buttonCapture.Name = "_buttonCapture";
            this._buttonCapture.Size = new System.Drawing.Size(75, 25);
            this._buttonCapture.TabIndex = 14;
            this._buttonCapture.Text = "Capture";
            // 
            // _pictureBox
            // 
            this._pictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._pictureBox.Location = new System.Drawing.Point(105, 83);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(32, 32);
            this._pictureBox.TabIndex = 27;
            this._pictureBox.TabStop = false;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.Location = new System.Drawing.Point(30, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 20);
            this.label7.TabIndex = 26;
            this.label7.Text = "Finder Tool:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.Location = new System.Drawing.Point(25, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(611, 40);
            this.label6.TabIndex = 25;
            this.label6.Text = "Drag the Finder Tool over a window to select it, then release the mouse button. O" +
                "r enter a window handle (in hexadecimal).";
            // 
            // _textBoxRect
            // 
            this._textBoxRect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._textBoxRect.BackColor = System.Drawing.SystemColors.Window;
            this._textBoxRect.Location = new System.Drawing.Point(80, 226);
            this._textBoxRect.Name = "_textBoxRect";
            this._textBoxRect.Size = new System.Drawing.Size(285, 20);
            this._textBoxRect.TabIndex = 24;
            // 
            // _textBoxStyle
            // 
            this._textBoxStyle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._textBoxStyle.BackColor = System.Drawing.SystemColors.Window;
            this._textBoxStyle.Location = new System.Drawing.Point(80, 201);
            this._textBoxStyle.Name = "_textBoxStyle";
            this._textBoxStyle.Size = new System.Drawing.Size(285, 20);
            this._textBoxStyle.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.Location = new System.Drawing.Point(25, 226);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 20);
            this.label5.TabIndex = 22;
            this.label5.Text = "Rect:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.Location = new System.Drawing.Point(25, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 20);
            this.label4.TabIndex = 21;
            this.label4.Text = "Style:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.Location = new System.Drawing.Point(25, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "Caption:";
            // 
            // _textBoxHandle
            // 
            this._textBoxHandle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._textBoxHandle.BackColor = System.Drawing.SystemColors.Window;
            this._textBoxHandle.Location = new System.Drawing.Point(80, 126);
            this._textBoxHandle.Name = "_textBoxHandle";
            this._textBoxHandle.Size = new System.Drawing.Size(204, 20);
            this._textBoxHandle.TabIndex = 18;
            // 
            // _textBoxClass
            // 
            this._textBoxClass.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._textBoxClass.BackColor = System.Drawing.SystemColors.Window;
            this._textBoxClass.Location = new System.Drawing.Point(80, 176);
            this._textBoxClass.Name = "_textBoxClass";
            this._textBoxClass.Size = new System.Drawing.Size(285, 20);
            this._textBoxClass.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Location = new System.Drawing.Point(25, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "Parent:";
            // 
            // _textBoxText
            // 
            this._textBoxText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._textBoxText.BackColor = System.Drawing.SystemColors.Window;
            this._textBoxText.Location = new System.Drawing.Point(80, 151);
            this._textBoxText.Name = "_textBoxText";
            this._textBoxText.Size = new System.Drawing.Size(204, 20);
            this._textBoxText.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.Location = new System.Drawing.Point(25, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "Class:";
            // 
            // SetCaption
            // 
            this.SetCaption.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SetCaption.Location = new System.Drawing.Point(290, 151);
            this.SetCaption.Name = "SetCaption";
            this.SetCaption.Size = new System.Drawing.Size(75, 23);
            this.SetCaption.TabIndex = 28;
            this.SetCaption.Text = "Set Caption";
            this.SetCaption.UseVisualStyleBackColor = true;
            this.SetCaption.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_Enable
            // 
            this.button_Enable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_Enable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Enable.Location = new System.Drawing.Point(371, 124);
            this.button_Enable.Name = "button_Enable";
            this.button_Enable.Size = new System.Drawing.Size(75, 25);
            this.button_Enable.TabIndex = 14;
            this.button_Enable.Text = "Enable";
            this.button_Enable.Click += new System.EventHandler(this.button_Enable_Click);
            // 
            // checkBox_Child
            // 
            this.checkBox_Child.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBox_Child.AutoSize = true;
            this.checkBox_Child.Location = new System.Drawing.Point(460, 129);
            this.checkBox_Child.Name = "checkBox_Child";
            this.checkBox_Child.Size = new System.Drawing.Size(84, 17);
            this.checkBox_Child.TabIndex = 29;
            this.checkBox_Child.Text = "Child search";
            this.checkBox_Child.UseVisualStyleBackColor = true;
            this.checkBox_Child.CheckedChanged += new System.EventHandler(this.checkBox_Child_CheckedChanged);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.Location = new System.Drawing.Point(371, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 20);
            this.label8.TabIndex = 15;
            this.label8.Text = "Handle:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(427, 151);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(204, 20);
            this.textBox1.TabIndex = 18;
            // 
            // checkBox_RealOnly
            // 
            this.checkBox_RealOnly.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBox_RealOnly.AutoSize = true;
            this.checkBox_RealOnly.Location = new System.Drawing.Point(550, 129);
            this.checkBox_RealOnly.Name = "checkBox_RealOnly";
            this.checkBox_RealOnly.Size = new System.Drawing.Size(70, 17);
            this.checkBox_RealOnly.TabIndex = 30;
            this.checkBox_RealOnly.Text = "Real only";
            this.checkBox_RealOnly.UseVisualStyleBackColor = true;
            this.checkBox_RealOnly.CheckedChanged += new System.EventHandler(this.checkBox_RealOnly_CheckedChanged);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.Location = new System.Drawing.Point(25, 252);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 20);
            this.label9.TabIndex = 22;
            this.label9.Text = "Message:";
            // 
            // textBox_Message
            // 
            this.textBox_Message.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox_Message.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Message.Location = new System.Drawing.Point(80, 275);
            this.textBox_Message.Name = "textBox_Message";
            this.textBox_Message.Size = new System.Drawing.Size(121, 20);
            this.textBox_Message.TabIndex = 24;
            this.textBox_Message.Leave += new System.EventHandler(this.textBox_Message_Leave);
            // 
            // button_Send
            // 
            this.button_Send.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_Send.Location = new System.Drawing.Point(408, 199);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new System.Drawing.Size(75, 23);
            this.button_Send.TabIndex = 28;
            this.button_Send.Text = "Send";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new System.EventHandler(this.button_Send_Click);
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.Location = new System.Drawing.Point(287, 252);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 20);
            this.label10.TabIndex = 22;
            this.label10.Text = "L:";
            // 
            // textBox_L
            // 
            this.textBox_L.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox_L.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_L.Location = new System.Drawing.Point(307, 249);
            this.textBox_L.Name = "textBox_L";
            this.textBox_L.Size = new System.Drawing.Size(58, 20);
            this.textBox_L.TabIndex = 24;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.Location = new System.Drawing.Point(206, 252);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 20);
            this.label11.TabIndex = 22;
            this.label11.Text = "W:";
            // 
            // textBox_W
            // 
            this.textBox_W.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox_W.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_W.Location = new System.Drawing.Point(226, 249);
            this.textBox_W.Name = "textBox_W";
            this.textBox_W.Size = new System.Drawing.Size(58, 20);
            this.textBox_W.TabIndex = 24;
            // 
            // comboBox_WmMessages
            // 
            this.comboBox_WmMessages.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBox_WmMessages.FormattingEnabled = true;
            this.comboBox_WmMessages.Location = new System.Drawing.Point(80, 251);
            this.comboBox_WmMessages.Name = "comboBox_WmMessages";
            this.comboBox_WmMessages.Size = new System.Drawing.Size(121, 21);
            this.comboBox_WmMessages.TabIndex = 31;
            this.comboBox_WmMessages.SelectedIndexChanged += new System.EventHandler(this.comboBox_WmMessages_SelectedIndexChanged);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(250, 275);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(115, 20);
            this.textBoxResult.TabIndex = 32;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(207, 278);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 33;
            this.label12.Text = "Result";
            // 
            // SpyWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(659, 318);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.comboBox_WmMessages);
            this.Controls.Add(this.checkBox_RealOnly);
            this.Controls.Add(this.checkBox_Child);
            this.Controls.Add(this.button_Send);
            this.Controls.Add(this.SetCaption);
            this.Controls.Add(this._pictureBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_W);
            this.Controls.Add(this.textBox_L);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox_Message);
            this.Controls.Add(this.label10);
            this.Controls.Add(this._textBoxRect);
            this.Controls.Add(this.label9);
            this.Controls.Add(this._textBoxStyle);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this._textBoxHandle);
            this.Controls.Add(this.label8);
            this.Controls.Add(this._textBoxClass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._textBoxText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._buttonCancel);
            this.Controls.Add(this._buttonOK);
            this.Controls.Add(this.button_Enable);
            this.Controls.Add(this._buttonCapture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SpyWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find Window";
            this.Load += new System.EventHandler(this.SpyWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion		

        /// <summary>
        /// Processes window messages sent to the Spy Window
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {									
            switch(m.Msg)
            {
                /*
                 * stop capturing events as soon as the user releases the left mouse button
                 * */
                case (int)Win32.WindowMessages.WM_LBUTTONUP:
                    this.CaptureMouse(false);
                    break;
                /*
                 * handle all the mouse movements
                 * */
                case (int)Win32.WindowMessages.WM_MOUSEMOVE:
                    this.HandleMouseMovements();
                    break;			
            };

            base.WndProc (ref m);
        }
        
        /// <summary>
        /// Processes the mouse down events for the finder tool 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFinderToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.CaptureMouse(true);
        }

        ///// <summary>
        ///// Raises the ImageReadyForDisplay event
        ///// </summary>
        ///// <param name="image"></param>
        //protected virtual void OnImageReadyForDisplay(Image image)
        //{
        //    try
        //    {
        //        if (this.ImageReadyForDisplay != null)
        //            this.ImageReadyForDisplay(image, false, PictureBoxSizeMode.CenterImage);
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //}
        
        /// <summary>
        /// Returns the caption of a window
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        private string GetWindowText(IntPtr hWnd)
        {			
            StringBuilder sb = new StringBuilder(256);							
            Win32.GetWindowText(hWnd, sb, 256);								
            return sb.ToString();
        }

        /// <summary>
        /// Returns the name of a window's class
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        private string GetClassName(IntPtr hWnd)
        {			
            StringBuilder sb = new StringBuilder(256);							
            Win32.GetClassName(hWnd, sb, 256);								
            return sb.ToString();
        }

        /// <summary>
        /// Captures or releases the mouse
        /// </summary>
        /// <param name="captured"></param>
        private void CaptureMouse(bool captured)
        {
            // if we're supposed to capture the window
            if (captured)
            {
                // capture the mouse movements and send them to ourself
                Win32.SetCapture(this.Handle);

                // set the mouse cursor to our finder cursor
                Cursor.Current = _cursorFinder;

                // change the image to the finder gone image
                _pictureBox.Image = _finderGone;
            }
            // otherwise we're supposed to release the mouse capture
            else
            {
                // so release it
                Win32.ReleaseCapture();

                // put the default cursor back
                Cursor.Current = _cursorDefault;

                // change the image back to the finder at home image
                _pictureBox.Image = _finderHome;

                // and finally refresh any window that we were highlighting
                if (_hPreviousWindow != IntPtr.Zero)
                {
                    WindowHighlighter.Refresh(_hPreviousWindow);
                    //_hPreviousWindow = IntPtr.Zero;
                }
            }

            // save our capturing state
            _capturing = captured;
        }

        /// <summary>
        /// Handles all mouse move messages sent to the Spy Window
        /// </summary>
        private void HandleMouseMovements()
        {
            // if we're not capturing, then bail out
            if (!_capturing)
                return;
                                            
            try
            {
                IntPtr hWnd=IntPtr.Zero;
                _parentHandle = Win32.WindowFromPoint(Cursor.Position);
                hWnd = _parentHandle;
                // capture the window under the cursor's position
                if (_childSearch)
                {
                    //Point ClientPoint = Cursor.Position;
                    //Win32.ScreenToClient(_parentHandle,out ClientPoint);
                    //if (_realOnly)
                    //{
                    //    _childHandle = Win32.RealChildWindowFromPoint(_parentHandle, ClientPoint);
                    //}
                    //else
                    //{
                        //_childHandle = Win32.ChildWindowFromPoint(_parentHandle, ClientPoint);
                    _childHandle = GetChildMost(_parentHandle, Cursor.Position);
                     //}
                    hWnd = _childHandle;
                    
                }
               
                if (hWnd == IntPtr.Zero)
                {
                    return;
                }
                
                // if the window we're over, is not the same as the one before, and we had one before, refresh it
                if (_hPreviousWindow != IntPtr.Zero && _hPreviousWindow != hWnd)
                    WindowHighlighter.Refresh(_hPreviousWindow);

                // if we didn't find a window.. that's pretty hard to imagine. lol
                if (hWnd == IntPtr.Zero)
                {
                    _textBoxHandle.Text = null;
                    _textBoxClass.Text = null;
                    _textBoxText.Text = null;
                    _textBoxStyle.Text = null;
                    _textBoxRect.Text = null;
                }
                else
                {
                    // save the window we're over
                    _hPreviousWindow = hWnd;

                    // handle
                    _textBoxHandle.Text = string.Format("{0}", _parentHandle.ToInt32().ToString());

                    // class
                    _textBoxClass.Text = this.GetClassName(hWnd);

                    // caption
                    _textBoxText.Text = this.GetWindowText(hWnd);
                    
                    Win32.Rect rc = new Win32.Rect();
                    Win32.GetWindowRect(hWnd, ref rc);
                        
                    // rect
                    _textBoxRect.Text = string.Format("[{0} x {1}], ({2},{3})-({4},{5})", rc.right - rc.left, rc.bottom - rc.top, rc.left, rc.top, rc.right, rc.bottom);

                    // highlight the window
                    WindowHighlighter.Highlight(hWnd);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private IntPtr GetChildMost(IntPtr parent_window, Point parent_point)
        {
            Point testPoint = parent_point;
            Win32.ScreenToClient(parent_window, out testPoint);
            IntPtr child = Win32.ChildWindowFromPointEx(_parentHandle, testPoint, Win32.CWP_ALL);
            if (child == null || child == parent_window)
            {
                return parent_window;
            }

            return GetChildMost(child, testPoint);
        }

        /// <summary>
        /// Occurs when the OK button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonOKClicked(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Occurs when the Cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonCancelClicked(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Occurs when the user wants to capture the window that we've captured
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonCaptureClicked(object sender, EventArgs e)
        {
            //try
            //{
            //    // parse the window handle
            //    int handle = int.Parse(_textBoxHandle.Text);
                
            //    // capture that window
            //    Image image = ScreenCapturing.GetWindowCaptureAsBitmap(handle);
                
            //    // fire our image read event, which the main window will display for us
            //    this.OnImageReadyForDisplay(image);
            //}
            //catch(Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
        }

        /// <summary>
        /// Occurs when the handle textbox changes, to determine if the capture button is enabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTextBoxHandleTextChanged(object sender, EventArgs e)
        {
            _buttonCapture.Enabled = (_textBoxHandle.Text != null && _textBoxHandle.Text != string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Win32.SendMessage(_hPreviousWindow, (int)Win32.WindowMessages.WM_SETTEXT, IntPtr.Zero,_textBoxText.Text);
            //TabPage myTab = new TabPage();
            
            //NativeWindow windowTarget = (NativeWindow)myTab.;
            //NativeWindow myNative = new NativeWindow();
            //myNative.AssignHandle(_hPreviousWindow);
            //Control myControl = Control.FromHandle(_hPreviousWindow);
            //if (myControl == null)
            //{
            //    myControl = Control.FromChildHandle(_hPreviousWindow);
            //}
            //if (myControl != null) ;
        }

        private void button_Enable_Click(object sender, EventArgs e)
        {
            Win32.SendMessage(_hPreviousWindow, (int)Win32.WindowMessages.WM_ENABLE, IntPtr.Zero, IntPtr.Zero);
        }

        private void checkBox_Child_CheckedChanged(object sender, EventArgs e)
        {
            _childSearch = (sender as CheckBox).Checked;
        }

        private void checkBox_RealOnly_CheckedChanged(object sender, EventArgs e)
        {
            _realOnly = (sender as CheckBox).Checked;
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            //TCM_GETITEMRECT
            if (textBox_W.Text == String.Empty) textBox_W.Text = "0";
            if (textBox_L.Text == String.Empty) textBox_L.Text = "0";
            IntPtr wparam = new IntPtr(int.Parse(textBox_W.Text));
            IntPtr lparam = new IntPtr(int.Parse(textBox_L.Text));
            int message = int.Parse(textBox_Message.Text);
            int ret = Win32.SendMessage(_hPreviousWindow,message,wparam,lparam);
            textBoxResult.Text = ret == 0 ?  "OK (" + ret.ToString() + ")" : "Failed (" + ret.ToString()+")";
        }

        private void SpyWindow_Load(object sender, EventArgs e)
        {           
            string[] enums = Enum.GetNames(typeof(ShoWinCS.Win32.WindowMessages));
            int[] values = (int[])Enum.GetValues(typeof(ShoWinCS.Win32.WindowMessages));
            for (int i=0;i<enums.Length;i++)
            {
                myHibridDictionary.Add(enums[i], values[i]);
                comboBox_WmMessages.Items.Add(enums[i].ToString());
            }
            comboBox_WmMessages.Text = comboBox_WmMessages.Items[8].ToString();
        }

        private void comboBox_WmMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Message.Text = myHibridDictionary[comboBox_WmMessages.Text].ToString();
        }

        private void textBox_Message_Leave(object sender, EventArgs e)
        {
            comboBox_WmMessages.Text = Enum.Parse(typeof(ShoWinCS.Win32.WindowMessages), textBox_Message.Text.ToString()).ToString();
        }
    }
}
