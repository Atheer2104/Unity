using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;

	public Animator animator;

	private Queue<string> sentences;

	void Start ()
	{
		sentences = new Queue<string>();
	}

	public void startDialogue(Dialogue dialogue)
	{  
		animator.SetBool ("IsOpen", true);
		
		nameText.text = dialogue.name;

		sentences.Clear ();

		foreach (string s in dialogue.sentences) 
		{
			sentences.Enqueue (s);
		}

		displayNextSentences ();
	}

	public void displayNextSentences()
	{
		if (sentences.Count == 0) 
		{
			endDialogue ();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines ();
		StartCoroutine (typeSentence (sentence));
	}

	IEnumerator typeSentence (string sentence)
	{
		dialogueText.text = "";

		foreach (char l in sentence.ToCharArray()) 
		{
			dialogueText.text += l;
			yield return null;
		}
	}

	void endDialogue()
	{
		animator.SetBool ("IsOpen", false);
	}
}
