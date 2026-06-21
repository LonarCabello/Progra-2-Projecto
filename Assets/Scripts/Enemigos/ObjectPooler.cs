using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
    public float velocity;
    public int damage;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    public List<Pool> pools;

    private Dictionary<string, Queue<GameObject>> poolDictionary;
    
    void Awake()
    {
        // Implementación del patrón Singleton.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Si ya existe otra instancia, la destruimos.
            Destroy(gameObject);
            return;
        }
        
        // No destruir al cargar una nueva escena.
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Iterar a través de las reservas para inicializarlas.
        foreach (Pool pool in pools)
        {
            // Crear una nueva cola (Queue) para el tipo de objeto.
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Instanciar los objetos y añadirlos a la cola.
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); // Desactivar el objeto al crearlo.
                objectPool.Enqueue(obj);
            }

            // Añadir la cola al diccionario con su etiqueta.
            poolDictionary.Add(pool.tag, objectPool);
        }

        
    }

    // Método para obtener un objeto de la reserva.
    public GameObject SpawnFromPool(string tag, GameObject firePoint, Vector3 direction)
    {
        if (poolDictionary == null)
        {
            Debug.LogWarning("PoolDictionary no ha sido inicializado.");
            return null;
        }
        // Si el tag no existe, lanzar un error y salir.
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool con tag " + tag + " no existe.");
            return null;
        }

        // Obtener el objeto de la cola.
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        Rigidbody rb = objectToSpawn.GetComponent<Rigidbody>();

        // Activarlo y reubicarlo.
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = firePoint.transform.position;
        objectToSpawn.transform.up = direction;
        float v = pools.Find(p => p.tag == tag).velocity;
        rb.linearVelocity = direction * v;
        return objectToSpawn;
    }

     // Método para devolver un objeto a la reserva.
    public void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        poolDictionary[objectToReturn.tag].Enqueue(objectToReturn);
    }

    
}
