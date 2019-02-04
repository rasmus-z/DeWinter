using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;

namespace Ambition
{
	public class EventOptionButton : MonoBehaviour
	{
	    private Text _text;
		private int _option;
		private Button _btn;
        private TransitionVO _transition;

	    void Awake()
	    {
			_text = GetComponentInChildren<Text>();
			_option = gameObject.transform.GetSiblingIndex();
			_btn = gameObject.GetComponent<Button>();
			_btn.onClick.AddListener(OnClick);
			AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
			_btn.onClick.RemoveAllListeners();
	    }

		private void HandleTransitions(TransitionVO[] transitions)
	    {
			bool show = _option < transitions.Length && (transitions.Length > 1 || transitions[0].Text.Length > 0);
            _transition = show ? transitions[_option] : null;
            if (show) _text.text = _transition.Text;
			gameObject.SetActive(show);
	    }

		private void OnClick()
		{
            AmbitionApp.SendMessage(IncidentMessages.INCIDENT_OPTION, _transition);
		}
	}
}
