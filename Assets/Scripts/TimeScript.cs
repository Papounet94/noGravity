using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeScript : MonoBehaviour
{
    [SerializeField] float timeElapsed = 0f;
    [SerializeField] float timeAtStart;

    private TextMeshProUGUI timeText;
    // Start is called before the first frame update
    void Start()
    {
        timeAtStart = Time.time;
        timeText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = string.Format("Time: {0:N1} s", getTimeElapsed());
    }

    float getTimeElapsed()
    {
        PersistentSettings.Instance.winTime = Time.time - timeAtStart;
        return PersistentSettings.Instance.winTime;
    }
}
