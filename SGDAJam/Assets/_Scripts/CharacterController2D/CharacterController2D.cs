using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MichaelWolfGames.CC2D
{
    /// <summary>
    /// Core class for CharacterController2D.
    /// Responsibilities:
    /// -
    /// 
    /// Michael Wolf
    /// May, 2018
    /// </summary>
    public class CharacterController2D : MonoBehaviour
    {
        // EXPERIMENTAL APPROACH
        public Func<float, Vector2> UpdateMovementFunc = (deltaTime) => { return Vector2.zero; };

        public void SetUpdateMovementFunc(Func<float, Vector2> updateFunc, string funcName = null)
        {
            if(funcName != null)
                Debug.Log("Setting Movement Func: " + funcName);
            if (updateFunc != null)
            {
                UpdateMovementFunc = updateFunc;
            }
        }

        #region Events

        public event Action OnBecomeGrounded = delegate { };
        public event Action OnBecomeUngrounded = delegate { };

        public event Action<bool> OnChangeFacingDirection = delegate { };

        #endregion

        #region Fields
        // By Default, using a Capsule collider... (refactor later?)
        public CapsuleCollider2D capsuleCollider;
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] protected Animator _animator;

        [Header("Movement")]
        [SerializeField] protected float _maxHorzSpeed = 10f;
        [SerializeField] protected float _maxVertSpeed = 10f;
        [Header("Grounding")]
        [SerializeField] protected LayerMask _groundMask;
        [SerializeField] protected float _skinWidth = 0.05f;
        [SerializeField] [Range(1f, 80f)] protected float _slopeLimit = 60f;
        [SerializeField] [Range(1f, 45f)] protected float _ceilingSlopeLimit = 20f;

        #endregion
        #region Protected Member Variables

        private Vector2 d_CurrentRBVelocity; // For debugging

        #endregion
        #region Properties
        public bool LimitSpeed { get; set; }

        public bool IsGrounded { get; protected set; }
        public bool WasGrounded { get; protected set; }

        public bool IsOnCeiling { get; protected set; }
        public bool WasOnCeiling { get; protected set; }

        public virtual bool IsFacingRight { get; protected set; }
        public virtual bool Suspended { get; set; }

        public float CurrentSlopeAngle { get; protected set; }
        public Vector2 CurrentGroundNormal { get; protected set; }

        public Rigidbody2D Rigidbody
        {
            get { return _rigidbody; }
            protected set { _rigidbody = value; }
        }
        public Animator CharacterAnimator
        {
            get { return _animator; }
            protected set { _animator = value; }
        }
        public float SlopeLimit
        {
            get { return _slopeLimit; }
        }

        public Vector2 GroundCheckPos
        {
            get
            {
                if (capsuleCollider)
                    return capsuleCollider.transform.TransformPoint(capsuleCollider.offset
                                + (new Vector2(0f, -(capsuleCollider.size.y / 2f) + (capsuleCollider.size.x / 2f))));
                return this.transform.position;
            }
        }
        public Vector2 Center
        {
            get
            {
                if (capsuleCollider)
                    return capsuleCollider.transform.TransformPoint(capsuleCollider.offset);
                return this.transform.position;
            }
            
        }
        public Vector2 Bottom
        {
            get
            {
                if (capsuleCollider)
                    return capsuleCollider.transform.TransformPoint(capsuleCollider.offset - (new Vector2(0f,capsuleCollider.size.y/2f)));
                return this.transform.position;
            }
        }
        public float CapsuleRadius
        {
            get
            {
                if (capsuleCollider)
                    return (capsuleCollider.size.x / 2f);
                return 0f;
            }
        }

        #endregion
        #region Unity Callbacks
        protected virtual void Awake()
        {
            LimitSpeed = true;
            UpdateMovementFunc = deltaTime => { return _rigidbody.velocity; };
            StartCoroutine(CoDoLateFixedUpdate());
        }
        protected virtual void Start()
        {
            if (!capsuleCollider) capsuleCollider = GetComponent<CapsuleCollider2D>();
            if (!_rigidbody) _rigidbody = GetComponent<Rigidbody2D>();
            if (!_animator) _animator = GetComponentInChildren<Animator>();
        }

        //ToDo: Optimize pieces into FixedUpdate after core function works nicely.
        // THIS IS ALL FRAME RATE DEPENDENT!!!!
        protected virtual void Update()
        {
            return;
            // Ground Check
            float deltaTime = Time.deltaTime;
            UpdateGroundDetection(deltaTime);

            // Movement Update
            Vector2 resultVelocity = Vector2.zero;
            if (!Suspended)
            {
                resultVelocity = UpdateMovementFunc(deltaTime); 
            }

            // Finalize Movement 
            FinalizeVelocity(resultVelocity);
        }

        protected virtual void FixedUpdate()
        {
            // Ground Check
            float deltaTime = Time.fixedDeltaTime;
            UpdateGroundDetection(deltaTime);

            // Movement Update
            Vector2 resultVelocity = Vector2.zero;
            if (!Suspended)
            {
                resultVelocity = UpdateMovementFunc(deltaTime);
            }

            // Finalize Movement 
            FinalizeVelocity(resultVelocity);
        }

        protected virtual void LateFixedUpdate()
        {
            //// Ground Check
            //float deltaTime = Time.fixedDeltaTime;
            //UpdateGroundDetection(deltaTime);

            //// Movement Update
            //Vector2 resultVelocity = Vector2.zero;
            //if (!Suspended)
            //{
            //    resultVelocity = UpdateMovementFunc(deltaTime);
            //}

            //// Finalize Movement 
            //FinalizeVelocity(resultVelocity);
        }

        #endregion

        #region Movement Logic

        protected virtual void FinalizeVelocity(Vector2 velocity)
        {
            if (Mathf.Abs(velocity.x) > 0.001) // Do not change if 0.
            {
                bool newFace = (velocity.x > 0);
                if (newFace != IsFacingRight)
                {
                    OnChangeFacingDirection(newFace);
                }
                IsFacingRight = newFace;
            }
            _rigidbody.velocity = (!LimitSpeed)? velocity :
                new Vector2(Mathf.Clamp(velocity.x, -_maxHorzSpeed, _maxHorzSpeed), Mathf.Clamp(velocity.y, -_maxVertSpeed, _maxVertSpeed));

            d_CurrentRBVelocity = _rigidbody.velocity;
        }

        #endregion

        #region Grounding Logic

        protected void UpdateGroundDetection(float deltaTime)
        {
            // Ground Check
            WasGrounded = IsGrounded;
            IsGrounded = false;
            Vector2 dir = Vector2.down;
            RaycastHit2D[] hits = new RaycastHit2D[4];
            capsuleCollider.Cast(dir, hits, _skinWidth);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == null) break;
                var go = hits[i].collider.attachedRigidbody
                    ? hits[i].collider.attachedRigidbody.gameObject
                    : hits[i].collider.gameObject;
                if (go != this.gameObject)
                {
                    var point = hits[i].point;
                    var norm = hits[i].normal;

                    var dot = Vector2.Dot(Vector2.up, norm);
                    var angle = Mathf.Rad2Deg * Mathf.Acos(dot);
                    CurrentSlopeAngle = angle;
                    if (angle < _slopeLimit)
                    {
                        Debug.DrawLine(point, point + norm, Color.cyan, 0.5f);
                        IsGrounded = true;
                        CurrentGroundNormal = norm;
                        break;
                    }
                    else
                    {
                        Debug.DrawLine(point, point + norm, Color.magenta, 0.5f);
                    }
                }
            }
            if (WasGrounded != IsGrounded)
            {
                if (IsGrounded)
                    OnBecomeGrounded();
                else
                    OnBecomeUngrounded();
            }
            // Ceiling Check
            WasOnCeiling = IsOnCeiling;
            IsOnCeiling = false;
            dir = Vector2.up;
            hits = new RaycastHit2D[4];
            capsuleCollider.Cast(dir, hits, _skinWidth);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == null) break;
                var go = hits[i].collider.attachedRigidbody
                    ? hits[i].collider.attachedRigidbody.gameObject
                    : hits[i].collider.gameObject;
                if (go != this.gameObject)
                {
                    var dot = Vector2.Dot(Vector2.down, hits[i].normal); // Angle against ceiling downward
                    var angle = Mathf.Rad2Deg * Mathf.Acos(dot);
                    if (angle < _ceilingSlopeLimit)
                    {
                        IsOnCeiling = true;
                        break;
                    }
                }
            }
        }

        public bool CheckForWalkableSlope(float horizontalSpeed)
        {
            var hits = Physics2D.RaycastAll(GroundCheckPos,
                (horizontalSpeed > 0) ? Vector2.right : Vector2.left,
                (capsuleCollider.transform.TransformVector(capsuleCollider.size).x / 2f) + _skinWidth,
                _groundMask);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == null) continue;
                var go = hits[i].collider.attachedRigidbody
                    ? hits[i].collider.attachedRigidbody.gameObject
                    : hits[i].collider.gameObject;
                if (go != this.gameObject)
                {
                    var dot = Vector2.Dot(Vector2.up, hits[i].normal);
                    var angle = Mathf.Rad2Deg * Mathf.Acos(dot);
                    CurrentSlopeAngle = angle;
                    if (angle > _slopeLimit)
                    {
                        Debug.DrawLine(GroundCheckPos, hits[i].point, Color.red, 0.5f);
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckForWall(float horizontalSpeed)
        {
            return CheckForWall(horizontalSpeed, 5f, _groundMask);
        }
        public bool CheckForWall(float horizontalSpeed, float toleranceAngle, LayerMask wallMask)
        {
            var moveDir = (horizontalSpeed > 0) ? Vector2.right : Vector2.left;
            //ToDo: Use Multiple Raycasts from different origins to assure that the surface is a wall.
            var hits = Physics2D.RaycastAll(Center,
                moveDir,
                (capsuleCollider.transform.TransformVector(capsuleCollider.size).x / 2f) + _skinWidth,
                _groundMask);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider == null) continue;
                var go = hits[i].collider.attachedRigidbody
                    ? hits[i].collider.attachedRigidbody.gameObject
                    : hits[i].collider.gameObject;
                if (go != this.gameObject)
                {
                    var dot = Vector2.Dot(-moveDir, hits[i].normal);
                    var angle = Mathf.Rad2Deg * Mathf.Acos(dot);
                    CurrentSlopeAngle = angle;
                    if (angle < toleranceAngle)
                    {
                        Debug.DrawLine(GroundCheckPos, hits[i].point, new Color(1f,0.3f, 0f,1f), 0.5f);
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Directional Move (Dashing, Knockback, etc.)

        protected Coroutine _directionalMoveCoroutine = null;
        protected Func<float, Vector2> _previousMoveFunc;

        public virtual void StartDirectionalMove(Vector2 velocity, float duration)
        {
            StopDirectionalMove();
            _directionalMoveCoroutine = StartCoroutine(CoDirectionalMove(velocity, duration));
        }

        public virtual void StopDirectionalMove()
        {
            if (_directionalMoveCoroutine != null)
            {
                StopCoroutine(_directionalMoveCoroutine);
                _directionalMoveCoroutine = null;
                UpdateMovementFunc = _previousMoveFunc;// Return to default movement Func. (UNTESTED!)
                //Suspended = false;
            }
        }

        protected IEnumerator CoDirectionalMove(Vector2 velocity, float duration)
        {
            //Suspended = true;
            _previousMoveFunc = UpdateMovementFunc;
            var moveVelocity = velocity;
            UpdateMovementFunc = delegate(float deltaTime) { return moveVelocity;  };
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                // NOTE: Changing moveVelocity will effect it in the delegate.
                //moveVelocity = Vector2.Lerp(velocity, Vector2.zero, timer/duration);
                yield return null;
            }
            StopDirectionalMove();
        }

        #endregion

        #region Utility

        protected void Debug_SetSpriteColor(Color color)
        {
            var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr)
            {
                sr.color = color;
            }
        }

        #endregion

        private IEnumerator CoDoLateFixedUpdate()
        {
            while (isActiveAndEnabled)
            {
                yield return new WaitForFixedUpdate();
                LateFixedUpdate();
            }
        }
        
    }
}