using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Tooltip("Sencetivity while aiming and vice versa")]
        public float Sensitivity = 1.0f;
        public float UsualSensitivity;
        public float AimingSensitvity;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        public GameObject spellObject;
        public GameObject spellEffect;
        public Transform spellPoint;
        public GameObject playerFollowCamera;
        public GameObject playerAimCamera;
        public LayerMask aimColliderMask = new LayerMask();
        public Transform debugTransform;
        public Transform pfSpellProjectile;
        private Vector3 mouseWorldPosition;
        public GameObject healthRecoilObject;
        public Transform healthRecoilPoint;

        public GameObject buffRecoilObject;
        public Transform buffRecoilPoint;

        //Combo Atack
        public float atackCooldownTime = 0.5f;
        private float nextAtack = 0f;
        public static int numOfClicks = 0;
        private float lastClickedTime = 0f;
        private float maxComboDelay = 1.7f;
        public GameObject firstHit;
        public GameObject secondHit;
        public GameObject lastHit;

        public GameObject hitObject2;
        public int _power;

        public Transform secondHitPoint;
        public Transform hitPoint1;
        public Transform hitPoint2;
        public Transform hitPoint2Direction;
        public Transform hitPoint31;
        public Transform hitPoint32;
        public Transform hitPoint33;

        private float ClickCooldownTime = 0.2f;
        private float cooldownUntilNextClick = 0.01f;

        //Magic field 
        public GameObject shieldObject;
        public GameObject shieldCharge;
        public float timeOfShieldLife = 10f;
        private Vector3 shieldStamina;
        public Slider _shieldBar;
        private float shieldCooldown = 2.5f;
        private float shieldCooldownUntilNextRecoiling = 0.01f;

        private const float _threshold = 0.01f;

        private float CooldownTime = 0.8f;
        private float cooldownUntilNextPress = 0.01f;

        private bool _hasAnimator;

        //Player Health
        public int playerHP;
        public Slider _healthBar;

        //Player Power Stamina 
        public float playerPowerStamina;
        public Slider _powerBar;

        public bool isMegaPowerActive;
        private float PowerCooldownTime = 0.3f;
        private float PowerCoolDownUntilNextPress = 0.01f;
        public GameObject megaHitObject;
        public GameObject powerAuraEffect;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            playerHP = PlayerDataHolder.hp;

            _healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
            _powerBar = GameObject.FindGameObjectWithTag("PowerBar").GetComponent<Slider>();
            _shieldBar = GameObject.FindGameObjectWithTag("ShieldBar").GetComponent<Slider>();

            _healthBar.maxValue = PlayerDataHolder.hp;
            _power = PlayerDataHolder.power;
            SprintSpeed = PlayerDataHolder.speed;
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            shieldStamina = new Vector3(8.8f, 5.6f, 100f);
            numOfClicks = 0;
            playerPowerStamina = 100f;
            isMegaPowerActive = false;


        }

        private void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Standing Death Right 02"))
            {
                return;
            }
            _healthBar.value = playerHP;
            _hasAnimator = TryGetComponent(out _animator);
            _shieldBar.value = shieldStamina.z;
            _powerBar.value = playerPowerStamina;

            JumpAndGravity();
            GroundedCheck();
            Move();
            AimAtack();
            Health();
            Buffing();
            Block();
            MegaHit();
            Crouch();

            if (_animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.6f && _animator.GetCurrentAnimatorStateInfo(1).IsName("Hit1"))
            {
                _animator.SetBool("Hit1", false);
            }
            if (_animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.6f && _animator.GetCurrentAnimatorStateInfo(1).IsName("Hit2"))
            {
                _animator.SetBool("Hit2", false);
            }
            if (_animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.6f && _animator.GetCurrentAnimatorStateInfo(1).IsName("Hit3"))
            {
                _animator.SetBool("Hit3", false);
                numOfClicks = 0;
            }
            if (Time.time - lastClickedTime > maxComboDelay)
            {
                numOfClicks = 0;
            }
            if (Time.time > nextAtack)
            {
                ComboAtack();
            }
            hitObject2.GetComponent<CloseHitCollider>().isMega = isMegaPowerActive;
            spellObject.GetComponent<Spells>().isMega = isMegaPowerActive;
        }
        public void GetDamage(int damage)
        {
            playerHP -= damage;
            if (playerHP <= 0)
            {
                _animator.SetTrigger("Die");
                _healthBar.gameObject.SetActive(false);
                GetComponent<Collider>().enabled = false;
                _mainCamera.GetComponent<PlayerDataController>().isDeath = true;
                _mainCamera.GetComponent<PlayerDataController>().Invoke("SpentPotions", 5f);
            }
            else
            {
                if (damage <= 15)
                {
                    _animator.SetTrigger("SmallDamage");
                }
                else if (damage > 15)
                {
                    _animator.SetTrigger("BigDamage");
                }
            }
        }

        private void AimAtack()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Standing Death Right 02"))
            {
                return;
            }
            mouseWorldPosition = Vector3.zero;
            Vector2 centerPointOnScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
            {
                debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }  
            if (_input.aiming && Grounded && !_input.sprint)
            {
                Cursor.lockState = CursorLockMode.Locked;
                _animator.SetBool("Aiming", _input.aiming);
                _animator.SetBool("Atack", _input.atack);
                playerFollowCamera.SetActive(false);
                playerAimCamera.SetActive(true);
                SetSensitivity(AimingSensitvity);
                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            }
            else
            {
                _animator.SetBool("Aiming", false);
                _animator.SetBool("Atack", false);
                playerAimCamera.SetActive(false);
                playerFollowCamera.SetActive(true);
                SetSensitivity(UsualSensitivity);
            }
        }
        private void Fire()
        {
            Vector3 aimDirection = (mouseWorldPosition - spellPoint.position).normalized;
            Vector3 effectCorrection = new Vector3(0f, 0.3f, 0f);
            GameObject spell = Instantiate(spellObject, spellPoint.position, transform.rotation);
            GameObject effects = Instantiate(spellEffect, spellPoint.position + effectCorrection, transform.rotation);
            spell.GetComponent<Rigidbody>().AddForce(aimDirection *50f, ForceMode.Impulse);
            Destroy(effects, 2f);
        }
        private void ComboAtack()
        {
            if (_input.atack && Grounded && !_input.aiming && !_input.throwPotion && cooldownUntilNextClick < Time.time)
            {
                cooldownUntilNextClick = Time.time + ClickCooldownTime;
                lastClickedTime = Time.time;
                numOfClicks++;
                if (numOfClicks == 1)
                {
                    _animator.SetBool("Hit1", true);
                }
                numOfClicks = Mathf.Clamp(numOfClicks, 0, 3);
                if (numOfClicks >= 2 && _animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.6f && _animator.GetCurrentAnimatorStateInfo(1).IsName("Hit1"))
                {
                    _animator.SetBool("Hit1", false);
                    _animator.SetBool("Hit2", true);
                }
                if (numOfClicks >= 3 && _animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.6f && _animator.GetCurrentAnimatorStateInfo(1).IsName("Hit2"))
                {
                    _animator.SetBool("Hit2", false);
                    _animator.SetBool("Hit3", true);
                }
            }
            Debug.Log(numOfClicks);
        }
        private void FirstHit()
        {
            Vector3 hitEffectCorrection = new Vector3(-0.3f, -0.5f, 0.5f);
            GameObject fh = Instantiate(firstHit, secondHitPoint.position + hitEffectCorrection, transform.rotation);
            GameObject h1 = Instantiate(hitObject2, hitPoint1.position, transform.rotation);
            fh.GetComponent<Rigidbody>().AddForce(fh.transform.forward * 3.5f, ForceMode.Impulse);
            h1.GetComponent<Rigidbody>().AddForce(h1.transform.forward * 4.0f, ForceMode.Impulse); 
            Destroy(fh, 1f);
        }
        private void SecondHit()
        {
            GameObject sh = Instantiate(secondHit, secondHitPoint.position, transform.rotation);
            GameObject h2 = Instantiate(hitObject2, hitPoint2.position, transform.rotation);
            h2.GetComponent<Rigidbody>().AddForce((hitPoint2Direction.position - secondHitPoint.position) * 4f, ForceMode.Impulse);
            Destroy(sh, 1f);
        }
        private void ThirdHit()
        {
            playerFollowCamera.transform.position = Vector3.back;
            Vector3 effectCorrection = new Vector3(0f, -3.3f, 0f);
            GameObject hitEffect = Instantiate(lastHit, hitPoint31.position + effectCorrection, transform.rotation);
            hitEffect.GetComponent<Rigidbody>().AddForce(hitEffect.transform.up * 3.8f, ForceMode.Impulse);
            GameObject h31 = Instantiate(hitObject2, hitPoint31.position, transform.rotation);
            GameObject h32 = Instantiate(hitObject2, hitPoint32.position, transform.rotation);
            GameObject h33 = Instantiate(hitObject2, hitPoint32.position, transform.rotation);
            Destroy(hitEffect, 0.6f);
        }
        private void Health()
        {
            if (_input.health && Grounded && !_input.aiming && !_input.buffing && cooldownUntilNextPress < Time.time)
            {
                if (_mainCamera.GetComponent<PlayerDataController>().healthPotionsQuant > 0)
                {
                    cooldownUntilNextPress = Time.time + CooldownTime;
                    GameObject healthRecoil = Instantiate(healthRecoilObject, healthRecoilPoint.position, transform.rotation);
                    Destroy(healthRecoil, 0.7f);
                    if (playerHP > 50)
                    {
                        playerHP = (int)_healthBar.maxValue;
                    }
                    else
                    {
                        playerHP *= 2;
                    }
                    _mainCamera.GetComponent<PlayerDataController>().healthPotionsQuant--;
                }
            }
        }
        private void Buffing()
        {
            if (_input.buffing && Grounded && !_input.aiming && !_input.health && cooldownUntilNextPress < Time.time)
            {
                if (_mainCamera.GetComponent<PlayerDataController>().buffPotionsQuant > 0)
                {
                    cooldownUntilNextPress = Time.time + CooldownTime;
                    GameObject buffRecoil = Instantiate(buffRecoilObject, buffRecoilPoint.position, transform.rotation);
                    Destroy(buffRecoil, 1.2f);
                    shieldStamina.z += 80;
                    playerPowerStamina *= 1.5f;
                    if (playerPowerStamina >= 100)
                    {
                        playerPowerStamina = 100;
                    }
                    _mainCamera.GetComponent<PlayerDataController>().buffPotionsQuant--;
                }
            }
        }

        private void Block()
        {
            if (_input.block && Grounded! && !_input.aiming && !_input.sprint && !_input.jump && shieldStamina.z > 0.1f)
            {
                _animator.SetBool("Block", _input.block);
                shieldObject.SetActive(true);
                shieldStamina = new Vector3(shieldStamina.x, shieldStamina.y, shieldStamina.z - 0.1f);
            }
            else
            {
                _animator.SetBool("Block", false);
                shieldObject.SetActive(false);
                if (shieldCooldownUntilNextRecoiling < Time.time)
                {
                    shieldCooldownUntilNextRecoiling = Time.time + shieldCooldown;
                    if (shieldStamina.z < 100f)
                    {
                        shieldStamina.z += 5f;
                        shieldCharge.SetActive(true);
                    }
                    else if (shieldStamina.z > 100f)
                    {
                        shieldStamina.z = 100f;
                        shieldCharge.SetActive(false);
                    }
                }
            }
        }
        private void MegaHit()
        {
            if (_input.megaHit && Grounded && PowerCoolDownUntilNextPress < Time.time)
            {
                PowerCoolDownUntilNextPress = Time.time + PowerCooldownTime;
                if (isMegaPowerActive == false)
                {
                    if (playerPowerStamina > 0)
                    {
                        isMegaPowerActive = true;
                        if (playerPowerStamina == 100)
                        {
                            _animator.SetTrigger("MegaHit");
                        }
                        powerAuraEffect.SetActive(true);
                    }
                }
                else
                {
                    isMegaPowerActive = false;
                    powerAuraEffect.SetActive(false);
                }
            }
            if (isMegaPowerActive == true)
            {
                playerPowerStamina -= 0.01f;
            }
            if (playerPowerStamina <= 0)
            {
                isMegaPowerActive = false;
                powerAuraEffect.SetActive(false);
            }
        }
        private void MegaHitRelease()
        {
            Vector3 correction = new Vector3 (0, 0.3f, 0);
            GameObject mh = Instantiate(megaHitObject, transform.position + correction, transform.rotation);
            Destroy(mh, 3f);
        }
        private void Crouch()
        {
            if (_input.crouch && Grounded && !_input.aiming && !_input.sprint)
            {
                _animator.SetBool("Crouch", _input.crouch);
            }
            else
            {
                _animator.SetBool("Crouch", false);
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * Sensitivity;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * Sensitivity;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // suspend moving during combat 
            if (_animator.GetCurrentAnimatorStateInfo(1).IsName("Hit1") || _animator.GetCurrentAnimatorStateInfo(1).IsName("Hit2") || _animator.GetCurrentAnimatorStateInfo(1).IsName("Hit3"))
            {
                return;
            }
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Standing Death Right 02"))
            {
                return;
            }
            if (_animator.GetCurrentAnimatorStateInfo(1).IsName("MegaAttack"))
            {
                return;
            }

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void SetSensitivity(float newSens)
        {
            Sensitivity = newSens;
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}