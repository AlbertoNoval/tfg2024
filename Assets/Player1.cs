using UnityEngine;
using Photon.Pun;

public class Player1 : MonoBehaviourPunCallbacks
{
    public int playerID;
    public GameObject turnButton;
    PhotonView viewPhoton;
    private void Awake()
    {
        viewPhoton = GetComponent<PhotonView>();
        turnButton = GameObject.FindGameObjectWithTag("boton");
    }
    public void ActivarTurno()
    {
        // Activar el botón de turno si este jugador es el actual
        if (viewPhoton.IsMine)
        {
            turnButton.SetActive(true);
        }
    }

    public void DesactivarTurno()
    {
        // Desactivar el botón de turno si este jugador no es el actual
        if (photonView.IsMine)
        {
            turnButton.SetActive(false);
        }
    }
}
