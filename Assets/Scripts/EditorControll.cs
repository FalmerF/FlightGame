using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class EditorControll : MonoBehaviour
{
    /*private void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 pos = transform.GetChild(i).transform.position;
                pos.x = Mathf.Round(pos.x / 5) * 5;
                pos.z = Mathf.Round(pos.z / 5) * 5;
                pos.y = Mathf.Round(pos.y / 5) * 5;
                //transform.GetChild(i).transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                transform.GetChild(i).transform.position = pos;
            }
        }
    }*/

    public GameObject[] rocks;
    public GameObject crystall;

    [ContextMenu("Spawn")]
    private void Spawn()
    {
        RaycastHit hit;
        for(int i = 0; i < 100; i++)
        {
            Vector3 pos;
            pos.x = Random.Range(-2950, 2950);
            pos.z = Random.Range(-2950, 2950);
            pos.y = 150;
            if (Physics.Raycast(pos, new Vector3(0, -1, 0), out hit))
            {
                if(hit.collider.gameObject.tag != "NotSpawn")
                {
                    GameObject obj = Instantiate(rocks[Random.Range(0, rocks.Length)], hit.point, Quaternion.identity);
                    float scaleObj = Random.Range(10.00f, 25.00f);
                    obj.transform.localScale = new Vector3(scaleObj, scaleObj, scaleObj);
                    obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - 3, obj.transform.position.z);
                    obj.transform.rotation = Quaternion.Euler(Random.Range(-7.00f, 7.00f), Random.Range(-360.00f, 360.00f), Random.Range(-7.00f, 7.00f));
                }
            }
        }
    }
    [ContextMenu("Add Crystall")]
    private void CrystallSpawn()
    {
        RaycastHit hit;
        for (int i = 0; i < 100; i++)
        {
            Vector3 pos;
            pos.x = Random.Range(-2950, 2950);
            pos.z = Random.Range(-2950, 2950);
            pos.y = 150;
            if (Physics.Raycast(pos, new Vector3(0, -1, 0), out hit))
            {
                if (hit.collider.gameObject.tag == "NotSpawn")
                {
                    GameObject obj = Instantiate(crystall, hit.point, Quaternion.Euler(-151.81f, 0, 51.45f));
                    break;
                }
            }
        }
    }

    private void Update()
    {
        
    }
}
