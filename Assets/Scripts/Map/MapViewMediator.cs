﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class MapViewMediator : MonoBehaviour
	{
		private const int PAN_TOLERTANCE = 50;
		private const int TILES_PER_SECOND=10;
	    public GameObject roomButtonPrefab;

	    private Dictionary<RoomVO, RoomButton> _buttons;

	    private MapModel _model;
		private PartyModel _partyModel;
		private Rect _bounds;

		public MapVO Map
		{
			get { return _model.Map; }
		}

		void Awake()
		{
			_model = AmbitionApp.GetModel<MapModel>();
			_partyModel = AmbitionApp.GetModel<PartyModel>();
			_buttons = new Dictionary<RoomVO, RoomButton>();
			AmbitionApp.Subscribe<RoomVO>(HandleRoom);
            AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, Recenter);
            AmbitionApp.Subscribe(PartyMessages.SHOW_ROOM, Lock);
            AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, Unlock);
		}

 		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RoomVO>(HandleRoom);
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, Recenter);
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_ROOM, Lock);
            AmbitionApp.Unsubscribe<string>(GameMessages.DIALOG_CLOSED, Unlock);
			_buttons.Clear();
			_buttons = null;
		}

	    void Start()
	    {
			//Make the Room Buttons ----------------------
	        Array.ForEach(Map.Rooms, DrawRoom);
			_bounds.xMin *= _model.MapScale;
			_bounds.xMax *= _model.MapScale;
			_bounds.yMin *= _model.MapScale;
			_bounds.yMax *= _model.MapScale;
			AmbitionApp.Subscribe<RoomVO>(HandleRoom);

			HandleRoom(Map.Entrance);
	    }

        private void Lock()
        {
            enabled = false;
        }
		
        private void Unlock(string DialogID)
        {
            enabled = DialogID == "END_CONVERSATION" || DialogID == "DEFEAT";
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Recenter();
            }
            else
            {
                Vector3 offset = Vector3.zero;
                float d = Time.deltaTime * _model.MapScale;//*TILES_PER_SECOND;
                if (Input.mousePosition.x < PAN_TOLERTANCE)
                    offset[0] = d;
                else if (Input.mousePosition.x > Screen.width - PAN_TOLERTANCE)
                    offset[0] = -d;
                else if (Input.GetKey(KeyCode.LeftArrow)) offset[0] = d * .5f;
                else if (Input.GetKey(KeyCode.RightArrow)) offset[0] = -d * .5f;

                if (Input.mousePosition.y < PAN_TOLERTANCE)
                    offset[1] = d;
                else if (Input.mousePosition.y > Screen.height - PAN_TOLERTANCE)
                    offset[1] = -d;
                else if (Input.GetKey(KeyCode.UpArrow)) offset[1] = -d * .5f;
                else if (Input.GetKey(KeyCode.DownArrow)) offset[1] = d * .5f;

                transform.Translate(offset);
            }
        }

	    private void Recenter()
	    {
            if (Map != null)
            {
                RoomVO room = _model.Room ?? Map.Entrance;
                if (room != null)
                {
                    Vector3 center;
                    int[] bounds = room.Bounds;
                    float q = -.5f * _model.MapScale;
                    center.x = (bounds[0] + bounds[2]) * q;
                    center.y = (bounds[1] + bounds[3]) * q;
                    center.z = 0f;
                    transform.localPosition = center;
                }
            }
	    }

		private void DrawRoom(RoomVO room)
		{
			if (room != null)
			{
				GameObject mapButton = Instantiate<GameObject>(roomButtonPrefab, gameObject.transform) as GameObject;
				RoomButton roomButton = mapButton.GetComponent<RoomButton>();
				int [] bounds = room.Bounds;
				mapButton.transform.SetAsFirstSibling();
				roomButton.Room = room;
				if (bounds[0] < _bounds.xMin) _bounds.xMin = bounds[0];
				if (bounds[1] < _bounds.yMin) _bounds.yMin = bounds[1];
				if (bounds[2] > _bounds.xMax) _bounds.xMax = bounds[2];
				if (bounds[3] > _bounds.xMax) _bounds.xMax = bounds[3];
				_buttons.Add(room, roomButton);
			}
	    }

		private void HandleRoom(RoomVO room)
		{
			foreach(KeyValuePair<RoomVO, RoomButton> kvp in _buttons)
			{
				kvp.Value.UpdatePlayerRoom(room);
			}
		}
	}
}
