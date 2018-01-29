using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    public Texture2D cursorHover;
    public bool cursorTypeDefault = true;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCursorToHover()
    {
        Cursor.SetCursor(cursorHover, Vector2.zero, CursorMode.ForceSoftware);
        cursorTypeDefault = false;
    }

    public void SetCursorToDefault ()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        cursorTypeDefault = true;
    }
}
