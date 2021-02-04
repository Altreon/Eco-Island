using System.IO.Ports;
using UnityEngine;
using System;
using System.Globalization;

public class SerialHandler : MonoBehaviour
{
    
    private SerialPort _serial;

    // Common default serial device on a Linux machine
    [SerializeField] private string serialPort = "COM5";
    [SerializeField] private int baudrate = 115200;

    public Action<bool> Swap {get;set;}

    public int ValuePotentiometre = 0;

    public bool IsSerialReady {
        get{
            return _serial != null && _serial.IsOpen;
        }
    }

    private void Awake () {
        _serial = new SerialPort(serialPort,baudrate);
        _serial.ReadTimeout = 500;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Once configured, the serial communication must be opened just like a file : the OS handles the communication.
        _serial.Open();
    }

    // Update is called once per frame
    void Update()
    {
        // Prevent blocking if no message is available as we are not doing anything else
        // Alternative solutions : set a timeout, read messages in another thread, coroutines, futures...
        if (_serial.BytesToRead <= 0) return;
        
        var message = _serial.ReadLine();
        // Arduino sends "\r\n" with println, '\n' is removed by ReadLine()
        message = message.Trim('\r');
        string[] tokens = message.Split(':');
        
        switch (tokens[0])
        {
            case "bt" :
                switch (tokens[1])
                {
                    case "on":
                        Debug.Log("Arduinon button on");
                        SetLedInternal(true);
                        break;
                    case "off":
                        Debug.Log("Arduinon button off");
                        SetLedInternal(false);
                        break;
                }
                break;
            case "sw" :
                switch (tokens[1])
                {
                    case "l":
                        //Debug.Log("Arduinon Swap Left");
                        Swap?.Invoke(false);
                        break;
                    case "r":
                        //Debug.Log("Arduinon Swap Right");
                        Swap?.Invoke(true);
                        break;
                        }
                break;
            case "tp" :
                SeasonManager.Instance.TempSet(float.Parse(tokens[1], CultureInfo.InvariantCulture));
                break;
                
            case "po" :
            int val;
            if(int.TryParse(tokens[1],out val))
            {
                ValuePotentiometre = val;
            }
            break;
        }
    }

    public void SetLedInternal(bool newState)
    {
        //_serial.WriteLine(newState ? "LED ON" : "LED OFF");
        //TODO J'ai plus mon bouton branché, à tester
        if(IsSerialReady){
            _serial.WriteLine("L:" + (newState ? "ON" : "OFF"));
        }
    }

    public void SetLed(int nLed, float percent)
    {
        String test = "V:" + nLed + ":" + Mathf.Clamp(percent, 0, 1).ToString().Replace(",", ".");
        if(IsSerialReady){
            _serial.WriteLine(test);
        }
    }

    private void UpdateDistance () {

    }
    
    private void OnDestroy()
    {
        _serial.Close();
    }
}
