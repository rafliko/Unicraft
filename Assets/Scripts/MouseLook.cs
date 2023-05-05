using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 200f;

    public Transform playerBody;

    float xRotation = 0f;

    RaycastHit hit;

    public LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(transform.position, transform.forward, out hit, 5, groundMask))
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);

                hit.point = hit.point + transform.forward * 0.01f;

                var chunk = hit.transform.gameObject.GetComponent<TerrainChunk>();
                int x = (int)Mathf.Floor(hit.point.x) - (int)chunk.offsetX;
                int y = (int)Mathf.Floor(hit.point.y);
                int z = (int)Mathf.Floor(hit.point.z) - (int)chunk.offsetZ;
                Debug.Log(x+","+y+","+z);
                chunk.blocks[x, y, z] = BlockType.Air;
                chunk.UpdateChunk();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5, groundMask))
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);

                hit.point = hit.point - transform.forward * 0.01f;

                var chunk = hit.transform.gameObject.GetComponent<TerrainChunk>();
                int x = (int)Mathf.Floor(hit.point.x) - (int)chunk.offsetX;
                int y = (int)Mathf.Floor(hit.point.y);
                int z = (int)Mathf.Floor(hit.point.z) - (int)chunk.offsetZ;
                Debug.Log(x + "," + y + "," + z);
                chunk.blocks[x, y, z] = BlockType.Grass;
                chunk.UpdateChunk();
            }
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
