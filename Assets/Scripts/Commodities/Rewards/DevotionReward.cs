using System;
using Core;

namespace Ambition
{
    public class DevotionReward : ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            NotableVO notable = Array.Find(model.Notables, n => n.Name == reward.ID);
            if (notable != null) notable.Devotion += reward.Value;
        }
    }
}
