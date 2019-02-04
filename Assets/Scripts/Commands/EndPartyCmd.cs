using System;
using Core;

namespace Ambition
{
    // Ends the current party and nullifies the reference in the model.
	public class EndPartyCmd : ICommand
	{
	    public void Execute()
	    {
	    	PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO party = model.Party;
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            calendar.Schedule(model.Party.ExitIncident, calendar.Today);
            Array.ForEach(calendar.GetEvents<PartyVO>(), calendar.Complete);
            if (party != null) AmbitionApp.GetModel<CalendarModel>().Schedule(party.ExitIncident);
            model.Party = null;

            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            OutfitVO outfit = inventory.GetEquipped(ItemConsts.OUTFIT) as OutfitVO;
            if (outfit != null) outfit.Novelty -= inventory.NoveltyDamage;

            inventory.Equipped[ItemConsts.LAST_OUTFIT] = outfit;
            // Reset the player's equipped items
            inventory.Equipped.Remove(ItemConsts.OUTFIT);
            inventory.Equipped.Remove(ItemConsts.ACCESSORY);
        }
    }
}
