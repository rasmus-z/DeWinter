using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.Linq;

namespace Core
{
	public class LocalizationModel : IModel
	{
		public const string LOCALIZATIONS_DIRECTORY = "Localization/";
		public const string DEFAULT_CONFIG = "Default";

		protected string _language;
		protected Dictionary<string, string> _localizations;

		public LocalizationModel ()
		{
			_language = Application.systemLanguage.ToString();
			_localizations = new Dictionary<string, string>();

			TextAsset file = Resources.Load<TextAsset>(LOCALIZATIONS_DIRECTORY + _language);
			if (file == null)
			{
				_language = DEFAULT_CONFIG;
				file = Resources.Load<TextAsset>(LOCALIZATIONS_DIRECTORY + DEFAULT_CONFIG);
			}
			if (file != null)
			{
				JsonConvert.PopulateObject(file.text, _localizations);

			}
		}

		public string this[string key]
		{
			get
			{
				string value;
				return _localizations.TryGetValue(key, out value) ? value : null;
			}
		}

		public string[] GetList(string key)
		{
			return _localizations.Where(k=>k.Key.StartsWith(key)).Select(v=>v.Value).ToArray();
		}

		public string GetString(string key)
		{
			string value;
			return _localizations.TryGetValue(key, out value) ? value : null;
		}

		public string GetString(string key, Dictionary<string,string> substitutions)
		{
			string value;
			_localizations.TryGetValue(key, out value);
			if (value != null && substitutions != null)
			{				
				foreach(KeyValuePair<string, string> kvp in substitutions)
				{
					value = value.Replace(kvp.Key, kvp.Value);
				}
			}
			return value;
		}

/*
IEnumerator GetForm()
{
    UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/" + SPREADSHEED_ID + "/values/Sheet1!A1:D5");
    yield return www.SendWebRequest();
    _phrases = Newtonsoft.Json.JsonConvert.DeserializeObject<Phrases>(www.downloadHandler.text);
}
*/