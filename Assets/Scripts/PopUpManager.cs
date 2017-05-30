﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;

public class PopUpManager : MonoBehaviour
{
// TODO: Refactor to work with Dialog Manager
    public GameObject screenFader;

    public GameObject twoPartyRSVPdModal;
	public GameObject cantAffordModal;
    public GameObject pierreQuestModal;
    public GameObject gossipSaleModal;
    public GameObject buyOrSellModal;
    public GameObject hireAndFireServantModal;
    public GameObject confidenceTallyModal;
    public GameObject roomChoiceModal;
    public GameObject alterOutfitModal;
    public GameObject sewNewOutfitModal;

    public GameObject hostRemarkSlotPrefab;

    //This confirmation modal is used for Hiring and Firing Servants
    void CreateHireAndFireModal(ServantVO s)
    {
        //Make the Pop up
        GameObject popUp = Instantiate(buyOrSellModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        Text hireFireButtonText = popUp.transform.Find("HireFireButton").Find("Text").GetComponent<Text>();
        Text dontHireFireButtonText = popUp.transform.Find("DontHireFireButton").Find("Text").GetComponent<Text>();

        //Set the Pop Up Values
        //Fill in the Text
        if (s.Hired)
        {
            titleText.text = "Fire + " + s.Name + "?";
            bodyText.text = "Are you sure you want fire " + s.NameAndTitle + "?";
            hireFireButtonText.text = "Fire";
            dontHireFireButtonText.text = "Don't Fire";
        }
        else
        {
			titleText.text = "Hire + " + s.Name + "?";
            bodyText.text = "Are you sure you want to hire " + s.NameAndTitle + " for £" + s.Wage.ToString() + "?";
            hireFireButtonText.text = "Hire";
            dontHireFireButtonText.text = "Don't Hire";
        }
    }

    //This is used in the Wardrobe Screen to confirm buying or selling Items
    void CreateBuyOrSellModal(object[] objectStorage)
    {
        //Info Is Parsed Out Here
        string inventoryType = objectStorage[0] as string;
        string itemType = objectStorage[1] as string;
        int itemPrice;

        //Make the Pop up
        GameObject popUp = Instantiate(buyOrSellModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();

        //Set the Pop Up Values
        BuyAndSellPopUpController controller = popUp.GetComponent<BuyAndSellPopUpController>();
        controller.inventoryType = inventoryType;
        controller.itemType = itemType;
		controller.outfit = objectStorage[2] as Outfit;
		controller.accessory = objectStorage[2] as ItemVO;

        //Fill in the Text
		if (controller.outfit != null)
        {
            if (inventoryType == "personal")
            {
                titleText.text = "Sell This?";
				itemPrice = controller.outfit.OutfitPrice(inventoryType); //Items are at Half Price from the Player Inventory to the Merchant
				bodyText.text = "Are you sure you want to sell this " + controller.outfit.Name();
			}
            else
            {
                titleText.text = "Buy This?";
				itemPrice = controller.outfit.OutfitPrice(inventoryType);
				bodyText.text = "Are you sure you want to buy this " + controller.outfit.Name() + " for " + itemPrice.ToString("£" + "#,##0") + "?";
            }
        }
		else if (controller.accessory != null)
        {
            if (inventoryType == "personal")
            {
                titleText.text = "Sell This?";
				itemPrice = controller.accessory.SellPrice; //Items are at Half Price from the Player Inventory to the Merchant
				bodyText.text = "Are you sure you want to sell this " + controller.accessory.Name + " for " + itemPrice.ToString("£" + "#,##0") + "?";
            }
            else
            {
                titleText.text = "Buy This?";
				itemPrice = controller.accessory.Price;
				bodyText.text = "Are you sure you want to buy this " + controller.accessory.Name + " for " + itemPrice.ToString("£" + "#,##0") + "?";
            }
        }
    }

    //This is used in the Wardrobe Screen and Servant Hiring Screen to tell Players they don't have enough money to afford something
    void CreateCantAffordModal(object[] objectStorage)
    {
        string objectString = (string)objectStorage[0];

        //Make the Pop Up
        GameObject popUp = Instantiate(cantAffordModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Oh No!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "I'm sorry Madamme, but you do not have enough Livres to afford the " + objectString + "." +
                        "\n\nYou could either sell some of your existing wardrobe, or you could borrow money from your various friends." +
                        "\n\nThis will cost you 20 Reputation but it would get you 200 Livres.";
    }

    //This is used in the Wardrobe Screen so Players can use the 'Alteration' function of the Tailor Servant
    void CreateAlterOutfitModal(object[] objectStorage)
    {
        int inventoryNumber = (int)objectStorage[0];
        Outfit outfit = OutfitInventory.outfitInventories["personal"][inventoryNumber];

        //Make the Pop Up
        GameObject popUp = Instantiate(alterOutfitModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        AlterOufitPopUpController popUpController = popUp.GetComponent<AlterOufitPopUpController>();
        popUpController.outfit = outfit;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Alter Outfit";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "How would you like me to alter the " + outfit.Name() + "? This will cost 20 Livres."
            + "\nSelect One:";
        Slider modestyBar = popUp.transform.Find("ModestyText").Find("Slider").GetComponent<Slider>();
        modestyBar.value = outfit.modesty;
        Slider luxuryBar = popUp.transform.Find("LuxuryText").Find("Slider").GetComponent<Slider>();
        luxuryBar.value = outfit.luxury;
    }

    //This is used in the Wardrobe Screen so Players can use the 'Sew New Outfit' function of the Seamstress Servant
    void CreateSewNewOutfitModal(object[] objectStorage)
    {
        OutfitInventoryList personalOutfitInventoryList = objectStorage[0] as OutfitInventoryList;
        //Make the Pop Up
        GameObject popUp = Instantiate(sewNewOutfitModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        SewNewOutfitPopUpController popUpController = popUp.GetComponent<SewNewOutfitPopUpController>();
        popUpController.personalInventoryList = personalOutfitInventoryList;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "New Outfit";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "What would you like me to create?";
    }

    //This is used in the Party Scene for Players to choose whether they wish to engage in conversation (Work the Room) or try to avoid everyone (Move Through)
    void CreateRoomChoiceModal(int[] intStorage)
    {
    	MapModel model = AmbitionApp.GetModel<MapModel>();

        int xPos = intStorage[0];
        int yPos = intStorage[1];

		RoomVO room = model.Map.Rooms[xPos, yPos];

        //Make the Pop Up
        GameObject popUp = Instantiate(roomChoiceModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = GameData.tonightsParty.Name();
        //Body Text and Buttons
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        
        Text moveThroughText = popUp.transform.Find("MoveThroughButton").Find("Text").GetComponent<Text>();
        Image moveThroughButtonImage = popUp.transform.Find("MoveThroughButton").GetComponent<Image>();

        if (!model.Room.HostHere) //If the Host isn't here
        {
            if(!model.Room.IsImpassible)
            {
                moveThroughButtonImage.color = Color.white;
                moveThroughText.color = Color.white;
                int moveThroughChance = room.MoveThroughChance;
                //Is the Player using the Cane Accessory? If so then increase the chance to Move Through by 10%!
                if (GameData.partyAccessory != null)
                {
					if (GameData.partyAccessory.Type == "Cane")
                    {
                        moveThroughChance += 10;
                    }
                }
                moveThroughText.text = "Move Through (" + moveThroughChance.ToString() + "%)";
                bodyText.text = "You've entered the " + room.Name +
                            "\n\nWould you like to 'Work the Room' and engage the party goers in Conversation, or would you like to 'Move Through' and hope nobody notices you?";
            } else //If the Player has entered a Room where they are not allowed to Move Through
            {
                moveThroughButtonImage.color = Color.clear;
                moveThroughText.color = Color.clear;
                bodyText.text = "You've entered the " + room.Name +
                            "\n\nClick the button below to 'Work the Room' and engage the party goers in Conversation.";
            }
        } else // If the Host Is there
        {
            moveThroughButtonImage.color = Color.clear;
            moveThroughText.color = Color.clear;
            bodyText.text = "You've entered the " + room.Name +
                       "\n\nPrepare to 'Work the Room' and engage the Host in Conversation. They may be alone but they'll be far more demanding than a regular Guest.";
        }
        Text workTheRoomText = popUp.transform.Find("WorkTheRoomButton").Find("Text").GetComponent<Text>();
        Image workTheRoomImage = popUp.transform.Find("WorkTheRoomButton").GetComponent<Image>();
        if (!room.Cleared)
        {
            workTheRoomImage.color = Color.white;
            workTheRoomText.color = Color.white;
            workTheRoomText.text = "Work the Room";
        }
        else
        {
            workTheRoomImage.color = Color.clear;
            workTheRoomText.color = Color.clear;
        }
    }

    //This is used in the Estate Tab to tell Players that they were caught trading in Gossip Items
    void CreateCaughtTradingGossipModal(string faction)
    {
    	Dictionary<string,string> subs = new Dictionary<string, string>(){{"$FACTION",faction}};
    	if (GameData.factionList["Third Estate"].ReputationLevel >= 2)
    	{
    		AmbitionApp.OpenMessageDialog(DialogConsts.CAUGHT_GOSSIPING_THIRD_ESTATE_DIALOG, subs);
    	}
    	else
    	{
			AmbitionApp.OpenMessageDialog(DialogConsts.CAUGHT_GOSSIPING_DIALOG, subs);
    	}
    }

    //This is used in the Estate Tab to tell Players that Pierre has assigned a new Quest
    void CreateNewPierreQuestModal(object[] objectStorage)
    {
        PierreQuest quest = objectStorage[0] as PierreQuest;
        PierreQuestInventoryList questList = objectStorage[1] as PierreQuestInventoryList;    
        //Make the Pop Up
        GameObject popUp = Instantiate(pierreQuestModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        popUp.transform.SetAsFirstSibling();
        //Set the Quest and the QuestList
        PierreQuestModal questModal = popUp.GetComponent<PierreQuestModal>();
        questModal.quest = quest;
        questModal.questList = questList;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "A Call for Gossip!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "Madamme, it's urgent! My finely honed journalistic senses are telling me that the public is currently crying out for Gossip concerning the " + quest.Faction + "." +
                "\n\nIf you can get that to me in " + quest.daysTimeLimit + " Days then I'll be able to get you a reward of " + quest.reward.Name + ". \n\nHow does that sound?";
    }

    //This is used in the Estate Tab to confirm selling various bits of Gossip
    void CreateSellGossipModal(object[] objectStorage)
    {
        GossipTrading gossipTrader = objectStorage[0] as GossipTrading;
        Gossip gossip = objectStorage[1] as Gossip;
        string tradeFor = objectStorage[2] as string;
        int tradeForAmount = (int)objectStorage[3];
        string faction = objectStorage[4] as string;
        int caughtChance = (int)objectStorage[5];
        //Make the Pop Up
        GameObject popUp = Instantiate(gossipSaleModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Set the Quest and the QuestList
        GossipTradingPopUpController popUpController = popUp.GetComponent<GossipTradingPopUpController>();
        popUpController.gossipTrading = gossipTrader;
        popUpController.tradeFor = tradeFor;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Are You Sure?";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "Madamme,  are you sue that you'd like to sell me this " + gossip.Name();
        switch (tradeFor)
        {
            case "Livres":
                bodyText.text += " in exchange for " + tradeForAmount + " Livres?";
                break;
            case "Allegiance":
                if (tradeForAmount > 0)
                {
                    bodyText.text += " in exchange for " + tradeForAmount + " Allegiance to the Crown from the " + faction + "?";
                } else
                {
                    bodyText.text += " in exchange for " + tradeForAmount + " Allegiance to the Third Estate from the " + faction + "?";
                }
                break;
            case "Power":
                bodyText.text += " in exchange for " + tradeForAmount + " Power for the " + faction + "?";
                break;
        }
        if (caughtChance == 0)
        {
            bodyText.text += "\n\n I can easily conceal you as my source. There's no chance you'll be caught.";
        } else
        {
            bodyText.text += "\n\n The more you leak me Gossip in a single day, the harder it is for me to conceal my sources. Today, I'd guess there is currently a " + caughtChance + "% chance of you being caught. Can you acccept that risk?";
        }
    }
}