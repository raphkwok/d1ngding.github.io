using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject imageObject;
    public Animator pictureAnimator;

    public bool show;

    private void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball Collider")
        {
            print("bruh");
            imageObject.SetActive(true);
            pictureAnimator.SetBool("Show", true);
            Time.timeScale = 0;
        }
    }


    public void ExitPicture()
    {
        StartCoroutine(Timer());
        pictureAnimator.SetBool("Show", false);
        Time.timeScale = 1;
    }

    IEnumerator Timer()
    {

        yield return new WaitForSeconds(1);
        imageObject.SetActive(false);
    }
}
