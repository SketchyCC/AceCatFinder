using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace GameEvents
{
    public class TestEvent : GameEvent
    {
        public TestEvent()
        {            
        }

        public override bool isValid()
        {
            return base.isValid(); //check if event is valid. I actually didn't use it in my past projects lol
        }
    }

    public class SimpleTestEvent : GameEvent { }

    public class DayPassedEvent : GameEvent
    {
        public int NumberofDays;
        public DayPassedEvent(int days)
        {
            NumberofDays = days;
        }
    }

    public class MissingPosterReleasedEvent : GameEvent
    {
        public PosterObject Posterobject;
        public MissingPosterReleasedEvent(PosterObject obj)
        {
            Posterobject = obj;
        }
    }

    public class PosterAcceptedEvent : GameEvent
    {
        public PosterObject Posterobject;
        public GameObject PosterEntity;//i don't remember what this was for :I
        public PosterAcceptedEvent(PosterObject obj, GameObject gObj)
        {
            Posterobject = obj;
            PosterEntity = gObj;
        }
    }

    public class PosterCompletedEvent : GameEvent
    {
        public PosterObject Posterobject;
        public PosterCompletedEvent(PosterObject obj)
        {
            Posterobject = obj;
        }
    }

    public class PosterFailedEvent : GameEvent
    {
        public PosterObject Posterobject;
        public PosterFailedEvent(PosterObject obj)
        {
            Posterobject = obj; //may have just made this event obsolete
        }
    }

    public class CatCaughtEvent : GameEvent
    {
        public int CatObj;
        public CatCaughtEvent(int obj)
        {
            CatObj = obj;
        }
    }
    public class RightCatEvent : GameEvent {
        public int CatObj;
        public RightCatEvent(int obj)
        {
            CatObj = obj;
        }
    }
    public class WrongCatEvent : GameEvent
    {
        public int CatObj;
        public bool pressYes = true;
        public WrongCatEvent(int obj, bool yes)
        {
            CatObj = obj;
            pressYes = yes;
        }
    }

    //------Animation Triggers ------

    public class WalkingEvent : GameEvent
    {
        public bool IsWalking;
        public WalkingEvent(bool walk)
        {
            IsWalking = walk;
        }
    }

    public class SneakEvent : GameEvent
    {
        public bool IsSneaking;
        public SneakEvent(bool sneak)
        {
            IsSneaking = sneak;
        }
    }

    public class BagOpenEvent : GameEvent
    {
        public bool openBag;
        public BagOpenEvent(bool bag)
        {
            openBag = bag;
        }
    }

    public class NetOutEvent : GameEvent
    {
        public bool netOut;
        public NetOutEvent(bool net)
        {
            netOut = net;
        }
    }

    public class SwingEvent : GameEvent
    {
        public SwingEvent() { }
    }
    //------------

    public class CatReturnedEvent : GameEvent { }

    public class MoneyUpdate : GameEvent { }

    public class CommunityBoardLook : GameEvent { }
    public class CommunityBoardLeave : GameEvent { }

    public class PosterLookingEvent : GameEvent
    {
        public GameObject posterSlot;
        public PosterLookingEvent(GameObject obj)
        {
            posterSlot = obj;
        }
    }

    public class PosterSoundEvent : GameEvent { }

    public class GenericButtonPressedEvent : GameEvent { }//for all buttons to play button pressing sound, excludes bag, poster

    public class ItemBoughtEvent : GameEvent
    {
        public ItemObject item;
        public ItemBoughtEvent(ItemObject _item)
        {
            item = _item;
        }
    }

    public class SpeedUpgradeBought : GameEvent
    {
        public float speed;
        public SpeedUpgradeBought(float _speed)
        {
            speed = _speed;
        }
    }

    public class ClothingChangeEvent : GameEvent
    {
        public int clothing;
        public ClothingChangeEvent(int cloth)
        {
            clothing = cloth;
        }
    }

    public class GamePausedEvent : GameEvent {
        public bool GameIsPaused;
        public GamePausedEvent(bool isPaused)
        {
            GameIsPaused = isPaused;
        }
    }

    public class TimeStoppedEvent : GameEvent
    {
        public bool timeStopped;
        public TimeStoppedEvent(bool isPaused)
        {
            timeStopped = isPaused;
        }
    }

    public class UIOpened : GameEvent //ideally, this could be merged with the community board looking event but it's used in too many other scripts
    {
        public bool UIisopened;
        public GameObject UIOrigin;
        public UIOpened(bool ui, GameObject obj)
        {
            UIisopened = ui;
            UIOrigin = obj;
        }
    }

    //--save system--//
    public class LoadEvent : GameEvent { }
}