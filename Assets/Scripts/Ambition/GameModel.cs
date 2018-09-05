﻿using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace Ambition
{
	public class GameModel : DocumentModel
	{
		private ReputationVO _reputation;

		public string Allegiance;
		public string Scene;

		public string PlayerName = "Yvette DeWinter";

		[JsonProperty("livre")]
		int _livre;
		public int Livre
		{
			get { return _livre; }
			set
			{
				_livre = value;
				AmbitionApp.SendMessage<int>(GameConsts.LIVRE, _livre);
			}
		}

        int _exhaustion;
        public int Exhaustion
        {
            get { return _exhaustion; }
            set {
                _exhaustion = value > 100 ? 100 : value < 0 ? 0 : value;
                AmbitionApp.SendMessage(GameConsts.EXHAUSTION, _exhaustion);
            }
        }

		[JsonProperty("reputation", Order = 10)]
		public int Reputation
		{
			get { return _reputation.Reputation; }
			set
			{
				if (value < 0) value = 0;
				int level = _reputation.Level = Array.FindIndex(_levels, r => r > value);
				_reputation.Reputation = value;
				_reputation.ReputationMax = _levels[level];
				if (level > 0)
				{
					_reputation.Reputation -= _levels[level-1];
					_reputation.ReputationMax -= _levels[level-1];
				}
				AmbitionApp.SendMessage<ReputationVO>(_reputation);
			}
		}

		[JsonProperty("confidence")]
		private readonly int[] _confidence;
        public int ConfidenceBonus => _confidence[Level];

        [JsonProperty("vip")]
		private readonly int[] _vip;
        public int PartyInviteImportance => _vip[Level];

        public int Level
		{
			get { return _reputation.Level; }
		}

		public GameModel() : base("GameData") {}

		[JsonProperty("levels")]
		private int[] _levels;

		private OutfitVO _outfit;
		public OutfitVO Outfit
		{
			get { return _outfit; }
			set {
                AmbitionApp.SendMessage(_outfit = value);
			}
		}

		public OutfitVO LastOutfit;
	}
}
