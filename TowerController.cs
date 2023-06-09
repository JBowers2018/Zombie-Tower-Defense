using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private List<GameObject> enemies;
    private GameObject target;
    private int i, level;
    private float time;

    public void Start()
    {
        enemies = new List<GameObject>();
        i = 0; time = 5f;
        target = null;

        StartCoroutine("Begin");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")  
            enemies.Add(other.gameObject);
    }

    public void OnTriggerExit(Collider other)
    {
        enemies.Remove(other.gameObject);

        if (target == other.gameObject)
            target = null;
    }

    IEnumerator Begin()
    {
        while(true)
        {
            if (enemies.Count > 0)
            {
                i = Random.Range(0, enemies.Count);
                target = enemies[i];

                StartCoroutine("Shoot", target);
                yield return new WaitForSeconds(5);
                StopCoroutine("Shoot");

                if (!target)
                    enemies.Remove(target);
                    

                time += 5f;
            }
            else
                yield return null;
        }
    }

    IEnumerator Shoot(GameObject theGuy)
    {
        while(true)
        {
            if (!theGuy)
                yield break;

            if (name == "Red Tower(Clone)")
            {
                GameObject bomb = Resources.Load<GameObject>("Prefabs/Bomb"), bClone;

                for (int l = 0; l < level; l++)
                {
                    if (!theGuy)
                        yield break;

                    bClone = Instantiate(bomb, transform.position + Vector3.up, bomb.transform.rotation);
                    foreach(GameObject zombo in GameObject.FindGameObjectsWithTag("Enemy"))
                        Physics.IgnoreCollision(bClone.GetComponent<SphereCollider>(), zombo.GetComponent<BoxCollider>());
                    bClone.GetComponent<Rigidbody>().AddForce((theGuy.transform.position -
                        transform.position).normalized * 4f, ForceMode.Impulse);

                    if (level > 1) yield return new WaitForSeconds(0.2f);
                }

               yield return new WaitForSeconds(2.5f);
            }

            if (name == "Blue Tower(Clone)")
            {
                int direction = 0, j = -1;
                Vector3 offset = Vector3.zero, pointRotation = Vector3.zero;

                GameObject spike = Resources.Load<GameObject>("Prefabs/Spike");
                GameObject[] spikes = new GameObject[3];

                for (int l = 0; l < level; l++)
                {
                    if (!theGuy)
                        yield break;

                    if (theGuy.transform.position.z > transform.position.z)
                        direction = 1;
                    else
                        direction = -1;

                    for (int i = 0; i < spikes.Length; i++)
                    {
                        if (i == 0)
                            offset = Vector3.left / 50;
                        if (i == 1)
                            offset = Vector3.zero / 50;
                        if (i == 2)
                            offset = Vector3.right / 50;

                        pointRotation = Quaternion.LookRotation((theGuy.transform.position -
                            transform.position).normalized).eulerAngles + new Vector3(0, 15 * direction * j, 0);

                        spikes[i] = Instantiate(spike, transform.position + Vector3.up + offset * 5f,
                        Quaternion.Euler(pointRotation));

                        spikes[i].GetComponent<Rigidbody>().velocity = spikes[i].transform.forward * 5f;
                        GetComponent<AudioSource>().Play();

                        j++;
                    }

                    if (level > 1) yield return new WaitForSeconds(0.2f);
                }

                yield return new WaitForSeconds(1.5f);
            }

            if (name == "Green Tower(Clone)")
            {
                GameObject bullet = Resources.Load<GameObject>("Prefabs/Turret Bullet"), bClone;

                for (int l = 0; l < level; l++)
                {
                    if (!theGuy)
                        yield break;

                    bClone = Instantiate(bullet, transform.position + Vector3.up,
                        Quaternion.LookRotation((theGuy.transform.position - transform.position).normalized));
                    bClone.GetComponent<Rigidbody>().velocity = bClone.transform.forward * 5f;
                    bClone.transform.Rotate(90, 0, 0);
                    GetComponent<AudioSource>().Play();

                    if (level > 1) yield return new WaitForSeconds(0.2f);
                }

                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
        
    }

    public void SetLevel(int l)
    { level = l; }

    public int GetLevel()
    { return level; }
}
