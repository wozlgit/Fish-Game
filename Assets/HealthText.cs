using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    TMP_Text textCmp;
    private GameObject player;
    void Start()
    {
        textCmp = GetComponent<TMP_Text>();
        player = GameObject.Find("Player");
    }
    void Update()
    {
        Player p = player.GetComponent<Player>();
        textCmp.text = "Health " + p.health + "/" + p.maxHealth;
    }
}
