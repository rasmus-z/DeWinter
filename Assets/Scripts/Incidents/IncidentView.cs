﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Dialog;

namespace Ambition
{
	public class IncidentView : MonoBehaviour
	{
	    public Text titleText;
	    public Text descriptionText;
		public Text SpeakerName;
        public GameObject Header;

	    public AvatarView Character1;
		public AvatarView Character2;
		private Image _background;

	    void Awake ()
		{
            _background = gameObject.GetComponent<Image>();
            //AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident); This isn't working because Start Incident is the same message that creates this modal. 
            //It never has a chance to listen for the message, because the message already happened
            AmbitionApp.Subscribe<MomentVO>(HandleMoment);
            SetIncidentTitle();
        }

        void OnDestroy ()
		{
            //AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
	    }

        private void SetIncidentTitle()
        {
            CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            titleText.text = model.Incident.Name;
        }

        /*
        private void HandleIncident(IncidentVO incident)
        {
            if (incident != null) titleText.text = incident.Name;
            print("Incident Name = " + incident.Name);
        }*/

        private void HandleMoment(MomentVO moment)
		{
			if (moment != null)
			{
				descriptionText.text = moment.Text;
				if (moment.Background != null) _background.sprite=moment.Background;
				Character1.ID = moment.Character1.AvatarID;
				Character1.Pose = moment.Character1.Pose;
				Character2.ID = moment.Character2.AvatarID;
				Character2.Pose = moment.Character2.Pose;

                if (moment.Music.Name.Length > 0)
    				AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, moment.Music);
                if (moment.AmbientSFX.Name.Length > 0)
                    AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENTSFX, moment.AmbientSFX);
                if (moment.OneShotSFX.Name.Length > 0)
                    AmbitionApp.SendMessage(AudioMessages.PLAY_ONESHOTSFX, moment.OneShotSFX);

                SpeakerName.enabled = (moment.Speaker != SpeakerType.None);
				switch(moment.Speaker)
				{
					case SpeakerType.Player:
						SpeakerName.text = AmbitionApp.GetModel<GameModel>().PlayerName;
						break;
					case SpeakerType.Character1:
						SpeakerName.text = moment.Character1.Name;
						break;
					case SpeakerType.Character2:
						SpeakerName.text = moment.Character2.Name;
						break;
				}
			}
		}
	}
}
