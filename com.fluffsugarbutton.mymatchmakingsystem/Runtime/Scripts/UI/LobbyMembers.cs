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
    [SerializeField] private Button backButton;

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

        startGameButton.onClick.AddListener(LobbyManager.Instance.UpdateGameStatus);
        startGameButton.interactable = false;

        backButton.onClick.AddListener(LeaveLobby);
    }

    private void updateMembers()
    {
        List<Player> players = LobbyManager.Instance.MyLobby.Players;

        // Case no. 1: new member was added
        if(players.Count > members.Count)
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
        else if(players.Count < members.Count) // Case no. 2: A player was disconnected
        {
            Debug.Log("A player has disconnected");
            foreach(LobbyMemberDisplay tempmember in members)
            {
                Debug.Log("Checking player " + tempmember.username.text + " 's connection status...");
                if(!players.Exists(x => x.Data["playerName"].Value == tempmember.username.text))
                {
                    Debug.Log("Player " + tempmember.username.text + " has disconnected.");
                    members.Remove(tempmember);
                    Destroy(tempmember);
                    break;
                }else
                {
                    Debug.Log("Player " + tempmember.username.text + " is still connected.");
                }
            }
        }

        // Case no. 3: A player's readiness level has changed
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
        if(LobbyManager.Instance.isHost)
        {
            startGameButton.interactable = true;
        }
        statusButton.GetComponentInChildren<TMP_Text>().text = "Not Ready";
    }

    private void changePlayerStatus(){
        LobbyManager.Instance.UpdatePlayerStatus();
    }

    private async void LeaveLobby(){
        await LobbyManager.Instance.LeaveLobby();
    }
}
