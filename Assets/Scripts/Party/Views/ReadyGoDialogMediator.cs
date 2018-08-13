﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using Dialog;
using Core;

namespace Ambition
{
	public class ReadyGoDialogMediator : DialogView, IPointerClickHandler
	{
		public const string DIALOG_ID = "READY_GO";
        public Text DialogText;
		void Start()
		{
			LocalizationModel model = AmbitionApp.GetModel<LocalizationModel>();
			string[] conversationIntroList = model.GetList("conversation_intro");
			DialogText.text = conversationIntroList[Util.RNG.Generate(conversationIntroList.Length)];
		}

		public void OnPointerClick(PointerEventData eventData)
	    {
			Close();
        }

		public override void OnClose ()
		{
			AmbitionApp.SendMessage<string>(GameMessages.DIALOG_CLOSED, ID);
		}
	}
}
