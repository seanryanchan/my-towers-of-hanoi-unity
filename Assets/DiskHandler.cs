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

    void Start()
    {
        HanoiDisk h = new HanoiDisk(1, Vector2.zero, disk, leftPeg.transform.position, middlePeg.transform.position, rightPeg.transform.position, drawMode);
        HanoiDisk h2 = new HanoiDisk(2, Vector2.up, disk, leftPeg.transform.position, middlePeg.transform.position, rightPeg.transform.position, drawMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
