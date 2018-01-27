using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) { _instance = FindObjectOfType<GameManager>(); }
            return _instance;
        }
    }

	public GameObject BaseTile;
	public Color AliveColour = Color.green;
	public Color DeadColour = Color.red;
	GameOfLife game = new GameOfLife();
	List<List<Cell>> tiles;

	public int Width = 100;
	public int Height = 100;

	// Use this for initialization
	void Start () {
		string genLayout = "";

		for (int y = 0; y < Height; y++) {
			for (int x = 0; x < Width; x++) {
				genLayout += '0';
			}
			if (y!=Height-1) genLayout += '\n';
		}

		game.raw_layout = genLayout;
		game.Initialise();
		tiles = new List<List<Cell>>();
		for (int y =0; y < game.map.Count; y++) {
			tiles.Add(new List<Cell>());
			for (int x=0; x < game.map[y].Count; x++) {
				bool cell = game.map[y][x];
				GameObject objCell = Instantiate(BaseTile, new Vector2(x, y), Quaternion.identity);
				objCell.name = string.Format("Cell ({0},{1})", x, y);
				tiles[y].Add(objCell.GetComponent<Cell>());
				tiles[y][x].gridPosition = new GridPosition(x, y);
				tiles[y][x].game = game;
			}
		}
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			game.NextTurn();
		}
	}
}
