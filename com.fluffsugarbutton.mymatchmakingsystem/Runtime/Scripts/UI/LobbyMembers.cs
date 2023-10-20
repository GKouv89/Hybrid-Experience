using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using TMPro; 
using Button = UnityEngine.UI.Button;

namespace MatchMaking.LobbySetup.UI
{
    public class LobbyMembers : MonoBehaviour
    {
        public LobbyMemberDisplay prefab;
        public Transform parent;
        private LobbyMemberDisplay myMemberDisplay; 
        private List<LobbyMemberDisplay> members;
        private LobbyMemberDisplay header;
        [SerializeField] private Button statusButton;
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button backButton;
        [SerializeField] private GameObject Popup;
        private PopupView view;
        private LobbyManager m_LobbyManager = LobbyManager.Instance;
        private LobbyMemberDisplay createMember(Player player, Transform parent){
            LobbyMemberDisplay member;
            member = Instantiate(prefab, parent);
            member.username.text = player.Data["playerName"].Value;
            if(player.Data["playerName"].Value.Equals(m_LobbyManager.player.Data["playerName"].Value))
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
            // Disabling the popup
            view = Popup.GetComponent<PopupView>();
            view.HidePopup();

            members = new List<LobbyMemberDisplay>();
            header = createHeader(parent);
            List<Player> players = m_LobbyManager.MyLobby.Players;
            for (int i = 0; i < players.Count; i++){
                members.Add(createMember(players[i], parent));
            }

            statusButton.onClick.AddListener(changePlayerStatus);
            m_LobbyManager.OnLobbyChange += updateMembers;

            startGameButton.onClick.AddListener(m_LobbyManager.UpdateGameStatus);
            startGameButton.interactable = false;

            backButton.onClick.AddListener(() => LeaveLobby(false));
        }

        private void updateMembers()
        {
            List<Player> players = m_LobbyManager.MyLobby.Players;
            bool allPlayersReady = true;

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
                if(!m_LobbyManager.isHost) 
                {
                    // If I'm not the host, and I'm still connected
                    // and a player has disconnected, 
                    // this means it was the host
                    // Instead of implementing host migration
                    // Just force exit 
                    LeaveLobby(true);
                    return;
                }

                Debug.Log("A player has disconnected");
                foreach(LobbyMemberDisplay tempmember in members)
                {
                    Debug.Log("Checking player " + tempmember.username.text + " 's connection status...");
                    if(!players.Exists(x => x.Data["playerName"].Value == tempmember.username.text))
                    {
                        Debug.Log("Player " + tempmember.username.text + " has disconnected.");
                        members.Remove(tempmember);
                        Destroy(tempmember.gameObject);
                        break;
                    }else
                    {
                        Debug.Log("Player " + tempmember.username.text + " is still connected.");
                    }
                }
                allPlayersReady = false;
            }

            // Case no. 3: A player's readiness level has changed
            LobbyMemberDisplay member;
            for(int i = 0; i < players.Count; i++){
                member = members.Find(x => x.username.text == players[i].Data["playerName"].Value);
                member.editMember(players[i]);
                if(players[i].Data["ready"].Value.Equals("Not Ready"))
                {

                    allPlayersReady = false;
                }
            }

            // Can the host start the game?
            if(members.Count == 2 && allPlayersReady && m_LobbyManager.isHost)
            {
                startGameButton.interactable = true;
            }else{
                startGameButton.interactable = false;
            }

            if(myMemberDisplay.isReady())
            {
                statusButton.GetComponentInChildren<TMP_Text>().text = "Not Ready";
                return;
            }
            statusButton.GetComponentInChildren<TMP_Text>().text = "Ready";
        }

        private void changePlayerStatus(){
            m_LobbyManager.UpdatePlayerStatus();
        }

        private void LeaveLobby(bool isForced){
            // No need to keep updating this scene.
            m_LobbyManager.OnLobbyChange -= updateMembers;
            
            // Destroy everything here
            // Header
            Destroy(header.gameObject);
            // Members
            foreach(LobbyMemberDisplay member in members)
            {
                Destroy(member.gameObject);
            }
            // Does the list need cleanup?
            members.Clear();
            // Buttons
            Destroy(statusButton.gameObject);
            Destroy(startGameButton.gameObject);
            Destroy(backButton.gameObject);

            // Enabling popup
            if(isForced)
            {
                // We will show the popup and wait for the user to leave the lobby through clicking the button
                view.ShowPopup();
            }else{
                MainManager.Instance.LeaveLobby();
            }
        }

    }
}
