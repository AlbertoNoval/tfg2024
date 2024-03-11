using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Tablero : MonoBehaviour
{
    public GameObject hexCellPrefab; // Prefab del hexágono
    public int radius = 5; // Radio del hexágono (en número de celdas)
    public float cellSize = 1f; // Tamaño de la celda
    public float yOffset = 0.1f; // Desplazamiento vertical de las celdas

    private void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        GenerateBoard();
        Node[] casillas = FindObjectsOfType<Node>();
        for (int i = 0; i < casillas.Length; i++)
        {
            for (int j = 0; j < casillas.Length; j++)
            {
                if (Vector3.Distance(casillas[i].transform.position, casillas[j].transform.position) < 2)
                {
                    casillas[i].addNeighbourNode(casillas[j].transform);
                }
            }
                
        }
    }
    void Start()
    {
        // Llama a la función para instanciar el tablero
        navMeshSurface = GetComponent<NavMeshSurface>();

    }
    private NavMeshSurface navMeshSurface;

    void GenerateBoard()
    {
        // Factor de escala vertical para un hexágono equilátero
        float verticalScale = Mathf.Sqrt(3) / 2.3f;

        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                float xPos = q * (cellSize * 1.5f);
                float zPos = r * (cellSize * verticalScale * 2f);

                // Si la columna es impar, desplazar las celdas en Z
                if (q % 2 == 1)
                {
                    zPos += (cellSize * verticalScale);
                }

                Vector3 position = new Vector3(xPos, yOffset, zPos);

                // Instanciar el Prefab del hexágono en la posición calculada
                GameObject hexCell = Instantiate(hexCellPrefab, position, Quaternion.Euler(new Vector3(-90, 0, 0)));
                //hexCell.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
                hexCell.transform.SetParent(transform);
            }
        }
    }
}
