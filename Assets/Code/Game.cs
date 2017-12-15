﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static Game instance;

    public static Game GetInstance()
    {
        return instance;
    }

    protected virtual void Awake()
    {
        instance = this;
    }

    public IEnumerator Reload()
    {
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}