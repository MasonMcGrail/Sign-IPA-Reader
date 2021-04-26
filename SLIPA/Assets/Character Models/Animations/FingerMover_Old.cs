using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Text.RegularExpressions;
using System.Linq;

public class FingerMover_Old : MonoBehaviour
{
    [SerializeField]
    private AnimatorController animatorController;
    // The Animator is only used to instantiate the HumanPoseHandler
    private Animator anim;
    private HumanPoseHandler handler;
    private HumanPose pose;

    //https://github.com/umiyuki/HumanoidHandPoseHelper/blob/master/Assets/HumanoidHandPoseHelper/HumanoidHandPoseHelper.cs

    // Renamed from (left/right)SidePoseValueMap
    public Dictionary<string, float> sidePoseValueDict;

    // Renamed from (left/right)SidePoseValueMap
    public Dictionary<string, HumanBodyBones> symbolBoneDict;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        handler = new HumanPoseHandler(anim.avatar, anim.transform);
        pose = new HumanPose();
        handler.GetHumanPose(ref pose);

        sidePoseValueDict = InitSidePoseValueMap();
        symbolBoneDict = InitSymbolBoneDict();

        //UpdateArmPosition(new float[] { 1f, 1f, 1f, 1f, 1f }, LRSide.Right);

        shapes = new Handshape[] { new Handshape() };
        UpdatePose();

        //Test();
    }

    /// <summary>
	///   <para>Send float values to the Animator to affect transitions.</para>
	/// </summary>
	/// <param name="name">The parameter name.</param>
	/// <param name="id">The parameter ID.</param>
	/// <param name="value">The new parameter value.</param>
	/// <param name="dampTime">The damper total time.</param>
	/// <param name="deltaTime">The delta time to give to the damper.</param>

    /// <summary>
	///   <para>Initializes sidePoseValueDict.</para>
	/// </summary>
    private Dictionary<string, float> InitSidePoseValueMap()
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        for (int i = 0; i < Variables.NumSides; i++)
        {
            // fingers
            dict.Add(Variables.SideNames[i] + " Thumb 1 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Thumb 2 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Thumb 3 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Thumb Spread", 0f);
            dict.Add(Variables.SideNames[i] + " Index 1 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Index 2 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Index 3 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Index Spread", 0f);
            dict.Add(Variables.SideNames[i] + " Middle 1 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Middle 2 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Middle 3 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Middle Spread", 0f);
            dict.Add(Variables.SideNames[i] + " Ring 1 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Ring 2 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Ring 3 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Ring Spread", 0f);
            dict.Add(Variables.SideNames[i] + " Little 1 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Little 2 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Little 3 Stretched", 0f);
            dict.Add(Variables.SideNames[i] + " Little Spread", 0f);

            // hand; values change from the wrist
            // controls how much the wrist is bent
            dict.Add(Variables.SideNames[i] + " Hand Down-Up", 0f);
            //   // controls how much the hand is rotated
            //dict.Add(Variables.SideNames[i] + " Hand In-Out", 0f);

            // forearm; values change from the elbow
            // controls how much the forearm is bent
            dict.Add(Variables.SideNames[i] + " Forearm Stretch", 1f);
            // controls how much the forearm is rotated
            dict.Add(Variables.SideNames[i] + " Forearm Twist In-Out", 0.5f);

            // upper arm; values change from the shoulder
            // controls how much the arm abducts/adducts from the body
            dict.Add(Variables.SideNames[i] + " Arm Down-Up", -0.5f);
            // controls how much the arm moves forward/backward
            dict.Add(Variables.SideNames[i] + " Arm Front-Back", 0.25f);
            // controls how much the arm is rotated
            dict.Add(Variables.SideNames[i] + " Arm Twist In-Out", 0.5f);
        }

        return dict;
    }

    private Dictionary<string, HumanBodyBones> InitSymbolBoneDict()
    {
        Dictionary<string, HumanBodyBones> dict = new Dictionary<string, HumanBodyBones>
        {
            // head symbols (all are in different positions than base)
            { "m", HumanBodyBones.Head },
            { "p", HumanBodyBones.Head },
            { "b", HumanBodyBones.Head },
            { "f", HumanBodyBones.Head },
            { "v", HumanBodyBones.Head },
            // torso symbols
            { "n", HumanBodyBones.Neck },
            { "t", HumanBodyBones.UpperChest },
            { "d", HumanBodyBones.Chest },
            { "s", HumanBodyBones.Spine },
            { "z", HumanBodyBones.Hips },
            // arm symbols
                // left
                { "ɲl", HumanBodyBones.LeftUpperArm },
                { "cl", HumanBodyBones.LeftUpperArm }, // between upper and lower
                { "ɟl", HumanBodyBones.LeftLowerArm },
                { "ʃl", HumanBodyBones.LeftLowerArm }, // between lower and hand
                { "ʒl", HumanBodyBones.LeftHand },
                // right
                { "ɲr", HumanBodyBones.RightUpperArm },
                { "cr", HumanBodyBones.RightUpperArm }, // between upper and lower
                { "ɟr", HumanBodyBones.RightLowerArm },
                { "ʃr", HumanBodyBones.RightLowerArm }, // between lower and hand
                { "ʒr", HumanBodyBones.RightHand },
            // finger symbols
                // left
                    // left thumb
                    { "ŋ1l", HumanBodyBones.LeftThumbDistal }, // further beyond distal
                    { "k1l", HumanBodyBones.LeftThumbDistal }, // beyond distal
                    { "g1l", HumanBodyBones.LeftThumbIntermediate }, // between intermediate and prox.
                    { "x1l", HumanBodyBones.LeftThumbProximal }, // between prox. and intermediate
                    { "ɣ1l", HumanBodyBones.LeftThumbProximal },
                    // left index
                    { "ŋ2l", HumanBodyBones.LeftIndexDistal }, // further beyond distal
                    { "k2l", HumanBodyBones.LeftIndexDistal }, // beyond distal
                    { "g2l", HumanBodyBones.LeftIndexIntermediate }, // between intermediate and prox.
                    { "x2l", HumanBodyBones.LeftIndexProximal }, // between prox. and intermediate
                    { "ɣ2l", HumanBodyBones.LeftIndexProximal },
                    // left middle
                    { "ŋ3l", HumanBodyBones.LeftMiddleDistal }, // further beyond distal
                    { "k3l", HumanBodyBones.LeftMiddleDistal }, // beyond distal
                    { "g3l", HumanBodyBones.LeftMiddleIntermediate }, // between intermediate and prox.
                    { "x3l", HumanBodyBones.LeftMiddleProximal }, // between prox. and intermediate
                    { "ɣ3l", HumanBodyBones.LeftMiddleProximal },
                    // left ring
                    { "ŋ4l", HumanBodyBones.LeftRingDistal }, // further beyond distal
                    { "k4l", HumanBodyBones.LeftRingDistal }, // beyond distal
                    { "g4l", HumanBodyBones.LeftRingIntermediate }, // between intermediate and prox.
                    { "x4l", HumanBodyBones.LeftRingProximal }, // between prox. and intermediate
                    { "ɣ4l", HumanBodyBones.LeftRingProximal },
                    // left little
                    { "ŋ5l", HumanBodyBones.LeftLittleDistal }, // further beyond distal
                    { "k5l", HumanBodyBones.LeftLittleDistal }, // beyond distal
                    { "g5l", HumanBodyBones.LeftLittleIntermediate }, // between intermediate and prox.
                    { "x5l", HumanBodyBones.LeftLittleProximal }, // between prox. and intermediate
                    { "ɣ5l", HumanBodyBones.LeftLittleProximal },
                // right
                    // right thumb
                    { "ŋ1r", HumanBodyBones.RightThumbDistal }, // further beyond distal
                    { "k1r", HumanBodyBones.RightThumbDistal }, // beyond distal
                    { "g1r", HumanBodyBones.RightThumbIntermediate }, // between intermediate and prox.
                    { "x1r", HumanBodyBones.RightThumbProximal }, // between prox. and intermediate
                    { "ɣ1r", HumanBodyBones.RightThumbProximal },
                    // right index
                    { "ŋ2r", HumanBodyBones.RightIndexDistal }, // further beyond distal
                    { "k2r", HumanBodyBones.RightIndexDistal }, // beyond distal
                    { "g2r", HumanBodyBones.RightIndexIntermediate }, // between intermediate and prox.
                    { "x2r", HumanBodyBones.RightIndexProximal }, // between prox. and intermediate
                    { "ɣ2r", HumanBodyBones.RightIndexProximal },
                    // right middle
                    { "ŋ3r", HumanBodyBones.RightMiddleDistal }, // further beyond distal
                    { "k3r", HumanBodyBones.RightMiddleDistal }, // beyond distal
                    { "g3r", HumanBodyBones.RightMiddleIntermediate }, // between intermediate and prox.
                    { "x3r", HumanBodyBones.RightMiddleProximal }, // between prox. and intermediate
                    { "ɣ3r", HumanBodyBones.RightMiddleProximal },
                    // right ring
                    { "ŋ4r", HumanBodyBones.RightRingDistal }, // further beyond distal
                    { "k4r", HumanBodyBones.RightRingDistal }, // beyond distal
                    { "g4r", HumanBodyBones.RightRingIntermediate }, // between intermediate and prox.
                    { "x4r", HumanBodyBones.RightRingProximal }, // between prox. and intermediate
                    { "ɣ4r", HumanBodyBones.RightRingProximal },
                    // right little
                    { "ŋ5r", HumanBodyBones.RightLittleDistal }, // further beyond distal
                    { "k5r", HumanBodyBones.RightLittleDistal }, // beyond distal
                    { "g5r", HumanBodyBones.RightLittleIntermediate }, // between intermediate and prox.
                    { "x5r", HumanBodyBones.RightLittleProximal }, // between prox. and intermediate
                    { "ɣ5r", HumanBodyBones.RightLittleProximal },
            // foot/leg symbols
                // left
                { "ɴl", HumanBodyBones.LeftUpperLeg },
                { "ql", HumanBodyBones.LeftUpperLeg }, // between upper and lower
                { "ɢl", HumanBodyBones.LeftLowerLeg },
                { "χl", HumanBodyBones.LeftLowerLeg }, // between lower and foot
                { "ʁl", HumanBodyBones.LeftFoot },
                // right
                { "ɴr", HumanBodyBones.RightUpperLeg },
                { "qr", HumanBodyBones.RightUpperLeg }, // between upper and lower
                { "ɢr", HumanBodyBones.RightLowerLeg },
                { "χr", HumanBodyBones.RightLowerLeg }, // between lower and foot
                { "ʁr", HumanBodyBones.RightFoot },
            // minor place symbols
                { "θl", HumanBodyBones.Head },
                { "θr", HumanBodyBones.Head },
                { "ðl", HumanBodyBones.Head },
                { "ðr", HumanBodyBones.Head },
                { "ɾl", HumanBodyBones.LeftHand },
                { "ɾr", HumanBodyBones.RightHand },
            { "ʔ", HumanBodyBones.Hips }, //below the hips
            { "h", HumanBodyBones.Hips } //below the hips
        };

        return dict;
    }

    private int cnt = 0;
    private Handshape[] shapes;

    // Update is called once per frame
    void Update()
    {
        if (cnt < shapes.Length)
        {
            UpdateHandshape(shapes[cnt++], Variables.Left);
            UpdatePose();
        }
    }

    /// <summary>
	///   <para>Checks input against a pattern; returns true if the input is valid.</para>
	/// </summary>
	/// <param name="input">The input from the text field.</param>
    public bool ReadInput(string input)
    {
        Match match = Regex.Match(input, Variables.RegexPattern);
        GroupCollection groups = match.Groups;
        Debug.Log("groups count: " + groups.Count);
        string handshape = groups["handshape"].Success ? groups["handshape"].ToString() : string.Empty;
        //string place = groups["place"].Success ? groups["place"].ToString() : string.Empty;
        string[] places = null;
        if (groups["place"].Success)
        {
            CaptureCollection captures = groups["place"].Captures;
            places = new string[captures.Count];
            for (int i = 0; i < captures.Count; i++)
            {
                places[i] = captures[i].ToString();
            }
        }
        string placesStr = "\"\"";
        if (places != null)
        {
            placesStr = "";
            for (int i = 0; i < places.Length; i++)
            {
                if (i != 0) { placesStr += @","; }
                placesStr += "\"" + places[i] + "\"";
            }
        }
        string movement = groups["movement"].Success ? groups["movement"].ToString() : string.Empty;
        Debug.Log("handshape: \"" + handshape + "\", places: [" + placesStr +
            "], movement: \"" + movement + "\"");

        // Note that on this computer, the program runs at roughly 80 fps
        if (Variables.HandshapeDict.ContainsKey(handshape))
        {
            //tempHandshape = Variables.handshapeDict[handshape];
            shapes = InterpolateAnimation(shapes[shapes.Length - 1],
                Variables.HandshapeDict[handshape], 20);
            cnt = 0;
        }
        else
        {
            Debug.Log("Key does not exist in dictionary.");
            //OnAnimatorIK(0);
        }
        // the count is 1 only when the input is invalid
        return groups.Count > 1;
    }

    /// <summary>
	///   <para>Updates the pose of the avatar.</para>
	/// </summary>
    public void UpdatePose()
    {
        //ハンドポーズを流し込む
        for (int i = 0; i < pose.muscles.Length; i++)
        {
            string muscle = HumanTrait.MuscleName[i];
            if (Variables.PlaceNamesDict.ContainsKey(muscle)) //両手ポーズプロパティ
            {
                pose.muscles[i] = sidePoseValueDict[muscle];
            }
        }
        ////makes him float real high with any input
        //pose.bodyPosition += new Vector3(0f, 0.1f, 0f);
        handler.SetHumanPose(ref pose); //ポーズセット
    }

    /// <summary>
	///   <para>???.</para>
	/// </summary>
	/// <param name="hs">???.</param>
    /// <param name="side">???.</param>
    private void UpdateHandshape(Handshape hs, Variables.Side side)
    {
        for (int i = 0; i < Handshape.fingerNames.Length; i++)
        {
            string keyStart = side.ToString() + " " + Handshape.fingerNames[i];
            sidePoseValueDict[keyStart + " 1 Stretched"] = hs.FingerProperties[i, 0];
            sidePoseValueDict[keyStart + " 2 Stretched"] = hs.FingerProperties[i, 1];
            sidePoseValueDict[keyStart + " 3 Stretched"] = hs.FingerProperties[i, 2];
            sidePoseValueDict[keyStart + " Spread"] = hs.FingerProperties[i, 3];
        }
    }

    //private void UpdateArmPosition(float[] armProperties, Variables.Side side)
    //{
    //    Dictionary<string, float> poseValueMap = sidePoseValueDict[side == Variables.Left ? 0 : 1];
    //    string[] upperArmProps = { "Forearm Stretch", "Forearm Twist In-Out",
    //    "Arm Down-Up", "Arm Front-Back", "Arm Twist In-Out" };
    //    for (int i = 0; i < upperArmProps.Length; i++)
    //    {
    //        poseValueMap[side.ToString() + " " + upperArmProps[i]] = armProperties[i];
    //    }
    //}

    // currently only works for the hand
    // adapted from HandshapeHelper
    public AnimationClip ExportHandPose(string clipName)
    {
        var clip = new AnimationClip { frameRate = 30 };
        clip.name = clipName;
        AnimationUtility.SetAnimationClipSettings(clip, new AnimationClipSettings { loopTime = true });

        for (int i = 0; i < HumanTrait.MuscleCount; i++)
        {
            var muscle = HumanTrait.MuscleName[i];
            if (Variables.PlaceNamesDict.ContainsKey(muscle))
            {
                var curve = new AnimationCurve();
                curve.AddKey(0f, sidePoseValueDict[muscle]);

                string musclePropName = Variables.PlaceNamesDict[muscle];
                clip.SetCurve("", typeof(Animator), musclePropName, curve);
            }
        }
        return clip;
    }

    public void ActivateAnimator()
    {
        string newStateName = "newState";
        // AnimatorController.layers returns a copy, so the changed variable needs
        // to be referentiable for later reassignment
        AnimatorControllerLayer[] layers = animatorController.layers;
        AnimatorStateMachine rootStateMachine = layers[0].stateMachine;

        // if the state corresponding to newState already exists, assign it,
        // otherwise make a new state
        int newStateIndex = System.Array.IndexOf(rootStateMachine.states.Select(s => s.state.name).ToArray(), newStateName);
        AnimatorState newState = (newStateIndex == -1) ?
            rootStateMachine.AddState(newStateName) :
            rootStateMachine.states[newStateIndex].state;

        newState.motion = ExportHandPose("newState anim");
        animatorController.layers = layers;

        foreach (AnimatorState state in rootStateMachine.states.Select(s => s.state))
        {
            Debug.Log("state.name: " + state.name + ", has motion: " + (state.motion != null ? "true" : "false"));
        }

        //ExportHandPose("_TestClip.anim");

        //anim.enabled = true;
        //foreach (HumanBodyBones bone in System.Enum.GetValues(typeof(HumanBodyBones)))
        //{
        //    if (bone == HumanBodyBones.LastBone || anim.GetBoneTransform(bone) == null)
        //    {
        //        continue;
        //    }
        //    anim.SetBoneLocalRotation(bone, anim.GetBoneTransform(bone).rotation);
        //}

        //Transform armTransform = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
        //armTransform.Translate(new Vector3(0.01f, 0.1f, 1f));
        //Debug.Log(armTransform.rotation.ToString());
    }

    //private float rot_cnt = 0f;

    void OnAnimatorIK(int layerIndex)
    {
        //HumanBodyBones bone = HumanBodyBones.RightUpperArm;
        //Vector3 rotationV3 = anim.GetBoneTransform(bone).rotation.eulerAngles;
        //rot_cnt++;
        //rotationV3.x += rot_cnt; rotationV3.y += rot_cnt; rotationV3.z += rot_cnt;
        //anim.SetBoneLocalRotation(bone, Quaternion.Euler(rotationV3));
        //Debug.Log(rotationV3);

        //foreach (HumanBodyBones bone in System.Enum.GetValues(typeof(HumanBodyBones)))
        //{
        //    if (bone == HumanBodyBones.LastBone || anim.GetBoneTransform(bone) == null)
        //    {
        //        continue;
        //    }
        //    Quaternion rotation = anim.GetBoneTransform(bone).rotation;
        //    anim.SetBoneLocalRotation(bone, rotation);
        //    Debug.Log(rotation);
        //    //anim.SetBoneLocalRotation(bone, anim.GetBoneTransform(bone).rotation);
        //}
    }

    /// <summary>
    /// Using two <c>Handshape</c>s which act as keyframes, creates an array of
    /// handshapes that is the result of linear interpolation between them.
    /// </summary>
    /// <param name="hsStart">The <c>Handshape</c> being transitioned from.</param>
    /// <param name="hsEnd">The <c>Handshape</c> being transitioned into.</param>
    /// <param name="numFrames">The number of frames in the interpolation.</param>
    /// <returns>An array of handshapes which, when played sequentially, act
    /// like a single animation.</returns>
    private Handshape[] InterpolateAnimation(Handshape hsStart, Handshape hsEnd, int numFrames)
    {
        Handshape[] hsTemps = new Handshape[numFrames];

        float[,] propertiesStart = hsStart.FingerProperties;
        float[,] propertiesEnd = hsEnd.FingerProperties;

        for (int i = 0; i < numFrames; i++)
        {
            float[,] hsTemp = new float[propertiesStart.GetLength(0), propertiesStart.GetLength(1)];
            for (int j = 0; j < hsTemp.GetLength(0); j++)
            {
                for (int k = 0; k < hsTemp.GetLength(1); k++)
                {
                    hsTemp[j, k] =
                        propertiesStart[j, k] * (numFrames - 1 - i) / (numFrames - 1) +
                        propertiesEnd[j, k] * i / (numFrames - 1);
                }
            }
            hsTemps[i] = new Handshape(hsTemp);
        }
        return hsTemps;
    }

    private void Test()
    {
        foreach (KeyValuePair<string, HumanBodyBones> entry in symbolBoneDict)
        {
            Transform transform = anim.GetBoneTransform(entry.Value);
            Debug.Log(entry.Key + ": " + transform.position.ToString());
            Debug.Log(entry.Key + " parent: " + transform.parent);
        }
    }

}
