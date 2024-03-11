using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Casilla : MonoBehaviour
{
    public string codigo;
    public bool seleccionada = false;
    PhotonView view;
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public abstract void interactuar(Player player);
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && seleccionada)
        {
            Player player = other.GetComponent<Player>();
            player.pisarCasilla(this);
        }
    }
}
