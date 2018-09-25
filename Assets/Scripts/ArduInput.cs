using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Scripting
{

    public class ArduInput : MonoBehaviour
    {
        [Header("Arduino")]
        [SerializeField]
        private int m_baudRate = 250000;

        [SerializeField]
        private int m_readTimeout = 20;

        [SerializeField]
        private int m_queueLenght = 1;

        [SerializeField]
        private string m_portName = "COM5";

        wrmhl m_arduino = new wrmhl();

        private bool m_microOn = false;
        internal bool microOn{get { return m_microOn;} }

        private bool m_acceleroOn = false;
        internal bool acceleroOn{get { return m_acceleroOn;} }

        private bool m_buttonOn = false;
        internal bool buttonOn{get { return m_buttonOn;} }

        private bool m_lightOn = false;
        internal bool lightOn{get { return m_lightOn;} }

        private bool m_microPreviouslyOn = false, m_acceleroPreviouslyOn = false, m_buttonPreviouslyOn = false, m_lightPreviouslyOn = false;
        internal bool microJustOn{ get { return m_microOn && !m_microPreviouslyOn; }}

        public bool acceleroJustOn
        {
            get { return m_acceleroOn && !m_acceleroPreviouslyOn; }
        }

        public bool buttonJustOn
        {
            get { return m_buttonOn && !m_buttonPreviouslyOn; }
        }

        public bool lightJustOn
        {
            get { return m_lightOn && !m_lightPreviouslyOn; }
        }


        // Use this for initialization
        void Start()
        {
            string path = Application.dataPath + "/../Arduino.conf";
            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path, false);
                sw.WriteLine(m_portName);
                sw.Close();
            }
            else
            {
                StreamReader sr = new StreamReader(path);
                m_portName = sr.ReadLine();
                sr.Close();
            }

            m_arduino.set(m_portName, m_baudRate, m_readTimeout, m_queueLenght);
            m_arduino.connect();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            m_lightPreviouslyOn = m_lightOn;
            m_buttonPreviouslyOn = m_buttonOn;
            m_acceleroPreviouslyOn = m_acceleroOn;
            m_microPreviouslyOn = m_microOn;

            string data = m_arduino.readQueue();
        }


    }
}
