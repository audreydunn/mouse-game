
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCommon
{
    public static bool CheckGroundNear(
        Vector3 charPos,
        float jumpableGroundNormalMaxAngle,
        float rayDepth, //how far down from charPos will we look for ground?
        float rayOriginOffset, //charPos near bottom of collider, so need a fudge factor up away from there
        out bool isJumpable
    )
    {

        bool ret = false;
        bool _isJumpable = false;


        float totalRayLen = rayOriginOffset + rayDepth;

        Ray ray = new Ray(charPos + Vector3.up * rayOriginOffset, Vector3.down);

        int layerMask = 1 << LayerMask.NameToLayer("Default");


        RaycastHit[] hits = Physics.RaycastAll(ray, totalRayLen, layerMask);

        RaycastHit groundHit = new RaycastHit();

        foreach (RaycastHit hit in hits)
        {

            if (hit.collider.gameObject.CompareTag("ground"))
            {

                ret = true;

                groundHit = hit;

                _isJumpable = Vector3.Angle(Vector3.up, hit.normal) < jumpableGroundNormalMaxAngle;

                break; //only need to find the ground once

            }

        }

        DrawRay(ray, totalRayLen, hits.Length > 0, groundHit, Color.magenta, Color.green);

        isJumpable = _isJumpable;

        return ret;
    }

    public static void DrawRay(Ray ray, float rayLength, bool hitFound, RaycastHit hit, Color rayColor, Color hitColor)
    {

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * rayLength, rayColor);

        if (hitFound)
        {
            //draw an X that denotes where ray hit
            const float ZBufFix = 0.01f;
            const float edgeSize = 0.2f;
            Color col = hitColor;

            Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.forward * edgeSize, col);
            Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.left * edgeSize, col);
            Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.right * edgeSize, col);
            Debug.DrawRay(hit.point + Vector3.up * ZBufFix, Vector3.back * edgeSize, col);
        }
    }
}
