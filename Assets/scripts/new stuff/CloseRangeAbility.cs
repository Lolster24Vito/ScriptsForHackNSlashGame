using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability Events/Ranged Projectile")]

public class CloseRangeAbility : Ability
{
    [SerializeField] Vector2 colliderSize;
    [SerializeField] Vector2 colliderOffset;

    [SerializeField] GameObject abiliteffecty;

    [SerializeField] int abilityDamage;


    private RotatorOfCollider rot;
    private Animator animator;
    private Transform pos;
    float timerHelper;



    public override void Initialize(GameObject obj)
    {
        rot = obj.transform.GetChild(0).GetComponent<RotatorOfCollider>();
        animator = obj.GetComponent<Animator>();
        pos = rot.transform.GetChild(1).transform;

    }
    public override void Begin(GameObject obj)
    {
        //Set animation parameters
        if (abilityAnimatorParametersForCast != null)
            for (int i = 0; i < abilityAnimatorParametersForCast.Length; i++)
            {
                animator.SetBool(abilityAnimatorParametersForCast[i].ParameterName, abilityAnimatorParametersForCast[i].Value);
                //   Debug.Log("Here on the end Right now "+i);

            }
        obj.GetComponent<AbilityController>().isCasting = true;
        Vector2 mouseDir = GetMouseDir(pos.position);
        animator.SetFloat("AttackDirX", mouseDir.x);
        animator.SetFloat("AttackDirY", mouseDir.y);
    }

    public override void AbilityFunction()
    {
        rot.RotateCollider();
       

        pos = rot.transform.GetChild(1).transform;

        Collider2D[] col = Physics2D.OverlapBoxAll(pos.position, colliderSize, pos.eulerAngles.z);
        Instantiate(abiliteffecty, pos.position, pos.rotation);

        if (col != null)
        {
            foreach(Collider2D c in col)
            {
                //Do Damage to enemy
            }
        }



    }
    public override void End(GameObject obj)
    {
        //Set animation parameters
        if (abilityAnimatorParametersForCast != null)
            for (int i = 0; i < abilityAnimatorParametersForOver.Length; i++)
            {
                animator.SetBool(abilityAnimatorParametersForOver[i].ParameterName, abilityAnimatorParametersForOver[i].Value);
                //   Debug.Log("Here on the end Right now "+i);

            }
        obj.GetComponent<AbilityController>().isCasting = false;

    }

    Vector2 GetMouseDir(Vector3 pos)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = new Vector2(mousePos.x - pos.x, mousePos.y - pos.y).normalized;
        // Debug.Log(mouseDir+"Old");

        //  Debug.Log(mouseDir + "new");

        return mouseDir;
      //  Debug.Log(mouseDir);

    }
}
