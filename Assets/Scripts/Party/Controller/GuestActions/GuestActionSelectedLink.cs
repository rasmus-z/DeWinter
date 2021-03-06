using UFlow;

namespace Ambition
{
    public class GuestActionSelectedLink:ULink<string>
    {
        private string _actionType;
        override public void SetValue(string data)
        {
            _actionType = data;
        }

        override public bool Validate()
        {
            MapModel map = AmbitionApp.GetModel<MapModel>();
            UController controller = _machine._uflow.GetController(_machine);
            int index = controller.transform.GetSiblingIndex();
            if (index >= map.Room.Guests.Length) return false;
            GuestVO guest = map.Room.Guests[index];
            return (guest.Action != null && guest.Action.Type == _actionType);
        }
    }
}
