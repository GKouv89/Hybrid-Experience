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
        List<Player> players = LobbyManager.Instance.MyLobby.Players;
        for (int i = 0; i < players.Count; i++){
            members.Add(createMember(players[i], parent));
        }

        statusButton.onClick.AddListener(changePlayerStatus);
        LobbyManager.Instance.OnLobbyChange += updateMembers;

        startGameButton.onClick.AddListener(startGame);
        startGameButton.interactable = false;
    }

    private void updateMembers()
    {
        List<Player> players = LobbyManager.Instance.MyLobby.Players;
        foreach ( Player player in players){
            Debug.Log("Player name: " + player.Data["playerName"].Value.ToString());
        }

        // Case no. 1: new member was added
        if(players.Count != members.Count)
        {
            foreach(Player player in players)
            {
                if(!members.Exists(x => x.username.text == player.Data["playerName"].Value))
                {
                    // Create new member UI
                    members.Add(createMember(player, parent));
                }
            }
        }

        // Case no. 2: Some player data has changed (readiness level)
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

    private void startGame()
    {
        ScenesManager.Scene gameScene; 
        #if UNITY_STANDALONE_WIN
            gameScene = ScenesManager.Scene.DIMuseumVR;
        #elif UNITY_ANDROID
            gameScene = ScenesManager.Scene.SampleScene;
        #endif
        ScenesManager.Instance.LoadScene(gameScene);
    }
}
