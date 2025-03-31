using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTutorial : MonoBehaviour
{
    public static HandTutorial Instance;
    public Transform handObj;
    public Transform posA, posB;
    public float speed = 10;
    public float smooth = 0.1f;

    void Start()
    {
        Instance = this;
        StartCoroutine(MoveHand());
    }

    public void StopAll()
    {
        StopAllCoroutines();
        handObj.gameObject.SetActive(false);
    }

    IEnumerator MoveHand()
    {
        handObj.position = posA.position;
        while (true)
        {
            //float alpha = 0;
            while (Vector2.Distance(handObj.position, posB.position) > 0.2f)
            {
                handObj.position = Vector3.Lerp(handObj.position, posB.position, speed * smooth * Time.deltaTime);
                //alpha += speed * Time.deltaTime;
                //alpha = Mathf.Clamp01(alpha);
                //handObj.position = Vector3.MoveTowards(handObj.position, posB.position, alpha);
                yield return null;
            }

            while (Vector2.Distance(handObj.position, posA.position) > 0.2f)
            {
                handObj.position = Vector3.Lerp(handObj.position, posA.position, speed * smooth * Time.deltaTime * 3);
                yield return null;
            }
        }
    }
}
