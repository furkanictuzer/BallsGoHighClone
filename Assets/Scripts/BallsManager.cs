using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using Object = UnityEngine.Object;

public class BallsManager : MonoSingleton<BallsManager>
{
    public List<GameObject> balls = new List<GameObject>();
    
    public GameObject ballPrefab;
    
    public Transform ballsParent;

    public int maxSortingLayer = 4;

    [Space] [Range(0, 300)] 
    public int ballCount;

    private bool _isDecrease;
    

    private void OnValidate()
    {
        if (ballCount>balls.Count && !_isDecrease)
        {
            CreateBalls();
        }
        else if (balls.Count > ballCount)
        {
            _isDecrease = true;
            
            DestroyBalls();
        }
        else
        {
            SortV();
        }
    }

    private void CreateBalls()
    {
        for (var var = balls.Count; var < ballCount; var++)
        {
            var go = Instantiate(ballPrefab, ballsParent);
            
            balls.Add(go);

            go.transform.localPosition = Vector3.zero;
        }
        SortV();
    }

    private void DestroyBalls()
    {
        for (var i = balls.Count-1; i >= ballCount; i--)
        {
            var go = balls[balls.Count - 1];
            
            balls.RemoveAt(i);

            EditorCoroutineUtility.StartCoroutine(DestroyObject(go), this);
        }

        _isDecrease = false;
        SortV();
    }

    /*private void MoveObjects(Transform objectTransform, float degree)
    {
        var pos = Vector3.zero;
        
        pos.x = Mathf.Cos(degree * Mathf.Deg2Rad);
        pos.z = Mathf.Sin(degree * Mathf.Deg2Rad);

        objectTransform.localPosition = pos * distance;
    }*/
    private void MoveObjectsV(Transform objectTransform, float degree , float dist)
    {
        var pos = Vector3.zero;
        
        pos.x = Mathf.Cos(degree * Mathf.Deg2Rad);
        pos.z = Mathf.Sin(degree * Mathf.Deg2Rad);

        objectTransform.localPosition = pos * dist;
    }

    /*private void Sort()
    {
        float ballsCount = balls.Count;

        var angle = 360 / ballsCount;

        for (var i = 0; i < ballsCount; i++)
            MoveObjects(balls[i].transform, i * angle);
    }*/

    private void SortV()
    {
        int ballsCount = balls.Count;

        var num = 0;

        for (var i = 0; i < maxSortingLayer; i++)
        {
            if (ballsCount == num)
                break;

            var innerCount = i * 6 == 0 ? 1 : i * 6;
            var angle = 360 / innerCount;
            
            //Debug.Log(angle);
            
            for (var j = 0; j < innerCount; j++)
            {
                if (ballsCount == num)
                    break;
                
                MoveObjectsV(balls[num].transform, j * angle,i);
                num++;
            }
        }
    }

    private new static IEnumerator DestroyObject(Object go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }

    public void AddBalls(int addingNum)
    {
        ballCount += addingNum;

        for (var i = 0; i < addingNum; i++)
        {
            var go = Instantiate(ballPrefab, ballsParent);
            
            balls.Add(go);

            go.transform.localPosition = Vector3.zero;
        }
        
        SortV();
    }
    
    public void RemoveBalls(int removingNum)
    {
        //ballCount -= removingNum;

        for (var i = ballCount-1; i >= ballCount-removingNum; i--)
        {
            var go = balls[balls.Count - 1];
            
            balls.RemoveAt(i);

            EditorCoroutineUtility.StartCoroutine(DestroyObject(go), this);
        }

        ballCount = balls.Count;
        SortV();
    }
}
