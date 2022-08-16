using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BannerVisual : MonoBehaviour
{
    // on method,
        // slow time
        // zoom in
        // banner down


	public Text txt, ScoreText;
	public Vector3 player1ConfettiPosition, player2ConfettiPosition, player1AngryPosition, player2AngryPosition, AngryPulseMaxScale;

	public Transform Confetti, AngryMark;

	public Transform MoverParent;
	public Vector3 MPStartingLocation, MPDestinationLocale;
	
	


	public Vector3 StartingLocale;
	public Vector3 DestinationLocale;
	public float TimeToMoveDown, TimeToStayInPlace, TimeToMoveUp;
	
	[ContextMenu("Proc Banner Movement")] 
    public void Proc(GameManager gm){
        txt.text = "Tagged!";
		//move the banner down to position

		SetDisplayInfoCorrectly(gm);

		StartCoroutine(BeginBannerMovement());
    }

    
	IEnumerator BeginBannerMovement(){
		
		//slowly move text right 20.6

		// pulse angrymark
		Tween myTween3 = AngryMark.DOScale(AngryPulseMaxScale, 0.2f).SetLoops(-1, LoopType.Yoyo);

		// Move whole banner down, then stay, then back up
		
		Tween myTween = transform.DOLocalMove(DestinationLocale, TimeToMoveDown);
		yield return myTween.WaitForCompletion();

		myTween = transform.DOLocalMove(DestinationLocale, TimeToStayInPlace);
		
		// move mover to teh right
		Tween myTween2 = MoverParent.DOLocalMove(MPDestinationLocale, TimeToStayInPlace);

		yield return myTween.WaitForCompletion();

		myTween = transform.DOLocalMove(StartingLocale, TimeToMoveUp);
		yield return myTween.WaitForCompletion();
		
		//
		MoverParent.transform.localPosition = MPStartingLocation;

		



	}

	// called to change the score display tot he corrrect info and align confetti / angry gameobjects with correct player model
	public void SetDisplayInfoCorrectly(GameManager gm){
		// display player score next to relevant player

		ScoreText.text = $"{gm.player1.playerWins} - {gm.player2.playerWins}";

		// make visuals change depending on which player is winning

		if(gm.player1.playerWins >= gm.player2.playerWins ){
			Confetti.localPosition = player1ConfettiPosition;
			AngryMark.localPosition = player2AngryPosition;

		} else {
			Confetti.localPosition = player2ConfettiPosition;
			AngryMark.localPosition = player1AngryPosition;
		}



	}


}
