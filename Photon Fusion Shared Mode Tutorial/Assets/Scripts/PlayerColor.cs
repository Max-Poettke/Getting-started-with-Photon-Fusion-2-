using UnityEngine;
using Fusion;

public class PlayerColor : NetworkBehaviour
{
    private MeshRenderer MeshRenderer;

    [Networked, OnChangedRender(nameof(ColorChanged))]
    public Color networkedColor {get; set;}
    

    // Update is called once per frame
    void Update()
    {
        if(HasStateAuthority && Input.GetKeyDown(KeyCode.E)){
            networkedColor = new Color(Random.Range(0f, 1f), Random.Range(0f,1f), Random.Range(0f,1f)); 
        }
    }

    void ColorChanged(){
        GetComponent<MeshRenderer>().material.color = networkedColor;
    }
}
