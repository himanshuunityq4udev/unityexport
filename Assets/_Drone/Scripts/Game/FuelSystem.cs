using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDC
{
    public class FuelSystem : Singleton<FuelSystem>
    {
        [SerializeField] GameEvent GameOver;
        public float startFuel;//start fuel
        public float maxFuel = 100f;//max fuel
        public float fuelConsumptionRate; //fuel drop rate
        public Slider fuelIndicatorSld; //slider to indicate the fuel level
        public TextMeshProUGUI fuelIndicatorTxt; //text to indicate the fuel level

        public GameObject lowFuelIndictor;
        public GameObject lowFuelIndictorText;

        public bool resetTime;

        // Use this for initialization
        void Start()
        {

            ///cap the fuel
            if (startFuel > maxFuel)
            {
                startFuel = maxFuel;
            }
            //update ui elements
            fuelIndicatorSld.maxValue = maxFuel;
            UpdateUI();
        }

        private bool gameOverTriggered = false;
        public void ReduceFuel()
        {
            // Reduce fuel level and update UI elements
            if (startFuel > 0)
            {
                startFuel -= Time.deltaTime * fuelConsumptionRate;
                UpdateUI();
            }
            else if (!gameOverTriggered)
            {
                gameOverTriggered = true; // Set the flag to true to prevent further invocations
                GameOver.Invoke();
            }
        }

        public void ConsumeFuel(float notMoving, float moving)
        {

            bool isThrottleActive = InputManager.Instance.Throttle == 1 || InputManager.Instance.Throttle == -1;
            bool isCyclicActive = false;

            foreach (var direction in Direction.Directions)
            {
                if (InputManager.Instance.Cyclic == direction)
                {
                    isCyclicActive = true;
                    break;
                }
            }

            if (isThrottleActive || isCyclicActive)
            {
                fuelConsumptionRate = moving;
            }
            else
            {
                fuelConsumptionRate = notMoving;
            }
            ReduceFuel();
        }



        public void PausetheGame()
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        public void AddFuel()
        {

            resetTime = true;
            Time.timeScale = 1;
            startFuel = maxFuel;
            UpdateUI();
            AudioListener.pause = false;
        }

        public void AddFuel(float addToAdd)
        {
            startFuel += addToAdd;
            Debug.Log(startFuel);
            ///cap the fuel
            if (startFuel > maxFuel)
            {
                Debug.Log("inside");
                startFuel = maxFuel;
            }
            UpdateUI();
        }

        //PICK UP JerryCan 
        void OnTriggerEnter(Collider other)
        {
            /* if (other.gameObject.CompareTag("Ground"))
             {
                 startFuel += 30;
                 ///cap the fuel
                 if (startFuel > maxFuel)
                 {
                     startFuel = maxFuel;
                 }
                 UpdateUI();


                 Destroy(other.gameObject);
             }*/
        }

        //ENTER the gas station
        /* void OnTriggerStay(Collider other)
         {
             if (other.gameObject.CompareTag("GasStation"))
             {
                 startFuel += Time.deltaTime * 5f;

                 if (startFuel > maxFuel)
                 {
                     startFuel = maxFuel;
                 }
                 UpdateUI();
             }
         }*/

        public Color[] colors;
        public float time;
        private int currentColorIndex = 0;
        private int targetColorIndex = 1;
        private float targetpoint;
        public float blinkSpeed = 1.0f; // Add this variable to control the blink speed

        void UpdateUI()
        {
            fuelIndicatorSld.value = startFuel;
            fuelIndicatorTxt.text = startFuel.ToString("0") + "%";

            fuelIndicatorSld.fillRect.gameObject.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, fuelIndicatorSld.value / maxFuel);
            if (startFuel < 20)
            {
                lowFuelIndictor.SetActive(true);
                ColorTransition();
                //BlinkObject();
            }
            else
            {
                lowFuelIndictor.SetActive(false);
            }
            //if there is no fuel inform the user
            if (startFuel <= 0)
            {
                startFuel = 0;
                fuelIndicatorTxt.text = "Out of fuel!!!";
            }
        }

        void ColorTransition()
        {
            targetpoint += (Time.deltaTime * blinkSpeed) / time;
            fuelIndicatorSld.fillRect.gameObject.GetComponent<Image>().color = Color.Lerp(colors[currentColorIndex], colors[targetColorIndex], targetpoint);
            lowFuelIndictor.GetComponent<Image>().color = Color.Lerp(colors[currentColorIndex], colors[targetColorIndex], targetpoint);
            if (targetpoint >= 1f)
            {
                targetpoint = 0f;
                currentColorIndex = targetColorIndex;
                targetColorIndex++;
                if (targetColorIndex == colors.Length)
                {
                    targetColorIndex = 0;
                }
            }
        }


        private float blinkTimer;
        [SerializeField] private float speed;
        void BlinkObject()
        {
            blinkTimer += Time.deltaTime * speed;

            if (blinkTimer >= 1f)
            {
                lowFuelIndictorText.SetActive(!lowFuelIndictorText.activeSelf); // Toggle the active state
                blinkTimer = 0f; // Reset the timer
            }
        }
    }
}