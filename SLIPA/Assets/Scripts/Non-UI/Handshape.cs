using System.Collections;
using System.Collections.Generic;

/// <summary>
///   <para>Objects of this class represent individual handshapes, and they store
///   information that <see cref="AvatarAnimator"/> can use to animate.</para>
/// </summary>
public class Handshape : System.ICloneable
{
    public enum Finger { Thumb, Index, Middle, Ring, Little }
    public static Finger Thumb = Finger.Thumb;
    public static Finger Index = Finger.Index;
    public static Finger Middle = Finger.Middle;
    public static Finger Ring = Finger.Ring;
    public static Finger Little = Finger.Little;
    public static string[] fingerNames = System.Enum.GetNames(typeof(Handshape.Finger));

    private Dictionary<Finger, int> fingerIndexDict = new Dictionary<Finger, int>
    {
        { Thumb, 0 }, { Index, 1 }, { Middle, 2 }, { Ring, 3 }, { Little, 4 }
    };

    public float[,] FingerProperties { get; set; }
    /// <summary>
    ///   <para>The properties of the <see cref="Handshape"/>'s thumb.</para>
    /// </summary>
    public float[] ThumbProperties
    {
        get => GetFingerProperties(Thumb);
        set
        {
            if (value.Length == FingerProperties.GetLength(1))
            {
                SetFingerProperties(Thumb, value);
            }
        }
    }
    /// <summary>
    ///   <para>The properties of the <see cref="Handshape"/>'s index finger.</para>
    /// </summary>
    public float[] IndexProperties
    {
        get => GetFingerProperties(Index);
        set
        {
            if (value.Length == FingerProperties.GetLength(1))
            {
                SetFingerProperties(Index, value);
            }
        }
    }
    /// <summary>
    ///   <para>The properties of the <see cref="Handshape"/>'s middle finger.</para>
    /// </summary>
    public float[] MiddleProperties
    {
        get => GetFingerProperties(Middle);
        set
        {
            if (value.Length == FingerProperties.GetLength(1))
            {
                SetFingerProperties(Middle, value);
            }
        }
    }
    /// <summary>
    ///   <para>The properties of the <see cref="Handshape"/>'s ring finger.</para>
    /// </summary>
    public float[] RingProperties
    {
        get => GetFingerProperties(Ring);
        set
        {
            if (value.Length == FingerProperties.GetLength(1))
            {
                SetFingerProperties(Ring, value);
            }
        }
    }
    /// <summary>
    ///   <para>The properties of the <see cref="Handshape"/>'s little finger.</para>
    /// </summary>
    public float[] LittleProperties
    {
        get => GetFingerProperties(Little);
        set
        {
            if (value.Length == FingerProperties.GetLength(1))
            {
                SetFingerProperties(Little, value);
            }
        }
    }
    /// <summary>
    ///   <para>The properties of the <see cref="Handshape"/>'s non-thumb fingers.</para>
    /// </summary>
    public float[] NonThumbProperties
    {
        set
        {
            if (value.Length == FingerProperties.GetLength(1))
            {
                SetFingerProperties(Index, value);
                SetFingerProperties(Middle, value);
                SetFingerProperties(Ring, value);
                SetFingerProperties(Little, value);
            }
        }
    }

    public Handshape() : this(new float[5, 4])
    {
    }

    public Handshape(float[,] fingerProperties)
    {
        if (fingerProperties.Rank != 2 || fingerProperties.GetLength(0) != 5 ||
            fingerProperties.GetLength(1) != 4)
        {
            throw new System.Exception("Error: fingerProperties does not have the appropriate dimensions.");
        }
        FingerProperties = fingerProperties;
    }

    public Handshape(float[] thumbProperties, float[] indexProperties,
        float[] middleProperties, float[] ringProperties, float[] littleProperties)
    {
        if (thumbProperties.Length != 4)
        {
            throw new System.Exception("thumbProperties must have 4 elements.");
        }
        else if (indexProperties.Length != 4)
        {
            throw new System.Exception("indexProperties must have 4 elements.");
        }
        else if (middleProperties.Length != 4)
        {
            throw new System.Exception("middleProperties must have 4 elements.");
        }
        else if (ringProperties.Length != 4)
        {
            throw new System.Exception("ringProperties must have 4 elements.");
        }
        else if (littleProperties.Length != 4)
        {
            throw new System.Exception("littleProperties must have 4 elements.");
        }
        FingerProperties = new float[5, 4];
        for (int i = 0; i < FingerProperties.GetLength(1); i++)
        {
            FingerProperties[0, i] = thumbProperties[i];
            FingerProperties[1, i] = indexProperties[i];
            FingerProperties[2, i] = middleProperties[i];
            FingerProperties[3, i] = ringProperties[i];
            FingerProperties[4, i] = littleProperties[i];
        }
    }

    public Handshape(float[] thumbProperties, float[] otherProperties) :
        this(thumbProperties, otherProperties, otherProperties, otherProperties, otherProperties)
    {
    }

    /// <summary>Creates a copy of a <see cref="Handshape"/>.</summary>
    /// <returns>
    ///   A <see cref="Handshape"/> whose properties are identical to the one being cloned.
    /// </returns>
    public object Clone()
    {
        return new Handshape(FingerProperties.Clone() as float[,]);
    }

    public float[] GetFingerProperties(Finger finger)
    {
        int fingerIndex = fingerIndexDict[finger];
        float[] row = new float[FingerProperties.GetLength(1)];
        for (int i = 0; i < FingerProperties.GetLength(1); i++)
        {
            row[i] = FingerProperties[fingerIndex, i];
        }
        return row;
    }

    public void SetFingerProperties(Finger finger, float[] properties)
    {
        int fingerIndex = fingerIndexDict[finger];
        for (int i = 0; i < properties.Length; i++)
        {
            FingerProperties[fingerIndex, i] = properties[i];
        }
    }

}
