//Create Buttons to host or join common game
using Unity.Netcode;
using UnityEngine;

    public class NetCodeManager : MonoBehaviour
    {
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                //If the manager belong to server and not client
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                {
                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                    //NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HwPlayer>().Move();
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).transform.position = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<OnlinePlayerMovement>().Position.Value;

                }
                else
                {
                    //var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    //var player = playerObject.GetComponent<OnlinePlayerMovement>();
                    //player.Move();
                }
            }
        }
    }