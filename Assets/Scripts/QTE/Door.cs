using Scripting.GameManagers;
using UnityEngine;

namespace Scripting.QTE
{
    internal class Door : MonoBehaviour
    {
        /// The door ghost form
        [SerializeField]
        Sprite m_defaultDoor = null;

        /// The door ghost form
        [SerializeField]
        Sprite m_ghostDoor = null;

        /// The renderer
        SpriteRenderer m_renderer = null;

        /// Awake
        void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
        }

        /// Update
        void Update()
        {
            // Get camera bounds
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = Camera.main.orthographicSize * 2;
            Bounds bounds = new Bounds(
            (Vector2)Camera.main.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            Debug.DrawLine(bounds.max, bounds.max + Vector3.down * bounds.size.y, Color.red, 0.1f);
            //
            
            if (bounds.Intersects(m_renderer.bounds) == true)
            {
                if (Managers.instance.playerManager.player.inputs.lightOn == true)
                {
                   m_renderer.sprite = m_ghostDoor;
                }
                else
                {
                    m_renderer.sprite = m_defaultDoor;
                }

                enabled = false;
            }
        }
    }
}