﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ambition
{
	public class RemarkView : MonoBehaviour, IPointerClickHandler
	{
		public int Index;

		private RemarkVO _remark;

		void Awake ()
		{
			AmbitionApp.Subscribe<RemarkVO[]>(HandleHand);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RemarkVO[]>(HandleHand);
		}

		private void HandleHand(RemarkVO[] hand)
		{
			_remark = (Index < hand.Length) ? hand[Index] : null;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			AmbitionApp.GetModel<PartyModel>().Remark = _remark;
		}
	}
}