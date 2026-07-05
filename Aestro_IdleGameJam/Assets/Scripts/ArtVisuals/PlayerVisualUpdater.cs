using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> Meant to handle visuals for the player as needed programatically... This subscribes to GameManager</para>
/// </summary>
public class PlayerVisualUpdater : MonoBehaviour
{
    public MeshFilter diceAboveWand;

    // Start is called before the first frame update
    void Start()
    {
        if (Game_Manager.instance) Game_Manager.instance.InjectPVUcs(this);
    }

    public void SetDiceMesh(Mesh _mesh)
    {
        if (!diceAboveWand) { Debug.LogWarning("PlayerVisualUpdater - missing mesh filter reference to dice"); return; }

        diceAboveWand.mesh = _mesh;
    }


}
