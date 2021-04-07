using UnityEngine;
using UnityEngine.UI;
using GameEvents;

public class TextBoxManager : MonoBehaviour
{
    public GameObject textBox;
    public Text nameBox;
    public Text theText;
    string firstName;
    string secondName;

    public TextAsset textFile;
    
    public string[] textlines;

    public int currentLine;
    public int endAtLine;

    void Start()
    {
        if (textFile != null)
        {
            textlines = (textFile.text.Split('\n'));
        }
        firstName = textlines[0];
        secondName = textlines[1];
        nameBox.text = firstName;
        if (endAtLine == 0)
        {
            endAtLine = textlines.Length - 1;
        }
        currentLine = 2;
        theText.text = textlines[2];
    }

    private void Update()
    {
        if (textBox.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                currentLine += 1;
                ShowLines();
            }
        }
    }
    public void ShowLines()
    {
        if (currentLine > endAtLine)
        {
            textBox.SetActive(false);
            GameEventManager.Raise(new UIOpened(false, gameObject));
        }
        else if (textlines[currentLine] == firstName|| textlines[currentLine] == secondName)
        {
            nameBox.text = textlines[currentLine];
            currentLine += 1;
            theText.text = textlines[currentLine];
        }
        else
        {
            theText.text = textlines[currentLine];
        }

    }
    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<PosterCompletedEvent>(OnPosterCompleted);
    }
    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<PosterCompletedEvent>(OnPosterCompleted);
    }

    public virtual void OnPosterCompleted(PosterCompletedEvent e)
    {
        if (e.Posterobject.ReturnedDialogue != null)
        {
            textFile = e.Posterobject.ReturnedDialogue;
            textlines = (textFile.text.Split('\n'));

            firstName = textlines[0];
            secondName = textlines[1];
            nameBox.text = firstName;
            currentLine = 2;
            textBox.SetActive(true);
            GameEventManager.Raise(new UIOpened(textBox.activeSelf, gameObject));


            endAtLine = textlines.Length - 1;
            theText.text = textlines[2];
        }

    }
}
