using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;

namespace Scripting.QTE
{
    internal class Door : MonoBehaviour
    {
        /// The door ghost form
        [SerializeField]
        Sprite m_defaultDoorBack = null;

        /// The door ghost form
        [SerializeField]
        Sprite m_ghostDoorBack = null;

        
        /// The door ghost form
        [SerializeField]
        Sprite m_defaultDoorFront = null;

        /// The door ghost form
        [SerializeField]
        Sprite m_ghostDoorFront = null;

        /// The renderer
        [SerializeField]
        SpriteRenderer m_rendererBack = null;

        /// The renderer
        [SerializeField]
        SpriteRenderer m_rendererFront = null;

        /// Form of the door
        internal EPlayerForm form {get; private set;}

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
            
            if (bounds.Intersects(m_rendererBack.bounds) == true)
            {
                if (Managers.instance.playerManager.player.inputs.lightOn == true)
                {
                   m_rendererBack.sprite = m_ghostDoorBack;
                   m_rendererFront.sprite = m_ghostDoorFront;
                    form = EPlayerForm.Ghost;
                }
                else
                {
                    m_rendererBack.sprite = m_defaultDoorBack;
                    m_rendererFront.sprite = m_defaultDoorFront;
                    form = EPlayerForm.Default;
                }

                enabled = false;
            }
        }
    }
}