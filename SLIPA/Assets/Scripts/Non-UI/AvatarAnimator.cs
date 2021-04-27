using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine.Animations.Rigging;
using System.Text.RegularExpressions;
//using System.Linq;

/// <summary>
///   <para>This class does the bulk of the work in this project. User input is read
///   from <see cref="SignIO"/>, and if valid, the avatar's values are updated to match,
///   and <see cref="AnimationClip"/>s are made for the <see cref="AnimatorController"/>
///   to faciliate the animation.</para>
/// </summary>
public class AvatarAnimator : MonoBehaviour
{
    [SerializeField] private AnimatorController animatorController;
    private Animator animator;

    //https://github.com/umiyuki/HumanoidHandPoseHelper/blob/master/Assets/HumanoidHandPoseHelper/HumanoidHandPoseHelper.cs

    /// <summary>
	///   <para>A dictionary where its keys in this dictionary are the names in
    ///   <see cref="HumanTrait.MuscleName"/>, and their corresponding values are
    ///   the values of those muscles in the <see cref="Animator"/>.</para>
	/// </summary>
    private Dictionary<string, float> placeValueDict;

    // The following four variables are components of signs that are specified
    // in the user's current input, and they are updated in the function ReadInput.

    private string handshape = null; // The handshape specified in the current input.
    private string[] places  = null; // The places specified in the current input.
    private string facing    = null; // The facing specified in the current input.
    private string movement  = null; // The movement specified in the current input.

    /// <summary>
    ///   <para>This is used in <see cref="ReadInput"/> to prevent redundant
    ///   modifications to <see cref="animatorController"/>.</para>
    /// </summary>
    private string lastPlayedEntry = null;

    /// <summary>
    ///   <para>The name of default entry state for the <see cref="AnimatorStateMachine"/>
    ///   that animates the hands.</para>
    /// </summary>
    /// <remarks>
    ///   Used with <see cref="lastPlayedEntry"/> in <see cref="ReadInput"/>
    ///   to prevent redundant modifications to <see cref="animatorController"/>.
    /// </remarks>
    private string startStateName;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        placeValueDict = InitPlaceValueDict();
        startStateName = animatorController.layers[1].stateMachine.states[0].state.name;
    }

    /// <summary>
	///   <para>Initializes <see cref="placeValueDict"/>.</para>
	/// </summary>
    private Dictionary<string, float> InitPlaceValueDict()
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        for (int i = 0; i < AAVariables.NumSides; i++)
        {
            // Fingers
            dict.Add(AAVariables.SideNames[i] + " Thumb 1 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Thumb 2 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Thumb 3 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Thumb Spread", 0f);
            dict.Add(AAVariables.SideNames[i] + " Index 1 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Index 2 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Index 3 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Index Spread", 0f);
            dict.Add(AAVariables.SideNames[i] + " Middle 1 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Middle 2 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Middle 3 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Middle Spread", 0f);
            dict.Add(AAVariables.SideNames[i] + " Ring 1 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Ring 2 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Ring 3 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Ring Spread", 0f);
            dict.Add(AAVariables.SideNames[i] + " Little 1 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Little 2 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Little 3 Stretched", 0f);
            dict.Add(AAVariables.SideNames[i] + " Little Spread", 0f);

            // hand; values change from the wrist
                // controls how much the wrist is bent
            dict.Add(AAVariables.SideNames[i] + " Hand Down-Up", 0f);
                // controls how much the hand is rotated
            //dict.Add(Variables.SideNames[i] + " Hand In-Out", 0f);
        }

        return dict;
    }

    /// <summary>
	///   <para>Checks input against the validation pattern.</para>
	/// </summary>
    /// <returns>
    ///   <see langword="true"/> if the input is valid, <see langword="false"/> otherwise.
    /// </returns>
	/// <param name="input">The input from the text field.</param>
    public bool ReadInput(string input)
    {
        Match match = Regex.Match(input, AAVariables.RegexPattern);
        GroupCollection groups = match.Groups;
        // The count is 1 only when the input is invalid.
        if (groups.Count <= 1) { return false; }
        // Set handshape equal to the handshape that is part of the input if it exists,
        // or an empty string otherwise.
        handshape = groups["handshape"].Success ? groups["handshape"].ToString() : string.Empty;
        // Set places equal to all specified places in the input, or null if there
        // are no places specified in the input.
        places = null;
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
                if (i != 0) { placesStr += ","; }
                placesStr += "\"" + places[i] + "\"";
            }
        }
        // Set facing equal to the facing that is part of the input if it exists,
        // or an empty string otherwise.
        facing = groups["facing"].Success ? groups["facing"].ToString() : string.Empty;
        // Set movement equal to the facing that is part of the input if it exists,
        // or an empty string otherwise.
        movement = groups["movement"].Success ? groups["movement"].ToString() : string.Empty;
        Debug.Log("handshape: \"" + handshape + "\", places: [" + placesStr +
            "], facing: \"" + facing + "\", movement: \"" + movement + "\"");

        // Note that on this computer, the program runs at roughly 80 fps
        // Currently updates both the left and right handshapes
        if (AAVariables.HandshapeDict.ContainsKey(handshape))
        {
            UpdateHandshape(AAVariables.HandshapeDict[handshape], AAVariables.Left);
            UpdateHandshape(AAVariables.HandshapeDict[handshape], AAVariables.Right);
        }
        else
        {
            Debug.Log("Key does not exist in Handshape dictionary.");
        }

        // If the user's input is the same as the last successful input entered,
        // the AnimatorController doesn't get updated with the same values as it
        // already had and instead just plays the already extant clips.
        if (lastPlayedEntry == input)
        {
            animator.Play(startStateName);
        }
        else
        {
            lastPlayedEntry = input;
            UpdateAC();
        }

        return true;
    }

    /// <summary>
	///   <para>Updates the values in <see cref="placeValueDict"/> to match a particular handshape.</para>
	/// </summary>
	/// <param name="hs">The <see cref="Handshape"/> used to update.</param>
    /// <param name="side">Which hand to update.</param>
    private void UpdateHandshape(Handshape hs, AAVariables.Side side)
    {
        for (int i = 0; i < Handshape.fingerNames.Length; i++)
        {
            string keyStart = side.ToString() + " " + Handshape.fingerNames[i];
            placeValueDict[keyStart + " 1 Stretched"] = hs.FingerProperties[i, 0];
            placeValueDict[keyStart + " 2 Stretched"] = hs.FingerProperties[i, 1];
            placeValueDict[keyStart + " 3 Stretched"] = hs.FingerProperties[i, 2];
            placeValueDict[keyStart + " Spread"] = hs.FingerProperties[i, 3];
        }
    }

    // ** Currently unusued.
    [SerializeField] private TwoBoneIKConstraint leftHandConstraint;
    // The IK target for the left hand.
    [SerializeField] private Transform leftHandTarget;
    // ** Currently unusued.
    [SerializeField] private TwoBoneIKConstraint rightHandConstraint;
    // The IK target for the right hand.
    [SerializeField] private Transform rightHandTarget;

    // When entering input for the first time, some things need to be set up
    // differently than later.
    private bool firstTimeCalled = true;
    // Exists just to give a different name to each animation
    private int animCount = 0;

    // This clip is used for the default entry state in the AnimationController,
    // which is generally derived from the clip in the state that it transitions into.
    private AnimationClip oldClip = null;
    // ** Need to explain these
    private AnimationClip clipA = null, clipB = null;

    /// <summary>
	///   <para> Updates the <see cref="AnimatorController"/> with clips that reflect user input.</para>
	/// </summary>
    public void UpdateAC()
    {
        // AnimatorController.layers returns a copy, so the changed variable needs
        // to be referentiable for later reassignment.
        AnimatorControllerLayer[] layers = animatorController.layers;
        AnimatorStateMachine rootStateMachine = layers[1].stateMachine;

        // ** left-assuming
        string leftPlace = places == null ? "0l" : places[0];
        //string rightPlace = places == null ? "0r" : places[0];

        Vector3 newLeftPosition;
        if (movement != string.Empty)
        {
            newLeftPosition = GetPositionOfPlace(leftPlace) +
                AAVariables.Translation(movement);
        }
        else if (places != null && places.Length > 1)
        {
            newLeftPosition = GetPositionOfPlace(places[1]);
        }
        else //do nothing
        {
            newLeftPosition = leftHandTarget.position;
        }
        

        leftHandTarget.position = newLeftPosition;
        // ** left-assuming
        rightHandTarget.position = GetPositionOfPlace("0r");

        // oldClip is set to be the previous unweighted clip when it is not the
        // first time that the user entered input.
        oldClip = firstTimeCalled ? oldClip : clipA;
        // Most generally, if there should be no movement
        if (string.IsNullOrEmpty(movement) && (places == null || places.Length <= 1))
        {
            clipA = ExportHandPose("AnimationClip " + ++animCount, 1f);
            clipB = clipA;
        }
        else
        {
            clipA = ExportHandPose("AnimationClip " + animCount, 1f);
            clipB = ExportHandPose("AnimationClip " + ++animCount, 0f);
        }
        // oldClip is unchanged when it is not the first time that the user entered input.
        oldClip = firstTimeCalled ? clipA : oldClip;

        // The default entry state has its animation changed to be the clip
        // played by the state that it transitions into
        rootStateMachine.states[0].state.motion = oldClip;
        rootStateMachine.states[1].state.motion = clipB;

        // The layers of the AnimationController are updated.
        animatorController.layers = layers;
        firstTimeCalled = false;
    }

    /// <summary>
	///   <para>Creates an <see cref="AnimationClip"/> object based off of the
    ///   current values of the avatar.</para>
	/// </summary>
    /// <remarks>This is mostly adapted from HandshapeHelper.</remarks>
    public AnimationClip ExportHandPose(string clipName, float weight)
    {
        AnimationClip clip = new AnimationClip { frameRate = 30 };
        clip.name = clipName;
        AnimationUtility.SetAnimationClipSettings(clip, new AnimationClipSettings { loopTime = true });

        for (int i = 0; i < HumanTrait.MuscleCount; i++)
        {
            string muscle = HumanTrait.MuscleName[i];
            if (AAVariables.PlaceNamesDict.ContainsKey(muscle))
            {
                AnimationCurve curve = new AnimationCurve();
                curve.AddKey(0f, placeValueDict[muscle]);
                curve.postWrapMode = WrapMode.Once;

                string musclePropName = AAVariables.PlaceNamesDict[muscle];
                clip.SetCurve("", typeof(Animator), musclePropName, curve);
            }
        }

        // ** Currently works if specifying a non-place before a place or vice versa
        // ** Might be different now
        AnimationCurve IKcurve = new AnimationCurve();
        IKcurve.AddKey(0f, weight);
        IKcurve.postWrapMode = WrapMode.Once;
        clip.SetCurve("", typeof(Animator), "HandIKWeight", IKcurve);

        return clip;
    }

    void OnAnimatorIK(int layerIndex)
    {
        SetIK(AAVariables.Left);
        SetIK(AAVariables.Right);
    }

    /// <summary>
	///   <para>This function changes the IK weights for the left and right sides
    ///   of the body, including target position, hint position, and rotation.</para>
	/// </summary>
    private void SetIK(AAVariables.Side side)
    {
        // Constraint weights are weaker than function-driven weights

        // ** At present, just gets the first place of the places input.
        // If there are no no places specified, neutral space is used instead.
        string place = places != null ? places[0] : (side == AAVariables.Left ? "0l" : "0r");

        //// Gets the offset of the target for a particular place.
        //Vector3 targetOffset = Variables.PlaceValueOffsets[place].Item1;
        // Gets the offset of the hint for a particular place.
        Vector3? hintOffset = AAVariables.PlaceValueOffsets[place].Item2;

        AvatarIKGoal hand = side == AAVariables.Left ?
            AvatarIKGoal.LeftHand : AvatarIKGoal.RightHand;

        // ** left-assuming
        float handIKWeight = side == AAVariables.Left ? animator.GetFloat("HandIKWeight") : 0f;

        if (hintOffset != null)
        {
            AvatarIKHint elbow = side == AAVariables.Left ?
                AvatarIKHint.LeftElbow : AvatarIKHint.RightElbow;
            animator.SetIKHintPositionWeight(elbow, handIKWeight);
            animator.SetIKHintPosition(elbow, (Vector3)hintOffset);
        }

        animator.SetIKPositionWeight(hand, handIKWeight);
        Vector3 position = GetPositionOfPlace(place);

        if (!string.IsNullOrEmpty(facing))
        {
            Quaternion rotation = AAVariables.Rotation(facing);
            animator.SetIKRotationWeight(hand, 1f);
            animator.SetIKRotation(hand, rotation);
        }
        else if (places == null) // sets up fingerspelling orientation
        {
            animator.SetIKRotationWeight(hand, 1f);
            animator.SetIKRotation(hand, Quaternion.Euler(-90f, 0f, 0f));
        }

        // ** Additional offsets for particular places. Places by default are
        // positioned near the surface of ybot's body, and these move them away
        // so that the hands aren't phasing through the body.
        switch (place)
        {
            case "m":
            case "p":
            case "b":
            case "f":
            case "v":
                position += new Vector3(0f, 0f, 0.05f);
                break;
            case "n":
            case "t":
            case "d":
            case "s":
            case "z":
                position += new Vector3(0f, 0f, 0.08f);
                break;
            case "θl":
            case "ðl":
                position += new Vector3(-0.1f, 0f, 0f);
                break;
            case "θr":
            case "ðr":
                position += new Vector3(0.1f, 0f, 0f);
                break;
            default:
                //Debug.Log("Throw an error in the future; places not working");
                break;
        }

        animator.SetIKPosition(hand, position);
    }

    private Vector3 GetPositionOfPlace(string p)
    {
        /*
        // Finger symbols
            // Left thumb
            { "ŋ1l", incompletePosition }, // further beyond distal
            { "k1l", incompletePosition }, // beyond distal
            { "g1l", incompletePosition }, // between distal and prox.
            { "x1l", incompletePosition }, // between prox. and intermediate
            { "ɣ1l", incompletePosition },
        */
        string place = p.Substring(0, 1), side = p.Substring(p.Length - 1, 1);
        string fingerNumber = side == "l" || side == "r" ? p.Substring(1, 1) : string.Empty;
        //HumanBodyBones[] fingerBones = { HumanBodyBones.LeftIndexDistal };
        switch (place)
        {
            case "c":
                return GetMidpointPlace("ɲ" + side, "ɟ" + side);
            case "ʃ":
                return GetMidpointPlace("ɟ" + side, "ʒ" + side);
            case "q":
                return GetMidpointPlace("ɴ" + side, "ɢ" + side);
            case "χ":
                return GetMidpointPlace("ɢ" + side, "ʁ" + side);
            case "g":
                return GetMidpointPlace("k" + fingerNumber + side, "g" + fingerNumber + side);
            case "x":
                return GetMidpointPlace("g" + fingerNumber + side, "x" + fingerNumber + side);
            default:
                return animator.GetBoneTransform(AAVariables.SymbolBoneDict[p]).position +
                    AAVariables.PlaceValueOffsets[p].Item1;
        }
    }

    private Vector3 GetMidpointPlace(string a, string b)
    {
        Vector3 endpointPositionA =
            animator.GetBoneTransform(AAVariables.SymbolBoneDict[a]).position +
            AAVariables.PlaceValueOffsets[a].Item1;
        Vector3 endpointPositionB =
            animator.GetBoneTransform(AAVariables.SymbolBoneDict[b]).position +
            AAVariables.PlaceValueOffsets[b].Item1;
        return (endpointPositionA + endpointPositionB) / 2;
    }

    private Vector3 GetMidpointPlace(HumanBodyBones a, HumanBodyBones b)
    {
        Vector3 endpointPositionA = animator.GetBoneTransform(a).position;
        Vector3 endpointPositionB = animator.GetBoneTransform(b).position;
        return (endpointPositionA + endpointPositionB) / 2;
    }

}
