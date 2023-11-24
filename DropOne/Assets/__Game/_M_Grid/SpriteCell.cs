

using Gamelogic.Extensions;
using Gamelogic.Extensions.Internal;
using Gamelogic.Grids;
using UnityEngine;

namespace Cihan
{

    public class SpriteCell : TileCell, IGLScriptableObject
    {

        public PartItem PartItem;
        [SerializeField] private Color color;

        [SerializeField]
        private Sprite[] sprites = new Sprite[0];

        [SerializeField] private int frameIndex;

        [SerializeField] private bool highlightOn;

        public bool HighlightOn
        {
            get { return highlightOn; }

            set
            {
                highlightOn = value;
                __UpdatePresentation(true);
            }
        }

        public override Color Color
        {
            get { return color; }

            set
            {
                color = value;
                __UpdatePresentation(true);
            }
        }

        /// <summary>
        /// Sets the current sprite by indexing into the 
        /// list of sprites set up in the inspector.
        /// </summary>
        public int FrameIndex
        {
            get { return frameIndex; }

            set
            {
                frameIndex = value;
                __UpdatePresentation(true);
            }
        }

        protected SpriteRenderer SpriteRenderer
        {
            get
            {
                var sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();

                if (sprite == null)
                {
                    Debug.LogError("The cell needs a child with a SpriteRenderer component attached");
                }

                return sprite;
            }
        }

        public override Vector2 Dimensions
        {
            get { return SpriteRenderer.sprite.bounds.size; }
        }

        public int I;
        public int J;

        public bool IsBusy;
        public void Awake()
        {
            highlightOn = false;
        }

        public override void __UpdatePresentation(bool forceUpdate)
        {
            //for now, always update, regardless of forceUpdate value
            if (frameIndex < sprites.Length)
            {
                SpriteRenderer.sprite = sprites[frameIndex];
            }

            SpriteRenderer.color = highlightOn ? Color.Lerp(color, Color.white, 0.8f) : color;
        }

        public override void SetAngle(float angle)
        {
            SpriteRenderer.transform.SetLocalRotationZ(angle);
        }

        public override void AddAngle(float angle)
        {
            SpriteRenderer.transform.RotateAroundZ(angle);
        }
    }
}