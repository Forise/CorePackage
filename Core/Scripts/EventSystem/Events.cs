//Developed by Pavel Kravtsov.
namespace Core.EventSystem
{
    public class Events
    {
        public class Daily
        {
            public const string NEX_DAY = "NextDay";
        }
        public class Rewards
        {
            public const string WHEEL_STARTED = "WheelStarted";
            public const string WHEEL_STOPPED = "WheelStopped";
        }
        public class SaveData
        {
            public const string NEW_DATA_APPLIED = "NewDataApplied";
        }
        public class DateTime
        {
            public const string NIST_DATE_GOTTEN = "NistDateGotten";
            public const string URMOBI_DATE_GOTTEN = "UrmobiDateGotten";
        }

        public class PlayerEvents
        {
            public const string PLAYER_ATTACK = "PlayerAttack";
            public const string PLAYER_GROA = "PlayerGroa";
            public const string PLAYER_JUMP = "PlayerJump";
            public const string PLAYER_GOT_DAMAGE = "PlayerGotDamage";
            public const string PLAYER_DIED = "PlayerDied";
            public const string PLAYER_USE_SKILL = "PlayerUseSkill";
            public const string PLAYER_HP_CHANGED = "PlayerHPChanged";
            public const string PLAYER_START_SLIDE = "PlayerStartSlide";
            public const string PLAYER_SLIDING = "PlayerSliding";
            public const string PLAYER_LANDED = "PlayerLanded";
            public const string PLAYER_PICKUP_COIN = "PlayerPickUpCoin";
            public const string PLAYER_JUMPED = "PlayerJumped";
            public const string PLAYER_RESURRECTED = "PlayerResurrected";
            public const string PLAYER_RESURRECTION_START = "PlayerResurrectedStart";
        }

        public class ADEvents
        {
            public const string SHOW_AD = "ShowAD";
            public const string SHOW_DEFAULT_AD = "ShowDefaultAD";
            public const string SHOW_VIDEO_AD = "ShowVideoAD";
            public const string SHOW_REWARD_AD = "ShowRewardAD";
        }

        public class Achievments
        {
            public const string ACHIEVEMENT_TRIGGERED = "AchievementTriggered";
        }

        public class DamageEvents
        {
            public const string OBJECT_KILLED = "ObjectKilled";
        }

        public class EnemyEvents
        {
            public const string ENEMY_GOT_DAMAGE = "EnemyGotDamage";
            public const string ENEMY_DIED = "EnemyDied";
        }

        public class SettingsEvents
        {
            public const string LANGUAGE_CHANGED = "LanguageChanged";
        }

        public class InputEvents
        {
            public const string BLOCK_HUD = "BlockHUD";
            public const string UNBLOCK_HUD = "UnblockHUD";
            public const string BLOCK_INPUT = "BlockInput";
            public const string UNBLOCK_INPUT = "UnblockInput";
            public const string UI_TAPPED = "UITapped";
        }

        public class AudioEvents
        {
            public const string PLAY_GAMEPLAY_MUSIC = "PlayGameplayMusic";
            public const string STOP_GAMEPLAY_MUSIC = "StopGameplayMusic";
            public const string PLAY_MENU_MUSIC = "PlayMenuMusic";
            public const string STOP_MENU_MUSIC = "StopMenuMusic";
            public const string PLAY_BONUS_LOCATION_MUSIC = "PlayBonusLocationMusic";
            public const string STOP_BONUS_LOCATION_MUSIC = "StopBonusLocationMusic";
        }

        public class ApplicationEvents
        {
            public const string APPLICATION_STARTED = "ApplicationStarted";

            public const string FOCUS_ON = "FocusOn";

            public const string FOCUS_LOST = "FocusLost";

            public const string SETTINGS_CHANGED = "SettingsChanged";
        }

        public class SocialEvents
        {
            public const string SIGNED_IN = "SignedIn";
            public const string FAIL_SIGN_IN = "FailedSignIn";
            public const string SIGNED_OUT = "SignedOut";

            public const string DATA_LOADED = "DataLoaded";
            public const string FAIL_DATA_LOADING = "FailDataLoading";
        }

        public class BossEvents
        {
            public const string BOSS_ATTACK = "BossAttack";
            public const string BOSS_DIED = "BossDied";
        }

        public class EnvironmentEvents
        {
            public const string TNT_EXPLOSION = "TNTExplosion";
            public const string GLUE_TRIGGERED = "GlueTriggered";
            public const string DAMAGED_TILE_TRIGGERED = "DamagedTileTriggered";
            public const string DAMAGED_TILE_CRUSHED = "DamagedTileCrushed";
            public const string MOVING_TILE_START_MOVE = "MovingTileStartMove";
            public const string MOVING_TILE_MOVING = "MovingTileMoving";
            public const string MOVING_TILE_END_MOVE = "MovingTileEndMove";
            public const string CHEST_OPENED = "ChestOpened";
        }

        public class GameEvents
        {
            public const string MINIGAME_STARTED = "MinigameStarted";
            public const string MINIGAME_ENDED = "MinigameEnded";
            public const string GAME_STARTED = "GameStarted";
            public const string GAME_ENDED = "GameEnded";
            public const string BONUS_LEVEL_STARTED = "BonusLevelStarted";
            public const string BONUS_LEVEL_ENDED = "BonusLevelEnded";
        }

        public class IAPEvents
        {
            public const string SUCCESSFULL_BOUGHT = "SuccessfullBought";
            public const string FAILED_BOUGHT = "FailedBought";
            public const string PRODUCT_RESTORED = "ProductRestored";
        }
    }
}