using UnityEngine;

public class Tablero : MonoBehaviour
{
    public GameObject cellPrefab; // El prefab de la celda
    public int rows = 8; // Número de filas
    public int columns = 8; // Número de columnas
    public float cellSpacing = 1.1f; // Espacio entre las celdas
    public int numeroCAsillasConsumibles = 12;
    public int numeroCAsillasArma = 7;
    private void Awake()
    {
        GenerateBoard();
    }
    void Start()
    {
        // Llama a la función para instanciar el tablero
        
    }

    void GenerateBoard()
    {
        
        // Bucle para crear las celdas del tablero
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                
                float xPos = x * Mathf.Sqrt(3) * cellSpacing + (y % 2 == 1 ? Mathf.Sqrt(3) / 2 * cellSpacing : 0);
                float yPos = 1;
                float zPos = y * 1.5f * cellSpacing;

                Vector3 position = new Vector3(xPos, yPos, zPos);

                GameObject cellGO = Instantiate(cellPrefab, position, Quaternion.Euler(new Vector3(-90,0,0)));
                cellGO.transform.SetParent(transform);
                
            }
        }
        transform.position = new Vector3(0, 0, 0);

    }
}
