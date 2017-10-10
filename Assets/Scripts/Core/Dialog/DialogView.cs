﻿using System;
using UnityEngine;
using Util;

namespace Dialog
{
	public class DialogView : MonoBehaviour
	{
		public string ID;

		internal DialogCanvasManager Manager;

		public void Close()
		{
			Manager.Close(this.gameObject);
		}

		public virtual void OnOpen() {}
		public virtual void OnClose() {}
	}
}
