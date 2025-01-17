using Managers;
using MyBox;
using Scene;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace UI
{
    public class UI_EndScreen : MonoBehaviour
    {
        #region Constants
            private const string VE_BG = "VE_Background";
        
            private const string BT_MENU = "BT_Menu";
            private const string BT_REPLAY = "BT_Replay";
            
            private const string LB_TIMER = "LB_Timer";
            private const string LB_KILLS = "LB_Kills";
            private const string LB_SCORE = "LB_Score";
        #endregion
        
        [SerializeField] private UIDocument layout;
        [SerializeField, Scene] private string gameScene, menuScene;
        
        #region Visual Elements
            private VisualElement bgVE;
            private Button menuBT, replayBT;
            private Label timerLB, killsLB, scoreLB;
        #endregion
        
        private void Start()
        {
            var root = layout.rootVisualElement;
            
            // Background
            bgVE = root.Q<VisualElement>(VE_BG);
            
            // Buttons
            menuBT = root.Q<Button>(BT_MENU);
            replayBT = root.Q<Button>(BT_REPLAY);
            
            // Labels
            timerLB = root.Q<Label>(LB_TIMER);
            killsLB = root.Q<Label>(LB_KILLS);
            scoreLB = root.Q<Label>(LB_SCORE);
            
            // Binding
            Utilities.BindButton(menuBT, ToMenu, true, false);
            Utilities.BindButton(replayBT, Replay, true, false);
            
            GameManager.Instance.OnEndGame -= Display;
            GameManager.Instance.OnEndGame += Display;
        }

        private void Display()
        {
            var nbKills = GameManager.Instance.GetNbEnemiesKilled();
            var timer = GameManager.Instance.currentTimer();
            var nbCoins = GameManager.Instance.GetTotalCoins();
            timerLB.text = $"{(int) timer} s";
            killsLB.text = nbKills + " kill" + (nbKills > 1 ? "s" : "");
            scoreLB.text = nbCoins + " coin" + (nbCoins > 1 ? "s" : "");

            replayBT.Focus();
            
            bgVE.visible = true;
        }
        
        private void ToMenu()
        {
            SceneController.Instance.QuickLoad(menuScene);
        }
        
        private void Replay()
        {
            SceneController.Instance.Reload(gameScene);
        }
        
    }
}
