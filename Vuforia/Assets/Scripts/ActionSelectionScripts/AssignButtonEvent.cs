﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class AssignButtonEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public Button moveButton;
    public Button attackButton;
    public Button guardButton;
    public Button specialAbilitiesButton;
    public Button endTurnButton;
    public bool allAssigned;
    private int frame;

    void Start()
    {
        allAssigned = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        assignListener();
        
    }

    public void assignListener()
    {
        
        if (!allAssigned)
        { //check if all button have been assigned or not
            if(GameManager.instance.players.Length > 0)
            {
                PlayerController player = GameManager.instance.GetPlayer(PhotonNetwork.NickName);
                if (player)
                {
                    // assign all action buttons after the player exist in the game
                    foreach (Transform child in player.transform)
                    {
                        BotController botScript = child.GetComponent<BotController>();
                        moveButton.onClick.AddListener(delegate { botScript.Move(); });
                        attackButton.onClick.AddListener(delegate { botScript.Attack(); });
                        guardButton.onClick.AddListener(delegate { botScript.Guard(); });
                        specialAbilitiesButton.onClick.AddListener(delegate { botScript.Abilities(); });
                        
                    }
                    assignEventTrigger(player);
                    
                    allAssigned = true;
                }
            
            }


        }
       
    }

    public void assignEventTrigger(PlayerController player)
    {
        EventTrigger trigger = endTurnButton.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { player.OnEndTurnButtonPressed(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((eventData) => { player.OnEndTurnRelease(); });
        trigger.triggers.Add(entry2);
    }


}
