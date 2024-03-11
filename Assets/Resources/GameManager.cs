using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using Unity.Mathematics;
using ExitGames.Client.Photon.StructWrapping;

public class GameManager : MonoBehaviourPunCallbacks
{
    


    private Dictionary<int, bool> playerTurns = new Dictionary<int, bool>();
    private int currentTurnIndex = 0; // Índice del jugador actual en la lista de turnos
    IEnumerator ComenzarTurnoCuandoListo()
    {
        print("OJo");
        print(PhotonNetwork.CurrentRoom.MaxPlayers);
        while (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            yield return null; // Espera hasta que todos los jugadores estén en la sala
        }
        yield return new WaitForSecondsRealtime(2f);
        AssignPlayerTurns();
    }
    private void Start()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(ComenzarTurnoCuandoListo());
        }
    }

    private void AssignPlayerTurns()
    {
        int playerCount = PhotonNetwork.PlayerList.Length;

        // Inicializar los turnos para todos los jugadores
        for (int i = 0; i < playerCount; i++)
        {
            int actorNumber = PhotonNetwork.PlayerList[i].ActorNumber;
            playerTurns.Add(actorNumber, i == 0); // El primer jugador comienza el juego
        }

        // Sincronizar los turnos con todos los jugadores
        SyncPlayerTurns();
    }

    // Método para sincronizar los turnos con todos los jugadores
    private void SyncPlayerTurns()
    {
        // Enviar los turnos a todos los jugadores mediante RPC
        photonView.RPC("ReceivePlayerTurnsFromMaster", RpcTarget.All, playerTurns, currentTurnIndex);
    }

    // RPC para recibir los turnos del maestro y actualizarlos en todos los jugadores
    [PunRPC]
    private void ReceivePlayerTurnsFromMaster(Dictionary<int, bool> playerTurnsData, int currentTurn)
    {
        playerTurns = playerTurnsData;
        currentTurnIndex = currentTurn;

        // Actualizar el turno local del jugador
        UpdateLocalPlayerTurn();
    }

    // Método para actualizar el turno del jugador local
    private void UpdateLocalPlayerTurn()
    {
        if (playerTurns.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            
            bool isMyTurn = playerTurns[PhotonNetwork.LocalPlayer.ActorNumber];
            GameObject jugadorTurnoObject = PhotonNetwork.LocalPlayer.TagObject as GameObject;

            Player jugadorTurnoPlayer = jugadorTurnoObject.GetComponent<Player>();
            jugadorTurnoPlayer.esMiTurno= isMyTurn;
            jugadorTurnoPlayer.setTeHasMovido(!isMyTurn);
            //jugadorTurnoPlayer.visualizarCasillasPosibles();

            //jugadorTurnoPlayer.playerList = jugadorTurnoPlayer.getListOtherPlayer();
            // Aquí puedes realizar las acciones necesarias según el turno del jugador local
        }
    }

    // Método para pasar al siguiente turno
    public void NextTurn()
    {
        playerTurns[PhotonNetwork.PlayerList[currentTurnIndex].ActorNumber] = false;
        // Incrementar el índice del turno actual y asegurarse de que esté dentro del rango
        currentTurnIndex = (currentTurnIndex + 1) % PhotonNetwork.PlayerList.Length;
        playerTurns[PhotonNetwork.PlayerList[currentTurnIndex].ActorNumber] = true;

        // Sincronizar los turnos actualizados con todos los jugadores
        SyncPlayerTurns();
    }






    
    
}
    
