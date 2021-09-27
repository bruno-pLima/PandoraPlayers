using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexZone : MonoBehaviour
{
    [SerializeField]
    private LayerMask l_Mask;

    private Collider[] l_collider;

    [SerializeField]
    private float Power = 1.5f;
    [SerializeField]
    private int Duration = 5;

    [Space(30)]

    [SerializeField] float Speed = 3;

    private PlayerController Player;

    Rigidbody rb;

    private Vector3 PlayT;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Move());
        StartCoroutine(Vortex());
        GetComponent<Collider>().enabled = false;
    }

    public void SetPlayer(PlayerController n_Player)
    {
        Player = n_Player;
        PlayT = Player.transform.forward;
    }


    IEnumerator Move()
    {
        while (true)
        {
            Collider[] _collider = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius * 2, l_Mask);
            Vector3 Dir = Vector3.zero;
            Vector3 Mov = Vector3.zero;

            if (_collider.Length > 0)
            {
                foreach (Collider item in _collider)
                {
                    Dir += item.gameObject.transform.position;
                }

                Dir /= _collider.Length;
                Mov = Vector3.Lerp(transform.position, Dir, Time.deltaTime);
                Mov.y = 1;
                transform.position = Mov;
            }
            else
            {
                //transform.Translate(PlayT * Time.deltaTime);
                transform.Translate(transform.forward * Time.deltaTime * 3);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Vortex()
    {
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        float TimeDuration = Time.time + Duration;

        while (TimeDuration >= Time.time)
        {
            l_collider = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius, l_Mask);

            foreach (Collider obj in l_collider)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();

                if (obj.gameObject.GetComponent<Boss>())
                {
                    Boss _b = obj.gameObject.GetComponent<Boss>();

                    if (_b.PhaseCount == 3)
                    {
                        Vector3 Direction = (transform.position - rb.transform.position).normalized;

                        float Distance = Vector3.Distance(transform.position, rb.transform.position);
                        Distance = Mathf.Pow(Distance, 1.5f);

                        float PowerForce = Power / Distance * 15;
                        PowerForce = Mathf.Clamp(PowerForce, 0.01f, Mathf.Pow(10, 3));
                        Direction.y = 0;

                        rb.AddForce(Direction * PowerForce, ForceMode.VelocityChange);
                    }

                }
                else if (obj.gameObject.GetComponent<SwordMan>() || obj.gameObject.GetComponent<BowMan>())
                {
                    Vector3 Direction = (transform.position - rb.transform.position).normalized;

                    float Distance = Vector3.Distance(transform.position, rb.transform.position);
                    Distance = Mathf.Pow(Distance, 1.5f);

                    float PowerForce = Power / Distance * 50;
                    PowerForce = Mathf.Clamp(PowerForce, 0.01f, Mathf.Pow(10, 3));
                    Direction.y = 0;

                    rb.AddForce(Direction * PowerForce, ForceMode.VelocityChange);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        l_collider = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius, l_Mask);

        foreach (Collider obj in l_collider)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }

        Player.GetComponent<Vortex>().CountCD();
        Destroy(gameObject);
        yield return null;
    }
}
