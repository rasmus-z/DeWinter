﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Dialog;

namespace Ambition
{
	public class RoomViewMediator : MonoBehaviour
	{
		void Start()
		{
			AmbitionApp.SendMessage(PartyMessages.CLEAR_REMARKS);
			RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
			AmbitionApp.Execute<GenerateGuestsCmd, RoomVO>(room);
			AmbitionApp.OpenDialog(DialogConsts.READY_GO);
		}

	    // Poll for input
	    void Update()
	    {
			if(Input.GetKeyDown(KeyCode.D))
	        {
	        	AmbitionApp.SendMessage(PartyMessages.DRINK);
	        }
			else if (Input.GetKeyDown(KeyCode.C))
	        {
				AmbitionApp.SendMessage(PartyMessages.BUY_REMARK);
	        }
			else if (Input.GetKeyDown(KeyCode.E))
	        {
				AmbitionApp.SendMessage(PartyMessages.EXCHANGE_REMARK, AmbitionApp.GetModel<PartyModel>().Remark);
	        }
        }
	}
}