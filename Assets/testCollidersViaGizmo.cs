using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCollidersViaGizmo : MonoBehaviour
{
    [SerializeField] Vector2 colliderSizeForTestingInEditor;
    [SerializeField] Vector3 OffsetColliderForTestingInEditor;
    [SerializeField] Transform ColliderTransformForTestingInEditor;


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(ColliderTransformForTestingInEditor.position + OffsetColliderForTestingInEditor, colliderSizeForTestingInEditor);
        //  Gizmos.DrawWireCube(idkkk.position , chargeCollider);

    }
}
