using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
    public class LoadingEffect : CommMonoScriptObject
    {
        public float DefaultAlpha;
        public float AlphaSubSize;
        public float DefaultAngle;
        public float AngleAddSize;

        public float RunAngle;

        public Image image;

        public void Awake()
        {
            image = GetComponent<Image>();
        }

        public void Update()
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + AngleAddSize);
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (AlphaSubSize / 255f));

            if (image.color.a == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
