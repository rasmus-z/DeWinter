﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FactionReputationTracker : MonoBehaviour {
    public string factionTracked;
    Text myText;

	void Start () {
        myText = this.GetComponent<Text>();
	}

	void Update () {
		myText.text = "- Your Reputation Level with The " + factionTracked + " is " + Ambition.AmbitionApp.GetModel<Ambition.FactionModel>()[factionTracked].Level.ToString();
	}
}
