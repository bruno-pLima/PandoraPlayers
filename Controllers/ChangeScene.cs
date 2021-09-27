using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameController.Singleton.LoadScene("4_Boss");
    }
}
