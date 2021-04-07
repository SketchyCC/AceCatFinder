using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catmovement : MonoBehaviour
{

    public GameObject Footprint;
    public float Footprinttime;
    float LastFootprinttime=0f;

    public AudioSource CatSounds; //cat sound to play
    public AudioClip[] CatSoundPool; //all sounds needed for the cat
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("PlayCatSound");
    }

    // Update is called once per frame
    void Update()
    {
        if (LastFootprinttime >= Footprinttime)
        {
            SpawnDecal(Footprint);
            LastFootprinttime=0;
        }
        LastFootprinttime += Time.deltaTime;
    }

    private void SpawnDecal(GameObject Decalprefab)
    {
        Vector3 from = this.transform.position;
        Vector3 to = new Vector3(this.transform.position.x, this.transform.position.y - (this.transform.localScale.y / 2.0f) + 0.4f, this.transform.position.z);
        Vector3 direction = to - from;

        RaycastHit hit;
        if(Physics.Raycast(from, direction, out hit) == true)
        {

            Quaternion rotation = transform.rotation;
            ObjectPooler.Instance.SpawnFromPool("Footprint", hit.point, rotation);
            //GameObject decal = Instantiate(Decalprefab);
            //decal.transform.position = hit.point;
            //decal.transform.Rotate(Vector3.up);
        }
    }

    IEnumerator PlayCatSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10, 15));
            int amount = CatSoundPool.Length;
            CatSounds.clip = CatSoundPool[Random.Range(0, amount - 1)];
            //CatSounds.clip = CatSoundPool[0];
            CatSounds.Play();
        }

    }
}
