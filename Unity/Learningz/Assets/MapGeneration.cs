using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGeneration : MonoBehaviour {

	public int Width;
	public int Height;

	public string seed;
	public bool UseRandSeed;

	[Range(0,100)]
	public int RandFillPercent;

	int[,] map;

	void Start(){
		GenerateMap ();
	}

	void GenerateMap(){
		map = new int[Width,Height];
		RandFillMap ();
	}

	void RandFillMap(){
		if (UseRandSeed) {
			seed = Time.time.ToString();

		}

		System.Random PseudoRandNumGen = new System.Random (seed.GetHashCode());

		for (int x = 0; x < Width; x ++) {
			for (int y = 0; y < Height; y ++) {
				map [x, y] = (PseudoRandNumGen.Next (0, 100) < RandFillPercent)? 1 : 0;
			}

		}
	}

	
	void OnDrawGizmos(){
		if (map != null) {
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++){
					Gizmos.color = (map[x,y] == 1)?Color.black : Color.white;
					Vector3 pos = new Vector3(-Width/2 + x + .5f,0, -Height/2 + y + .5f);
					Gizmos.DrawCube (pos, Vector3.one);
					}

				}

			}
		}





		
}