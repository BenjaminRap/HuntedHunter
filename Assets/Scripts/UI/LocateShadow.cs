using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateShadow : MonoBehaviour
{
    private Level       level;
    private GameObject  player;
    private GameObject  shadow;
    [SerializeField]
    private Camera      cam;
    [SerializeField]
    private GameObject  arrow;
    private float       width;

    void    Start()
    {
        level = null;
        width = arrow.GetComponent<SpriteRenderer>().sprite.rect.width / 2;
    }
    public void Locate(Level level, GameObject shadow, GameObject player)
    {
        this.level = level;
        this.player = player;
        this.shadow = shadow;
    }

    void    Update()
    {
        Vector2 screenPosition;

        if (level != null)
        {
            screenPosition = cam.WorldToScreenPoint(shadow.transform.position);
            if(IsOffScreen(screenPosition))
            {
                if (arrow.activeSelf == false)
                    arrow.SetActive(true);
                if (screenPosition.y > Screen.height - width)
                    screenPosition.y = Screen.height - width;
                if (screenPosition.y < 0 + width)
                    screenPosition.y = 0 + width;
                if (screenPosition.x > Screen.width - width)
                    screenPosition.x = Screen.width - width;
                if (screenPosition.x < 0 + width)
                    screenPosition.x = 0 + width;
                arrow.transform.position = cam.ScreenToWorldPoint(screenPosition);
                arrow.transform.position = new Vector3(arrow.transform.position.x, arrow.transform.position.y, 0f);
                arrow.transform.up = shadow.transform.position - player.transform.position;
            }
            else if (arrow.activeSelf == true)
                arrow.SetActive(false);
        }
    }

    private bool    IsOffScreen(Vector3 position)
    {
        if (position.x > Screen.width)
            return (true);
        if (position.x < 0)
            return (true);
        if (position.y > Screen.height)
            return (true);
        if (position.y < 0)
            return (true);
        return (false);
    }
}
