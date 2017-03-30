﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Dialog
{
	public class DialogCanvasManager : MonoBehaviour
	{
		public GameObject[] DialogPrefabs;

		protected Dictionary<string, GameObject> _prefabs;
		protected Dictionary<string, GameObject> _dialogs;
		protected Canvas _canvas;

		void Awake()
		{
			App.Service<DialogSvc>().RegisterManager(this);

			_canvas = this.gameObject.GetComponent<Canvas>();
			_prefabs = new Dictionary<string, GameObject>();
			_dialogs = new Dictionary<string, GameObject>();

			foreach (GameObject prefab in DialogPrefabs)
			{
				_prefabs.Add(prefab.name, prefab);
			}
		}

		public GameObject Open(string dialogID)
		{
			Close(dialogID);
			GameObject prefab;
			if (!_prefabs.TryGetValue(dialogID, out prefab))
				return null;
			
			GameObject dialog = GameObject.Instantiate<GameObject>(prefab);
			DialogView cmp = dialog.GetComponent<DialogView>();
			if (cmp != null) cmp.Manager = this;
			_dialogs.Add(dialogID, dialog);
			dialog.transform.SetParent(_canvas.transform, false);
			dialog.GetComponent<RectTransform>().SetAsLastSibling();
			return dialog;
		}

		public GameObject Open<T>(string dialogID, T vo)
		{
			GameObject dlg = Open(dialogID);
			if (dlg != null)
			{
				DialogView cmp = dlg.GetComponent<DialogView>();
				if (cmp is IDialog<T>)
					(cmp as IDialog<T>).OnOpen(vo);
			}
			return dlg;
		}

		public bool Close(string dialogID)
		{
			GameObject dlog;
			if (_dialogs.TryGetValue(dialogID, out dlog))
			{
				HandleCloseDialog(dialogID, dlog);
				return true;
			}
			return false;
		}

		public bool Close(GameObject dialog)
		{
			foreach(KeyValuePair<string, GameObject> kvp in _dialogs)
			{
				if (kvp.Value == dialog)
				{
					HandleCloseDialog(kvp.Key, kvp.Value);
					return true;
				}
			}
			return false;
		}

		protected void HandleCloseDialog(string dialogID, GameObject dlg)
		{
			DialogView dlgCmp = dlg.GetComponent<DialogView>();
			if (dlgCmp is IDisposable)
			{
				(dlgCmp as IDisposable).Dispose();
			}
			_dialogs.Remove(dialogID);
			GameObject.Destroy(dlg);
		}

		public void CloseAll()
		{
			DialogView view;
			foreach(KeyValuePair<string, GameObject> kvp in _dialogs)
			{
				view = kvp.Value.GetComponent<DialogView>();
				if (view is IDisposable)
				{
					(view as IDisposable).Dispose();
				}
				GameObject.Destroy(kvp.Value);
			}
			_dialogs.Clear();
		}
	}
}