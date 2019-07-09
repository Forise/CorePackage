//Developed by Pavel Kravtsov.
using Core.AD;
using Core.EventSystem;
using Core.GooglePlayServices;
using UnityEngine;

namespace Core
{
    public class UITests : MonoBehaviour
    {
        #region TEST CHEATS
        public GameObject fps;
        void TestRayCast()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 700f))
            {
                if (hit.transform)
                {
                    Debug.Log(hit.transform.gameObject.name, hit.transform);
                }
            }
        }

        void Test2DRaycast()
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.transform)
            {
                Debug.Log(hit.transform.gameObject.name, hit.transform);
            }
        }

        void OnGUI()
        {
            GUI.BeginGroup(new Rect(0, 300, 350, 500));
            //if (GUILayout.Button("Stop Game", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            //{
            //    GameplayManager.Instance.StopGame();
            //}
            if (GUILayout.Button("Show DEFAULT AD", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            {
                EventManager.Notify(this, new GameEventArgs(Events.ADEvents.SHOW_AD, (int)InterstitialType.Default));
            }
            if (GUILayout.Button("Show VIDEO AD", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            {
                EventManager.Notify(this, new GameEventArgs(Events.ADEvents.SHOW_AD, (int)InterstitialType.Video));
            }
            if (GUILayout.Button("Show REWARD AD", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            {
                EventManager.Notify(this, new GameEventArgs(Events.ADEvents.SHOW_AD, (int)InterstitialType.Reward));
            }
            if (GUILayout.Button("On/Off FPS", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            {
                fps.SetActive(!fps.activeInHierarchy);
            }
            if (GUILayout.Button("PostScore", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            {
                GooglePlayLeaderboard.Instance.PostScore(1);
            }
            if (GUILayout.Button("LoadScore", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            {
                GooglePlayLeaderboard.Instance.LoadScores();
            }
            if (GUILayout.Button("LoadSingleScore", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            {
                GooglePlayLeaderboard.Instance.LoadSingleScore();
            }
            if (GUILayout.Button("ShowUI", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            {
                GooglePlayLeaderboard.Instance.ShowLeaderboardUI();
            }
            if (GUILayout.Button("load board", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            {
                GooglePlayLeaderboard.Instance.LoadBoard(GPGSIds.leaderboard_test_leaderboard);
            }
            //if (GUILayout.Button("Refresh Field buster", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            //{
            //    EventManager.Notify(new GameEvent(GameplayEvents.USE_BUSTER, (int)BusterType.ShuffleBooster), this);
            //}
            //if (GUILayout.Button("Clear Tiles of type buster", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            //{
            //    EventManager.Notify(new GameEvent(GameplayEvents.USE_BUSTER, (int)BusterType.BomdBooster), this);
            //}
            //if (GUILayout.Button("ADD BOMB BUSTER", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.03f)))
            //{
            //    UserDataControl.Instance.AddBombBooster(5);
            //}
            GUI.EndGroup();
        }
        #endregion
    }
}