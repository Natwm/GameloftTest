
using UnityEngine;

public static class TransformExtension
{
    #region On X vector
    /// <summary>
	/// Create a new Transform from the given Transform with a new value for its x coordinate.
	/// </summary>
	/// <param name="t">The source Transform</param>
	/// <param name="x">The new x value</param>
	/// <returns>A new Transform</returns>
    public static Transform ChangeX( this Transform t, float x)
    {
        t.position = new Vector3(x, t.position.y, t.position.z);
        return t;
    }

    /// <summary>
	/// Create a new Transform from the given Transform with a value added to its x coordinate.
	/// </summary>
	/// <param name="t">The source Transform</param>
	/// <param name="x">The value added to the x coordinate</param>
	/// <returns>A new Transform</returns>
    public static Transform IncreaseX(this Transform t, float x)
    {
        t.position = new Vector3(t.position.x + x, t.position.y, t.position.z);
        return t;
    }

    /// <summary>
	/// Create a new Transform from the given Transform with its x coordinate multiplied.
	/// </summary>
	/// <param name="t">The source Transform</param>
	/// <param name="x">The multiplier value</param>
	/// <returns>A new Transform</returns>
    public static Transform multiplyX(this Transform t, float x)
    {
        t.position = new Vector3(t.position.x * x, t.position.y, t.position.z);
        return t;
    }

    #endregion

    #region On Y vector
    /// <summary>
	/// Create a new Transform from the given Transform with a new value for its y coordinate.
	/// </summary>
	/// <param name="t">The source Transform</param>
	/// <param name="y">The new y value</param>
	/// <returns>A new Transform</returns>
    public static Transform ChangeY(this Transform t, float y)
    {
        t.position = new Vector3(t.position.x,y, t.position.z);
        return t;
    }

    /// <summary>
	/// Create a new Transform from the given Transform with a value added to its y coordinate.
	/// </summary>
	/// <param name="t">The source Transform</param>
	/// <param name="y">The value added to the y coordinate</param>
	/// <returns>A new Transform</returns>
    public static Transform IncreaseY(this Transform t, float y)
    {
        t.position = new Vector3(t.position.x, t.position.y + y, t.position.z);
        return t;
    }

    /// <summary>
	/// Create a new Transform from the given Transform with its y coordinate multiplied.
	/// </summary>
	/// <param name="t">The source Transform</param>
	/// <param name="y">The multiplier value</param>
	/// <returns>A new Transform</returns>
    public static Transform multiplyY(this Transform t, float y)
    {
        t.position = new Vector3(t.position.x, t.position.y * y, t.position.z);
        return t;
    }

    #endregion

    #region On Z vector
    /// <summary>
	/// Create a new Transform from the given Transform with a new value for its z coordinate.
	/// </summary>
	/// <param name="t">The source Transform</param>
	/// <param name="z">The new z value</param>
	/// <returns>A new Transform</returns>
    public static Transform ChangeZ(this Transform t, float z)
    {
        t.position = new Vector3(t.position.x, t.position.y, z);
        return t;
    }

    /// <summary>
	/// Create a new Transform from the given Transform with a value added to its z coordinate.
	/// </summary>
	/// <param name="t">The source Transform</param>
	/// <param name="z">The value added to the z coordinate</param>
	/// <returns>A new Transform</returns>
    public static Transform IncreaseZ(this Transform t, float z)
    {
        t.position = new Vector3(t.position.x, t.position.y, t.position.z + z);
        return t;
    }

    /// <summary>
	/// Create a new Transform from the given Transform with its z coordinate multiplied.
	/// </summary>
	/// <param name="t">The source Transform</param>
	/// <param name="z">The multiplier value</param>
	/// <returns>A new Transform</returns>
    public static Transform multiplyZ(this Transform t, float z)
    {
        t.position = new Vector3(t.position.x, t.position.y, t.position.z * z);
        return t;
    }

    #endregion

}
