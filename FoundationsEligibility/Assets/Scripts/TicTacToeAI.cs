using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TicTacToeState{none = -1, cross = 1, circle = 2} // sayi ekledim

[System.Serializable]
public class WinnerEvent : UnityEvent<int>
{
	
}

public class TicTacToeAI : MonoBehaviour
{
	public int turning = 0;
	int _aiLevel;
	public bool won = false;

	public TicTacToeState[,] boardState;

	[SerializeField]
	public bool _isPlayerTurn;

    [SerializeField]
    private int _gridSize = 3;

    [SerializeField]
	private TicTacToeState playerState = TicTacToeState.circle;
	TicTacToeState aiState = TicTacToeState.cross;

	[SerializeField]
	private GameObject _xPrefab;

	[SerializeField]
	private GameObject _oPrefab;

	public UnityEvent onGameStarted;

	//Call This event with the player number to denote the winner
	public WinnerEvent onPlayerWin;

	public ClickTrigger[,] _triggers;
	
	private void Awake()
	{
		if(onPlayerWin == null){
			onPlayerWin = new WinnerEvent();
		}
		boardState = new TicTacToeState[3,3];
	}

	public void StartAI(int AILevel){
		_aiLevel = AILevel;
		StartGame();
	}

	public void RegisterTransform(int myCoordX, int myCoordY, ClickTrigger clickTrigger)
	{
		_triggers[myCoordX, myCoordY] = clickTrigger;
	}

	private void StartGame()
	{
		_triggers = new ClickTrigger[3,3];
		turning = 0;
		onGameStarted.Invoke();
	}

	public void PlayerSelects(int coordX, int coordY){

		SetVisual(coordX, coordY, playerState);
		turning++;
		_isPlayerTurn = false;
	}

	public void AiSelects(int coordX, int coordY){

		SetVisual(coordX, coordY, aiState);
		turning++;
		_isPlayerTurn = true;
	}

	private void SetVisual(int coordX, int coordY, TicTacToeState targetState)
	{	
		Instantiate(
			targetState == TicTacToeState.circle ? _oPrefab : _xPrefab,
			_triggers[coordX, coordY].transform.position,
			Quaternion.identity
		);
		//sonradan ekledim
		boardState[coordX, coordY] = targetState;
		CheckWinner(targetState);
	}

	void CheckWinner(TicTacToeState targetState)
    {
		switch (targetState)
		{
			case TicTacToeState.circle:
				CheckInstructions(targetState);
				break;
			case TicTacToeState.cross:
				CheckInstructions(targetState);
				break;
		}
	}

    void CheckInstructions(TicTacToeState target)
    {
		//capraz kazanma
        if (boardState[0,0] == target && boardState[1,1] == target && boardState[2,2] == target)
        {			
			won = true;
			Winning(target);
		}
		else if (boardState[2, 0] == target && boardState[1, 1] == target && boardState[0, 2] == target)
        {
			won = true;
			Winning(target);
		}
		//dikey kazanma
		else if (boardState[0, 0] == target && boardState[1, 0] == target && boardState[2, 0] == target)
		{
			won = true;
			Winning(target);
		}
		else if (boardState[0, 1] == target && boardState[1, 1] == target && boardState[2, 1] == target)
		{
			won = true;
			Winning(target);
		}
		else if (boardState[0, 2] == target && boardState[1, 2] == target && boardState[2, 2] == target)
		{
			won = true;
			Winning(target);
		}
		//yatay kazanma
		else if (boardState[0, 0] == target && boardState[0, 1] == target && boardState[0, 2] == target)
		{
			won = true;
			Winning(target);
		}
		else if (boardState[1, 0] == target && boardState[1, 1] == target && boardState[1, 2] == target)
		{
			won = true;
			Winning(target);
		}
		else if (boardState[2, 0] == target && boardState[2, 1] == target && boardState[2, 2] == target)
		{
			won = true;
			Winning(target);
		}
	}

	public void Winning(TicTacToeState target)
    {
		if(turning == 9 && !won)
        {
			onPlayerWin.Invoke((int)TicTacToeState.none);
		}			
		onPlayerWin.Invoke((int)target);
	}
}
