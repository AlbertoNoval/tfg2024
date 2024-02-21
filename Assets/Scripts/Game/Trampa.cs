using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trampa : Casilla
{
    /// <summary>
    /// METODO QUE SOBREESCRIBEN SUS HIJOS(LOS TIPOS DE TRAMPA) Y CADA TRAMPA QUE SU INTERACCIÓN CON EL JUGADOR SEA DIFERENTE, ES DISTINTO
    /// AL CONSUMIBLE.
    /// </summary>
    /// <param name="player"></param>
    public override void interactuar(Player player)
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
