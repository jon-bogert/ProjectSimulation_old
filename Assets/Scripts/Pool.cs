using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int size = 10;

    List<GameObject> pool;
    int currentIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
            go.SetActive(false);
            pool.Add(go);
        }
    }

    public int GetSize()
    {
        return size;
    }

    public GameObject GetNext()
    {
        GameObject go = pool[currentIndex];
        go.SetActive(true);
        currentIndex = (currentIndex + 1) % size;
        return go;
    }
}
