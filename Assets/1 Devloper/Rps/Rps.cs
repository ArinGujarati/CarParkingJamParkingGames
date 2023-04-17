using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rps : MonoBehaviour
{

    public GameObject[] Target;
    GameObject temp;
    public List<GameObject> TempList = new List<GameObject>();
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f))
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                int random = Random.Range(0, Target.Length);
                temp = Instantiate(Target[random],hit.point,Quaternion.identity);
                TempList.Add(temp);
                temp.transform.GetComponent<MeshRenderer>().material.DOFade(0.1f, .0f);
                for (int i = 0; i < TempList.Count; i++)
                {
                    if (i < TempList.Count - 1)
                    {
                        TempList[i].GetComponent<MeshRenderer>().material.DOFade(1f, .5f);
                    }
                }
                temp.transform.DOShakeScale(.1f, .5f).OnComplete(() =>
                    {
                        temp.transform.DOShakeScale(.1f, .5f);
                    });

            }
            if (Input.GetMouseButton(0))
            {
                if (temp.transform.localScale.x <= 15)
                    temp.transform.DOBlendableScaleBy(new Vector3(.1f, .1f, .1f), .5f);

            }
            if (Input.GetMouseButton(1))
            {
                if (temp.transform.localScale.x >= 5f)
                    temp.transform.DOBlendableScaleBy(new Vector3(-.1f, -.1f, -.1f), .5f);
            }
            temp.transform.position = hit.point;

        }

    }
}
