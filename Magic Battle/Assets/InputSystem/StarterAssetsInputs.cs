using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aiming;
		public bool atack;
		public bool health;
		public bool buffing;
		public bool throwPotion;
		public bool block;
		public bool megaHit;
		public bool crouch;
		public bool pause;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAiming(InputValue value)
		{
			aiming = value.isPressed;
		}
		public void OnAtack(InputValue value)
		{
			atack = value.isPressed;
		}
		public void OnHealth(InputValue value)
		{
			health = value.isPressed;
		}
		public void OnBuffing(InputValue value)
		{
			buffing = value.isPressed;
		}
		public void OnThrowPotion(InputValue value)
		{
			throwPotion = value.isPressed;
		}
		public void OnBlock(InputValue value)
		{
			block = value.isPressed;
		}
		public void OnMegaHit(InputValue value)
		{
			megaHit = value.isPressed;
		}
		public void OnCrouch(InputValue value)
		{
			crouch = value.isPressed;
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}