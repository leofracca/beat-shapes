using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float jumpValue;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // The character doesn't move
        rb.bodyType = RigidbodyType2D.Static;
    }

    void Update()
    {
        switch (Level.state)
        {
            case State.WaitingToStart:
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
                {
                    // The character starts moving
                    Level.state = State.Playing;
                    rb.bodyType = RigidbodyType2D.Dynamic;

                    Jump();
                }
                break;
            case State.Playing:
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
                {
                    Jump();
                }
                transform.eulerAngles = new Vector3(0, 0, rb.velocity.y * .5f);
                break;
            default:
                break;
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * rb.gravityScale * jumpValue * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name != "Projectile(Clone)")
        {
            Level.state = State.Dead;

            MakeSceneDarker();

            ScoreWindow.HideScoreOverWindow();
            GameOverWindow.ShowGameOverWindow();
        }
    }

    private void MakeSceneDarker()
    {
        Light2D l = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        l.intensity = Mathf.Lerp(l.intensity, 0f, Time.deltaTime * 0.5f);

        Volume volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        Vignette vignette;
        volume.profile.TryGet(out vignette);
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 1f, Time.deltaTime * 0.5f);
    }
}
