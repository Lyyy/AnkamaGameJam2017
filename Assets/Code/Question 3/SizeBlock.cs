using UnityEngine;

public class SizeBlock : AnswerBlock
{
    public float sizeFactor;

    void Update()
    {
        var scale = transform.localScale;
        scale.x = Mathf.Min(scale.x + Time.deltaTime * sizeFactor * (scale.x < 0.5f ? 1 / scale.x : 1f), 2f);
        scale.y = Mathf.Min(scale.y + Time.deltaTime * sizeFactor * (scale.x < 0.5f ? 1 / scale.x : 1f), 2f);
        if(scale.x < 0f)
            Destroy(gameObject);
        transform.localScale = scale;
    }
}