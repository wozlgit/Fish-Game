using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float movement_speed = 1.0f;
    [SerializeField] float angleStep = 15.0f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(-angleStep * Time.deltaTime * Vector3.forward);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(angleStep * Time.deltaTime * Vector3.forward);
        }
        if (transform.eulerAngles.z < 225)
        {
            transform.eulerAngles = new Vector3(0, 0, 225);
        }
        else if (transform.eulerAngles.z > 315)
        {
            transform.eulerAngles = new Vector3(0, 0, 315);
        }
        transform.position += movement_speed * Time.deltaTime * transform.up;
    }
    void OnTriggerEnter2D(Collider2D collider) {
        movement_speed *= 2;
        Destroy(collider.gameObject);
    }
}