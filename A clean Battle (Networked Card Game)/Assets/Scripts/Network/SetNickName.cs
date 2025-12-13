using UnityEngine;
using TMPro;

public class SetNickName : MonoBehaviour
{
    [SerializeField] private TMP_Text nickNameText;

    public void SetLocalNickName(string nickName){
        nickNameText.text = nickName;
    } 
}
