using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class Gauge
{
    public const int MIN_SIZE_GAUGE = 50;
    public const int MAX_SIZE_GAUGE = 500;
    [SerializeField] [Range(MIN_SIZE_GAUGE, MAX_SIZE_GAUGE)] private int _maxSize = MIN_SIZE_GAUGE;
    [SerializeField] private int _currentValue = MIN_SIZE_GAUGE / 2;
    [SerializeField] [Tooltip("Negative Number : Gauge depletes all the time. Positive number : Gauge rises all the time. 0 = Gauge stays the same")] [Range(-5f, 5f)] private float _modifierPerSecond;
    public delegate void GaugeEvent();

    public event GaugeEvent Depleted;
    public event GaugeEvent Full;
    public int MinSize
    {
        get { return MIN_SIZE_GAUGE; }
    }
    public int MaxSize
    {
        get { return _maxSize; }
        private set
        {
            Assert.IsTrue(value >= MIN_SIZE_GAUGE && value <= MAX_SIZE_GAUGE, "Can't set Max size to "+value+" for this gauge must be beetween " + MIN_SIZE_GAUGE + " and " + MAX_SIZE_GAUGE+ " ! If you want to change these parameters, please go to Gauge.cs struct and change constants");
            _maxSize = value;
        }
    }
    
    public float Rate
    {
        get { return ((float) _currentValue) / ((float) _maxSize); }
        set
        {
            Assert.IsTrue(value <= 1f && value >= 0f, "Can't set rate to "+value+" Need to set rate to a number beetween [0 ; 1] !");
            //ça fonctionne pas dans certain cas...
            //Value = (Mathf.Abs(value - 1f) <= float.Epsilon ? _maxSize : (int) Mathf.Lerp(0f, (float)_maxSize, value));
            Value = (Mathf.Abs(value - 1f) <= float.Epsilon ? _maxSize : (int) Mathf.Lerp(0f, (float)_maxSize, value));
        }
    }

    public int Value
    {
        get { return _currentValue; }
        set
        {
            //info : bah en fait on veut juste les caper dans ce cas, pas faire planter l'appli :/
            /*Assert.IsTrue(value >= 0 && value <= _maxSize,
                "Can't set the value of this gauge to " + value + " : needs to be in [0 ; " + _maxSize + " (maxSize)]");
            _currentValue = value;*/

            if(value < 0){
                _currentValue = 0;
            }else if(value > _maxSize){
                _currentValue = _maxSize;
            }else{
                _currentValue = value;
            }

            if (Depleted != null && _currentValue == 0)
                Depleted();
            else if (Full != null && _currentValue == _maxSize)
                Full();
        }
    }

    public float ModifPerSecond
    {
        get { return _modifierPerSecond; }
        set
        {
            if (Mathf.Abs(value) < float.Epsilon)
                _modifierPerSecond = 0f;
            else
            {
                Assert.IsTrue(Mathf.Abs(value) < MIN_SIZE_GAUGE, "Warning, the modifier might be too high. if you're sure about this and don't want an error, contact Jalik");
                _modifierPerSecond = value;
            }
        }
    }
    
    /**<summary>
     * Updates max size of the gauge. Can change the current value to keep the old rate.
     * </summary>
     * <param name="newMax">The new maximum for this gauge (needs to be beetween <see cref="MIN_SIZE_GAUGE"/> and <see cref="MAX_SIZE_GAUGE"/>)</param>
     * <param name="keepRate">Default value is true : will keep the old rate. Example : 50 / 100. Update max to 150 => new current is 75 / 150 when keep rate is true. If keepRate is false : will not change the current value, unless it is greater than the newMax. Example : old = 120 / 150. newMax = 100 => new = 100 / 100 (and not 120 / 100) if keepRate is false</param>
     */
    public void UpdateMax(int newMax, bool keepRate = true)
    {
        float currentRate = 0f;
        if(keepRate)
            currentRate = Rate;
        MaxSize = newMax;
        if(keepRate){
            Rate = currentRate;}
        else if (_currentValue > _maxSize)
            Value = MaxSize;
    }

    public static implicit operator int(Gauge a)
    {
        return a._currentValue;
    }

}
