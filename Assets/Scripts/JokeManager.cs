using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class JokeManager : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI textMeshJoke;

    private List<string> jokes = new List<string>();
    void Start()
    {
        JokesInitialization();
        DisplayRandomJoke();
    }

    private void JokesInitialization()
    {
        jokes.Add("Gdzie moge oddac\n elkektrosmieci?"); //Where can I leave electro-waste?
        jokes.Add("Wiedziales, \nze kocham maratony?"); // Did you know that I love marathons?
        jokes.Add("Zawsze doniose\ncieple buleczki"); // I will always deliver hot rolls!
        jokes.Add("Zadna dziewczyna\nnie chce ze mn¹ chodzic");                     // None girl wants to goes with me
        jokes.Add("Chyba stracilem\n czucie w nogach");                             // Probably I don't feel my legs
        jokes.Add("W dziecinstwie mia³em\n pieszo 10km do szkoly");                 // When I was a child, I had to walk 10 km to school
        jokes.Add("Kiedys dziewczyny przede mna uciekaly, \nteraz nie maja szans"); // The girls used to run away from me, but now they have no chance.
        jokes.Add("Poscigamy sie?"); // Do you want race with me?
        jokes.Add("Wychodze tylko po mleko"); //I just go out for milk.
        jokes.Add("Jestem czlonkiem\n Republic Of Gamers,\na Ty?"); // I'm member of Republic of Gamers and You?
    }

    public void DisplayRandomJoke()
    {
        int randomJoke = Random.Range(0, jokes.Count);
        textMeshJoke.text = jokes[randomJoke];
    }
}
