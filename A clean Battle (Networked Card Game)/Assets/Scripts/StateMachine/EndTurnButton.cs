using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    private bool subscribed = false;

    private void Start(){
        if(!subscribed){
            GamePlayState.Instance.OnStateChanged += EnableButtonWhenPlayerTurn;
            subscribed = true;
        }
    }

    private void OnEnable(){
        if(GamePlayState.Instance == null){
            return;
        }
        if(!subscribed){
            GamePlayState.Instance.OnStateChanged += EnableButtonWhenPlayerTurn;
            subscribed = true;
        }
    }

    private void OnDisable(){
        if(subscribed){
            GamePlayState.Instance.OnStateChanged -= EnableButtonWhenPlayerTurn;
            subscribed = false;
        }
    }

    public void EnableButtonWhenPlayerTurn(){
        if(GamePlayState.Instance.CurrentState == GamePlayState.GameState.PlayerTurn){
            GetComponent<Button>().interactable = true;
        } else {
            GetComponent<Button>().interactable = false;
        }
    }
}
