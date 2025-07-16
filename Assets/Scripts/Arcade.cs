using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports; //Per a descarregar el paquet: Herramientas -> Administrador de paquetes NuGet -> Administrar paquetes de NuGet para la solucion
// Buscar o Ctrl + L -> System.io.ports -> INSTALAR Y REINICIAR VISUAL STUDIO COMUNITY
using System;
using System.Linq;
using System.IO;

public class Arcade : MonoBehaviour
{
    public static Arcade ac;
    private String portText;
    private String[] portsAvailable;
    private SerialPort serialPort;
    private readonly string[] keys = { "la", "ra", "lb", "rb", "l1", "r1", "l2", "r2", "j1_Up", "j2_Up", "j1_Down", "j2_Down", "j1_Left", "j2_Left", "j1_Right", "j2_Right", "start", "select" };
    private Vector2 j1 = new(0, 0);
    private Vector2 j2 = new(0, 0);
    private readonly Dictionary<string, bool> keyStates = new();
    private readonly Dictionary<string, bool> keyDown = new();
    private readonly Dictionary<string, bool> keyUp = new();

    void Awake()
    {
        portsAvailable = SerialPort.GetPortNames();
        try
        {
            portText = LoadPort();
            portText = portText.ToUpper();
            Debug.Log(portText);

        }
        catch (Exception e)
        {
            Debug.LogException(e);
            portText = "COM3";
        }

        if (ac != null && ac != this)
            Destroy(this);
        else
            ac = this;
        foreach (string key in keys)
        {
            keyStates[key] = false;
            keyDown[key] = false;
            keyUp[key] = false;
        }
        if (portsAvailable.Contains(portText))
        {
            try
            {
                serialPort = new SerialPort(portText, 9600);
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
                serialPort.ReadTimeout = 5;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }


    }

    public static string LoadPort()
    {
        string buildFolderPath = Path.GetDirectoryName(Application.dataPath);
        string parentFolderPath = Directory.GetParent(buildFolderPath).FullName;
        string filePath = Path.Combine(parentFolderPath, "port.txt");

        if (File.Exists(filePath))
        {
            StreamReader sr = new(filePath);
            string portName = sr.ReadToEnd().ToString();
            sr.Close();
            return $"{portName}";
        }
        else
        {
            Debug.LogError("Port settings file does not exist at path: " + filePath);
            return "COM3";
        }
    }

    public String GetPort()
    {
        if (serialPort != null)
        {
            return "Connected to port '" + portText + "'";
        }

        else
        {
            return "Not connected to port '" + portText + "'";
        }
    }

    private void Update()
    {
        if (serialPort != null)
        {
            if (serialPort.IsOpen)
            {
                ResetInputs();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SendDataToArduino(243, "rojo", "azul");
                }
                try
                {
                    string[] parDiv = serialPort.ReadLine().Split(":"); //INFORMACION LLEGA "ra:false" - "j1_left:false"
                    if (keys.Contains(parDiv[0]))
                    {
                        bool entrada = bool.Parse(parDiv[1]);
                        if (keyStates[parDiv[0]] != entrada)
                        {
                            if (entrada)
                            {
                                keyDown[parDiv[0]] = true;
                            }
                            else
                            {
                                keyUp[parDiv[0]] = true;
                            }
                        }
                        keyStates[parDiv[0]] = entrada;
                    }
                    else
                    {
                        string[] josti = parDiv[0].Split("_");
                        if (josti[0] == "j1" || josti[0] == "j2")//j1_Up:false
                        {
                            int h = 0;
                            int v = 0;
                            bool res = bool.Parse(parDiv[1]);
                            if (res)
                            {
                                switch (josti[1])
                                {
                                    case "Down":
                                        v = -1;
                                        break;
                                    case "Up":
                                        v = 1;
                                        break;
                                    case "Right":
                                        h = 1;
                                        break;
                                    case "Left":
                                        h = -1; break;
                                }
                            }
                            if (josti[0] == "j1")
                                j1 = new Vector2(h, v);
                            else
                                j2 = new Vector2(h, v);
                        }
                    }
                }
                catch
                {

                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                serialPort.Close();
                Debug.Log("Se cerro la conexion");
            }
        }
    }
    private void ResetInputs()
    {
        /// "LA" - "RA" - "LB" - "RB" - "L1" - "R2" - "j1_Up" - "j2_Down
        foreach (string key in keys)
        {
            keyDown[key] = false;
            keyUp[key] = false;
        }
    }
    /// <summary>
    /// Funcion que devuelve el valor de un boton
    /// "LA" - "RA" - "LB" - "RB" - "L1" - "R2" - "j1_Up" - "j2_Down
    /// </summary>
    public bool Button(string key)
    {
        return keyStates[key];
    }
    /// <summary>
    /// Funcion que devuelve cuando el boton se presiona por primera vez
    /// "LA" - "RA" - "LB" - "RB" - "L1" - "R2" - "j1_Up" - "j2_Down
    /// </summary>
    public bool ButtonDown(string key)
    {
        return keyDown[key];
    }
    /// <summary>
    /// Funcion que devuelve cuando el boton se suelta
    /// "LA" - "RA" - "LB" - "RB" - "L1" - "R2" - "j1_Up" - "j2_Down"
    /// </summary>
    public bool ButtonUp(string key)
    {
        return keyUp[key];
    }
    /// <summary>
    /// Funcion que devuelve un Vector(x,y) entre -1 y 1
    /// </summary>
    /// <param name="key"></param>
    /// <returns>Vector 2</returns>
    public Vector2 Joystick(string key)//JOySTICK pa los especialitos
    {
        if (key == "j1")
            return j1;
        else if (key == "j2")
            return j2;
        else
            return new Vector2(0, 0);
    }
    void SendDataToArduino(int porcentaje, string texto1, string texto2)
    {
        string message = porcentaje.ToString() + "," + texto1 + "," + texto2 + "\n";

        serialPort.Write(message);
    }
    void OnDestroy()
    {
        if (serialPort != null)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}