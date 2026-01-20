using UnityEngine;
using TMPro;

[AddComponentMenu("Layout/TMP Size Fitter")]
public class TMPSizeFitter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_TextMeshPro;
    private RectTransform m_TMPRectTransform;
    private RectTransform m_RectTransform;
    private float m_PreferredHeight;
    private float m_PreferredWidth;

    [SerializeField] private bool fitHeight = true;
    [SerializeField] private bool fitWidth = true;
    [SerializeField] private float extraHeight = 10f;
    [SerializeField] private float extraWidth = 10f;

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

    public float PreferredWidth { 
        get { 
            return m_PreferredWidth; 
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
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, m_PreferredHeight + extraHeight);
    }

    private void SetWidth(){
        if(TextMeshPro == null) return;
        m_PreferredWidth = TextMeshPro.preferredWidth;
        rectTransform.sizeDelta = new Vector2(m_PreferredWidth + extraWidth, rectTransform.sizeDelta.y);
    }

    private void OnEnable(){
        if(fitHeight) SetHeight();
        if(fitWidth) SetWidth();
    }

    private void Start(){
        if(fitHeight) SetHeight();
        if(fitWidth) SetWidth();
    }

    private void Update(){
        if(fitHeight && PreferredHeight != TextMeshPro.preferredHeight){
            SetHeight();
        }
        if(fitWidth && PreferredWidth != TextMeshPro.preferredWidth){
            SetWidth();
        }
    }

}
