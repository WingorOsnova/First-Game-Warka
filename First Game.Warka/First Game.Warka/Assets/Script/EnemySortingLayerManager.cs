using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySortingLayerManager : MonoBehaviour
{
    public static EnemySortingLayerManager Instance;
    List<SpriteRenderer> enemyesSpr = new List<SpriteRenderer>();

    public void Add(SpriteRenderer spp) { enemyesSpr.Add(spp);  }
    public void Dell(SpriteRenderer spp) { enemyesSpr.Remove(spp); }

    float[] posYs;
    SpriteRenderer[] spritesRends;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StartCoroutine(nameof(Check));
    }

    IEnumerator Check()
    { 
        yield return new WaitForSeconds (1);
        int n = enemyesSpr.Count;

        posYs = new float[n];
        spritesRends = new SpriteRenderer[n];

        for (int i = 0; i < n; i++)
        {
            posYs[i]
                = enemyesSpr[i].transform.position.y;
            spritesRends[i] = enemyesSpr[i];
        }
        Array.Sort(posYs, spritesRends);

        for (int i = 0; i < spritesRends.Length; i++)
        {
            spritesRends[i].sortingOrder = -i;

        }   

        StartCoroutine(nameof(Check));
    }
}
