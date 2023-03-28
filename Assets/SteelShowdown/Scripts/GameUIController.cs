using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class GameUIController : MonoBehaviour
{
    [SerializeField]
    private Vector3 uiOffset;
    [SerializeField]
    private Text playerNameText;
    [SerializeField]
    TankController target;
    Transform targetTransform;
    Renderer targetRenderer;
    CanvasGroup canvasGroup;
    Vector3 targetPosition;

    private void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
           
    }
    // Update is called once per frame
    void Update()
    {
        if(target==null)
        {
            Destroy(this.gameObject);
            return;
        }
        
    }
    private void LateUpdate()
    {
      if(targetTransform!=null)
        {
            targetPosition = targetTransform.position;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + uiOffset;
        }  
    }
    public void SetTarget(TankController target)
    {
        if(target==null)
        {
            return;
        }
        this.target = target;
        targetTransform = this.target.GetComponent<Transform>();
        targetRenderer = this.target.GetComponentInChildren<Renderer>();
        playerNameText.text = this.target.photonView.Owner.NickName;
    }
}
