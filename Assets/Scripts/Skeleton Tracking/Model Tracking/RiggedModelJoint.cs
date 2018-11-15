﻿using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;

public class RiggedModelJoint : MonoBehaviour {

    public Transform Bone;
    public JointType JointType;
    [HideInInspector] public Quaternion BaseRotOffset;
}