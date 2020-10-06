using System;
using System.Reflection;
using MAPI;
using System.Collections;

namespace MapiObjects
{
	/// <summary>
	/// Nicer interface on top of the MAPI stuff...
	/// </summary>
	public class MapiFolder: IEnumerable
	{
		public class MapiFolderEnumerator: IEnumerator
		{
			Folders folders;	// MAPI folders object...
			Folder folder = null;

			public MapiFolderEnumerator(Folders folders)
			{
				this.folders = folders;
			}

			bool IEnumerator.MoveNext()
			{
				if (folder == null)
					folder = (Folder) folders.GetFirst();
				else
					folder = (Folder) folders.GetNext();

				if (folder == null)
				{
					folders.GetLast();
					return false;
				}
				else
					return true;
			}

			void IEnumerator.Reset()
			{
				folder = null;
			}

			object IEnumerator.Current
			{
				get
				{
					return new MapiFolder(folder);
				}
			}

		}
		
		Folder folder;

		public MapiFolder(object folder)
		{
			this.folder = (Folder) folder;
		}

		public IEnumerator GetEnumerator()
		{
			return new MapiFolderEnumerator((Folders) folder.Folders);
		}

		public MapiFolder FindSubFolder(string name)
		{
			foreach (MapiFolder subFolder in this)
			{
				if (subFolder.Name == name)
					return subFolder;
			}
			return null;
		}

		public MapiMessages Messages
		{
			get
			{
				return new MapiMessages((Messages) folder.Messages);
			}
		}

		public string Name
		{
			get
			{
				return (string) folder.Name;
			}
		}
	}
}
