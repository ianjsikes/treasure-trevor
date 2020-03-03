using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Movement))]
public class FootStepper : MonoBehaviour
{
  public Foot leftFoot;
  public Foot rightFoot;
  public Transform leftHip;
  public Transform rightHip;
  public Transform bodyObj;
  public float baseMaxLegLength = 1.2f;
  [HideInInspector]
  public float maxLegLength
  {
    get
    {
      return Mathf.Max(baseMaxLegLength * crouchFactor, 0.75f);
    }
  }
  public float stepSpeed = 15f;
  private bool leftTurn = true;
  public float capsuleHeight = 2f;
  // Default distance from floor to bottom of body
  public float baseHeight = 0.5f;
  public float baseStepJitter = 0.2f;
  [HideInInspector]
  public float stepJitter
  {
    get
    {
      return baseStepJitter * crouchFactor;
    }
  }

  // Modifies the baseHeight
  public float baseStepBounceAmt = 0.3f;
  public float stepBounceAmt
  {
    get
    {
      return baseStepBounceAmt * crouchFactor;
    }
  }
  public float bounceOffset;
  public float crouchOffset;
  // Amount the player is crouching, in [0, 1] range
  public float crouchFactor
  {
    get
    {
      return 1f - crouchOffset / baseHeight;
    }
  }

  CharacterController controller;
  CapsuleCollider capsule;
  Movement movement;
  int groundMask;
  // Start is called before the first frame update
  void Start()
  {
    groundMask = 1 << 2;
    groundMask = ~groundMask;

    controller = GetComponent<CharacterController>();
    movement = GetComponent<Movement>();
    capsule = GetComponent<CapsuleCollider>();
    capsule.height = capsuleHeight;
    controller.height = capsuleHeight + baseHeight;
    controller.center = new Vector3(0f, -baseHeight, 0f);
  }

  // Update is called once per frame
  void Update()
  {
    float bodyOffset = bounceOffset - crouchOffset;
    bodyObj.localPosition = bodyObj.localPosition.WithY(bodyOffset);

    // RaycastHit hit = 
    if (leftTurn)
    {
      UpdateFoot(leftFoot, leftHip);
    }
    else
    {
      UpdateFoot(rightFoot, rightHip);
    }

    if (Input.GetKey(KeyCode.Q))
    {
      DOTween.To(() => crouchOffset, x => crouchOffset = x, 0.3f, 0.5f);
      // DOTween.To(() => maxLegLength, x => maxLegLength = x, 0.7f, 0.5f);
      // DOTween.To(() => stepJitter, x => stepJitter = x, 0.05f, 0.5f);
    }

    if (Input.GetKey(KeyCode.W))
    {
      DOTween.To(() => crouchOffset, x => crouchOffset = x, 0f, 0.5f);
      // DOTween.To(() => maxLegLength, x => maxLegLength = x, 1.2f, 0.5f);
      // DOTween.To(() => stepJitter, x => stepJitter = x, 0.2f, 0.5f);
    }
  }

  void UpdateFoot(Foot foot, Transform hip)
  {
    if (!leftFoot.stepping && !rightFoot.stepping)
    {
      // NOTE: This threshold method is probably "better" for leg accuracy
      // because it works well with different elevations. But it makes visualizing
      // the leg boundaries harder.
      // float legLength = Vector3.Distance(hip.position, foot.transform.position);
      // if (legLength > maxLegLength)

      // TODO: Probably safer to just raycast
      float distToGround = (capsuleHeight / 2f) + ((baseHeight + bounceOffset - crouchOffset) * 1.5f);
      Vector3 groundPos = transform.position.AddY(-distToGround);
      float stepDist = Vector3.Distance(groundPos, foot.transform.position);
      if (stepDist > maxLegLength)
      {
        leftTurn = !leftTurn;
        Vector3 dir = hip.position + (movement.velocity.normalized * maxLegLength * 1.3f);
        dir.y += 100f;
        dir += GetRandomOffset();

        int layerMask = 1 << 2;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(dir, Vector3.down, out hit, 1000f, layerMask))
        {
          foot.StepTo(hit.point, stepSpeed);
        }
      }
    }
  }

  Vector3 GetRandomOffset()
  {
    var rand = Random.insideUnitCircle;
    var mag = Random.Range(0f, stepJitter);
    return new Vector3(rand.x * mag, 0f, rand.y * mag);
  }
}
