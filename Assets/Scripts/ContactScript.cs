using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionBehaviour))]
public class ContactScript : MonoBehaviour
{
    protected InteractionBehaviour _intObj;
    // Start is called before the first frame update
    void Start()
    {
        _intObj = GetComponent<InteractionBehaviour>();
        _intObj.OnContactStay += onContactStay;
    }

    private void onContactStay()
    {
        foreach (InteractionController controller in _intObj.contactingControllers)
        {
            float count = 0;
            foreach (ContactBone bone in controller.contactBones)
            {
                foreach (float overlap in bone.contactingInteractionBehaviours.Values)
                {
                    count += 1;

                }
            }
            Debug.Log(count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
