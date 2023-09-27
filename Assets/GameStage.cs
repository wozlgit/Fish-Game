using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    [SerializeField] double fakePowerupSpawnFreq = 0.2;
    [SerializeField] double plantSharkSpawnFreq = 0.05;
    [SerializeField] double plantationSpawnFreq = 0.3;
    [SerializeField] double plantationSpawnMoreChanceMult = 0.2;
    float time_counter = 0;
    [SerializeField] float cycleTime = 0.02f;
    float sharkTimer = 0;
    [SerializeField] GameObject player;
    [SerializeField] float powerupSpawnOffset = 20;
    [SerializeField] float sharkSpawnOffset = 60;
    [SerializeField] GameObject powerUpPrefab;
    [SerializeField] GameObject sharkPrefab;
    [SerializeField] GameObject plantPrefab;
    [SerializeField] float gameAreaHeight = 10;
    void Start()
    {
        lastPlayerPosX = Mathf.FloorToInt(player.transform.position.x);
        GenerateTerrain(-12, 12);
    }
    void Update()
    {
        time_counter += Time.deltaTime;
        sharkTimer += Time.deltaTime;
        if (time_counter > cycleTime)
        {
            CreatePowerUp();
            time_counter = 0;
        }
        else if (sharkTimer > 2) {
            sharkTimer = 0;
            int sharkCount = Mathf.FloorToInt(time_counter / 2);
            for (int i = 0; i < sharkCount; i++) {
                CreateShark();
            }
        }
    }

    private void CreatePowerUp()
    {
        Vector3 pos = new Vector3(powerupSpawnOffset, 0, 0);
        pos.x += player.transform.position.x;
        pos.y = Random.Range(-gameAreaHeight, gameAreaHeight);
        GameObject powerUp = GameObject.Instantiate(powerUpPrefab, pos, Quaternion.identity);
        if (Random.value < fakePowerupSpawnFreq) {
            Destroy(powerUp.GetComponent<PowerUp>());
            FakePowerUp fakePowerUp = powerUp.AddComponent<FakePowerUp>();
        }
    }
    private void CreateShark()
    {
        Vector3 pos = new Vector3(-sharkSpawnOffset, 0, 0);
        pos.x += player.transform.position.x;
        pos.y = Random.Range(-gameAreaHeight, gameAreaHeight);
        Instantiate(sharkPrefab, pos, Quaternion.identity);
    }
    int lastPlayerPosX;
    public void PlayerMoved(Vector2 velocity) {
        int cx = Mathf.FloorToInt(player.transform.position.x);
        int difX = cx - lastPlayerPosX;
        if (difX > 0)
        {
            GenerateTerrain(lastPlayerPosX + 1, lastPlayerPosX + difX);
            lastPlayerPosX = cx;
        }
    }

    private void GenerateTerrain(int beginX, int endX)
    {
        for (int x = beginX; x <= endX; x++)
        {
            int plantationLevel = 0;
            for (int i = 0; i < 4; i++)
            {
                float r = Random.value;
                if (r < plantationSpawnFreq + i * plantationSpawnMoreChanceMult) plantationLevel++;
                else break;
            }
            for (int i = 0; i < plantationLevel; i++)
            {
                Vector3 pos = new(x, -gameAreaHeight + i, 0);
                GameObject plant = GameObject.Instantiate(plantPrefab, pos, Quaternion.identity);
                if (Random.value < plantSharkSpawnFreq) {
                    GameObject shark = Instantiate(sharkPrefab, pos, Quaternion.identity);
                    Shark sharkScript = shark.GetComponent<Shark>();
                    sharkScript.state = Shark.MentalState.SLEEPING;
                    sharkScript.tiredness = sharkScript.tirednessLimit;

                    plant.GetComponent<Plant>().isClaimed = true;
                    plant.GetComponent<Plant>().claimedBy = shark;
                }
            }
        }
    }
}