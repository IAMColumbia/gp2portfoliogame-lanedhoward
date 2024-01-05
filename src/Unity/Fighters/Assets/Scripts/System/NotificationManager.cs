using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{

    public float notificationTimeMax = 2f;

    public void SetupNotificationManager(FighterMain fighter)
    {
        fighter.SendNotification += Fighter_SendNotification;
    }

    private void Fighter_SendNotification(object sender, string e)
    {
        Transform lastChild = transform.GetChild(transform.childCount - 1);
        Notification notification = lastChild.GetComponent<Notification>();
        if (notification != null)
        {
            notification.EnableText(e, notificationTimeMax);
        }
    }
}
