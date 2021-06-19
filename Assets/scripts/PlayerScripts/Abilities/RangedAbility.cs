using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability Events/Ranged Projectile")]

public class RangedAbility : Ability
{
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject abiliteffecty;

    [SerializeField] float projectileSpeed;
    [SerializeField] int projectileDamage;
    private RotatorOfCollider rot;
    private Animator animator;
    private Transform pos;



    public override void Initialize(GameObject obj)
    {
        rot = obj.transform.GetChild(0).GetComponent<RotatorOfCollider>();
        animator = obj.GetComponent<Animator>();
        pos = rot.transform.GetChild(1).transform;

    }
    public override void Begin(GameObject obj)
    {
        //Set animation parameters
        if(abilityAnimatorParametersForCast != null)
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
        Instantiate(abiliteffecty, pos.position, pos.rotation);

        pos = rot.transform.GetChild(1).transform;

        GameObject projectil = Instantiate(projectile, pos.position, Quaternion.identity);
        projectil.GetComponent<Projectile>().dir = GetMouseDir(pos.position);
        projectil.GetComponent<Projectile>().speed = projectileSpeed;
        projectil.GetComponent<Projectile>().damage = projectileDamage;

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

    }
}
