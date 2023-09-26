using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    float time_counter = 0;
    [SerializeField] float spawn_frequence = 5;
    [SerializeField] GameObject player;
    [SerializeField] float spawn_offset = 20;
    [SerializeField] GameObject powerUpPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time_counter += Time.deltaTime;
        if (time_counter > spawn_frequence)
        {
            CreatePowerUp();
            time_counter = 0;
        }
    }

    private void CreatePowerUp()
    {
        Vector3 pos = new Vector3(spawn_offset, 0, 0);
        pos.x += player.transform.position.x;
        GameObject.Instantiate(powerUpPrefab, pos, Quaternion.identity);
    }
}