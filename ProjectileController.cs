using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public void Start()
    {
        if (name == "Explosion(Clone)")
            StartCoroutine("Explode");
        else if (name != "Bomb(Clone)")
            StartCoroutine("Destroy");
            
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
            other.GetComponent<ZomboController>().Ouch(name);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Terrain")
        {
            StartCoroutine("Countdown");
        }
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1);

        float flashTime = Time.time + 1f;
        while(Time.time <= flashTime)
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Red");
            yield return new WaitForSeconds(0.1f);
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Bomb Blue");
            yield return new WaitForSeconds(0.1f);
        }

        GameObject explosion = Resources.Load<GameObject>("Prefabs/Explosion"), exClone;
        exClone = Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);     
    }

    IEnumerator Explode()
    {
        GetComponent<AudioSource>().Play();
        float explosionTime = Time.time + 2f;

        while (Time.time < explosionTime)
        {
            transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            yield return new WaitForSeconds(0.01f);
        }
            
        Destroy(gameObject);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
