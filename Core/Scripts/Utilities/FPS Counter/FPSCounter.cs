//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    public class FPSCounter : MonoBehaviour
    {
        #region Fields
        public TMPro.TextMeshProUGUI highestFPSLabel, averageFPSLabel, lowestFPSLabel;
        public int frameRange = 60;
        [SerializeField]
        private FPSColor[] coloring;
        private int[] fpsBuffer;
        private int fpsBufferIndex;
        #endregion Fields

        #region Properties
        public int FPS { get; private set; }
        public int HighestFPS { get; private set; }
        public int AverageFPS { get; private set; }
        public int LowestFPS { get; private set; }
        #endregion Properties

        private void Update()
        {

            if (fpsBuffer == null || fpsBuffer.Length != frameRange)
            {
                InitializeBuffer();
            }
            UpdateBuffer();
            CalculateFPS();

            FPS = (int)(1 / Time.unscaledDeltaTime);
            Display(highestFPSLabel, HighestFPS, 'H');
            Display(averageFPSLabel, AverageFPS, 'A');
            Display(lowestFPSLabel, LowestFPS, 'L');
        }

        void InitializeBuffer()
        {
            if (frameRange <= 0)
            {
                frameRange = 1;
            }
            fpsBuffer = new int[frameRange];
            fpsBufferIndex = 0;
        }

        void UpdateBuffer()
        {
            fpsBuffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
            if (fpsBufferIndex >= frameRange)
            {
                fpsBufferIndex = 0;
            }
        }

        void CalculateFPS()
        {
            int sum = 0;
            int highest = 0;
            int lowest = int.MaxValue;
            for (int i = 0; i < frameRange; i++)
            {
                int fps = fpsBuffer[i];
                sum += fps;
                if (fps > highest)
                {
                    highest = fps;
                }
                if (fps < lowest)
                {
                    lowest = fps;
                }
            }
            AverageFPS = sum / frameRange;
            HighestFPS = highest;
            LowestFPS = lowest;
        }

        [System.Serializable]
        private struct FPSColor
        {
            public Color color;
            public int minimumFPS;
        }

        void Display(TMPro.TextMeshProUGUI label, int fps, char mark)
        {
            label.text = $"{mark}: {fps}";
            for (int i = 0; i < coloring.Length; i++)
            {
                if (fps >= coloring[i].minimumFPS)
                {
                    label.color = coloring[i].color;
                    break;
                }
            }
        }
    }
}