﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

namespace Ambition
{
    public class LocationPin : MonoBehaviour
    {
        public IncidentConfig IntroIncidentConfig;
        public GameObject Scene;
        public bool OneShot;
        public bool Discoverable;
        public bool Visited; //Used to determine whether the intro incident should play
        public string LocationWindowDescription;
        public Sprite LocationModalSprite;
        public GameObject Label;
        public Text LabelText;
        public CommodityVO[] Requirements;

        private string _name;

        private void Awake()
        {
            AmbitionApp.Subscribe<string>(ParisMessages.ADD_LOCATION, HandleShow);
            AmbitionApp.Subscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHide);
            _name = LabelText.text;
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(ParisMessages.ADD_LOCATION, HandleShow);
            AmbitionApp.Unsubscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHide);
        }

        public void Select()
        {
            AmbitionApp.SendMessage<LocationPin>(ParisMessages.SELECT_LOCATION, this);
        }

        public void ShowLabel()
        {
            Label.SetActive(true);
        }

        public void HideLabel()
        {
            Label.SetActive(false);
        }

        private void HandleSelect(string location)
        {
            //Tooltip.SetActive(location == gameObject.name);
        }

        public string Name()
        {
            return _name;
        }

        private void HandleShow(string locationID)
        {
            if (locationID == name)
                gameObject.SetActive(true);
        }

        private void HandleHide(string locationID)
        {
            if (locationID == name)
                gameObject.SetActive(false);
        }
    }
}
