using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using TMPro;

public class LobbyMemberDisplay : MonoBehaviour
{
    public TMP_Text username;
    public TMP_Text deviceType;
    public TMP_Text playerStatus;

    public void editMember(Player player){
        this.deviceType.text = player.Data["deviceType"].Value;
        string playerStatus = player.Data["ready"].Value;
        this.playerStatus.text = playerStatus;
        if(playerStatus == "Not Ready")
        {
            this.playerStatus.color = Color.red;
        }
        else
        {
            this.playerStatus.color = Color.green;
        }
    }

    public void isSameUser(){
        this.username.color = Color.blue;
    }

    // private void Update()
    // {
    //     Debug.Log("Update from LobbyMemberDisplay");
    // }
}
