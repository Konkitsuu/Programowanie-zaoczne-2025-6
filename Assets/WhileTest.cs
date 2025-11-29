using System.Collections;
using UnityEngine;

public class WhileTest : MonoBehaviour
{
    [SerializeField]
    [Range(0,1f)]
    private float testLerpT;
    [SerializeField] private new Light light;
    [SerializeField] private float blinkTime = 0.1f;
    [SerializeField]
    private float moveSpeed = 1;
    private float moveTime = 2;
    [SerializeField] Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MoveToTarget());
        //int i = 0;
        //while (i <= 5)
        //{
        //    print(i);
        //    i++;
        //    //Do something
        //}
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 startPosition = transform.position;
        while(testLerpT < 1)
        {
            testLerpT += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp
                (startPosition, target.position, testLerpT);
            //transform.position = Vector3.MoveTowards
            //    (transform.position, targetPostion.position, moveSpeed * Time.deltaTime);
            //Vector3 toTarget = targetPostion.position - transform.position;
            //transform.position += toTarget.normalized * moveSpeed * Time.deltaTime;
            yield return null; // To czeka 1 klatkê
        }
        transform.position = target.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, target.position);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Vector3.Lerp(transform.position, target.position, testLerpT), 0.3f);
    }

    private IEnumerator Jump()
    {
        while(transform.position.y < 2)
        {
            transform.position += Vector3.up * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (transform.position.y> 0)
        {
            transform.position += Vector3.down * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator BlinkLight()
    {
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                light.enabled = false;
                yield return new WaitForSeconds(blinkTime);
                light.enabled = true;
                yield return new WaitForSeconds(blinkTime);
            }

            yield return new WaitForSeconds(3);
        }

    }

    private IEnumerator RepeatEverySecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            print(Time.time);
        }
    }
}
