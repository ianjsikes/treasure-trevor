using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour
{
  public RobotJoint[] Joints;
  public Transform Effector;
  public float SamplingDistance = 0.1f;
  public float LearningRate = 1f;
  public float DistanceThreshold = 1f;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public Vector3 ForwardKinematics(float[] angles)
  {
    Vector3 prevPoint = Joints[0].transform.position;
    Quaternion rotation = Quaternion.identity;
    for (int i = 1; i < Joints.Length; i++)
    {
      rotation *= Quaternion.AngleAxis(angles[i - 1], Joints[i - 1].Axis);
      Vector3 nextPoint = prevPoint + rotation * Joints[i].StartOffset;
      prevPoint = nextPoint;
    }
    return prevPoint;
  }

  public float DistanceFromTarget(Vector3 target, float[] angles)
  {
    Vector3 point = ForwardKinematics(angles);
    return Vector3.Distance(point, target);
  }

  public float PartialGradient(Vector3 target, float[] angles, int i)
  {
    float angle = angles[i];

    float f_x = DistanceFromTarget(target, angles);
    angles[i] += SamplingDistance;
    float f_x_plus_d = DistanceFromTarget(target, angles);
    float gradient = (f_x_plus_d - f_x) / SamplingDistance;

    angles[i] = angle;
    return gradient;
  }

  public void InverseKinematics(Vector3 target, float[] angles)
  {
    if (DistanceFromTarget(target, angles) < DistanceThreshold)
      return;

    for (int i = Joints.Length - 1; i >= 0; i--)
    {
      float gradient = PartialGradient(target, angles, i);
      angles[i] -= LearningRate * gradient;
      angles[i] = Mathf.Clamp(angles[i], Joints[i].MinAngle, Joints[i].MaxAngle);

      if (DistanceFromTarget(target, angles) < DistanceThreshold)
        return;
    }
  }
}
