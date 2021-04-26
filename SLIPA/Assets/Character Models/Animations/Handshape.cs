using System.Collections;
using System.Collections.Generic;

public class Handshape : System.ICloneable
{
    public enum Finger { Thumb, Index, Middle, Ring, Little }
    public static Finger[] fingers = System.Enum.GetValues(typeof(Handshape.Finger)) as Finger[];
    public static string[] fingerNames = System.Enum.GetNames(typeof(Handshape.Finger));

    private Dictionary<Finger, int> fingerIndexDict = new Dictionary<Finger, int>
    {
        { Finger.Thumb,  0 },
        { Finger.Index,  1 },
        { Finger.Middle, 2 },
        { Finger.Ring,   3 },
        { Finger.Little, 4 }
    };

    public float[,] FingerProperties { get; set; }
    public float[] ThumbProperties
    {
        get => GetFingerProperties(Finger.Thumb);
        set
        {
            if (value.Length == this.FingerProperties.GetLength(1))
            {
                SetFingerProperties(Finger.Thumb, value);
            }
        }
    }
    public float[] IndexProperties
    {
        get => GetFingerProperties(Finger.Index);
        set
        {
            if (value.Length == this.FingerProperties.GetLength(1))
            {
                SetFingerProperties(Finger.Index, value);
            }
        }
    }
    public float[] MiddleProperties
    {
        get => GetFingerProperties(Finger.Middle);
        set
        {
            if (value.Length == this.FingerProperties.GetLength(1))
            {
                SetFingerProperties(Finger.Middle, value);
            }
        }
    }
    public float[] RingProperties
    {
        get => GetFingerProperties(Finger.Ring);
        set
        {
            if (value.Length == this.FingerProperties.GetLength(1))
            {
                SetFingerProperties(Finger.Ring, value);
            }
        }
    }
    public float[] LittleProperties
    {
        get => GetFingerProperties(Finger.Little);
        set
        {
            if (value.Length == this.FingerProperties.GetLength(1))
            {
                SetFingerProperties(Finger.Little, value);
            }
        }
    }
    public float[] NonThumbProperties
    {
        set
        {
            if (value.Length == this.FingerProperties.GetLength(1))
            {
                SetFingerProperties(Finger.Index, value);
                SetFingerProperties(Finger.Middle, value);
                SetFingerProperties(Finger.Ring, value);
                SetFingerProperties(Finger.Little, value);
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
            throw new System.Exception("Error: fingerProperties does not have the appropriate dimensions");
        }
        this.FingerProperties = fingerProperties;
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
        this.FingerProperties = new float[5, 4];
        for (int i = 0; i < this.FingerProperties.GetLength(1); i++)
        {
            this.FingerProperties[0, i] = thumbProperties[i];
            this.FingerProperties[1, i] = indexProperties[i];
            this.FingerProperties[2, i] = middleProperties[i];
            this.FingerProperties[3, i] = ringProperties[i];
            this.FingerProperties[4, i] = littleProperties[i];
        }
    }

    public Handshape(float[] thumbProperties, float[] otherProperties) :
        this(thumbProperties, otherProperties, otherProperties, otherProperties, otherProperties)
    {
    }

    /// <summary>
    /// Creates a copy of the handshape.
    /// </summary>
    /// <returns>A handshape whose properties are identical to the one being
    /// cloned.</returns>
    public object Clone()
    {
        return new Handshape(this.FingerProperties.Clone() as float[,]);
    }

    public float[] GetFingerProperties(Finger finger)
    {
        int fingerIndex = fingerIndexDict[finger];
        float[] row = new float[this.FingerProperties.GetLength(1)];
        for (int i = 0; i < this.FingerProperties.GetLength(1); i++)
        {
            row[i] = this.FingerProperties[fingerIndex, i];
        }
        return row;
    }

    public void SetFingerProperties(Finger finger, float[] properties)
    {
        int fingerIndex = fingerIndexDict[finger];
        for (int i = 0; i < properties.Length; i++)
        {
            this.FingerProperties[fingerIndex, i] = properties[i];
        }
    }

}
