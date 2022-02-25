using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnglesTool 
{
    /// <summary>
    /// Convert a angle from Degree To Radians
    /// </summary>
    /// <param name="degree"> the angle in degree</param>
    /// <returns> the value of the angle in radian </returns>
    public static float ConvertDegreeToRadians(float degree)
    {
        return degree * Mathf.Deg2Rad;
    }

    /// <summary>
    /// Convert a angle from Radians to Degree
    /// </summary>
    /// <param name="radians"> the angle in radian </param>
    /// <returns> the value of the angle in degree </returns>
    public static float ConvertRadiansToDegree(float radians)
    {
        return radians * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Get the position with a decal of angle degree
    /// </summary>
    /// <param name="pos"> A position </param>
    /// <param name="angle"> the angle decal </param>
    /// <returns>The new position</returns>
    public static Vector3 GetDecalByAngle(Vector3 pos, float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        return rotation * pos;
    }

    /// <summary>
    ///  Return the rotation
    /// </summary>
    /// <param name="angle"> angle </param>
    /// <returns> </returns>
    public static Quaternion GetStarDecalRotation(float angle)
    {
        Quaternion rotation = Quaternion.identity;
        return Quaternion.Euler(0, 0, angle);
    }

    public static Quaternion GetInverseStarDecalRotation(float angle)
    {
        Quaternion rotation = Quaternion.identity;
        return Quaternion.Euler(0, 0, -angle);
    }

    public static Vector3 GetStarDecalPositionByVector3(Vector3 newPos, Quaternion rotation)
    {
        return rotation * newPos;
    }

}
