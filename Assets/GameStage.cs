using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStage : MonoBehaviour
{
    [SerializeField] int maxPlantationLevel;
    [SerializeField] double plantationSpawnFreq;
    [SerializeField] double plantationSpawnMoreChanceMult;
    // For spawning plants next to obstacles
    [SerializeField] float plantationSpawnChance;
    [SerializeField] double plantSharkSpawnFreq;
    [SerializeField] double fakePowerupSpawnFreq;
    [SerializeField] int whaleSpawnCooldown;
    [SerializeField] int sharkSpawnCooldown;
    [SerializeField] float cycleTime;
    [SerializeField] float powerupSpawnOffset;
    [SerializeField] float sharkSpawnOffset;

    [SerializeField] GameObject player;
    [SerializeField] GameObject powerUpPrefab;
    [SerializeField] GameObject sharkPrefab;
    [SerializeField] GameObject plantPrefab;
    [SerializeField] GameObject whalePrefab;
    [SerializeField] GameObject staticObstaclePrefab;
    [SerializeField] GameObject whaleNestPrefab;

    public const int gameAreaHeight = 10;
    public const int gameAreaWidth = 18;
    float time_counter = 0;
    float sharkTimer = 0;
    public static GameStage Get() {
        return GameObject.Find("Stage").GetComponent<GameStage>();
    }
    void Start()
    {
        lastPlayerPosX = Mathf.FloorToInt(player.transform.position.x);
        whaleCountdown = whaleSpawnCooldown;
        sharkSpawnOffset = gameAreaWidth;
        GenerateTerrain(-gameAreaWidth, gameAreaWidth);
    }
    void Update()
    {
        sharkTimer += Time.deltaTime;
        time_counter += Time.deltaTime;
        if (time_counter > cycleTime)
        {
            CreatePowerUp();
            time_counter = 0;
        }
        else if (sharkTimer > sharkSpawnCooldown) {
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
            powerUp.AddComponent<FakePowerUp>();
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
    int whaleCountdown;
    public void PlayerMoved(Vector2 velocity) {
        int cx = Mathf.FloorToInt(player.transform.position.x);
        int difX = cx - lastPlayerPosX;
        if (difX > 0)
        {
            whaleCountdown -= difX;
            if (whaleCountdown <= 0)
            {
                whaleCountdown = whaleSpawnCooldown + whaleCountdown;
                Vector2 pos = new(cx + gameAreaWidth, Random.Range(-gameAreaHeight, gameAreaHeight));
                Instantiate(whalePrefab, pos, Quaternion.identity);
            }

            GenerateTerrain(lastPlayerPosX + 1 + gameAreaWidth, lastPlayerPosX + difX + gameAreaWidth);
            DestroyStaleTerrain(cx);
            lastPlayerPosX = cx;
        }
    }

    private void DestroyStaleTerrain(int currentPlayerX)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] plants = GameObject.FindGameObjectsWithTag("SharkSleepingPlace");
        GameObject[] whaleNests = GameObject.FindGameObjectsWithTag("WhaleNest");
        GameObject[] objects = new GameObject[obstacles.Length + plants.Length + whaleNests.Length];
        obstacles.CopyTo(objects, 0);
        plants.CopyTo(objects, obstacles.Length);
        whaleNests.CopyTo(objects, obstacles.Length + plants.Length);
        foreach (var obj in objects)
        {
            if (obj.transform.position.x <= currentPlayerX - gameAreaWidth * 2)
            {
                Destroy(obj);
            }
        }
    }

    private void GenerateTerrain(int beginX, int endX)
    {
        //GeneratePlantation(beginX, endX);
        GenerateObstacles(beginX, endX);
    }

    private void GeneratePlantation(int beginX, int endX)
    {
        for (int x = beginX; x <= endX; x++)
        {
            int plantationLevel = 0;
            for (int i = 0; i < maxPlantationLevel; i++)
            {
                float r = Random.value;
                if (r < plantationSpawnFreq + i * plantationSpawnMoreChanceMult) plantationLevel++;
                else break;
            }
            for (int i = 0; i < plantationLevel; i++)
            {
                Vector3 pos = new(x, -gameAreaHeight + i, 0);
                CreatePlant(pos);
            }
        }
    }

    private void CreatePlant(Vector3 pos)
    {
        GameObject plant = Instantiate(plantPrefab, pos, Quaternion.identity);
        if (Random.value < plantSharkSpawnFreq)
        {
            GameObject shark = Instantiate(sharkPrefab, pos, Quaternion.identity);
            Shark sharkScript = shark.GetComponent<Shark>();
            sharkScript.state = Shark.MentalState.SLEEPING;
            sharkScript.tiredness = sharkScript.tirednessLimit;

            plant.GetComponent<Plant>().isClaimed = true;
            plant.GetComponent<Plant>().claimedBy = shark;
        }
    }

    /*
    pillar generation:
    NOTE: Since the player can't change their orientation instantly, their angle
    in point A will affect the minimun x distance of each y of point B

    - calculate minimum x distance between two points given their y coordinates, where
    the player can go from point-to-point without hitting a wall
    - based on previous and settings calculate possible range of the point's x
    - based on above somehow calculate the relative x coordinates of two pillars, and
    the y coordinates of each of their holes
    - randomize the y-size of each hole based on settings and player collider y size
    - the width of each pillar could be constant for now atleast

    !1:
    - y distance = pointB.y - pointA.y
    - x distance = pointB.x - pointA.x
    - y speed of player = playerTrajectorySlopeFromOrientation()
    - y change in x distance = x distance * y speed of player
    - if y change in x distance >= y distance, good, else bad
    y change in x distance >= y distance
    x distance * y speed of player >= y distance
    x distance >= y distance / y speed of player
    x distance >= y distance / playerTrajectorySlopeFromOrientation()

    clarification: player's orientation is defined as the optimal orientation given
    the x and y distance

    optimal orientation: orientation where the trajectory is a straight line from point A to B,
    i.e. angle of dir vector (pointB - pointA).normalized

    whoops the actual problem is calculate minimum x distance between two points given their y coordinates,
    where if the player starts at orientation 0 at point A, they can reach point B without hitting any walls,
    if they rotate optimally every frame, i.e. rotate at maximum speed towards the optimal rotation at that frame

    optimal orientation to reach point B from A at frame x:
    Vec2 dif = point B - player position at current frame
    Vec2 dir = dif.normalized

    a given x distance is sufficient if the sums of y changes of each frame spent moving from point A to B
    is >= distanceY

    f(t) = 
    current orientation: orientationFromOptimalAndAmountOfRotationAvailable(t * angleStep, optimalOrientation())
    y change: current orientation * t * movementSpeed
    where t is time spent moving so far

    is splitting movement into 2 frames (f(0.5 * timeToMove) + f(0.5 * timeToMove)) = f(timeToMove) ?
    yes, because at 2 frames the first frame's movement is applied first with that frame's orientation, and then
    the same with the second, whereas with only 1 frame, all the rotating is done first, meaning the orientation
    is closer to the optimal, and then the movement

    the rotation done during previous frames is always beneficial

    problem: current implementation calculates the movement of each frame
    */

    /*
    Hole generation constraints:
    - Each pillar must have atleast one hole which can be reached well from atleast one hole
    on the previous pillar
    ---I.e. the minimum distance between sequential pillars is 
    - Could implement this:
        - First pillar doesn't need any checks
        - Subsequent pillars first generate a hole that can be reached well from atleast one hole on
            the previous pillar
    - 
    */
    private int previousPillarX = 0;
    private float[] previousPillarHoles;
    void GenerateObstacles(int beginX, int endX) {
        if (beginX >= previousPillarX) {
            if (previousPillarX == 0) {
                previousPillarHoles = GeneratePillarHoles();
                CreatePillarObject(previousPillarX, previousPillarHoles);
            }
            while (previousPillarX <= endX) {
                (previousPillarX, previousPillarHoles) = GeneratePillar(previousPillarX, previousPillarHoles);
                CreatePillarObject(previousPillarX, previousPillarHoles);
            }
        }
    }
    (int posX, float[] holes) GeneratePillar(int previousPillarX, float[] previousPillarHoles)
    {
        float[] holes = GeneratePillarHoles();
        int distanceX = CalculateCompliantHole(previousPillarHoles, holes[0]);

        return (posX: previousPillarX + distanceX, holes);
    }

    [SerializeField] float minHoleDistance;
    [SerializeField] int maxHoleCount;
    [SerializeField] int minHoleCount;
    [SerializeField] float holeSize;
    const float pillarSizeY = gameAreaHeight * 2;
    private float[] GeneratePillarHoles()
    {      
        int holeCount = Random.Range(minHoleCount, maxHoleCount + 1);
        int regionCount = holeCount + 1;
        float pillarObstacleSpace = pillarSizeY - holeSize * holeCount;
        float regionSize = pillarObstacleSpace / regionCount;
      
        float[] holes = new float[holeCount];
        float holeDistance = holeSize + regionSize;
        holes[0] = regionSize;
        for (int i = 1; i < holeCount; i++) {
            holes[i] = holes[i - 1] + holeDistance;
        }

        // currentHole + holeSize + minHoleDistance <= nextHole
        // nextHole - currentHole >= holeSize + minHoleDistance
        float actualMinHoleDistance = holeSize + minHoleDistance;
        // maximum absolute change for both holes to be atleast minimum distance from each other
        float aHoleRange = (holeDistance - actualMinHoleDistance) / 2;
        holes[0] += Random.Range(-regionSize, aHoleRange);
        for (int i = 1; i < holeCount; i++) {
            holes[i] += Random.Range(-aHoleRange, aHoleRange);
        }
        return holes;
    }

    private int CalculateCompliantHole(float[] previousPillarHoles, float currentHoleY)
    {
        // randomly pick 1 hole from previousPillarHoles
        int index = Random.Range(0, previousPillarHoles.Length - 1);
        float prevHoleY = previousPillarHoles[index];
        // calculate minimum x
        Player playerScript = player.GetComponent<Player>();
        int minimumX = minimumXDistanceBetweenTwoPoints(currentHoleY - prevHoleY, 0.016f, playerScript.baseSpeed, playerScript.angleStep);
        // add error and player imperfection margins to minimum x
        minimumX = Mathf.FloorToInt(minimumX * 1.3f);
        int maxX = Mathf.FloorToInt(minimumX * 1.3f);
        // randomize x distance
        return Random.Range(minimumX, maxX);
    }

    void CreatePillarObject(int pillarX, float[] pillarHoles) {
        float y = 0;
        foreach (float holeY in pillarHoles) {
            CreateSubPillar(pillarX, y, holeY);
            y = holeY + holeSize;
        }
        CreateSubPillar(pillarX, y, pillarSizeY);
    }
    void CreateSubPillar(int x, float pillarStartY, float pillarEndY) {
        GameObject pillar = Instantiate(staticObstaclePrefab);
        float pillarSize = pillarEndY - pillarStartY;
        pillar.transform.position = new Vector3(x, pillarStartY + pillarSize / 2 - gameAreaHeight, 0);
        pillar.transform.localScale = new(1, pillarSize, 1);
        for (float y = pillarStartY; y <= pillarEndY; y++) {
            foreach (int offsetX in new int[] {-1, 1}) {
                Vector3 pos = pillar.transform.position;
                pos.x += offsetX;
                pos.y = y - gameAreaHeight;
                if (Random.value <= 0.02) {
                    Instantiate(whaleNestPrefab, pos, Quaternion.identity);
                }
                else if (Random.value <= plantationSpawnChance) {
                    CreatePlant(pos);
                }
            }
        }
    }
    int minimumXDistanceBetweenTwoPoints(float distanceY, float spf, float movementSpeed, float angleStep) {
        return 8;
        int minDifX = -1;
        Vector2 distance = new(0, distanceY);
        for (int x = 0; minDifX == -1; x++) {
            distance.x = x;
            if (validDistanceX(distance, spf, movementSpeed, angleStep)) {
                return x;
            }
        }
        return -1;
    }

    bool validDistanceX(Vector2 distance, float tpf, float movementSpeed, float angleStep) {
        int frame = 0;
        float accumulatedYChange = 0;
        Vector2 currentPos = Vector2.zero;
        while (accumulatedYChange < distance.y) {
            Vector2 optimalDir = (distance - currentPos).normalized;
            float deltaTime = tpf;
            Vector2 currentDir = directionFromOptimalAndAmountOfRotationAvailable(frame * tpf * angleStep, optimalDir);
            Vector2 movement = currentDir * deltaTime * movementSpeed;
            currentPos += movement;
            frame++;
        }
        return currentPos.x <= distance.x;
    }
    Vector2 directionFromOptimalAndAmountOfRotationAvailable(float rotationAvailable, Vector2 optimalDir) {
        /*
        Vector2 startDir = Vector2.right;
        float optimalOrientation = optimalDir.toEuler;
        float startOrientation = startDir.toEuler;
        
        float orientationDifference = optimalOrientation - startOrientation;
        float orientation;
        if (startOrientation < optimalOrientation) {
            orientation = Math.Min(startOrientation + rotationAvailable, optimalOrientation);
        }
        else {
            orientation = Math.Max(startOrientation - rotationAvailable, optimalOrientation);
        }

        return orientation.toVec2();
        */
        return Vector2.zero;
    }
}