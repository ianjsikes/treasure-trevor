using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FootStepper))]
public class FootStepperEditor : Editor
{
  private void OnSceneGUI()
  {
    FootStepper stepper = target as FootStepper;
    var pos = stepper.transform.position;
    var bottomDst = stepper.capsuleHeight / 2f;
    var floorDst = bottomDst + stepper.baseHeight * 1.5f;

    var bottomOfBodyPos = pos.AddY(-bottomDst);
    var floorPos = pos.AddY(-floorDst);

    Handles.color = Color.white;
    Handles.DrawLine(bottomOfBodyPos, floorPos);

    Handles.color = Color.red;
    Handles.DrawWireDisc(floorPos, Vector3.up, stepper.maxLegLength);
  }
}
