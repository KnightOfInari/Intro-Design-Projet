
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleFlow : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
	private int battleState = 0;
	public static float userHP = 1;
	private float oppHP = 1;
	private string lastAction = null;
	[SerializeField]
    private GameObject Menu;
	[SerializeField]
    private GameObject PlayerHPBar;
	[SerializeField]
    private GameObject OpponentHPBar;
	[SerializeField]
    private GameObject Narrative;
	[SerializeField]
    private GameObject NarrativeText;
	private float newHP;
	//
	// TODO
	//     	Add correct text
	//		Add sprites
	//		Add transitions
	//		Add animations
	//
	
	void Start()
	{
		Narrative.SetActive(false);
	}
	
	void ReduceUserLife()
	{
		if (userHP > newHP && userHP > 0) {
			userHP -= .03f;
			PlayerHPBar.gameObject.transform.localScale = new Vector3(userHP,1f);
		}
		if (userHP <= 0) {
			Debug.Log("game over");
			CancelInvoke();
			//TODO Add game over call
		}
		if (userHP <= newHP)
			CancelInvoke();
	}
	
	void ReduceOpponentLife()
	{
		Debug.Log("OPP HP : " + oppHP);
		Debug.Log("NEW HP : " + newHP);
		if (oppHP > newHP) {
			oppHP -= .01f;
			OpponentHPBar.gameObject.transform.localScale = new Vector3(oppHP,1f);
		}
		if (oppHP <= newHP)
			CancelInvoke();
	}

	
    void Update()
    {
		if ((battleState == 3 || battleState == 5) && Input.GetKeyDown(KeyCode.Space)) {
			battleState ++;
		}
		if (battleState == 1) {
			Menu.SetActive(false);
			battleState ++;
		}
		if (battleState == 2) {
			Narrative.SetActive(true);
			doAction();
		}
		if (battleState == 4) {
			OpponentAnswer();
		}
		if (battleState == 6) {
			Narrative.SetActive(false);
			Menu.SetActive(true);
			battleState = 0;
		}
    }
	
	private void OpponentAnswer() {
		switch (lastAction) {
			case "Hit":
				NarrativeText.GetComponent<Text>().text = "Le méchant vous tape plus fort";
				newHP = userHP - 0.8f;
				InvokeRepeating("ReduceUserLife", 0.1f, 0.1f);
				break;
			case "Insult":
				NarrativeText.GetComponent<Text>().text = "Le méchant vous insulte plus fort";
				newHP = userHP - 0.3f;
				InvokeRepeating("ReduceUserLife", 0.1f, 0.1f);
				break;
			case "Flee":
				NarrativeText.GetComponent<Text>().text = "Le méchant vous regarde en ricannant";
				//TODO : add changement de scène
				break;
			case "CallPolice":
				NarrativeText.GetComponent<Text>().text = "Le méchant vous fait une clé de bras et raccroche votre téléphone";
				newHP = userHP - 0.5f;
				break;
			default:
				Debug.Log("Error 404");
				break;
		}
		battleState ++;
	}
		
	private void doAction()
	{
		Debug.Log("called");
		switch (lastAction) {
			case "Hit":
				NarrativeText.GetComponent<Text>().text = "Vous tapez le méchant";
				newHP = oppHP - 0.05f;
				InvokeRepeating("ReduceOpponentLife", 0.1f, 0.3f);
				break;
			case "Insult":
				NarrativeText.GetComponent<Text>().text = "Vous insultez le méchant";
				newHP = oppHP - 0.05f;
				InvokeRepeating("ReduceOpponentLife", 0.1f, 0.3f);
				break;
			case "Flee":
				NarrativeText.GetComponent<Text>().text = "Vous changez d'avis et baissez la tête devant le méchant";
				break;
			case "CallPolice":
				NarrativeText.GetComponent<Text>().text = "Vous appelez la police";
				break;
			default:
				Debug.Log("Error 404");
				break;
		}
		battleState ++;
	}
	 
	public void action(string actionString)
	{
		battleState ++;
		lastAction = actionString;
	}
}