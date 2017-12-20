using System.Collections;

public class Question8 : PlatformGame {

    public override IEnumerator Reload()
    {
        if(GameState.GetInstance().CurrentResponse.text.Contains("siècles"))
            yield break;

        yield return base.Reload();
    }
}
