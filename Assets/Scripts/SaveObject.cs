using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SaveObject : MonoBehaviour
{
    SaveManager _saveManager;

    void Awake()
    {
        _saveManager = FindObjectOfType<SaveManager>();
        if (_saveManager != null)
        {
            _saveManager.AddObject(this);
        }
        else
        {
            Debug.LogWarning("Save Manager not found. Check GameObject load order");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.name);
    }

    public void InitSaveData(List<SaveData> data)
    {
        for (int i = 0; i < data.Count; ++i)
        {
            SaveData curr = data[i];
            Debug.Log("Key: " + curr.key);
            for (int j = 0; j < curr.vals.Count; j++)
            {
                Debug.Log("    Val: " + curr.vals[j]);
            }
        }
    }
}
