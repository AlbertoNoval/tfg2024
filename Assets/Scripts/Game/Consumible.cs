using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// LOS CONSUMIBLES TIENEN DOS METODOS, UNO IGUALES PARA TODOS QUE SE ACTIVA AL CAER EN LA CASILLA, TE EQUIPA EL CONSUMIBLE O PREGUNTA SI QUIERES CAMBIAR EL QUE TIENES
/// POR EL NUEVO. EL SEGUNDO METODO SERÁ EL EFECTO DEL CONSUMIBLE, QUE SE ACTIVA CUANDO EL JUGADOR DECIDE USAR EL OBJETO, ESTE METODO ESTARA IMPLEMENTADO DIFERENTE EN CADA UNO
/// DE SUS HIJOS
/// </summary>
public abstract class Consumible : Almacenable
{
    /// <summary>
    /// SE ACTIVA CUANDO EL JUGADOR DECIDE USAR EL OBJETO, ESTE METODO ESTARA IMPLEMENTADO DIFERENTE EN CADA UNO
    /// DE SUS HIJOS
    /// </summary>
    /// <param name="player"></param>
    

    void Start()
    {
        
    }
}
