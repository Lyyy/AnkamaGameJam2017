using System.Collections;
using UnityEngine;

public class Question6 : PlatformGame
{
    public GameObject response1;
    public GameObject response2;
    public GameObject response3;

    protected override void Update()
    {
        base.Update();
        if (response1 == null && response2 == null && response3 == null)
            StartCoroutine(Finish());
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(5f);
        Answer("Hidden");
    }

    public override IEnumerator Reload()
    {
        yield break;
    }
}