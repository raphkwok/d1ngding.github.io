using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{


    [Header("References")]
    public LayerMask ballLayer;
    public GameObject clawObject;
    public GameObject grabObject;
    public Animator clawAnim;

    [Header("Claw Settings")]
    public float clawSpeed;
    public float clawLerpSpeed;
    public float position;
    public float maxClawPosition;
    public float minClawPosition;

    [Header("Claw Gram Animation")]
    public float clawYSpeed;
    public bool grabbing;
    public float clawDropPosition;

    [Header("Input")]
    public bool leftDown;
    public bool rightDown;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 || rightDown)
        {
            MoveRight();
        }
        else if (Input.GetAxisRaw("Horizontal") == -1 || leftDown)
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.E) && clawObject.transform.localPosition.x > minClawPosition)
        {
            StartCoroutine(Grab());
        }
        if (!grabbing) clawObject.transform.localPosition = new Vector2(Mathf.Lerp(clawObject.transform.localPosition.x, position, clawLerpSpeed), clawObject.transform.localPosition.y);
    }

    public void MoveLeft()
    {
        if (!grabbing)
        {
            position -= clawSpeed * Time.deltaTime;
            position = Mathf.Clamp(position, minClawPosition, maxClawPosition);


            clawAnim.Play("Claw Move Left");
        }
    }

    public void MoveRight()
    {
        if (!grabbing)
        {
            position += clawSpeed * Time.deltaTime;
            position = Mathf.Clamp(position, minClawPosition, maxClawPosition);
            clawAnim.Play("Claw Move Right");
        }
    }

    public void LeftDown() { leftDown = true; }
    public void LeftUp() { leftDown = false; }
    public void RightDown() { rightDown = true; }
    public void RightUp() { rightDown = false; }
    public void GrabButton()
    {
        if (clawObject.transform.localPosition.x > minClawPosition)
        {
            StartCoroutine(Grab());
        }
    }

    IEnumerator Grab()
    {
        if (grabbing) yield break;
        grabbing = true;
        float targetHeight;
        float originalPosition = grabObject.transform.position.y;

        RaycastHit2D hit = Physics2D.Raycast(grabObject.transform.position, Vector2.down, 10, ballLayer);
        targetHeight = hit.point.y - 0.2f;

        print(originalPosition + ", " + hit.collider.name);
        print(grabObject.transform.position);
        while (grabObject.transform.position.y > targetHeight + 0.1f)
        {
            grabObject.transform.position = new Vector2(grabObject.transform.position.x, Mathf.Lerp(grabObject.transform.position.y, targetHeight, clawYSpeed));
            yield return null;
        }

        clawAnim.Play("Claw Grab");

        yield return new WaitForSeconds(1);

        print("coming up");
        while (grabObject.transform.position.y < originalPosition - 0.1f)
        {
            grabObject.transform.position = new Vector2(grabObject.transform.position.x, Mathf.Lerp(grabObject.transform.position.y, originalPosition, clawYSpeed));
            yield return null;
        }

        print("Moving to drop");
        while (clawObject.transform.position.x > clawDropPosition + 0.05f)
        {
            clawObject.transform.localPosition = new Vector2(Mathf.Lerp(clawObject.transform.localPosition.x, clawDropPosition, clawYSpeed), clawObject.transform.localPosition.y);
            yield return null;
        }

        clawAnim.Play("Claw Idle");
        yield return new WaitForSeconds(1);
        grabbing = false;
    }
}
