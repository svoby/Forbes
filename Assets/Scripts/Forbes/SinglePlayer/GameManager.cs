using UnityEngine;
using Forbes.Cameras;
using Forbes.Inputs;
using Forbes.Utils;

namespace Forbes.SinglePlayer
{
    public class GameManager
    {
        public event System.Action<IPlayerController> OnLocalPlayerJoined;

        protected GameObject gameObject;

        private static GameManager m_Instance;
        public static GameManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new GameManager();
                    m_Instance.gameObject = new GameObject("_gameManager");
                    m_Instance.gameObject.AddComponent<InputController>();
                    m_Instance.gameObject.AddComponent<Utils.Timer>();
                    m_Instance.gameObject.AddComponent<CameraController>();
                }
                return m_Instance;
            }
        }

        private InputController m_InputController;
        public InputController InputController
        {
            get
            {
                if (m_InputController == null)
                    m_InputController = gameObject.GetComponent<InputController>();

                return m_InputController;
            }
        }

        private IPlayerController m_LocalPlayer;
        public IPlayerController LocalPlayer
        {
            get
            {
                return m_LocalPlayer;
            }
            set
            {
                m_LocalPlayer = value;
                if (OnLocalPlayerJoined != null)
                    OnLocalPlayerJoined(m_LocalPlayer);
            }
        }

        private Timer m_Timer;
        public Timer Timer
        {
            get
            {
                if (m_Timer == null)
                    m_Timer = gameObject.GetComponent<Timer>();

                return m_Timer;
            }
        }

        private CameraController m_CameraController;
        public CameraController CameraController
        {
            get
            {
                if (m_CameraController == null)
                    m_CameraController = gameObject.GetComponent<CameraController>();

                return m_CameraController;
            }
        }

        private IGameLogic m_GameLogic;
        public IGameLogic GameLogic
        {
            get
            {
                return m_GameLogic;
            }
            set
            {
                m_GameLogic = value;
            }
        }

        private static Spawner m_Spawner;
        public static Spawner Spawner
        {
            get
            {
                if (m_Spawner == null)
                    m_Spawner = new Spawner();

                return m_Spawner;
            }
        }
    }
}