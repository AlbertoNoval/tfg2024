using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public float minX, minY, maxX, maxY;
    public List<Player> players;
    private void Awake()
    {
        
    }


    private Transform randomCasilla()
    {
        
        GameObject[] casillas = GameObject.FindGameObjectsWithTag("casilla");
        int random = Random.Range(0, casillas.Length);
        Transform casilla = casillas[random].transform;
        return casilla;
        
    }
    void Start()
    {
        Transform random = randomCasilla();
        Vector3 randomPosition = new Vector3(random.position.x, 1, random.position.z);
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        PhotonNetwork.LocalPlayer.TagObject = player;
        players.Add(player.GetComponent<Player>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
