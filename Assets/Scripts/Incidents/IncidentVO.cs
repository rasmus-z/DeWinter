using System;
using UnityEngine;
using Util;

namespace Ambition
{
    [Serializable]
    public class IncidentVO : DirectedGraph<MomentVO, TransitionVO>, ICalendarEvent
    {
        [SerializeField]
        private string _name;
        public string ID => _name;
        public bool Late = false; //True = This incident will only happen after Parties and Locations have ended, regardless of when it is awarded.

        [SerializeField]
        private long _date = -1;
        public DateTime Date
        {
            get { return _date < 0 ? default(DateTime) : new DateTime(_date); }
            set { _date = value.Ticks; }
        }

        public RequirementVO[] Requirements;

        public bool OneShot = true;

        public IncidentVO() : base() {}
        public IncidentVO(DirectedGraph<MomentVO, TransitionVO> incident) : base(incident)
        {
            Nodes = incident.Nodes;
            Links = incident.Links;
            LinkData = incident.LinkData;
        }

        public IncidentVO(IncidentVO incident) : this(incident as DirectedGraph<MomentVO, TransitionVO>)
        {
            this._name = incident._name;
            this.Date = incident.Date;
        }

        public MomentVO Moment;
    }

    [Serializable]
    public class TransitionVO
    {
        public string Text;
        public RequirementVO[] Requirements;
        public CommodityVO[] Rewards;
        public bool IsDefault => string.IsNullOrWhiteSpace(Text) && (Requirements == null || Requirements.Length == 0);
    }
}
