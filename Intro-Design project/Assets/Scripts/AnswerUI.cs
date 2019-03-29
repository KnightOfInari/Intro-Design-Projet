using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerUI : MonoBehaviour
{
    public void Answer(bool fightBack)
    {
        if (fightBack == true)
        {

            //TODO systeme de combat
            GameObject.FindObjectOfType<GameManager>().PlayerFight(gameObject.GetComponentInParent<Transform>().position);
            //GameObject.FindObjectOfType<GameManager>().UnblockPlayer(); //temporaire pour test
        }
        else
        {
            GameObject.FindObjectOfType<GameManager>().UnblockPlayer();
        }

    }
}
