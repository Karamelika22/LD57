using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public static PlayerAnim Instance { get; private set; }
    private Animator anim;
    void Awake()
    {
        if (Instance == null) Instance = this;
        anim= GetComponent<Animator>();
    }

    public void PlaySelectAnimation()
    {
        anim.SetTrigger("Select");
    }

    public void PlayWalkAnimation(bool isWalking)
    {
        anim.SetBool("IsWalking", isWalking);
    }

    public void PlayClimbAnimation(bool isClimbing)
    {
        anim.SetBool("IsClimbing", isClimbing);
    }

    public void PlayAttackAnimation()
    {
        anim.SetTrigger("Attack");
    }

    public void PlayDeathAnimation()
    {
        anim.SetTrigger("Die");
    }

    public void PlayCraftAnimation()
    {
        anim.SetTrigger("Craft");
    }

    // Пустая заготовка
    public void CustomAction()
    {
        // Заглушка для будущего действия
    }
}
