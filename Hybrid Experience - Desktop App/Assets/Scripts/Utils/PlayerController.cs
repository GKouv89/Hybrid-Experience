using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace DIMuseumVR.Utils
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float movingSpeed = 3;

        [SerializeField] private Transform cameraHolder;
        [SerializeField] private float cameraUpLimit = -50;
        [SerializeField] private float cameraDownLimit = 50;
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private bool hideMouseCursor = false;

        private float gravity = -10.0f;
        private float velocityY = 0.0f;
        private float jumpHeight = 1.0f;

        //public Animator animator;

        private void Awake()
        {
            if (hideMouseCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Update()
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        private void LateUpdate()
        {
            Rotate();
        }

        private void Move()
        {
            float horizontalMove = Input.GetAxis("Horizontal");
            float verticalMove = Input.GetAxis("Vertical");

            if (characterController.isGrounded && velocityY < 0)
                velocityY = 0.0f;

            velocityY += gravity * Time.deltaTime;

            if (Input.GetButtonDown("Jump") && characterController.isGrounded)
            {
                velocityY += Mathf.Sqrt(jumpHeight * -1.0f * gravity);
            }

            Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove + Vector3.up * velocityY;
            characterController.Move(move * movingSpeed * Time.deltaTime);

            //animator.SetBool("walk", verticalMove != 0 || horizontalMove != 0);
        }

        private void Rotate()
        {
            float horizontalRotation = Input.GetAxis("Mouse X");
            float verticalRotation = Input.GetAxis("Mouse Y");

            transform.Rotate(0, horizontalRotation * mouseSensitivity, 0);
            cameraHolder.Rotate(-verticalRotation * mouseSensitivity, 0, 0);

            Vector3 currentRotation = cameraHolder.localEulerAngles;
            if (currentRotation.x > 180)
                currentRotation.x -= 360;
            currentRotation.x = Mathf.Clamp(currentRotation.x, cameraUpLimit, cameraDownLimit);
            cameraHolder.localRotation = Quaternion.Euler(currentRotation);
        }
    }
}