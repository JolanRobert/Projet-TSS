using System.Collections.Generic;
using DefaultNamespace;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static List<PlayerController> Players;
    
    [SerializeField] private PlayerDeviceBuffer devicesSO;
    [SerializeField] private Transform p1SpawnPoint;
    [SerializeField] private Transform p2SpawnPoint;

    private void Start()
    {
        if (devicesSO.player1Device is null && Gamepad.all.Count > 0) devicesSO.player1Device = Gamepad.all[0];
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
        
        var pim = PlayerInputManager.instance;
        var playerInput1 = pim.JoinPlayer(playerIndex:0, controlScheme: "Gamepad", pairWithDevice: devicesSO.player1Device);
        var playerInput2 = pim.JoinPlayer(playerIndex:1, controlScheme: "Gamepad", pairWithDevice: devicesSO.player2Device);

        Players = new List<PlayerController>();
        Players.Add(playerInput1.GetComponent<PlayerController>());
        Players.Add(playerInput2.GetComponent<PlayerController>());
        
        Players[0].Init(p1SpawnPoint.position, PlayerColor.Blue);
        Players[1].Init(p2SpawnPoint.position, PlayerColor.Red);
        
        CameraManager.instance.InitializePlayer(playerInput1);
        CameraManager.instance.InitializePlayer(playerInput2);
    }
}