using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIScene
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button _playButton = null;
        [SerializeField] private Button _quitButton = null;


        private static UIManager _instance;
        public static UIManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            Init();
        }

        private void Init()
        {
            _playButton.onClick.AddListener(PlayGame);
            _quitButton.onClick.AddListener(QuitGame);
        }

        private void PlayGame()
        {
            SceneManager.LoadScene("TetrisMainScene");
        }

        private void QuitGame()
        {
            Application.Quit();
        }
    }
}
