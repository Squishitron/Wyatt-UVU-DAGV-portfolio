using TMPro;
using UnityEngine;
using UnityEngine.UI; // importing UI for UI purposes

public class MoneyManager : MonoBehaviour
{

    // settup -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public Text moneyText; // this is so we can set the text we are using for this UI, so we can update it as the score changes

    int moneyAmount = 20; // this is the score we are going to effect, 20 bucks by default because why not, and its funnier than starting at zero
    
    // Start ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {

        moneyText.text = "MONIES : $" + moneyAmount.ToString(); // updating the score text to use the current score amount


    }

    // Update -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void changeMoney(int x)
    {

        moneyAmount += x; // changes the amount of money based on the passed value
        moneyText.text = "MONIES : $" + moneyAmount.ToString();

    }

}
