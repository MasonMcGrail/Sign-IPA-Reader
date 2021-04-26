using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///   <para>This class contains all of the variables in FingerMover that are
///   static and read-only. Many of these are dictionaries which are manually
///   instantied and thus take up a lot of space in a file.</para>
/// </summary>
public sealed class Variables
{
    // The following four lines implement a singleton design pattern.
    private static readonly Variables instance = new Variables();
    static Variables() { }
    private Variables() { }
    public static Variables Instance { get => instance; }

    // Sides of the body, in this case only left and right.
    public enum Side { Left, Right }
    /// <summary>
    ///   <para>The enum <see cref="Side.Left"/> in <see cref="Variables"/>.</para>
    /// </summary>
    public static Side Left { get => Side.Left; }
    /// <summary>
    ///   <para>The enum <see cref="Side.Right"/> in <see cref="Variables"/>.</para>
    /// </summary>
    public static Side Right { get => Side.Right; }

    private static string[] sideNames = null;
    /// <summary>
	///   <para>Names of the constants in <see cref="Side"/>, which are only "Left" and "Right".</para>
	/// </summary>
    public static string[] SideNames
    {
        get
        {
            if (sideNames == null)
            {
                sideNames = System.Enum.GetNames(typeof(Side));
            }
            return sideNames;
        }
    }

    private static int numSides = 0;
    /// <summary>
	///   <para>The number of constants in <see cref="Side"/>, which is only 2.</para>
	/// </summary>
    public static int NumSides
    {
        get
        {
            if (numSides == 0)
            {
                numSides = System.Enum.GetNames(typeof(Side)).Length;
            }
            return numSides;
        }
    }

    private static Dictionary<string, string> placeNamesDict = null;
    /// <summary>
	///   <para>A dictionary where its keys in this dictionary are the names in
    ///   <see cref="HumanTrait.MuscleName"/>, and their corresponding values are the names
    ///   of those muscles in the <see cref="Animator"/>.</para>
	/// </summary>
    /// <remarks>
    ///   When Unity updates to C# 8.0, can be greatly simplified with the ??= operator.
    /// </remarks>
    public static Dictionary<string, string> PlaceNamesDict
    {
        get
        {
            if (placeNamesDict == null)
            {
                placeNamesDict = InitPlaceNamesDict();
            }
            return placeNamesDict;
        }
    }

    private static Dictionary<string, Handshape> handshapeDict = null;
    /// <summary>
	///   <para>A dictionary where its keys are symbols for handshapes in Sign IPA
    ///   and their corresponding values are their corresponding <see cref="Handshape"/> objects.</para>
	/// </summary>
    /// <remarks>
    ///   When Unity updates to C# 8.0, can be greatly simplified with the ??= operator.
    /// </remarks>
    public static Dictionary<string, Handshape> HandshapeDict
    {
        get
        {
            if (handshapeDict == null)
            {
                handshapeDict = InitHandShapeDict();
            }
            return handshapeDict;
        }
    }

    private static Dictionary<string, HumanBodyBones> symbolBoneDict = null;
    /// <summary>
	///   <para>A dictionary where its keys are symbols for places in Sign IPA
    ///   and their corresponding values are bones in <see cref="HumanBodyBones"/>
    ///   that are used as a reference point for defining places on the avatar.
    ///     <br>
    ///       It is used with <see cref="PlaceValueOffsets"/> to get the actual place positions.
    ///     </br>
    ///   </para>
	/// </summary>
    /// <remarks>
    ///   When Unity updates to C# 8.0, can be greatly simplified with the ??= operator.
    /// </remarks>
    public static Dictionary<string, HumanBodyBones> SymbolBoneDict
    {
        get
        {
            if (symbolBoneDict == null)
            {
                symbolBoneDict = InitSymbolBoneDict();
            }
            return symbolBoneDict;
        }
    }

    private static Dictionary<string, (Vector3, Vector3?)> placeValueOffsets = null;
    /// <summary>
	///   <para>A dictionary where its keys are symbols for places in Sign IPA
    ///   and their corresponding values are offets in position from the reference
    ///   bones in <see cref="SymbolBoneDict"/>.</para>
	/// </summary>
    /// <remarks>
    ///   When Unity updates to C# 8.0, can be greatly simplified with the ??= operator.
    /// </remarks>
    public static Dictionary<string, (Vector3, Vector3?)> PlaceValueOffsets
    {
        get
        {
            if (placeValueOffsets == null)
            {
                placeValueOffsets = InitPlaceValueOffsets();
            }
            return placeValueOffsets;
        }
    }

    private static string regexPattern = null;
    /// <summary>
	///   <para>A regex pattern of valid input for text entered in Sign IPA.</para>
	/// </summary>
    /// <remarks>
    ///   When Unity updates to C# 8.0, can be greatly simplified with the ??= operator.
    /// </remarks>
    public static string RegexPattern
    {
        get
        {
            if (regexPattern == null)
            {
                regexPattern = InitRegexPattern();
            }
            return regexPattern;
        }
    }

    /// <summary>
	///   <para>Initializes <see cref="PlaceNamesDict"/>.</para>
	/// </summary>
    private static Dictionary<string, string> InitPlaceNamesDict()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();

        for (int i = 0; i < numSides; i++)
        {
            // fingers
            dict.Add(SideNames[i] + " Thumb 1 Stretched", SideNames[i] + "Hand.Thumb.1 Stretched");
            dict.Add(SideNames[i] + " Thumb 2 Stretched", SideNames[i] + "Hand.Thumb.2 Stretched");
            dict.Add(SideNames[i] + " Thumb 3 Stretched", SideNames[i] + "Hand.Thumb.3 Stretched");
            dict.Add(SideNames[i] + " Thumb Spread", SideNames[i] + "Hand.Thumb.Spread");
            dict.Add(SideNames[i] + " Index 1 Stretched", SideNames[i] + "Hand.Index.1 Stretched");
            dict.Add(SideNames[i] + " Index 2 Stretched", SideNames[i] + "Hand.Index.2 Stretched");
            dict.Add(SideNames[i] + " Index 3 Stretched", SideNames[i] + "Hand.Index.3 Stretched");
            dict.Add(SideNames[i] + " Index Spread", SideNames[i] + "Hand.Index.Spread");
            dict.Add(SideNames[i] + " Middle 1 Stretched", SideNames[i] + "Hand.Middle.1 Stretched");
            dict.Add(SideNames[i] + " Middle 2 Stretched", SideNames[i] + "Hand.Middle.2 Stretched");
            dict.Add(SideNames[i] + " Middle 3 Stretched", SideNames[i] + "Hand.Middle.3 Stretched");
            dict.Add(SideNames[i] + " Middle Spread", SideNames[i] + "Hand.Middle.Spread");
            dict.Add(SideNames[i] + " Ring 1 Stretched", SideNames[i] + "Hand.Ring.1 Stretched");
            dict.Add(SideNames[i] + " Ring 2 Stretched", SideNames[i] + "Hand.Ring.2 Stretched");
            dict.Add(SideNames[i] + " Ring 3 Stretched", SideNames[i] + "Hand.Ring.3 Stretched");
            dict.Add(SideNames[i] + " Ring Spread", SideNames[i] + "Hand.Ring.Spread");
            dict.Add(SideNames[i] + " Little 1 Stretched", SideNames[i] + "Hand.Little.1 Stretched");
            dict.Add(SideNames[i] + " Little 2 Stretched", SideNames[i] + "Hand.Little.2 Stretched");
            dict.Add(SideNames[i] + " Little 3 Stretched", SideNames[i] + "Hand.Little.3 Stretched");
            dict.Add(SideNames[i] + " Little Spread", SideNames[i] + "Hand.Little.Spread");
            // hand
            dict.Add(SideNames[i] + " Hand Down-Up", SideNames[i] + " Hand Down-Up");
            //dict.Add(SideNames[i] + " Hand In-Out", SideNames[i] + " Hand In-Out");
        }

        return dict;
    }

    /// <summary>
	///   <para>Initializes <see cref="HandshapeDict"/>.</para>
	/// </summary>
    private static Dictionary<string, Handshape> InitHandShapeDict()
    {
        // The values of the proximal, intermediate, and dorsal segments of a
        // finger when it is curled in on itself.
        float[] curledIn = new float[] { -1f, -1.3f, -1.6f };
        // The values of the proximal, intermediate, and dorsal segments of a
        // finger when it is basically straight.
        float[] straight = new float[] { 0.4f, 0.8f, 0.8f };

        // The model is initialized so that it begins in standard anatomical position.
        Dictionary<string, Handshape> dict = new Dictionary<string, Handshape>
        {
            // Open handshapes
            { "A",
                new Handshape (new float[] {   -2f,  0.4f,  0.3f,    0f }, //thumb
                               new float[] { curledIn[0], curledIn[1], curledIn[2], -0.5f }) //others
            },
            { "I",
                new Handshape (new float[,] {
                    {   -3f,  0.4f,  0.3f,    0f }, //thumb
                    { straight[0], straight[1], straight[2],    0f }, //index
                    { curledIn[0], curledIn[1], curledIn[2], -0.5f }, //middle
                    { curledIn[0], curledIn[1], curledIn[2], -0.5f }, //ring
                    { curledIn[0], curledIn[1], curledIn[2], -0.5f }  //little
                })
            },
            { "U",
                new Handshape (new float[,] {
                    {   -2f,  0.4f,  0.3f,  0.5f }, //thumb
                    { straight[0], straight[1], straight[2], -0.5f }, //index
                    { straight[0], straight[1], straight[2], -0.5f }, //middle
                    { curledIn[0], curledIn[1], curledIn[2],    0f }, //ring
                    { curledIn[0], curledIn[1], curledIn[2],    0f }  //little
                })
            },
            { "Y",
                new Handshape (new float[,] {
                    {   -2f,  0.4f,  0.3f,  0.5f }, //thumb
                    { straight[0], straight[1], straight[2],    0f }, //index
                    { straight[0], straight[1], straight[2],   -1f }, //middle
                    { curledIn[0], curledIn[1], curledIn[2],    0f }, //ring
                    { curledIn[0], curledIn[1], curledIn[2],    0f }  //little
                })
            },
            { "E",
                new Handshape (new float[,] {
                    { -0.8f, -0.3f,  0.3f,  1.4f }, //thumb
                    { straight[0], straight[1], straight[2],    0f }, //index
                    { straight[0], straight[1], straight[2],   -1f }, //middle
                    { straight[0], straight[1], straight[2],  1.5f }, //ring
                    { straight[0], straight[1], straight[2],    1f }  //little
                })
            },
            { "O",
                new Handshape (new float[,] {
                    { -0.8f, -0.3f,  0.3f,  1.4f }, //thumb
                    { straight[0], straight[1], straight[2], -0.5f }, //index
                    { straight[0], straight[1], straight[2],    0f }, //middle
                    { straight[0], straight[1], straight[2],   -1f }, //ring
                    { straight[0], straight[1], straight[2], -0.8f }  //little
                })
            },
            // "Labial" thumb combinations
            { "P",
                new Handshape (new float[,] {
                    {   -1f, -0.5f, -0.5f, -0.3f }, //thumb
                    {  0.3f, -0.6f, -0.6f,    0f }, //index
                    { curledIn[0], curledIn[1], curledIn[2],    0f }, //middle
                    { curledIn[0], curledIn[1], curledIn[2],    0f }, //ring
                    { curledIn[0], curledIn[1], curledIn[2],    0f }  //little
                })
            },
            { "F",
                new Handshape (new float[,] {
                    { -0.4f, -0.5f, -0.5f, -0.3f }, //thumb
                    {  0.2f, -0.2f, -0.2f,    0f }, //index
                    { curledIn[0], curledIn[1], curledIn[2],    0f }, //middle
                    { curledIn[0], curledIn[1], curledIn[2],    0f }, //ring
                    { curledIn[0], curledIn[1], curledIn[2],    0f }  //little
                })
            },
            // "Coronal" thumb combinations
            { "T",
                new Handshape (new float[] {   -2f, -0.5f, -0.5f, -0.7f }, //thumb
                               new float[] {  0.3f, -0.6f, -0.6f, -0.5f }) //others
            },
            { "S",
                new Handshape (new float[] {  -0.5f, -0.5f, -0.5f, -2.5f }, //thumb
                               new float[] {   0.2f, -0.2f, -0.2f, -0.5f }) //others
            },
            // "Dorsal" thumb combinations
            { "K",
                new Handshape (new float[,] {
                    {   -1f, -0.5f, -0.5f, -0.3f }, //thumb
                    {  0.3f, -0.6f, -0.6f,    0f }, //index
                    {  0.1f,  0.3f,  0.3f,    0f }, //middle
                    {  0.3f,  0.5f,  0.5f,    0f }, //ring
                    {  0.5f,  0.7f,  0.7f,    0f }  //little
                })
            },
            { "X",
                new Handshape (new float[,] {
                    {   -1f, -0.5f, -0.5f,   -1f }, //thumb
                    {  0.2f, -0.2f, -0.2f,    0f }, //index
                    {  0.1f,  0.3f,  0.3f,    0f }, //middle
                    {  0.3f,  0.5f,  0.5f,    0f }, //ring
                    {  0.5f,  0.7f,  0.7f,    0f }  //little
                })
            }
        };

        string[] openHandshapeKeys = { "A", "I", "U", "Y", "E", "O" };
        string[] otherHandshapeKeys = { "P", "F", "T", "S", "K", "X" };
        string[][] handshapeKeys = { openHandshapeKeys, otherHandshapeKeys };
        string[] thumbPositions = { "N", "Q", "Z" };

        Dictionary<string, float[]> fingerPositionDict = new Dictionary<string, float[]>
        {
            // thumb across // fix thumb
            { "N", new float[] { -1.5f,   -1f, -0.5f, -1.2f } },
            // thumb forward
            { "Q", new float[] {   -1f,    0f,    0f, -2.5f } },
            // thumb out
            { "Z", new float[] {    1f,  0.5f,  0.5f,    0f } },

            // bent flat
            { "M", new float[] {   -1f,  0.8f,  0.8f,    0f } },
            // bent curved
            { "C", new float[] { -0.8f,  0.2f,  0.2f,    0f } },
            // bent hooked
            { "V", new float[] {  0.8f, -1.3f, -1.6f, -0.5f } }
        };

        foreach (KeyValuePair<string, float[]> entry in fingerPositionDict)
        {
            foreach (string[] keys in handshapeKeys)
            {
                // Only open handshapes can take thumb positions, so skips
                // if thumb positions are being considered with other handshapes.
                if (keys == otherHandshapeKeys && thumbPositions.Contains(entry.Key))
                {
                    continue;
                }
                foreach (string key in keys)
                {
                    Handshape tempHandshape = dict[key].Clone() as Handshape;
                    if (thumbPositions.Contains(entry.Key))
                    {
                        tempHandshape.ThumbProperties = entry.Value;
                    }
                    else
                    {
                        tempHandshape.NonThumbProperties = entry.Value;
                    }
                    dict[key + entry.Key] = tempHandshape;
                }
            }
        }

        //Handshape.Finger[] last3 = { Handshape.Middle,
        //    Handshape.Ring, Handshape.Little };
        //for (int i = 0; i < last3.Length; i++)
        //{
        //    Handshape tempHandshape = dict["I"].Clone() as Handshape;
        //    SwapFingers(tempHandshape, Handshape.Index, last3[i]);
        //    dict["I" + (i + 3)] = tempHandshape;
        //}

        return dict;
    }

    /// <summary>
	///   <para>Initializes <see cref="SymbolBoneDict"/>.</para>
	/// </summary>
    private static Dictionary<string, HumanBodyBones> InitSymbolBoneDict()
    {
        Dictionary<string, HumanBodyBones> dict = new Dictionary<string, HumanBodyBones>
        {
            // Head symbols
            { "m", HumanBodyBones.Head },
            { "p", HumanBodyBones.Head },
            { "b", HumanBodyBones.Head },
            { "f", HumanBodyBones.Head },
            { "v", HumanBodyBones.Head },
            // Torso symbols
            { "n", HumanBodyBones.Neck },
            { "t", HumanBodyBones.UpperChest },
            { "d", HumanBodyBones.UpperChest },
            { "s", HumanBodyBones.Chest },
            { "z", HumanBodyBones.Spine },
            // Arm symbols
                // Left
                { "ɲl", HumanBodyBones.LeftUpperArm },
                { "cl", HumanBodyBones.LeftUpperArm }, // between upper and lower
                { "ɟl", HumanBodyBones.LeftLowerArm },
                { "ʃl", HumanBodyBones.LeftLowerArm }, // between lower and hand
                { "ʒl", HumanBodyBones.LeftHand },
                // Right
                { "ɲr", HumanBodyBones.RightUpperArm },
                { "cr", HumanBodyBones.RightUpperArm }, // between upper and lower
                { "ɟr", HumanBodyBones.RightLowerArm },
                { "ʃr", HumanBodyBones.RightLowerArm }, // between lower and hand
                { "ʒr", HumanBodyBones.RightHand },
            // Finger symbols
                // Left
                    // Left thumb
                    { "ŋ1l", HumanBodyBones.LeftThumbDistal }, // further beyond distal
                    { "k1l", HumanBodyBones.LeftThumbDistal }, // beyond distal
                    { "g1l", HumanBodyBones.LeftThumbIntermediate }, // between distal and prox.
                    { "x1l", HumanBodyBones.LeftThumbProximal }, // between prox. and intermediate
                    { "ɣ1l", HumanBodyBones.LeftThumbProximal },
                    // Left index
                    { "ŋ2l", HumanBodyBones.LeftIndexDistal }, // further beyond distal
                    { "k2l", HumanBodyBones.LeftIndexDistal }, // beyond distal
                    { "g2l", HumanBodyBones.LeftIndexIntermediate }, // between distal and prox.
                    { "x2l", HumanBodyBones.LeftIndexProximal }, // between prox. and intermediate
                    { "ɣ2l", HumanBodyBones.LeftIndexProximal },
                    // Left middle
                    { "ŋ3l", HumanBodyBones.LeftMiddleDistal }, // further beyond distal
                    { "k3l", HumanBodyBones.LeftMiddleDistal }, // beyond distal
                    { "g3l", HumanBodyBones.LeftMiddleIntermediate }, // between distal and prox.
                    { "x3l", HumanBodyBones.LeftMiddleProximal }, // between prox. and intermediate
                    { "ɣ3l", HumanBodyBones.LeftMiddleProximal },
                    // Left ring
                    { "ŋ4l", HumanBodyBones.LeftRingDistal }, // further beyond distal
                    { "k4l", HumanBodyBones.LeftRingDistal }, // beyond distal
                    { "g4l", HumanBodyBones.LeftRingIntermediate }, // between distal and prox.
                    { "x4l", HumanBodyBones.LeftRingProximal }, // between prox. and intermediate
                    { "ɣ4l", HumanBodyBones.LeftRingProximal },
                    // Left little
                    { "ŋ5l", HumanBodyBones.LeftLittleDistal }, // further beyond distal
                    { "k5l", HumanBodyBones.LeftLittleDistal }, // beyond distal
                    { "g5l", HumanBodyBones.LeftLittleIntermediate }, // between distal and prox.
                    { "x5l", HumanBodyBones.LeftLittleProximal }, // between prox. and intermediate
                    { "ɣ5l", HumanBodyBones.LeftLittleProximal },
                // Right
                    // Right thumb
                    { "ŋ1r", HumanBodyBones.RightThumbDistal }, // further beyond distal
                    { "k1r", HumanBodyBones.RightThumbDistal }, // beyond distal
                    { "g1r", HumanBodyBones.RightThumbIntermediate }, // between distal and prox.
                    { "x1r", HumanBodyBones.RightThumbProximal }, // between prox. and intermediate
                    { "ɣ1r", HumanBodyBones.RightThumbProximal },
                    // Right index
                    { "ŋ2r", HumanBodyBones.RightIndexDistal }, // further beyond distal
                    { "k2r", HumanBodyBones.RightIndexDistal }, // beyond distal
                    { "g2r", HumanBodyBones.RightIndexIntermediate }, // between distal and prox.
                    { "x2r", HumanBodyBones.RightIndexProximal }, // between prox. and intermediate
                    { "ɣ2r", HumanBodyBones.RightIndexProximal },
                    // Right middle
                    { "ŋ3r", HumanBodyBones.RightMiddleDistal }, // further beyond distal
                    { "k3r", HumanBodyBones.RightMiddleDistal }, // beyond distal
                    { "g3r", HumanBodyBones.RightMiddleIntermediate }, // between distal and prox.
                    { "x3r", HumanBodyBones.RightMiddleProximal }, // between prox. and intermediate
                    { "ɣ3r", HumanBodyBones.RightMiddleProximal },
                    // Right ring
                    { "ŋ4r", HumanBodyBones.RightRingDistal }, // further beyond distal
                    { "k4r", HumanBodyBones.RightRingDistal }, // beyond distal
                    { "g4r", HumanBodyBones.RightRingIntermediate }, // between distal and prox.
                    { "x4r", HumanBodyBones.RightRingProximal }, // between prox. and intermediate
                    { "ɣ4r", HumanBodyBones.RightRingProximal },
                    // Right little
                    { "ŋ5r", HumanBodyBones.RightLittleDistal }, // further beyond distal
                    { "k5r", HumanBodyBones.RightLittleDistal }, // beyond distal
                    { "g5r", HumanBodyBones.RightLittleIntermediate }, // between distal and prox.
                    { "x5r", HumanBodyBones.RightLittleProximal }, // between prox. and intermediate
                    { "ɣ5r", HumanBodyBones.RightLittleProximal },
            // Foot/leg symbols
                // Left
                { "ɴl", HumanBodyBones.Hips },
                { "ql", HumanBodyBones.LeftUpperLeg }, // between upper and lower
                { "ɢl", HumanBodyBones.LeftLowerLeg },
                { "χl", HumanBodyBones.LeftLowerLeg }, // between lower and foot
                { "ʁl", HumanBodyBones.LeftToes },
                // Right
                { "ɴr", HumanBodyBones.Hips },
                { "qr", HumanBodyBones.RightUpperLeg }, // between upper and lower
                { "ɢr", HumanBodyBones.RightLowerLeg },
                { "χr", HumanBodyBones.RightLowerLeg }, // between lower and foot
                { "ʁr", HumanBodyBones.RightToes },
            // Minor place symbols
                { "θl", HumanBodyBones.Head },
                { "θr", HumanBodyBones.Head },
                { "ðl", HumanBodyBones.Head },
                { "ðr", HumanBodyBones.Head },
                { "ɾl", HumanBodyBones.LeftHand },
                { "ɾr", HumanBodyBones.RightHand },
            { "ʔ", HumanBodyBones.Hips }, //below the hips
            { "h", HumanBodyBones.Hips }, //below the hips
            // Neutral space
            { "0", HumanBodyBones.Chest },
            { "0l", HumanBodyBones.LeftUpperArm }, // used for the left hand
            { "0r", HumanBodyBones.RightUpperArm } // used for the right hand
        };

        return dict;
    }

    /// <summary>
	///   <para>Initializes <see cref="PlaceValueOffsets"/>.</para>
	/// </summary>
    private static Dictionary<string, (Vector3, Vector3?)> InitPlaceValueOffsets()
    {
        // The dictionary is initialized to contain only the left versions of
        // places; the right versions of those are added later.

        // This exists while some of the places' positions are incomplete.
        (Vector3, Vector3?) incompletePosition = (Vector3.zero, null);
        Dictionary<string, (Vector3, Vector3?)> dict = new Dictionary<string, (Vector3, Vector3?)>
        {
            // Head symbols
            { "m", (new Vector3(0f, 0.15f, 0.11f), null) },
            { "p", (new Vector3(0f, 0.09f, 0.13f), null) },
            { "b", (new Vector3(0f, 0.05f, 0.14f), null) },
            { "f", (new Vector3(0f, 0f, 0.13f), null) },
            { "v", (new Vector3(0f, -0.06f, 0.12f), null) },
            // Torso symbols
            { "n", (new Vector3(0f, 0.05f, 0.07f), null) },
            { "t", (new Vector3(0f, 0.09f, 0.15f), null) },
            { "d", (new Vector3(0f, 0.04f, 0.18f), null) },
            { "s", (new Vector3(0f, 0.04f, 0.18f), null) },
            { "z", (new Vector3(0f, -0.04f, 0.16f), null) },
            // Arm symbols
            { "ɲl", (new Vector3(0f, 0.09f, 0f), null) },
            { "cl", incompletePosition }, // between upper and lower
            { "ɟl", (new Vector3(0f, -0.04f, -0.04f), null) },
            { "ʃl", incompletePosition }, // between lower and hand
            { "ʒl", incompletePosition },
            // Finger symbols
                // Left thumb
                { "ŋ1l", incompletePosition }, // further beyond distal
                { "k1l", incompletePosition }, // beyond distal
                { "g1l", incompletePosition }, // between distal and prox.
                { "x1l", incompletePosition }, // between prox. and intermediate
                { "ɣ1l", incompletePosition },
                // Left index
                { "ŋ2l", incompletePosition }, // further beyond distal
                { "k2l", incompletePosition }, // beyond distal
                { "g2l", incompletePosition }, // between distal and prox.
                { "x2l", incompletePosition }, // between prox. and intermediate
                { "ɣ2l", incompletePosition },
                // Left middle
                { "ŋ3l", incompletePosition }, // further beyond distal
                { "k3l", incompletePosition }, // beyond distal
                { "g3l", incompletePosition }, // between distal and prox.
                { "x3l", incompletePosition }, // between prox. and intermediate
                { "ɣ3l", incompletePosition },
                // Left ring
                { "ŋ4l", incompletePosition }, // further beyond distal
                { "k4l", incompletePosition }, // beyond distal
                { "g4l", incompletePosition }, // between distal and prox.
                { "x4l", incompletePosition }, // between prox. and intermediate
                { "ɣ4l", incompletePosition },
                // Left little
                { "ŋ5l", incompletePosition }, // further beyond distal
                { "k5l", incompletePosition }, // beyond distal
                { "g5l", incompletePosition }, // between distal and prox.
                { "x5l", incompletePosition }, // between prox. and intermediate
                { "ɣ5l", incompletePosition },
            // Foot/leg symbols
            { "ɴl", (new Vector3(-0.09f, 0f, 0.14f), null) },
            { "ql", incompletePosition }, // between upper and lower
            { "ɢl", (new Vector3(0f, 0f, 0.08f), null) },
            { "χl", incompletePosition }, // between lower and foot
            { "ʁl", (new Vector3(0f, 0.07f, 0f), null) },
            // Minor place symbols
            { "θl", (new Vector3(-0.1f, 0.08f, 0f), null) },
            { "ðl", incompletePosition },
            { "ɾl", incompletePosition },
            { "ʔ", incompletePosition }, //below the hips
            { "h", incompletePosition }, //below the hips
            // Neutral space
            { "0", (new Vector3(0f, 0f, 0.35f), null) },
            { "0l", (new Vector3(-0.15f, -0.15f, 0.35f), null) }
        };

        // New entries are stored in this temporary dictionary initially so
        // that the dictionary doesn't change in the foreach loop.
        Dictionary<string, (Vector3, Vector3?)> tempDict =
            new Dictionary<string, (Vector3, Vector3?)>();

        foreach (KeyValuePair<string, (Vector3, Vector3?)> entry in dict)
        {
            if (entry.Key.Last() == 'l')
            {
                string newKey = entry.Key.Substring(0, entry.Key.Length - 1) + "r";
                Vector3 target = entry.Value.Item1;
                target.x *= -1;
                Vector3? hint;
                if (entry.Value.Item2 != null)
                {
                    Vector3 temp = (Vector3)entry.Value.Item2;
                    temp.x *= -1;
                    hint = (Vector3?)temp;
                }
                else { hint = null; }
                tempDict[newKey] = (target, hint);
            }
        }

        // Adds the entries of the temporary dictionary to the main one.
        foreach (KeyValuePair<string, (Vector3, Vector3?)> entry in tempDict)
        {
            dict[entry.Key] = entry.Value;
        }

        return dict;
    }

    /// <summary>
	///   <para>Initializes <see cref="RegexPattern"/>.</para>
	/// </summary>
    private static string InitRegexPattern()
    {
        // A list of all acceptable patterns for Sign IPA input.
        List<string> acceptable = new List<string>();
        string basicHeadPlaces = @"[mpbfv]";
        string basicUnpairedPlaces = @"[ntdszʔh]";
        string basicPairedPlaces = @"[ɲcɟʃʒɴqɢχʁθðɾ]";
        string basicFingerPlaces = @"[ŋkgxɣ]", fingerNumbers = @"[12345]";
        string sides = @"[lr]", relSides = @"(<{1,2}|>{1,2})", otherSide = @"'";
        // The accented non-ASCII characters have to be dealt with separately
        // because they are each encoded as two separate characters.
        // The combining  characters are between square brackets after [ɛɨəɔ],
        // but they may not be visible because they are combining the left bracket.
        string basicDirectionsAndMovements =
            @"([ieɛɨəauoɔ][˦˧˨]?|[íéɨ́ə́áúóɔ́ìèɛ̀ɨ̀ə̀àùòɔ̀īēɛ̄ɨ̄ə̄āūōɔ̄]|[ɛɨəɔ][́̀̄])";
        
        string basicOpenHandshapes = @"[AIUYEO]";
        string basicOtherHandshapes = @"[PBFWTDSLKGXJ]";
        string handshapeMods = @"[NQZMCV]";
        //string rotations = "[yɯøɤœʌ]";

        // Head places can have a single relative side marker.
        string headPlaces = basicHeadPlaces + @"[<>]?";
        // The torso and groin can optionally be marked for relative side.
        string unpairedPlaces = basicUnpairedPlaces + relSides + @"?";
        // Paired places are obligatorily specified for absolute side and can
        // also be marked for relative side.
        string pairedPlaces = basicPairedPlaces + sides + relSides + @"?";
        // Finger places are obligatorily specified for finger and absolute side,
        // and they can optionally be marked for relative side.
        string fingerPlaces = basicFingerPlaces + fingerNumbers + sides + relSides + @"?";
        // A place can be any of the aforementioned kinds of places
        // with the opposite side being optionally marked.
        string place = @"(?<place>(" + headPlaces + @"|" + unpairedPlaces + @"|"
            + pairedPlaces + @"|" + fingerPlaces + @")" + otherSide + @"?)";

        // When only the direction is supplied, the hand doesn't move.
        string directionOnly = @"(" + basicDirectionsAndMovements + @",)";
        // If direction is unspecified, the hand is facing a default direction.
        string movement = @"((?<facing>" + basicDirectionsAndMovements + @")?,(?<movement>"
            + basicDirectionsAndMovements + @"))";

        // Open handshapes can take any basic handshape modifiers.
        string openHandshapes = basicOpenHandshapes + handshapeMods + @"?";
        // Other handshapes can only take modifiers for bending.
        string otherHandshapes = basicOtherHandshapes + @"[MCV]?";
        // A handshape can either be open or not open.
        string handshape = @"(?<handshape>" + openHandshapes + @"|" + otherHandshapes + @")";

        // A maximally PMP sign, allowing for PMP, PM, MP, M, or P.
        // Currently doesn't consider secondary articulation considerations.
        string maxPMPsign = @"(" + place + @"?" + movement + @"?" + place + @"?|" + place + @")";

        // An optional handshape plus a maximally PMP sign.
        acceptable.Add(handshape + @"?" + maxPMPsign);
        // A handshape plus an an optional maximally PMP sign or direction.
        acceptable.Add(handshape + @"(" + maxPMPsign + @"|" + directionOnly + @")?");

        // Separate all of the the acceptable inputs by '|' and places
        // '^' and '$' at the beginning and end of the pattern, respectively.
        System.Text.StringBuilder pattern = new System.Text.StringBuilder(@"^(");
        for (int i = 0; i < acceptable.Count; i++)
        {
            if (i != 0) { pattern.Append(@"|"); }
            pattern.Append(acceptable[i]);
        }
        pattern.Append(@")$");
        return pattern.ToString();
    }

    /// <summary>
	///   <para>Swaps the values of two fingers in a <see cref="Handshape"/>.</para>
	/// </summary>
    /// <param name="hs">The <see cref="Handshape"/> affected.</param>
	/// <param name="a">The first finger being swapped.</param>
	/// <param name="b">The second finger being swapped.</param>
    private static void SwapFingers(Handshape hs, Handshape.Finger a, Handshape.Finger b)
    {
        float[] propsA = hs.GetFingerProperties(a);
        float[] propsB = hs.GetFingerProperties(b);
        hs.SetFingerProperties(a, propsB);
        hs.SetFingerProperties(b, propsA);
    }

    /// <summary>
    ///   Returns the translation corresponding to the direction given.
    /// </summary>
    /// <param name="movement">
    ///   A string of the input symbols for movement in Sign IPA.
    /// </param>
    /// <remarks>
    ///   When Unity updates to C# 8.0, can be greatly simplified with a switch expression.
    /// </remarks>
    public static Vector3 Translation(string movement)
    {
        Vector3 translation = Vector3.zero;
        switch (movement)
        {
            case "í":
            case "i˦":
                translation = Vector3.forward + Vector3.left + Vector3.up;
                break;
            case "i":
            case "ī":
            case "i˧":
                translation = Vector3.forward + Vector3.left;
                break;
            case "ì":
            case "i˨":
                translation = Vector3.forward + Vector3.left + Vector3.down;
                break;

            case "é":
            case "e˦":
                translation = Vector3.forward + Vector3.up;
                break;
            case "e":
            case "ē":
            case "e˧":
                translation = Vector3.forward;
                break;
            case "è":
            case "e˨":
                translation = Vector3.forward + Vector3.down;
                break;

            case "ɛ́":
            case "ɛ˦":
                translation = Vector3.forward + Vector3.right + Vector3.up;
                break;
            case "ɛ":
            case "ɛ̄":
            case "ɛ˧":
                translation = Vector3.forward + Vector3.right;
                break;
            case "ɛ̀":
            case "ɛ˨":
                translation = Vector3.forward + Vector3.right + Vector3.down;
                break;

            case "ɨ́":
            case "ɨ˦":
                translation = Vector3.left + Vector3.up;
                break;
            case "ɨ":
            case "ɨ̄":
            case "ɨ˧":
                translation = Vector3.left;
                break;
            case "ɨ̀":
            case "ɨ˨":
                translation = Vector3.left + Vector3.down;
                break;

            case "ə́":
            case "ə˦":
                translation = Vector3.up;
                break;
            case "ə̀":
            case "ə˨":
                translation = Vector3.down;
                break;

            case "á":
            case "a˦":
                translation = Vector3.right + Vector3.up;
                break;
            case "a":
            case "ā":
            case "a˧":
                translation = Vector3.right;
                break;
            case "à":
            case "a˨":
                translation = Vector3.right + Vector3.down;
                break;

            case "ú":
            case "u˦":
                translation = Vector3.back + Vector3.left + Vector3.up;
                break;
            case "u":
            case "ū":
            case "u˧":
                translation = Vector3.back + Vector3.left;
                break;
            case "ù":
            case "u˨":
                translation = Vector3.back + Vector3.left + Vector3.down;
                break;

            case "ó":
            case "o˦":
                translation = Vector3.back + Vector3.up;
                break;
            case "o":
            case "ō":
            case "o˧":
                translation = Vector3.back;
                break;
            case "ò":
            case "o˨":
                translation = Vector3.back + Vector3.down;
                break;

            case "ɔ́":
            case "ɔ˦":
                translation = Vector3.back + Vector3.right + Vector3.up;
                break;
            case "ɔ":
            case "ɔ̄":
            case "ɔ˧":
                translation = Vector3.back + Vector3.right;
                break;
            case "ɔ̀":
            case "ɔ˨":
                translation = Vector3.back + Vector3.right + Vector3.down;
                break;
        }
        return translation;
    }

    /// <summary>
    ///   Returns the rotation corresponding to the direction given.
    /// </summary>
    /// <param name="facing">
    ///   A string of the input symbols for direction in Sign IPA.
    /// </param>
    /// <remarks>
    ///   When Unity updates to C# 8.0, can be greatly simplified with a switch expression.
    /// </remarks>
    public static Quaternion Rotation(string facing)
    {
        Quaternion rotation, undecided = Quaternion.Euler(0f, 0f, 0f);
        switch (facing)
        {
            case "í":
            case "i˦":
                rotation = undecided;
                break;
            case "i":
            case "ī":
            case "i˧":
                rotation = undecided;
                break;
            case "ì":
            case "i˨":
                rotation = undecided;
                break;

            case "é":
            case "e˦":
                rotation = Quaternion.Euler(-135f, 0f, 0f);
                break;
            case "e":
            case "ē":
            case "e˧":
                rotation = Quaternion.Euler(-90f, 0f, 0f);
                break;
            case "è":
            case "e˨":
                rotation = Quaternion.Euler(-45f, 0f, 0f);
                break;

            case "ɛ́":
            case "ɛ˦":
                rotation = undecided;
                break;
            case "ɛ":
            case "ɛ̄":
            case "ɛ˧":
                rotation = undecided;
                break;
            case "ɛ̀":
            case "ɛ˨":
                rotation = undecided;
                break;

            case "ɨ́":
            case "ɨ˦":
                rotation = Quaternion.Euler(-90f, 0f, -135f);
                break;
            case "ɨ":
            case "ɨ̄":
            case "ɨ˧":
                rotation = Quaternion.Euler(-90f, 0f, -90f);
                break;
            case "ɨ̀":
            case "ɨ˨":
                rotation = Quaternion.Euler(-90f, 0f, -45f);
                break;

            case "ə́":
            case "ə˦":
                rotation = Quaternion.Euler(180f, 0f, 0f);
                break;
            case "ə̀":
            case "ə˨":
                rotation = Quaternion.Euler(0f, 0f, 0f);
                break;

            case "á":
            case "a˦":
                rotation = Quaternion.Euler(-90f, 0f, 135f);
                break;
            case "a":
            case "ā":
            case "a˧":
                rotation = Quaternion.Euler(-90f, 0f, 90f);
                break;
            case "à":
            case "a˨":
                rotation = Quaternion.Euler(-90f, 0f, 45f);
                break;

            case "ú":
            case "u˦":
                rotation = undecided;
                break;
            case "u":
            case "ū":
            case "u˧":
                rotation = undecided;
                break;
            case "ù":
            case "u˨":
                rotation = undecided;
                break;

            case "ó":
            case "o˦":
                rotation = Quaternion.Euler(-135f, 90f, 90f);
                break;
            case "o":
            case "ō":
            case "o˧":
                rotation = Quaternion.Euler(-90f, 90f, 90f);
                break;
            case "ò":
            case "o˨":
                rotation = Quaternion.Euler(-45f, 90f, 90f);
                break;

            case "ɔ́":
            case "ɔ˦":
                rotation = undecided;
                break;
            case "ɔ":
            case "ɔ̄":
            case "ɔ˧":
                rotation = undecided;
                break;
            case "ɔ̀":
            case "ɔ˨":
                rotation = undecided;
                break;

            default:
                rotation = undecided;
                break;
        }
        return rotation;
    }
}
