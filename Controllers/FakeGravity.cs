using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGravity : MonoBehaviour {


    private Rigidbody rb;


    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }


    private void Update() {
        FakeGrav();
    }





    private void FakeGrav() {
        rb.velocity += Vector3.down;
    }


}
