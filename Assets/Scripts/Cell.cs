using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour {

	public GridPosition gridPosition;
	public GameOfLife game;
	public SpriteRenderer _sr;

	void Start()
	{
			_sr = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
			bool? alive = game.getItem(gridPosition);
			if (alive.HasValue) {
				_sr.color = alive.Value ? GameManager.Instance.AliveColour : GameManager.Instance.DeadColour;
			}
	}

	void OnMouseDown()
	{
		Debug.Log("Click");
		game.ToggleCell(gridPosition);
	}
}
