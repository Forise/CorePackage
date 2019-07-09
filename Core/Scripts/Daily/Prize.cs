using UnityEngine;

namespace Core.Rewards
{
    [System.Serializable]
    public struct Prize
    {
        [Range(0, 255)]
        public byte id;
        [Range(0f, 100f)]
        public float chance;
        public Reward reward;
    }

    [System.Serializable]
    public struct SpritePrize
    {
        [Range(0, 255)]
        public byte id;
        [Range(0f, 100f)]
        public float chance;
        public Reward reward;
        public Sprite sprite;
    }
}