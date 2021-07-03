using UnityEngine;

//Class to control the targetting display when casting spells, when targetting is active more a targetting object around with the mouse and the spell engine will request the
//position and rotation of that object in order to instantiate the spell object
public class TargettingPointer : MonoBehaviour
{
    //Prefab to use if no valid object is selected, will almost always use object assigned to pointer prefab but used as a fall-back
    [SerializeField]
    GameObject defaultPointerPrefab;
    GameObject pointerPrefab = default;

    //Transparent material used by all targetting objects to show the object is used for targetting
    [SerializeField]
    Material targettingMaterial = default;

    //Max distance the raycast will travel from the camera (i.e. how far away can a player target a spell)
    [SerializeField, Min(0f)]
    float maxProbeDistance = 10f;

    bool targettingActive = false;
    bool targetFloor = false;
    Camera mainCamera;
    Transform targettingTransform;
    Renderer targettingRenderer;

    private void Awake()
    {
        mainCamera = Camera.main;
        ResetTargetting();
    }

    //When targetting is active (i.e. a spell has been selected and the targetting button has been pressed), set the targetting reticule to the position selected by the cursor
    private void FixedUpdate()
    {
        if (targettingActive)
        {
            Vector3 newPosition;
            Quaternion newRotation;
            if (GetNewTargetPosition(out newPosition, out newRotation))
            {
                targettingTransform.position = newPosition;
                targettingTransform.rotation = newRotation;
                if (!targettingTransform.GetComponent<Renderer>().enabled)
                {
                    targettingRenderer.enabled = true;
                }
            }
            else
            {
                targettingRenderer.enabled = false;
            }
        }
    }

    //Return the current position and rotation of the targetting object, returns false and zero/identity values if targetting isn't active
    public bool GetCurrentTargetPositioning(out Vector3 position, out Quaternion rotation)
    {
        Vector3 currentPosition = Vector3.zero;
        Quaternion currentRotation = Quaternion.identity;
        bool validTarget = false;

        if (targettingActive)
        {
            currentPosition = targettingTransform.position;
            currentRotation = targettingTransform.rotation;
            validTarget = targettingRenderer.enabled;
        }

        position = currentPosition;
        rotation = currentRotation;
        return validTarget;
    }

    //Raycast from the camera position out by the max probe distance towards the cursor, if the raycast hits a collider on the "Placeable Surface" layer set targetting 
    //transform to be on that surface at the cursor point, returns true if a valid position could be generated
    bool GetNewTargetPosition(out Vector3 newPosition, out Quaternion newRotation)
    {
        Vector3 targetPosition = Vector3.zero;
        Quaternion targetRotation = Quaternion.identity;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = 1 << 8; //Layer 8 = PlaceableSurface
        bool hitSurface = false;
        if (Physics.Raycast(ray, out hit, maxProbeDistance, layerMask))
        {
            if(targetFloor)
            {
                if(hit.collider.tag == "Floor")
                {
                    hitSurface = true;
                    targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    targetPosition = hit.point + (hit.normal * (targettingTransform.localScale.y * 0.5f));
                }
            }
            else
            {
                hitSurface = true;
                targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                targetPosition = hit.point + (hit.normal * (targettingTransform.localScale.y * 0.5f));
            }
        }
        newPosition = targetPosition;
        newRotation = targetRotation;

        return hitSurface;
    }
    //Public function to read targettingActive value
    public bool CheckTargetting()
    {
        return targettingActive;
    }

    //Set parameters to begin targetting
    public void ActivateTargetting()
    {
        if (!targettingActive)
        {
            targettingActive = true;
            Transform parentTransform = this.transform;
            targettingTransform = Instantiate(pointerPrefab, parentTransform).GetComponent<Transform>();
            targettingTransform.name = "Targetting Pointer";
            targettingRenderer = targettingTransform.GetComponent<Renderer>();
            targettingRenderer.material = targettingMaterial;
            targettingRenderer.receiveShadows = false;
            targettingRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            Vector3 spawnPosition = Vector3.zero;
            Quaternion spawnRotation = Quaternion.identity;
            bool validPosition = GetNewTargetPosition(out spawnPosition, out spawnRotation);
            targettingTransform.position = spawnPosition;
            targettingTransform.rotation = spawnRotation;
            targettingRenderer.enabled = validPosition;
        }
    }

    //If targetting is active destroy the targetting transform and reset the targettign parameters
    public void DisableTargetting()
    {
        if (targettingActive)
        {
            targettingActive = false;
            if(targettingTransform != null)
            {
                Destroy(targettingTransform.gameObject);
                ResetTargetting();
            }
        }
    }
    
    //Reset targetting parameters back to null
    void ResetTargetting()
    {
        targettingTransform = null;
        targettingRenderer = null;
        SetTargetPrefab(defaultPointerPrefab);
        SetTargetFloor(false);
    }

    //Set new targetting prefab object
    public void SetTargetPrefab(GameObject go)
    {
        pointerPrefab = go;
    }

    //Set if raycast should check if surface is the floor or not when targetting
    public void SetTargetFloor(bool targetFloor)
    {
        this.targetFloor = targetFloor;
    }


}
