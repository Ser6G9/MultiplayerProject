using DilmerGames.Core.Singletons;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private TextMeshProUGUI playersInGameText;
    
    [SerializeField]
    private Button executePhysicsButton;
    
    private bool hasServerStarted;
    
    private void Awake()
    {
        Cursor.visible = true;
    }

    void Update()
    {
        playersInGameText.text = $"Jugadores en partida: {PlayersManager.Instance.PlayersInGame}";
    }

    void Start()
    {
        // START HOST
        startHostButton?.onClick.AddListener(async () =>
        {
            /*if (RelayManager.Instance.IsRelayEnabled)
            {
                await RelayManager.Instance.SetupRelay();
            }*/

            if (NetworkManager.Singleton.StartHost())
            {
                Logger.Instance.LogInfo("Host started...");
            }
            else
            {
                Logger.Instance.LogInfo("Unable to start host...");
            }
        });
        
        // START SERVER
        startServerButton?.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
            {
                Logger.Instance.LogInfo("Server started...");
            }
            else
            {
                Logger.Instance.LogInfo("Unable to start server...");
            }
        });

        // START CLIENT
        startClientButton?.onClick.AddListener(async () =>
        {
            /*if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text))
            {
                await RelayManager.Instance.JoinRelay(joinCodeInput.text);
            }*/

            if (NetworkManager.Singleton.StartClient())
            {
                Logger.Instance.LogInfo("Client started...");
            }
            else
            {
                Logger.Instance.LogInfo("Unable to start client...");
            }
        });
        
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            hasServerStarted = true;
        };

        executePhysicsButton.onClick.AddListener(() => 
        {
            if (!hasServerStarted)
            {
                Logger.Instance.LogWarning("Server has not started...");
                return;
            }
            SpawnerControl.Instance.SpawnObjects();
        });
    }
}
