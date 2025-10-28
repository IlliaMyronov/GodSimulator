using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private GameObject charPrefab;
    [SerializeField] private WorldManager worldManager;

    private void Awake()
    {
        this.transform.position = new Vector3(WorldSettingsRuntime.Instance.Settings.worldSize.x / 2, WorldSettingsRuntime.Instance.Settings.worldSize.y / 2, this.transform.position.z);
    }
    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 myPos = this.GetComponent<Transform>().position;

        this.GetComponent<Transform>().position = new Vector3(myPos.x + (Time.deltaTime * speed * moveX * (this.GetComponent<Camera>().orthographicSize / 25)), myPos.y + (Time.deltaTime * speed * moveY * (this.GetComponent<Camera>().orthographicSize / 25)), myPos.z);

        if (Input.GetKey(KeyCode.Q))
        {
            this.GetComponent<Camera>().orthographicSize += Time.deltaTime * zoomSpeed;
        }

        if (Input.GetKey(KeyCode.E))
        {
            this.GetComponent<Camera>().orthographicSize -= Time.deltaTime * zoomSpeed;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = this.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector3Int position = new Vector3Int(Mathf.FloorToInt(mousePos.x), Mathf.CeilToInt(mousePos.y), 0);

            GameObject newChar = Instantiate(charPrefab, position, Quaternion.identity) as GameObject;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                this.GetComponent<Camera>().orthographicSize -= zoomSpeed / 2;
            }
            else
            {
                this.GetComponent<Camera>().orthographicSize += zoomSpeed / 2;
            }
            
        }
    }
}
