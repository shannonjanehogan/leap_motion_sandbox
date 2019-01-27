using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionBehaviour))]
public class ContactScript : MonoBehaviour
{
    protected InteractionBehaviour _intObj;
    GameObject groundObject;
    // Start is called before the first frame update
    void Start()
    {
        groundObject = GameObject.FindGameObjectWithTag("Ground");
        _intObj = GetComponent<InteractionBehaviour>();
        _intObj.OnContactStay += OnContactStay;
        _intObj.OnContactEnd += OnContactEnd;
    }

    private void OnContactEnd()
    {
        groundObject.GetComponent<GroundScript>().HandleCollisionEnded();
    }

    private void OnContactStay()
    {
        groundObject.GetComponent<GroundScript>().HandleCollisionStarted();
        foreach (InteractionController controller in _intObj.contactingControllers)
        {
            float count = 0;
            if (controller.softContactEnabled)
            {
                count = 10;
            } else
            {
                foreach (ContactBone bone in controller.contactBones)
                {
                    foreach (float overlap in bone.contactingInteractionBehaviours.Values)
                    {
                        count += 1;
                    }
                }
            }
            groundObject.GetComponent<GroundScript>().SetNewScore((int) count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
