using UnityEngine;

/*
 * The script must be placed on all sounds.
 * But all content, except animationName, is unnecessary for anything but testing.
 * */
public class Sound : MonoBehaviour {

    public string soundName;
    public Animal myAnimal;
    public Color dinglingColor = Color.white;

    private Vector3 myPosition;
    
    public void SetCurrentAnimal(Animal animal) {
        myAnimal = animal;
        transform.localPosition = myAnimal.transform.localPosition;
        myPosition = transform.localPosition;
    }
}
