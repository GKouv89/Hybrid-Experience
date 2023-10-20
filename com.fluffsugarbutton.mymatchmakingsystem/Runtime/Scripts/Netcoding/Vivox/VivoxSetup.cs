using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.Android;
using VivoxUnity;

// This follows closely Unity's Game Lobby Sample repo, that provides
// a sample for integrating Lobby, Relay and Vivox.
// https://github.com/Unity-Technologies/com.unity.services.samples.game-lobby/tree/main

namespace MatchMaking.vivox
{
    public class VivoxSetup
    {
        private bool m_hasInitialized = false;
        private bool m_isMidInitialize = false;
        private ILoginSession m_loginSession = null;
        private IChannelSession m_channelSession = null;
        private string m_id;
        private List<VivoxUserHandler> m_userHandlers = new List<VivoxUserHandler>();

        // This does not complete any initialization of authentication service
        // This takes place in the Lobby Manager
        public void Initialize()
        {
            // Helps avoid double initialization attempts
            if(m_isMidInitialize)
                return;
            m_isMidInitialize = true;
            
            for(int i = 0; i < 2; i++)
            {
                m_userHandlers.Add(new VivoxUserHandler());
            }

            VivoxService.Instance.Initialize();

            m_id = AuthenticationService.Instance.PlayerId;
            Account account = new Account(m_id);
            m_loginSession = VivoxService.Instance.Client.GetLoginSession(account);
            string token = m_loginSession.GetLoginToken();

            // This starts logging in the user to the Vivox service
            m_loginSession.BeginLogin(token, SubscriptionMode.Accept, null, null, null, result =>
            {
                try
                {
                    // This method is called to signify that the Login was successfult and it can end.
                    m_loginSession.EndLogin(result);
                    m_hasInitialized = true;
                    // Right now, no need for any handler on connect
                }
                catch (Exception ex)
                {
                    Debug.Log("Vivox failed to login: " + ex.Message);
                    // Right now, no need for any handler on connect
                }
                finally
                {
                    m_isMidInitialize = false;
                }
            });
        }

        public void JoinChannel(string lobbyId)
        {
            if (!m_hasInitialized || m_loginSession.State != LoginState.LoggedIn)
            {
                Debug.Log("Can't join a Vivox audio channel, as Vivox login hasn't completed yet.");
                return;
            }

            RequestPermissions();

            ChannelType channelType = ChannelType.NonPositional;
            Channel channel = new Channel(lobbyId + "_voice", channelType, null);
            Debug.Log("Channel name: " + lobbyId + "_voice");
            m_channelSession = m_loginSession.GetChannelSession(channel);
            string token = m_channelSession.GetConnectToken();
            
            m_channelSession.BeginConnect(true, false, true, token, result =>
            {
                try
                {
                    // Special case: It's possible for the player to leave the lobby between the time we called BeginConnect and the time we hit this callback.
                    // If that's the case, we should abort the rest of the connection process.
                    if (m_channelSession.ChannelState == ConnectionState.Disconnecting ||
                        m_channelSession.ChannelState == ConnectionState.Disconnected)
                    {
                        Debug.Log("Vivox channel is already disconnecting. Terminating the channel connect sequence.");
                        // So, wait while we're not connecting, and trigger the Leave method
                        HandleEarlyDisconnect();
                        return;
                    }

                    m_channelSession.EndConnect(result);
                    Debug.Log("Joined channel!");
                    foreach (VivoxUserHandler userHandler in m_userHandlers)
                        userHandler.OnChannelJoined(m_channelSession);
                }
                catch (Exception ex)
                {
                    Debug.Log("Vivox failed to connect: " + ex.Message);
                    m_channelSession?.Disconnect();
                }
            });
        }


        public void LeaveChannel()
        {
            if (m_channelSession != null)
            {
                // Special case: The EndConnect call requires a little bit of time before the connection actually completes, but the player might
                // disconnect before then. If so, sending the Disconnect now will fail, and the played would stay connected to voice while no longer
                // in the lobby. So, wait until the connection is completed before disconnecting in that case.
                if (m_channelSession.ChannelState == ConnectionState.Connecting)
                {
                    Debug.Log("Vivox channel is trying to disconnect while trying to complete its connection. Will wait until connection completes.");
                    // So, wait while we're not connecting, and trigger the Leave method
                    HandleEarlyDisconnect();
                    return;
                }

                ChannelId id = m_channelSession.Channel;
                m_channelSession?.Disconnect(
                    (result) =>
                    {
                        m_loginSession.DeleteChannelSession(id);
                        m_channelSession = null;
                    });

            }

            foreach (VivoxUserHandler userHandler in m_userHandlers)
                userHandler.OnChannelLeft();
        }

        private void HandleEarlyDisconnect()
        {
            DisconnectOnceConnected();
        }

        async void DisconnectOnceConnected()
        {
            while (m_channelSession?.ChannelState == ConnectionState.Connecting)
            {
                await Task.Delay(200);
                return;
            }

            LeaveChannel();
        }

        private void RequestPermissions()
        {
            // Request Runtime Permissions
            #if UNITY_ANDROID
                if(Permission.HasUserAuthorizedPermission(Permission.Microphone))
                {
                    Debug.Log("Mic Permissions already granted!");
                }
                else
                {
                    Permission.RequestUserPermission(Permission.Microphone);
                }
            #endif
        }
    }
}
