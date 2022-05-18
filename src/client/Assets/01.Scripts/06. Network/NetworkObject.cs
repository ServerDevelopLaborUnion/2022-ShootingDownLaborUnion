using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    public string ObjectID => _objectID;
    private string _objectID = "EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE";
    public string OwnerID => _ownerID;
    private string _ownerID = "EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE";

    public void Initialize(string objectID, string ownerID)
    {
        _objectID = objectID;
        _ownerID = ownerID;
    }
}
