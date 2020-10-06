using System;
using System.Reflection;
using MAPI;
using System.Collections;

namespace MapiObjects
{
	/// <summary>
	/// Nicer interface on top of the MAPI stuff...
	/// </summary>
	public class MapiMessages: IEnumerable
	{
		public class MapiMessagesEnumerator: IEnumerator
		{
			Messages messages;	// MAPI Messagess object...
			Message message = null;

			public MapiMessagesEnumerator(Messages messages)
			{
				this.messages = messages;
			}

			bool IEnumerator.MoveNext()
			{
				if (message == null)
					message = (Message) messages.GetFirst(null);
				else
					message = (Message) messages.GetNext();

				if (message == null)
				{
					messages.GetLast(null);
					return false;
				}
				else
					return true;
			}

			void IEnumerator.Reset()
			{
				message = null;
			}

			object IEnumerator.Current
			{
				get
				{
					return message;
				}
			}

		}
		
		Messages messages;

		public Messages Messages
		{
			get
			{
				return messages;
			}
		}

		public MapiMessages(Messages messages)
		{
			this.messages = messages;
		}

		public IEnumerator GetEnumerator()
		{
			return new MapiMessagesEnumerator(messages);
		}
	}
}