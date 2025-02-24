using UnityEngine;

public class MetallicDoor : MonoBehaviour
{
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;
    private bool isLeftDoorOpen;
    private bool isRightDoorOpen;
    void Start()
    {
        isLeftDoorOpen = false;
        isRightDoorOpen = false;
    }

    public void LeftDoor()
    {
        if (leftDoorAnimator != null && !isLeftDoorOpen)
        {
            leftDoorAnimator.Play("LeftDoorOpening");
            isLeftDoorOpen = true;
        }
        else
        {
            leftDoorAnimator.Play("LeftDoorClosing");
            isLeftDoorOpen = false;
        }
    }

    public void RightDoor()
    {
        if (rightDoorAnimator != null && !isRightDoorOpen)
        {
            rightDoorAnimator.Play("RightDoorOpening");
            isRightDoorOpen = true;
        }
        else
        {
            rightDoorAnimator.Play("RightDoorClosing");
            isRightDoorOpen = false;
        }
    }
}
