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
            string data = m_arduino.readQueue();
        }
    }
}
