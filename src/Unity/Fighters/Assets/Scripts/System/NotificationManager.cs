using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{

    public Color32 activeColor;
    public Color32 exitColor;

    public float notificationTimeMax = 2f;
    public float fadeTime = 1f;

    public void SetupNotificationManager(FighterMain fighter)
    {
        fighter.SentNotification += Fighter_SendNotification;
    }

    private void Fighter_SendNotification(object sender, string e)
    {
        Transform lastChild = transform.GetChild(transform.childCount - 1);
        Notification notification = lastChild.GetComponent<Notification>();
        if (notification != null)
        {
            notification.EnableText(e, notificationTimeMax, activeColor, exitColor, fadeTime);
        }
    }
}
