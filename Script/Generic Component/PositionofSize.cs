using UnityEngine;

namespace NagaisoraFamework
{
    public class PositionofSize : CommMonoScriptObject
    {
        public SpriteRenderer SpriteRenderer;

        public void OnEnable()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            if (SpriteRenderer == null)
            {
                return;
            }

            transform.localPosition = new(transform.localPosition.x, (SpriteRenderer.size.y / 2));
		}
    }
}
