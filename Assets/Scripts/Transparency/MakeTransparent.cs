using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour
{
    [SerializeField] private List<ObjectObstructingVision> currentlyObstructing;
    [SerializeField] private List<ObjectObstructingVision> alreadyTransparent;
    [SerializeField] private Transform player;
    private Transform camera;

    private void Awake()
    {
        currentlyObstructing = new List<ObjectObstructingVision>();
        alreadyTransparent = new List<ObjectObstructingVision>();

        camera = this.gameObject.transform;
    }

    private void Update()
    {
        GetAllObstructing();

        MakeObjectsSolid();
        MakeObjectsTransparent();
    }

    private void GetAllObstructing()
    {

        currentlyObstructing.Clear();

        float cameraPlayerDistance = Vector3.Magnitude(camera.position - player.position);

        Ray ray1Forward = new Ray(camera.position, player.position - camera.position);
        Ray ray1Backward = new Ray(player.position, camera.position - player.position);

        var hits1Forward = Physics.RaycastAll(ray1Forward, cameraPlayerDistance);
        var hits1Backward = Physics.RaycastAll(ray1Backward, cameraPlayerDistance);

        foreach (var hit in hits1Forward)
        {
            if (hit.collider.gameObject.TryGetComponent(out ObjectObstructingVision obstructing)) {
                if (!currentlyObstructing.Contains(obstructing))
                    currentlyObstructing.Add(obstructing);
            }
        }

        foreach (var hit in hits1Backward)
        {
            if (hit.collider.gameObject.TryGetComponent(out ObjectObstructingVision obstructing))
            {
                if (!currentlyObstructing.Contains(obstructing))
                    currentlyObstructing.Add(obstructing);
            }
        }
    }

    private void MakeObjectsTransparent()
    {

        for (int i = 0; i < currentlyObstructing.Count; i++)
        {
            ObjectObstructingVision obstructing = currentlyObstructing[i];

            if (!alreadyTransparent.Contains(obstructing))
            {
                //Debug.Log("Making object transparent.");
                obstructing.ShowTransparent();
                alreadyTransparent.Add(obstructing);
            }
        }
    }


    private void MakeObjectsSolid()
    {

        for (int i = alreadyTransparent.Count-1; i >= 0; i--)
        {
            ObjectObstructingVision transparent = alreadyTransparent[i];

            if (!currentlyObstructing.Contains(transparent))
            {
                //Debug.Log("Making object solid again.");
                transparent.ShowSolid();
                alreadyTransparent.Remove(transparent);
            }
        }
    }


}
