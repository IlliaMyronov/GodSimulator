using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void OnClickLoadScene()
    {
        SceneController.Instance.LoadScene(sceneName);
    }
}
