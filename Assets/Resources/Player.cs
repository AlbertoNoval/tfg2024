using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    public int rango;
    public bool esMiTurno = false;
    public PhotonView view;
    public static int cont=0;
    public GameObject canvasTUrno,canvasJugadorStats,canvasAtacar;
    GameManager gameManager;
    private bool teHasMovido = false;
    private bool hasAtacado = false;
    public Node miCasilla;
    public int id;
    /**/
    public string objetoVacioName;
    public string objetoDesconocidoName;
    public RawImage consumibleRawImage;
    public RawImage armaRawImage;
    private string nombre = "Player";
    public TMP_Text nombreTxt;
    private int dañoAtaque = 10;
    public TMP_Text dañoAtaqueTxt;
    private int vida;
    public int vidaMax = 100;
    private ShortestPath shortestPath;

    public TMP_Text healthText;

    public void setTeHasMovido(bool teHasMovido)
    {
        if (view.IsMine)
        {
            this.teHasMovido = teHasMovido;
        }
    }
    public void setHasAtacado(bool hasAtacado)
    {
        if (view.IsMine)
        {
            this.hasAtacado = hasAtacado;
        }
    }
    public Texture getConsumibleSprite()
    {
        return consumibleRawImage.texture;
    }
    public void setConsumibleSprite(string spriteConsumibleName)
    {
        Sprite consumibleTexture = Resources.Load<Sprite>(spriteConsumibleName);
        this.armaRawImage.texture = consumibleTexture.texture;
        UpdatePlayerProperties(spriteConsumibleName,armaRawImage.texture.name);
    }
    public Texture getArmaSprite()
    {
        return armaRawImage.texture;
    }
    public void setArmaSprite(string armaSpriteName)
    {
        Sprite armaTexture = Resources.Load<Sprite>(armaSpriteName);
        this.armaRawImage.texture = armaTexture.texture;
        UpdatePlayerProperties(consumibleRawImage.texture.name,armaSpriteName);
    }

    public string getNombre()
    {
        return nombre;
    }
    public void setNombre(string nombre)
    {
        this.nombre = nombre;
        this.nombreTxt.text = nombre;
        UpdatePlayerProperties(consumibleRawImage.texture.name, armaRawImage.texture.name);
    }
    public int getVida()
    { 
        return vida; 
    }
    public void setVida(int vida)
    {
        this.vida = vida;
        this.vida = Mathf.Clamp(vida, 0, vidaMax);
        healthText.text = "Vida: " + vida;
        UpdatePlayerProperties(consumibleRawImage.texture.name, armaRawImage.texture.name);
    }
    
    public int getDañoAtaque()
    {
        return dañoAtaque;
    }
    public void setDañoAtaque(int dañoAtaque)
    {
        this.dañoAtaque = dañoAtaque;
        this.dañoAtaqueTxt.text = "Daño: " + dañoAtaque;
        UpdatePlayerProperties(consumibleRawImage.texture.name,armaRawImage.texture.name);
    }

    

    private void UpdatePlayerProperties(string recursoConsumible,string recursoArma)
    {
        view.RPC("SyncPlayerProperties", RpcTarget.AllBuffered,
        recursoConsumible, recursoArma, nombre, vida, dañoAtaque); 
    }

    [PunRPC]
    private void SyncPlayerProperties(string consumibleSpriteName, string armaSpriteName, string nombre, int vida, int dañoAtaque)
    {
        Sprite consumibleTexture = Resources.Load<Sprite>(consumibleSpriteName);
        Sprite armaTexture = Resources.Load<Sprite>(armaSpriteName);
        print(consumibleTexture.name);
        print(armaTexture.name);
        this.consumibleRawImage.texture = consumibleTexture.texture;
        this.armaRawImage.texture = armaTexture.texture;
        this.nombre = nombre;
        this.nombreTxt.text = this.nombre + id;
        this.vida = vida;
        this.healthText.text = "Vida: " + this.vida;
        this.dañoAtaque = dañoAtaque;
        this.dañoAtaqueTxt.text = "Daño: " + this.dañoAtaque;
    }

    




    public float rangoMovimiento = 3;
    private List<GameObject> reachableCells = new List<GameObject>();
    /// <summary>
    /// Metodo que visualiza....
    /// </summary>
    public void visualizarCasillasPosibles()
    {
        if (view.IsMine)
        {
            GameObject[] allCells = GameObject.FindGameObjectsWithTag("casilla");
            reachableCells.Clear();
            foreach (GameObject cell in allCells)
            {
                cell.GetComponent<Renderer>().material.color = Color.black;
            }
            foreach (GameObject cell in allCells)
            {
                Renderer renderer = cell.GetComponent<Renderer>();
                if (Vector3.Distance(transform.position, cell.transform.position) <= rangoMovimiento)
                {
                    // Resalta visualmente la casilla alcanzable cambiando su material

                    if (renderer != null)
                    {
                        Material originalMaterial = renderer.material;
                        renderer.material.color = Color.red;
                        reachableCells.Add(cell);
                    }

                }
            }
        }
        
    }
    public void desvisualizarCasillasPosibles()
    {
        GameObject[] allCells = GameObject.FindGameObjectsWithTag("casilla");
        reachableCells.Clear();
        foreach (GameObject cell in allCells)
        {
            cell.GetComponent<Renderer>().material.color = Color.black;
        }
    }
    
    public void seleccionarCasilla(Node casilla)
    {
        miCasilla = casilla;
    }

    NavMeshAgent agent;
    private void Awake()
    {
        shortestPath = FindObjectOfType<ShortestPath>();
        view = GetComponent<PhotonView>();
        gameManager = FindObjectOfType<GameManager>();
        id = cont;
        cont++;
        vida = vidaMax;

    }
    void Start()
    {
        //playerList = getListOtherPlayer();
    }
    public void usarObjeto(Almacenable consumible)
    {
        consumible.interactuar(this);
    }
    public void pisarCasilla(Casilla casilla)
    {
        casilla.interactuar(this);
    }

    public void moverACasilla(Transform posicionCasilla)
    {

    }

    public void pasarTurno()
    {
        if(view.IsMine)
        {
            gameManager.NextTurn();
        }
    }
    public void Die()
    {
        // Desactivar los controles del jugador muerto
        // ...

        // Notificar a los demás jugadores que el jugador ha muerto
        view.RPC("PlayerDied", RpcTarget.AllBuffered, view.Owner.ActorNumber);

        // Eliminar al jugador muerto de la partida
        PhotonNetwork.DestroyPlayerObjects(view.Owner);

        // Salir de la sala
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    private void PlayerDied(int actorNumber)
    {
        // Aquí puedes realizar acciones visuales para indicar que el jugador con el actorNumber dado ha muerto
        // Por ejemplo, eliminar su personaje de la escena o cambiar su apariencia para indicar que está muerto
        
    }

    private IEnumerator corrutinaCaminoMasCorto(HashSet<Transform> lista)
    {
        print("Mi casilla: " + miCasilla.name);
        setTeHasMovido(true);
        foreach (Transform t in lista)
        {
            GameObject objeto = t.gameObject;
            Renderer renderer = objeto.GetComponent<Renderer>();
            renderer.material.color = Color.yellow;

            // Mover al jugador a la posición de la casilla actual
            Vector3 targetPosition = objeto.transform.position;
            float moveSpeed = 10f; // Velocidad de movimiento del jugador

            print("Objeto: " + objeto.name);
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Esperar un corto tiempo entre cada movimiento
            yield return new WaitForSeconds(0.0f);
        }
        GameObject[] casillas = GameObject.FindGameObjectsWithTag("casilla");
        for(int i = 0; i < casillas.Length; i++)
        {
            if (Vector3.Distance(casillas[i].transform.position,transform.position) < 0.1)
            {
                miCasilla = casillas[i].GetComponent<Node>();
            }
        }
        desvisualizarCasillasPosibles();

    }
    public HashSet<Transform> caminoMasCorto = new HashSet<Transform>();
    // Update is called once per frame
    void Update()
    {
        
        if(view.IsMine)
        {
            //transform.Translate(new Vector3(joystick.Horizontal * speed * Time.deltaTime, 0, joystick.Vertical * speed * Time.deltaTime));
            
            if(esMiTurno)
            {
                if(teHasMovido == false)
                {
                    visualizarCasillasPosibles();
                }
                canvasTUrno.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject detectado = getRaycastGameobjectFromTag("casilla");
                    if (detectado != null)
                    {
                        if(reachableCells.Contains(detectado) && teHasMovido == false)
                        {
                            caminoMasCorto = shortestPath.findShortestPath(miCasilla.transform, detectado.transform);
                            caminoMasCorto.Add(detectado.transform);
                            StartCoroutine(corrutinaCaminoMasCorto(caminoMasCorto));
                        }
                    }
                }
            }
            else
            {
                canvasTUrno.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject detectado = getRaycastGameobjectFromTag("Player");
                if(detectado != null)
                {
                    Player playerScript = detectado.GetComponent<Player>();

                    if (playerScript.id != this.id)
                    {
                        playerScript.canvasJugadorStats.SetActive(true);
                        int randomVida = Random.Range(1, 100);
                        int randomDaño = Random.Range(1, 100);
                        playerScript.setVida(randomVida);
                        playerScript.setDañoAtaque(randomDaño);
                        playerScript.setConsumibleSprite(objetoDesconocidoName);
                        playerScript.setArmaSprite(objetoDesconocidoName);
                        playerScript.setNombre("Player" + id);

                    }
                }
            }
            if (vida <= 0)
            {
                Die();
            }
        }
        //healthText.text = "Vida: " + this.vida;
        

    }
    /// <summary>
    /// METODO QUE NOS DEVUELVE EL OBJETO DE LA ESCENA QUE HAYA TOCADO CON EL DEDO CON EL TAG QUE LE PASEMOS POR PARAMETRO
    /// USAREMOS ESTE METODO PARA LLAMARLO, VERIFICAR QUE EL OBJETO NO SEA NULO Y EN ESE CASO HACER LO QUE SEA
    /// EJEMPLO: DETECTAR CUANDO SE PULSA SOBRE UN JUGADOR PARA MOSTRAR SU INFORMACION, O DETECTAR A LA CASILLA QUE TE QUIERES MOVER
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public GameObject getRaycastGameobjectFromTag(string tag)
    {
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        GameObject objeto = null;
        // Si el rayo golpea un collider con el tag "casilla"
        if (Physics.Raycast(rayo, out hit,Mathf.Infinity) && hit.collider.CompareTag(tag))
        {
            objeto = hit.collider.gameObject;
        }
        return objeto;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, rangoMovimiento);
    }

}
