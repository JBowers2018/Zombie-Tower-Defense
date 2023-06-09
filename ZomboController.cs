using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomboController : MonoBehaviour
{
    private GameObject manager, terrain;
    private GameObject[] zombos;
    private List<GameObject> path;
    private int i, health;
    private float speed;
    private bool setup, died;
    private Material original, original2, original3;
    private Material[] redMaterials, originalMaterials;
    private AudioClip normalAudio, hitAudio;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Game Manager");
        i = 0;
        transform.position = Vector3.zero;
        setup = false; died = false;
        terrain = GameObject.Find("Terrain");

        Physics.IgnoreCollision(GetComponent<BoxCollider>(), terrain.GetComponent<TerrainCollider>());

        switch (name)
        {
            case "zombie(Clone)":
                {
                    health = 100; 

                    original = Resources.Load<GameObject>("Prefabs/zombie").transform.GetChild(1).
                        GetComponent<SkinnedMeshRenderer>().sharedMaterial;

                    normalAudio = Resources.Load<AudioClip>("Audio/Zombie Normal");
                    hitAudio = Resources.Load<AudioClip>("Audio/Zombie Hit");
                }
                break;
            case "zombiegirl_w_kurniawan(Clone)":
                {
                    health = 50; 
                    
                    original = Resources.Load<GameObject>("Prefabs/zombiegirl_w_kurniawan").transform.GetChild(1).
                        GetComponent<SkinnedMeshRenderer>().sharedMaterial; 
                    original2 = Resources.Load<GameObject>("Prefabs/zombiegirl_w_kurniawan").transform.GetChild(2).
                        GetComponent<SkinnedMeshRenderer>().sharedMaterial; 
                    original3 = Resources.Load<GameObject>("Prefabs/zombiegirl_w_kurniawan").transform.GetChild(3).
                        GetComponent<SkinnedMeshRenderer>().sharedMaterial;

                    normalAudio = Resources.Load<AudioClip>("Audio/Zombie Girl Normal");
                    hitAudio = Resources.Load<AudioClip>("Audio/Zombie Girl Hit");
                }
                break;
            case "maynard(Clone)":
                {
                    health = 200;

                    redMaterials = new Material[2] 
                    { Resources.Load<Material>("Materials/Red"), Resources.Load<Material>("Materials/Red") };

                    originalMaterials = new Material[2]
                    { 
                            Resources.Load<GameObject>("Prefabs/maynard").transform.GetChild(0).GetComponent
                            <SkinnedMeshRenderer>().sharedMaterials[0],
                            Resources.Load<GameObject>("Prefabs/maynard").transform.GetChild(0).GetComponent
                            <SkinnedMeshRenderer>().sharedMaterials[1]
                    };

                    normalAudio = Resources.Load<AudioClip>("Audio/Maynard Normal");
                    hitAudio = Resources.Load<AudioClip>("Audio/Maynard Hit");
                } 
                break;
            default: break;
        }

        GetComponent<AudioSource>().clip = normalAudio;

        StartCoroutine("MakeNoises");
    }

    // Update is called once per frame
    void Update()
    {
        zombos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject zombo in zombos)
            Physics.IgnoreCollision(GetComponent<BoxCollider>(), zombo.GetComponent<BoxCollider>());

        if (!setup && manager.GetComponent<PathGenerator>().GetFinished())
        {
            path = manager.GetComponent<PathGenerator>().GetPath();
            transform.position = path[0].transform.position;
            setup = true;
        }
            

        if(path.Count > 0 && i < path.Count - 1)
        {
            transform.position += (path[i + 1].transform.position - transform.position).normalized
                * speed * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(path[i + 1].transform.position -
               path[i].transform.position).normalized;

            if(Vector3.Distance(transform.position, path[i + 1].transform.position) < 0.1f)
                i++;
        }

        if (i >= path.Count - 1)
        {
            manager.GetComponent<Instantiate>().Ouch(name);
            Destroy(gameObject);
        }

        if (health <= 0 && !died)
        {
            died = true;
            Die();
        }
    }

    public void Ouch(string name)
    { 
        switch(name)
        {
            case "Explosion(Clone)": health -= 10; break;
            case "Spike(Clone)": health -= 5; break;
            case "Turret Bullet(Clone)": health -= 1; break;
            default: break;
        }

        StartCoroutine("Damage"); 
    }

    IEnumerator Damage()
    {
        GetComponent<AudioSource>().clip = hitAudio;

        switch (name)
        {
            case "zombie(Clone)":
                {
                    transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = 
                        Resources.Load<Material>("Materials/Red");

                    GetComponent<AudioSource>().Play();

                    yield return new WaitForSeconds(0.5f);

                    transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = original;
                }
                break;
            case "zombiegirl_w_kurniawan(Clone)":
                {
                    transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material =
                        Resources.Load<Material>("Materials/Red");
                    transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material =
                        Resources.Load<Material>("Materials/Red");
                    transform.GetChild(3).GetComponent<SkinnedMeshRenderer>().material =
                        Resources.Load<Material>("Materials/Red");

                    GetComponent<AudioSource>().Play();

                    yield return new WaitForSeconds(0.5f);

                    transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = original;
                    transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = original2;
                    transform.GetChild(3).GetComponent<SkinnedMeshRenderer>().material = original3;
                }
                break;

            case "maynard(Clone)":
                {
                    transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials = redMaterials;

                    GetComponent<AudioSource>().Play();

                    yield return new WaitForSeconds(0.5f);

                    transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials = originalMaterials;
                }
                break;

            default: break;
        }

        GetComponent<AudioSource>().clip = hitAudio;
    }

    public void Die()
    {
        int money = 0;

        switch (name)
        {
            case "zombie(Clone)": money = 30; break;
            case "zombiegirl_w_kurniawan(Clone)": money = 10; break;
            case "maynard(Clone)": money = 50; break;
            default: break;
        }
        manager.GetComponent<Instantiate>().AddMoney(money);
        Destroy(gameObject);
    }


    public void SetSpeed(float s)
    { 
        speed = s;

        switch(name)
        {
            case "zombiegirl_w_kurniawan(Clone)": speed += 0.5f; break;
            case "maynard(Clone)": speed -= 0.5f; break;
            default: break;
        }
    }


    IEnumerator MakeNoises()
    {
        while(true)
        {
            if(GetComponent<AudioSource>().clip == normalAudio)
                GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(Random.Range(1, 6));
        }
    }
}
