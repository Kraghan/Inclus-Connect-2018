using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;

namespace Scripting.QTE
{
    internal class Door : MonoBehaviour
    {
        
        public enum EDoorType
        {
            Default,
            Ghost,
            Adaptative
        }

        /// The door type
        public EDoorType m_type = EDoorType.Adaptative;

        /// The door ghost form
        public Sprite m_defaultDoorBack = null;

        /// The door ghost form
        public Sprite m_ghostDoorBack = null;

        
        /// The door ghost form
        public Sprite m_defaultDoorFront = null;

        /// The door ghost form
        public Sprite m_ghostDoorFront = null;


        /// The renderer
        public SpriteRenderer m_rendererBack = null;

        /// The renderer
        public SpriteRenderer m_rendererFront = null;

        /// The magma renderer
        public SpriteRenderer m_magma = null;
        internal SpriteRenderer magma {get{return m_magma;} set {m_magma = value;}}
        
        /// Form of the door
        internal EPlayerForm form {get; private set;}

        /// Update
        void Update()
        {
            switch( m_type )
            {
                case EDoorType.Adaptative:
                {
                    // Get camera bounds
                    float screenAspect = (float)Screen.width / (float)Screen.height;
                    float cameraHeight = Camera.main.orthographicSize * 2;
                    Bounds bounds = new Bounds(
                    (Vector2)Camera.main.transform.position,
                    new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
                    Debug.DrawLine(bounds.max, bounds.max + Vector3.down * bounds.size.y, Color.red, 0.1f);
                    //
                    
                    // Debug.LogFormat("UPDATE ({1}) Door {0}", m_magma.name, gameObject.transform.position);
                    if (bounds.Intersects(m_magma.bounds) == true)
                    {
                        if (Managers.instance.playerManager.player.inputs.lightOn == true)
                        {
                            if(m_rendererBack != null)
                                m_rendererBack.sprite = m_ghostDoorBack;
                            if(m_rendererFront != null)
                                m_rendererFront.sprite = m_ghostDoorFront;
                            form = EPlayerForm.Ghost;
                        }
                        else
                        {
                            if(m_rendererBack != null)
                                m_rendererBack.sprite = m_defaultDoorBack;
                            if(m_rendererFront != null)
                                m_rendererFront.sprite = m_defaultDoorFront;
                            form = EPlayerForm.Default;
                        }

                        enabled = false;
                    }
                } break;

                case EDoorType.Ghost:
                {
                    if(m_rendererBack != null)
                        m_rendererBack.sprite = m_ghostDoorBack;
                    if(m_rendererFront != null)
                        m_rendererFront.sprite = m_ghostDoorFront;
                    form = EPlayerForm.Ghost;
                } break;

                case EDoorType.Default:
                {
                    if(m_rendererBack != null)
                        m_rendererBack.sprite = m_defaultDoorBack;
                    if(m_rendererFront != null)
                        m_rendererFront.sprite = m_defaultDoorFront;
                    form = EPlayerForm.Default;
                }break;
            }
            
        }
    }
}