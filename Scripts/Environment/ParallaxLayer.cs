using UnityEngine;

/// <summary>
/// Used for parallaxing of layers in the scene according to how far they should appear from the 
/// camera. Layers more in the background should move slower (for the viewer), than the layers in
/// the foreground, when player/camera moves. The layers move together with the camera. 
/// </summary>
public class ParallaxLayer : MonoBehaviour {
    /// <summary>
    /// Give higher multiplier to the layers that are more in the background. 
    /// Give negative values of multiplier to the layers, that are in front of
    /// middle ground, on which player moves. 
    /// </summary>
    [SerializeField] 
    private float multiplier = 0.0f;

    private Transform cameraTransform;
    private Vector3 startCameraPos;
    private Vector3 startPos;

    /// <summary>
    /// Gets position of cammera and this layer. 
    /// </summary>
    void Start() {
        cameraTransform = Camera.main.transform;
        startCameraPos = cameraTransform.position;
        startPos = transform.position;
    }

    /// <summary>
    /// Runs after all objects is updated (camera already moved)
    /// </summary>
    private void LateUpdate() {
        //current minus original position
        float cameraPositionDifference = cameraTransform.position.x - startCameraPos.x; 
        
        Vector3 newPosition = startPos;
        newPosition.x += multiplier * cameraPositionDifference;

        //sets new position
        transform.position = newPosition;
    }

}

