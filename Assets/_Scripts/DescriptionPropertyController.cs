using System;
using UnityEngine;

public class DescriptionPropertyController : MonoBehaviour
{
    public bool Locked
    {
        get { return _locked; }
        set
        {
            _locked = value;
            _lockedFlag.gameObject.SetActive(_locked);
        }
    }

    public int Index { get; set; }
    public event Action<int> RerollClicked;
    public event Action<int> LockClicked;

    public void InvokeReroll()
    {
        RerollClicked?.Invoke(Index);
    }
    public void InvokeLock()
    {
        LockClicked?.Invoke(Index);
        Locked = !_locked;
    }

    [SerializeField]
    private Transform _lockedFlag;

    private bool _locked = false;
}
