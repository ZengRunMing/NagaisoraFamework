using UnityEngine;
using UnityEngine.UI;

using NagaisoraFamework;

public class RawImageUV : CommMonoScriptObject
{
    public float x;
    public float y;
    public float w;
    public float h;

    public RawImage RawImage;

	private void Start()
	{
        RawImage = this.GetComponent<RawImage>();
        w = RawImage.uvRect.width;
        h = RawImage.uvRect.height;
	}

	private void FixedUpdate()
    {
        RawImage.uvRect = new Rect(RawImage.uvRect.x + x, RawImage.uvRect.y + y, w, h);
    }
}
