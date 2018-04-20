using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data {

    private List<string> cityNames;
    private List<string> provinceNames;
    private List<string> countryNames;


    public Data()
    {

        generateCityNames();
    }

    private void generateCityNames()
    {
        cityNames = new List<string>();

        cityNames.Add("London");
        cityNames.Add("Oxford");
        cityNames.Add("Falmouth");
        cityNames.Add("Frankfurt");
        cityNames.Add("Bonn");
        cityNames.Add("Berlin");
        cityNames.Add("Paris");
        cityNames.Add("");
        cityNames.Add("");
        cityNames.Add("");
        cityNames.Add("");
        cityNames.Add("");
        cityNames.Add("");
        cityNames.Add("");
        cityNames.Add("");
        cityNames.Add("");
        cityNames.Add("");
        cityNames.Add("");
        cityNames.Add("");

    }

	
}
