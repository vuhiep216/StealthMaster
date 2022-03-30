using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharactorAnimController : MonoBehaviour
{
    Animator _animator;
    [SerializeField] List<Rigidbody> rigs;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        RagdollSetUp();
    }
    private void RagdollSetUp()
    {
        rigs = GetComponentsInChildren<Rigidbody>().ToList();
        foreach(var r in rigs)
        {
            r.isKinematic = true;
        }
    }
    public void ChangeAnimIdle()
    {
        _animator.SetTrigger("Idle");
    }
    public void ChangeAnimWalk()
    {
        _animator.SetTrigger("Walk");
    }
    public void ChangeAnimRun()
    {
        _animator.SetTrigger("Run");
    }
    public void ChangeAnimAttack()
    {
        _animator.SetTrigger("Attack");
    }
    public void ChangeAnimDie()
    {
        _animator.enabled = false;
        foreach (var r in rigs)
        {
            r.isKinematic = false;
            r.AddForce(GamePlay.Instance.playerController.transform.forward * 500f);
        }
    }
    public void ChangeAnimVictory()
    {
        _animator.SetTrigger("Win");
    }
}
