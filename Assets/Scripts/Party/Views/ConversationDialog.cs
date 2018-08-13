using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Dialog;

namespace Ambition
{
    public class ConversationDialog : DialogView, Util.IInitializable<CommodityVO[]>, IPointerClickHandler
    {
        public Text TitleText;
        public Text SubText;
        public CommodityTableView Commodities;

        public void SetPhrase(string phrase)
        {
            TitleText.text = AmbitionApp.GetString(phrase + ".title");
            SubText.text = AmbitionApp.GetString(phrase + ".body");
        }

        public void Initialize(CommodityVO[] commodities)
        {
            Commodities.SetCommodities(commodities);
        }

        public void OnPointerClick(PointerEventData data)
        {
            AmbitionApp.SendMessage(GameMessages.DIALOG_CLOSED, ID);
            Close();
        }
    }
}
