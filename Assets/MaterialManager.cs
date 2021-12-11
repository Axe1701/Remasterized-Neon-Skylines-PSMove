/*
 * Written by Maxi Levi <maxilevi@live.com>, November 2017
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MaterialManager : NetworkBehaviour {

    public Material Terrain;
    [SyncVar]
    public Color[] PossibleColors;
    [SyncVar]
    public Color CurrentColor;
    [SyncVar]
	public float MaxThickness = 0.3f, MinThickness = 0.15f;
    [SyncVar]
    public float Time;
    [SyncVar]
    public float ChangeTime = 32; 

	void Update(){

		if (Time >= ChangeTime) {
			CurrentColor = PossibleColors[Random.Range(0,PossibleColors.Length)];
			this.Time = 0;
		}

		this.Time += UnityEngine.Time.deltaTime;

		Terrain.SetColor("_Color", Lerp(Terrain.GetColor("_Color"), CurrentColor, UnityEngine.Time.deltaTime * 2f));
		Terrain.SetColor("_GColor", Terrain.GetColor("_Color"));
		Terrain.SetColor("_WColor", Terrain.GetColor("_Color"));
		Terrain.SetColor("_EmissionColor", Terrain.GetColor("_Color"));

		//TODO Thickness
	}

	Color Lerp(Color c1, Color c2, float delta){
		return new Color (Mathf.Lerp (c1.r, c2.r, delta), Mathf.Lerp (c1.g, c2.g, delta), Mathf.Lerp (c1.b, c2.b, delta), Mathf.Lerp (c1.a, c2.a, delta));
	}
			
}
