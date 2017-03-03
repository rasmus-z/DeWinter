﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Faction {

    string name;
    public int playerReputation; //What's the Player's Rep with this faction? Raw Number
    private int playerReputationLevel; //The Player's 'Reputation Level'. Intended to make the Reputation system a little more understandable
    private int[] playerReputationLevelRequirements;
    private int[] playerConfidenceBonus;
    float power; //How powerful is this faction in the game?
    public string knownPower = "Unknown"; //Used in the 'Test the Waters' screen
    public int powerKnowledgeTimer;
    public int modestyLike; //How modest do they like their outfits?
    public int luxuryLike; //How luxurious do they like their outfits?
    float allegiance; //-100 means the Faction is devoted to the Revolution, 100 means they are devoted to the Crown
    public string knownAllegiance = "Unknown"; //Used in the 'Test the Waters' screen
    public int allegianceKnowledgeTimer;

    public List<string> benefitsList = new List<string>();

    public Faction (string nme, int mL, int lL, int all)
    {
        name = nme;
        modestyLike = mL;
        luxuryLike = lL;
        power = 10;
        allegiance = all;
        playerReputation = 20;
        playerReputationLevelRequirements = new int[11];
        playerConfidenceBonus = new int[11];
        StockPlayerReputationLevelRequirements();
        StockBenefitsList();
    }

    public string Likes()
    {
        string likes;
        string ll;
        string ml;
        //----------
        if (luxuryLike > 0)
        {
            ll = "Luxurious";
        } else if (luxuryLike < 0 )
        {
            ll = "Vintage";
        } else
        {
            ll = "Doesn't Care"; // This will never get used, but it's here just in case...
        }
        //---------
        if (modestyLike > 0)
        {
            ml = "Modest";
        } else if (modestyLike < 0)
        {
            ml = "Racy";
        } else
        {
            ml = "Doesn't Care"; // This will never get used, but it's here just in case...
        }
        //---------
        if (luxuryLike == 0 && modestyLike == 0) //Military Only (Temporary)
        {
            likes = "They don't care about your clothes";
        } else
        {
            likes = ll + " and " + ml + " Outfits";
        }
        return likes;
    }

    public string Dislikes()
    {
        string dislikes;
        string ld;
        string md;
        //----------
        if (luxuryLike > 0)
        {
            ld = "Vintage";
        }
        else if (luxuryLike < 0)
        {
            ld = "Luxurious";
        }
        else
        {
            ld = "Doesn't Care"; // This will never get used, but it's here just in case...
        }
        //---------
        if (modestyLike > 0)
        {
            md = "Racy";
        }
        else if (modestyLike < 0)
        {
            md = "Modest";
        }
        else
        {
            md = "Doesn't Care"; // This will never get used, but it's here just in case...
        }
        //---------
        if (luxuryLike == 0 && modestyLike == 0)
        {
            dislikes = "They don't care about your clothes";
        }
        else
        {
            dislikes = ld + " and " + md + " Outfits";
        }
        return dislikes;
    }

    private void StockPlayerReputationLevelRequirements()
    {
        playerReputationLevelRequirements[0] = 0;
        playerReputationLevelRequirements[1] = 20;
        playerReputationLevelRequirements[2] = 50;
        playerReputationLevelRequirements[3] = 100;
        playerReputationLevelRequirements[4] = 150;
        playerReputationLevelRequirements[5] = 200;
        playerReputationLevelRequirements[6] = 250;
        playerReputationLevelRequirements[7] = 300;
        playerReputationLevelRequirements[8] = 350;
        playerReputationLevelRequirements[9] = 400;
        playerReputationLevelRequirements[10] = 999999; //A cap just so it doesn't freak out should the player run over 400
    }

    private void StockConfidenceBenefits()
    {
        playerConfidenceBonus[0] = 0;
        playerConfidenceBonus[1] = 10;
        playerConfidenceBonus[2] = 20;
        playerConfidenceBonus[3] = 30;
        playerConfidenceBonus[4] = 40;
        playerConfidenceBonus[5] = 50;
        playerConfidenceBonus[6] = 60;
        playerConfidenceBonus[7] = 70;
        playerConfidenceBonus[8] = 80;
        playerConfidenceBonus[9] = 90;
    }

    void StockBenefitsList()
    {
        benefitsList.Add("Level 0: No new invitations to Trivial " + name + " Parties");
        benefitsList.Add("Level 1: Invited to Trivial " + name + " Parties");
        benefitsList.Add("Level 2: Wine upon entering " + name + " Parties");
        switch (name)
        {
            case "Crown":
                benefitsList.Add("Level 3: Training in Courtly Dances");
                break;
            case "Church":
                benefitsList.Add("Level 3: Confessions allow Scandals to disappear faster");
                break;
            case "Military":
                benefitsList.Add("Level 3: Alcohol effects you less");
                break;
            case "Bourgeoisie":
                benefitsList.Add("Level 3: The merchant stocks 4 Outfits per day instead of 3");
                break;
            case "Revolution":
                benefitsList.Add("Level 3: Selling Gossip to the press is now less risky");
                break;
        }
        benefitsList.Add("Level 4: Invited to Decent " + name + " Parties, extra time at Trivial " + name + " Parties");
        benefitsList.Add("Level 5: Extra attention from the drink servers at " + name + " Parties");
        benefitsList.Add("Level 6: Always know the Power of the " + name);
        benefitsList.Add("Level 7: Invited to Grand " + name + " Parties, extra time at Decent " + name + " Parties");
        benefitsList.Add("Level 8: Always know Allegiance of the " + name);
        switch (name)
        {
            case "Crown":
                benefitsList.Add("Level 9: A title and Royal Allowance. Extra time at Grand " + name + " Parties");
                break;
            case "Church":
                benefitsList.Add("Level 9: Confessions to a Cardinal lend you immunity to Scandal. Extra time at Grand " + name + " Parties");
                break;
            case "Military":
                benefitsList.Add("Level 9: Your Enemies in the Military Faction have been surpressed, Extra time at Grand " + name + " Parties");
                break;
            case "Bourgeoisie":
                benefitsList.Add("Level 9: You are given influence at the Fashion Houses and may pick styles. Extra time at Grand " + name + " Parties");
                break;
            case "Revolution":
                benefitsList.Add("Level 9: You are a member of the Revolutionary Council. Extra time at Grand " + name + " Parties");
                break;
        }
    }

    public int PlayerReputationLevel()
    {
        if(playerReputation <= -20)
        {
            return -1;
        } else
        {
            int i = 0;
            while (playerReputation > playerReputationLevelRequirements[i])
            {
                //Debug.Log(name + " Faction Level: " + i);
                i++;
            }
            playerReputationLevel = i;
            return playerReputationLevel;
        }
        
    }

    public int PlayerConfidenceBenefit()
    {
        if(PlayerReputationLevel() == -1)
        {
            return -10;
        } else
        {
            return playerConfidenceBonus[PlayerReputationLevel()];
        }
    }

    public int largestAllowableParty()
    {
        switch (PlayerReputationLevel())
        {
            case 0:
                return 0;
            case 1:
                return 1;
            case 2:
                return 1;
            case 3:
                return 1;
            case 4:
                return 2;
            case 5:
                return 2;
            case 6:
                return 2;
            default:
                return 3;
        }

    }

    public string Name()
    {
        return name;
    }

    public float Allegiance()
    {
        return allegiance;
    }

    public void ChangeAllegiance(int changeAmount)
    {
        if (name != "Revolution" || name != "Crown")
        {
            allegiance = Mathf.Clamp(allegiance + changeAmount, -100, 100);
        }
    }

    public void ChangePower(int changeAmount)
    {
        power = Mathf.Clamp(power + changeAmount, 0, 100);
    }

    public float Power()
    {
        return power;
    }

    public string PowerString()
    {
        //Text Conversion
        if (power >= 90)
        {
            return "Dominating";
        }
        else if (power < 90 && power >= 60)
        {
            return "Formidable";
        }
        else if (power < 60 && power > 40)
        {
            return "Significant";
        }
        else if (power <= 40 && power > 10)
        {
            return "Weak";
        }
        else
        {
            return "Insignificant";
        }
    }

    public void CalculateKnownPower()
    {
        knownPower = PowerString();
        powerKnowledgeTimer = 0;
    }

    public string AllegianceString()
    {
        //Text Conversion
        if (allegiance > 80)
        {
            return "Ultra Monarchist";
        }
        else if (allegiance > 50 && allegiance <= 80)
        {
            return "Monarchist";
        }
        else if (allegiance > 20 && allegiance <= 50)
        {
            return "Leaning Towards the Monarchy";
        }
        else if (allegiance >= -20 && allegiance <= 20)
        {
            return "Undecided";
        }
        else if (allegiance >= -50 && allegiance < -20)
        {
            return "Leaning Towards the Revolution";
        }
        else if (allegiance >= -80 && allegiance < -50)
        {
            return "Revolutionary";
        }
        else
        {
            return "Radical Revolutionary";
        }
    }

    public void CalculateKnownAllegiance()
    {
        knownAllegiance = AllegianceString();
        allegianceKnowledgeTimer = 0;
    }
}
