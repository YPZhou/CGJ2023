using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CGJ2023
{
    /// <summary>
    /// 玩家角色球
    /// </summary>
    public class PlayerBall : BaseBall
    {
        Transform DirectionRender;
        Vector3 ArrowScale;
        Quaternion ArrowRot;

        protected override void StartCore()
        {
            canPush = true;
            DirectionRender = transform.GetChild(2);
            ArrowScale = DirectionRender.localScale;
            ArrowRot = DirectionRender.rotation;
        }

        protected override void UpdateCore()
        {
            //var moveDirection = Vector3.zero;
            //if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    moveDirection += Vector3.left;
            //}

            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    moveDirection += Vector3.right;
            //}

            //if (Input.GetKey(KeyCode.UpArrow))
            //{
            //    moveDirection += Vector3.up;
            //}

            //if (Input.GetKey(KeyCode.DownArrow))
            //{

            //    moveDirection += Vector3.down;
            //}

            //transform.position = transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime;

            //if (transform.position.x < -8.6f)
            //{
            //	var rigidBody = GetComponent<Rigidbody2D>();
            //	if (rigidBody != null)
            //	{
            //		rigidBody.AddForce(Vector2.right * Mathf.Abs(rigidBody.velocity.x), ForceMode2D.Impulse);
            //	}
            //}

            //if (transform.position.x > 8.6f)
            //{
            //	var rigidBody = GetComponent<Rigidbody2D>();
            //	if (rigidBody != null)
            //	{
            //		rigidBody.AddForce(Vector2.left * Mathf.Abs(rigidBody.velocity.x), ForceMode2D.Impulse);
            //	}
            //}

            //if (transform.position.y > 3.5f)
            //{
            //	var rigidBody = GetComponent<Rigidbody2D>();
            //	if (rigidBody != null)
            //	{
            //		rigidBody.AddForce(Vector2.down * Mathf.Abs(rigidBody.velocity.y), ForceMode2D.Impulse);
            //	}
            //}

            //if (transform.position.y < -4.7f)
            //{
            //	var rigidBody = GetComponent<Rigidbody2D>();
            //	if (rigidBody != null)
            //	{
            //		rigidBody.AddForce(Vector2.up * Mathf.Abs(rigidBody.velocity.y), ForceMode2D.Impulse);
            //	}
            //}

            var rigidBody = GetComponent<Rigidbody2D>();
            if (rigidBody != null)
            {
                if (canPush)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        var pushDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                        rigidBody.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
                        canPush = false;

                        DirectionRender.localScale = ArrowScale;
                        DirectionRender.rotation = ArrowRot;
                    }
                }

                DragAllAttachedBalls(rigidBody.velocity);

                if (!canPush)
                {
                    canPush = rigidBody.velocity.sqrMagnitude < 0.01f;
                    if (canPush)
                    {
                        room.FinishCurrentPush();

                    }
                }
            }

            if (playerIcon != null)
            {
                if (canPush)
                {
                    playerIcon.transform.localPosition = new Vector3(0f, Mathf.Cos(Time.time * 5f) * 0.25f + 1.5f);
                    playerIcon.color = Color.green;
                }
                else
                {
                    playerIcon.transform.localPosition = new Vector3(0f, 1.5f);
                    playerIcon.color = Color.red;
                }
            }

            DirectionRender.GetComponentInChildren<SpriteRenderer>().enabled = canPush;

            if (canPush)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var distance = (mousePos - DirectionRender.position).magnitude - 10;
                distance = Mathf.Clamp(Mathf.Sqrt(distance), 0.4f, Mathf.Sqrt(distance));

                DirectionRender.localScale = new Vector3(DirectionRender.localScale.x, distance, DirectionRender.localScale.z);
                DirectionRender.rotation = Quaternion.LookRotation(mousePos - DirectionRender.position, Vector3.forward);

                Debug.Log($"MousePos:{mousePos} Distance: {distance}");
            }
        }

        bool canPush;

        void DragAllAttachedBalls(Vector3 velocity)
        {
            var attachedBalls = room.collectableBalls.Select(obj => obj.GetComponent<CollectableBall>()).Where(ball => ball.IsAttached);
            foreach (var attachedBall in attachedBalls)
            {
                var rigidBody = attachedBall.GetComponent<Rigidbody2D>();
                rigidBody.velocity = velocity;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var colliderGameObject = collision.gameObject;
            var ball = colliderGameObject.GetComponent<CollectableBall>();
            var item = colliderGameObject.GetComponent<BaseItem>();

            if (ball != null)
            {
                var color = ball.BallColor;
                if (room.ThemeColor == color)
                {
                    room.OnCollectBall();
                    ball.DestroyBall(isTouch: true);
                }
                else
                {
                    room.ComboCount = 0;
                    ball.AttachTo(this);
                }
                attachedBalls.Add(ball);
            }
            else if (item != null)
            {
                item.ApplyEffect(this);
            }
        }

        #region ItemEffects
        public void ClearAttachedBalls()
        {
            foreach (var ball in attachedBalls)
            {
                ball.DestroyBall();
            }
            attachedBalls.Clear();
        }

        #endregion

        [SerializeField]
        float pushForce;

        [SerializeField]
        SpriteRenderer playerIcon;

        HashSet<CollectableBall> attachedBalls = new();
    }
}
