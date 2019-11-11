public class CormfirmPopupViewManager : PopupViewManager
{
    public delegate void AgreeDelete();
    public AgreeDelete deleteAction;
    // Start is called before the first frame update
    public void AgreeDeleted()
    {
        //값이 있으면 실행하고 , 없으면 실행 x
        deleteAction?. Invoke();
        Close();
    }
    public void DisAgreeDelete()
    {
        Close();
    }
}