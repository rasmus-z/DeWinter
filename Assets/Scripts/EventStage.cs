﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ambition;
using Newtonsoft.Json;

public class EventStage
{
	[JsonProperty("description")]
    public string Description;

	[JsonProperty("options")]
    public EventOption[] Options;

	[JsonProperty("rewards")]
    public RewardVO[] Rewards;

    //TODO: Eliminate constructor in favor of JSON configuration
    public EventStage(string desc, RewardVO[] rewards, params EventOption[] options)
    {
        Description = desc;
        Rewards = (rewards == null ? new RewardVO[0] : rewards);
        Options = options;
    }

	//TODO: Eliminate constructor in favor of JSON configuration
    public EventStage(string desc, RewardVO reward, params EventOption[] options)
    {
        Description = desc;
        Rewards = new RewardVO[]{ reward };
        Options = options;
    }

    //TODO: Eliminate constructor in favor of JSON configuration
    public EventStage(string desc, params EventOption[] options)
    {
        Description = desc;
		Rewards = new RewardVO[0];
        Options = options;
    }
}