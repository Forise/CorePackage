using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Rewards
{
    public class Slot : MonoBehaviour
    {
        public List<UISlotPrize> prizes;
        public List<AnimationCurve> animationCurves;
        public float spinDuration;
        public bool useRandomDuration;
        [Tooltip("X = min, Y = max")]
        public Vector2Int randomSpinDuration;
        public bool useRandomCurve;

        private float itemHeight;
        private float rowHeight;
        private float maxPos;
        [SerializeField]
        private int targetItem;
        [SerializeField]
        private int direction = 1;
        private bool spinning;

        public bool Spinning => spinning;

        void Start()
        {
            spinning = false;
            itemHeight = prizes[0].transform.localPosition.y - prizes[1].transform.localPosition.y;
            rowHeight = prizes.Count * itemHeight;
        }

        public int Spin(int target)
        {
            if (spinning)
                return 0;
            targetItem = target;

            if(useRandomDuration)
                spinDuration = Random.Range(randomSpinDuration.x, randomSpinDuration.y+1);

            //targetItem = Random.Range(0, prizes.Count);

            maxPos = rowHeight * spinDuration + (targetItem * itemHeight);
            if (maxPos < rowHeight * 2)
            {
                maxPos += rowHeight * 2;
            }

            Debug.Log("randomTime = " + spinDuration);
            Debug.Log("itemNumber = " + targetItem);
            Debug.Log("maxAngle = " + maxPos);


            StartCoroutine(Spin(spinDuration, maxPos));
            return prizes[targetItem].prize.reward.reward;
        }

        private IEnumerator Spin(float time, float maxPos)
        {
            spinning = true;

            float timer = 0.0f;
            float startPos = transform.localPosition.y;
            maxPos = maxPos - startPos;

            int animationCurveNumber = useRandomCurve ? Random.Range (0, animationCurves.Count) : animationCurves.Count - 1;
            Debug.Log("Animation Curve No. : " + animationCurveNumber);

            while (timer < time)
            {
                float pos = maxPos * animationCurves[animationCurveNumber].Evaluate(timer / time);
                transform.localPosition = new Vector3(transform.localPosition.x, (pos + startPos) % rowHeight, 0.0f);
                timer += Time.deltaTime;
                yield return 0;
            }

            transform.localPosition = new Vector3(transform.localPosition.x, (maxPos + startPos) % rowHeight, 0.0f);
            spinning = false;
            prizes[targetItem].prize.reward.Consume();

            Debug.Log("Prize: " + prizes[targetItem]);
        }
    }
}