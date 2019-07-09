using UnityEngine;

namespace Core.Rewards.Scratch
{
    public class ScratchCard : MonoBehaviour
    {
        public enum Quality
        {
            Low = 4,
            Medium = 2,
            High = 1
        }
        #region Fields
        public Camera MainCamera;
        public Transform Surface;
        public Quality RenderTextureQuality = Quality.Medium;
        public Material Eraser;
        public Material Progress;
        public Material ScratchSurface;
        public bool isEnable;

        private Camera thisCamera;
        private RenderTexture renderTexture;
        private Renderer scratchRenderer;
        private RectTransform rectTransform;
        private Rect textureRect = new Rect(0f, 0f, 1f, 1f);
        private Vector2 scratchBoundsSize;
        private Vector2 imageSize;
        private Vector2 eraseStartPosition;
        private Vector2 eraseEndPosition;
        private Vector2 erasePosition;
        private bool isCanvasCamera;
        private bool isCanvasOverlay;
        private bool isFirstFrame = true;
        private bool isScratching;
        private bool isStartPosition = true;
#if !UNITY_WEBGL
        private int fingerId = -1;
#endif
        private const string MaskTexProperty = "_MaskTex";
        private const string MainTexProperty = "_MainTex";
        #endregion Fields

        #region Properties
        public bool IsScratching => isScratching;
        #endregion Properties

        private void Start()
        {
            isEnable = CycleRewardsManager.Instance.CurrentScratchDay != UserDataControl.Instance.UserData.RewardsData.lastClaimedScratch;
            GetScratchBounds();
            CreateRenderTexture();
        }

        private void OnPostRender()
        {
            if (isFirstFrame)
            {
                GL.Clear(false, true, Color.clear);
                isFirstFrame = false;
            }

            if (isScratching)
            {
                if (eraseStartPosition == eraseEndPosition)
                {
                    ScratchHole();
                    //if (AudioManager.I.EffectsAudioSource.isPlaying == false)
                    //{
                    //    AudioManager.I.PlayAudio("Scratch Long");
                    //}
                }
                else
                {
                    ScratchLine();
                    //if (AudioManager.I.EffectsAudioSource.isPlaying == false)
                    //{
                    //    AudioManager.I.PlayAudio("Scratch Long");
                    //}
                }
            }


        }

        private void Update()
        {
            if (isEnable)
            {
                if (Input.touchCount == 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        isScratching = false;
                        //if (AudioManager.I.EffectsAudioSource.isPlaying)
                        //{
                        //    AudioManager.I.EffectsAudioSource.Stop();
                        //}
                        isStartPosition = true;
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        isScratching = false;
                        //if (AudioManager.I.EffectsAudioSource.isPlaying)
                        //{
                        //    AudioManager.I.EffectsAudioSource.Stop();
                        //}
                    }
                    if (Input.GetMouseButton(0))
                    {
                        OnScratch(Input.mousePosition);
                    }
                }
                #region #if !UNITY_WEBGL
#if !UNITY_WEBGL
                if (Input.touchSupported)
                {
                    foreach (Touch touch in Input.touches)
                    {
                        if (touch.phase == TouchPhase.Began && fingerId == -1)
                        {
                            fingerId = touch.fingerId;
                            isScratching = false;

                            //if (AudioManager.I.EffectsAudioSource.isPlaying)
                            //{
                            //    AudioManager.I.EffectsAudioSource.Stop();
                            //}


                            isStartPosition = true;
                        }
                        if (touch.fingerId == fingerId)
                        {
                            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                            {
                                OnScratch(touch.position);
                            }
                            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                            {
                                fingerId = -1;
                                isScratching = false;

                                //if (AudioManager.I.EffectsAudioSource.isPlaying)
                                //{
                                //    AudioManager.I.EffectsAudioSource.Stop();
                                //}
                            }
                        }
                    }
                }
#endif
                #endregion #if !UNITY_WEBGL
            }
        }

        #region Methods
        private void OnScratch(Vector2 position)
        {
            if (isEnable)
            {
                Vector3 clickPosition;
                if (isCanvasOverlay)
                {
                    clickPosition = position;
                }
                else if (isCanvasCamera)
                {
                    if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, position, MainCamera, out clickPosition))
                        return;
                }
                else
                {
                    clickPosition = MainCamera.ScreenToWorldPoint(position);
                }
                var surfaceLocalClickPosition = Surface.InverseTransformPoint(clickPosition);
                var clickLocalPosition = new Vector3(surfaceLocalClickPosition.x * Surface.lossyScale.x, surfaceLocalClickPosition.y * Surface.lossyScale.y);
                var boundsSize = new Vector3(scratchBoundsSize.x * Surface.lossyScale.x, scratchBoundsSize.y * Surface.lossyScale.y);
                Vector2 scratchSurfaceClickLocalPosition;
                if (isCanvasCamera)
                {
                    scratchSurfaceClickLocalPosition = clickLocalPosition + (Vector3)scratchBoundsSize / 2f;
                }
                else
                {
                    var bottomLeftLocalPosition = Surface.InverseTransformPoint(Surface.position - boundsSize / 2f);
                    scratchSurfaceClickLocalPosition = clickLocalPosition - bottomLeftLocalPosition;
                }
                var PPI = new Vector2(
                    imageSize.x / scratchBoundsSize.x / Surface.lossyScale.x,
                    imageSize.y / scratchBoundsSize.y / Surface.lossyScale.y
                    );

                erasePosition = new Vector2(
                    scratchSurfaceClickLocalPosition.x * Surface.lossyScale.x * PPI.x,
                    scratchSurfaceClickLocalPosition.y * Surface.lossyScale.y * PPI.y
                    );

                if (isStartPosition)
                {
                    eraseEndPosition = eraseStartPosition;
                    eraseStartPosition = erasePosition;
                }
                else
                {
                    eraseEndPosition = erasePosition;
                }
                isStartPosition = !isStartPosition;

                if (!isScratching)
                {
                    eraseEndPosition = eraseStartPosition;
                    isScratching = true;
                }
            }
        }

        private void CreateRenderTexture()
        {
            thisCamera = GetComponent<Camera>();
            if (thisCamera != null)
            {
                var renderTextureSize = new Vector2(imageSize.x / (float)RenderTextureQuality, imageSize.y / (float)RenderTextureQuality);
                renderTexture = new RenderTexture((int)renderTextureSize.x, (int)renderTextureSize.y, 0, RenderTextureFormat.ARGB32);
                renderTexture.Create();
                ScratchSurface.SetTexture(MaskTexProperty, renderTexture);
                Progress.SetTexture(MainTexProperty, renderTexture);
                thisCamera.targetTexture = renderTexture;
            }
            else
            {
                Debug.LogError("Camera not found!");
            }
        }

        private void GetScratchBounds()
        {
            scratchRenderer = Surface.GetComponent<Renderer>();
            rectTransform = Surface.GetComponent<RectTransform>();
            if (scratchRenderer != null)
            {
                imageSize = new Vector2(scratchRenderer.sharedMaterial.mainTexture.width, scratchRenderer.sharedMaterial.mainTexture.height);
                scratchBoundsSize = scratchRenderer.bounds.size;
            }
            else if (rectTransform != null)
            {
                imageSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
                scratchBoundsSize = new Vector2(rectTransform.rect.size.x * rectTransform.lossyScale.x,
                                                rectTransform.rect.size.y * rectTransform.lossyScale.y);

                var canvas = Surface.transform.GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    isCanvasOverlay = canvas.renderMode == RenderMode.ScreenSpaceOverlay;
                    isCanvasCamera = !isCanvasOverlay;
                }
            }
            else
            {
                Debug.LogError("Can't find Renderer or RectTransform Component!");
            }
        }

        private void DrawQuad(Rect positionRect)
        {
            GL.TexCoord2(textureRect.xMin, textureRect.yMax);
            GL.Vertex3(positionRect.xMin, positionRect.yMax, 0f);
            GL.TexCoord2(textureRect.xMax, textureRect.yMax);
            GL.Vertex3(positionRect.xMax, positionRect.yMax, 0f);
            GL.TexCoord2(textureRect.xMax, textureRect.yMin);
            GL.Vertex3(positionRect.xMax, positionRect.yMin, 0f);
            GL.TexCoord2(textureRect.xMin, textureRect.yMin);
            GL.Vertex3(positionRect.xMin, positionRect.yMin, 0f);
        }

        private void ScratchHole()
        {
            var positionRect = new Rect(
                (erasePosition.x - 0.5f * Eraser.mainTexture.width) / imageSize.x,
                (erasePosition.y - 0.5f * Eraser.mainTexture.height) / imageSize.y,
                Eraser.mainTexture.width / imageSize.x,
                Eraser.mainTexture.height / imageSize.y
                );

            GL.PushMatrix();
            GL.LoadOrtho();
            Eraser.SetPass(0);
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            DrawQuad(positionRect);
            GL.End();
            GL.PopMatrix();
        }

        private void ScratchLine()
        {
            GL.PushMatrix();
            GL.LoadOrtho();
            Eraser.SetPass(0);
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);

            var holesCount = (int)Vector2.Distance(eraseStartPosition, eraseEndPosition) / (int)RenderTextureQuality;
            for (int i = 0; i < holesCount; i++)
            {
                var holePosition = eraseStartPosition + (eraseEndPosition - eraseStartPosition) / holesCount * i;
                var positionRect = new Rect(
                    (holePosition.x - 0.5f * Eraser.mainTexture.width) / imageSize.x,
                    (holePosition.y - 0.5f * Eraser.mainTexture.height) / imageSize.y,
                    Eraser.mainTexture.width / imageSize.x,
                    Eraser.mainTexture.height / imageSize.y
                    );
                DrawQuad(positionRect);
            }
            GL.End();
            GL.PopMatrix();
        }

        public void Reset()
        {
            CreateRenderTexture();
        }
        #endregion Methods
    }
}