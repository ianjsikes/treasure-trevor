using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Foot : MonoBehaviour
{
  [HideInInspector]
  public Vector3 startOffset;
  [HideInInspector]
  public float startDistance;
  public float stepHeight = 2f;
  [HideInInspector]
  public bool stepping = false;

  [Space]
  public Color legColor = Color.red;
  public int legSegments = 20;
  public float legWidth = 0.2f;
  public Transform hip;

  float kneeRaisedAmt = 0f;
  public float maxKneeHeight = 2f;
  GameObject knee;
  public Transform body;
  LineRenderer lineRenderer;
  FootStepper footStepper;

  void Start()
  {
    footStepper = body.GetComponent<FootStepper>();

    GameObject leg = new GameObject("leg");
    leg.transform.SetParent(transform.parent);
    leg.transform.localPosition = Vector3.zero;

    lineRenderer = leg.AddComponent<LineRenderer>();
    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    lineRenderer.widthMultiplier = legWidth;
    lineRenderer.positionCount = legSegments;
    lineRenderer.startColor = lineRenderer.endColor = legColor;

    knee = new GameObject("knee");
    knee.transform.SetParent(body);
    knee.transform.position = Vector3.Lerp(hip.position, transform.position, 0.5f);
  }

  void Awake()
  {
    startOffset = transform.localPosition;
    startDistance = startOffset.magnitude;
  }

  void Update()
  {
    // float crouchAmount = 1f - footStepper.height / footStepper.baseHeight;
    float crouchFactor = maxKneeHeight * footStepper.crouchOffset;

    knee.transform.position = Vector3.Lerp(hip.position, transform.position, 0.5f);
    knee.transform.localPosition += Vector3.forward * Mathf.Clamp(kneeRaisedAmt + crouchFactor, 0f, maxKneeHeight);

    for (int i = 0; i < legSegments; i++)
    {
      float progress = i / (float)legSegments;
      Vector3 point = Bezier.GetPoint(hip.position, knee.transform.position, transform.position, progress);
      lineRenderer.SetPosition(i, point);
    }
  }

  public void StepTo(Vector3 pos, float speed)
  {
    if (stepping)
    {
      return;
    }
    stepping = true;
    float stepMaxY = Mathf.Max(pos.y, transform.position.y) + stepHeight;
    float stepDistance = Vector3.Distance(pos, transform.position);
    float stepTime = stepDistance / speed;

    Sequence mySequence = DOTween.Sequence();
    mySequence.Append(transform.DOMoveY(stepMaxY, stepTime / 2).SetEase(Ease.OutSine));
    mySequence.Append(transform.DOMoveY(pos.y, stepTime / 2).SetEase(Ease.InSine));

    mySequence.Insert(0, transform.DOMoveX(pos.x, stepTime).SetEase(Ease.InOutSine));
    mySequence.Insert(0, transform.DOMoveZ(pos.z, stepTime).SetEase(Ease.InOutSine));

    // DOTween.To(()=> myFloat, x=> myFloat = x, 52, 1);
    mySequence.Insert(0, DOTween.To(() => kneeRaisedAmt, x => kneeRaisedAmt = x, maxKneeHeight, stepTime / 2).SetEase(Ease.OutSine));
    mySequence.Insert(stepTime / 2, DOTween.To(() => kneeRaisedAmt, x => kneeRaisedAmt = x, 0f, stepTime / 2).SetEase(Ease.InSine));

    Sequence bounceSequence = DOTween.Sequence();
    bounceSequence.Insert(0, DOTween.To(() => footStepper.bounceOffset, x => footStepper.bounceOffset = x, footStepper.stepBounceAmt, stepTime * 0.65f).SetEase(Ease.InOutSine));
    bounceSequence.Insert(stepTime * 0.65f, DOTween.To(() => footStepper.bounceOffset, x => footStepper.bounceOffset = x, 0f, stepTime * 0.65f).SetEase(Ease.InOutSine));

    mySequence.OnComplete(() =>
    {
      stepping = false;
    });
  }
}
