﻿using System;
using UFlow;

namespace Ambition
{
	public class WaitForMessageTransition : UTransition
	{
		private string _messageID;

		public override bool InitializeAndValidate ()
		{
			if (Parameters != null && Parameters.Length > 0)
			{
				_messageID = Parameters[0] as String;
				AmbitionApp.Subscribe(_messageID, Validate);
			}
			return false;
		}

		public override void Dispose ()
		{
			AmbitionApp.Unsubscribe(_messageID, Validate);
		}
	}
}