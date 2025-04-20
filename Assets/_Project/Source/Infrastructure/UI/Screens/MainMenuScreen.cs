using UnityEngine;

namespace Source.Infrastructure.Services
{
    public class MainMenuScreen : UIScreen
    {
        public override void Show()
        {
            base.Show();
            Debug.Log("<color=green>[SCREEN] MainMenuScreen.Show</color>");
        }

        public override void Hide()
        {
            base.Hide();
            Debug.Log("<color=green>[SCREEN] MainMenuScreen.Hide</color>");
        }
    }
}
