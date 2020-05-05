using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************/
/* Script for displaying a Mini Radar Map            */
/* Based on the solution available on Unity Web Site */
/* Uses a camera positioned above the player         */
/* and looking at a single layer in Depth Only mode  */
/*****************************************************/

public class Radar : MonoBehaviour
{
    // List of objects tracked by the radar
    public GameObject[] trackedObjects;

    // List of objects beyond the border and displayed stuck at it
    List<GameObject> borderObjects;

    // Distance at which the objects go beyond visibility
    public float switchDistance;

    // Utility to align the player with the tracked objects
    public Transform helpTransform;

    // List of objects displayed in the radar
    private List<GameObject> radarObjects;

    // prefabs of both detected and target objects to display in the radar area 
    public GameObject radarPrefab, targetPrefab;


    // Start is called before the first frame update
    void Start()
    {
        createRadarObjects();
    }

    // Update is called once per frame
    void Update()
    {
        // Browse through radar detected objects
        for(int index = 0; index < radarObjects.Count; index++)
        {
            // Test the distance from player to each detected object
            if (Vector3.Distance(radarObjects[index].transform.position, transform.position) > switchDistance)
            {
                // switch to border objects if beyond display area
                // look at the detected object
                helpTransform.LookAt(radarObjects[index].transform);
                // Position it at the border in the right position
                borderObjects[index].transform.position = transform.position + switchDistance * helpTransform.forward;
                // Push the border object to Radar Layer
                borderObjects[index].layer = LayerMask.NameToLayer("Radar");
                // Push the radar object in the Invisible layer
                radarObjects[index].layer = LayerMask.NameToLayer("Invisible");
            }
            else
            {
                // switch to radarobjects
                // Put the radar object in the Radar Layer
                radarObjects[index].layer = LayerMask.NameToLayer("Radar");
                // Make the Border object Invisible
                borderObjects[index].layer = LayerMask.NameToLayer("Invisible");
            }
        }
    }

    private void createRadarObjects()
    {
        radarObjects = new List<GameObject>();
        borderObjects = new List<GameObject>();

        foreach(GameObject obj in trackedObjects)
        {
            GameObject k, j;
            if (obj.name != "ISS")
            {
                if (obj.name != "Target")
                {
                    // Instantiate two radar prefabs at the object's position
                    k = Instantiate(radarPrefab,
                        obj.transform.position,
                        Quaternion.identity) as GameObject;
                    j = Instantiate(radarPrefab,
                        obj.transform.position,
                        Quaternion.identity) as GameObject;
                }
                else
                {
                    // Instantiate two target prefabs at the target's position
                    k = Instantiate(targetPrefab,
                        obj.transform.position,
                        Quaternion.identity) as GameObject;
                    j = Instantiate(targetPrefab,
                        obj.transform.position,
                        Quaternion.identity) as GameObject;
                }

                // Put temporarily both prefabs in the Radar layer
                k.layer = LayerMask.NameToLayer("Radar");
                j.layer = LayerMask.NameToLayer("Radar");

                // add them to Radar and Border lists
                radarObjects.Add(k);
                borderObjects.Add(j);
            }
        }
    }
}
