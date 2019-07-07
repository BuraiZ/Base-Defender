using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using System.Text;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;

        //public RectTransform lobbyServerList;
        public RectTransform lobbyPanel;
        
        //public InputField matchNameInput;
        public Button joinButton;

        private List<MatchInfoSnapshot> matches;

        public void OnEnable()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.matchMaker.ListMatches(0, 4, "", true, 0, 0, OnMatchList);

            //joinButton.onClick.RemoveAllListeners();
            //joinButton.onClick.AddListener(() => { JoinMatch(); });
        }

        public void OnClickCreateMatchmakingGame()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.matchMaker.CreateMatch(
                GenerateMatchName(),
                (uint)lobbyManager.maxPlayers,
                true,
				"", "", "", 0, 0,
				lobbyManager.OnMatchCreate);

            lobbyManager.backDelegate = lobbyManager.StopHost;
            lobbyManager.DisplayIsConnecting();
        }

        private string GenerateMatchName() {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            string number = "0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < 3; i++) {
                ch = alpha[Random.Range(0, alpha.Length)];
                builder.Append(ch);
            }

            builder.Append("-");

            for (int i = 0; i < 3; i++) {
                ch = number[Random.Range(0, number.Length)];
                builder.Append(ch);
            }
            return builder.ToString();
        }

        //public void OnClickOpenServerList()
        //{
        //    //lobbyManager.StartMatchMaker();
        //    lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
        //    //lobbyManager.ChangeTo(lobbyServerList);
        //}

        private NetworkID FindMatch() {
            if (matches.Count == 0) return NetworkID.Invalid;
            return matches[Random.Range(0, matches.Count - 1)].networkId;
        }

        public void JoinMatch() {
            lobbyManager.StartMatchMaker();

            NetworkID networkID = FindMatch();
            if (networkID != NetworkID.Invalid) {
                lobbyManager.matchMaker.JoinMatch(networkID, "", "", "", 0, 0, lobbyManager.OnMatchJoined);

                lobbyManager.backDelegate = lobbyManager.StopClientClbk;
                lobbyManager.DisplayIsConnecting();
            } else {
                lobbyManager.DisplayJoinError();
            }

            //lobbyManager.backDelegate = lobbyManager.StopClient;
        }

        public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
            this.matches = matches;
        }

        public void CountMatches() {
            if (matches != null) print(matches.Count);
        }

        public void RefreshMatchList() {
            lobbyManager.matchMaker.ListMatches(0, 4, "", true, 0, 0, OnMatchList);
        }
    }
}
