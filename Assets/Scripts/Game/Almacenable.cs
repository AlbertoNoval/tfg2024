using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CLASE PADRE DE CONSUMIBLE Y ARMA.
/// CLASE HIJA DE CASILLA
/// </summary>
public abstract class Almacenable : Casilla
{
    /// <summary>
    /// METODO PARA COGER EL CONSUMIBLE O ARMA Y GUARDARLO EN EL INVENTARIO. EN EL CASO DE TENER UN CONSUMIBLE YA, PREGUNTAR
    /// POR SI QUIERES CAMBIARLO
    /// </summary>
    /// <param name="player"></param>
    public override void interactuar(Player player)
    {
        
    }
    /// <summary>
    /// SE ACTIVA CUANDO EL JUGADOR DECIDE USAR EL OBJETO, ESTE METODO ESTARA IMPLEMENTADO DIFERENTE EN CADA UNO
    /// DE SUS HIJOS
    /// </summary>
    /// <param name="player"></param>
    public abstract void efectoAlUsar(Player player);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
