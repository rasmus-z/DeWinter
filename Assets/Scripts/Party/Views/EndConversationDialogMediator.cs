using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using Core;

namespace Ambition
{
    public class EndConversationDialogMediator : DialogView, IPointerClickHandler
    {
        private ModelSvc _models = App.Service<ModelSvc>();
        private RoomVO _room;
        private GuestVO[] _guests;

        public const string DIALOG_ID = "END_CONVERSATION";
        public Text TitleText;
        public Text SubText;
        public CommodityTableView Commodities;
               
        //This is not being handled via an Initialize function because this dialog needs to be brought up via the state machine, and there's currently no way to use 'Open Dialog' and set an array of commodities at the same time
        //While I could have made the state machine able to accept both a string and CommodityVO[] in the same state, I didn't feel comfortable making changes to something as critical as the state machine
        public void Awake()
        {
            RoomVO _room = _models.GetModel<ConversationModel>().Room;
            _guests = _room.Guests;
            Commodities.SetCommodities(_room.Rewards);
            SetPhrase("after_conversation_dialog.success");
        }

        public void SetPhrase(string phrase)
        {
            TitleText.text = AmbitionApp.GetString(phrase + ".title");
            SubText.text = SetBodyText(phrase);
        }

        public void OnPointerClick(PointerEventData data)
        {
            AmbitionApp.SendMessage(GameMessages.DIALOG_CLOSED, ID);
            FadeView view = GetComponent<FadeView>();
            if (view == null) Close();
            else
            {
                view.FadeOut();
                StartCoroutine(WaitToClose(view.FadeOutSeconds));
            }
        }

        private string SetBodyText(string phrase)
        {
            int charmedTally = 0;
            int offendedTally = 0;
            foreach (GuestVO g in _guests)
            {
                if(g.State == GuestState.Charmed)
                {
                    charmedTally++;
                } else if(g.State == GuestState.Offended)
                {
                    offendedTally++;
                }
            } 
            if(offendedTally == 0)
            {
                return AmbitionApp.GetString(phrase + ".body_charmed_all");
            } else
            {
                Dictionary<string, string> dialogSubstitutions = new Dictionary<string, string>();
                dialogSubstitutions.Add("$CHARMEDAMOUNT", charmedTally.ToString());
                if (charmedTally == 1) dialogSubstitutions.Add("$CHARMEDSINGULARORPLURAL", AmbitionApp.GetString("guest_was"));
                else dialogSubstitutions.Add("$CHARMEDSINGULARORPLURAL", AmbitionApp.GetString("guests_were"));
                dialogSubstitutions.Add("$OFFENDEDAMOUNT", offendedTally.ToString());
                if (offendedTally == 1) dialogSubstitutions.Add("$OFFENDEDSINGULARORPLURAL", AmbitionApp.GetString("guest_was"));
                else dialogSubstitutions.Add("$OFFENDEDSINGULARORPLURAL", AmbitionApp.GetString("guests_were"));
                return AmbitionApp.GetString(phrase + ".body_charmed_some",dialogSubstitutions);
            }
        }

        IEnumerator WaitToClose(float time)
        {
            yield return new WaitForSeconds(time);
            Close();
        }
    }
}
