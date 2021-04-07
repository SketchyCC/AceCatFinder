using UnityEngine;
using Cinemachine;
using GameEvents;

public class MouseLockSettings: MonoBehaviour
{
    public CinemachineFreeLook playercamera;
    float defaultspeedX=200f;
    float defaultspeedY=20f;

    int mouselockcount;

    void Start()
    {
        playercamera.m_YAxis.m_MaxSpeed = defaultspeedY;
        playercamera.m_XAxis.m_MaxSpeed = defaultspeedX;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<BagOpenEvent>(OnBagOpen);
        GameEventManager.AddListener<CommunityBoardLook>(OnBoardLook);
        GameEventManager.AddListener<CommunityBoardLeave>(OnBoardLeave);
        GameEventManager.AddListener<GamePausedEvent>(OnGamePaused);
        GameEventManager.AddListener<UIOpened>(OnInUI);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<BagOpenEvent>(OnBagOpen);
        GameEventManager.RemoveListener<CommunityBoardLook>(OnBoardLook);
        GameEventManager.RemoveListener<CommunityBoardLeave>(OnBoardLeave);
        GameEventManager.RemoveListener<GamePausedEvent>(OnGamePaused);
        GameEventManager.RemoveListener<UIOpened>(OnInUI);
    }

    private void OnInUI(UIOpened e)
    {
        if (e.UIisopened)
        {
            mouselockcount += 1;
            playercamera.m_YAxis.m_MaxSpeed = 0;
            playercamera.m_XAxis.m_MaxSpeed = 0;
        }
        else if (!e.UIisopened)
        {
            mouselockcount -= 1;
            playercamera.m_YAxis.m_MaxSpeed = defaultspeedY;
            playercamera.m_XAxis.m_MaxSpeed = defaultspeedX;
        }
        CheckMouseLock();
    }

    private void OnGamePaused(GamePausedEvent e)
    {
        if (e.GameIsPaused)
        {
            mouselockcount += 1;
        }
        else if (!e.GameIsPaused)
        {
            mouselockcount -= 1;
        }
        CheckMouseLock();
    }


    private void OnBagOpen(BagOpenEvent e)
    {
        if (e.openBag)
        {
            playercamera.m_YAxis.m_MaxSpeed = 0;
            playercamera.m_XAxis.m_MaxSpeed = 0;
            mouselockcount += 1;
            CheckMouseLock();
        }
        else if (!e.openBag)
        {
            playercamera.m_YAxis.m_MaxSpeed = defaultspeedY;
            playercamera.m_XAxis.m_MaxSpeed = defaultspeedX;
            mouselockcount -= 1;
            CheckMouseLock();
        }

    }

    private void OnBoardLook(CommunityBoardLook e)
    {
        mouselockcount += 1;
        CheckMouseLock();
    }

    private void OnBoardLeave(CommunityBoardLeave e)
    {
        mouselockcount -= 1;
        CheckMouseLock();
    }

    private void OnApplicationQuit()
    {
        playercamera.m_YAxis.m_MaxSpeed = defaultspeedY;
        playercamera.m_XAxis.m_MaxSpeed = defaultspeedX;
    }

    void CheckMouseLock()
    {
        if (mouselockcount == 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (mouselockcount > 0)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (mouselockcount < 0)
        {
            mouselockcount = 0;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
