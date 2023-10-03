using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    TMP_Text textCmp;
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        textCmp = GetComponent<TMP_Text>();
    }
    void Update()
    {
        int points = Mathf.FloorToInt(player.transform.position.x / 10) * 5;
        textCmp.text = points + " Points";
    }
}
