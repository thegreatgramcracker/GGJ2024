using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnimalTracker : MonoBehaviour
{
    public Animal myAnimal;
    private int amount;
    public int Amount
    {
        get
        {
            return amount;
        }
        set
        {
            if (amount != value)
            {
                displayAmount = amount;
                StopAllCoroutines();
                StartCoroutine(DisplayValueCounter());
            }
            amount = value;
        }
    }

    public int targetAmount;
    public int displayAmount;
    public float fadeTime;
    Color neutralColor = new Color(0, 0, 0, 186f/255f);
    Color overpopColor = new Color(0, 100f/255f, 0, 186f/255f);
    Color underpopColor = new Color(100f/255f, 0, 0, 186f/255f);
    Color targetColor = Color.black;

    TextMeshProUGUI myText;
    Animator animator;
    Image bg;
    private void Start()
    {
        animator = GetComponent<Animator>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        bg = GetComponent<Image>();
    }

    private void FixedUpdate()
    {

        if (amount < myAnimal.startAmount * 0.5f)
        {
            targetColor = underpopColor;
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("danger_underpop"))
            //{
            //    animator.Play("danger_underpop");
            //}
        }
        else if (amount > myAnimal.startAmount * 1.5f)
        {
            targetColor = overpopColor;
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("danger_overpop"))
            //{
            //    animator.Play("danger_overpop");
            //}
        }
        else
        {
            targetColor = neutralColor;
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            //{
            //    animator.Play("idle");
            //}
        }
        bg.color = Color.Lerp(neutralColor, targetColor, Mathf.PingPong(Time.time, 1f));

        //if (displayAmount > amount)
        //{
        //    displayAmount -= 1;
        //}
        //else if (displayAmount < amount)
        //{
        //    displayAmount += 1;
        //}

        if (amount < displayAmount)
        {
            myText.text = myAnimal.name + ": " + displayAmount + " -" + Mathf.Abs(amount - displayAmount);
        }
        else if (amount > displayAmount)
        {
            myText.text = myAnimal.name + ": " + displayAmount + " +" + Mathf.Abs(amount - displayAmount);
        }
        else
        {
            myText.text = myAnimal.name + ": " + displayAmount;
        }

    }

    IEnumerator DisplayValueCounter()
    {
        yield return new WaitForSeconds(0.5f);
        float startAmount = displayAmount;
        float startTime = Time.time;
        float endTime = Time.time + 1f;
        while (Time.time < endTime)
        {
            displayAmount = Mathf.RoundToInt(Mathf.Lerp(startAmount, amount, (Time.time - startTime) / 1f));
            yield return null;
        }
        displayAmount = amount;
        yield return null;
    }
}
