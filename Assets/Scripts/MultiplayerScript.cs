using UnityEngine;
using System.Collections;

/// <summary>
/// Multiplayer script.
/// Attached to the multiplayer manager and provides multiplayer
/// functionality within the game
/// </summary>

public class MultiplayerScript : MonoBehaviour {

	public string playerName;
	public string serverName;
	public string serverNameForClient;
	public bool showDisconnectWindow = false;

	private string titleMessage = "GTGD Series 1 Prototype";
	private string connectToIP = "127.0.0.1";
	private int connectionPort = 26500;
	private bool useNAT = false;
	private string ipAddress;
	private string port;
	private int numberOfPlayers = 10;
	private bool iWantToSetupAServer = false;
	private bool iWantToConnectToAServer = false;
	private Rect connectionWindowRect;
	private int connectionWindowWidth = 400;
	private int connectionWindowHeight = 280;
	private int buttonHeight = 60;
	private int leftIndent;
	private int topIndent;
	private Rect serverDisWindowRect;
	private int serverDisWindowWidth = 300;
	private int serverDisWindowHeight = 150;
	private int serverDisWindowLeftIndent = 10;
	private int serverDisWindowTopIndent = 10;
	private Rect clientDisWindowRect;
	private int clientDisWindowWidth = 300;
	private int clientDisWindowHeight = 170;

	// Use this for initialization
	void Start () {
		serverName = PlayerPrefs.GetString("serverName");
		if (serverName == "") {
			serverName = "Server";
		}

		playerName = PlayerPrefs.GetString("playerName");
		if (playerName == "") {
			playerName = "Player";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			showDisconnectWindow = !showDisconnectWindow;
		}
	}

	void OnGUI() {
		if (Network.peerType == NetworkPeerType.Disconnected) {
			leftIndent = (Screen.width / 2) - (connectionWindowWidth / 2);
			topIndent = (Screen.height / 2) - (connectionWindowHeight / 2);
			connectionWindowRect = new Rect(leftIndent, topIndent, connectionWindowWidth, connectionWindowHeight);
			connectionWindowRect = GUILayout.Window(0, connectionWindowRect, ConnectWindow, titleMessage);
		} else if (Network.peerType == NetworkPeerType.Server) {
			serverDisWindowRect = new Rect(serverDisWindowLeftIndent, serverDisWindowTopIndent, serverDisWindowWidth, serverDisWindowHeight);
			serverDisWindowRect = GUILayout.Window(1, serverDisWindowRect, ServerDisconnectWindow, "");
		} else if (Network.peerType == NetworkPeerType.Client && showDisconnectWindow) {
			int clientLeftIndent = (Screen.width / 2) - (clientDisWindowWidth / 2);
			int clientTopIndent = (Screen.height / 2) - (clientDisWindowHeight / 2);
			clientDisWindowRect = new Rect(clientLeftIndent, clientTopIndent, clientDisWindowWidth, clientDisWindowHeight);
			clientDisWindowRect = GUILayout.Window(2, clientDisWindowRect, ClientDisconnectWindow, "");
		}
	}

	void OnDisconnectedFromServer() {
		Application.LoadLevel(Application.loadedLevel);
	}

	void OnPlayerDisconnected(NetworkPlayer networkPlayer) {
		Network.RemoveRPCs(networkPlayer);
		Network.DestroyPlayerObjects(networkPlayer);
	}

	void OnPlayerConnected(NetworkPlayer networkPlayer) {
		networkView.RPC("TellPlayerServerName", networkPlayer, serverName);
	}

	void ConnectWindow(int windowID){
		// leave a gap from the header
		GUILayout.Space(15);

		// when the player launches the game they have the option
		// to create a server or join a server
		if (!iWantToSetupAServer && !iWantToConnectToAServer) {
			if (GUILayout.Button("Setup a server", GUILayout.Height(buttonHeight))) {
				iWantToSetupAServer = true;
			}

			GUILayout.Space(10);

			if (GUILayout.Button("Connect to a server", GUILayout.Height(buttonHeight))) {
				iWantToConnectToAServer = true;
			}

			GUILayout.Space(10);

			if (!Application.isWebPlayer && !Application.isEditor) {
				if (GUILayout.Button("Exit prototype", GUILayout.Height(buttonHeight))) {
					Application.Quit();
				}
			}
		}

		if (iWantToSetupAServer) {
			GUILayout.Label("Enter a name for your server");
			serverName = GUILayout.TextField(serverName);
			GUILayout.Space(5);

			GUILayout.Label("Server port");
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			GUILayout.Space(10);

			if (GUILayout.Button("Start my own server", GUILayout.Height(30))) {
				// Create server
				Network.InitializeServer(numberOfPlayers, connectionPort, useNAT);
				PlayerPrefs.SetString("serverName", serverName);
				iWantToSetupAServer = false;
			}

			if (GUILayout.Button("Go back", GUILayout.Height(30))) {
				iWantToSetupAServer = false;
			}
		}

		if (iWantToConnectToAServer) {
			GUILayout.Label("Enter your player name");

			playerName = GUILayout.TextField(playerName);
			GUILayout.Space(5);

			GUILayout.Label("Server IP");
			connectToIP = GUILayout.TextField(connectToIP);
			GUILayout.Space(5);

			GUILayout.Label("Server port");
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			GUILayout.Space(5);

			if (GUILayout.Button("Connect to server", GUILayout.Height(25))) {
				// ensure that the player has specified a name
				if (playerName == "") {
					playerName = "Player";
				}

				// connect to server
				Network.Connect(connectToIP, connectionPort);
				PlayerPrefs.SetString("playerName", playerName);
			}

			GUILayout.Space(5);

			if (GUILayout.Button("Go Back", GUILayout.Height(25))) {
				iWantToConnectToAServer = false;
			}
		}
	}

	void ServerDisconnectWindow(int windowID) {
		GUILayout.Label("Server name: " + serverName);
		GUILayout.Label("Players connected: " + Network.connections.Length);
		if (Network.connections.Length > 0) {
			GUILayout.Label ("Avg ping: " + Network.GetAveragePing(Network.connections[0]));
		}
		if (GUILayout.Button("Shut down server")) {
			Network.Disconnect();
		}
	}

	void ClientDisconnectWindow(int windowID) {
		GUILayout.Label("Connected to server: " + serverName);
		GUILayout.Label ("Avg ping: " + Network.GetAveragePing(Network.connections[0]));
		GUILayout.Space(7);
		if (GUILayout.Button("Disconnect", GUILayout.Height(25))) {
			Network.Disconnect();
		}
		GUILayout.Space(5);
		if (GUILayout.Button ("Return to game", GUILayout.Height (25))) {
			showDisconnectWindow = false;
		}
	}

	[RPC]
	void TellPlayerServerName(string _serverName) {
		serverName = _serverName;
	}
}
