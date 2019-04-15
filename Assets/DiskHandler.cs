using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DiskHandler : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    public Sprite disk;

    public GameObject leftPeg;

    public GameObject middlePeg;

    public GameObject rightPeg;

    public Sprite peg;

    public SpriteDrawMode drawMode = SpriteDrawMode.Sliced;

    public TMP_Text counterText;
    public TMP_Text statusText;

    public float winDuration = 5f;
    public float helpDuration = 5f;

    public Stack<HanoiDisk> lPeg = new Stack<HanoiDisk>();
    public Stack<HanoiDisk> mPeg = new Stack<HanoiDisk>();
    public Stack<HanoiDisk> rPeg = new Stack<HanoiDisk>();

    private int noDisks;

    // scaleFact implementation is not perfect, should not use linear sizing but rather exponential/logarithmic sizing
    [SerializeField]
    public float scaleFact;

    // todo - implement flashy tetris-particle placement disk effect?
    // currently uses Dictionary to bind peg and stack.
    // todo - bind stack to gameobject -- impossible (because it's binded already to the script) do the latter below.
    // todo - refactor diskhander to manage disks-prefabs (?)


    public Dictionary<Stack<HanoiDisk>, GameObject> stackDict = new Dictionary<Stack<HanoiDisk>, GameObject>();

    private int moveCount;

    void Start()
    {
        // initialize disks.
        noDisks = PlayerPrefs.GetInt("no_disks");
        for (int i =1; i < noDisks + 1; i++)
        {
            HanoiDisk h = new HanoiDisk(noDisks - i, disk, leftPeg.transform.position, middlePeg.transform.position, rightPeg.transform.position, drawMode);
            h.gameObject.transform.SetParent(this.gameObject.transform);

            //fix world positions
            h.gameObject.transform.position = leftPeg.transform.position + Vector3.up * (i - 1) + Vector3.back;

            if (i > 1)
                h.gameObject.transform.position = GetTop(lPeg.Peek().gameObject);

            BottomToCenter(h.gameObject);

            lPeg.Push(h);
        }

        // initialize pegs.
        SpriteRenderer lPegSpr = leftPeg.AddComponent<SpriteRenderer>();
        SpriteRenderer mPegSpr = middlePeg.AddComponent<SpriteRenderer>();
        SpriteRenderer rPegSpr = rightPeg.AddComponent<SpriteRenderer>();

        lPegSpr.sprite = mPegSpr.sprite = rPegSpr.sprite = peg;
        lPegSpr.drawMode = mPegSpr.drawMode = rPegSpr.drawMode = drawMode;

        lPegSpr.size = mPegSpr.size = rPegSpr.size += Vector2.up * noDisks * scaleFact;
        Vector2 tempBase = leftPeg.transform.position;

        BottomToCenter(leftPeg);
        BottomToCenter(middlePeg);
        BottomToCenter(rightPeg);

        stackDict.Add(lPeg, leftPeg);
        stackDict.Add(mPeg, middlePeg);
        stackDict.Add(rPeg, rightPeg);
    }


    Stack<HanoiDisk> fromStack = null;
    Stack<HanoiDisk> targetStack = null;
    HanoiDisk selectDisk = null;
    Vector3 initial= Vector3.zero;
    float timer;
    int helpLevel;
    float helpTimer;
    void Update()
    {

        if (WonGame())
        {
            if(timer < float.Epsilon)
                statusText.text = "You won! Going back to Menu...";
            timer += Time.deltaTime;
            if (timer > winDuration)
            {
                statusText.text = "";
                SceneManager.LoadScene("Menu");
            }
        }
        else
        {
            if (helpTimer < float.Epsilon)
            {
                if (helpLevel == 0)
                    statusText.text = "Welcome to Towers of Hanoi! Try to move all the disks to the rightmost peg by dragging them.";
                if (helpLevel == 1)
                    statusText.text = "You can only place disks on top! You cannot place a bigger disk on top of a smaller one.";
                if (helpLevel == 2)
                    statusText.text = "The best solution is done in " + (Mathf.Pow(2,noDisks) -1) + " moves.";

            }
            if (helpLevel < 3)
                helpTimer += Time.deltaTime;
            if (helpTimer > helpDuration)
            {
                helpTimer = 0;
                statusText.text = "";
                helpLevel++;
            }

            if (Input.GetMouseButton(0) && fromStack == null)
            {
                fromStack = GetStackFromDisk();
                if (fromStack != null)
                {
                    selectDisk = fromStack.Peek();
                    initial = selectDisk.gameObject.transform.position;
                }
            }
            if (Input.GetMouseButton(0) && fromStack != null)
            {
                Vector3 loc = Input.mousePosition;
                loc.z = selectDisk.gameObject.transform.position.z;
                loc = Camera.main.ScreenToWorldPoint(loc);
                loc.z = selectDisk.gameObject.transform.position.z;
                selectDisk.gameObject.transform.position = loc;
            }
            if (!Input.GetMouseButton(0) && fromStack != null)
            {
                targetStack = GetStackFromPeg();
                if (targetStack != null && targetStack != fromStack && AllowedMove(fromStack,targetStack))
                {
                    if(AllowedMove(fromStack, targetStack))
                    {
                        MoveDisk(fromStack, targetStack);
                        IncrementMove();
                        fromStack = null; targetStack = null; selectDisk = null;
                    }
                }
                else
                {
                    selectDisk.gameObject.transform.position = initial;
                    fromStack = null; targetStack = null; selectDisk = null; 
                }
            }

        }

    }

    public void BottomToCenter(GameObject g)
    {
        Vector3 tempBase = g.transform.position;
        Vector3 btm = tempBase; 
        btm.y = g.GetComponent<SpriteRenderer>().bounds.min.y;
        g.transform.position += tempBase - btm;
    }

    public Vector3 GetTop(GameObject g)
    {
        Vector3 v = g.transform.position;
        v.y = g.GetComponent<SpriteRenderer>().bounds.max.y;
        return v;
    }

    public Vector3 GetBottom(GameObject g)
    {
        Vector3 v = g.transform.position;
        v.y = g.GetComponent<SpriteRenderer>().bounds.min.y;
        return v;
    }



    public bool AllowedMove(Stack<HanoiDisk> former, Stack<HanoiDisk> target)
    {
        if (target.Count == 0)
            return true;
        else
            return former.Peek().GetRank() < target.Peek().GetRank();
    }

    public void MoveDisk(Stack<HanoiDisk> former, Stack<HanoiDisk> target)
    {
        // todo - try catch?



        HanoiDisk h = former.Pop();
        HanoiDisk top;
        if (target.Count == 0)
        {
            GameObject g;
            stackDict.TryGetValue(target, out g);
            h.gameObject.transform.position = GetBottom(g);
        }
        else
        {
            top = target.Peek();
            h.gameObject.transform.position = GetTop(top.gameObject);
        }
        target.Push(h);
        BottomToCenter(h.gameObject);
        h.gameObject.transform.position += Vector3.back;
    }

    public bool WonGame()
    {
        return rPeg.Count == noDisks;
    }

    public Stack<HanoiDisk> GetStackFromDisk()
    {

        foreach (KeyValuePair<Stack<HanoiDisk>, GameObject> p in stackDict)
        {
            if (p.Key.Count > 0)
            {
                Vector3 loc = Input.mousePosition;
                Bounds b = p.Key.Peek().gameObject.GetComponent<SpriteRenderer>().bounds;
                loc.z = b.center.z;
                loc = Camera.main.ScreenToWorldPoint(loc);
                loc.z = b.center.z;
                if (b.Contains(loc))
                    return p.Key;
            }
        }
        return null;
    }

    public Stack<HanoiDisk> GetStackFromPeg()
    {
        foreach (KeyValuePair<Stack<HanoiDisk>, GameObject> p in stackDict)
        {
            Vector3 loc = Input.mousePosition;
            Bounds b = p.Value.GetComponent<SpriteRenderer>().bounds;
            loc.z = b.center.z;
            loc = Camera.main.ScreenToWorldPoint(loc);
            loc.z = b.center.z;
            if (b.Contains(loc))
                return p.Key;
        }
        return null;
    }

    public void IncrementMove()
    {
        moveCount += 1;
        counterText.text = moveCount.ToString();
    }

}
