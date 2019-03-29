
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
	//		Add sprites
	//
	
	void Start()
	{
		Narrative.SetActive(false);
		PlayerHPBar.gameObject.transform.localScale = new Vector3(userHP,1f);
	}
	
	void ReduceUserLife()
	{
		if (userHP > newHP && userHP > 0) {
			userHP -= .03f;
			PlayerHPBar.gameObject.transform.localScale = new Vector3(userHP,1f);
		}
		if (userHP <= 0) {
			GameObject.FindObjectOfType<GameManager>().GameOver();
			userHP = 1;
			oppHP = 1;
			CancelInvoke();
			//TODO Add game over call
		}
		if (userHP <= newHP)
			CancelInvoke();
	}
	
	void ReduceOpponentLife()
	{
		if (oppHP > newHP) {
			oppHP -= .01f;
			OpponentHPBar.gameObject.transform.localScale = new Vector3(oppHP,1f);
		}
		if (oppHP <= newHP)
			CancelInvoke();
	}

	
    void Update()
    {
		if ((battleState == 3 || battleState == 5) && Input.GetMouseButtonDown(0)) {
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
				NarrativeText.GetComponent<Text>().text = "The bad guy hits you harder !";
				newHP = userHP - 0.8f;
				InvokeRepeating("ReduceUserLife", 0.1f, 0.1f);
				break;
			case "Insult":
				NarrativeText.GetComponent<Text>().text = "The bad guy teaches you new things about your mother";
				newHP = userHP - 0.3f;
				InvokeRepeating("ReduceUserLife", 0.1f, 0.1f);
				break;
			case "CallPolice":
				NarrativeText.GetComponent<Text>().text = "The bad guy hits you and hang up your phone";
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
				NarrativeText.GetComponent<Text>().text = "You hit the bad guy";
				newHP = oppHP - 0.05f;
				InvokeRepeating("ReduceOpponentLife", 0.1f, 0.3f);
				break;
			case "Insult":
				NarrativeText.GetComponent<Text>().text = "You insult the bad guy";
				newHP = oppHP - 0.05f;
				InvokeRepeating("ReduceOpponentLife", 0.1f, 0.3f);
				break;
			case "Flee":
				NarrativeText.GetComponent<Text>().text = "You change your mind and lower your head";
                oppHP = 1;
				GameObject.FindObjectOfType<GameManager>().ReturnFromFight();
                break;
			case "CallPolice":
				NarrativeText.GetComponent<Text>().text = "You try to call the police";
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