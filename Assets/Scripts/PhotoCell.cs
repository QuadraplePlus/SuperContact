using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhotoCell
{
    void DidSelectedPhotoCell(PhotoCell photoCell);
}

public class PhotoCell : MonoBehaviour
{
    public IPhotoCell photoCellDelegater;

    public void OnClick()
    {
        photoCellDelegater.DidSelectedPhotoCell(this);
    }
}
    