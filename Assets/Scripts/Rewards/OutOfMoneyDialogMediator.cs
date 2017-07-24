﻿using System;
using Core;
using Dialog;
using UnityEngine.UI;

namespace Ambition
{
	public class OutOfMoneyDialogMediator : DialogView
	{
		private const string PHRASE_ID = "out_of_money_dialog";

		public Text BodyTxt;
		public Text TitleTxt;
		public Text ButtonLabelTxt;

		void Start()
		{
			LocalizationModel localization = AmbitionApp.GetModel<LocalizationModel>();
			TitleTxt.text = localization.GetString(PHRASE_ID + DialogConsts.TITLE);
			if (AmbitionApp.GetModel<GameModel>().Reputation > 20)
			{
				BodyTxt.text = localization.GetString(PHRASE_ID + DialogConsts.BODY);
				ButtonLabelTxt.text = localization.GetString(DialogConsts.OK);
			}
			else
			{
				BodyTxt.text = localization.GetString("out_of_money_and_rep_dialog" + DialogConsts.BODY);
				ButtonLabelTxt.text = localization.GetString("out_of_money_and_rep_dialog" + DialogConsts.OK);
			}
		}

		void OnDestroy()
		{
			AmbitionApp.Execute<BorrowMoneyCmd>();
		}
	}
}