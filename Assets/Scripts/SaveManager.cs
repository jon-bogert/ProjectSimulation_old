using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public string key;
    public List<string> vals;
}

public class SaveManager : MonoBehaviour
{
    [SerializeField] string _fileName;
    
    List<SaveObject> _saveObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello World");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObject(SaveObject obj)
    {
        _saveObjects.Add(obj);
        //obj.InitSaveData();
    }
}
