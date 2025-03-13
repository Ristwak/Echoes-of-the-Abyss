using System.Collections.Generic;
using UnityEngine;

public class MorgueBox : MonoBehaviour
{
    private Transform player;
    public Animator[] door;
    public Animator[] bed;
    private bool isDoorOpen;
    void Start()
    {
        isDoorOpen = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void PlayAnim(string name)
    {
        if (!isDoorOpen)
        {
            foreach (Animator anim in door)
            {
                if (anim.gameObject.name == name)
                {
                    anim.Play("Opening");
                    isDoorOpen = true;
                }
            }
        }
        else
        {
            foreach (Animator anim in door)
            {
                if (anim.gameObject.name == name)
                {
                    anim.Play("Closing");
                    isDoorOpen = false;
                }
            }
        }
    }
}
