using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Rewards.Scratch
{
    public class ScratchCardManager : ARewardMechanicManager<ScratchCardManager>
    {
        public enum ScratchCardRenderType
        {
            MeshRenderer,
            SpriteRenderer,
            CanvasRenderer
        }
        #region Fields
        public SpritePrize[] prizes;
        public Camera MainCamera;
        public ScratchCardRenderType RenderType;
        public Sprite ScratchSurfaceSprite;
        public Texture EraseTexture;

        public ScratchCard Card;
        public EraseProgress Progress;
        public GameObject MeshCard;
        public GameObject SpriteCard;
        public GameObject ImageCard;
        public Image RawardImage;
        public Shader MaskShader;
        public Shader BrushShader;
        public Shader MaskProgressShader;
        public int RequiredProgress;

        private Material scratchSurfaceMaterial;
        private Material eraserMaterial;
        private Material progressMaterial;

        private float progress;
        private SpritePrize currentPrize;
        #endregion Fields

        #region Properties
        protected override byte GetRewardByDay
        {
            get
            {
                if (Daily.Daily.Instance.CurrentDailyDay <= 3)
                    return (byte)Random.Range(0, 2);
                else if (Daily.Daily.Instance.CurrentDailyDay <= 5)
                    return (byte)Random.Range(1, 5);
                else if (Daily.Daily.Instance.CurrentDailyDay == 6)
                    return 5;
                return 0;
            }
        }
        #endregion Properties

        private void Awake()
        {
            Progress.OnProgress += OnProgress_Handler;
            if (Card.MainCamera == null)
            {
                Card.MainCamera = MainCamera;
            }

            if (Card.ScratchSurface == null)
            {
                scratchSurfaceMaterial = new Material(MaskShader);
                scratchSurfaceMaterial.mainTexture = ScratchSurfaceSprite.texture;
                Card.ScratchSurface = scratchSurfaceMaterial;
            }

            if (Card.Eraser == null)
            {
                eraserMaterial = new Material(BrushShader);
                eraserMaterial.mainTexture = EraseTexture;
                Card.Eraser = eraserMaterial;
            }

            if (Card.Progress == null)
            {
                progressMaterial = new Material(MaskProgressShader);
                Card.Progress = progressMaterial;
            }

            switch (RenderType)
            {
                case ScratchCardRenderType.MeshRenderer:
                    MeshCard.SetActive(true);
                    SpriteCard.SetActive(false);
                    ImageCard.SetActive(false);
                    Card.Surface = MeshCard.transform;
                    MeshCard.GetComponent<Renderer>().material = scratchSurfaceMaterial;
                    break;
                case ScratchCardRenderType.SpriteRenderer:
                    MeshCard.SetActive(false);
                    SpriteCard.SetActive(true);
                    ImageCard.SetActive(false);
                    Card.Surface = SpriteCard.transform;
                    var sprite = SpriteCard.GetComponent<SpriteRenderer>();
                    sprite.sprite = ScratchSurfaceSprite;
                    sprite.material = scratchSurfaceMaterial;
                    break;
                case ScratchCardRenderType.CanvasRenderer:
                    MeshCard.SetActive(false);
                    SpriteCard.SetActive(false);
                    ImageCard.SetActive(true);
                    Card.Surface = ImageCard.transform;
                    var image = ImageCard.GetComponent<Image>();
                    image.sprite = ScratchSurfaceSprite;
                    image.material = scratchSurfaceMaterial;
                    currentPrize = prizes[GetRewardByDay];
                    RawardImage.sprite = currentPrize.sprite;
                    break;
            }
        }

        #region Methods
        public void SetEraseTexture(Texture texture)
        {
            eraserMaterial.mainTexture = texture;
        }

        public void ResetScratchCard()
        {
            if (CycleRewardsManager.Instance.CurrentScratchDay != UserDataControl.Instance.UserData.RewardsData.lastClaimedScratch)
            {
                currentPrize = prizes[GetRewardByDay];
                RawardImage.sprite = currentPrize.sprite;
                Card.Reset();
            }
        }
        #endregion Methods

        #region Handlers
        private void OnProgress_Handler(float progress)
        {
            progress = Mathf.Round(progress * 100f);
            if (progress >= RequiredProgress && CycleRewardsManager.Instance.CurrentScratchDay != UserDataControl.Instance.UserData.RewardsData.lastClaimedScratch)
            {
                currentPrize.reward.Consume();
                UserDataControl.Instance.UserData.RewardsData.lastClaimedScratch = CycleRewardsManager.Instance.CurrentScratchDay;
                UIControl.Instance.ShowInfoPopUp(new UIPopUp.PopupData($"You got {currentPrize.reward.reward} coins!"));
                UserDataControl.Instance.SaveData();
            }
        }
        #endregion Handlers
    }
}