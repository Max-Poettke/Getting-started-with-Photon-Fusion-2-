using UnityEngine;
using TMPro;

[AddComponentMenu("Layout/TMP Size Fitter")]
public class TMPSizeFitter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_TextMeshPro;
    private RectTransform m_TMPRectTransform;
    private RectTransform m_RectTransform;
    private float m_PreferredHeight;


    public float PreferredHeight { 
        get { 
            return m_PreferredHeight; 
            } 
        }

    public RectTransform TMPRectTransform { 
        get { 
            return m_TMPRectTransform; 
            } 
        }
    

    public RectTransform rectTransform { 
        get { 
            if(m_RectTransform == null){
                m_RectTransform = transform.GetComponent<RectTransform>();
            }
            return m_RectTransform; 
        } 
    }
    
    public TextMeshProUGUI TextMeshPro { 
        get {
            if(m_TextMeshPro == null && transform.GetComponentInChildren<TextMeshProUGUI>()){
                m_TextMeshPro = transform.GetComponentInChildren<TextMeshProUGUI>();
                m_TMPRectTransform = m_TextMeshPro.rectTransform;
            }
            return m_TextMeshPro; 
        } 
    }

    private void SetHeight(){
        if(TextMeshPro == null) return;

        m_PreferredHeight = TextMeshPro.preferredHeight;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, m_PreferredHeight + 10);
    }

    private void OnEnable(){
        SetHeight();
    }

    private void Start(){
        SetHeight();
    }

    private void Update(){
        if(PreferredHeight != TextMeshPro.preferredHeight){
            SetHeight();
        }
    }

}
