using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskHandler : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    public Sprite disk;

    [SerializeField]
    public GameObject leftPeg;

    [SerializeField]
    public GameObject middlePeg;

    [SerializeField]
    public GameObject rightPeg;

    [SerializeField]
    public Sprite peg;

    [SerializeField]
    public SpriteDrawMode drawMode = SpriteDrawMode.Sliced;

    [SerializeField]
    public GameObject gameObject;

    public List<HanoiDisk> lPeg;
    public List<HanoiDisk> mPeg;
    public List<HanoiDisk> rPeg;


    void Start()
    {
        // initialize disks.
        int noDisks = PlayerPrefs.GetInt("no_disks");
        for (int i =1; i < noDisks + 1; i++)
        {
            lPeg.Add(new HanoiDisk(noDisks - i, leftPeg.transform.position + Vector3.up * (i-1), disk, leftPeg.transform.position, middlePeg.transform.position, rightPeg.transform.position, drawMode));
        }

        // initialize pegs.


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
