using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTrigger : MonoBehaviour
{
	TicTacToeAI _ai;

	EndMessage end = new EndMessage();

	int sayac = 0;
	int artis = 0;
	int tempCoordX = 0;
	int tempCoordY = 0;
	bool yatay = true;
	bool dikey = true;

	[SerializeField]
	private int _myCoordX = 0;
	[SerializeField]
	private int _myCoordY = 0;

	[SerializeField]
	private bool canClick;

	private void Awake()
	{
		_ai = FindObjectOfType<TicTacToeAI>();
	}

	// - 3
	private void Start(){
		
		_ai.onGameStarted.AddListener(AddReference);
		_ai.onGameStarted.AddListener(() => SetInputEndabled(true));
		_ai.onPlayerWin.AddListener((win) => SetInputEndabled(false));
		_ai.onPlayerWin.AddListener(end.OnGameEnded);
	}

	//tiklandiysa true yapılıyor
	private void SetInputEndabled(bool val){
		canClick = val;
	}

	// - 4 
	private void AddReference()
	{
		_ai.RegisterTransform(_myCoordX, _myCoordY, this);
		canClick = true;
	}

	private void OnMouseDown()
	{
		if(canClick){
			_ai.PlayerSelects(_myCoordX, _myCoordY);
			canClick = false;//sonradan ekledim					
            if (_ai.turning == 9 && !_ai.won)
            {
				_ai.Winning(TicTacToeState.none);
            }
            if (!_ai.won)
            {
				Invoke("AiPlaying", 0.5f);//Ai oynayacak
			}			
		}
	}

	void _AiRandom()
    {
        if (_ai._triggers[1, 1].canClick == true)
        {
			_ai.AiSelects(1, 1);
			_ai._triggers[1, 1].canClick = false;
        }
        else
        {
			for (int i = 0; i < 3; i++)
			{
				for (int z = 0; z < 3; z++)
				{
					if (_ai._triggers[i, z].canClick == true && !_ai._isPlayerTurn)
					{
						_ai.AiSelects(i, z);
						_ai._triggers[i, z].canClick = false;
						break;
					}
				}
				if (_ai._isPlayerTurn)
				{
					break;
				}
			}
		}		
	}
	void AiPlaying()
	{
		if (_ai.turning <= 2)
		{
			_AiRandom();
		}
		//yatay kontrol
		if (_ai.turning > 2)
		{
			sayac = 0;
			for (int i = 0; i < 3; i++)
			{
				for (int z = 0; z < 3; z++)
				{
					if (_ai.boardState[i, z] == TicTacToeState.circle && !_ai._isPlayerTurn)
					{
						sayac++;
					}
					if (_ai._triggers[i, z].canClick == true && !_ai._isPlayerTurn)
					{
						tempCoordX = i;
						tempCoordY = z;
					}
					if (_ai._triggers[i, z].canClick == false && !_ai._isPlayerTurn)
					{
						artis++;
					}
					if (sayac == 2)
					{
						if (artis == 3)
						{
							break;
						}
						else
						{
							if (z == 1)
							{
								//dikey kontrole yonlendirme yapmalıyım
								if (_ai._triggers[i, z + 1].canClick == false)
								{
									//_AiRandom();
									break;//bu satirda iki tane O var ama, bosluk yok.
								}
								_ai.AiSelects(i, z + 1);
								_ai._triggers[i, z + 1].canClick = false;
								yatay = false;
								break;
							}
							else
							{
								_ai.AiSelects(tempCoordX, tempCoordY);
								_ai._triggers[tempCoordX, tempCoordY].canClick = false;
								yatay = false;
								break;
							}
						}
					}
				}
				sayac = 0;
				artis = 0;
				if (_ai._isPlayerTurn)
				{
					break;
				}
			}			
		}
		//dikey kontrol
		if (_ai.turning > 2 && !_ai._isPlayerTurn)
		{
			sayac = 0;
			if (yatay)
			{
				for (int z = 0; z < 3; z++)
				{
					for (int i = 0; i < 3; i++)
					{
						if (_ai.boardState[i, z] == TicTacToeState.circle && !_ai._isPlayerTurn)
						{
							sayac++;
						}
						if (_ai._triggers[i, z].canClick == true && !_ai._isPlayerTurn)
						{
							tempCoordX = i;
							tempCoordY = z;
						}
						if (_ai._triggers[i, z].canClick == false && !_ai._isPlayerTurn)
						{
							artis++;
						}
						if (sayac == 2)
						{
							if (artis == 3)
							{
								break;
							}
							else
							{
								if (i == 1 && sayac == 2)
								{
									//capraz kontrole yonlendirme yapmalıyım
									if (_ai._triggers[i + 1, z].canClick == false)
									{
										//_AiRandom();
										break;//bu sütunda iki tane O var ama, bosluk yok.
									}
									_ai.AiSelects(i + 1, z);
									_ai._triggers[i + 1, z].canClick = false;
									yatay = false;
									break;
								}
								else
								{
									_ai.AiSelects(tempCoordX, tempCoordY);
									_ai._triggers[tempCoordX, tempCoordY].canClick = false;
									yatay = false;
									break;
								}
							}
						}
					}
					sayac = 0;
					artis = 0;
					if (_ai._isPlayerTurn)
					{
						break;
					}
				}
			}
		}
		//capraz kontrol
		if (_ai.turning > 2 && !_ai._isPlayerTurn)
		{
			sayac = 0;
			if (dikey)
			{
				if (_ai.boardState[0, 0] == TicTacToeState.circle && _ai.boardState[1, 1] == TicTacToeState.circle && !_ai._isPlayerTurn)
				{
					sayac++;
				}
				if (_ai.boardState[0, 0] == TicTacToeState.circle && _ai.boardState[2, 2] == TicTacToeState.circle && !_ai._isPlayerTurn)
				{
					sayac++;
				}
				if (_ai.boardState[1, 1] == TicTacToeState.circle && _ai.boardState[2, 2] == TicTacToeState.circle && !_ai._isPlayerTurn)
				{
					sayac++;
				}
				if (sayac == 1)
				{
					for (int i = 0; i < 3; i++)
					{
						if (_ai._triggers[i, i].canClick == false && !_ai._isPlayerTurn)
						{
							artis++;
						}
						if (_ai._triggers[i, i].canClick == true && !_ai._isPlayerTurn)
						{
							tempCoordX = i;
							tempCoordY = i;
							break;
						}					
					}
					if (artis == 3)
					{
						//bir sey yapma
					}
					else
					{
						_ai.AiSelects(tempCoordX, tempCoordY);
						_ai._triggers[tempCoordX, tempCoordY].canClick = false;
						yatay = false;
					}
					artis = 0;
					sayac = 0;
				}
			}
            //diger taraf           
            if (_ai.boardState[0, 2] == TicTacToeState.circle && _ai.boardState[1, 1] == TicTacToeState.circle && !_ai._isPlayerTurn)
            {
                sayac++;
            }
            if (_ai.boardState[0, 2] == TicTacToeState.circle && _ai.boardState[2, 0] == TicTacToeState.circle && !_ai._isPlayerTurn)
            {
                sayac++;
            }
            if (_ai.boardState[2, 0] == TicTacToeState.circle && _ai.boardState[1, 1] == TicTacToeState.circle && !_ai._isPlayerTurn)
            {
                sayac++;
            }
            if (sayac == 1)
            {
                for (int i = 0; i < 3; i++)
                {
					if (_ai._triggers[i, 2-i].canClick == false && !_ai._isPlayerTurn)
					{
						artis++;
					}
					if (_ai._triggers[i, 2-i].canClick == true && !_ai._isPlayerTurn)
                    {
                        tempCoordX = i;
                        tempCoordY = 2 - i;
                        break;
                    }
                }
				if (artis == 3)
				{
					//bir sey yapma
				}
				else
				{
					_ai.AiSelects(tempCoordX, tempCoordY);
					_ai._triggers[tempCoordX, tempCoordY].canClick = false;
					yatay = false;
				}
				artis = 0;
				sayac = 0;				
            }
        }
        if (!_ai._isPlayerTurn)
        {
			_AiRandom();
		}		
    }
}
