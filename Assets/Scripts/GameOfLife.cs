using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct GridPosition{
	public int x;
	public int y;

	public GridPosition(int i, int j) {
		x = i;
		y = j;
	}
}

public class GameOfLife {
	const char ON= '1';
	public string raw_layout = 
	@"00000000
11100000
00000000
00000000
00000000
00000000
00000000
00000000";

	public List<List<bool>> map;

	public void Initialise() {
		string[] layout = raw_layout.Split('\n');
		map = new List<List<bool>>();

		for (int y =0; y < layout.Length; y++) {
			map.Add(new List<bool>());
			for (int x=0; x < layout[y].Length; x++) {
				map[y].Add(layout[y][x] == ON);
			}
		}
	}

	public void ToggleCell(GridPosition pos) {
		bool? c = getItem(pos);
		if (c.HasValue) {
			setItem(pos, !c.Value);
		} else {
			Debug.LogError("Trying to toggle cell that is not in the grid");
		}
	}

	bool inGrid(GridPosition pos) {
		int width = map[0].Count;
		int height = map.Count;

		return pos.x < width && pos.x >= 0 && pos.y < height && pos.y >= 0;
	}

	public bool? getItem(GridPosition pos) {
		if (inGrid(pos)) {
			return map[pos.y][pos.x];
		} else {
			return null;
		}
	}

	void setItem(GridPosition pos, bool value) {
		if (inGrid(pos)) {
			map[pos.y][pos.x] = value;
		} else {
			Debug.LogError("Trying to set cell that is not in the grid");
		}
	}

	bool[] getNeighbours(GridPosition pos) {
		if (inGrid(pos)) {
			List<bool> neighbours = new List<bool>();
			for (int i = -1; i <= 1; i++){
				for (int j = -1; j <= 1; j++){
					if (j == 0 && i == 0) {continue;}
					bool? item = getItem(new GridPosition(pos.x+i, pos.y+j));
					if (item.HasValue) {
						neighbours.Add(item.Value);
					}
				}
			}
			return neighbours.ToArray();
			
		} else {
			Debug.LogError("No neigbours outside of the grid");
			return new bool[] {};
		}
	}

	///Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
	// Any live cell with two or three live neighbours lives on to the next generation.
	// Any live cell with more than three live neighbours dies, as if by overpopulation.
	// Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
	public void NextTurn() {
		//orig2d.Select(a => a.ToArray()).ToArray();
		List<List<bool>> newMap = map.Select(a => a.ToList()).ToList();

		for (int y = 0; y < map.Count; y++) {
			for (int x=0; x < map[y].Count; x++) {
				bool thisCell = getItem(new GridPosition(x, y)).Value;
				bool[] neighbours = getNeighbours(new GridPosition(x, y));
				byte alive =0;
				foreach (bool cell in neighbours) {
					if (cell) {alive++;}
				}
				if (thisCell && (alive < 2 || alive > 3)) {
					newMap[y][x] = false;
				} else if (alive == 3) {
					newMap[y][x] = true;
				}
			}
		}

		map = newMap;
	}
}
