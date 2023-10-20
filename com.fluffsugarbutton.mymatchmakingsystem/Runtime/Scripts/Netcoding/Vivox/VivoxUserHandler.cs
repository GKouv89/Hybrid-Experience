using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.Android;
using VivoxUnity;

namespace MatchMaking.vivox
{
    public class VivoxUserHandler
    {
        private IChannelSession m_channelSession;
        private static void ValidateArgs(object[] objs)
        {
            foreach (var obj in objs)
            {
                if (obj == null)
                    throw new ArgumentNullException(obj.GetType().ToString(), "Specify a non-null/non-empty argument.");
            }
        }

        private void OnParticipantAdded(object sender, KeyEventArg<string> keyEventArg)
        {
            var source = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;
            var participant = source[keyEventArg.Key];
            // var username = participant.Account.DisplayName;

            // bool isThisUser = username == m_id;
            // if (isThisUser)
            if(participant.IsSelf)
            {
                Debug.Log("C'est moi!");
                // m_vivoxId = keyEventArg.Key; // Since we couldn't construct the Vivox ID earlier, retrieve it here.
                // m_lobbyUserVolumeUI.IsLocalPlayer = participant.IsSelf;

                if (!participant.IsMutedForAll)
                    // m_lobbyUserVolumeUI.EnableVoice(false); //Should check if user is muted or not.
                    Debug.Log("Seems I'm not muted at all!");
                else
                    Debug.Log("Uh-oh! I'm muted for all!");
                    // m_lobbyUserVolumeUI.DisableVoice(false);
            }
            else
            {
                Debug.Log("C'est ne pas moi!");
                if (!participant.LocalMute)
                    // m_lobbyUserVolumeUI.EnableVoice(false); //Should check if user is muted or not.
                    Debug.Log("Participant is a-ok for me!");
                else
                    Debug.Log("Participant is local muted for me!");
                    // m_lobbyUserVolumeUI.DisableVoice(false);
            }
        }

        private void OnParticipantValueUpdated(object sender, ValueEventArg<string, IParticipant> valueEventArg)
        {
            Debug.Log("Participant Value Updated");
            
            ValidateArgs(new object[] { sender, valueEventArg });

            var source = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;
            var participant = source[valueEventArg.Key];
            Debug.Log("Issa me?");
            if(participant.IsSelf)
            {
                Debug.Log("Oui!");
            }
            else
            {
                Debug.Log("Mais non!");
            }
            string username = valueEventArg.Value.Account.Name;
            // ChannelId channel = valueEventArg.Value.ParentChannelSession.Key;
            string property = valueEventArg.PropertyName;
            // Debug.Log("Property: " + property);

            switch (property)
            {
                // case "LocalMute":
                // {
                //     if (username != accountId.Name) //can't local mute yourself, so don't check for it
                //     {
                //         //update their muted image
                //         Debug.Log("User local mute");
                //     }
                //     break;
                // }
                case "SpeechDetected":
                {
                    //update speaking indicator image
                    Debug.Log("SpeechDetected");
                    if(participant.IsSelf)
                    {
                        Debug.Log("By me");
                    }
                    else
                    {
                        Debug.Log("By someone else");
                    }
                    break;
                }
                case "UnavailableCaptureDevice":
                {
                    if (participant.UnavailableCaptureDevice)
                    {
                        Debug.Log("UnavailableCaptureDevice");
                        // m_lobbyUserVolumeUI.DisableVoice(false);
                        // participant.SetIsMuteForAll(true, null); // Note: If you add more places where a player might be globally muted, a state machine might be required for accurate logic.
                    }
                    else
                    {
                        Debug.Log("Capture Device A-OK");
                        // m_lobbyUserVolumeUI.EnableVoice(false);
                        // participant.SetIsMuteForAll(false, null); // Also note: This call is asynchronous, so it's possible to exit the lobby before this completes, resulting in a Vivox error.
                    }
                    break;
                }
                case "IsMutedForAll":
                {
                    if (participant.IsMutedForAll)
                        Debug.Log("IsMutedForAll");
                        // m_lobbyUserVolumeUI.DisableVoice(false);
                    else
                        Debug.Log("NO IsMutedForAll");
                        // m_lobbyUserVolumeUI.EnableVoice(false);                    
                    break;
                }
                default:
                break;
            }
        }

        public void OnChannelJoined(IChannelSession channelSession)
        {
            m_channelSession = channelSession;
            m_channelSession.Participants.AfterKeyAdded += OnParticipantAdded;
            m_channelSession.Participants.AfterValueUpdated += OnParticipantValueUpdated;
        }

        public void OnChannelLeft()
        {
            if (m_channelSession != null) // It's possible we'll attempt to leave a channel that isn't joined, if we leave the lobby while Vivox is connecting.
            {
                m_channelSession.Participants.AfterValueUpdated -= OnParticipantValueUpdated;
                m_channelSession = null;
            }
        }
    }
}