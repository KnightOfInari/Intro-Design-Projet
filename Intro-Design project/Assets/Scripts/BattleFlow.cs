
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleFlow : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
	private int battleState = 0;
	private string lastAction = null;
	[SerializeField]
    private GameObject Menu;
	[SerializeField]
    private GameObject Narrative;
	[SerializeField]
    private GameObject NarrativeText;
	
	//
	// TODO
	//     	Add correct text
	//		Add sprites
	//		Add PV for characters
	//		Add transitions
	//		Add animations
	//
	
	void Start()
	{
		Narrative.SetActive(false);
	}
	
    void Update()
    {
		if ((battleState == 2 || battleState == 3) && Input.GetKeyDown(KeyCode.Space)) {
			battleState ++;
		}
		if (battleState == 1) {
			Menu.SetActive(false);
			battleState ++;
		}
		if (battleState == 2) {
			Narrative.SetActive(true);
            StartCoroutine("doAction");
		}
		if (battleState == 3) {
			Debug.Log("Hello");
            StartCoroutine("OpponentAnswer");
		}
		if (battleState == 4) {
			Narrative.SetActive(false);
			Menu.SetActive(true);
			battleState = 0;
		}
    }
	
	private void OpponentAnswer() {
		switch (lastAction) {
			case "Hit":
				NarrativeText.GetComponent<Text>().text = "Le méchant vous tape plus fort";
				break;
			case "Insult":
				NarrativeText.GetComponent<Text>().text = "Le méchant vous insulte plus fort";
				break;
			case "Flee":
				NarrativeText.GetComponent<Text>().text = "Le méchant vous rattrappe et vous fracasse contre le sol";
				break;
			case "CallPolice":
				NarrativeText.GetComponent<Text>().text = "Le méchant vous vole votre téléphone";
				break;
			default:
				Debug.Log("Error 404");
				break;
		}
	}
		
	private void doAction()
	{
		switch (lastAction) {
			case "Hit":
				NarrativeText.GetComponent<Text>().text = "Vous tapez le méchant";
				break;
			case "Insult":
				NarrativeText.GetComponent<Text>().text = "Vous insultez le méchant";
				break;
			case "Flee":
				NarrativeText.GetComponent<Text>().text = "Vous fuyez devant le méchant";
				break;
			case "CallPolice":
				NarrativeText.GetComponent<Text>().text = "Vous appelez la police";
				break;
			default:
				Debug.Log("Error 404");
				break;
		}
	}
	 
	public void action(string actionString)
	{
		battleState ++;
		lastAction = actionString;
	}
}