using UnityEngine;

public class SelectionManager : MonoBehaviour, ITurnDependant
{
    FlashFeedBack flashFeedBack;
    SelectionIndicatorFeedBack selectionFeedback;

    AgentOutlineFeedBack outlineFeedBack;
  
    public void HandleSelection(GameObject detectedCollider)
    {
        DeselectOldObject();

        if (detectedCollider == null)
            return;

        selectionFeedback = detectedCollider.GetComponent<SelectionIndicatorFeedBack>();

        if (selectionFeedback!=null)
            selectionFeedback.Select();

        outlineFeedBack = detectedCollider.GetComponent<AgentOutlineFeedBack>();
        if (outlineFeedBack!=null)
        {
            outlineFeedBack.Select();
        }   

      Unit unit = detectedCollider.GetComponent<Unit>();

        if (unit != null)
        {
            if (unit.CanStillMove() == false)
                return;
        }
        flashFeedBack = detectedCollider.GetComponent<FlashFeedBack>();
        if (flashFeedBack!=null)
        {
            flashFeedBack.PlayFeedBack();
        }     
    }

    public void WaitTurn()
    {
        DeselectOldObject();
    }

    public void DeselectOldObject()
    {
        if (flashFeedBack != null)
        {
            flashFeedBack.StopFeedBack();
            flashFeedBack = null;
        }

        if (selectionFeedback !=null)
        {
            selectionFeedback.DeSelect();
            selectionFeedback = null;
        }
        if (outlineFeedBack!=null)
        {
            outlineFeedBack.Deselect();
            outlineFeedBack = null;
        }
    }
}
