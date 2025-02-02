using UnityEngine;

namespace RayWenderlich.SpaceInvadersUnity
{
    public class CannonControl : MonoBehaviour
    {
        public float speed = 10.5f;

        [SerializeField] 
        private Transform muzzle;

        [SerializeField] 
        private AudioClip shooting;

        [SerializeField] 
        private float coolDownTime = 0.5f;

        [SerializeField] 
        private Bullet bulletPrefab;

        [SerializeField] 
        private float respawnTime = 2f;

        [SerializeField] 
        private SpriteRenderer sprite;
        
        [SerializeField] 
        private Collider2D cannonCollider;

        private Vector2 startPos;
        private void Start() => startPos = transform.position;

        private float shootTimer;

        void Update()
        {
            Vector3 pos = transform.position;
            if (Input.GetKey("w"))
            {
                pos.y += speed * Time.deltaTime;
            }
            if (Input.GetKey("s")) 
            {
                pos.y -= speed * Time.deltaTime;
            }
            if (Input.GetKey("d")) 
            {
                pos.x += speed * Time.deltaTime;
            }
            if (Input.GetKey("a"))
            {
                pos.x -= speed * Time.deltaTime;
            }
            transform.position = pos;


            shootTimer += Time.deltaTime;
            if (shootTimer > coolDownTime && Input.GetKey(KeyCode.Space))
            {
                shootTimer = 0f;
                Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
                GameManager.Instance.PlaySfx(shooting);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            GameManager.Instance.UpdateLives();
            StopAllCoroutines();
            StartCoroutine(Respawn());
        }

        System.Collections.IEnumerator Respawn()
        {
            enabled = false;
            cannonCollider.enabled = false;
            ChangeSpriteAlpha(0.0f);
            yield return new WaitForSeconds(0.25f * respawnTime);
            transform.position = startPos;
            enabled = true;
            ChangeSpriteAlpha(0.25f);

            yield return new WaitForSeconds(0.75f * respawnTime);

            ChangeSpriteAlpha(1.0f);
            cannonCollider.enabled = true;
        }

        private void ChangeSpriteAlpha(float value)
        {
            var color = sprite.color;
            color.a = value;
            sprite.color = color;
        }

    }
}