﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using Core;

namespace Ambition
{
	public class NoOutfitModal : DialogView
	{
		private const string DIALOG_PHRASE = "no_outfit_dialog";

		public Text BodyText;
		public Text TitleText;
	    private MainScreenTabsController tabsController;

	    public override void OnOpen ()
		{
			base.OnOpen ();
	    	LocalizationModel localization = AmbitionApp.GetModel<LocalizationModel>();
			BodyText.text = localization.GetString(DIALOG_PHRASE + DialogConsts.BODY);
			TitleText.text = localization.GetString(DIALOG_PHRASE + DialogConsts.TITLE);

	        tabsController = GameObject.Find("MainScreenTabsContainer").GetComponent<MainScreenTabsController>();
	    }

	    public void CreateCancellationModal()
	    {
	    	CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            PartyVO party = Array.Find(calendar.GetEvents<PartyVO>(), p => p.RSVP == RSVP.Accepted);
            if (party != null) AmbitionApp.OpenDialog(DialogConsts.CANCEL, party);
	    }

        public void GoToTheMerchant() => tabsController.WardrobeSelected();
	}
}