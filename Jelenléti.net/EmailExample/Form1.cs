using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using MAPI;
using System.Reflection;
using MapiObjects;

namespace EmailExample
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Session session = new Session();

			session.Logon(	"Default Outlook Profile", 
				Missing.Value,
				Missing.Value,
				Missing.Value,
				Missing.Value,
				Missing.Value,
				Missing.Value
				);

			Folder folder = (Folder) session.Inbox;

			Messages messages = (Messages) folder.Messages;

			int messageCount = (int) messages.Count;

			TraverseCalendar(session);
			//int totalSize = TraverseFolder(new MapiFolder(session.Inbox));
		}

		public int TraverseFolder(MapiFolder folder)
		{
			int size = 0;

			foreach (MapiFolder subFolder in folder)
			{
				size += TraverseFolder(subFolder);
			}

			foreach (MAPI.Message message in folder.Messages)
			{
				size += (int) message.Size;
			}
			return size;
		}

		public void TraverseCalendar(Session session)
		{
			Folder calendar =
				(Folder) session.GetDefaultFolder(ActMsgDefaultFolderTypes.ActMsgDefaultFolderCalendar);

			Messages messages = (Messages)
				calendar.Messages;
			
			AppointmentItem message = (AppointmentItem) messages.GetFirst(Missing.Value);
			while (message != null)
			{
				string subject = (string) message.Subject;
				message = (AppointmentItem) messages.GetNext();
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
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
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "Form1";
		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
	}
}
