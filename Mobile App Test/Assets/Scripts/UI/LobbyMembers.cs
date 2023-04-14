using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using TMPro; 

public class LobbyMembers : MonoBehaviour
{
    public LobbyMemberDisplay prefab;
    public Transform parent;
    private LobbyMemberDisplay myMemberDisplay;
    private List<LobbyMemberDisplay> members;
    public Button statusButton;
    public Button startGameButton;
    
    private LobbyMemberDisplay createMember(Player player, Transform parent){
        LobbyMemberDisplay member;
        member = Instantiate(prefab, parent);
        member.username.text = player.Data["playerName"].Value;
        if(player.Data["playerName"].Value.Equals(LobbyManager.Instance.player.Data["playerName"].Value))
        {
            member.isSameUser();
            myMemberDisplay = member;
        }

        member.editMember(player);
        return member;
    }

    private LobbyMemberDisplay createHeader(Transform parent){
        LobbyMemberDisplay header;
        header = Instantiate(prefab, parent);
        header.username.text = "User";
        header.deviceType.text = "Device";
        header.playerStatus.text = "Status";
        return header;
    }

    // Start is called before the first frame update
    void Start()
    {
        members = new List<LobbyMemberDisplay>();
        LobbyMemberDisplay header = createHeader(parent);
        List<Player> players = LobbyManager.Instance.CreatedLobby.Players;
        for (int i = 0; i < players.Count; i++){
            members.Add(createMember(players[i], parent));
        }

        statusButton.onClick.AddListener(changePlayerStatus);
        LobbyManager.Instance.OnLobbyChange += updateMembers;

        startGameButton.interactable = false;
    }

    private void updateMembers()
    {
        List<Player> players = LobbyManager.Instance.CreatedLobby.Players;
        // Update code
        bool allPlayersReady = true;
        LobbyMemberDisplay member;
        for(int i = 0; i < players.Count; i++){
            member = members.Find(x => x.username.text == players[i].Data["playerName"].Value);
            member.editMember(players[i]);
            if(players[i].Data["ready"].Value.Equals("Not Ready"))
            {

                allPlayersReady = false;
            }
        }
        if(!allPlayersReady)
        {
            startGameButton.interactable = false;
            statusButton.GetComponentInChildren<TMP_Text>().text = "Ready";
            return;
        }
        startGameButton.interactable = true;
        statusButton.GetComponentInChildren<TMP_Text>().text = "Not Ready";
    }

    private void changePlayerStatus(){
        LobbyManager.Instance.UpdatePlayerStatus();
    }
}