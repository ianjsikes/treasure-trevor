
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
  public float speed = 10.0f;
  public float jumpVelocity = 10.0f;
  CharacterController controller;
  public Vector3 velocity;

  // Use this for initialization
  void Start()
  {
    controller = GetComponent<CharacterController>();
  }

  // Update is called once per frame
  void Update()
  {
    if (controller.isGrounded)
    {
      Vector3 inputDir = new Vector3(
          Input.GetAxis("Horizontal"),
          0,
          Input.GetAxis("Vertical")
      );
      velocity = transform.TransformDirection(inputDir) * speed;

      if (Input.GetKey(KeyCode.Space))
        velocity.y = 10.0f;
    }

    velocity.y -= 20.0f * Time.deltaTime;

    controller.Move(velocity * Time.deltaTime);
  }
}
