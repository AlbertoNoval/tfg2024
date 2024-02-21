using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManager1 : MonoBehaviourPunCallbacks
{
    private const int NUMERO_JUGADORES_ESPERADOS = 1;
    private int numeroJugadoresActuales = 0;
    private Player1[] jugadores;
    private int indiceJugadorActual = 0;

    public TextMeshProUGUI textoEspera;

    void Start()
    {
        jugadores = new Player1[NUMERO_JUGADORES_ESPERADOS];
        if (PhotonNetwork.IsMasterClient)
        {
            textoEspera.text = "Esperando jugadores...";
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.IsMasterClient)
        {
            // Incrementar el número de jugadores actuales
            numeroJugadoresActuales++;

            // Actualizar la UI con el número de jugadores actuales
            textoEspera.text = "Esperando jugadores... (" + numeroJugadoresActuales + "/" + NUMERO_JUGADORES_ESPERADOS + ")";

            // Si ya hay suficientes jugadores, comenzar el juego
            if (numeroJugadoresActuales >= NUMERO_JUGADORES_ESPERADOS)
            {
                ComenzarJuego();
            }
        }
    }

    void ComenzarJuego()
    {
        // Ocultar el texto de espera
        textoEspera.gameObject.SetActive(false);

        // Obtener y activar los jugadores en la escena
        jugadores = FindObjectsOfType<Player1>();
        foreach (Player1 jugador in jugadores)
        {
            jugador.gameObject.SetActive(true);
        }

        // Iniciar el turno para el primer jugador
        IniciarTurnoParaPrimerJugador();
    }

    void IniciarTurnoParaPrimerJugador()
    {
        // Activar el turno para el primer jugador
        jugadores[indiceJugadorActual].ActivarTurno();
    }

    public void PasarSiguienteTurno()
    {
        // Desactivar el turno del jugador actual
        jugadores[indiceJugadorActual].DesactivarTurno();

        // Pasar al siguiente jugador
        indiceJugadorActual = (indiceJugadorActual + 1) % NUMERO_JUGADORES_ESPERADOS;

        // Activar el turno para el siguiente jugador
        jugadores[indiceJugadorActual].ActivarTurno();
    }
}
